<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MyKeyContactsList.ascx.cs" Inherits="controls_MyKeyContactsList" %>
<div class="title">
    <%-- placeholder so title area of My Key Contacts matches Key Contacts - this should be removed if something else is added to div. --%>
    
</div>
 <div class="tools">
   <%-- <a href="javascript:onExportClicked('print')" style='margin-right:15px'>Print</a>
    <a href="javascript:onExportClicked('excel')" style='margin-right:15px'>Excel</a>--%>
    <a id="AddKCLnk" href="javascript:OpenMyKC('AddKC','');">Add +</a>
 </div> 
 <div class="clearAll"></div>
 <div id="myKcView">
     <telerik:RadGrid SkinID="radTable" runat="server" ID="gridMyContacts" AllowSorting="true" AllowPaging="false" AllowFilteringByColumn="false" EnableEmbeddedSkins="false">        
        <MasterTableView autogeneratecolumns="false" ClientDataKeyNames="KC_ID" AllowPaging="false">
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
        <ClientSettings ClientEvents-OnRowSelecting="onMyKCGridRowClick" >
            <DataBinding Location="~/todaysaccounts/services/pathfinderclientdataservice.svc" DataService-TableName="KeyContactBasicSet" SelectCountMethod="GetContactCount"/>
            <Scrolling AllowScroll="false" UseStaticHeaders="true"/> 
            <Selecting AllowRowSelect="true" />
        </ClientSettings>   
        
    </telerik:RadGrid>
    <pinso:RadGridWrapper runat="server" ID="gridWrapper" Target="gridMyContacts" PagingSelector="#divTile4Container .pagination" />
 </div>
    
