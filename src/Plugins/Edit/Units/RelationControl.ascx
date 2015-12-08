<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RelationControl.ascx.cs"
    Inherits="EPiCode.Relations.Plugins.Edit.Units.RelationControl" %>
        <%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web"%>

<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="RemoveRelationLinkButton" EventName="Click" />
    </Triggers>
    <ContentTemplate>
        <asp:PlaceHolder runat="server" ID="ControlPlaceHolder">
            <div class="relationControl">
                <div class="relationControlImage">
                    <asp:LinkButton runat="server" ToolTip='<%#EPiCode.Relations.Core.LanguageHandler.Translate("/epicode.relations/editplugin/remove") %>' OnClick="RemoveRelation_Click"
                        ID="RemoveRelationLinkButton">
                        <img src="/App_Themes/Default/Images/General/deleteIcon.png" />
                    </asp:LinkButton>
                </div>
                <div class="relationControlText">
                     <a href='<%#RelatedPage.LinkURL %>' target="_top" title="[<%#RelatedPage.PageLink.ID %>]"><%#GetPageName() %></a>
                    
                </div>
            </div>
        </asp:PlaceHolder>
    </ContentTemplate>
</asp:UpdatePanel>
