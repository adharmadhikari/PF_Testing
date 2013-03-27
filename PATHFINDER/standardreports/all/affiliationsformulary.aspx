<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleSection.master" AutoEventWireup="true" CodeFile="affiliationsformulary.aspx.cs" Inherits="standardreports_all_affiliationsformulary" %>
<%@ Register src="~/standardreports/controls/ParentFormulary.ascx" tagname="ParentFormulary" tagprefix="pinso" %>
<%@ Register src="~/standardreports/controls/ChildFormulary.ascx" tagname="ChildFormulary" tagprefix="pinso" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptContainer" Runat="Server">
    <script type="text/javascript">
        clientManager.add_pageInitialized(pageInitialized);
        clientManager.add_pageUnloaded(pageUnloaded);
        
        function pageInitialized() {
            var gridDrilldown = $find("ctl00_Tile3_ChildGrid_ChildPlanRadGrid").GridWrapper; 

            gridDrilldown.add_dataBound(ChildPlanRadGrid_onDataBound);
        }

        function pageUnloaded() {
            clientManager.remove_pageInitialized(pageInitialized);
            clientManager.remove_pageUnloaded(pageUnloaded);
        }

        function ChildPlanRadGrid_onDataBound(sender, args)
        {
            var data = clientManager.get_SelectionData();
            var radGrid = $find('ctl00_Tile3_ChildGrid_ChildPlanRadGrid');
            var table = radGrid.get_masterTableView();
            var column = table.getColumnByUniqueName("Segment_Name");
            var column_pbmservice = table.getColumnByUniqueName("PBM_Service");
            //Show or hide Segment Name column if PBM selected
            
            if (column)
            {
                if (data["Section_ID"].value != "4")
                    table.hideColumn(column.get_element().cellIndex);
                else
                    table.showColumn(column.get_element().cellIndex);
            }

            var column_partd = table.getColumnByUniqueName("Plan_Medicare_PartD_Lives");
            var column_pharmacy = table.getColumnByUniqueName("Plan_Pharmacy_Lives");

            if (column_partd && column_pharmacy)
            {
                if (data["Section_ID"].value == "17")
                {
                    table.hideColumn(column_pharmacy.get_element().cellIndex);
                    table.showColumn(column_partd.get_element().cellIndex);
                }
                else if(data["Section_ID"].value == "4") 
                {
                    table.showColumn(column_pharmacy.get_element().cellIndex);
                    table.showColumn(column_partd.get_element().cellIndex);
                }
                else
                {
                    table.hideColumn(column_partd.get_element().cellIndex);
                    table.showColumn(column_pharmacy.get_element().cellIndex);
                }
            }
            if (column_pbmservice && data["Section_ID"].value == "4")
                table.showColumn(column_pbmservice.get_element().cellIndex);
            else
                table.hideColumn(column_pbmservice.get_element().cellIndex);
           
        }
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Tile3Title" Runat="Server">
    <asp:Literal runat="server" ID="Literal1" Text='<%$ Resources:Resource, SectionTitle_AffiliationFormularyReport %>' />
</asp:Content>
<asp:Content ID="optionsMenuContent" ContentPlaceHolderID="Tile3Tools" Runat="Server">
    <pinso:TileOptionsMenu runat="server" ID="optionsMenu" UserRole="export"/>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Tile3" Runat="Server">
<div style="overflow:auto; width:100%; height: 100%;"> 
<pinso:ParentFormulary ID="ParentGrid" runat="server" />
<pinso:ChildFormulary ID="ChildGrid" runat="server" />
</div>
</asp:Content>

