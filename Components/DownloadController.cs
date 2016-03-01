using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Christoc.Modules.SGGameDistribution.Data;

namespace Christoc.Modules.SGGameDistribution.Components
{
    public class DownloadController
    {
        /// <summary>
        /// Saves Game details within system.
        /// Used for adding and updating of game.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="tabId"></param>
        /// <returns>ID of Saved Game</returns>
        public static int SaveDownload(Download d, int tabId)
        {
            if (d.DownloadId < 1)
            {
                DataProvider.Instance().AddDownload(d);
            }
            else
            {
                DataProvider.Instance().UpdateDownload(d);
            }
            // TODO: Can probably remove this return as I'm not using content item integration for downloads
            return d.DownloadId;
        }
    }
}