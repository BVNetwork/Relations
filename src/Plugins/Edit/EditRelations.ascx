<%@ Control Language="c#" CodeBehind="EditRelations.ascx.cs" AutoEventWireup="False"
    Inherits="EPiCode.Relations.Plugins.Edit.EditRelations" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web" %>
<%@ Register TagPrefix="ET" TagName="RelationSelector" Src="~/EPiCode/Relations/Plugins/Edit/Units/RelationSelector.ascx" %>

<link rel="stylesheet" type="text/css" href="/EPiCode/Relations/Styles/Edit.css" />
<div class="epi-contentContainer epi-padding">
    <div class="epi-contentArea">
        <h1>
            <%=string.Format(EPiCode.Relations.Core.LanguageHandler.Translate("/epicode.relations/editplugin/pagesRelated"), CurrentPage.PageName) %>
        </h1>
        <asp:Repeater runat="server" ID="RulesRepeater">
            <ItemTemplate>
                <ET:RelationSelector runat="server" CurrentRule='<%#(Container.DataItem as EPiCode.Relations.Core.Rule) %>' />
            </ItemTemplate>
        </asp:Repeater>
        <div class="EP-validationSummary" visible="false" runat="server" id="ErrorMessageContainer">
            <div class="noRelations">
                No relations are available for page type '<%=CurrentPage.PageTypeName %>'.
            </div>
        </div>
    </div>
</div>
<script type="text/javascript" src="/EPiCode/Relations/Script/RelationEditor.js" />
