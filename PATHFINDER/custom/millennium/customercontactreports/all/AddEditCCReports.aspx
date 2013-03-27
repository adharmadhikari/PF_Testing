<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Modal.master" AutoEventWireup="true"
    CodeFile="AddEditCCReports.aspx.cs" Inherits="custom_millennium_customercontactreports_all_AddEditCCReports" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register src="~/custom/millennium/customercontactreports/controls/AddEditCCRScript.ascx" tagname="CCRScript" tagprefix="pinso" %>
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
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" runat="Server">
    <asp:Literal runat="server" ID="titleText" />
    <pinso:CCRScript ID ="CCRScript1" runat="server" />    
</asp:Content><asp:Content ID="Content3" ContentPlaceHolderID="tools" runat="server">  
     </asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="main" runat="Server">
    <asp:HiddenField ID="PlanNameHdn" runat="server" Value="" Visible="false" />
    <asp:HiddenField Id="hdnPrdsDisccused" runat="server" Value="" Visible="true" />
    <asp:HiddenField ID="hdnMeetOutcome" runat="server" Value="" Visible="true" />
    <asp:HiddenField ID="hdncontacttitle" runat="server" Value="" Visible="true" />
    <asp:HiddenField ID ="hdnKeyContacts" runat="server" Value="" Visible="true" />    
    <div id="AddCCRMain" class="ccrModalContainer customercontactreports">
    <asp:FormView runat="server" ID="formViewCCR" CellPadding="0" Width="100%" DataSourceID="dsContactReport">
        <ItemTemplate>
            <table width="100%" >
                <tr>
                    <td valign="top">
                        <table align="left" >
                            <tr align="left">
                                <td width="140px">
                                    <label for="rdCCRDate">Date of Contact *</label>
                                </td>
                                <td>
                                    <asp:TextBox id="rdCCRDate" TabIndex="1" name="Contact_Date" runat="server" CssClass="datePicker" class="datePicker" style="margin-bottom: 2px;" value='<%# Eval("Contact_Date", "{0:M/dd/yyyy}") %>' />
                                    <%--<ajax:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="rdCCRDate" MaskType="Date" runat="server" Mask="99/99/9999" />--%>
                                    <asp:CompareValidator runat="server" ID="compareDate" ControlToValidate="rdCCRDate" ErrorMessage="Please enter a valid report date." Display="None" Type="Date" Operator="DataTypeCheck" />   
                                </td>
                            </tr>
                            <tr align="left">
                                <td >
                                    <label for="rdlMeetingType" >Interaction Type *</label><br />                                    
                                </td>
                                <td >
                                   <telerik:RadComboBox ID="rdlMeetingType" runat="server" AppendDataBoundItems="true" Width="220px"
                                        DropDownWidth="220px"  
                                        DataSourceID="dsMeetingType" TabIndex="2" Skin="pathfinder" EnableEmbeddedSkins="false" DataTextField="Meeting_Type_Name"
                                        DataValueField="Meeting_Type_ID" SelectedValue='<%# Eval("Meeting_Type_ID") %>' >
                                    <Items>
                                        <telerik:RadComboBoxItem Text="-Select Meeting Type-" Value="0" Selected="true" />
                                    </Items>   
                                    </telerik:RadComboBox>
                                </td>
                            </tr> 
                            <tr align="left">
                                <td> 
                                    <label for="rdlMeetingActivity">Meeting Topic *</label>
                                </td>
                                <td>
                                    <telerik:RadComboBox runat="server" ID="rdlMeetingActivity" DataSourceID="dsMeetingActivity"
                                        TabIndex="3" Skin="pathfinder" Width="220px" DropDownWidth="220px"
                                        EnableEmbeddedSkins="false" DataTextField="Meeting_Activity_Name" DataValueField="Meeting_Activity_ID"
                                        SelectedValue='<%# Eval("Meeting_Activity_ID") %>' AppendDataBoundItems="true" >
                                    <Items>
                                        <telerik:RadComboBoxItem Text="-Select Meeting Topic-" Value="0" Selected="true"/>
                                    </Items>       
                                    </telerik:RadComboBox>
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
                                               <asp:CheckBox runat="server" ID="chkProductDiscussed" Text ='<%# Eval("Drug_Name") %>' Checked="true"
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
                           
                            
                            <tr align="left">
                                <td width="130px">                                    
                                </td>
                                <td>
                                </td>
                            </tr> 
                            <tr align="left">
                                <td colspan="2">
                                    <label>Attendees *</label>
                                </td>                                
                            </tr>
                            <tr align="left">
                                <td colspan="2" style="padding-left: 5px;">
                                    <telerik:RadComboBox ID="rdlKeyContacts" runat="server" AppendDataBoundItems="true"
                                        TabIndex="5" Skin="pathfinder" Width="343px" DropDownWidth="343px" DataSourceID="dsPersonsMet" 
                                        DataTextField="Full_Name" DataValueField ="KC_ID" EnableEmbeddedSkins="false" AllowCustomText="True" Text="-Select Attendees-" 
                                        OnClientDropDownClosed="setPersonsMetText" OnClientLoad="setPersonsMetText">
                                       <ItemTemplate>
                                        <span id='<%# String.Format("k{0}",Eval("KC_ID")) %>'>
                                            <asp:CheckBox runat="server" ID="chkKeyContact" Text ='<%# Eval("Full_Name") %>' onclick='<%# string.Format("KeyContactChanged(this,{0})", Eval("Full_ID")) %>' />
                                        </span>
                                        </ItemTemplate>
                                    </telerik:RadComboBox>
                                </td>
                            </tr> 
                           
                            <tr align="left">
                                <td colspan="2" style="padding-left: 5px;">
                                    <div id="spanKeyContacts">                          
                                    </div>
                                </td>
                            </tr>                
                        </table>          
                    </td>
                    <td valign="top">
                        <table cellpadding="2" cellspacing="2" >
                            
                            <tr align="left">
                                <td width="130px"> 
                                    <label for="rdlFollowupRequired">Follow-up Required?  *</label>
                                </td>
                                <td>
                                
                                    <telerik:RadComboBox TabIndex="6" ID="rdlFollowupRequired" runat="server" AppendDataBoundItems="true"
                                         Skin="pathfinder" Width="220px" SelectedValue='<%# Eval("followup_requires") %>'
                                        DropDownWidth="220px"  EnableEmbeddedSkins="false" OnClientDropDownClosed="enablevalidation" OnClientLoad="enablevalidation">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="-Select Follow-up Required-" Value="2" Selected="true"/>
                                        <telerik:RadComboBoxItem Text="No" Value="0"/>
                                        <telerik:RadComboBoxItem Text="Yes" Value="1"/>
                                    </Items>   
                                    </telerik:RadComboBox>
                                </td>
                            </tr> 
                            <tr align="left">
                                <td width="130px">
                                    <label for ="rdFollowUpDate"> Follow-up Date</label>
                                </td>
                                <td>
                                    <asp:TextBox id="rdFollowUpDate" TabIndex="7" name="Followup_Date" runat="server" CssClass="datePicker" class="datePicker" value='<%# Eval("Followup_Date", "{0:M/dd/yyyy}") %>' />
                                    <%--<ajax:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="rdFollowUpDate" MaskType="Date" runat="server" Mask="99/99/9999" />--%>
                                    <asp:CompareValidator runat="server" ID="CompareValidator1" ControlToValidate="rdFollowUpDate" ErrorMessage="Please enter a valid follow-up date." Display="None" Type="Date" Operator="DataTypeCheck" />  
                                </td>
                            </tr>
                            
                            <tr align="left">
                                <td colspan="2">
                                    <label>Follow-up Topic</label>
                                </td>
                            </tr>
                            <tr align="left">
                                <td colspan="2" style="padding-left: 5px;">
                                    <telerik:RadComboBox ID="rdlMeetingOutCome" runat="server" AppendDataBoundItems="true" DataSourceID="dsMeetingOutcome"
                                         TabIndex="8" Skin="pathfinder" Width="343px" DropDownWidth="343px"  DataTextField ="Meeting_Outcome_Name"
                                        EnableEmbeddedSkins="false" DataValueField="Meeting_Outcome_ID" AllowCustomText="True" Text="-Select Follow-up Topic-"
                                        OnClientDropDownClosed="setMeetOutcome" OnClientLoad="setMeetOutcome">
                                        <ItemTemplate>
                                           <span id='<%# String.Format("m{0}",Eval("Meeting_Outcome_ID")) %>'>
                                           <asp:CheckBox runat="server" ID="chkMeetingOutcome" Text ='<%# Eval("Meeting_Outcome_Name") %>' onclick='<%# string.Format("MeetOutcomeChanged(this,{0})", Eval("Meeting_Outcome_ID")) %>' />
                                           </span>
                                        </ItemTemplate>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                           
                            <tr align="left">
                                <td colspan="2" style="padding-left: 5px;">
                                    <div id="spanMeetOut">                            
                                   </div>
                                </td>
                            </tr>
                            <tr align="left">
                                <td colspan="2">
                                    <label> Follow-up Contact Title</label>
                                </td>                                
                            </tr>
                            <tr align="left">
                                <td colspan="2" style="padding-left: 5px;">
                                    <telerik:RadComboBox ID="rdlcontacttitle" runat="server" AppendDataBoundItems="true" DataSourceID="dsContactTitle"
                                         TabIndex="9" Skin="pathfinder" Width="343px" DropDownWidth="343px"  DataTextField ="Followup_Contact_Title_Name"
                                        EnableEmbeddedSkins="false" DataValueField="Followup_Contact_Title_ID" AllowCustomText="True" Text="-Select Follow-up Contact Title-" 
                                        OnClientDropDownClosed="setcontacttitle" OnClientLoad="setcontacttitle">
                                        <ItemTemplate>
                                           <span id='<%# String.Format("t{0}",Eval("Followup_Contact_Title_ID")) %>'>
                                           <asp:CheckBox runat="server" ID="chkContactTitle" Text ='<%# Eval("Followup_Contact_Title_Name") %>' onclick='<%# string.Format("ContactTitleChanged(this,{0})", Eval("Followup_Contact_Title_ID")) %>' />
                                           </span>
                                        </ItemTemplate>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            
                            <tr align="left">
                                <td colspan="2" style="padding-left: 5px;">
                                    <div id="spancontacttitle">                            
                                   </div>
                                </td>
                            </tr>
                             
                        </table>
                    </td>                  
                </tr>                
            </table>            
            <pinso:ClientValidator ID= "RequiredVal" Target="rdCCRDate" Required ="true" Text="Please select a Date" runat="server"/>
            <asp:requiredfieldvalidator ID="Requiredfieldvalidator1" controltovalidate="rdCCRDate" display="none" errormessage="Date of Contact Required" runat="server" />
            <asp:requiredfieldvalidator ID="Requiredfieldvalidator6" controltovalidate="rdlMeetingActivity" display="none" InitialValue="-Select Meeting Topic-" errormessage="Meeting Topic Required" runat="server" />            
            <asp:requiredfieldvalidator ID="Requiredfieldvalidator2" controltovalidate="rdlMeetingType" display="none" InitialValue="-Select Meeting Type-" errormessage="Interaction Type Required" runat="server" />
            <asp:requiredfieldvalidator ID="Requiredfieldvalidator3" controltovalidate="rdlProductsDiscussed" display="none" InitialValue="-Select Products Discussed-" errormessage="Products Discussed Required" runat="server" />
             <asp:requiredfieldvalidator ID="Requiredfieldvalidator4" controltovalidate="rdlKeyContacts" display="none" InitialValue="-Select Attendees-" errormessage="Attendees Required" runat="server" />
            <asp:requiredfieldvalidator ID="Requiredfieldvalidator5" controltovalidate="rdlFollowupRequired" display="none" InitialValue="-Select Follow-up Required-" errormessage="Follow-up Required" runat="server" />
            <asp:requiredfieldvalidator ID="Requiredfieldvalidator7" controltovalidate="rdlMeetingOutCome" Enabled="false" display="none" InitialValue="-Select Follow-up Topic-" errormessage="Follow-up Topic Required" runat="server" />
            <asp:requiredfieldvalidator ID="Requiredfieldvalidator8" controltovalidate="rdlcontacttitle" Enabled="false" display="none" InitialValue="-Select Follow-up Contact Title-" errormessage="Contact Title Required" runat="server" />
            
            <%--<ajax:MaskedEditValidator runat="server" ID="MaskedEditValidator1" ControlExtender="MaskedEditExtender1" ControlToValidate="rdCCRDate" InvalidValueMessage='<%$ Resources:Resource, Label_Invalid_Date %>' />--%> 
            <%--<ajax:MaskedEditValidator runat="server" ID="MaskedEditValidator2" ControlExtender="MaskedEditExtender2" ControlToValidate="rdFollowUpDate" InvalidValueMessage='<%$ Resources:Resource, Label_Invalid_Date %>' />--%> 
            <%--<asp:requiredfieldvalidator ID="Requiredfieldvalidator3" controltovalidate="rdFollowUpDate" display="none" errormessage="Follow Up Date Required" runat="server" />--%>
            <asp:CompareValidator ID="comparefieldvalidator4" ControlToCompare="rdCCRDate" operator="GreaterThan" type="Date" ControlToValidate="rdFollowupDate" Display="None" ErrorMessage="Follow-up Date must be greater than the Customer Date of Contact" runat="server" />
            <div style="height:15px">
                <asp:ValidationSummary runat="server" ID="validationSummary" DisplayMode="List" />
            </div>
            <div class="modalFormButtons">
                 <pinso:CustomButton TabIndex="10" ID="btnEdit" runat="server" Text="Update" Visible="true" OnClick="Editbtn_Click" />
                 <pinso:CustomButton ID="btnReset" TabIndex="11" runat="server" Text="Reset" OnClientClick="javascript:ClearForm(); return false;" />                     
            </div>
        </ItemTemplate>
        <InsertItemTemplate>
          <table width="100%" >
                <tr>
                    <td valign="top">
                        <table align="left" >
                            <tr align="left">
                                <td width="140px">
                                    <label for="rdCCRDate">Date of Contact *</label>
                                </td>
                                <td>
                                    <asp:TextBox id="rdCCRDate" TabIndex="1" name="Contact_Date" runat="server" CssClass="datePicker" class="datePicker" style="margin-bottom: 2px;" value='<%# Eval("Contact_Date", "{0:M/dd/yyyy}") %>' />
                                    <%--<ajax:MaskedEditExtender ID="MaskedEditExtender1" TargetControlID="rdCCRDate" MaskType="Date" runat="server" Mask="99/99/9999" />--%>
                                    <asp:CompareValidator runat="server" ID="compareDate" ControlToValidate="rdCCRDate" ErrorMessage="Please enter a valid report date." Display="None" Type="Date" Operator="DataTypeCheck" />   
                                </td>
                            </tr>
                            <tr align="left">
                                <td width="130px"> 
                                    <label for="rdlMeetingType">Interaction Type *</label>
                                </td>
                                <td>
                                    <telerik:RadComboBox TabIndex="2" ID="rdlMeetingType" runat="server" AppendDataBoundItems="true"
                                        DataSourceID="dsMeetingType" Skin="pathfinder" Width="220px"
                                        DropDownWidth="220px"  EnableEmbeddedSkins="false" DataTextField="Meeting_Type_Name"
                                        DataValueField="Meeting_Type_ID" SelectedValue='<%# Eval("Meeting_Type_ID") %>'  >
                                    <Items>
                                        <telerik:RadComboBoxItem Text="-Select Meeting Type-" Value="0" Selected="true"/>
                                    </Items>   
                                    </telerik:RadComboBox>
                                </td>
                            </tr> 
                            <tr align="left">
                                <td> 
                                    <label for="rdlMeetingActivity">Meeting Topic *</label>
                                </td>
                                <td>
                                    <telerik:RadComboBox runat="server" ID="rdlMeetingActivity" DataSourceID="dsMeetingActivity"
                                        TabIndex="3" Skin="pathfinder" Width="220px" DropDownWidth="220px"
                                        EnableEmbeddedSkins="false" DataTextField="Meeting_Activity_Name" DataValueField="Meeting_Activity_ID"
                                        SelectedValue='<%# Eval("Meeting_Activity_ID") %>' AppendDataBoundItems="true" >
                                    <Items>
                                        <telerik:RadComboBoxItem Text="-Select Meeting Topic-" Value="0" Selected="true"/>
                                    </Items>   
                                    </telerik:RadComboBox>
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
                                               <asp:CheckBox runat="server" ID="chkProductDiscussed" Text ='<%# Eval("Drug_Name") %>'  Checked="true"
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
                          
                            <tr align="left">
                                <td width="130px">                                    
                                </td>
                                <td>
                                </td>
                            </tr>  
                            <tr align="left">
                                <td colspan="2">
                                    <label>Attendees *</label>
                                </td>                                
                            </tr>
                            <tr align="left">
                                <td colspan="2" style="padding-left: 5px;">
                                    <telerik:RadComboBox ID="rdlKeyContacts" runat="server" AppendDataBoundItems="true"
                                        TabIndex="5" Skin="pathfinder" Width="343px" DropDownWidth="343px" DataSourceID="dsPersonsMet" 
                                        DataTextField="Full_Name" DataValueField ="KC_ID" EnableEmbeddedSkins="false" AllowCustomText="True" Text="-Select Attendees-" 
                                        OnClientDropDownClosed="setPersonsMetText" OnClientLoad="setPersonsMetText">
                                       <ItemTemplate>
                                        <span id='<%# String.Format("k{0}",Eval("KC_ID")) %>'>
                                            <asp:CheckBox runat="server" ID="chkKeyContact" Text ='<%# Eval("Full_Name") %>' onclick='<%# string.Format("KeyContactChanged(this,{0})", Eval("Full_ID")) %>' />
                                        </span>
                                        </ItemTemplate>
                                    </telerik:RadComboBox>
                                </td>
                            </tr> 
                             <tr align="left">
                                <td colspan="2" style="padding-left: 5px;">
                                    <div id="spanKeyContacts">                          
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
                                <td width="130px"> 
                                    <label for="rdlFollowupRequired">Follow-up Required?  *</label>
                                </td>
                                <td>
                                    <telerik:RadComboBox TabIndex="6" ID="rdlFollowupRequired" runat="server" AppendDataBoundItems="true"
                                         Skin="pathfinder" Width="220px" SelectedValue='<%# Eval("followup_requires") %>'
                                        DropDownWidth="220px"  EnableEmbeddedSkins="false" OnClientDropDownClosed="enablevalidation" OnClientLoad="enablevalidation">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="-Select Follow-up Required-" Value="2" Selected="true"/>
                                        <telerik:RadComboBoxItem  Text="No" Value="0"/>
                                        <telerik:RadComboBoxItem  Text="Yes" Value="1"/>
                                    </Items>   
                                    </telerik:RadComboBox>
                                </td>
                            </tr> 
                           
                           
                            <tr align="left">
                                <td width="130px">
                                    <label for ="rdFollowUpDate"> Follow-up Date</label>
                                </td>
                                <td>
                                    <asp:TextBox id="rdFollowUpDate" TabIndex="7" name="Followup_Date" runat="server" CssClass="datePicker" class="datePicker" value='<%# Eval("Followup_Date", "{0:M/dd/yyyy}") %>' />
                                    <%--<ajax:MaskedEditExtender ID="MaskedEditExtender2" TargetControlID="rdFollowUpDate" MaskType="Date" runat="server" Mask="99/99/9999" />--%>
                                     <asp:CompareValidator runat="server" ID="CompareValidator1" ControlToValidate="rdFollowUpDate" ErrorMessage="Please enter a valid follow-up date." Display="None" Type="Date" Operator="DataTypeCheck" />  
                                </td>
                            </tr>
                             <tr align="left">
                                <td width="130px" colspan="2">
                                    <label>Follow-up Topic</label>
                                </td>
                            </tr>
                            <tr align="left">
                                <td colspan="2" style="padding-left: 5px;">
                                    <telerik:RadComboBox ID="rdlMeetingOutCome" runat="server" AppendDataBoundItems="true" DataSourceID="dsMeetingOutcome"
                                        TabIndex="8" Skin="pathfinder" Width="343px" DropDownWidth="343px"  DataTextField ="Meeting_Outcome_Name"
                                        EnableEmbeddedSkins="false" DataValueField="Meeting_Outcome_ID" AllowCustomText="True" Text="-Select Follow-up Topic-" 
                                        OnClientDropDownClosed="setMeetOutcome" OnClientLoad="setMeetOutcome">
                                        <ItemTemplate>
                                           <span id='<%# String.Format("m{0}",Eval("Meeting_Outcome_ID")) %>'>
                                           <asp:CheckBox runat="server" ID="chkMeetingOutcome" Text ='<%# Eval("Meeting_Outcome_Name") %>' onclick='<%# string.Format("MeetOutcomeChanged(this,{0})", Eval("Meeting_Outcome_ID")) %>' />
                                           </span>
                                        </ItemTemplate>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                         
                            <tr align="left">
                                <td colspan="2" style="padding-left: 5px;">
                                    <div id="spanMeetOut">                            
                                   </div>
                                </td>
                            </tr>
                         
                             <tr align="left">
                                <td colspan="2">
                                    <label> Follow-up Contact Title</label>
                                </td>                                
                            </tr>
                            <tr align="left">
                                <td colspan="2" style="padding-left: 5px;">
                                    <telerik:RadComboBox ID="rdlcontacttitle" runat="server" AppendDataBoundItems="true" DataSourceID="dsContactTitle"
                                         TabIndex="9" Skin="pathfinder" Width="343px" DropDownWidth="343px"  DataTextField ="Followup_Contact_Title_Name"
                                        EnableEmbeddedSkins="false" DataValueField="Followup_Contact_Title_ID" AllowCustomText="True" Text="-Select Follow-up Contact Title-" 
                                         OnClientDropDownClosed="setcontacttitle" OnClientLoad="setcontacttitle">
                                        <ItemTemplate>
                                           <span id='<%# String.Format("t{0}",Eval("Followup_Contact_Title_ID")) %>'>
                                           <asp:CheckBox runat="server" ID="chkContactTitle" Text ='<%# Eval("Followup_Contact_Title_Name") %>' onclick='<%# string.Format("ContactTitleChanged(this,{0})", Eval("Followup_Contact_Title_ID")) %>' />
                                           </span>
                                        </ItemTemplate>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            
                            <tr align="left">
                                <td colspan="2" style="padding-left: 5px;">
                                    <div id="spancontacttitle">                            
                                   </div>
                                </td>
                            </tr>
                             
                        </table>
                    </td>                  
                </tr>                
            </table>           
            <pinso:ClientValidator ID= "RequiredVal" Target="rdCCRDate" Required ="true" Text="Please select a Date" runat="server"/>
            <asp:requiredfieldvalidator ID="Requiredfieldvalidator1" controltovalidate="rdCCRDate" display="none" errormessage="Date of Contact Required" runat="server" />
            <asp:requiredfieldvalidator ID="Requiredfieldvalidator6" controltovalidate="rdlMeetingActivity" display="none" InitialValue="-Select Meeting Topic-" errormessage="Meeting Topic Required" runat="server" />            
            <asp:requiredfieldvalidator ID="Requiredfieldvalidator2" controltovalidate="rdlMeetingType" display="none" InitialValue="-Select Meeting Type-" errormessage="Interaction Type Required" runat="server" />
            <asp:requiredfieldvalidator ID="Requiredfieldvalidator3" controltovalidate="rdlProductsDiscussed" display="none" InitialValue="-Select Products Discussed-" errormessage="Products Discussed Required" runat="server" />
            <asp:requiredfieldvalidator ID="Requiredfieldvalidator4" controltovalidate="rdlKeyContacts" display="none" InitialValue="-Select Attendees-" errormessage="Attendees Required" runat="server" />
            <asp:requiredfieldvalidator ID="Requiredfieldvalidator5" controltovalidate="rdlFollowupRequired" display="none" InitialValue="-Select Follow-up Required-" errormessage="Follow-up Required" runat="server" />
            <asp:requiredfieldvalidator ID="Requiredfieldvalidator7" Enabled="false" controltovalidate="rdlMeetingOutCome" display="none" InitialValue="-Select Follow-up Topic-" errormessage="Follow-up Topic Required" runat="server" />
            <asp:requiredfieldvalidator ID="Requiredfieldvalidator8" Enabled="false" controltovalidate="rdlcontacttitle" display="none" InitialValue="-Select Follow-up Contact Title-" errormessage="Contact Title Required" runat="server" />
            
            <%--<ajax:MaskedEditValidator runat="server" ID="MaskedEditValidator1" ControlExtender="MaskedEditExtender1" ControlToValidate="rdCCRDate" InvalidValueMessage='<%$ Resources:Resource, Label_Invalid_Date %>' />--%> 
            <%--<ajax:MaskedEditValidator runat="server" ID="MaskedEditValidator2" ControlExtender="MaskedEditExtender2" ControlToValidate="rdFollowUpDate" InvalidValueMessage='<%$ Resources:Resource, Label_Invalid_Date %>' />--%> 
            <%--<asp:requiredfieldvalidator ID="Requiredfieldvalidator3" controltovalidate="rdFollowUpDate" display="none" errormessage="Follow Up Date Required" runat="server" />--%>
            <asp:CompareValidator ID="comparefieldvalidator4" ControlToCompare="rdCCRDate" operator="GreaterThan" type="Date" ControlToValidate="rdFollowupDate" Display="None" ErrorMessage="Follow-up Date must be greater than the Customer Date of Contact" runat="server" />
            <div style="height:15px">
                <asp:ValidationSummary runat="server" ID="validationSummary" DisplayMode="List" />
            </div>
            <div class="modalFormButtons">                
                <pinso:CustomButton TabIndex="10" ID="btnEdit" runat="server" Text="Add" Visible="true" OnClick="Editbtn_Click" />
                <pinso:CustomButton ID="btnReset" TabIndex="11" runat="server" Text="Reset" OnClientClick="javascript:ClearForm(); return false;" />                       
            </div>
        </InsertItemTemplate>
    </asp:FormView>
    </div>
    <asp:EntityDataSource runat="server" ID="dsMeetingType" ConnectionString="name=PathfinderMillenniumEntities"
        DefaultContainerName="PathfinderMillenniumEntities" EntitySetName="LkpMeetingTypeSet"
        EntityTypeFilter="LkpMeetingType" OrderBy="it.Sort_Index, it.[Meeting_Type_Name]">
    </asp:EntityDataSource>
    <asp:EntityDataSource runat="server" ID="dsMeetingActivity" ConnectionString="name=PathfinderMillenniumEntities"
        DefaultContainerName="PathfinderMillenniumEntities" 
        EntitySetName="LkpMeetingActivitySet" OrderBy="it.Sort_Index, it.[Meeting_Activity_Name]">
    </asp:EntityDataSource>
    <asp:EntityDataSource runat="server" ID="dsProductsDiscussed" ConnectionString="name=PathfinderMillenniumEntities"
        DefaultContainerName="PathfinderMillenniumEntities" EntitySetName="LkpProductsDiscussedSet"
        EntityTypeFilter="LkpProductsDiscussed" OrderBy="it.Sort_Index, it.[Drug_Name]">
    </asp:EntityDataSource>
    <asp:EntityDataSource runat="server" ID="dsContactReport" ConnectionString="name=PathfinderMillenniumEntities"
        DefaultContainerName="PathfinderMillenniumEntities" 
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
    
    <asp:EntityDataSource runat ="server" ID ="dsMeetingOutcome" 
        ConnectionString="name=PathfinderMillenniumEntities" 
        DefaultContainerName="PathfinderMillenniumEntities" 
        EntitySetName="LkpMeetingOutcomeSet" OrderBy="it.Sort_Index, it.[Meeting_Outcome_Name]">
    </asp:EntityDataSource>
    <asp:EntityDataSource runat ="server" ID ="dsContactTitle" 
        ConnectionString="name=PathfinderMillenniumEntities" 
        DefaultContainerName="PathfinderMillenniumEntities" 
        EntitySetName="LkpFollowupContactTitleSet" OrderBy="it.Sort_Index, it.[Followup_Contact_Title_Name]">
    </asp:EntityDataSource>
   
   <asp:SqlDataSource ID="dsPersonsMet" runat="server"  
        ConnectionString="<%$ ConnectionStrings:PathfinderClientDB_Format %>" 
        SelectCommand="usp_GetAvailableContactsForCCR"
        SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter  Name="Plan_ID" QueryStringField="PlanID" DbType ="Int32"  />
            <%--<asp:QueryStringParameter  Name="ccrID" QueryStringField="CRID" DefaultValue="0" />--%>
        </SelectParameters>  
    </asp:SqlDataSource> 
    <div>
        <asp:Label ID="Msglbl" runat="server" Text="" Visible="false"></asp:Label>
        <asp:Label ID="Label2" runat="server" Text='<%= Request.Form["PlanID"]%>' Visible="false"></asp:Label>
    </div>
</asp:Content>
