<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Modal.master" AutoEventWireup="true"
    CodeFile="AddEditCCReports.aspx.cs" Inherits="custom_pinso_customercontactreports_all_AddEditCCReports" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register src="~/custom/Alcon/customercontactreports/controls/AddEditCCRScript.ascx" tagname="CCRScript" tagprefix="pinso" %>
<%@ OutputCache VaryByParam="None" Duration="1" NoStore="true"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--[if lte IE 7]>
        <style type="text/css">
            .ccrModalContainer
            {
                padding: 0px !important;
            }
        </style>
    <![endif]-->
    <script type="text/javascript">
        function RefreshGrid() 
        {
            return window.top.$find("ctl00_ctl00_Tile3_Tile6_CCRGridList1_gridCCReports");
        }

        function RefreshCCRs() 
        {
            RefreshGrid().get_masterTableView().rebind();
            window.setTimeout(CloseWin, 2000);
        }
        
        function ConfirmMsg() 
        {
            window.setTimeout(CloseWin, 2000);
        }
        
        function CloseWin() 
        {
            var manager = window.top.GetRadWindowManager();           

            var window1 = manager.getWindowByName("AddCCR");
            if (window1 != null)
                window1.close();

            var window2 = manager.getWindowByName("EditCCR");
            if (window2 != null)
                window2.close();
        }
        function onPrintClicked() {
            var querystring = window.location.search;
            var CRID = '<%= Request.QueryString["CRID"] %>';
            var data = { Contact_Report_ID: CRID };
            window.top.clientManager.set_SelectionData(data, 1);
            var type = 'print';
                       
            window.top.clientManager.exportView(type, true, 'customercontactreport');

        }           
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" runat="Server">
    <asp:Literal runat="server" ID="titleText" />
    <pinso:CCRScript ID ="CCRScript1" runat="server" />    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="tools" runat="server">  
     <a href="javascript:onPrintClicked()" style='margin-right:15px'><asp:Label ID="Print" runat="server" Text="Print" ></asp:Label></a>  
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="main" runat="Server">
    <asp:HiddenField ID="PlanNameHdn" runat="server" Value="" Visible="false" />
    <asp:HiddenField Id="hdnPrdsDisccused" runat="server" Value="" Visible="true" />   
    <asp:HiddenField ID ="hdnKeyContacts" runat="server" Value="" Visible="true" />  
    <asp:HiddenField Id="hdnMeetActivity" runat="server" Value="" Visible="true" />  
    <div id="AddCCRMain" class="ccrModalContainer customercontactreports">
    <asp:FormView runat="server" ID="formViewCCR" CellPadding="0" Width="100%" DataSourceID="dsContactReport">
        <ItemTemplate>
            <table width="100%" >
                <tr>
                    <td valign="top">
                        <table align="left" >
                            <tr align="left">
                                <td width="140px">
                                    <label for="rdCCRDate">Contact Report Date *</label>
                                </td>
                                <td>
                                    <asp:TextBox id="rdCCRDate" TabIndex="1" name="Contact_Date" runat="server" CssClass="datePicker" class="datePicker" style="margin-bottom: 2px;" value='<%# Eval("Contact_Date", "{0:M/dd/yyyy}") %>' />                                    
                                    <asp:CompareValidator runat="server" ID="compareDate" ControlToValidate="rdCCRDate" ErrorMessage="Please enter a valid report date." Display="None" Type="Date" Operator="DataTypeCheck" />   
                                </td>
                            </tr>
                            <tr align="left">
                                <td >
                                    <label for="rdlMeetingType" >Meeting Type *</label><br />                                    
                                </td>
                                <td >
                                   <telerik:RadComboBox ID="rdlMeetingType" runat="server" AppendDataBoundItems="true" Width="220px"
                                        DropDownWidth="220px"  
                                        OnClientLoad="onMeetingTypeChanged"
                                        OnClientSelectedIndexChanged="onMeetingTypeChanged"
                                        DataSourceID="dsMeetingType" TabIndex="2" Skin="pathfinder" EnableEmbeddedSkins="false" DataTextField="Meeting_Type_Name"
                                        DataValueField="Meeting_Type_ID" SelectedValue='<%# Eval("Meeting_Type_ID") %>' >
                                    <Items>
                                        <telerik:RadComboBoxItem Text="-Select Meeting Type-" Value="0" Selected="true" />
                                    </Items>   
                                    </telerik:RadComboBox>
                                </td>
                            </tr> 
                            <tr align="left">
                                <td colspan="2" style="padding-left: 5px;">
                                    <asp:TextBox runat="server" ID="txtMeetingTypeOther" Text='<%# Eval("Meeting_Type_Other") %>' CssClass="meetingTypeOther" TextMode="MultiLine" Rows="4" Width="99%" />
                                    <pinso:MaxLengthValidator ID="MaxLengthValidator1" runat="server" ControlToValidate="txtMeetingTypeOther" ErrorMessage="Meeting Type notes must be less than 150 characters." MaxLength="150" Display="None" />
                                </td>
                            </tr>     
                            <tr align="left">
                                <td colspan="2"><label>Products Discussed *</label></td>                                
                            </tr>
                            <tr align="left">
                                <td colspan="2" style="padding-left: 5px;">
                                    <telerik:RadComboBox runat="server" ID="rdlProductsDiscussed" DataSourceID="dsProductsDiscussed"
                                         TabIndex="4" Skin="pathfinder" Width="343px" DropDownWidth="343px"
                                         EnableEmbeddedSkins="false" DataTextField="Drug_Name" DataValueField="Products_Discussed_ID"
                                         AppendDataBoundItems="true" AllowCustomText="True" Text="-Select Products Discussed-" 
                                         OnClientDropDownClosed="setProdDiscussedText" OnClientLoad="setProdDiscussedText">
                                         <ItemTemplate>
                                            <span id='<%# String.Format("p{0}",Eval("Products_Discussed_ID")) %>'>
                                               <asp:CheckBox runat="server" ID="chkProductDiscussed" Text ='<%# Eval("Drug_Name") %>' 
                                                             onclick='<%# string.Format("ProdsDiscussChanged(this,{0})", Eval("Products_Discussed_ID")) %>' 
                                                              />
                                               </span>
                                         </ItemTemplate>
                                      </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr align="left" valign="top">
                                <td colspan="2" style="padding-left: 5px; ">
                                    <div id="spanProducts">                                                
                                    </div>
                                </td>
                            </tr>       
                        </table>          
                    </td>
                    <td valign="top">
                        <table cellpadding="2" cellspacing="2" >
                            <tr align="left">
                                <td width="130px">
                                    <div style="height: 25px;"/>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr align="left">
                                <td> 
                                    <label for="rdlMeetingActivity">Meeting Activity *</label>
                                </td>
                             </tr> 
                             <tr align="left">
                                <td colspan="2" style="padding-left: 5px;">                                   
                                    <telerik:RadComboBox runat="server" ID="rdlMeetingActivity" DataSourceID="dsMeetingActivity"
                                    TabIndex="3" Skin="pathfinder" Width="230px" DropDownWidth="220px"
                                    EnableEmbeddedSkins="false" DataTextField="Meeting_Activity_Name" DataValueField="Meeting_Activity_ID"
                                    AppendDataBoundItems="true" AllowCustomText="True" Text="-Select Meeting Activity-" 
                                    OnClientDropDownClosed="setMeetActivityText" OnClientLoad="setMeetActivityText">
                                     <ItemTemplate>
                                        <span id='<%# String.Format("y{0}",Eval("Meeting_Activity_ID")) %>'>
                                           <asp:CheckBox runat="server" ID="chkMeetActivity" Text ='<%# Eval("Meeting_Activity_Name") %>' 
                                                         onclick='<%# string.Format("MeetActivityChanged(this,{0})", Eval("Meeting_Activity_ID")) %>' />
                                        </span>
                                     </ItemTemplate>
                                  </telerik:RadComboBox>
                                </td>
                            </tr> 
                            <tr align="left" valign="top">
                                <td colspan="2" style="padding-left: 5px; ">
                                    <div id="spanMeetActivity">                            
                                   </div>
                                </td>
                            </tr>
                            <tr align="left">
                                <td colspan="2" style="padding-left: 5px;">
                                    <asp:TextBox runat="server" ID="txtMeetingActivityOther" Text='<%# Eval("Meeting_Activity_Other") %>' CssClass="meetingActivityOther" TextMode="MultiLine" Rows="2" Width="99%" />
                                    <pinso:MaxLengthValidator ID="MaxLengthValidator32" runat="server" ControlToValidate="txtMeetingActivityOther" ErrorMessage="Meeting Activity notes must be less than 50 characters." MaxLength="50" Display="None" />
                                </td>
                            </tr>     
                            <tr align="left">
                                <td colspan="2" visible='<%# Convert.ToBoolean(Request.QueryString["Profiled"]) %>' runat="server">
                                    <label>Persons Met *</label>
                                </td>                                
                            </tr>
                            <tr align="left">
                                <td colspan="2" style="padding-left: 5px;">
                                    <telerik:RadComboBox ID="rdlKeyContacts" runat="server" AppendDataBoundItems="true"  
                                        Visible='<%# Convert.ToBoolean(Request.QueryString["Profiled"]) %>'
                                        TabIndex="5" Skin="pathfinder" Width="343px" DropDownWidth="343px" DataSourceID="dsPersonsMet" 
                                        DataTextField="Full_Name" DataValueField ="KC_ID" EnableEmbeddedSkins="false" AllowCustomText="True" Text="-Select Persons Met-" 
                                        OnClientDropDownClosed="setPersonsMetText" OnClientLoad="setPersonsMetText">
                                       <ItemTemplate>
                                        <span id='<%# String.Format("k{0}",Eval("KC_ID")) %>'>
                                            <asp:CheckBox runat="server" ID="chkKeyContact" Text ='<%# Eval("Full_Name") %>' onclick='<%# string.Format("KeyContactChanged(this,{0})", Eval("Full_ID")) %>' />
                                        </span>
                                        </ItemTemplate>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr align="left" runat="server" visible='<%# Convert.ToBoolean(Request.QueryString["Profiled"]) %>'>
                                <td colspan="2" style="padding-left: 5px;">
                                    <div id="spanKeyContacts">                          
                                    </div>
                                </td>
                            </tr> 
                             <tr align="left">
                                <td colspan="2">
                                    <label id="otherKC" runat="server" visible='<%# !Convert.ToBoolean(Request.QueryString["Profiled"]) %>'> Other Key Contacts *</label>
                                </td>                                
                            </tr>
                            <tr align="left">
                                <td colspan="2" style="padding-left: 5px;padding-bottom : 2px;">                                   
                                     <asp:TextBox id="txtKeyContactsOther" Columns = "64" MaxLength="255"  Rows="4" TextMode="MultiLine" runat="server" 
                                            Text='<%# Eval("Key_Contacts_Other") %>' 
                                            visible='<%# !Convert.ToBoolean(Request.QueryString["Profiled"]) %>' />                                                                    
                                    <pinso:MaxLengthValidator ID="MaxLengthValidator12" runat="server" ControlToValidate="txtKeyContactsOther" ErrorMessage="Key Contacts must be less than 255 characters." MaxLength="255" Display="None" />
                                </td>
                            </tr>
                            <tr align="left">
                                <td width="130px">
                                    <label for ="rdFollowUpDate"> Follow-up Date</label>
                                </td>
                                <td>
                                    <asp:TextBox id="rdFollowUpDate" TabIndex="7" name="Followup_Date" runat="server" CssClass="datePicker" class="datePicker" value='<%# Eval("Followup_Date", "{0:M/dd/yyyy}") %>' />                                   
                                    <asp:CompareValidator runat="server" ID="CompareValidator1" ControlToValidate="rdFollowUpDate" ErrorMessage="Please enter a valid follow-up date." Display="None" Type="Date" Operator="DataTypeCheck" />  
                                </td>
                            </tr>                            
            </table>            
            <pinso:ClientValidator ID= "RequiredVal" Target="rdCCRDate" Required ="true" Text="Please select a Date" runat="server"/>
            <asp:requiredfieldvalidator ID="Requiredfieldvalidator1" controltovalidate="rdCCRDate" display="none" errormessage="Contact Report Date required" runat="server" />
            <asp:requiredfieldvalidator ID="Requiredfieldvalidator6" controltovalidate="rdlMeetingActivity" display="none" InitialValue="-Select Meeting Activity-" errormessage="Meeting Activity Required" runat="server" />            
            <asp:requiredfieldvalidator ID="Requiredfieldvalidator2" controltovalidate="rdlMeetingType" display="none" InitialValue="-Select Meeting Type-" errormessage="Meeting Type Required" runat="server" />
            <asp:requiredfieldvalidator ID="Requiredfieldvalidator3" controltovalidate="rdlProductsDiscussed" display="none" InitialValue="-Select Products Discussed-" errormessage="Products Discussed Required" runat="server"/>
            <asp:requiredfieldvalidator ID="Requiredfieldvalidator4" controltovalidate="rdlKeyContacts" display="none" InitialValue="-Select Persons Met-" errormessage="Persons Met Required" runat="server" />
           
            <asp:CompareValidator ID="comparefieldvalidator4" ControlToCompare="rdCCRDate" operator="GreaterThan" type="Date" ControlToValidate="rdFollowupDate" Display="None" ErrorMessage="Follow-up Date must be greater than the Customer Contact Report Date" runat="server" />
            <div style="height:15px">
                <asp:ValidationSummary runat="server" ID="validationSummary" DisplayMode="List" />
            </div>
            <div class="modalFormButtons">
                 <pinso:CustomButton TabIndex="9" ID="btnEdit" runat="server" Text="Update" Visible="true" OnClick="Editbtn_Click" />
                 <pinso:CustomButton ID="btnReset" TabIndex="10" runat="server" Text="Reset" OnClientClick="javascript:ClearForm(); return false;" />                     
            </div>
        </ItemTemplate>
        <InsertItemTemplate>
          <table width="100%" >
                <tr>
                    <td valign="top">
                        <table align="left" >
                            <tr align="left">
                                <td width="140px">
                                    <label for="rdCCRDate">Contact Report Date *</label>
                                </td>
                                <td>
                                    <asp:TextBox id="rdCCRDate" TabIndex="1" name="Contact_Date" runat="server" CssClass="datePicker" class="datePicker" style="margin-bottom: 2px;" value='<%# Eval("Contact_Date", "{0:M/dd/yyyy}") %>' />                                    
                                    <asp:CompareValidator runat="server" ID="compareDate" ControlToValidate="rdCCRDate" ErrorMessage="Please enter a valid report date." Display="None" Type="Date" Operator="DataTypeCheck" />   
                                </td>
                            </tr>
                            <tr align="left">
                                <td width="130px"> 
                                    <label for="rdlMeetingType">Meeting Type *</label>
                                </td>
                                <td>
                                    <telerik:RadComboBox TabIndex="2" ID="rdlMeetingType" runat="server" AppendDataBoundItems="true"
                                        DataSourceID="dsMeetingType" Skin="pathfinder" Width="220px"
                                        OnClientLoad="onMeetingTypeChanged"
                                        OnClientSelectedIndexChanged="onMeetingTypeChanged"
                                        DropDownWidth="220px"  EnableEmbeddedSkins="false" DataTextField="Meeting_Type_Name"
                                        DataValueField="Meeting_Type_ID" SelectedValue='<%# Eval("Meeting_Type_ID") %>'  >
                                    <Items>
                                        <telerik:RadComboBoxItem Text="-Select Meeting Type-" Value="0" Selected="true"/>
                                    </Items>   
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr align="left">
                                <td colspan="2" style="padding-left: 5px;">
                                    <asp:TextBox runat="server" ID="txtMeetingTypeOther" Text='<%# Eval("Meeting_Type_Other") %>' CssClass="meetingTypeOther" TextMode="MultiLine" Rows="4" Width="99%" />
                                    <pinso:MaxLengthValidator ID="MaxLengthValidator2" runat="server" ControlToValidate="txtMeetingTypeOther" ErrorMessage="Meeting Type notes must be less than 150 characters." MaxLength="150" Display="None" />
                                </td>
                            </tr>      
                            <tr align="left">
                                <td colspan="2"><label>Products Discussed *</label></td>                                
                            </tr>
                            <tr align="left">
                                <td colspan="2" style="padding-left: 5px;">
                                    <telerik:RadComboBox runat="server" ID="rdlProductsDiscussed" DataSourceID="dsProductsDiscussed"
                                         TabIndex="4" Skin="pathfinder" Width="343px" DropDownWidth="343px"
                                         EnableEmbeddedSkins="false" DataTextField="Drug_Name" DataValueField="Products_Discussed_ID"
                                         AppendDataBoundItems="true" AllowCustomText="True" Text="-Select Products Discussed-" 
                                         OnClientDropDownClosed="setProdDiscussedText" OnClientLoad="setProdDiscussedText">
                                         <ItemTemplate>
                                            <span id='<%# String.Format("p{0}",Eval("Products_Discussed_ID")) %>'>
                                               <asp:CheckBox runat="server" ID="chkProductDiscussed" Text ='<%# Eval("Drug_Name") %>' 
                                                             onclick='<%# string.Format("ProdsDiscussChanged(this,{0})", Eval("Products_Discussed_ID")) %>' 
                                                              />
                                               </span>
                                         </ItemTemplate>
                                      </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr align="left" valign="top">
                                <td colspan="2" style="padding-left: 5px; ">
                                    <div id="spanProducts">                                                
                                    </div>
                                </td>
                            </tr>  
                           
                        </table>          
                    </td>
                    <td valign="top">
                        <table cellpadding="2" cellspacing="2" >
                            <tr align="left">
                                <td width="130px">
                                    <div style="height: 25px;"/>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr align="left">
                                <td> 
                                    <label for="rdlMeetingActivity">Meeting Activity *</label>
                                </td>
                            </tr> 
                            <tr align="left">
                                <td colspan="2" style="padding-left: 5px;">                                 
                                    <telerik:RadComboBox runat="server" ID="rdlMeetingActivity" DataSourceID="dsMeetingActivity"
                                    TabIndex="3" Skin="pathfinder" Width="230px" DropDownWidth="220px"
                                    EnableEmbeddedSkins="false" DataTextField="Meeting_Activity_Name" DataValueField="Meeting_Activity_ID"
                                    AppendDataBoundItems="true" AllowCustomText="True" Text="-Select Meeting Activity-" 
                                    OnClientDropDownClosed="setMeetActivityText" OnClientLoad="setMeetActivityText" >
                                     <ItemTemplate>
                                        <span id='<%# String.Format("y{0}",Eval("Meeting_Activity_ID")) %>'>
                                           <asp:CheckBox runat="server" ID="chkMeetActivity" Text ='<%# Eval("Meeting_Activity_Name") %>' 
                                                         onclick='<%# string.Format("MeetActivityChanged(this,{0})", Eval("Meeting_Activity_ID")) %>' />
                                        </span>
                                     </ItemTemplate>
                                  </telerik:RadComboBox>
                                </td>
                            </tr> 
                            <tr align="left" valign="top">
                                <td colspan="2" style="padding-left: 5px; ">
                                    <div id="spanMeetActivity">                            
                                   </div>
                                </td>
                            </tr>
                            <tr align="left">
                                <td colspan="2" style="padding-left: 5px;">
                                    <asp:TextBox runat="server" ID="txtMeetingActivityOther" Text='<%# Eval("Meeting_Activity_Other") %>' CssClass="meetingActivityOther" TextMode="MultiLine" Rows="2" Width="99%" />
                                    <pinso:MaxLengthValidator ID="MaxLengthValidator32" runat="server" ControlToValidate="txtMeetingActivityOther" ErrorMessage="Meeting Activity notes must be less than 50 characters." MaxLength="50" Display="None" />
                                </td>
                            </tr>     
                            <tr align="left">
                                <td colspan="2"  visible='<%# Convert.ToBoolean(Request.QueryString["Profiled"]) %>' runat="server">
                                    <label>Persons Met *</label>
                                </td>                                
                            </tr>
                            <tr align="left">
                                <td colspan="2" style="padding-left: 5px;">
                                    <telerik:RadComboBox ID="rdlKeyContacts" runat="server" AppendDataBoundItems="true"
                                        visible='<%# Convert.ToBoolean(Request.QueryString["Profiled"]) %>'
                                        TabIndex="5" Skin="pathfinder" Width="343px" DropDownWidth="343px" DataSourceID="dsPersonsMet" 
                                        DataTextField="Full_Name" DataValueField ="KC_ID" EnableEmbeddedSkins="false" AllowCustomText="True" 
                                        Text="-Select Persons Met-" OnClientDropDownClosed="setPersonsMetText" OnClientLoad="setPersonsMetText">
                                       <ItemTemplate>
                                        <span id='<%# String.Format("k{0}",Eval("KC_ID")) %>'>
                                            <asp:CheckBox runat="server" ID="chkKeyContact" Text ='<%# Eval("Full_Name") %>' onclick='<%# string.Format("KeyContactChanged(this,{0})", Eval("Full_ID")) %>' />
                                        </span>
                                        </ItemTemplate>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr align="left" runat="server" visible='<%# Convert.ToBoolean(Request.QueryString["Profiled"]) %>'>
                                <td colspan="2" style="padding-left: 5px;">
                                    <div id="spanKeyContacts">                          
                                    </div>
                                </td>
                            </tr>
                            <tr align="left">
                                <td colspan="2">
                                    <label id="otherKC" runat="server" visible='<%# !Convert.ToBoolean(Request.QueryString["Profiled"]) %>'> Other Key Contacts *</label>
                                </td>                                
                            </tr>
                            <tr align="left">
                                <td colspan="2" style="padding-left: 5px;padding-bottom : 2px;">                                  
                                   <asp:TextBox id="txtKeyContactsOther" Columns = "64" MaxLength="255"  Rows="4" TextMode="MultiLine"  
                                        runat="server" Text='<%# Eval("Key_Contacts_Other") %>'  
                                        visible='<%# !Convert.ToBoolean(Request.QueryString["Profiled"]) %>'/>                                                                
                                   <pinso:MaxLengthValidator ID="MaxLengthValidator12" runat="server" ControlToValidate="txtKeyContactsOther" ErrorMessage="Key Contacts must be less than 255 characters." MaxLength="255" Display="None" />
                                </td>
                            </tr> 
                            <tr align="left">
                                <td width="130px">
                                    <label for ="rdFollowUpDate"> Follow-up Date</label>
                                </td>
                                <td>
                                    <asp:TextBox id="rdFollowUpDate" TabIndex="7" name="Followup_Date" runat="server" CssClass="datePicker" class="datePicker" value='<%# Eval("Followup_Date", "{0:M/dd/yyyy}") %>' />
                                    <asp:CompareValidator runat="server" ID="CompareValidator1" ControlToValidate="rdFollowUpDate" ErrorMessage="Please enter a valid follow-up date." Display="None" Type="Date" Operator="DataTypeCheck" />  
                                </td>
                            </tr>                           
            </table>           
            <pinso:ClientValidator ID= "RequiredVal" Target="rdCCRDate" Required ="true" Text="Please select a Date" runat="server"/>
            <asp:requiredfieldvalidator ID="Requiredfieldvalidator1" controltovalidate="rdCCRDate" display="none" errormessage="Contact Report Date required" runat="server" />
            <asp:requiredfieldvalidator ID="Requiredfieldvalidator6" controltovalidate="rdlMeetingActivity" display="none" InitialValue="-Select Meeting Activity-" errormessage="Meeting Activity Required" runat="server" />            
            <asp:requiredfieldvalidator ID="Requiredfieldvalidator2" controltovalidate="rdlMeetingType" display="none" InitialValue="-Select Meeting Type-" errormessage="Meeting Type Required" runat="server" />
            <asp:requiredfieldvalidator ID="Requiredfieldvalidator3" controltovalidate="rdlProductsDiscussed" display="none" InitialValue="-Select Products Discussed-" errormessage="Products Discussed Required" runat="server" />
            <asp:requiredfieldvalidator ID="Requiredfieldvalidator4" controltovalidate="rdlKeyContacts" display="none" InitialValue="-Select Persons Met-" errormessage="Persons Met Required" runat="server" />
            
           
            <asp:CompareValidator ID="comparefieldvalidator4" ControlToCompare="rdCCRDate" operator="GreaterThan" type="Date" ControlToValidate="rdFollowupDate" Display="None" ErrorMessage="Follow-up Date must be greater than the Customer Contact Report Date" runat="server" />
            <div style="height:15px">
                <asp:ValidationSummary runat="server" ID="validationSummary" DisplayMode="List" />
            </div>
            <div class="modalFormButtons">                
                <pinso:CustomButton TabIndex="9" ID="btnEdit" runat="server" Text="Add" Visible="true" OnClick="Editbtn_Click" />
                <pinso:CustomButton ID="btnReset" TabIndex="10" runat="server" Text="Reset" OnClientClick="javascript:ClearForm(); return false;" />                       
            </div>
        </InsertItemTemplate>
    </asp:FormView>
    </div>
    <asp:EntityDataSource runat="server" ID="dsMeetingType" ConnectionString="name=PathfinderClientEntities"
        DefaultContainerName="PathfinderClientEntities" EntitySetName="LkpMeetingTypeSet"
        EntityTypeFilter="LkpMeetingType" OrderBy="it.Sort_Index, it.[Meeting_Type_Name]">
    </asp:EntityDataSource>
    <asp:EntityDataSource runat="server" ID="dsMeetingActivity" ConnectionString="name=PathfinderClientEntities"
        DefaultContainerName="PathfinderClientEntities" 
        EntitySetName="LkpMeetingActivitySet" OrderBy="it.Sort_Index, it.[Meeting_Activity_Name]">
    </asp:EntityDataSource>
    <asp:EntityDataSource runat="server" ID="dsProductsDiscussed" ConnectionString="name=PathfinderClientEntities"
        DefaultContainerName="PathfinderClientEntities" EntitySetName="LkpProductsDiscussedSet"
        EntityTypeFilter="LkpProductsDiscussed" OrderBy="it.Sort_Index, it.[Drug_Name]">
    </asp:EntityDataSource>
    <asp:EntityDataSource runat="server" ID="dsContactReport" ConnectionString="name=PathfinderClientEntities"
        DefaultContainerName="PathfinderClientEntities" 
        EntitySetName="ContactReportSet" AutoGenerateWhereClause="True">
        <WhereParameters>
            <asp:QueryStringParameter QueryStringField="PlanID" Name="Plan_ID" Type="Int32" ConvertEmptyStringToNull="true" />
            <asp:QueryStringParameter QueryStringField="CRID" Name="Contact_Report_ID" Type="Int32"
                ConvertEmptyStringToNull="true" />
        </WhereParameters>
        <InsertParameters>
            <asp:QueryStringParameter QueryStringField="PlanID" Name="Plan_ID" Type="Int32" ConvertEmptyStringToNull="true" />
        </InsertParameters>
        <UpdateParameters>
            <asp:QueryStringParameter QueryStringField="PlanID" Name="Plan_ID" Type="Int32" ConvertEmptyStringToNull="true" />
            <asp:QueryStringParameter QueryStringField="CRID" Name="Contact_Report_ID" Type="Int32"
                ConvertEmptyStringToNull="true" />
        </UpdateParameters>
    </asp:EntityDataSource>   
   
   <asp:SqlDataSource ID="dsPersonsMet" runat="server"  
        ConnectionString="<%$ ConnectionStrings:PathfinderClientDB_Format %>" 
        SelectCommand="usp_GetAvailableContactsForCCR"
        SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter  Name="Plan_ID" QueryStringField="PlanID" DbType ="Int32"  />
           
        </SelectParameters>  
    </asp:SqlDataSource> 
    <div>
        <asp:Label ID="Msglbl" runat="server" Text="" Visible="false"></asp:Label>
        <asp:Label ID="Label2" runat="server" Text='<%= Request.Form["PlanID"]%>'></asp:Label>
    </div>
</asp:Content>
 