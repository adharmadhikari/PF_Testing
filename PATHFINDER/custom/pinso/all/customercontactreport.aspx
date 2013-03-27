<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleSection.master"
    AutoEventWireup="true" CodeFile="customercontactreport.aspx.cs" Inherits="custom_pinso_all_customercontactreport" %>

<asp:Content ContentPlaceHolderID="scriptContainer" runat="server">

    <script type="text/javascript">
    
        // Opens a new call report
        function OpenReport(id, planID)
        {
            if (!id) id = 0;

            var url = clientManager.getUrl("ccrentry.aspx") + "?id=" + id + "&planid=" + planID;

            $openWindow(url, null, null, 980, 800, "ccr");         
            
        }
        // Opens the already existing call report on selecting a row from the 'Call Summary' screen
        function onCCRRowSelected(sender, args)
        {
            var dataItem = args.get_item().get_dataItem();
            OpenReport(dataItem.Contact_Report_ID, dataItem.Plan_ID);
        }
        
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Tile3Title" runat="Server">
    Customer Contact Reports
</asp:Content>
<asp:Content ID="optionsMenuContent" ContentPlaceHolderID="Tile3Tools" runat="server">
       <div class="addreport"><a href='javascript:OpenReport()'>Add Report</a></div>
       <pinso:TileOptionsMenu runat="server" ID="optionsMenu" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Tile3" runat="Server">
    <br />
    <table>
        <tr>
            <td>
                <asp:Label ID="AccountMgr_ID" runat="server" Text="Account Manager"></asp:Label>
            </td>
            <td align="right">
                <asp:Label ID="lblAcctMgr" runat="server" Font-Bold="True"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Territory_ID" runat="server" Text="Territory ID"></asp:Label>
            </td>
            <td align="right">
                <asp:Label ID="lblTerritoryID" runat="server" Font-Bold="True"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <telerik:RadGrid SkinID="radTable" runat="server" ID="gridCustomerContactReports"
        AllowSorting="true" PageSize="50" AllowFilteringByColumn="false" AllowPaging="true"
        EnableEmbeddedSkins="false" Width="100%">
        <MasterTableView AutoGenerateColumns="False" ClientDataKeyNames="Contact_Report_ID,Plan_ID,Plan_Name"
            PageSize="50" Width="100%">
            <Columns>
                <telerik:GridBoundColumn HeaderStyle-Width="10%" HeaderText='Date' DataFormatString="{0:d}"
                    SortExpression="Contact_Date" UniqueName="Contact_Date" DataField="Contact_Date" ItemStyle-CssClass="firstCol" />
                <telerik:GridBoundColumn DataField="Plan_Name" HeaderStyle-Width="18%" HeaderText='Account Name'
                    SortExpression="Plan_Name" UniqueName="Plan_Name" />
                <telerik:GridBoundColumn DataField="Contact_Start_Time" HeaderStyle-Width="11%" HeaderText='Contact Start Time'
                    SortExpression="Contact_Start_Time" UniqueName="Start_Time" DataFormatString="{0:t}"/>
                <telerik:GridBoundColumn DataField="Contact_End_Time" HeaderStyle-Width="10%" HeaderText='Contact End Time'
                    SortExpression="Contact_End_Time" UniqueName="End_Time" DataFormatString="{0:t}"/> 
                <telerik:GridBoundColumn DataField="Drug_Name" HeaderStyle-Width="10%" HeaderText='Products Discussed'
                    SortExpression="Drug_Name" UniqueName="Products_Discussed" />
                <telerik:GridBoundColumn DataField="Contact_Objective_Name" HeaderStyle-Width="11%" HeaderText='Contact Objectives'
                    SortExpression="Contact_Objective_Name" UniqueName="Contact_Objectives" />
                <telerik:GridBoundColumn DataField="Call_Outcome" HeaderStyle-Width="10%" HeaderText='Contact Outcome'
                    SortExpression="Call_Outcome" UniqueName="Call_Outcome" />
                <telerik:GridBoundColumn DataField="Followup_Date" HeaderStyle-Width="10%" HeaderText='Follow Up Date'
                    SortExpression="Followup_Date" DataFormatString="{0:d}" UniqueName="Followup_Date" />
                <telerik:GridBoundColumn DataField="Followup_Notes" HeaderStyle-Width="10%" HeaderText='Follow Up Notes'
                    SortExpression="Followup_Notes" UniqueName="Followup_Notes" />
            </Columns>
            <SortExpressions>
                <telerik:GridSortExpression FieldName="Contact_Date" SortOrder="Descending" />
            </SortExpressions>
            <PagerStyle Visible="false" />
        </MasterTableView>
        
        <ClientSettings ClientEvents-OnRowSelected="onCCRRowSelected">
            <DataBinding Location="~/custom/pinso/services/pathfinderdataservice.svc" DataService-TableName="ContactReportFinalSet"
                SelectCountMethod="GetCustomerContactReportCount" />
            <Scrolling AllowScroll="true" UseStaticHeaders="true" />
            <Selecting AllowRowSelect="true" />
        </ClientSettings>
    </telerik:RadGrid>
    <pinso:RadGridWrapper runat="server" ID="gridWrapper" AutoUpdate="true" RequiresFilter="true" ClientTypeName="Pathfinder.UI.CustomerContactReportGridWrapper"
        MergeRows="false" Target="gridCustomerContactReports" PagingSelector="#divTile3Container .pagination" />
</asp:Content>
