<%@ Control Language="C#" AutoEventWireup="true" CodeFile="KDMDetailsRTA.ascx.cs" Inherits="custom_millennium_todaysaccounts_controls_KDMDetailsRTA" %>
 <div class="tools">
        <a id="AddKDM" href="javascript:OpenKDM('AddKDM','','RTA');">Add </a>
        <span id="EditDeleteButtons"> | <a id="EditKDM" href="javascript:OpenKDM('EditKDM','','RTA');">Edit </a> |
        <a id="DeleteKDM" href="javascript:OpenKDM('DelKDM','','RTA');">Delete </a></span>
 </div> 
     <div class="clearAll"></div>
<div id="kcView">
     <telerik:RadGrid SkinID="radTable" runat="server" ID="gridKDMDetailsRTA" AllowSorting="true" AllowPaging="false" AllowFilteringByColumn="false" EnableEmbeddedSkins="false" PageSize="200">        
        <MasterTableView autogeneratecolumns="false" ClientDataKeyNames="KDM_ID" AllowPaging="false" Width="100%" PageSize="200">
           <Columns>
            <telerik:GridBoundColumn DataField="KDM_F_Name" HeaderText='<%$ Resources:Resource, label_first_name %>' HeaderStyle-Width="115px" SortExpression="KDM_F_Name" UniqueName="KDM_F_Name" ItemStyle-CssClass="firstCol" />
            <telerik:GridBoundColumn DataField="KDM_L_Name" HeaderText='<%$ Resources:Resource, label_last_name %>' HeaderStyle-Width="115px" SortExpression="KDM_L_Name" UniqueName="KDM_L_Name" />
            <telerik:GridBoundColumn DataField="Titles" HeaderText='<%$ Resources:Resource, label_title %>' HeaderStyle-Width="131px" SortExpression="Titles" UniqueName="Titles" />
            <telerik:GridBoundColumn DataField="Credentials" HeaderText='Credentials' HeaderStyle-Width="135px" SortExpression="Credentials" UniqueName="Credentials" />
            <telerik:GridBoundColumn DataField="Specialty" HeaderText='Specialty' HeaderStyle-Width="135px" SortExpression="Specialty" UniqueName="Specialty" />
            <telerik:GridBoundColumn DataField="JobFunction" HeaderText='Job Function' HeaderStyle-Width="110px" SortExpression="JobFunction" UniqueName="JobFunction" />    
            <telerik:GridBoundColumn DataField="RTA_Affiliation" HeaderText='Affiliation' HeaderStyle-Width="80px" SortExpression="RTA_Affiliation" UniqueName="RTA_Affiliation" />        
            <telerik:GridBoundColumn DataField="KDM_Email" HeaderText='<%$ Resources:Resource, label_email %>' HeaderStyle-Width="115px" SortExpression="KDM_Email" UniqueName="KDM_Email" />
            <telerik:GridBoundColumn DataField="KDM_Phone" HeaderText='<%$ Resources:Resource, label_phone %>' HeaderStyle-Width="90px" SortExpression="KDM_Phone" UniqueName="KDM_Phone" />
            <telerik:GridBoundColumn DataField="KDM_Fax" HeaderText='<%$ Resources:Resource, Label_Fax %>' HeaderStyle-Width="90px" SortExpression="KDM_Fax" UniqueName="KDM_Fax" />
            <telerik:GridCheckBoxColumn DataField="KDM_AMA_No_Contact" HeaderText='AMA No Contact' HeaderStyle-Width="100px" SortExpression="KDM_AMA_No_Contact" UniqueName="KDM_AMA_No_Contact" ></telerik:GridCheckBoxColumn>            
            <telerik:GridCheckBoxColumn DataField="KDM_AMA_No_Contact_Override" HeaderText='AMA No Contact Override' HeaderStyle-Width="120px" SortExpression="KDM_AMA_No_Contact_Override" UniqueName="KDM_AMA_No_Contact_Override" ></telerik:GridCheckBoxColumn>
            </Columns>                
        </MasterTableView>
        <%--<PagerStyle Wrap="true"  Mode="NumericPages" /> --%>
        <ClientSettings ClientEvents-OnRowSelecting="onKDMGridRowClick">
            <DataBinding Location="~/custom/millennium/todaysaccounts/services/MillenniumDataService.svc" DataService-TableName="KDMDetailSet"  />
            <Scrolling AllowScroll="true" UseStaticHeaders="true"  ScrollHeight="220px" /> 
            <Selecting AllowRowSelect="true" />
        </ClientSettings>   
     
    </telerik:RadGrid>    
     <pinso:RadGridWrapper runat="server" ID="gridWrapper" Target="gridKDMDetailsRTA" OnClientDataBinding="hideButtons" /><%--PagingSelector="#divTile3Container .pagination"--%>
</div> 