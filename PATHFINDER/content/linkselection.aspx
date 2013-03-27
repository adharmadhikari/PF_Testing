<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Popup.master" AutoEventWireup="true" CodeFile="linkselection.aspx.cs" Inherits="content_linkselection" %>

<asp:Content ID="Content1" ContentPlaceHolderID="scriptContainer" Runat="Server">
</asp:Content>

<asp:Content ContentPlaceHolderID="title" runat="server">
    <asp:Literal runat="server" ID="titleLabel" Text='<%$ Resources:Resource, PopupTitle_Website_Selection %>' />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="main" Runat="Server">
    <div style="padding:3px;">
        <asp:GridView runat="server" ID="gridLinks" ShowHeader="false" AutoGenerateColumns="false" GridLines="None" BorderStyle="None" Width="100%">
            <Columns>
                <asp:HyperLinkField DataTextField="url" DataTextFormatString="{0}" DataNavigateUrlFields="url" DataNavigateUrlFormatString="http://{0}" Target="_blank" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>

