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
using System.Collections;
using Christoc.Modules.SGGameDistribution.Components;
using DotNetNuke.Entities.Users;
using DotNetNuke.Services.Exceptions;

namespace Christoc.Modules.SGGameDistribution
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The EditSGGameDistribution class is used to manage content
    /// 
    /// Typically your edit control would be used to create new content, or edit existing content within your module.
    /// The ControlKey for this control is "Edit", and is defined in the manifest (.dnn) file.
    /// 
    /// Because the control inherits from SGGameDistributionModuleBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// 
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class Edit : SGGameDistributionModuleBase
    {
        #region Initialization Handlers.

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);
        }
        /// <summary>
        /// Runs when the control is loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsPostBack) return;
                //Get List of All users for current portal
                //Poral ID accesble from SGGammeDistributionModuleBase inheretence - SGGammeDistributionModuleBase inherits from PortalModuleBase.
                ddlDeveloper.DataSource = UserController.GetUsers(PortalId);
                ddlDeveloper.DataTextField = "Username";
                ddlDeveloper.DataValueField = "UserId";
                ddlDeveloper.DataBind();

                ddlAgeRating.DataSource = new ArrayList {0, 12, 15, 18, 21};
                ddlAgeRating.DataBind();
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion

        protected void buttonSubmit_Click(object sender, EventArgs e)
        {
            Game g;
            g = new Game
            { 
                DeveloperId = Convert.ToInt32(ddlDeveloper.SelectedValue),
                PublishedDate = DateTime.Now,
                VerifiedById = UserId,
                GameName = txtName.Text.Trim(),
                GameDescription = txtDescription.Text.Trim(),
                AgeRating = Convert.ToInt32(ddlAgeRating.SelectedValue),
                DownloadUrl = txtDownloadUrl.Text.Trim()
            };
            GameController.SaveGame(g, TabId);
            //TODO: Pass parameters into NavigateUrl to Show message that Game was Saved.
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL());
        }

        protected void buttonCancel_Click(object sender, EventArgs e)
        {
            //Return User to Page they one one prior to Adding Game.
            //To Return to specific page = pass in tabId etc to NavigateUrl()
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL());
        }
    }
}