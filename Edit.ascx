<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Christoc.Modules.SGGameDistribution.Edit" %>
<%@ Register TagPrefix="dnn" TagName="label" src="~/controls/LabelControl.ascx" %>

<dnn:label ID="lblName" runat="server" Text="Name" HelpText="Name of the Game"></dnn:label>
<asp:TextBox ID="txtName" runat="server"></asp:TextBox>
<br/>

<dnn:label ID="lblDescription" runat="server" Text="Description" HelpText="Description of the Game"></dnn:label>
<asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="5" Columns="50"></asp:TextBox>
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

<!-- attempt at upload implementation 
    TODO: wire up upload/download functionality
    TODO: ensure dnn file restrictions are active if not impleemnt my own ones on uploads.
    TODO: Add default image functionality i.e if they don't supply image give it a default one based on genre. Make Genre required.
    TODO: The actuall install needs be required - enforce this.
-->

<dnn:label ID="lblImage" runat="server" Text="Image" HelpText="Image to display with the game"></dnn:label>
<asp:FileUpload id="FileUploadControl" runat="server" text="Image File"/>
<br/>
<dnn:label ID="lblInstaller" runat="server" Text="Installer" HelpText="Install File for the game"></dnn:label>
<asp:FileUpload id="InstallUploadControl" runat="server" text="Install File"/>

<div class="right">
   <asp:LinkButton ID="buttonSubmit" Text="Submit" runat="server" OnClick="buttonSubmit_Click"></asp:LinkButton>
    &nbsp;
    <asp:LinkButton ID="buttonCancel" Text="Cancel" runat="server" OnClick="buttonCancel_Click"></asp:LinkButton> 
</div>
