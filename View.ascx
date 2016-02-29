<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="Christoc.Modules.SGGameDistribution.View" %>

<asp:Repeater ID="rptGameList" runat="server">
<HeaderTemplate>
    <ul class ="gameDist_gameList">   
</HeaderTemplate>

<ItemTemplate>
    <li class="gameDist_game">
        <h3>
            <asp:Label ID="lblGameName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "GameName").ToString() %>'/>
        </h3>
        <asp:Label ID="lblCreatedOnDate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "CreatedOnDate").ToString() %>' CssClass="gameDist_CreatedOnDate"/>
        <asp:Label ID="lblGameDescription" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "GameDescription").ToString() %>' CssClass="gameDist_gameDescription"/>
    
    </li>
</ItemTemplate>
<FooterTemplate>
</ul>
</FooterTemplate>
</asp:Repeater>