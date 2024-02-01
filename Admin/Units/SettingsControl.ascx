<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="SettingsControl.ascx.cs" EnableViewState="true"
    Inherits="EPiCode.Relations.Plugins.Admin.Units.SettingsControl" %>
<%@ Import Namespace="EPiCode.Relations.Core" %>
<div class="alert alert-danger">Careful! Wrong settings could cause the site to stop responding!</div>
<div class="well">

    <div class="buttonRow">
        <span class="epi-buttonDefault"><span class="epi-cmsButton">
            <asp:Button ID="ChangeButton" CssClass="epi-cmsButton-text epi-cmsButton-tools epi-cmsButton-Save"
                runat="server" Text="Change providers" OnClick="SearchProviders_Click" />
        </span></span>
    </div>

    <div id="ProviderDropDowns" Visible="false" runat="server">
            <div class="row">

        <asp:Label AssociatedControlID="RelationProvidersDropDown" CssClass="col-sm-2" Text="Relation Provider:" runat="server"></asp:Label>
        <div class="col-sm-6">
            <asp:DropDownList runat="server" ID="RelationProvidersDropDown" CssClass="form-control"></asp:DropDownList>
        </div>
    </div>

    <div class="row">
        <asp:Label AssociatedControlID="RuleProvidersDropDown" CssClass="col-sm-2" Text="Rule Provider:" runat="server"></asp:Label>
        <div class="col-sm-6">
            <asp:DropDownList runat="server" ID="RuleProvidersDropDown" class="form-control"></asp:DropDownList>
        </div>
    </div>
    
    </div>
    
    
        <div id="ProviderOverview" Visible="true" runat="server">
            <br/>
            <div class="row">
                 <div class="col-sm-2">
        <b>Relation Provider:</b></div>
        <div class="col-sm-6">
            <%#Settings.GetSettingValue("DefaultRelationProviderString") %>
        </div>
    </div>

    <div class="row">
        <div class="col-sm-2">
        <b>Rule Provider:</b>
            </div>
        <div class="col-sm-6">
            <%#Settings.GetSettingValue("DefaultRelationProviderString") %>
        </div>
    </div>
    
    </div>


    <br />
    <br />
    <div class="buttonRow">
        <span class="epi-buttonDefault"><span class="epi-cmsButton">
            <asp:Button ID="SaveButton" Visible="false" CssClass="epi-cmsButton-text epi-cmsButton-tools epi-cmsButton-Save"
                runat="server" Text="Save" OnClick="SaveSettings_Click" />
        </span></span>
    </div>

</div>
<br />
Default: EPiCode.Relations.Core.RelationProviders.DynamicDataStoreProvider.DDSRuleProvider
<br />
