<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FormularyStatusDataTemplate.ascx.cs" Inherits="standardreports_controls_FormularyStatusTemplateData" %>

<asp:Label runat="server" ID="labelTitle" CssClass="reportDataTitle" />

<telerik:RadGrid SkinID="radTable" runat="server" ID="gridformularystatus" AllowSorting="false" AutoGenerateColumns="false"
     AllowPaging="false" AllowFilteringByColumn="false" EnableEmbeddedSkins="false" MasterTableView-AllowMultiColumnSorting="false" OnItemDataBound="gridformularystatus_ItemDataBound" >
     <MasterTableView AutoGenerateColumns="false" ClientDataKeyNames="Drug_ID,Drug_Name">
         <Columns>
            <telerik:GridBoundColumn DataField="Drug_Name" ItemStyle-Width="8.33%" HeaderText='Drug Name' SortExpression="Drug_Name" UniqueName="Drug_Name" /> 
            <telerik:GridBoundColumn DataField="Section_Name" ItemStyle-Width="10%" HeaderText="Section" SortExpression="Section_Name" UniqueName="Section_Name" Display="False" />
            <%--<telerik:GridBoundColumn DataField="Covered_Lives" ItemStyle-Width="8.33%" HeaderText='Total Lives' SortExpression="Covered_Lives" UniqueName="Covered_Lives" DataFormatString="{0:n0}" ItemStyle-CssClass="alignRight totalLives"/> --%>
            <telerik:GridBoundColumn DataField="Formulary_Lives" ItemStyle-Width="8.33%" HeaderText='Pharmacy Lives' SortExpression="Formulary_Lives" UniqueName="Formulary_Lives" DataFormatString="{0:n0}" ItemStyle-CssClass="alignRight pharmacyLives"/>         
            <%--<telerik:GridHyperLinkColumn DataTextFormatString="{0:n2}%" DataTextField="F1" ItemStyle-Width="9%" HeaderText='<%$ Resources:Resource, Label_Formulary_Status_1 %>' SortExpression="F1" UniqueName="F1" ItemStyle-CssClass="alignRight coveredPA"/> 
            <telerik:GridHyperLinkColumn DataTextFormatString="{0:n2}%" DataTextField="F2" ItemStyle-Width="9%" HeaderText='<%$ Resources:Resource, Label_Formulary_Status_2 %>' SortExpression="F2" UniqueName="F2" ItemStyle-CssClass="alignRight coveredwithPA"/> 
            <telerik:GridHyperLinkColumn DataTextFormatString="{0:n2}%" DataTextField="F3" ItemStyle-Width="9%" HeaderText='<%$ Resources:Resource, Label_Formulary_Status_3 %>' SortExpression="F3" UniqueName="F3" ItemStyle-CssClass="alignRight NotCovered" />                 
           --%> <%--<telerik:GridHyperLinkColumn DataTextFormatString="{0:n2}%" DataTextField="F4" DataNavigateUrlFormatString='javascript:gridFSDrilldown_setfilter({0}, "{1}", 4, f4Text, "{2}")' DataNavigateUrlFields="Drug_ID, Drug_Name, Geography_ID" ItemStyle-Width="9%" HeaderText='<%$ Resources:Resource, Label_Formulary_Status_2 %>' SortExpression="F4" UniqueName="F4" ItemStyle-CssClass="alignRight NotCovered" />--%>   
           <%-- <telerik:GridHyperLinkColumn DataTextFormatString="{0:n2}%" DataTextField="F5"  ItemStyle-Width="9%" HeaderText='<%$ Resources:Resource, Label_Formulary_Status_5 %>' SortExpression="F5" UniqueName="F5" ItemStyle-CssClass="alignRight NotCovered" />                               
           --%> <%--<telerik:GridBoundColumn DataField="F5" ItemStyle-Width="8.33%" HeaderText='<%$ Resources:Resource, Label_Formulary_Status_5 %>' SortExpression="F5" UniqueName="F5" DataFormatString="{0:n2}%" ItemStyle-CssClass="alignRight NotCovered"/>             --%>
         </Columns>
         <PagerStyle Visible="false" />
        <%-- 
         <SortExpressions>
            <telerik:GridSortExpression FieldName="Drug_Name" SortOrder="Ascending"  />   
            <telerik:GridSortExpression FieldName="Covered_Lives" SortOrder="Ascending"  />   
            <telerik:GridSortExpression FieldName="Pharmacy_Lives" SortOrder="Ascending"  />   
         </SortExpressions> 
        --%>
     </MasterTableView>        
     
    <ClientSettings>    
        <%-- <DataBinding Location="~/standardreports/services/pathfinderdataservice.svc"  DataService-TableName = "FormularyStatusSummarySet"/>
        --%>
        <Scrolling AllowScroll="True" UseStaticHeaders="True" />
    </ClientSettings>
    
</telerik:RadGrid>

 <%--
<pinso:RadGridWrapper ID="radgridwrapper" runat="server" AutoUpdate="true" Target="gridformularystatus" RequiresFilter="true"  />
--%>