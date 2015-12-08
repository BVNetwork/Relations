<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImportExport.ascx.cs" EnableViewState="true"
    Inherits="EPiCode.Relations.Plugins.Admin.Units.ImportExport" %>
<br />

  <div class="well">
<div class="checkbox">
<label>    <asp:CheckBox runat="server" ID="CheckBoxRules" CssClass="" Checked="true" Text="Include rules" /><br />
    <asp:CheckBox runat="server" ID="CheckBoxRelations" CssClass="" Checked="true" Text="Include relations" />
    </label>
</div>

<br/>
        <asp:Button ID="Button8" CssClass="btn btn-success"
            runat="server" Text="Export" OnClick="Export_Click" />
  
  
        <asp:Button ID="Button1" CssClass="btn btn-success"
            runat="server" Text="Import" OnClick="Import_Click" />
      </div>

<div class="alert alert-success">
   <strong> Relation import / export data<br/></strong>
<asp:TextBox ID="DataTextBox" runat="server" Rows="9" Width="700" TextMode="MultiLine"></asp:TextBox>
    </div>
<div class="alert alert-warning">
    <strong>Status</strong>
<div class="statusTextBox">
    <asp:Label runat="server" ID="StatusLabel"></asp:Label>
</div>
</div>