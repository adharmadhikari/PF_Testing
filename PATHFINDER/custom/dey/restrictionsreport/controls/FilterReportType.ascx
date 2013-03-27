<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterReportType.ascx.cs" Inherits="restrictionsreport_controls_FilterReportType" %>

<asp:PlaceHolder runat="server" ID="placeholder">
    <div class="filterGeo">
        <asp:Literal runat="server" ID="filterLabel" Text='Report Type' />
    </div>
    <telerik:RadComboBox runat="server" ID="Rank" CssClass="queryExt" EnableEmbeddedSkins="false" Skin="pathfinder"
        MaxHeight="300px" AppendDataBoundItems="true" DataTextField="Rank_Name" DataValueField="Rank_Value">
        <Items>
            <telerik:RadComboBoxItem Text="--All--" Value="" />
            <%--<telerik:RadComboBoxItem Text="Top 10 Plans" Value="10" />
            <telerik:RadComboBoxItem Text="Top 20 Plans" Value="20" />--%>
        </Items>
    </telerik:RadComboBox>

    <div id="filterPlan">
        <div class="searchTextBoxFilter">
            <input type="text" id="Plan_ID" class="textBox" />
        </div>
        <pinso:SearchList ID="searchlist" runat="server" Target="Plan_ID" ClientManagerID="mainSection" OffsetX="-6" 
                                    ContainerID="moduleOptionsContainer" 
                                    ServiceUrl="services/pathfinderservice.svc/PlanInfoListViewSet" 
                                    QueryFormat="$filter=substringof('{0}',Plan_Name)&$top=50&$orderby=Plan_Name" 
                                    QueryValues=""
                                    DataField="Plan_ID" 
                                    TextField="Plan_Name"
                                    MultiSelect="true" 
                                    MultiSelectHeaderText="Selected Accounts"
                                    WaterMarkText="Type to search"/>
    </div>    
</asp:PlaceHolder>
