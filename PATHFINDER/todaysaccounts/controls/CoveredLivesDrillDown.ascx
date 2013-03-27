<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CoveredLivesDrillDown.ascx.cs" Inherits="todaysaccounts_controls_CoveredLivesDrillDown" %>
 <div ID="CoveredLivesDrilldownTitle" class="areaHeader">Commercial</div>
 <div ID="CoveredLivesNoRecMsg" visible="true"></div>
 
                <telerik:RadComboBox ID="rdcmbCLTheraClass" runat="server" EnableEmbeddedSkins="false" Width="55%" DropDownWidth="300px" MaxHeight="250px" Skin="pathfinder" OnClientSelectedIndexChanged="rdcmbCLTheraClass_SelectedIndexChanged">
                </telerik:RadComboBox>
                
                <telerik:RadComboBox ID="rdcmbCLDrugs" runat="server" EnableEmbeddedSkins="false" SkinID="planInfoCombo" Width="35%" Skin="pathfinder" OnClientDropDownClosed="rdcmbCLDrugs_DropDownClosed" Height="160px">
                </telerik:RadComboBox>
                
<telerik:RadGrid SkinID="radTable" runat="server" ID="gridcoveredlivesdrilldown" AllowSorting="true" AutoGenerateColumns="false"
     AllowPaging="false" AllowFilteringByColumn="false" EnableEmbeddedSkins="false" PageSize="10" Width="100%" 
     style="visibility: hidden;">
        <MasterTableView PageSize="10" Width="100%" AutoGenerateColumns="false" ClientDataKeyNames="Plan_ID,Drug_ID">
            <Columns>
                <telerik:GridBoundColumn DataField="Drug_Name" HeaderStyle-Width="23%" HeaderText='Drug Name' SortExpression="Drug_Name" UniqueName="Drug_Name" HeaderStyle-HorizontalAlign="Left" /> 
                <telerik:GridBoundColumn DataField="Tier_Name" HeaderStyle-Width="10%" HeaderText='Tier' SortExpression="Tier_Name" UniqueName="Tier_Name" HeaderStyle-HorizontalAlign="right" /> 
                <telerik:GridBoundColumn UniqueName="Formulary_Status_Abbr" DataFormatString='{0}' DataField ="Formulary_Status_Abbr" HeaderText="Status" HeaderStyle-Width="13%"></telerik:GridBoundColumn>               
                <telerik:GridHyperLinkColumn DataNavigateUrlFormatString='javascript:OpenPAForm({0},{1},{2});' DataNavigateUrlFields="Plan_ID,Segment_ID,Drug_ID" UniqueName="PA_Restrictions" DataTextFormatString='{0}' DataTextField="PA_Restrictions" HeaderText="PA" HeaderStyle-Width="7%" ></telerik:GridHyperLinkColumn> 
                
              <%--  <telerik:GridBoundColumn UniqueName="QL" DataField ="QL_Restrictions" 
                    HeaderText="QL" HeaderStyle-Width="7%"></telerik:GridBoundColumn>--%>
                <telerik:GridHyperLinkColumn DataNavigateUrlFormatString='javascript:OpenRestrictionCriteria({0},{1},{2},{3},{4},"QL");' 
                    DataNavigateUrlFields="Plan_ID,Drug_ID,Formulary_ID,Segment_ID,Product_ID" 
                    UniqueName="QL_RestrictionCriteria" DataTextFormatString='{0}' DataTextField ="QL_Restrictions" 
                    HeaderText="QL" HeaderStyle-Width="7%"></telerik:GridHyperLinkColumn> 
                <%-- QL & ST Notes are not being used --%>  
                <%--<telerik:GridBoundColumn UniqueName="QL_Restrictions" HeaderText="QL" HeaderStyle-Width="7%" ></telerik:GridBoundColumn>  --%>             
                <%--<telerik:GridHyperLinkColumn DataNavigateUrlFormatString='javascript:OpenNotesViewer({0},{1},{2},{3},"ST",null,null,200,200);' DataNavigateUrlFields="Plan_ID,Drug_ID,Formulary_ID,Segment_ID" UniqueName="ST_Restrictions" DataTextFormatString='{0}' DataTextField ="ST_Restrictions" HeaderText="ST" HeaderStyle-Width="7%"></telerik:GridHyperLinkColumn>--%>
                <telerik:GridBoundColumn UniqueName="ST_Restrictions" DataField ="ST_Restrictions" HeaderText="ST" HeaderStyle-Width="7%"></telerik:GridBoundColumn>
                <telerik:GridHyperLinkColumn DataNavigateUrlFormatString='javascript:OpenMedPolicyForm({0},{1},{2});' DataNavigateUrlFields="Plan_ID,Segment_ID,Drug_ID" UniqueName="Med_Policy" DataTextFormatString='{0}' DataTextField="Med_Policy" HeaderText="Med Policy" HeaderStyle-Width="16%" ></telerik:GridHyperLinkColumn>                  
                <telerik:GridBoundColumn DataField="Co_Pay" DataFormatString="{0:n0}" HeaderStyle-Width="12%" HeaderText='Copay' SortExpression="Co_Pay" UniqueName="Co_Pay" ItemStyle-CssClass="alignRight" /> 
                <telerik:GridHyperLinkColumn DataNavigateUrlFormatString='javascript:OpenNotesViewer({0},{1},{2},{3},"comments",null,null,275,200);' DataNavigateUrlFields="Plan_ID,Drug_ID,Formulary_ID,Segment_ID" UniqueName="Comments" DataTextFormatString='{0}' DataTextField ="Comments" ItemStyle-CssClass="commentsCell" HeaderText="&nbsp;" HeaderStyle-Width="5%"></telerik:GridHyperLinkColumn>
                
            </Columns>         
            <PagerStyle Visible="false" />     
            <SortExpressions>
                <telerik:GridSortExpression FieldName="Drug_Name" SortOrder="Ascending" />
            </SortExpressions>
        </MasterTableView>
                
        <ClientSettings>
            <DataBinding Location="~/todaysaccounts/services/pathfinderdataservice.svc"  DataService-TableName="CoveredLivesDrilldownSet" />               
            <Scrolling AllowScroll="True" UseStaticHeaders="True"  />            
            <Selecting AllowRowSelect="false" /> 
        </ClientSettings>        
   
</telerik:RadGrid>
<%--<div ID="CoveredLivesDrilldownFooter" class="clDate"></div>--%>
<pinso:RadGridWrapper runat="server" ID="radGridWrapper" Target="gridcoveredlivesdrilldown" AutoUpdate="true" RequiresFilter="true" DrillDownLevel="1" />
