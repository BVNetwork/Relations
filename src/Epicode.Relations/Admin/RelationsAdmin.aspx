<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="true" CodeBehind="RelationsAdmin.aspx.cs" Inherits="EPiCode.Relations.Admin.RelationsAdmin"  EnableEventValidation="false"  %>

<%@ Import Namespace="EPiServer.Framework.Web.Resources" %>
<%@ Import Namespace="EPiServer.Framework.Web.Mvc.Html" %>
<%@ Register TagPrefix="sc" Assembly="EPiServer.Shell" Namespace="EPiServer.Shell.Web.UI.WebControls" %>
<%@ Register TagPrefix="Relations" TagName="EditRule" Src="Units/EditRule.ascx" %>
<%@ Register TagPrefix="Relations" TagName="RelationValidator" Src="Units/RelationValidator.ascx" %>
<%@ Register TagPrefix="Relations" TagName="ImportExport" Src="Units/ImportExport.ascx" %>
<%@ Register TagPrefix="Relations" TagName="RuleOverview" Src="Units/RuleOverview.ascx" %>
<%@ Register TagPrefix="Relations" TagName="Settings" Src="Units/SettingsControl.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" style="margin: 0; padding: 0;">
<head id="Head1">
    <title>
        <EPiServer:Translate Text="/relations/admin/relationsadmin" runat="server" />
    </title>

    <asp:PlaceHolder ID="shellScripts" runat="server">
        <%=Page.ClientResources("Dojo", new[] { ClientResourceType.Style })%>
        <%=Page.ClientResources("ShellCore")%>
        <%=Page.ClientResources("ShellWidgets")%>
        <%=Page.ClientResources("Navigation")%>
    </asp:PlaceHolder>

    <link rel="Stylesheet" type="text/css" href="/Modules/EPiCode.Relations/Static/css/bootstrap.css">
    <link rel="Stylesheet" type="text/css" href="/Modules/EPiCode.Relations/Static/css/bootstrap-theme.css">
    <link rel="Stylesheet" type="text/css" href="/Modules/EPiCode.Relations/Static/Styles/Admin.css?v=2">
</head>
<body style="background-color: #ddd;">

    <form id="Form1" runat="server" style="background-color: #ddd; " class="form-horizontal" role="form">
       
        <sc:PlatformNavigationMenu ID="ShellMenu2" runat="server" Area="Relations" />
        
       
        <div class="container" style="background-color: #fff;">
            <br />
            <div class="row">
                <div class="col-lg-12">
                    <br />
                    <div class="darkarea">
                        <asp:LinkButton runat="server" ID="CreateNewRuleButton" OnClick="CreateNewRuleButton_OnClick" CssClass="btn btn-default btn-xs pull-right" Text="<span class='glyphicon glyphicon-plus-sign'></span> Create new rule"></asp:LinkButton>
                        <%-- <h1 class="title"><EPiServer:Translate runat="server" Text="/relations/admin/relations"/></h1>--%>
                    </div>
                </div>
                <div class="col-lg-12">
                    <asp:TextBox runat="server" Visible="False" Text="overview" ID="CurrentTabTextBox"></asp:TextBox>
                    <br />
                    <asp:PlaceHolder runat="server" ID="TabsPlaceHolder">
                        <ul class="nav nav-tabs" role="tablist" id="tabs" data-tabs="tabs">
                            <li class="<%=IsActiveTab("overview") %>">
                                <asp:LinkButton runat="server" OnCommand="SetTab_OnCommand" CommandArgument="overview" Text="Overview"></asp:LinkButton>
                            </li>
                            <li class="<%=IsActiveTab("edit") %>">
                                <asp:LinkButton runat="server" OnCommand="SetTab_OnCommand" CommandArgument="edit" Text="Rule editor"></asp:LinkButton>
                            </li>
                            <li class="<%=IsActiveTab("validator") %>">
                                <asp:LinkButton runat="server" OnCommand="SetTab_OnCommand" CommandArgument="validator" Text="Validator"></asp:LinkButton>
                            </li>
                            <li class="<%=IsActiveTab("import") %>">
                                <asp:LinkButton runat="server" OnCommand="SetTab_OnCommand" CommandArgument="import" Text="Import and export"></asp:LinkButton>
                            </li>
                            <li class="<%=IsActiveTab("settings") %>">
                                <asp:LinkButton runat="server" OnCommand="SetTab_OnCommand" CommandArgument="settings" Text="Settings"></asp:LinkButton>
                            </li>
                        </ul>
                    </asp:PlaceHolder>
                    <br />
                    <div class="tab-content">
                        <div class="tab-pane <%=IsActiveTab("overview") %>" id="overview">
                            <Relations:RuleOverview runat="server"></Relations:RuleOverview>
                        </div>
                        <div class="tab-pane <%=IsActiveTab("edit") %>" id="edit">
                            <Relations:EditRule runat="server" ID="EditRuleControl"></Relations:EditRule>
                        </div>
                        <div class="tab-pane <%=IsActiveTab("validator") %>" id="validator">
                            <Relations:RelationValidator runat="server"></Relations:RelationValidator>
                        </div>
                        <div class="tab-pane <%=IsActiveTab("import") %>" id="import">
                            <Relations:ImportExport runat="server"></Relations:ImportExport>
                        </div>
                        <div class="tab-pane <%=IsActiveTab("settings") %>" id="settings">
                            <Relations:Settings runat="server"></Relations:Settings>
                        </div>
                    </div>
                </div>
                    
                <div class="col-lg-12">
                    <div class="well well-sm">Copyright 2019 - BV Network AS</div>
                </div>
            </div>

            <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js" />
            <script type="text/javascript" src="assets/twitterbootstrap/js/bootstrap-tab.js"></script>
            <script type="text/javascript">
                jQuery(document).ready(function ($) {
                    $('#tabs').tab();
                });


            </script>
            <script type="text/javascript" src="/Modules/EPiCode.Relations/Static/js/bootstrap.js" />

        </div>
    </form>
</body>
</html>


