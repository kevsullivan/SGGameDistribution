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
using System.Linq;
using System.Web.UI.WebControls;
using Christoc.Modules.SGGameDistribution.Components;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Utilities;

namespace Christoc.Modules.SGGameDistribution
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The View class displays the content
    /// 
    /// Typically your view control would be used to display content or functionality in your module.
    /// 
    /// View may be the only control you have in your project depending on the complexity of your module
    /// 
    /// Because the control inherits from SGGameDistributionModuleBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// 
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class View : SGGameDistributionModuleBase, IActionable
    {
        /*
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);
        }
        */
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // ModuleId Exposed from PortalModuleBase
                rptGameList.DataSource = GameController.GetGames(ModuleId);
                rptGameList.DataBind();
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// When list of games bound and each time game is added to list this gets called
        /// Runs through list and sets proper command arguments (for allowing edits).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RptGameListOnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                var linkEdit = e.Item.FindControl("linkEdit") as LinkButton;
                var linkDelete = e.Item.FindControl("linkDelete") as LinkButton;
                var panelAdminControls = e.Item.FindControl("panelAdmin") as Panel;

                var currentGame = (Game) e.Item.DataItem;

                // Check User Logged in has edit rights and edit control + panel exist.
                // TODO: Might be better to seperate contruction of Delete/Edit Visbilities e.g User Can edit games but not delete.
                if (IsEditable && linkDelete != null && linkEdit != null && panelAdminControls != null)
                {
                    panelAdminControls.Visible = true;
                    linkDelete.CommandArgument = linkEdit.CommandArgument = currentGame.GameId.ToString();
                    linkDelete.Enabled = linkDelete.Visible = linkEdit.Enabled = linkEdit.Visible = true;

                    // Adds JS to button to create popup confirmation dialog that is set in view.ascx.resx file
                    ClientAPI.AddButtonConfirm(linkDelete, Localization.GetString("ConfirmDelete", LocalResourceFile));
                }
                else
                {
                    panelAdminControls.Visible = false;
                }
            }
        }

        /// <summary>
        /// Runs when comand link clicked. In this case checking if command is edit.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public void RptGameListOnItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                // Build out url to edit control passing in ID for Game that requires editing.
                // TODO: Add popup functionality for editing games.
                Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(string.Empty, string.Empty, "ctl", "Edit", "mid=" + ModuleId, "gid=" + e.CommandArgument));
            }
            if (e.CommandName == "Delete")
            {
                GameController.DeleteGame(Convert.ToInt32(e.CommandArgument));
            }

            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL());
        }

        public ModuleActionCollection ModuleActions
        {
            get
            {
                var actions = new ModuleActionCollection
                    {
                        {
                            GetNextActionID(), Localization.GetString("EditModule", LocalResourceFile), "", "", "",
                            EditUrl(), false, SecurityAccessLevel.Edit, true, false
                        }
                    };
                return actions;
            }
        }
    }
}