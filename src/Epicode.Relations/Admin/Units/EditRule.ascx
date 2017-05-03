<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditRule.ascx.cs" Inherits="EPiCode.Relations.Plugins.Admin.Units.EditRule" EnableViewState="true" %>
<%@ Import Namespace="EPiCode.Relations.Core" %>
<%@ Register TagPrefix="ux" Namespace="EPiServer.Web.WebControls" Assembly="EPiServer" %>
<%@ Register TagPrefix="ux" Namespace="EPiServer.Web.PropertyControls" Assembly="EPiServer" %>

<div class="row">
    <div class="col-lg-2">
        <asp:Repeater runat="server" ID="RulesRepeater">
            <HeaderTemplate>
                <div class="list-group">
            </HeaderTemplate>
            <ItemTemplate>
                <asp:LinkButton runat="server" CssClass='<%#CurrentRule != null ? CurrentRule.RuleName == (Container.DataItem as Rule).RuleName ? "list-group-item active":"list-group-item " : "" %>' CommandArgument='<%#(Container.DataItem as Rule).RuleName %>' OnCommand="OnCommand"> <%#(Container.DataItem as Rule).RuleName %> </asp:LinkButton>
            </ItemTemplate>
            <FooterTemplate></div></FooterTemplate>
        </asp:Repeater>
    </div>
    <div class="col-lg-10">
        <fieldset>
            <div class="row">
                <div class="col-lg-12">
                    <div class="row">
                        <div class="col-lg-8">

                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    Rule settings
                                </div>
                                <div class="panel-body">

                                    <div class="row">
                                        <asp:TextBox runat="server" ID="RuleId" Visible="false" CssClass="input-xlarge"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-4 control-label" for="<%=RuleName.ClientID %>">Rule Name</label>
                                        <div class="input-group col-sm-7">

                                            <asp:TextBox runat="server" ID="RuleName" CssClass="form-control"></asp:TextBox>
                                            <div class="input-group-addon">
                                                (<%=(CurrentRule != null && !string.IsNullOrEmpty(CurrentRule.RuleName)) ? EPiCode.Relations.Core.RelationEngine.Instance.GetRelationsCount(CurrentRule.RuleName) : 0 %>
                relations)
                                            </div>
                                        </div>

                                    </div>


                                    <div class="form-group">
                                        <label class="col-sm-4 control-label" for="<%=AccessLevelDropDownList.ClientID %>">Access level</label>
                                        <div class="input-group col-sm-7">

                                            <asp:DropDownList runat="server" ID="AccessLevelDropDownList" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>

                                </div>

                                <div class="marginBottom light" runat="server" visible="false">
                                    Rule ID: 
                <%#(CurrentRule != null) ? CurrentRule.Id.ToString(): "N/A"%>
                                </div>

                            </div>

                        </div>
                        <div class="col-lg-4">
                            <asp:LinkButton ID="Button4" CssClass="btn btn-success pull-right"
                                runat="server" Text="<span class='glyphicon glyphicon-floppy-disk'></span> Save rule" OnClick="AddRule_Click" />
                            <br />
                            <br />
                            <asp:Literal ID="StatusLiteral" runat="server"></asp:Literal>

                        </div>
                    </div>

                </div>
            </div>

            <div class="row">

                <div class="col-lg-6">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            Left relation
                            <a class="btn btn-primary btn-xs pull-right" data-toggle="modal" data-target="#RelationLeftRight">?</a>
                        </div>
                        <div class="modal fade" id="RelationLeftRight" tabindex="-1" role="dialog" aria-labelledby="RelationLeftRightLabel" aria-hidden="true">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                                        <h4 class="modal-title" id="RelationLeftRightLabel">Creating relations</h4>
                                    </div>
                                    <div class="modal-body">
                                        <strong>What is a rule?</strong><br />
                                        Rules are defining allowed relations between different content types. A rule can be restricted by page type or root page.
                                        
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>

                                    </div>
                                </div>
                            </div>


                        </div>
                        <div class="panel-body">
                            <div class="ruleEditor">

                                <div class="smallPadding">


                                    <div class="form-group">
                                        <asp:Label ID="Label1" AssociatedControlID="RuleTextLeft" CssClass="col-sm-4 control-label" Text="Rule text" runat="server"></asp:Label>
                                        <div class="col-sm-8">
                                            <asp:TextBox runat="server" CssClass="form-control" ID="RuleTextLeft"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label ID="Label2" AssociatedControlID="RuleDescriptionLeft" CssClass="col-sm-4 control-label" Text="Description"
                                            runat="server"></asp:Label>
                                        <div class="col-sm-8">
                                            <asp:TextBox runat="server" ID="RuleDescriptionLeft" CssClass="form-control" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="well">

                                        <div class="form-group">
                                            <asp:Label ID="Label7" AssociatedControlID="RulePageTypesLeftListBox" Text="Page types"
                                                runat="server" CssClass="col-sm-4 control-label"></asp:Label>
                                            <div class="input-group col-sm-7">

                                                <asp:DropDownList runat="server" ID="RulePageTypeLeft" CssClass="form-control" DataTextField="Name" DataValueField="Name">
                                                </asp:DropDownList>
                                                <div class="input-group-addon">
                                                    <asp:LinkButton ID="LinkButton1" CssClass="btn btn-success btn-xs"
                                                        runat="server" Text="Add" OnClick="AddPageTypeLeft_Click" />
                                                </div>

                                            </div>

                                        </div>
                                        <div class="form-group">
                                            <asp:Label ID="Label8" AssociatedControlID="RulePageTypesLeftListBox"
                                                runat="server" CssClass="col-sm-4 control-label"></asp:Label>
                                            <div class="input-group col-sm-7">
                                                <asp:ListBox runat="server" CssClass="form-control" ID="RulePageTypesLeftListBox"></asp:ListBox>
                                                <asp:Literal runat="server" ID="RulePageTypesLeftLiteral" Visible="false"></asp:Literal>
                                                <asp:Button ID="Button2" CssClass="btn btn-danger btn-xs" runat="server" Text="Delete" OnClick="RemovePageTypeLeft_Click" />

                                            </div>
                                        </div>
                                    </div>
                                    <div class="well">
                                        <div class="form-group">

                                            <asp:Label ID="Label11" CssClass="col-sm-4 control-label" AssociatedControlID="RuleHierarchyStartPageReferenceLeft"
                                                Text="Root page" runat="server"></asp:Label>
                                            <div class="row">
                                                <div style="margin-top: 5px;">
                                                    <ux:InputPageReference runat="server" CssClass="" ID="RuleHierarchyStartPageReferenceLeft" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">

                                        <asp:Label ID="Label12" AssociatedControlID="SortOrderLeft" CssClass="col-sm-4 control-label" Text="Sort order" runat="server"></asp:Label>
                                        <div class="input-group col-sm-7">
                                            <asp:DropDownList runat="server" ID="SortOrderLeft" CssClass="form-control" />
                                        </div>

                                    </div>
                                    <div class="form-group">

                                        <asp:Label CssClass="col-sm-4 control-label" Text="Visibility" AssociatedControlID="VisibleLeft"></asp:Label>

                                        <div class="col-sm-offset-4 col-sm-9">
                                            <div class="checkbox">
                                                <label>
                                                    <asp:CheckBox ID="VisibleLeft" runat="server" Text="Visible in Edit Mode?" />
                                                </label>
                                            </div>
                                        </div>

                                    </div>

                                </div>
                            </div>


                        </div>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            Right relation
                        </div>
                        <div class="panel-body">

                            <div class="ruleEditor">

                                <div class="smallPadding">


                                    <div class="form-group">
                                        <asp:Label ID="Label4" AssociatedControlID="RuleTextRight" CssClass="col-sm-4 control-label" Text="Rule text" runat="server"></asp:Label>
                                        <div class="col-sm-8">
                                            <asp:TextBox runat="server" CssClass="form-control" ID="RuleTextRight"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label ID="Label6" AssociatedControlID="RuleDescriptionRight" CssClass="col-sm-4 control-label" Text="Description"
                                            runat="server"></asp:Label>
                                        <div class="col-sm-8">
                                            <asp:TextBox runat="server" ID="RuleDescriptionRight" CssClass="form-control" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="well">
                                        <div class="form-group">
                                            <asp:Label ID="Label5" AssociatedControlID="RulePageTypesRightListBox" Text="Page types"
                                                runat="server" CssClass="col-sm-4 control-label"></asp:Label>
                                            <div class="input-group col-sm-7">

                                                <asp:DropDownList runat="server" ID="RulePageTypeRight" CssClass="form-control" DataTextField="Name" DataValueField="Name">
                                                </asp:DropDownList>
                                                <div class="input-group-addon">
                                                    <asp:LinkButton ID="Button1" CssClass="btn btn-success btn-xs"
                                                        runat="server" Text="Add" OnClick="AddPageTypeRight_Click" />
                                                </div>

                                            </div>

                                        </div>
                                        <div class="form-group">
                                            <asp:Label ID="Label10" AssociatedControlID="RulePageTypesRightListBox"
                                                runat="server" CssClass="col-sm-4 control-label"></asp:Label>
                                            <div class="input-group col-sm-7">
                                                <asp:ListBox runat="server" CssClass="form-control" ID="RulePageTypesRightListBox"></asp:ListBox>
                                                <asp:Literal runat="server" ID="RulePageTypesRightLiteral" Visible="false"></asp:Literal>
                                                <asp:Button ID="Button5" CssClass="btn btn-danger btn-xs" runat="server" Text="Delete" OnClick="RemovePageTypeRight_Click" />

                                            </div>
                                        </div>
                                    </div>
                                    <div class="well">
                                        <div class="form-group">

                                            <asp:Label ID="Label3" CssClass="col-sm-4 control-label" AssociatedControlID="RuleHierarchyStartPageReferenceRight"
                                                Text="Root page" runat="server"></asp:Label>
                                            <div class="row">
                                                <div style="margin-top: 5px;">
                                                    <ux:InputPageReference runat="server" CssClass="" ID="RuleHierarchyStartPageReferenceRight" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">

                                        <asp:Label ID="Label9" AssociatedControlID="SortOrderRight" CssClass="col-sm-4 control-label" Text="Sort order" runat="server"></asp:Label>
                                        <div class="input-group col-sm-7">
                                            <asp:DropDownList runat="server" ID="SortOrderRight" CssClass="form-control" />
                                        </div>

                                    </div>
                                    <div class="form-group">

                                        <asp:Label CssClass="col-sm-4 control-label" Text="Visibility" AssociatedControlID="VisibleRight"></asp:Label>

                                        <div class="col-sm-offset-4 col-sm-9">
                                            <div class="checkbox">
                                                <label>
                                                    <asp:CheckBox ID="VisibleRight" runat="server" Text="Visible in Edit Mode?" />
                                                </label>
                                            </div>
                                        </div>

                                    </div>

                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </fieldset>

        <div class="col-lg-12">

            <asp:LinkButton ID="LinkButton3" CssClass="btn btn-success pull-right"
                runat="server" Text="<span class='glyphicon glyphicon-floppy-disk'></span> Save rule" OnClick="AddRule_Click" />

            &nbsp;
                            <asp:LinkButton ID="LinkButton2" CssClass="btn btn-danger pull-right"
                                runat="server" Text="<span class='glyphicon glyphicon-wastebasket'></span> Delete rule" OnClientClick="return confirm('WARNING! Are you sure you want to delete rule? Rule will be deleted permanently!');" OnClick="DeleteRule_Click" />

            <br />
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>

        </div>

    </div>
</div>
