<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/PartialPage.master" AutoEventWireup="true" CodeFile="Filters.aspx.cs" Inherits="custom_pinso_customercontactreports_all_Filters" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register src="~/controls/FiltersContainer.ascx" tagname="FiltersContainer" tagprefix="pinso" %>
<%@ Register src="../controls/FiltersContainerScript.ascx"tagname="FiltersContainerScript" tagprefix="pinso" %>

<asp:Content runat="server" ID="scriptSection" ContentPlaceHolderID="scriptContainer">
    <pinso:FiltersContainerScript runat="server" ID="filtersContainerScript" />
        
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="partialPage" Runat="Server">
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

