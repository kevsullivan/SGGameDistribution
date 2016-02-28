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
using System.Data;
using System.Data.SqlClient;
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
        /// , @PublishedDate
        /// , @VerifiedBy
        /// , @AgeRating
        /// , @DownloadUrl nvarchar
        /// </summary>
        /// <param name="g"></param>
        /// <returns>An Integer = the ID of game that gets created.</returns>
        public override int AddGame(Game g)
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, NamePrefix + "AddGame"
                , new SqlParameter("@GameName", g.GameName)
                , new SqlParameter("@GameDescription", g.GameDescription)
                , new SqlParameter("@DeveloperId", g.DeveloperId)
                , new SqlParameter("@ModuleId", g.ModuleId)
                , new SqlParameter("@PublishedDate", g.PublishedDate)
                , new SqlParameter("@VerifiedBy", g.VerifiedById)
                , new SqlParameter("@AgeRating", g.AgeRating)
                , new SqlParameter("@DownloadUrl", g.DownloadUrl)
                ));
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
        /// , @DownloadUrl
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
                , new SqlParameter("@LastModifiedBy", g.LastModifiedByUserId)
                , new SqlParameter("@AgeRating", g.AgeRating)
                , new SqlParameter("@DownloadUrl", g.DownloadUrl)
                );
        }
        #endregion

    }

}