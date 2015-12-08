<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RelatedPagesTwoHop.ascx.cs" Inherits="EPiCode.Relations.EPiCode.Relations.DynamicContent.RelatedPagesTwoHop" %>
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