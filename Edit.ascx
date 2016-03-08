<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Christoc.Modules.SGGameDistribution.Edit" %>
<%@ Register TagPrefix="dnn" TagName="label" src="~/controls/LabelControl.ascx" %>

<dnn:label ID="lblName" runat="server" Text="Name" HelpText="Name of the Game"></dnn:label>
<asp:TextBox ID="txtName" runat="server"></asp:TextBox>
<br/>

<dnn:label ID="lblDescription" runat="server" Text="Description" HelpText="Description of the Game"></dnn:label>
<asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="5" Columns="100"></asp:TextBox>
<br/>

<dnn:label ID="lblDeveloper" runat="server" Text="Developer" HelpText="Developer who created the game."></dnn:label>
<asp:DropDownList ID="ddlDeveloper" runat="server"></asp:DropDownList>
<br/>

<dnn:label ID="lblAgeRating" runat="server" Text="Age Rating" HelpText="Suggested age rating for the game."></dnn:label>
<asp:DropDownList ID="ddlAgeRating" runat="server"></asp:DropDownList>
<br/>

<dnn:label ID="lblGenre" runat="server" Text="Game Genre" HelpText="What category the game falls into"></dnn:label>
<asp:DropDownList ID="ddlGameGenre" runat="server"></asp:DropDownList>
<br/>

<dnn:label ID="lblDownloadUrl" runat="server" Text="Download" HelpText="Download Link for the game."></dnn:label>
<asp:TextBox ID="txtDownloadUrl" runat="server"></asp:TextBox>
<br/>

<asp:LinkButton ID="buttonSubmit" Text="Submit" runat="server" OnClick="buttonSubmit_Click"></asp:LinkButton>
&nbsp;
<asp:LinkButton ID="buttonCancel" Text="Cancel" runat="server" OnClick="buttonCancel_Click"></asp:LinkButton>