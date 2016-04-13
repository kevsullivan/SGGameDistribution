/*
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Christoc.Modules.SGGameDistribution.Components;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Utilities;
using System.Web.UI.HtmlControls;
using DotNetNuke.Entities.Users;

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
            //this.Load += new System.EventHandler(this.Page_Load);
            this.Page.PreLoad += Page_PreLoad;
        }
        */

        private List<Game> games
        {
            get
            {
                if (this.Session["games"] == null)
                    return GameController.GetGames(ModuleId);

                return (List<Game>)this.Session["games"];
            }
            set { this.Session["games"] = value; }
        }
        /* TODO: I want to try get list of games showing like Image and header to left then added info on rightside instead of underneat - just requires some html/css design that I can't waste time on right now */
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) return;
            // ModuleId Exposed from PortalModuleBase
            Session["games"] = games = GameController.GetGames(ModuleId);
            LoadData();
            
        }

        public void LoadData()
        {
            try
            {
                
                orderByList.DataSource = new ArrayList { "Newest First", "Oldest First", "Popularity" };
                orderByList.DataBind();

                refineOptions.DataSource = new ArrayList { "All", "FPS", "Action", "Adventure", "Indie", "Massive Multiplayer", "Racing", "RPG", "Sim", "Sports", "Strategy" };
                refineOptions.DataBind();

                var devs = GameController.GetDevs();
                devs.Insert(0, "All");
                developers.DataSource = devs;
                developers.DataBind();


                //Use of temp games means only call into db on page load but can still mess with and refine data in this method without reloading page. (Usefull for search refines and/or paging)
                //TODO: make sure no issue of shallow vs deep copy here
                var sortedGames = games.ConvertAll(game => new Game(game));
                
                //Only enter these if refining in action otherwise we bind full list of games.
                if (Session["Genre"] != null && Session["Genre"] as string != "All")
                {
                    refineOptions.SelectedValue = Session["Genre"] as string;
                    sortedGames.RemoveAll(elem => elem.GameGenre != Session["Genre"] as string);
                }
                if (Session["Dev"] != null && Session["Dev"] as string != "All")
                {
                    developers.SelectedValue = Session["Dev"] as string;
                    sortedGames.RemoveAll(elem => elem.DeveloperName != Session["Dev"] as string);
                }
                if (Session["Order"] != null)
                {
                    orderByList.SelectedValue = Session["Order"] as string;
                }
                
                switch (orderByList.SelectedValue)
                {
                    case "Newest First":
                        sortedGames = sortedGames.OrderByDescending(o => o.CreatedOnDate).ToList();
                        break;
                    case "Olderst First":
                        sortedGames = sortedGames.OrderBy(o => o.CreatedOnDate).ToList();
                        break;
                    case "Popularity":
                        sortedGames = sortedGames.OrderByDescending(o => o.DownloadCount).ToList();
                        break;
                }
                PagedDataSource page = new PagedDataSource();
                page.DataSource = sortedGames;
                page.AllowPaging = true;
                page.PageSize = 5;
                page.CurrentPageIndex = CurrentPage;

                lblCurrentPageT.Text = lblCurrentPageB.Text = "Page: " + (CurrentPage + 1).ToString() + " of "
                + page.PageCount.ToString();

                // Disable Prev or Next buttons if necessary
                cmdPrevT.Enabled = cmdPrevB.Enabled = !page.IsFirstPage;
                cmdNextT.Enabled = cmdNextB.Enabled = !page.IsLastPage;

                rptGameList.DataSource = page;
                rptGameList.DataBind();

            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        public int CurrentPage
        {
            get
            {
                // look for current page in ViewState
                // TODO: may swap to viewstate and move pageload handling into Method rather than Page_Load so don't need to refresh page
                object o = this.Session["_CurrentPage"];
                if (o == null)
                    return 0; // default page index of 0
                else
                    return (int) o;
            }

            set
            {
                this.Session["_CurrentPage"] = value;
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
            //This gets called for each item.
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                
                var linkEdit = e.Item.FindControl("linkEdit") as LinkButton;
                var linkDelete = e.Item.FindControl("linkDelete") as LinkButton;
                var linkDownload = e.Item.FindControl("linkDownload") as LinkButton;
                var panelAdminControls = e.Item.FindControl("panelAdmin") as Panel;
                var paypalDonateButton = e.Item.FindControl("PayPalBtn") as ImageButton;
                var image = e.Item.FindControl("gamePhoto") as Image;
                var currentGame = (Game) e.Item.DataItem;
                var video = e.Item.FindControl("video") as HtmlControl;
                var itemBreaker = e.Item.FindControl("spacer") as HtmlControl;

                if (video != null)
                {
                    try
                    {
                        var request = (HttpWebRequest)HttpWebRequest.Create(currentGame.MoreInfo);
                        request.Method = "HEAD";
                        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                        {
                            video.Attributes["src"] = response.ResponseUri.ToString().Contains("youtube.com") ? currentGame.MoreInfo.Replace("watch?v=", "embed/") : "https://www.youtube.com/embed/RSMrtl2VbFE";
                        }
                    }
                    catch (Exception)
                    {
                        //TODO: create my own coming soon vid
                        video.Attributes["src"] = "https://www.youtube.com/embed/RSMrtl2VbFE";
                    }
                    
                }
                else
                {
                    video.Attributes["src"] = "https://www.youtube.com/embed/RSMrtl2VbFE";
                }
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
                if (paypalDonateButton != null)
                {
                    var payPalAddress = currentGame.PayPal;
                    if (string.IsNullOrEmpty(payPalAddress))
                    {
                        paypalDonateButton.Enabled = paypalDonateButton.Visible = false;
                    }
                    else
                    {
                        paypalDonateButton.Enabled = true;
                    }
                }
                
                if (image != null)
                {
                    //TODO: might have to pass default file here somehow
                    
                    if (currentGame.ImageFileName == "placeholder.png")
                    {
                        image.ImageUrl = "images/placeholder.png";
                    }
                    else
                    {
                        Directory.CreateDirectory(Server.MapPath("~\\SGData\\images\\" + currentGame.DeveloperId + "\\"));
                        image.ImageUrl = "~/SGData/images/" + currentGame.DeveloperId + "/" + currentGame.ImageFileName;
                        
                        if (!File.Exists(Server.MapPath("~\\SGData\\images\\" + currentGame.DeveloperId + "\\") + currentGame.ImageFileName))
                        {
                            image.ImageUrl = "images/placeholder.png";
                        }
                    }
                    
                }
                if (linkDownload != null)
                {
                    linkDownload.CommandArgument = currentGame.GameId.ToString();
                    var downloadLink = currentGame.InstallerFileName;
                    if (string.IsNullOrEmpty(downloadLink))
                    {
                        linkDownload.Enabled = linkDownload.Visible = false;
                        //TODO this return means nothing else handled after download
                        return;
                    }
                    // Adds JS to button to create popup announcement that game is downloading
                    ClientAPI.AddButtonConfirm(linkDownload, Localization.GetString("ConfirmDownload", LocalResourceFile));
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
            //NOTE: Only called after user confims selection
            if (e.CommandName == "Delete")
            {
                GameController.DeleteGame(Convert.ToInt32(e.CommandArgument));
            }
            //TODO: Handle downloads
            //NOTE: Only called after user confims selection
            if (e.CommandName == "Download")
            {
                //e.CommandArgument = gid - downloads attached to stored games so guarenteed valid gid.
                Game g = GameController.GetGame(Convert.ToInt32(e.CommandArgument));
                Download d = DownloadController.CheckDownload(UserId, g.GameId);
                // if (DownloadBefore(UserId)) ;
                // TODO: This probably isn't best solution for checking on downloads.
                if (d != null)
                {
                    d.GameId = g.GameId;
                    d.GameDevId = g.DeveloperId;
                    d.GameName = g.GameName;
                    d.DownloaderId = UserId;
                    d.ModuleId = ModuleId;
                    //TODO: Handle legal downloads
                    d.IsLegalDownload = true;
                }
                else
                {
                    d = new Download
                    {
                        GameId = g.GameId,
                        GameDevId = g.DeveloperId,
                        GameName = g.GameName,
                        DownloaderId = UserId,
                        ModuleId = ModuleId,
                        IsLegalDownload = true
                    };
                }

                DownloadController.SaveDownload(d, TabId);
                if (!(string.IsNullOrEmpty(g.InstallerFileName)))
                {
                    try
                    {
                        Response.ContentType = "application/exe";
                        Response.AppendHeader("Content-Disposition", "attachment; filename=" + g.InstallerFileName);
                        Response.TransmitFile(Server.MapPath("~/SGData/installers/" + g.DeveloperId + "/" + g.InstallerFileName));
                        Response.End();

                    }
                    catch (Exception ex)
                    {

                        ClientMessageBox.Show("Sorry something went wrong while downloading: " + ex, this);
                    }
                }
            }

            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL());
        }

        //TODO: this isn't working correctly in several places need a new method to provide client feedback
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

        public void PayPalBtn_Click(object sender, ImageClickEventArgs e)
        {
            var paypal = sender as ImageButton;
            var value = paypal.Attributes["param"];
            if (value == "")
            {
                ClientMessageBox.Show("Sorry the developer hasn't added a PayPal account", this);
                return;
            }
            
            string url = "";
            string business = value;
            string description = "Donation";
            string country = "IRL";
            string currency = "EUR";
            url += "https://www.paypal.com/cgi-bin/webscr" +
                    "?cmd=" + "_donations" +
                    "&business=" + business +
                    "&lc=" + country +
                    "&item_name=" + description +
                    "&currency_code=" + currency +
                    "&bn=" + "PP%2dDonationsBF";
            Response.Redirect(url);
        }

        public void RefineRepeater(object sender, EventArgs e)
        {
            Session["Genre"] = refineOptions.SelectedValue;
            Session["Dev"] = developers.SelectedValue;
            Session["Order"] = orderByList.SelectedValue;
            LoadData();
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

        protected void cmdNextT_Click1(object sender, EventArgs e)
        {
            // Set viewstate variable to the next page
            CurrentPage += 1;

            // Reload control
            LoadData();
        }

        protected void cmdPrevT_Click1(object sender, EventArgs e)
        {
            // Set viewstate variable to the previous page
            CurrentPage -= 1;

            // Reload control
            LoadData();
        }

        protected void cmdNextB_Click1(object sender, EventArgs e)
        {
            // Set viewstate variable to the next page
            CurrentPage += 1;

            // Reload control
            LoadData();
        }

        protected void cmdPrevB_Click1(object sender, EventArgs e)
        {
            // Set viewstate variable to the previous page
            CurrentPage -= 1;

            // Reload control
            LoadData();
        }
    }
}
