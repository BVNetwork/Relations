<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="RelationSelector.ascx.cs"
    Inherits="EPiCode.Relations.Plugins.Edit.Units.RelationSelector" %>
    <%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web"%>
    <%@ Register TagPrefix="ET" TagName="RelationControl" Src="~/EPiCode/Relations/Plugins/Edit/Units/RelationControl.ascx" %>
<div class="relationSelectorContainer">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional"  >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Button2" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="PageTypesDropDown" EventName="SelectedIndexChanged" />
        </Triggers>
        <ContentTemplate>
            <div style="" class="relationSelector">
                <div style="" class="header">
                    <%=string.Format(IsLeftRule ? EPiCode.Relations.Core.LanguageHandler.TranslateRuleTextLeft(CurrentRule) : EPiCode.Relations.Core.LanguageHandler.TranslateRuleTextRight(CurrentRule), CurrentPage.PageName)%>
                    (<%= EPiCode.Relations.Core.LanguageHandler.TranslateRuleName(CurrentRule)%>)</div>
                <div style="" class="browseArea">
                    <asp:PlaceHolder runat="server" ID="PageTypesContainer" Visible="false">
                    <div style="" class="pageTypeFilter">
                    <%=EPiCode.Relations.Core.LanguageHandler.Translate("/epicode.relations/editplugin/pageTypeFilter")%><asp:DropDownList ID="PageTypesDropDown" runat="server" AutoPostBack="true" OnSelectedIndexChanged="PageTypes_Changed" >
                    </asp:DropDownList>
                    </div>
                    </asp:PlaceHolder>
                    <asp:ListBox ID="AllRelations" runat="server" SelectionMode="Multiple" Width="355"
                        Height="170" DataTextField="PageName" DataValueField="PageLink"></asp:ListBox>
                    <br />
                    <div style="margin-top: 5px;">
                        <asp:TextBox runat="server" ID="SearchKeyWord" Width="265"></asp:TextBox>
                        <span class="epi-buttonDefault searchButton" style=""><span
                            class="epi-cmsButton">
                            <asp:Button ID="Button2" runat="server" Text='<%#EPiCode.Relations.Core.LanguageHandler.Translate("/epicode.relations/editplugin/search") %>' CssClass="epi-cmsButton-text epi-cmsButton-tools epi-cmsButton-Search"
                                OnClick="PerformSearch_Click" />
                        </span></span>
                    </div>
                    <br />
                    <span class="epi-buttonDefault"><span class="epi-cmsButton">
                        <asp:Button ID="Button1" runat="server" Text='<%#EPiCode.Relations.Core.LanguageHandler.Translate("/epicode.relations/editplugin/addRelations") %>' CssClass="epi-cmsButton-text epi-cmsButton-tools epi-cmsButton-Add"
                            OnClick="AddRelations_Click" />
                    </span></span>
                </div>
                <div runat="server" class="relationArea">
                    <div runat="server" id="RelationContainer">
                        <div class="relationContainer" onmouseover="PageContainer_CheckDropState('<%#RelationContainer.ClientID %>')"
                            onmouseup="AddRelation(this, '<%#Button1.ClientID %>', '<%#NewPageTextBox.ClientID %>', '<%#LoadingContainer.ClientID %>')">
                            <asp:Repeater ID="RelatedRelations" runat="server">
                                <HeaderTemplate>
                                    <div class="relationDescriptionContainer">
                                        <div class="relationDescription">
                                            <%=string.Format(IsLeftRule ? EPiCode.Relations.Core.LanguageHandler.TranslateRuleDescriptionLeft(CurrentRule) : EPiCode.Relations.Core.LanguageHandler.TranslateRuleDescriptionRight(CurrentRule), CurrentPage.PageName)%>
                                        </div>
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <ET:RelationControl runat="server" CurrentRelation='<%#Container.DataItem as EPiCode.Relations.Core.Relation %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                </FooterTemplate>
                            </asp:Repeater>
                            <div runat="server" id="LoadingContainer" class="loaderContainer">
                                <div class="loaderImage">
                                    <img src="/App_Themes/Default/Images/General/AjaxLoader.gif" />
                                </div>
                                <div class="loaderText">
                                    Creating relation...
                                </div>
                            </div>
                            <div class="errorMessageContainer">
                                <div class="EP-validationSummary" runat="server" id="ErrorMessageContainer">
                                    <div class="errorMessage">
                                        <asp:Literal runat="server" ID="ErrorMessage"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div style="display: none;">
                <asp:TextBox runat="server" ID="NewPageTextBox"></asp:TextBox>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<asp:Literal ID="RuleName" runat="server" Visible="false"></asp:Literal>
<asp:Literal ID="RuleDirectionLiteral" runat="server" Visible="false"></asp:Literal>
