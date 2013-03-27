<%@ Control Language="C#" AutoEventWireup="true" CodeFile="KeyContactsListGridVA.ascx.cs" Inherits="todaysaccounts_controls_VAKeyContactsListGrid" %>
<div id="kcView">
     <telerik:RadGrid SkinID="radTable" runat="server" ID="gridKeyContacts" AllowSorting="true" AllowPaging="false" AllowFilteringByColumn="false" EnableEmbeddedSkins="false" PageSize="25">        
        <MasterTableView autogeneratecolumns="false" ClientDataKeyNames="KC_ID,Parent_Plan_ID" PageSize="25">
           <Columns>
            <telerik:GridBoundColumn DataField="KC_F_Name" HeaderText='<%$ Resources:Resource, label_first_name %>' HeaderStyle-Width="150px" SortExpression="KC_F_Name" UniqueName="KC_F_Name" ItemStyle-CssClass="firstCol" />
            <telerik:GridBoundColumn DataField="KC_L_Name" HeaderText='<%$ Resources:Resource, label_last_name %>' HeaderStyle-Width="150px" SortExpression="KC_L_Name" UniqueName="KC_L_Name" />
            <telerik:GridBoundColumn DataField="KC_Title_Name" HeaderText='<%$ Resources:Resource, label_designation %>' HeaderStyle-Width="200px" SortExpression="KC_Title_Name" UniqueName="KC_Title_Name" />
            <telerik:GridBoundColumn DataField="KC_Role" HeaderText='<%$ Resources:Resource, label_title %>' HeaderStyle-Width="200px" SortExpression="KC_Role" UniqueName="KC_Role" />
            <telerik:GridHyperLinkColumn DataNavigateUrlFields="KC_Email" HeaderStyle-Width="150px" SortExpression="KC_Email" DataTextField="KC_Email" DataTextFormatString="{0}" DataNavigateUrlFormatString="mailto:{0}" HeaderText='<%$ Resources:Resource, label_email %>' UniqueName="KC_Email" />
            <telerik:GridBoundColumn DataField="KC_Phone" HeaderText='<%$ Resources:Resource, label_phone %>' HeaderStyle-Width="100px" SortExpression="KC_Phone" UniqueName="KC_Phone" />
            <telerik:GridBoundColumn DataField="KC_Admin_Name" HeaderText='<%$ Resources:Resource, label_assistant_name %>' HeaderStyle-Width="150px" SortExpression="KC_Admin_Name" UniqueName="KC_Admin_Name" />
            <telerik:GridBoundColumn DataField="KC_Admin_PH" HeaderText='<%$ Resources:Resource, label_assistant_phone %>' HeaderStyle-Width="100px" SortExpression="KC_Admin_PH" UniqueName="KC_Admin_PH" />            
            </Columns>                
        </MasterTableView>
        <%--<PagerStyle Wrap="true"  Mode="NumericPages" /> --%>
         <ClientSettings>
            <DataBinding Location="~/todaysaccounts/services/pathfinderdataservice.svc" DataService-TableName="PlanAffiliationKeyContactSet" SelectCountMethod="GetVAContactCount"/>
            <Scrolling AllowScroll="false" UseStaticHeaders="true"/> 
            <Selecting AllowRowSelect="true" />
        </ClientSettings>   
     
    </telerik:RadGrid>
    <pinso:RadGridWrapper runat="server" ID="gridWrapper" Target="gridKeyContacts" PagingSelector="#divTile3Container .pagination" />
</div> 