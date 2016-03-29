<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="Christoc.Modules.SGGameDistribution.View" %>
<link type="text/css" rel="stylesheet" href="/bootstrap.css"/>
<div class="row" id="options">
<div class="col-md-4" id="orderBy">
    <asp:Label runat="server" Text="Order By: "></asp:Label>
    <asp:DropDownList id="orderByList" AutoPostBack="True" OnSelectedIndexChanged="RefineRepeater" runat="server"></asp:DropDownList>
</div>
<div class="col-md-4" id="refineGenre">
    <asp:Label runat="server" Text="Genre: "></asp:Label>
    <asp:DropDownList id="refineOptions" AutoPostBack="True" OnSelectedIndexChanged="RefineRepeater" runat="server"></asp:DropDownList>
</div>
<div class="col-md-4" id="refineDev">
    <asp:Label runat="server" Text="Developer: "></asp:Label>
    <asp:DropDownList id="developers" AutoPostBack="True" OnSelectedIndexChanged="RefineRepeater" runat="server"></asp:DropDownList>
</div>
<div>
</div>
<br/><br/>
<table width="100%" border="0">
   <tr>
      <td>  Page Navigation</td>
   </tr>
   <tr>
      <td>  <asp:label id="lblCurrentPageT" runat="server"></asp:label></td>
   </tr>
   <tr>
      <td>  <asp:button id="cmdPrevT" runat="server" text=" << " OnClick="cmdPrevT_Click1"></asp:button>
          <asp:button id="cmdNextT" runat="server" text=" >> " OnClick="cmdNextT_Click1"></asp:button></td>
   </tr>
</table>
<br/>
<asp:Repeater ID="rptGameList" runat="server" OnItemDataBound="RptGameListOnItemDataBound" OnItemCommand="RptGameListOnItemCommand">
    <HeaderTemplate>
    </HeaderTemplate>
    <ItemTemplate>
        <div class="row" id="itemDiv" runat="server" style="box-shadow: 0px 0px 5px #33ccff;
                                                            padding-top: 10px;
                                                            padding-bottom: 10px;">
            <div class="col-md-3" id="titleDiv" runat="server"><!-- Start title div -->
                <div>
                    <h3>
                    <asp:Label ID="lblGameName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "GameName").ToString() %>'/>
                    </h3>
                </div>
                <div id="imageContainer" height="100px" width="150px">
                     <asp:Image ID="gamePhoto" Width="150" runat="server"/>
                </div>
            </div><!-- End title div -->
                <br/><br/><!-- Line up title and content displays -->
            <div class="col-md-3" id="contentDiv" runat="server"><!-- Start content div -->
                <asp:Label ID="prefixDeveloper" runat="server" Text="Developer:" CssClass="gameDist_PrefixLabel"></asp:Label>
                    &nbsp;
                    <asp:Label ID="lblDeveloper" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "DeveloperName").ToString() %>' CssClass="gameDist_Developer"></asp:Label>
                    <br/>

                    <asp:Label ID="prefixCreatedOnDate" runat="server" Text="Published Date:" CssClass="gameDist_PrefixLabel"></asp:Label>
                    &nbsp;
                    <asp:Label ID="lblCreatedOnDate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "CreatedOnDate").ToString() %>' CssClass="gameDist_CreatedOnDate"></asp:Label>
                    <br/>

                    <asp:Label ID="prefixGameDescription" runat="server" Text="Game Description:" CssClass="gameDist_PrefixLabel"></asp:Label>
                    &nbsp;
                    <p>
                        <asp:Label ID="lblGameDescription" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "GameDescription").ToString() %>' CssClass="gameDist_gameDescription"></asp:Label>
                    </p>

                    <asp:Label ID="prefixGameGenre" runat="server" Text="Game Genre:" CssClass="gameDist_PrefixLabel"></asp:Label>
                    &nbsp;
                    <asp:Label ID="lblGameGenre" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "GameGenre").ToString() %>'></asp:Label>
                    <br/>

                    <asp:Label ID="prefixAgeRating" runat="server" Text="Age Rating:" CssClass="gameDist_PrefixLabel"></asp:Label>
                    &nbsp;
                    <asp:Label ID="lblAgeRating" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "AgeRating").ToString() %>'></asp:Label>
                    <br/>

                    <asp:Label ID="prefixDownloadCount" runat="server" Text="Number of Downloads:" CssClass="gameDist_PrefixLabel"></asp:Label>
                    &nbsp;
                    <asp:Label ID="lblDownloadCount" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "DownloadCount").ToString() %>'></asp:Label>
                    <br/>

                    <asp:ImageButton ID="PayPalBtn" runat="server" ImageUrl="https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif" param='<%#DataBinder.Eval(Container.DataItem, "PayPal").ToString() %>' OnClick="PayPalBtn_Click"/>

                    <asp:linkButton ID="linkDownload" CommandName="Download" ResourceKey="DownloadGame.Text" runat="server"></asp:linkButton>

                    <asp:Panel ID="panelAdmin" Visible="False" runat="server">
                        <asp:LinkButton ID="linkEdit" CommandName="Edit" ResourceKey="EditGame.Text" Visible="False" Enabled="False" runat="server"></asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="linkDelete" CommandName="Delete" ResourceKey="DeleteGame.Text" Visible="False" Enabled="False" runat="server"></asp:LinkButton>
                    </asp:Panel>
            </div><!-- End div for game content -->
            <div class="col-md-6" id="videoDiv" runat="server"><!-- Div for video -->
                <iframe id="video" runat="server" style="width: 80%;" height="315" src='<%#DataBinder.Eval(Container.DataItem, "MoreInfo").ToString() %>' allowfullscreen></iframe>
            </div>
        </div><!-- End item div -->
        <br id ="spacer" runat="server"/>
    </ItemTemplate>
    <FooterTemplate>
    </FooterTemplate>
</asp:Repeater>
<table width="100%" border="0">
   <tr>
      <td>  Page Navigation</td>
   </tr>
   <tr>
      <td>  <asp:label id="lblCurrentPageB" runat="server"></asp:label></td>
   </tr>
   <tr>
      <td>  <asp:button id="cmdPrevB" runat="server" text=" << " OnClick="cmdPrevB_Click1"></asp:button>
          <asp:button id="cmdNextB" runat="server" text=" >> " OnClick="cmdNextB_Click1"></asp:button></td>
   </tr>
</table>
</div>

