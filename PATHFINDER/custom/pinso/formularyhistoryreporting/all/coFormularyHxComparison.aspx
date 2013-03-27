<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleSection.master" AutoEventWireup="true" CodeFile="coFormularyHxComparison.aspx.cs" Inherits="formularyhistoryreporting_all_coFormularyHxComparison" EnableViewState="true"  %>
<%@ Register src="~/custom/pinso/formularyhistoryreporting/controls/fhrScript.ascx" TagName="fhrScript" TagPrefix="pinso" %>
<%@ Register src="~/custom/pinso/formularyhistoryreporting/controls/formularycomparison.ascx" tagname="fhrComparison" tagprefix="pinso" %>



<asp:Content ID="Content1" ContentPlaceHolderID="scriptContainer" Runat="Server">    
    <pinso:fhrScript ID="fhrScript1"  runat="server" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Tile3Title" runat="Server">
    <asp:Literal runat="server" ID="Literal1" Text="Formulary Hx Comparison Report" />
</asp:Content>

<asp:Content ID="optionsMenuContent" ContentPlaceHolderID="Tile3Tools" Runat="Server" >
    <pinso:TileOptionsMenu runat="server" ID="optionsMenu" UserRole="sr_fdx" ExportConfirm="true" />   
</asp:Content>

<asp:Content runat="server" ID="tile3" ContentPlaceHolderID="Tile3">
    <div id ="fhrGridContainer" class="fhrGridContainer" >
        <%--<pinso:fhrComparison ID="fhrComparison" runat="server" ContainerID="divTile3Container"/> --%>    
    </div>   
    <pinso:ThinGrid ID="ThinGrid"  runat="server" AutoLoad="true" StaticHeader="false" Target="fhrGridContainer" 
        Url="FormularyComparison.aspx" LoadSelector=".grid" RequestPageCount="true" EnablePaging="true" 
        pageContainer="#divTile3Container .pagination" pageSize="30" pageSelector=".gridCount" AutoUpdate="false" 
        getSelectedData="true"/>
</asp:Content>

