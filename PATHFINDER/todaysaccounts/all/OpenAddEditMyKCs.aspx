<%@ Page Language="C#" AutoEventWireup="true" Theme="pathfinder" MasterPageFile="~/MasterPages/Modal.master"  EnableViewState="true" CodeFile="OpenAddEditMyKCs.aspx.cs" Inherits="todaysaccounts_all_OpenAddEditMyKCs" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@ OutputCache Duration="1" VaryByParam="None" NoStore="true" %>

<asp:Content runat="server" ContentPlaceHolderID="head">

     <script type="text/javascript">
         function ClearForm()
         {
             $("#AddKCMain input[type != submit]").val("");
             $("TEXTAREA").val("");
         }
         
         function RefreshMyKCs()
         {
             getMyKCGrid().control.get_masterTableView().rebind();

             window.setTimeout(CloseWin, 4000);  
         }

         function CloseWin() {
             var manager = window.top.GetRadWindowManager();

             var window1 = manager.getWindowByName("AddKC");
             if (window1 != null)
             { window1.close(); }

             var window2 = manager.getWindowByName("UpdKC");
             if (window2 != null)
             { window2.close(); }

             var window3 = manager.getWindowByName("ViewKC");
             if (window3 != null)
             { window3.close(); }
         }
         function getMyKCGrid()
         {
             return window.top.$get("ctl00_Tile4_MyKeyContactsList1_gridMyContacts");
         }
    </script>  
</asp:Content>

<asp:Content runat="server" ID="title" ContentPlaceHolderID="title">
    <asp:Literal runat="server" id="titleText" />
</asp:Content>


<asp:Content runat="server" ID="main" ContentPlaceHolderID="main">

<%-- This page displays appropriate form(View/Add/Edit) for my key contacts based on user action.--%>  

  <asp:HiddenField ID="PlanNameHdn" runat="server" Value="" Visible="false" /> 
       
    <div id="AddKCMain">
         
    <asp:FormView  runat="server" ID="formViewKC" DefaultMode="Insert" 
           DataSourceID="dsKeyContacts" CellPadding="0" CellSpacing="0" Width="100%" DataKeyNames="KC_ID">
        <ItemTemplate>
                    <table class="genTable" cellpadding="0" cellspacing="0" border="0">
                     <tr>
                        <td><label>Prefix</label><%# Eval("KC_Prefix") %>&nbsp;</td>
                        <td><label for="">Address 1</label><%# Eval("KC_Address1") %>&nbsp;</td>
                    </tr>
                     <tr>
                        <td><label>First Name</label><%# Eval("KC_F_Name") %>&nbsp;</td>
                        <td><label>Address 2</label><%# Eval("KC_Address2") %>&nbsp;</td>
                    </tr>
                     <tr>
                        <td><label>Last Name</label><%# Eval("KC_L_Name") %>&nbsp;</td>
                        <td><label>City</label><%# Eval("KC_City") %>&nbsp;</td>
                    </tr>
                     <tr>
                        <td><label>Suffix</label><%# Eval("KC_Suffix") %>&nbsp;</td>
                        <td><label>State</label><%# Eval("KC_State") %>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<label>ZIP</label><%# Eval("KC_Zip") %>&nbsp;</td>
                    </tr>
                     <tr>
                        <td><label>Designation</label><%# Eval("KC_Title_Name")%>&nbsp;</td>
                        <td><label>Phone</label><%# Eval("KC_Phone") %>&nbsp;</td>
                    </tr>
                     <tr>
                        <td><label>Title</label><%# Eval("KC_Role")%>&nbsp;</td>
                        <td><label>Email</label><%# Eval("KC_Email") %>&nbsp;</td>
                    </tr>
                     <tr>
                        <td><label>Mobile</label><%# Eval("KC_Mobile") %>&nbsp;</td>
                        <td><label>Fax</label><%# Eval("KC_Fax") %>&nbsp;</td>
                    </tr>
                    </table>

                    <div class="tileContainerHeader" >
                    <div class="title">
                    <asp:Literal runat="server" ID="Literal2" Text='Assistant Details...' /></div>
                    </div>
                    <table class="genTable" cellpadding="0" cellspacing="0" border="0">
                     <tr>
                        <td><label>Name</label><%# Eval("KC_Admin_Name") %>&nbsp;</td>
                        <td rowspan="3"><label>Comments:</label><%# Eval("KC_Comments") %>&nbsp;</td>
                    </tr>
                     <tr>
                        <td><label>Office Phone</label><%# Eval("KC_Admin_PH") %>&nbsp;</td>
                    </tr>
                     <tr>
                        <td><label>Email</label><%# Eval("KC_Admin_Email") %>&nbsp;</td>
                    </tr>
                    </table>
               <%-- <div>
                    <%# Eval("Last_Update_DT", Resources.Resource.Label_Section_Last_Updated)%>
                </div>    --%>                  
                    <div class="modalFormButtons">
<%--             <span class="coreBtn">
                 <span class="bg">
                        <span class="bg2">
                    <asp:Button ID="UpdDelViewbtn" runat="server" Text="Update / Delete" Visible="true"  onclick="UpdDelViewbtn_Click" />
                        </span>
                    </span>
                </span>
--%>                
            </div>
          
        </ItemTemplate> 
        
        <EditItemTemplate>
                    <table class="genTable" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td>
                            <label for="Prefixtxt">Prefix</label>
                            <asp:TextBox TabIndex="1" ID="Prefixtxt" runat="server" Width="57px" Text='<%# Bind("KC_Prefix") %>'></asp:TextBox>
                        </td>
                        <td>
                            <label for="Addr1txt">Address 1</label>
                            <asp:TextBox TabIndex="11" ID="Addr1txt" runat="server" Width="331px" Text='<%# Bind("KC_Address1") %>'></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td><label for="FNametxt">First Name *</label>
                            <asp:TextBox TabIndex="2" ID="FNametxt" runat="server" Width="237px" Text='<%# Bind("KC_F_Name") %>'></asp:TextBox>
                        </td>
                        <td>
                            <label for="Addr2txt">Address 2</label>
                            <asp:TextBox TabIndex="12" ID="Addr2txt" runat="server" Width="330px" Text='<%# Bind("KC_Address2") %>'></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td><label for="LNametxt">Last Name *</label>
                            <asp:TextBox TabIndex="3" ID="LNametxt" runat="server" Width="237px" Text='<%# Bind("KC_L_Name") %>'></asp:TextBox>
                        
                        </td>
                        <td><label for="Citytxt">City</label>
                            <asp:TextBox TabIndex="13" ID="Citytxt" runat="server" Width="331px" Text='<%# Bind("KC_City") %>'></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td><label for="Suffixtxt">Suffix</label>
                            <asp:TextBox TabIndex="4" ID="Suffixtxt" runat="server" Width="57px" Text='<%# Bind("KC_Suffix") %>'></asp:TextBox>
                        </td>
                        <td><label for="rdlStates">State</label>
                                <telerik:RadComboBox TabIndex="14" EnableEmbeddedSkins="false" SkinID="planInfoCombo" Skin="pathfinder" runat="server" ID="rdlStates" DataSourceID="dsStates" Width="120px" DropDownWidth="180px" DataTextField="Name" DataValueField="ID" AppendDataBoundItems="true" MaxHeight="200" SelectedValue='<%# Bind("KC_State") %>'>
                                    <Items>
                                        <telerik:RadComboBoxItem Value="" Text="" />
                                    </Items>
                                </telerik:RadComboBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<label for="Ziptxt">ZIP</label>
                                <asp:TextBox TabIndex="15" ID="Ziptxt" runat="server" Width="57px" Text='<%# Bind("KC_Zip") %>'></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td><label for="rdcmbDesg">Designation *</label>
                                <telerik:RadComboBox TabIndex="5" ID="rdcmbDesg" DataSourceID="dsTitles" DataTextField="Name" DataValueField="ID" runat="server" Width="65%" EnableEmbeddedSkins="false" SkinID="planInfoCombo" Skin="pathfinder" AppendDataBoundItems="true" SelectedValue='<%# Bind("KC_Title_ID") %>'>
                                    <Items>
                                        <telerik:RadComboBoxItem Value="" Text="" />
                                    </Items>
                                </telerik:RadComboBox> 
                        </td>
                        <td><label for="Ph1txt">Phone</label>
                            <asp:TextBox TabIndex="16" ID="Ph1txt" runat="server" Width="111px" Text='<%# Bind("KC_Phone") %>'></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td><label for="Titletxt">Title</label>
                            <asp:TextBox TabIndex="6" ID="Titletxt" runat="server" Width="330px" Text='<%# Bind("KC_Role") %>'></asp:TextBox>
                        </td>
                        <td><label for="Mobiletxt">Mobile</label>
                            <asp:TextBox TabIndex="17" ID="Mobiletxt" runat="server" Width="111px" Text='<%# Bind("KC_Mobile") %>'></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td><label for="Email1txt">Email</label>
                            <asp:TextBox TabIndex="7" ID="Email1txt" runat="server" Width="331px" Text='<%# Bind("KC_Email") %>'></asp:TextBox>
                        </td>
                        <td><label for="Faxtxt">Fax</label>
                            <asp:TextBox ID="Faxtxt" TabIndex="18" runat="server" Width="111px" Text='<%# Bind("KC_Fax") %>'></asp:TextBox>
                        </td>
                    </tr>
                    </table>

                    <div class="tileContainerHeader" >
                    <div class="title">
                    <asp:Literal runat="server" ID="Literal2" Text='Assistant Details...' /></div>
                    </div>
                    <table class="genTable" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td><label for="AsstNmtxt">Name</label>
                            <asp:TextBox TabIndex="8" ID="AsstNmtxt" runat="server" Width="237px" Text='<%# Bind("KC_Admin_Name") %>'></asp:TextBox>
                        </td>
                        <td rowspan="3"><label for="Cmtstxt">Comments</label>
                            <asp:TextBox TabIndex="19" ID="Cmtstxt" runat="server" Height="50px" Rows="2" TextMode="MultiLine" Width="310px" Text='<%# Bind("KC_Comments") %>'></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td><label for="AsstPh1txt">Office Phone</label>
                            <asp:TextBox TabIndex="9" ID="AsstPh1txt" runat="server" Width="111px" Text='<%# Bind("KC_Admin_PH") %>'></asp:TextBox>
                        </td>
                    </tr>
                     <tr>
                        <td><label for="AsstEmailtxt">Email</label>
                            <asp:TextBox TabIndex="10" ID="AsstEmailtxt" runat="server" Width="331px" Text='<%# Bind("KC_Admin_Email") %>'></asp:TextBox>
                        </td>
                    </tr>
                    </table>                                                          
           <%-- <div>
                <%# Eval("Last_Update_DT", Resources.Resource.Label_Section_Last_Updated)%>
            </div> --%>     
            
                            <pinso:MaxLengthValidator ID="MaxLengthValidator1" runat="server" MaxLength="50" ErrorMessage="&bull;&nbsp;Prefix field is limited to 50 characters." Display="None" ControlToValidate="Prefixtxt" />                            
                            <asp:requiredfieldvalidator ID="Requiredfieldvalidator4" controltovalidate="FNametxt" display="none" errormessage='<%$ Resources:Resource, Message_Required_FirstName %>' runat="server" />
                            <pinso:MaxLengthValidator ID="MaxLengthValidator5" runat="server" MaxLength="255" ErrorMessage="&bull;&nbsp;First Name field is limited to 255 characters." Display="None" ControlToValidate="FNametxt" />                            
                            <pinso:MaxLengthValidator ID="MaxLengthValidator4" runat="server" MaxLength="255" ErrorMessage="&bull;&nbsp;Last Name field is limited to 255 characters." Display="None" ControlToValidate="LNametxt" />
                            <asp:requiredfieldvalidator ID="Requiredfieldvalidator5" controltovalidate="LNametxt" display="none" errormessage='<%$ Resources:Resource, Message_Required_LastName %>' runat="server" />                                             
                            <asp:requiredfieldvalidator ID="Requiredfieldvalidator6" controltovalidate="rdcmbDesg" display="none" errormessage='<%$ Resources:Resource, Message_Required_Designation %>' runat="server" />                           
                            <pinso:MaxLengthValidator ID="MaxLengthValidator9" runat="server" MaxLength="255" ErrorMessage="&bull;&nbsp;Title field is limited to 255 characters." Display="None" ControlToValidate="Titletxt" />                            
                            <pinso:MaxLengthValidator ID="MaxLengthValidator11" runat="server" MaxLength="255" ErrorMessage="&bull;&nbsp;Email field is limited to 255 characters." Display="None" ControlToValidate="Email1txt" />                                                                                    
                            <pinso:MaxLengthValidator ID="MaxLengthValidator2" runat="server" MaxLength="255" ErrorMessage="&bull;&nbsp;Address 1 field is limited to 255 characters." Display="None" ControlToValidate="Addr1txt" />                            
                            <pinso:MaxLengthValidator ID="MaxLengthValidator3" runat="server" MaxLength="255" ErrorMessage="&bull;&nbsp;Address 2 field is limited to 255 characters." Display="None" ControlToValidate="Addr2txt" />                            
                            <pinso:MaxLengthValidator ID="MaxLengthValidator6" runat="server" MaxLength="50" ErrorMessage="&bull;&nbsp;City field is limited to 50 characters." Display="None" ControlToValidate="Citytxt" />                            
                            <pinso:MaxLengthValidator ID="MaxLengthValidator7" runat="server" MaxLength="50" ErrorMessage="&bull;&nbsp;Suffix field is limited to 50 characters." Display="None" ControlToValidate="Suffixtxt" />                                
                            <pinso:MaxLengthValidator ID="MaxLengthValidator16" runat="server" MaxLength="10" ErrorMessage="&bull;&nbsp;ZIP field is limited to 10 characters." Display="None" ControlToValidate="Ziptxt" />                                                                                        
                            <pinso:MaxLengthValidator ID="MaxLengthValidator8" runat="server" MaxLength="50" ErrorMessage="&bull;&nbsp;Phone field is limited to 50 characters." Display="None" ControlToValidate="Ph1txt" />                                                        
                            <pinso:MaxLengthValidator ID="MaxLengthValidator10" runat="server" MaxLength="50" ErrorMessage="&bull;&nbsp;Mobile field is limited to 50 characters." Display="None" ControlToValidate="Mobiletxt" />                            
                            <pinso:MaxLengthValidator ID="MaxLengthValidator12" runat="server" MaxLength="50" ErrorMessage="&bull;&nbsp;Fax field is limited to 50 characters." Display="None" ControlToValidate="Faxtxt" />                            
                            <pinso:MaxLengthValidator ID="MaxLengthValidator13" runat="server" MaxLength="255" ErrorMessage="&bull;&nbsp;Assistant Name field is limited to 255 characters." Display="None" ControlToValidate="AsstNmtxt" />                            
                            <pinso:MaxLengthValidator ID="MaxLengthValidator18" runat="server" MaxLength="1000" ErrorMessage="&bull;&nbsp;Comments field is limited to 1000 characters." Display="None" ControlToValidate="Cmtstxt" />                            
                            <pinso:MaxLengthValidator ID="MaxLengthValidator15" runat="server" MaxLength="50" ErrorMessage="&bull;&nbsp;Office Phone field is limited to 50 characters." Display="None" ControlToValidate="AsstPh1txt" />                            
                            <pinso:MaxLengthValidator ID="MaxLengthValidator14" runat="server" MaxLength="50" ErrorMessage="&bull;&nbsp;Assistant Email field is limited to 50 characters." Display="None" ControlToValidate="AsstEmailtxt" />
                                        
                    <div style="height:15px">
                        <asp:ValidationSummary runat="server" ID="validationSummary" DisplayMode="SingleParagraph" />
                    </div>
           
            <div class="modalFormButtons">              
                  <pinso:CustomButton TabIndex="20" ID="Updbtn" runat="server" Text="Update" width="50px" Visible="true" CommandName="Update" />               
                  <pinso:CustomButton TabIndex="21" ID="Delbtn" runat="server" Text="Delete" width="50px" Visible="true" onclick="Delbtn_Click"/>                     
                  <pinso:CustomButton TabIndex="22" ID="Resetbtn" width="50px" runat="server" Text="Reset" OnClientClick="javascript:ClearForm(); return false;"/>                   
            </div>
         
        </EditItemTemplate>
        
        <InsertItemTemplate>
                    <table class="genTable" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td>
                            <label for="Prefixtxt">Prefix</label>
                            <asp:TextBox TabIndex="1" ID="Prefixtxt" runat="server" Width="57px" Text='<%# Bind("KC_Prefix") %>'></asp:TextBox>
                        </td>
                        <td>
                            <label for="Addr1txt">Address 1</label>
                            <asp:TextBox TabIndex="11" ID="Addr1txt" runat="server" Width="331px" Text='<%# Bind("KC_Address1") %>'></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td><label for="FNametxt">First Name *</label>
                            <asp:TextBox TabIndex="2" ID="FNametxt" runat="server" Width="237px" Text='<%# Bind("KC_F_Name") %>'></asp:TextBox>
                        </td>
                        <td>
                            <label for="Addr2txt">Address 2</label>
                            <asp:TextBox TabIndex="12" ID="Addr2txt" runat="server" Width="330px" Text='<%# Bind("KC_Address2") %>'></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td><label for="LNametxt">Last Name *</label>
                            <asp:TextBox TabIndex="3" ID="LNametxt" runat="server" Width="237px" Text='<%# Bind("KC_L_Name") %>'></asp:TextBox>
                        
                        </td>
                        <td><label for="Citytxt">City</label>
                            <asp:TextBox TabIndex="13" ID="Citytxt" runat="server" Width="331px" Text='<%# Bind("KC_City") %>'></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td><label for="Suffixtxt">Suffix</label>
                            <asp:TextBox TabIndex="4" ID="Suffixtxt" runat="server" Width="57px" Text='<%# Bind("KC_Suffix") %>'></asp:TextBox>
                        </td>
                        <td><label for="rdlStates">State</label>
                                <telerik:RadComboBox TabIndex="14" EnableEmbeddedSkins="false" SkinID="planInfoCombo" Skin="pathfinder" runat="server" ID="rdlStates" DataSourceID="dsStates" Width="120px" DropDownWidth="180px" DataTextField="Name" DataValueField="ID" AppendDataBoundItems="true" MaxHeight="200" SelectedValue='<%# Bind("KC_State") %>'>
                                    <Items>
                                        <telerik:RadComboBoxItem Value="" Text="" />
                                    </Items>
                                </telerik:RadComboBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<label for="Ziptxt">ZIP</label>
                                <asp:TextBox TabIndex="15" ID="Ziptxt" runat="server" Width="57px" Text='<%# Bind("KC_Zip") %>'></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td><label for="rdcmbDesg">Designation *</label>
                                <telerik:RadComboBox TabIndex="5" ID="rdcmbDesg" DataSourceID="dsTitles" DataTextField="Name" DataValueField="ID" runat="server" Width="65%" EnableEmbeddedSkins="false" SkinID="planInfoCombo" Skin="pathfinder" AppendDataBoundItems="true" SelectedValue='<%# Bind("KC_Title_ID") %>'>
                                    <Items>
                                        <telerik:RadComboBoxItem Value="" Text="" />
                                    </Items>
                                </telerik:RadComboBox> 
                        </td>
                        <td><label for="Ph1txt">Phone</label>
                            <asp:TextBox TabIndex="16" ID="Ph1txt" runat="server" Width="111px" Text='<%# Bind("KC_Phone") %>'></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td><label for="Titletxt">Title</label>
                            <asp:TextBox TabIndex="6" ID="Titletxt" runat="server" Width="330px" Text='<%# Bind("KC_Role") %>'></asp:TextBox>
                        </td>
                        <td><label for="Mobiletxt">Mobile</label>
                            <asp:TextBox TabIndex="17" ID="Mobiletxt" runat="server" Width="111px" Text='<%# Bind("KC_Mobile") %>'></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td><label for="Email1txt">Email</label>
                            <asp:TextBox TabIndex="7" ID="Email1txt" runat="server" Width="331px" Text='<%# Bind("KC_Email") %>'></asp:TextBox>
                        </td>
                        <td><label for="Faxtxt">Fax</label>
                            <asp:TextBox ID="Faxtxt" TabIndex="18" runat="server" Width="111px" Text='<%# Bind("KC_Fax") %>'></asp:TextBox>
                        </td>
                    </tr>
                    </table>

                    <div class="tileContainerHeader" >
                    <div class="title">
                    <asp:Literal runat="server" ID="Literal2" Text='Assistant Details...' /></div>
                    </div>
                    <table class="genTable" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td><label for="AsstNmtxt">Name</label>
                            <asp:TextBox TabIndex="8" ID="AsstNmtxt" runat="server" Width="237px" Text='<%# Bind("KC_Admin_Name") %>'></asp:TextBox>
                        </td>
                        <td rowspan="3"><label for="Cmtstxt">Comments</label>
                            <asp:TextBox TabIndex="19" ID="Cmtstxt" runat="server" Height="50px" Rows="2" TextMode="MultiLine" Width="310px" Text='<%# Bind("KC_Comments") %>'></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td><label for="AsstPh1txt">Office Phone</label>
                            <asp:TextBox TabIndex="9" ID="AsstPh1txt" runat="server" Width="111px" Text='<%# Bind("KC_Admin_PH") %>'></asp:TextBox>
                        </td>
                    </tr>
                     <tr>
                        <td><label for="AsstEmailtxt">Email</label>
                            <asp:TextBox TabIndex="10" ID="AsstEmailtxt" runat="server" Width="331px" Text='<%# Bind("KC_Admin_Email") %>'></asp:TextBox>
                        </td>
                    </tr>
                    </table>                                                          
   
            
                            <pinso:MaxLengthValidator ID="MaxLengthValidator1" runat="server" MaxLength="50" ErrorMessage="&bull;&nbsp;Prefix field is limited to 50 characters." Display="None" ControlToValidate="Prefixtxt" />                            
                            <asp:requiredfieldvalidator ID="Requiredfieldvalidator4" controltovalidate="FNametxt" display="none" errormessage='<%$ Resources:Resource, Message_Required_FirstName %>' runat="server" />
                            <pinso:MaxLengthValidator ID="MaxLengthValidator5" runat="server" MaxLength="255" ErrorMessage="&bull;&nbsp;First Name field is limited to 255 characters." Display="None" ControlToValidate="FNametxt" />                            
                            <pinso:MaxLengthValidator ID="MaxLengthValidator4" runat="server" MaxLength="255" ErrorMessage="&bull;&nbsp;Last Name field is limited to 255 characters." Display="None" ControlToValidate="LNametxt" />
                            <asp:requiredfieldvalidator ID="Requiredfieldvalidator5" controltovalidate="LNametxt" display="none" errormessage='<%$ Resources:Resource, Message_Required_LastName %>' runat="server" />                                             
                            <asp:requiredfieldvalidator ID="Requiredfieldvalidator6" controltovalidate="rdcmbDesg" display="none" errormessage='<%$ Resources:Resource, Message_Required_Designation %>' runat="server" />                           
                            <pinso:MaxLengthValidator ID="MaxLengthValidator9" runat="server" MaxLength="255" ErrorMessage="&bull;&nbsp;Title field is limited to 255 characters." Display="None" ControlToValidate="Titletxt" />                            
                            <pinso:MaxLengthValidator ID="MaxLengthValidator11" runat="server" MaxLength="255" ErrorMessage="&bull;&nbsp;Email field is limited to 255 characters." Display="None" ControlToValidate="Email1txt" />                                                                                    
                            <pinso:MaxLengthValidator ID="MaxLengthValidator2" runat="server" MaxLength="255" ErrorMessage="&bull;&nbsp;Address 1 field is limited to 255 characters." Display="None" ControlToValidate="Addr1txt" />                            
                            <pinso:MaxLengthValidator ID="MaxLengthValidator3" runat="server" MaxLength="255" ErrorMessage="&bull;&nbsp;Address 2 field is limited to 255 characters." Display="None" ControlToValidate="Addr2txt" />                            
                            <pinso:MaxLengthValidator ID="MaxLengthValidator6" runat="server" MaxLength="50" ErrorMessage="&bull;&nbsp;City field is limited to 50 characters." Display="None" ControlToValidate="Citytxt" />                            
                            <pinso:MaxLengthValidator ID="MaxLengthValidator7" runat="server" MaxLength="50" ErrorMessage="&bull;&nbsp;Suffix field is limited to 50 characters." Display="None" ControlToValidate="Suffixtxt" />                                
                            <pinso:MaxLengthValidator ID="MaxLengthValidator16" runat="server" MaxLength="10" ErrorMessage="&bull;&nbsp;ZIP field is limited to 10 characters." Display="None" ControlToValidate="Ziptxt" />                                                                                        
                            <pinso:MaxLengthValidator ID="MaxLengthValidator8" runat="server" MaxLength="50" ErrorMessage="&bull;&nbsp;Phone field is limited to 50 characters." Display="None" ControlToValidate="Ph1txt" />                                                        
                            <pinso:MaxLengthValidator ID="MaxLengthValidator10" runat="server" MaxLength="50" ErrorMessage="&bull;&nbsp;Mobile field is limited to 50 characters." Display="None" ControlToValidate="Mobiletxt" />                            
                            <pinso:MaxLengthValidator ID="MaxLengthValidator12" runat="server" MaxLength="50" ErrorMessage="&bull;&nbsp;Fax field is limited to 50 characters." Display="None" ControlToValidate="Faxtxt" />                            
                            <pinso:MaxLengthValidator ID="MaxLengthValidator13" runat="server" MaxLength="255" ErrorMessage="&bull;&nbsp;Assistant Name field is limited to 255 characters." Display="None" ControlToValidate="AsstNmtxt" />                            
                            <pinso:MaxLengthValidator ID="MaxLengthValidator18" runat="server" MaxLength="1000" ErrorMessage="&bull;&nbsp;Comments field is limited to 1000 characters." Display="None" ControlToValidate="Cmtstxt" />                            
                            <pinso:MaxLengthValidator ID="MaxLengthValidator15" runat="server" MaxLength="50" ErrorMessage="&bull;&nbsp;Office Phone field is limited to 50 characters." Display="None" ControlToValidate="AsstPh1txt" />                            
                            <pinso:MaxLengthValidator ID="MaxLengthValidator14" runat="server" MaxLength="50" ErrorMessage="&bull;&nbsp;Assistant Email field is limited to 50 characters." Display="None" ControlToValidate="AsstEmailtxt" />
                        
                    
                    <div style="height:15px">
                        <asp:ValidationSummary runat="server" ID="validationSummary" DisplayMode="SingleParagraph" />
                    </div>

            <div class="modalFormButtons">           
                 <pinso:CustomButton TabIndex="20" ID="Addbtn" runat="server" Text="Add" Visible="true" CommandName="Insert" />
                 <pinso:CustomButton TabIndex="21" ID="Resetbtn" runat="server" Text="Reset" OnClientClick="javascript:ClearForm(); return false;"/>                        
           </div>
           
        </InsertItemTemplate>
    </asp:FormView>

     </div>
     
    <asp:EntityDataSource runat="server" ID="dsTitles" ConnectionString="name=PathfinderEntities" DefaultContainerName="PathfinderEntities" EntitySetName="TitleSet" OrderBy="it.Name"></asp:EntityDataSource>   
  
    <asp:EntityDataSource runat="server" ID="dsStates" ConnectionString="name=PathfinderEntities" DefaultContainerName="PathfinderEntities" EntitySetName="StateSet"></asp:EntityDataSource>   

    <asp:EntityDataSource ID="dsKeyContacts" runat="server" EntitySetName="KeyContactSet" DefaultContainerName="PathfinderClientEntities" 
        AutoGenerateWhereClause="true" EnableInsert="true" EnableUpdate="true" OnInserting="AddData" OnUpdating="UpdateData" >
        <WhereParameters>       
            <asp:QueryStringParameter QueryStringField="Plan_ID" Name="Plan_ID" Type="Int32" ConvertEmptyStringToNull="true"/>
            <asp:QueryStringParameter QueryStringField="KCID" Name="KC_ID" Type="Int32" ConvertEmptyStringToNull="true" />
        </WhereParameters>
    </asp:EntityDataSource>    
    
    <asp:Label ID="Msglbl" runat="server" Text="" Visible="false"></asp:Label>

    <div align="left" style="padding-top:100px">
        &nbsp;&nbsp;<asp:Label align="left" ID="CloseMsglbl" runat="server" ForeColor="Red" Text="Please note that this window will close in 4 seconds...." Visible="false"></asp:Label>
    </div>        
</asp:Content>