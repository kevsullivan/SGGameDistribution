﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Content;

namespace Christoc.Modules.SGGameDistribution.Components
{
    public class Game:ContentItem
    {
        /// <summary>
        /// GameId Primary Key for Selected Game
        /// </summary>
        public int GameId { get; set; }

        /// <summary>
        /// Title of the Game
        /// </summary>
        public string GameName { get; set; }

        /// <summary>
        /// Description of Games elements - as laid out by Game's developer.
        /// </summary>
        public string GameDescription { get; set; }

        /// <summary>
        /// Id of Serenity Gaming User who created the game.
        /// </summary>
        public int DeveloperId { get; set; }

        /// <summary>
        /// The moduleId of where the game was created and gets displayed
        /// </summary>
        public int ModuleId { get; set; }

        /// <summary>
        /// Date the game was originaly added to Serenity Gaming
        /// </summary>
        public DateTime PublishedDate { get; set; }

        /// <summary>
        /// Id of system administrator who ok'd the publishing of game to Serenity Gaming service.
        /// </summary>
        public int VerifiedById { get; set; }

        /// <summary>
        /// Date of last information modification of the game.
        /// Modifications could occur after patches etc. when new information about game is available.
        /// </summary>
        public DateTime LastUpdatedOnDate { get; set; }

        /// <summary>
        /// Id of admin who implemented most recent modification.
        /// Currently only Admins may alter game information.
        /// TODO: Allow developer access to game editing within Serenity Gaming Platform.
        /// </summary>
        public int LastModifiedByUserId { get; set; }

        /// <summary>
        /// Age Rating assigned to the game.
        /// Age Ratings to be muttually agreed on by the developer's and Platform Admin.
        /// </summary>
        public int AgeRating { get; set; }

        /// <summary>
        /// Link for Game Download.
        /// </summary>
        public string DownloadUrl { get; set; }

        /// <summary>
        /// The portal where the game resides.
        /// </summary>
        public int PortalId { get; set; }

        /// <summary>
        /// Name of system administrator who ok'd the publishing of game to Serenity Gaming service.
        /// </summary>
        public string VerifiedByUserName => CreatedByUserID != 0 ? DotNetNuke.Entities.Users.UserController.GetUserById(PortalId, VerifiedById).Username : Null.NullString;

        /// <summary>
        /// Name of admin who implemented most recent modification.
        /// Currently only Admins may alter game information.
        /// TODO: Allow developer access to game editing within Serenity Gaming Platform.
        /// </summary>
        public string LastModifiedByUserName => LastModifiedByUserId != 0 ? DotNetNuke.Entities.Users.UserController.GetUserById(PortalId, LastModifiedByUserId).Username : Null.NullString;

        #region IHydratable Implementation

        /// <summary>
        /// Due to use of ContentItem - Require Implementation of IHydratable for Game
        /// IHydratable requires implementing Fill and KeyID.
        /// Automates process of mapping up datareader to game object.
        /// </summary>
        /// <param name="dr"></param>
        public override void Fill(IDataReader dr)
        {
            base.FillInternal(dr);
            // Map objects in Data reader to Game properties.
            GameId = Null.SetNullInteger(dr["GameId"]);
            ModuleId = Null.SetNullInteger(dr["ModuleID"]);
            GameName = Null.SetNullString(dr["GameName"]);
            GameDescription = Null.SetNullString(dr["GameDescription"]);
            DeveloperId = Null.SetNullInteger(dr["DeveloperId"]);
            PublishedDate = Null.SetNullDateTime(dr["PublishedDate"]);
            VerifiedById = Null.SetNullInteger(dr["VerifiedById"]);
            LastModifiedByUserId = Null.SetNullInteger(dr["LastModifiedById"]);
            LastUpdatedOnDate = Null.SetNullDateTime(dr["LastUpdatedOnDate"]);
            AgeRating = Null.SetNullInteger(dr["AgeRating"]);
            DownloadUrl = Null.SetNullString(dr["DownloadUrl"]);
        }

        /// <summary>
        /// Get/Set Key ID
        /// </summary>
        /// <returns>An Integer</returns>
        public override int KeyID
        {
            get { return GameId; }
            set { GameId = value; }
        }
        #endregion
    }
}