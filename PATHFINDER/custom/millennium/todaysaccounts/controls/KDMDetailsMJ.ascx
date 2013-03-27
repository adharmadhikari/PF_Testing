<%@ Control Language="C#" AutoEventWireup="true" CodeFile="KDMDetailsMJ.ascx.cs" Inherits="custom_millennium_todaysaccounts_controls_KDMDetailsMJ" %>
 <div class="tools">
        <a id="AddKDM" href="javascript:OpenKDM('AddKDM','','MJ');">Add </a>
        <span id="EditDeleteButtons"> | <a id="EditKDM" href="javascript:OpenKDM('EditKDM','','MJ');">Edit </a> |
        <a id="DeleteKDM" href="javascript:OpenKDM('DelKDM','','MJ');">Delete </a></span>
 </div> 
     <div class="clearAll"></div>
<div id="kcView">
     <telerik:RadGrid SkinID="radTable" runat="server" ID="gridKDMDetailsMJ" AllowSorting="true" AllowPaging="false" AllowFilteringByColumn="false" EnableEmbeddedSkins="false" PageSize="200">        
        <MasterTableView autogeneratecolumns="false" ClientDataKeyNames="KDM_ID" AllowPaging="false" Width="100%" PageSize="200">
           <Columns>
            <telerik:GridBoundColumn DataField="CAC_CMD" HeaderText="CAC / CMD" HeaderStyle-Width="60px" SortExpression="CAC_CMD" UniqueName="CAC_CMD" ItemStyle-CssClass="firstCol" />
            <telerik:GridBoundColumn DataField="KDM_F_Name" HeaderText='<%$ Resources:Resource, label_first_name %>' HeaderStyle-Width="130px" SortExpression="KDM_F_Name" UniqueName="KDM_F_Name" ItemStyle-CssClass="firstCol" />
            <telerik:GridBoundColumn DataField="KDM_L_Name" HeaderText='<%$ Resources:Resource, label_last_name %>' HeaderStyle-Width="130px" SortExpression="KDM_L_Name" UniqueName="KDM_L_Name" />
            <telerik:GridBoundColumn DataField="Titles" HeaderText='<%$ Resources:Resource, label_title %>' HeaderStyle-Width="170px" SortExpression="Titles" UniqueName="Titles" />
            <telerik:GridBoundColumn DataField="Credentials" HeaderText='Credentials' HeaderStyle-Width="170px" SortExpression="Credentials" UniqueName="Credentials" />
            <telerik:GridBoundColumn DataField="CAC" HeaderText='CAC Type' HeaderStyle-Width="60px" SortExpression="CAC" UniqueName="CAC" />
            <telerik:GridBoundColumn DataField="Aff_MJ_Org_Name" HeaderText='Organization' HeaderStyle-Width="110px" SortExpression="Aff_MJ_Org_Name" UniqueName="Aff_MJ_Org_Name" />
            <telerik:GridBoundColumn DataField="KDM_Email" HeaderText='<%$ Resources:Resource, label_email %>' HeaderStyle-Width="130px" SortExpression="KDM_Email" UniqueName="KDM_Email" />
            <telerik:GridBoundColumn DataField="KDM_Phone" HeaderText='<%$ Resources:Resource, label_phone %>' HeaderStyle-Width="110px" SortExpression="KDM_Phone" UniqueName="KDM_Phone" />          
            <telerik:GridBoundColumn DataField="KDM_Fax" HeaderText='<%$ Resources:Resource, Label_Fax %>' HeaderStyle-Width="110px" SortExpression="KDM_Fax" UniqueName="KDM_Fax" />
            <telerik:GridBoundColumn DataField="Professional_Customer_Master_ID" HeaderText='CM_ID' HeaderStyle-Width="90px" SortExpression="Professional_Customer_Master_ID" UniqueName="Professional_Customer_Master_ID" />
            <telerik:GridCheckBoxColumn DataField="KDM_AMA_No_Contact" HeaderText='AMA No Contact' HeaderStyle-Width="100px" SortExpression="KDM_AMA_No_Contact" UniqueName="KDM_AMA_No_Contact" ></telerik:GridCheckBoxColumn>            
            <telerik:GridCheckBoxColumn DataField="KDM_AMA_No_Contact_Override" HeaderText='AMA No Contact Override' HeaderStyle-Width="160px" SortExpression="KDM_AMA_No_Contact_Override" UniqueName="KDM_AMA_No_Contact_Override" ></telerik:GridCheckBoxColumn>
            </Columns>                
        </MasterTableView>
        <%--<PagerStyle Wrap="true"  Mode="NumericPages" /> --%>
        <ClientSettings ClientEvents-OnRowSelecting="onKDMGridRowClick">
            <DataBinding Location="~/custom/millennium/todaysaccounts/services/MillenniumDataService.svc" DataService-TableName="KDMDetailSet"  />
            <Scrolling AllowScroll="false" UseStaticHeaders="true" /> 
            <Selecting AllowRowSelect="true" />
        </ClientSettings>   
     
    </telerik:RadGrid>    
     <pinso:RadGridWrapper runat="server" ID="gridWrapper" Target="gridKDMDetailsMJ" OnClientDataBinding="hideButtons" />
</div> 