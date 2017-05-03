<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RelatedPages.ascx.cs" Inherits="EPiCode.Relations.EPiCode.Relations.DynamicContent.RelatedPages" %>
<asp:Repeater runat="server" ID="RelatedPagesRepeater">
    <HeaderTemplate>
        <ul>
    </HeaderTemplate>
    <ItemTemplate>
        <li>
            <EPiServer:Property runat="server" PropertyName="PageLink" />
        </li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>