<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageListRelationsBlockControl.ascx.cs" Inherits="EPiCode.Relations.Views.Blocks.PageListRelationsBlockControl" %>
<%@ Register Namespace="EPiServer.Web.WebControls" TagPrefix="EPiServer" %>


<episerver:Property ID="Heading" PropertyName="Heading" CustomTagName="h2" runat="server"></episerver:Property>

<asp:Repeater runat="server" ID="RelationsRepeater">
    <HeaderTemplate><ul></HeaderTemplate>
    <ItemTemplate>
        <li>
            <EPiServer:Property runat="server" propertyname="PageLink"></EPiServer:Property>
            </li>

    </ItemTemplate>
    <FooterTemplate></ul></FooterTemplate>
    

</asp:Repeater>

<EPiServer:Property ID="RelatedContentPlaceHolder" runat="server">
 </EPiServer:Property>