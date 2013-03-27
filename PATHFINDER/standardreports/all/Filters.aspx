<%@ Page Language="C#" AutoEventWireup="true" Theme="pathfinder" CodeFile="Filters.aspx.cs" MasterPageFile="~/MasterPages/PartialPage.master" Inherits="standardreports_all_Filters" %>
<%@ Register src="~/controls/FiltersContainer.ascx" tagname="FiltersContainer" tagprefix="pinso" %>
<%@ Register src="~/controls/FiltersContainerScript.ascx" tagname="FiltersContainerScript" tagprefix="pinso" %>

<asp:Content runat="server" ID="scriptSection" ContentPlaceHolderID="scriptContainer">

    <pinso:FiltersContainerScript runat="server" ID="filtersContainerScript" />
        
</asp:Content>

<asp:Content runat="server" ID="partialPage1" ContentPlaceHolderID="partialPage">
    <div class="tileContainerHeader">
                    <div class="title"><asp:Literal runat="server" ID="literalTitle" Text='<%$ Resources:Resource, Label_Report_Filters %>' />
                    </div>
                    <div class="tools"><div id="warnings" onclick="$alert()"></div>
                    </div>
                    <div class="clearAll">
                    </div>
                </div>
    <div id="filterControls">
        <pinso:FiltersContainer ID="filtersContainer" runat="server" />        
    </div>
    <div class="modalFormButtons" id="filterFormButtons">
        <pinso:CustomButtonNonServer runat="server" id="requestReportButton" Text="Submit" />
        <pinso:CustomButtonNonServer runat="server" id="clearFiltersButton" Text="Reset" />                
    </div>
    
    

</asp:Content> 