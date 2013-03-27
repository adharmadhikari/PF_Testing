<%@ Page Title="" Theme="pathfinder" Language="C#" MasterPageFile="~/MasterPages/Modal.master" AutoEventWireup="true" CodeFile="Favorites.aspx.cs" Inherits="usercontent_Favorites" %>
<%@ Register src="../Controls/favoritesList.ascx" tagname="favoritesList" tagprefix="pinso" %>
<%@ OutputCache NoStore="true" Duration="1" VaryByParam="None" %>

<asp:Content ContentPlaceHolderID="title" ID="titleContent" runat="server">
    <asp:Literal runat="server" ID="titleText" Text='<%$ Resources:Resource, DialogTitle_Favorites %>' />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="main" Runat="Server">
    <div style="overflow:scroll;height:450px;width:430px">
        <pinso:favoritesList ID="favoritesList1" runat="server" SelectFavoritesFunctionFormat="window.top.selectFavorite({0}, {1})" />
    </div>
    <asp:HiddenField runat="server" ID="hFavoriteID" />
    

</asp:Content>

