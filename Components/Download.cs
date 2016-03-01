using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Content;

namespace Christoc.Modules.SGGameDistribution.Components
{
    public class Download:ContentItem
    {
        /// <summary>
        /// Id of the Download
        /// </summary>
        public int DownloadId { get; set; }

        /// <summary>
        /// Id of Game that was downloaded
        /// </summary>
        public int GameId { get; set; }

        /// <summary>
        /// Name of game that was downloaded
        /// </summary>
        public string GameName { get; set; }

        /// <summary>
        /// Id of user who downloaded the game
        /// </summary>
        public int DownloaderId { get; set; }

        /// <summary>
        /// The moduleId of where the game downloaded from.
        /// </summary>
        public int ModuleId { get; set; }

        /// <summary>
        /// Whether or not the downloader was of age in terms of age rating attached to the downloaded game.
        /// </summary>
        public bool IsLegalDownload { get; set; }

        /// <summary>
        /// Id of the games Developer.
        /// </summary>
        public int GameDevId { get; set; }

    }
}