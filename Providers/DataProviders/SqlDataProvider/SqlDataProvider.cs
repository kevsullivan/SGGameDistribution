/*
' Copyright (c) 2016 Christoc.com
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Christoc.Modules.SGGameDistribution.Components;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework.Providers;
using Microsoft.ApplicationBlocks.Data;

namespace Christoc.Modules.SGGameDistribution.Data
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// SQL Server implementation of the abstract DataProvider class
    /// Pulls Database Information from Website install web.config file for database access.
    /// This concreted data provider class provides the implementation of the abstract methods 
    /// from data dataprovider.cs
    /// 
    /// In most cases you will only modify the Public methods region below.
    /// </summary>
    /// -----------------------------------------------------------------------------
    public class SqlDataProvider : DataProvider
    {

        #region Private Members

        private const string ProviderType = "data";
        private const string ModuleQualifier = "SGGameDistribution_";

        private readonly ProviderConfiguration _providerConfiguration = ProviderConfiguration.GetProviderConfiguration(ProviderType);
        private readonly string _connectionString;
        private readonly string _providerPath;
        private readonly string _objectQualifier;
        private readonly string _databaseOwner;

        #endregion

        #region Constructors

        public SqlDataProvider()
        {

            // Read the configuration specific information for this provider
            Provider objProvider = (Provider)(_providerConfiguration.Providers[_providerConfiguration.DefaultProvider]);

            // Read the attributes for this provider

            //Get Connection string from web.config
            _connectionString = Config.GetConnectionString();

            if (string.IsNullOrEmpty(_connectionString))
            {
                // Use connection string specified in provider
                _connectionString = objProvider.Attributes["connectionString"];
            }

            _providerPath = objProvider.Attributes["providerPath"];

            _objectQualifier = objProvider.Attributes["objectQualifier"];
            if (!string.IsNullOrEmpty(_objectQualifier) && _objectQualifier.EndsWith("_", StringComparison.Ordinal) == false)
            {
                _objectQualifier += "_";
            }

            _databaseOwner = objProvider.Attributes["databaseOwner"];
            if (!string.IsNullOrEmpty(_databaseOwner) && _databaseOwner.EndsWith(".", StringComparison.Ordinal) == false)
            {
                _databaseOwner += ".";
            }

        }

        #endregion

        #region Properties

        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
        }

        public string ProviderPath
        {
            get
            {
                return _providerPath;
            }
        }

        public string ObjectQualifier
        {
            get
            {
                return _objectQualifier;
            }
        }

        public string DatabaseOwner
        {
            get
            {
                return _databaseOwner;
            }
        }

        // used to prefect your database objects (stored procedures, tables, views, etc)
        private string NamePrefix
        {
            get { return DatabaseOwner + ObjectQualifier + ModuleQualifier; }
        }

        #endregion

        #region Private Methods

        private static object GetNull(object field)
        {
            return Null.GetNull(field, DBNull.Value);
        }

        #endregion

        #region Public Methods

        //public override IDataReader GetItem(int itemId)
        //{
        //    return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "spGetItem", itemId);
        //}

        //public override IDataReader GetItems(int userId, int portalId)
        //{
        //    return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "spGetItemsForUser", userId, portalId);
        //}

        public override IDataReader GetGames(int moduleId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix+ "GetGames", new SqlParameter("@ModuleId", moduleId));
        }

        public override IDataReader GetGame(int gameId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GetGame", new SqlParameter("@GameId", gameId));
        }

        public override ArrayList GetDevs()
        {
            List<string> resList =
                (from IDataRecord r in SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GetDevs")
                    select (string) r["QueryResult"]).ToList();
            return new ArrayList(resList);
        }

        public override void DeleteGame(int gameId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "DeleteGame", new SqlParameter("@GameId", gameId));
        }

        public override void DeleteGames(int moduleId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "DeleteGames", new SqlParameter("@ModuleId", moduleId));
        }

        /// <summary>
        /// From SqlDataProvider Stored Procedure Parameters are:
        /// @GameName nvarchar
        /// , @GameDescription nvarchar
        /// , @DeveloperId
        /// , @ModuleId
        /// , @CreatedOnDate
        /// , @CreatedByUserID
        /// , @AgeRating
        /// , @GameGenre
        /// , @MoreInfo
        /// , @ImageFileName
        /// , @InstallerFileName
        /// </summary>
        /// <param name="g"></param>
        /// <returns>An Integer = the ID of game that gets created.</returns>
        public override int AddGame(Game g)
        {
            var gId = Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, NamePrefix + "AddGame"
                , new SqlParameter("@GameName", g.GameName)
                , new SqlParameter("@GameDescription", g.GameDescription)
                , new SqlParameter("@DeveloperId", g.DeveloperId)
                , new SqlParameter("@ModuleId", g.ModuleId)
                , new SqlParameter("@CreatedOnDate", g.CreatedOnDate)
                , new SqlParameter("@CreatedByUserID", g.CreatedByUserIDId)
                , new SqlParameter("@AgeRating", g.AgeRating)
                , new SqlParameter("@GameGenre", g.GameGenre)
                , new SqlParameter("@MoreInfo", g.MoreInfo)
                , new SqlParameter("@ImageFileName", g.ImageFileName)
                , new SqlParameter("@InstallerFileName", g.InstallerFileName)
                , new SqlParameter("@PayPal", g.PayPal)
                ));
            return gId;
        }

        /// <summary>
        /// From SqlDataProvider Stored Procedure Parameters are:
        /// @GameId
        /// , @GameName nvarchar
        /// , @GameDescription
        /// , @DeveloperId
        /// , @ModuleId
        /// , @ContentItemId
        /// , @LastModifiedByUserId
        /// , @AgeRating
        /// , @GameGenre
        /// , @MoreInfo
        /// , @ImageFileName
        /// , @InstallerFileName
        /// </summary>
        /// <param name="g"></param>
        public override void UpdateGame(Game g)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, NamePrefix + "UpdateGame"
                , new SqlParameter("@GameId", g.GameId)
                , new SqlParameter("@GameName", g.GameName)
                , new SqlParameter("@GameDescription", g.GameDescription)
                , new SqlParameter("@DeveloperId", g.DeveloperId)
                , new SqlParameter("@ModuleId", g.ModuleId)
                , new SqlParameter("@ContentItemId", g.ContentItemId)
                , new SqlParameter("@LastModifiedByUserId", g.LastModifiedByUserId)
                , new SqlParameter("@AgeRating", g.AgeRating)
                , new SqlParameter("@GameGenre", g.GameGenre)
                , new SqlParameter("@MoreInfo", g.MoreInfo)
                , new SqlParameter("@ImageFileName", g.ImageFileName)
                , new SqlParameter("@InstallerFileName", g.InstallerFileName)
                , new SqlParameter("@PayPal", g.PayPal)
                );
        }

        /// <summary>
        /// From SqlDataProvider Stored Procedure Parameters are:
        /// @GameName nvarchar (max)
        /// , @GameId int
        /// , @GameDevId int
        /// , @ModuleId int
        /// , @DownloaderId int
        /// , @IsLegalDownload bit
        /// </summary>
        /// <param name="g"></param>
        public override void AddDownload(Download d)
        {
            SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, NamePrefix + "AddDownload"
                , new SqlParameter("@GameName", d.GameName)
                , new SqlParameter("@GameId", d.GameId)
                , new SqlParameter("@GameDevId", d.GameDevId)
                , new SqlParameter("@ModuleId", d.ModuleId)
                , new SqlParameter("@DownloaderId", d.DownloaderId)
                , new SqlParameter("@IsLegalDownload", d.IsLegalDownload ? 1 : 0)
                );
        }

        /// <summary>
        /// From SqlDataProvider Stored Procedure Parameters are:
        /// @GameId int
        /// , @DownloaderId int
        /// , @IsLegalDownload bit
        /// Update aims to just increment count of downloads for user hence low amount of required parameters.
        /// Legality of game can change over time as downloader ages.
        /// TODO: An idea would be to add to Downloads table a field for "Was ever illegaly downloaded"
        /// </summary>
        /// <param name="g"></param>
        public override void UpdateDownload(Download d)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, NamePrefix + "UpdateDownload"
                , new SqlParameter("@GameId", d.GameId)
                , new SqlParameter("@DownloaderId", d.DownloaderId)
                , new SqlParameter("@IsLegalDownload", d.IsLegalDownload ? 1 : 0)
                );
        }

        /// <summary>
        /// Check if user has downloaded specific game before.
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="downloaderId"></param>
        /// <returns>Data reader object containing result of query</returns>
        public override IDataReader CheckForDownload(int downloaderId, int gameId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, NamePrefix + "CheckForDownload"
                , new SqlParameter("@GameId", gameId)
                , new SqlParameter("@DownloaderId", downloaderId)
                );
        }

        public override void DeleteDownload(int gameId, int downloaderId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "DeleteDownload", new SqlParameter("@GameId", gameId), new SqlParameter("@DownloaderId", downloaderId));
        }
        #endregion

    }

}