<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RelationValidator.ascx.cs" EnableViewState="true"
    Inherits="EPiCode.Relations.Plugins.Admin.Units.RelationValidator" %>
<%@ Register TagPrefix="ET" TagName="RelationValidationControl" Src="RelationValidationControl.ascx" %>
<div class="relationValidator">
    <div class="well">
        <strong>Validate relations</strong><br/>
        Check if all relations are legal according to the current set of rules.<br/><br/>
        <asp:Button ID="Button8" CssClass="btn btn-success"
            runat="server" Text="Validate" OnClick="Validate_Click" />
                <asp:Button ID="RemoveInvalid" CssClass="btn btn-danger"
            runat="server" Text="Remove all invalid" OnClick="RemoveInvalid_OnClick" />
    <asp:Repeater runat="server" ID="Relations">
        <ItemTemplate>
            <ET:RelationValidationControl runat="server" CurrentRelation='<%#Container.DataItem as EPiCode.Relations.Core.Relation %>' />
        </ItemTemplate>
    </asp:Repeater>
    </div>
    <asp:Panel runat="server" Visible="false" ID="AllOkPanel">
        <div class="alert alert-success">
            All relations validates OK!
        </div>

    </asp:Panel>
</div>
