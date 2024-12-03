<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RelationValidationControl.ascx.cs" EnableViewState="true"
    Inherits="EPiCode.Relations.Plugins.Admin.Units.RelationValidationControl" %>
<div class="validationControl">
    <div class="validationButtonRow">
        <asp:LinkButton runat="server" ToolTip='<%#EPiCode.Relations.Core.LanguageHandler.Translate("/epicode.relations/editplugin/remove") %>'
            OnCommand="RemoveRelation_Click" CommandArgument='<%#CurrentRelation.Id.ToString() %>' ID="RemoveRelationLinkButton">
                        <img src="/App_Themes/Default/Images/General/deleteIcon.png" />
        </asp:LinkButton>
    </div>
    <div class="validationText">
        <strong>[<%#CurrentRelation.RuleName %>]</strong>
        <%#GetPageName(CurrentRelation.PageIDLeft) %>
        (<%#CurrentRelation.PageIDLeft %>) < - >
        <%#GetPageName(CurrentRelation.PageIDRight) %>
        (<%#CurrentRelation.PageIDRight %>)
    </div>
    <div class="validationError">
        <%#EPiCode.Relations.Core.Validator.Validate(CurrentRelation.RuleName, CurrentRelation.PageIDLeft, CurrentRelation.PageIDRight, true).ToString() %>
    </div>
</div>
