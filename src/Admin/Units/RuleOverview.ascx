<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RuleOverview.ascx.cs" Inherits="EPiCode.Relations.Admin.Units.RuleOverview" %>
<%@ Import Namespace="EPiCode.Relations.Core" %>
<div class="row">
    <div class="col-lg-12">
                                    <asp:PlaceHolder runat="server" ID="WarningPlaceHolder"></asp:PlaceHolder>
    
    </div></div>
<asp:Repeater runat="server" ID="RulesRepeater">
    <ItemTemplate>
        <div class="col-sm-6">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <div class="panel-title">
                        <strong><%#(Container.DataItem as Rule).RuleName %></strong>
                        <span style="color: #fff;">
                            <asp:LinkButton runat="server" ID="EditRuleBtn" CssClass="btn btn-default pull-right btn-xs" OnCommand="EditRuleBtn_OnClick" CommandArgument="<%#(Container.DataItem as Rule).RuleName %>" Text="<span class='glyphicon glyphicon-pencil'></span> Edit rule"></asp:LinkButton>

                        </span>
                    </div>
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-sm-6">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <strong><%#(Container.DataItem as Rule).RuleTextLeft %></strong>


                                </div>
                                <div class="panel-body">
                                    
                                    <strong>Page types:</strong><br />
                                    <%#GetPageTypeList(Container.DataItem as Rule, true) %><br />
                                    <strong>Root page:</strong><br />
                                    <%#GetPageName((Container.DataItem as Rule).RelationHierarchyStartLeft) %>
                                    <div class="clearfix"></div>
                                    <br />

                                </div>
                            </div>

                        </div>
                            <div class="col-sm-6">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <strong><%#(Container.DataItem as Rule).RuleTextRight %></strong>


                                </div>
                                <div class="panel-body">
                                    <strong>Page types:</strong><br />
                                    <%#GetPageTypeList(Container.DataItem as Rule, false) %><br />
                                    <strong>Root page:</strong><br />
                                    <%#GetPageName((Container.DataItem as Rule).RelationHierarchyStartRight) %>
                                    <div class="clearfix"></div>
                                    <br />
                                  
                                </div>
                            </div>

                        </div>
                    </div>
        </div>
            </div>
        </div>
    </ItemTemplate>
</asp:Repeater>

