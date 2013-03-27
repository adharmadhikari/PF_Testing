<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Pyramid.master" Theme="pathfinder" AutoEventWireup="true" CodeFile="coveredlives.aspx.cs" Inherits="standardreports_all_coveredlives" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Namespace="Pathfinder" TagPrefix="pinso" %>
<%@ Register Src="~/standardreports/controls/CoveredLivesChart.ascx" TagName="CoveredLivesChart" TagPrefix="pinso" %>

<asp:Content ContentPlaceHolderID="scriptContainer" runat="server">
    <script type="text/javascript">        

        clientManager.add_pageInitialized(pageInitialized);
        clientManager.add_pageUnloaded(pageUnloaded);

        function pageInitialized()
        {
            $("#divTile3 object").width("98%").height("98%");
            //            $("#divTile3 img").width(100).css("height","");
            scaleChart();

            var data = clientManager.get_SelectionData(1);
            if (data && data["MarketSegmentId"])
            {
                setSegmentColor(data["MarketSegmentId"]);
                set_TotalLives(data["MarketSegmentId"]);
            }

            var gw = $find("ctl00_Tile5_gridCL$GridWrapper");
            gw.add_dataBound(coveredLives_dataBound);
            $(gw.get_element()).css("visibility", "hidden");
        }

        function pageUnloaded()
        {
            clientManager.remove_pageInitialized(pageInitialized);
            clientManager.remove_pageUnloaded(pageUnloaded);
        }

        function set_TotalLives(segment, isLis)
        {
            var ftr = $get("ctl00_Tile5_gridCL").control.get_masterTableViewFooter();
            $(ftr.get_element().rows[0].cells[1]).attr("class", "alignRight").text($("#ms" + segment + "_" + isLis).attr("_lives"));
        }

        function setSegmentColor(segment, isLis)
        {
            $("#ctl00_Tile4_GridView1 tr").attr("className", "");
            var clr = $("#ms" + segment + "_" + isLis).attr("className", "rgSelectedRow").attr("_color");
            $("#tile5 .tileContainerHeader").css("background-color", clr);
        }
        
        function coveredLives_dataBound(sender, args)
        {
            $("#drilldownTitle").hide();
            $(sender.get_element()).css("visibility", "visible");
        }

        function showCoveredLivesDetails(segment_id, isLis)
        {

            if (segment_id < 100)
                clientManager.set_SelectionData({ MarketSegmentId: segment_id, Is_LIS: isLis }, 1);
            else
            {
                clientManager.clearSelectionData(true);
                var gw = $find("ctl00_Tile5_gridCL$GridWrapper");
                gw.clearGrid();
                gw.get_masterTableView().get_filterExpressions().clear();
                coveredLives_dataBound(gw, {});
            }

            set_TotalLives(segment_id, isLis);

            setSegmentColor(segment_id, isLis);
        }              
    </script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Tile3Title" runat="Server">
    <asp:Literal runat="server" ID="Literal1" Text='<%$ Resources:Resource, SectionTitle_CoveredLives %>' />
</asp:Content>
<asp:Content ID="optionsMenuContent" ContentPlaceHolderID="Tile3Tools" runat="Server">
    <pinso:TileOptionsMenu runat="server" ID="optionsMenu" UserRole="export"/>
</asp:Content>
<asp:Content runat="server" ID="tile3" ContentPlaceHolderID="Tile3">
    <%--<div id="divCoveredLivesChart">--%>
    <pinso:CoveredLivesChart ID="CoveredLivesChart" runat="server" Thumbnail="true" />    
   <%-- </div>--%>
    <%--<pinso:CoveredLivesChart ID="CoveredLivesChart1" runat="server" Thumbnail="true" />--%>    
</asp:Content>

<asp:Content ContentPlaceHolderID="tile4" runat="server">
    <div class="dashboardTable">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="dsLives"
            OnRowDataBound="GridView1_RowDataBound" Width="100%" CssClass="staticTable">
            <HeaderStyle HorizontalAlign="Center" BackColor="#2854a5" ForeColor="#ffffff" />
            <Columns>
                <asp:BoundField DataField="MarketSegmentId" HeaderText="Market Segment Id" Visible="false" />
                <asp:BoundField DataField="Is_LIS" HeaderText="Is LIS" Visible="false" />
                <asp:BoundField DataField="MarketSegmentName" HeaderText="Market Segment Name" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="TotalLives" DataFormatString="{0:n0}" HeaderText="Lives"
                    ItemStyle-CssClass="alignRight totalLives" />
            </Columns>
        </asp:GridView>
        <asp:EntityDataSource ID="dsLives" runat="server" ConnectionString="name=PathfinderEntities"
            DefaultContainerName="PathfinderEntities" EntitySetName="CoveredLivesSummarySet"
            Select="it.[sortorder],it.[MarketSegmentId], it.[MarketSegmentName], it.[TotalLives],it.[Is_LIS]"
            Where="it.MarketSegmentId <> 101"
            OrderBy="it.[sortorder]">
        </asp:EntityDataSource>
        <%--<div class="coveredLivesInfo">
            *Lives are included with Commercial Payers
        </div>--%>
    </div>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Tile5">  
<div id="drilldownTitle"><asp:Literal runat="server" ID="Literal2" Text='<%$ Resources:Resource, Message_Tier_Coverage_DrillDown %>' /></div>
<div id="tile5CLDataDrillDown">       
    <telerik:RadGrid  SkinID="radTable" runat="server" ID="gridCL" AllowSorting="true" PageSize="50" AllowPaging="true" AllowFilteringByColumn="false" EnableEmbeddedSkins="false">
        <MasterTableView AutoGenerateColumns="False" ClientDataKeyNames="PlanID" PageSize="50" ShowFooter="true" >
            <Columns>
                <telerik:GridBoundColumn DataField="Plan_Name" HeaderStyle-Width="75%" HeaderText='<%$ Resources:Resource, Label_Plan_Name %>'
                    SortExpression="Plan_Name" UniqueName="Plan_Name" DataType="System.String" ItemStyle-CssClass="firstCol"
                    FooterText="Total Lives: " FooterStyle-Font-Bold="true">
                    <FilterTemplate>
                    </FilterTemplate>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Total_Covered" DataFormatString="{0:n0}" ItemStyle-CssClass="alignRight totalCovered"
                    HeaderStyle-Width="25%" DataType="System.Int32" HeaderText='<%$ Resources:Resource, Label_Lives %>'
                    SortExpression="Total_Covered" UniqueName="Total_Covered" FooterStyle-Font-Bold="true">
                    <FilterTemplate>
                    </FilterTemplate>
                </telerik:GridBoundColumn>
            </Columns>
            <SortExpressions>
                <telerik:GridSortExpression FieldName="Total_Covered" SortOrder="Descending" />
            </SortExpressions>
            <PagerStyle Visible="false" />
        </MasterTableView>
        <ClientSettings>
            <DataBinding Location="~/standardreports/services/pathfinderdataservice.svc" DataService-TableName="CoveredLivesListViewSet"
                 />
            <Scrolling AllowScroll="True" UseStaticHeaders="True" />
            <Selecting AllowRowSelect="false" />
        </ClientSettings>
    </telerik:RadGrid>
    <pinso:RadGridWrapper runat="server" ID="radGridWrapper" Target="gridCL" PagingSelector="#divTile5Container .pagination" AutoUpdate="true" MergeRows="false" RequiresFilter="true" DrillDownLevel="1" />           
</div>

</asp:Content>

