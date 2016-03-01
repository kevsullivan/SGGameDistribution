/*
' Copyright (c) 2016  Christoc.com
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
using DotNetNuke.Entities.Modules;

namespace Christoc.Modules.SGGameDistribution
{
    /// <summary>
    /// Class can be used to provide any custom properties and methods that all controls can access.
    /// Can also access DNN Methods and Properties available off of Portalmodule base such as TabId, UserId, UserInfo etc.
    /// </summary>
    public class SGGameDistributionModuleBase : PortalModuleBase
    {
        /// <summary>
        /// GameId Property Allows for search of GameId from within Controls. 
        /// If returned value greater than 0 => Game being passed as query string parameter.
        /// If less than 0 => No Game Passed.
        /// </summary>
        /// <returns>An Interger The GameId if Found else -1</returns>
        public int GameId
        {
            get
            {
                var queryString = Request.QueryString["gid"];
                if (queryString != null)
                {
                    return Convert.ToInt32(queryString);
                }
                return -1;
            }

        }
        public int DownloadId
        {
            get
            {
                var queryString = Request.QueryString["DownloadId"];
                if (queryString != null)
                {
                    return Convert.ToInt32(queryString);
                }
                return -1;
            }

        }
    }
}