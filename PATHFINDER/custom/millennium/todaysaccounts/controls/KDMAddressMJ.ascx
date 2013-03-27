<%@ Control Language="C#" AutoEventWireup="true" CodeFile="KDMAddressMJ.ascx.cs" Inherits="custom_millennium_todaysaccounts_controls_KDMAddressMJ" %>
<div id="gap" style="width: 100%; height:20px;border-bottom:1px solid #CCC;"></div>
 <div class="tools">
        <span id="AddAddressButton"><a id="AddKDM" href="javascript:OpenKDMAddress('AddKDM','','MJ');">Add </a></span>   
        <span id="EditDeleteAddressButtons"> | <a id="EditKDM" href="javascript:OpenKDMAddress('EditKDM','','MJ');">Edit </a> |
        <a id="DeleteKDM" href="javascript:OpenKDMAddress('DelKDM','','MJ');">Delete </a></span>        
     </div> 
     <div class="clearAll"></div>
<div id="kcView">
     <telerik:RadGrid SkinID="radTable" runat="server" ID="gridKDMAddress" AllowSorting="true" AllowPaging="false" AllowFilteringByColumn="false" EnableEmbeddedSkins="false" PageSize="25">        
        <MasterTableView autogeneratecolumns="false" ClientDataKeyNames="KDM_ID, ID" AllowPaging="false" Width="100%" PageSize="25">
           <Columns>
            <telerik:GridBoundColumn DataField="KDM_Address1" HeaderText="Address 1" HeaderStyle-Width="150px" SortExpression="KDM_Address1" UniqueName="KDM_Address1" ItemStyle-CssClass="firstCol" />
            <telerik:GridBoundColumn DataField="KDM_Address2" HeaderText="Address 2" HeaderStyle-Width="150px" SortExpression="KDM_Address2" UniqueName="KDM_Address2" />
            <telerik:GridBoundColumn DataField="KDM_City" HeaderText='<%$ Resources:Resource, Label_City %>' HeaderStyle-Width="150px" SortExpression="KDM_City" UniqueName="KDM_City" />
            <telerik:GridBoundColumn DataField="KDM_State" HeaderText='<%$ Resources:Resource, Label_State %>' HeaderStyle-Width="200px" SortExpression="KDM_State" UniqueName="KDM_State" />
            <telerik:GridBoundColumn DataField="KDM_Zip" HeaderText='<%$ Resources:Resource, Label_ZIP %>' HeaderStyle-Width="100px" SortExpression="KDM_Zip" UniqueName="KDM_Zip" />
            <telerik:GridBoundColumn DataField="KDM_Zip_4" HeaderText="Zip + 4" HeaderStyle-Width="100px" SortExpression="KDM_Zip_4" UniqueName="KDM_Zip_4" />
            <telerik:GridCheckBoxColumn DataField="Is_Primary_Add" HeaderText='Primary Address' HeaderStyle-Width="100px" SortExpression="Is_Primary_Add" UniqueName="Is_Primary_Add" ></telerik:GridCheckBoxColumn>
            </Columns>                
        </MasterTableView>
        <%--<PagerStyle Wrap="true"  Mode="NumericPages" /> --%>
        <ClientSettings  ClientEvents-OnRowSelecting="onKDMAddressGridRowClick">
            <DataBinding Location="~/custom/millennium/todaysaccounts/services/MillenniumDataService.svc" DataService-TableName="KDMAddressSet"  />
            <Scrolling AllowScroll="false" UseStaticHeaders="true" /> 
            <Selecting AllowRowSelect="true" />
        </ClientSettings>   
     
    </telerik:RadGrid>    
     <pinso:RadGridWrapper runat="server" ID="gridWrapper" Target="gridKDMAddress" OnClientDataBinding="RefreshKDMAddress"/>
</div> 