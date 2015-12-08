<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Settings.ascx.cs" EnableViewState="true"
    Inherits="EPiCode.Relations.Plugins.Admin.Units.Settings" %>
Provider:
<asp:TextBox ID="ProviderTextBox" runat="server"></asp:TextBox>
<asp:Button Text="Save" OnClick="SaveProvider_Click" runat="server" />
<br />
Default: EPiCode.Relations.Core.Providers.DynamicDataStoreProvider.DDSRuleProvider
<br />
