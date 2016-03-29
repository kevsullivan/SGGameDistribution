using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Content;

namespace Christoc.Modules.SGGameDistribution.Components
{
    public class Game:ContentItem
    {
        public Game() { }
        public Game(Game aGame)
        {
            //Clone content item data - NOTE: dnn base uses the same method as below for cloning (i.e. doesn't implement ICLonable)
            base.Clone(this, aGame);
            //TODO: I was going use IClonable for this cloning requirement but other devs recon its poor choice for few reasons including need for deep copy on objects within the game object (e.g DataTime)
            GameId = aGame.GameId;
            GameName = aGame.GameName;
            GameDescription = aGame.GameDescription;
            DeveloperId = aGame.DeveloperId;
            DeveloperName = aGame.DeveloperName;
            ModuleId = aGame.ModuleId;
            CreatedOnDate = aGame.CreatedOnDate;
            CreatedByUserIDId = aGame.CreatedByUserIDId;
            LastModifiedOnDate = aGame.LastModifiedOnDate;
            LastModifiedByUserId = aGame.LastModifiedByUserId;
            AgeRating = aGame.AgeRating;
            GameGenre = aGame.GameGenre;
            MoreInfo = aGame.MoreInfo;
            ImageFileName = aGame.ImageFileName;
            InstallerFileName = aGame.InstallerFileName;
            PayPal = aGame.PayPal;
            PortalId = aGame.PortalId;
            DownloadCount = aGame.DownloadCount;
            KeyID = aGame.KeyID;

        }
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
        /// Name of Serenity Gaming User who created the game.
        /// </summary>
        public string DeveloperName { get; set; }

        /// <summary>
        /// The moduleId of where the game was created and gets displayed
        /// </summary>
        public int ModuleId { get; set; }

        /// <summary>
        /// Date the game was originaly added to Serenity Gaming
        /// </summary>
        public DateTime CreatedOnDate { get; set; }

        /// <summary>
        /// Id of system administrator who ok'd the publishing of game to Serenity Gaming service.
        /// </summary>
        public int CreatedByUserIDId { get; set; }

        /// <summary>
        /// Date of last information modification of the game.
        /// Modifications could occur after patches etc. when new information about game is available.
        /// </summary>
        public DateTime LastModifiedOnDate { get; set; }

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
        /// Genre assoictaed with the game, can currently be one of the following:
        /// "FPS", "Action", "Adventure", "Indie", "Massive Multiplayer", "Racing", "RPG", "Sim", "Sports", "Strategy"
        /// </summary>
        public string GameGenre { get; set; }

        /// <summary>
        /// Link for more info on game.
        /// </summary>
        public string MoreInfo { get; set; }

        /// <summary>
        /// Name of Game Image.
        /// </summary>
        public string ImageFileName { get; set; }

        /// <summary>
        /// Name of Game Installer.
        /// </summary>
        public string InstallerFileName { get; set; }

        /// <summary>
        /// PayPal Email to send donations to.
        /// </summary>
        public string PayPal { get; set; }

        /// <summary>
        /// The portal where the game resides.
        /// </summary>
        public int PortalId { get; set; }

        /// <summary>
        /// Number of times the game was uniquely downloaded (1 count per user).
        /// </summary>
        public int DownloadCount { get; set; }

        /// <summary>
        /// Name of system administrator who ok'd the publishing of game to Serenity Gaming service.
        /// </summary>
        public string CreatedByUserIDUserName => CreatedByUserIDId != 0 ? DotNetNuke.Entities.Users.UserController.GetUserById(PortalId, CreatedByUserIDId).Username : Null.NullString;

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
            //TODO: solve out of bounds issue here when calling save game.
            //TODO: Currently out of bounds issue resolved by altering naming conventions e.g. VerifiedBy -> CreatedByUserId
            //TODO: Find source of naming conflicts so I can use personal naming conventions - Might be Fill Method (IHydratable) or Some stored Values in Templates for ContentItems Data Table Values.
            //NOTE: the issue could be out of date stored procedures.
            base.FillInternal(dr);
            // Map objects in Data reader to Game properties.
            GameId = Null.SetNullInteger(dr["GameId"]);
            ModuleId = Null.SetNullInteger(dr["ModuleID"]);
            GameName = Null.SetNullString(dr["GameName"]);
            GameDescription = Null.SetNullString(dr["GameDescription"]);
            DeveloperId = Null.SetNullInteger(dr["DeveloperId"]);
            //TODO out of bounds on Username fix datareader
            DeveloperName = Null.SetNullString(dr["Username"]);
            CreatedOnDate = Null.SetNullDateTime(dr["CreatedOnDate"]);
            CreatedByUserIDId = Null.SetNullInteger(dr["CreatedByUserID"]);
            LastModifiedByUserId = Null.SetNullInteger(dr["LastModifiedByUserID"]);
            LastModifiedOnDate = Null.SetNullDateTime(dr["LastModifiedOnDate"]);
            AgeRating = Null.SetNullInteger(dr["AgeRating"]);
            GameGenre = Null.SetNullString(dr["GameGenre"]);
            MoreInfo = Null.SetNullString(dr["MoreInfo"]);
            ImageFileName = Null.SetNullString(dr["ImageFileName"]);
            InstallerFileName = Null.SetNullString(dr["InstallerFileName"]);
            PayPal = Null.SetNullString(dr["PayPal"]);
            DownloadCount = Null.SetNullInteger(dr["Downloads"]);
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