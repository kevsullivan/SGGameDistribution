<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="Christoc.Modules.SGGameDistribution.View" %>

<asp:Repeater ID="rptGameList" runat="server" OnItemDataBound="RptGameListOnItemDataBound" OnItemCommand="RptGameListOnItemCommand">
    <HeaderTemplate>
    </HeaderTemplate>
    <ItemTemplate>
        <div>
            <div>
                <h3>
                <asp:Label ID="lblGameName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "GameName").ToString() %>'/>
                </h3>
            </div>
            <div id="imageContainer" height="100px" width="150px">
                 <asp:Image ID="gamePhoto" Width="150" runat="server"/>
            </div>
        </div>
        <div>
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
                <asp:ImageButton ID="PayPalBtn" runat="server" ImageUrl="https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif" param='<%#DataBinder.Eval(Container.DataItem, "PayPal").ToString() %>' OnClick="PayPalBtn_Click"/>

                <asp:linkButton ID="linkDownload" CommandName="Download" ResourceKey="DownloadGame.Text" runat="server"></asp:linkButton>

                <asp:Panel ID="panelAdmin" Visible="False" runat="server">
                    <asp:LinkButton ID="linkEdit" CommandName="Edit" ResourceKey="EditGame.Text" Visible="False" Enabled="False" runat="server"></asp:LinkButton>
                    &nbsp;
                    <asp:LinkButton ID="linkDelete" CommandName="Delete" ResourceKey="DeleteGame.Text" Visible="False" Enabled="False" runat="server"></asp:LinkButton>
                </asp:Panel>
        </div>
               <br/> 

    </ItemTemplate>
    <FooterTemplate>
    </FooterTemplate>
</asp:Repeater>

