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
using System.IO;
using System.Web.UI;
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
                ddlAgeRating.DataSource = new ArrayList { 0, 12, 15, 18, 21 };
                ddlAgeRating.DataBind();
                ddlGameGenre.DataSource = new ArrayList { "FPS", "Action", "Adventure", "Indie", "Massive Multiplayer", "Racing", "RPG", "Sim", "Sports", "Strategy" };
                ddlGameGenre.DataBind();

                // Can Check GameId from Property Defined in SGGameDistributionModuleBase. if > 0 it means editing a game so grab stored info
                if (GameId > 0)
                {
                    var game = GameController.GetGame(GameId);
                    if (game != null)
                    {
                        txtName.Text = game.GameName;
                        txtDescription.Text = game.GameDescription;
                        txtMoreInfo.Text = game.MoreInfo;
                        ddlDeveloper.Items.FindByValue(game.DeveloperId.ToString()).Selected = true;
                        //TODO: Auto select previous age in edit view and Game Genre
                        ddlAgeRating.Items.FindByValue(game.AgeRating.ToString()).Selected = true;
                        ddlGameGenre.Items.FindByValue(game.GameGenre).Selected = true;
                        txtPayPal.Text = game.PayPal;
                    }
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion

        /// <summary>
        /// Submit exists in but edit and add functionality - if editing the GameId will exist for stored game so we can just edit that
        /// Other wise create a new game with defined values.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void buttonSubmit_Click(object sender, EventArgs e)
        {
            Game g;
            var imageFilename = "";
            var installFilename = "";
            if (InstallUploadControl.HasFile)
            {
                try
                {
                    //Create Directory only creates if the folder doesn't already exist - it also recursivley creates all directories/subdirectories if they don't exist
                    //So I check for specific directory for the developer if not there create it and we will store their installers in this folder
                    //This way the dev handles their unique installer names and we can let them specify the installer name rather than handling it ourselves.
                    Directory.CreateDirectory(Server.MapPath("~\\SGData\\installers\\" + ddlDeveloper.SelectedValue + "\\"));
                    installFilename = Path.GetFileName(InstallUploadControl.FileName);
                    InstallUploadControl.SaveAs(Server.MapPath("~\\SGData\\installers\\" + ddlDeveloper.SelectedValue + "\\") + installFilename);
                    //StatusLabel.Text = "Upload status: File uploaded!";
                }
                catch (Exception ex)
                {
                    //StatusLabel.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
                    ClientMessageBox.Show("There was an issue with installer upload: " + ex, this);
                    return;
                }
            }
            //Checks gameId as only want to show the error if creating new game rather than editing
            else if (GameId <=0)
            {
                ClientMessageBox.Show("You must provide an installation file with all new games.", this);
                return;
            }
            if (FileUploadControl.HasFile)
            {
                try
                {
                    //TODO: add folder handling like with installers
                    Directory.CreateDirectory(Server.MapPath("~\\SGData\\images\\" + ddlDeveloper.SelectedValue + "\\"));
                    imageFilename = Path.GetFileName(FileUploadControl.FileName);
                    FileUploadControl.SaveAs(Server.MapPath("~\\SGData\\images\\" + ddlDeveloper.SelectedValue + "\\") + imageFilename);
                    //StatusLabel.Text = "Upload status: File uploaded!";
                }
                catch (Exception ex)
                {
                    //StatusLabel.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
                    ClientMessageBox.Show("There was an issue with image upload: " + ex, this);
                    return;
                }
            }
            
            if (GameId > 0)
            {
                g = GameController.GetGame(GameId);
                g.GameName = txtName.Text.Trim();
                g.GameDescription = txtDescription.Text.Trim();
                g.LastModifiedByUserId = UserId;
                g.LastModifiedOnDate = DateTime.Now;
                g.MoreInfo = txtMoreInfo.Text.Trim();
                //TODO: Validate reasoning for developerId to be editable. Perhaps better to be able to append Developers in case extra come in futrue patches to games.
                g.DeveloperId = Convert.ToInt32(ddlDeveloper.SelectedValue);
                g.AgeRating = Convert.ToInt32(ddlAgeRating.SelectedValue);
                //TODO: should they be entitled to change the game genre? Perhaps if the devloper is not happy with what was assigned orignally to the game after altrerations/patches
                g.GameGenre = ddlGameGenre.SelectedValue;
                // New file added means update filename to filename + gamesId for uniqueness else leave it as it was
                g.ImageFileName = imageFilename != "" ? imageFilename : g.ImageFileName;
                // Only override installer if new one passed in TODO: double check comparison to empty string ok (might need null)
                //TODO: Right now ensuring uniqueness by appending DevID to installer name - so the dev shouldn't duplicate his/her installers personally - there won't be conflicts with other dev either.
                g.InstallerFileName = installFilename != "" ? installFilename : g.InstallerFileName;
                g.PayPal = txtPayPal.Text.Trim();

            }
            else
            { 
                g = new Game
                {
                    DeveloperId = Convert.ToInt32(ddlDeveloper.SelectedValue),
                    CreatedOnDate = DateTime.Now,
                    CreatedByUserIDId = UserId,
                    //TODO: Sort out difference between Verified By and Last Modified By.
                    LastModifiedByUserId = UserId,
                    GameName = txtName.Text.Trim(),
                    GameDescription = txtDescription.Text.Trim(),
                    AgeRating = Convert.ToInt32(ddlAgeRating.SelectedValue),
                    MoreInfo = txtMoreInfo.Text.Trim(),
                    ModuleId = ModuleId,
                    GameGenre = ddlGameGenre.SelectedValue,
                    // File submitted for new game means store the file name + gameId for uniqueness otherwise use placeholder image.
                    //TODO: Right now ensuring uniqueness by appending DevID to installer name - so the dev shouldn't duplicate his/her installers personally - there won't be conflicts with other dev either.
                    ImageFileName = imageFilename != "" ? imageFilename : "placeholder.png",
                    InstallerFileName = installFilename,
                    PayPal = txtPayPal.Text.Trim()

                };
            }

            /*Check Dates
            DateTime outputDate;
            if (DateTime.TryParse(txtCreatedOnDate.Text.Trim(), out outputDate))
            {
                g.CreatedOnDate = outputDate;
            }*/

            GameController.SaveGame(g, TabId);
            //TODO: Pass parameters into NavigateUrl to Show message that Game was Saved.
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL());
        }
        // TODO: leaving for reference but not using anymore the upload is handled along with rest of game data.
        protected void UploadButton_Click(object sender, EventArgs e)
        {

            /*Required html: < asp:Button runat = "server" id = "UploadButton" text = "Upload" onclick = "UploadButton_Click" />
            //               <asp:Label runat="server" id="StatusLabel" text="Upload status: " />
            if (FileUploadControl.HasFile)
            {
                try
                {
                    string filename = Path.GetFileName(FileUploadControl.FileName);
                    FileUploadControl.SaveAs(Server.MapPath("~\\SGData\\images\\") + filename);
                    StatusLabel.Text = "Upload status: File uploaded!";
                }
                catch (Exception ex)
                {
                    StatusLabel.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
                }
            }*/
        }
        protected void buttonCancel_Click(object sender, EventArgs e)
        {
            //Return User to Page they one one prior to Adding Game.
            //To Return to specific page = pass in tabId etc to NavigateUrl()
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL());
        }

        public static class ClientMessageBox
        {

            public static void Show(string message, Control owner)
            {
                Page page = (owner as Page) ?? owner.Page;
                if (page == null) return;

                page.ClientScript.RegisterStartupScript(owner.GetType(),
                    "ShowMessage", string.Format("<script type='text/javascript'>alert('{0}')</script>",
                    message));

            }

        }
    }
}