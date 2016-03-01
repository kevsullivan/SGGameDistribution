﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="Christoc.Modules.SGGameDistribution.View" %>

<asp:Repeater ID="rptGameList" runat="server" OnItemDataBound="RptGameListOnItemDataBound" OnItemCommand="RptGameListOnItemCommand">
    <HeaderTemplate>
        <ul class ="gameDist_gameList">   
    </HeaderTemplate>

    <ItemTemplate>
        <li class="gameDist_game">
            <h3>
                <asp:Label ID="lblGameName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "GameName").ToString() %>'/>
            </h3>
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

            <asp:Label ID="prefixAgeRating" runat="server" Text="Age Rating" CssClass="gameDist_PrefixLabel"></asp:Label>
            &nbsp;
            <asp:Label ID="lblAgeRating" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "AgeRating").ToString() %>' ></asp:Label>
            <br/>
           
            <asp:Label ID="prefixDownloadUrl" runat="server" Text="Download Url" CssClass="gameDist_PrefixLabel"></asp:Label>
            &nbsp;
            <a href='<%#DataBinder.Eval(Container.DataItem, "DownloadUrl").ToString() %>' target="_blank"><asp:Label ID="lblDownloadUrl" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "DownloadUrl").ToString() %>'></asp:Label></a>
            

            <asp:Panel ID="panelAdmin" Visible="False" runat="server">
                <asp:LinkButton ID="linkEdit" CommandName="Edit" ResourceKey="EditGame.Text" Visible="False" Enabled="False" runat="server"></asp:LinkButton>
                &nbsp;
                <asp:LinkButton ID="linkDelete" CommandName="Delete" ResourceKey="DeleteGame.Text" Visible="False" Enabled="False" runat="server"></asp:LinkButton>
            </asp:Panel>
        </li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>