<%@ Page Language="C#" AutoEventWireup="true" Theme="pathfinder" MasterPageFile="~/MasterPages/Modal.master" EnableViewState="true"  CodeFile="OpenAddEditKCs.aspx.cs" Inherits="todaysaccounts_all_OpenAddEditKCs" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    
     <script type="text/javascript">
         function ClearForm()
         {
             $("#AddKCMain input[type != submit]").val("");
             $("TEXTAREA").val("");
         }

         function ConfirmMsg() {
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
             {
                 window3.close(); 
             }

         }
         function onPrintClicked() {
             var querystring = window.location.search;
             var KCID = '<%= Request.QueryString["KCID"] %>';
            
             var data = { KC_ID: KCID };
             window.top.clientManager.set_SelectionData(data, 1);
             var type = 'print';
                          
             $openWindow("../../usercontent/confirmexport.aspx?type=" + type, null, null, 400, 220, "confirmexp");

         }        

    </script>  
</asp:Content>

<asp:Content runat="server" ID="title" ContentPlaceHolderID="title">    
        <asp:Literal runat="server" id="titleText" />
</asp:Content>
<asp:Content ContentPlaceHolderID="tools" runat="server">
     <div id="permission" runat="server" style="float:left; margin-top: 4px;"><a href="javascript:onPrintClicked()" id="printLink" runat="server" style='margin-right:15px'>Print</a></div>
</asp:Content>
<asp:Content runat="server" ID="main" ContentPlaceHolderID="main">
  
  <%-- This page displays appropriate form(View/Add/Edit) for key contacts based on user action.--%>  
  <asp:HiddenField ID="PlanNameHdn" runat="server" Value="" Visible="false" />         
     <div id="AddKCMain">               
    <asp:FormView  runat="server" ID="formViewKC" DefaultMode="Insert"  
           DataSourceID="dsKeyContacts" CellPadding="0" CellSpacing="0" Width="100%">
        <ItemTemplate>
                    <table class="genTable" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td><label>Prefix</label> <%# Eval("KC_Prefix") %>&nbsp;</td>
                        <td><label>Address 1</label> <%# Eval("KC_Address1") %>&nbsp;</td>
                    </tr>
                    <tr>
                        <td><label>First Name</label> <%# Eval("KC_F_Name") %>&nbsp;</td>
                        <td><label>Address 2</label> <%# Eval("KC_Address2") %>&nbsp;</td>
                    </tr>
                      <tr>
                        <td><label>Last Name</label> <%# Eval("KC_L_Name") %>&nbsp;</td>
                        <td><label>City</label> <%# Eval("KC_City") %>&nbsp;</td>
                    </tr>
                    <tr>
                        <td><label>Suffix</label> <%# Eval("KC_Suffix") %>&nbsp;</td>
                        <td><label>State</label> <%# Eval("KC_State") %>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Zip <%# Eval("KC_Zip") %>&nbsp;</td>
                    </tr>
                    <tr>
                        <td><label>Designation</label> <%# Eval("KC_Title_Name")%>&nbsp;</td>
                        <td><label>Phone</label> <%# Eval("KC_Phone") %>&nbsp;</td>
                    </tr>
                       <tr>
                        <td><label>Title</label> <%# Eval("KC_Role")%>&nbsp;</td>
                            <td><label>Mobile</label> <%# Eval("KC_Mobile") %>&nbsp;</td>
                       </tr>
                       <tr>
                        <td><label>Email</label> <%# Eval("KC_Email") %>&nbsp;</td>
                        <td><label>Fax</label> <%# Eval("KC_Fax") %>&nbsp;</td>
                       </tr>
                    </table>
                    </div>

                    <div class="tileContainerHeader" >
                    <div class="title">
                <asp:Literal runat="server" ID="Literal2" Text='Assistant Details' />
            </div>
                    
                    </div>
                    <table  class="genTable" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td><label>Name</label><%# Eval("KC_Admin_Name") %>&nbsp;</td>
                        <td rowspan="3"><label>Comments </label>
                        <p><%# Eval("KC_Comments") %>&nbsp;</p>
                        </td>
                    </tr> 
                    <tr>
                        <td><label>Office Phone</label> <%# Eval("KC_Admin_PH") %>&nbsp;</td>
                    </tr>
                     <tr>
                        <td><label>Email</label> <%# Eval("KC_Admin_Email") %>&nbsp;</td>
                    </tr>
                    </table>
                    <%--<div>
                        <%# Eval("Last_Update_DT", Resources.Resource.Label_Section_Last_Updated)%>
                    </div> --%>   
                    <div class="modalFormButtons">
             <span class="coreBtn" runat="server" id="btnWrapper" Visible='<%# Context.User.IsInRole("editcontacts") %>' >
                 <span class="bg">
                        <span class="bg2">
                            <asp:Button ID="SendUpdViewbtn" runat="server" Text="Send Update/Delete" onclick="SendUpdViewbtn_Click"/>
                        </span>
                    </span>
                </span>
                
<%--            <span class="coreBtn">
                 <span class="bg">
                        <span class="bg2">
                    <asp:Button ID="SendDelbtn" runat="server" Text="Send Delete" Visible="true" onclick="SendDelbtn_Click"/>
                        </span>
                    </span>
                </span>
--%>            </div>
        
        </ItemTemplate> 
        
        <EditItemTemplate>
                    <table class="genTable" cellpadding="0" cellspacing="0" border="0" >
                    <tr>
                        <td><label for="Prefixtxt">Prefix </label><asp:TextBox TabIndex="1" ID="Prefixtxt" runat="server" Width="57px" Text='<%# Bind("KC_Prefix") %>'></asp:TextBox></td>
                        <td><label for="Addr1txt">Address 1</label><asp:TextBox TabIndex="11" ID="Addr1txt" runat="server" Width="331px" Text='<%# Bind("KC_Address1") %>'></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><label for="FNametxt">First Name *</label><asp:TextBox TabIndex="2" ID="FNametxt" runat="server" Width="237px" Text='<%# Bind("KC_F_Name") %>'></asp:TextBox></td>
                        <td><label for="Addr2txt">Address 2 </label><asp:TextBox TabIndex="12" ID="Addr2txt" runat="server" Width="330px" Text='<%# Bind("KC_Address2") %>'></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><label for="LNametxt">Last Name *</label><asp:TextBox TabIndex="3" ID="LNametxt" runat="server" Width="237px" Text='<%# Bind("KC_L_Name") %>'></asp:TextBox></td>
                        <td><label for="Citytxt">City</label><asp:TextBox TabIndex="13" ID="Citytxt" runat="server" Width="331px" Text='<%# Bind("KC_City") %>'></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><label for="Suffixtxt">Suffix</label><asp:TextBox TabIndex="4" ID="Suffixtxt" runat="server" Width="57px" Text='<%# Bind("KC_Suffix") %>'></asp:TextBox></td>
                        <td><label for="rdlStates">State</label><telerik:RadComboBox TabIndex="14" EnableEmbeddedSkins="false" SkinID="planInfoCombo" Skin="pathfinder" runat="server" ID="rdlStates" DataSourceID="dsStates" Width="120px" DropDownWidth="180px" DataTextField="Name" DataValueField="ID" AppendDataBoundItems="true" MaxHeight="200" SelectedValue='<%# Bind("KC_State") %>'>
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="" />
                            </Items>
                        </telerik:RadComboBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <label for="Ziptxt">Zip</label>
                        <asp:TextBox TabIndex="15" ID="Ziptxt" runat="server" Width="57px" Text='<%# Bind("KC_Zip") %>'></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td><label for="rdcmbDesg">Designation *</label><telerik:RadComboBox TabIndex="5" ID="rdcmbDesg" DataSourceID="dsTitles" DataTextField="Name" DataValueField="ID" runat="server" Width="65%" EnableEmbeddedSkins="false" SkinID="planInfoCombo" Skin="pathfinder" AppendDataBoundItems="true" SelectedValue='<%# Bind("Title.ID") %>'>
                                <Items>
                                    <telerik:RadComboBoxItem Value="" Text="" />
                                </Items>
                            </telerik:RadComboBox> 
                        </td>
                        <td><label for="Ph1txt">Phone</label><asp:TextBox TabIndex="16" ID="Ph1txt" runat="server" Width="111px" Text='<%# Bind("KC_Phone") %>'></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><label for="Titletxt">Title</label><asp:TextBox TabIndex="6" ID="Titletxt" runat="server" Width="330px" Text='<%# Bind("KC_Role") %>'></asp:TextBox></td>
                        <td><label for="Mobiletxt">Mobile</label><asp:TextBox TabIndex="17" ID="Mobiletxt" runat="server" Width="111px" Text='<%# Bind("KC_Mobile") %>'></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><label for="Email1txt">Email</label><asp:TextBox TabIndex="7" ID="Email1txt" runat="server" Width="331px" Text='<%# Bind("KC_Email") %>'></asp:TextBox></td>
                        <td><label for="Faxtxt">Fax</label><asp:TextBox TabIndex="18" ID="Faxtxt" runat="server" Width="111px" Text='<%# Bind("KC_Fax") %>'></asp:TextBox></td>
                    </tr>
                    
                    </table>
                    </div>

                    <div class="tileContainerHeader" >
                    <div class="title">
                <asp:Literal runat="server" ID="Literal2" Text='Assistant Details' />
            </div>
                    
                    </div>
                    <table class="genTable" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td><label for="AsstNmtxt">Name</label><asp:TextBox TabIndex="8" ID="AsstNmtxt" runat="server" Width="237px" Text='<%# Bind("KC_Admin_Name") %>'></asp:TextBox></td>
                        <td rowspan="3"><label for="Cmtstxt">Comments</label><asp:TextBox TabIndex="19" ID="Cmtstxt" runat="server" Height="47px" Rows="2" TextMode="MultiLine" Width="310px" Text='<%# Bind("KC_Comments") %>'></asp:TextBox>
                        </td>
                    </tr> 
                    <tr>
                        <td><label for="AsstPh1txt">Office Phone </label><asp:TextBox TabIndex="9" ID="AsstPh1txt" runat="server" Width="111px" Text='<%# Bind("KC_Admin_PH") %>'></asp:TextBox></td>
                    </tr> 
                    <tr>
                        <td><label for="AsstEmailtxt">Email </label><asp:TextBox TabIndex="10" ID="AsstEmailtxt" runat="server" Width="331px" Text='<%# Bind("KC_Admin_Email") %>'></asp:TextBox></td>
                    </tr> 
                    </table>
                    <asp:requiredfieldvalidator ID="Requiredfieldvalidator4" controltovalidate="FNametxt" display="none" errormessage='<%$ Resources:Resource, Message_Required_FirstName %>' runat="server" />

                    <asp:requiredfieldvalidator ID="Requiredfieldvalidator5" controltovalidate="LNametxt" display="none" errormessage='<%$ Resources:Resource, Message_Required_LastName %>' runat="server" />
                      
                    <asp:requiredfieldvalidator ID="Requiredfieldvalidator6" controltovalidate="rdcmbDesg" display="none" errormessage='<%$ Resources:Resource, Message_Required_Designation %>' runat="server" />
            <%--<div>
                <%# Eval("Last_Update_DT", Resources.Resource.Label_Section_Last_Updated)%>
            </div> --%>             
                    <div style="height:15px">
                        <asp:ValidationSummary runat="server" ID="validationSummary" DisplayMode="SingleParagraph" />
                    </div>

            <div class="modalFormButtons">           
                 <pinso:CustomButton TabIndex="20" ID="SendUpdbtn" runat="server" Text="Send Update" Visible="true" onclick="SendUpdbtn_Click"/>                     
                 <pinso:CustomButton TabIndex="21" ID="SendDelbtn" runat="server" Text="Send Delete" Visible="true" onclick="SendDelbtn_Click"/>
                 <pinso:CustomButton TabIndex="22" ID="Resetbtn" runat="server" Text="Reset" OnClientClick="javascript:ClearForm(); return false;"/> 
            </div>
           
        </EditItemTemplate>
        
        <InsertItemTemplate>
            <table class="genTable" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td>
                        <label for="Prefixtxt">Prefix</label>
                        <asp:TextBox TabIndex="1" ID="Prefixtxt" runat="server" Width="57px" Text='<%# Bind("KC_Prefix") %>'></asp:TextBox>
                    </td>
                    <td class="rn">
                        <label for="Addr1txt">Address 1</label>
                        <asp:TextBox TabIndex="11" ID="Addr1txt" runat="server" Width="331px" Text='<%# Bind("KC_Address1") %>'></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label for="FNametxt">First Name *</label>
                        <asp:TextBox TabIndex="2" ID="FNametxt" runat="server" Width="237px" Text='<%# Bind("KC_F_Name") %>'></asp:TextBox>
                    </td>
                    <td class="rn">
                        <label for="Addr2txt">Address 2</label>
                        <asp:TextBox TabIndex="12" ID="Addr2txt" runat="server" Width="330px" Text='<%# Bind("KC_Address2") %>'></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label for="LNametxt">Last Name *</label>
                        <asp:TextBox TabIndex="3" ID="LNametxt" runat="server" Width="237px" Text='<%# Bind("KC_L_Name") %>'></asp:TextBox>
                    </td>
                    <td class="rn">
                        <label for="Citytxt">City</label>
                        <asp:TextBox TabIndex="13" ID="Citytxt" runat="server" Width="331px" Text='<%# Bind("KC_City") %>'></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label for="Suffixtxt">Suffix</label>
                        <asp:TextBox TabIndex="4" ID="Suffixtxt" runat="server" Width="57px" Text='<%# Bind("KC_Suffix") %>'></asp:TextBox>
                    </td>
                    <td class="rn">
                        <label for="rdlStates">State</label>
                        <telerik:RadComboBox TabIndex="14" EnableEmbeddedSkins="false" SkinID="planInfoCombo" Skin="pathfinder"
                            runat="server" ID="rdlStates" DataSourceID="dsStates" Width="120px" DropDownWidth="180px"
                            DataTextField="Name" DataValueField="ID" AppendDataBoundItems="true" MaxHeight="200"
                            SelectedValue='<%# Bind("KC_State") %>'>
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="" />
                            </Items>
                        </telerik:RadComboBox>
                        <label class="ml10" for="Ziptxt">Zip</label>
                        <asp:TextBox TabIndex="15" ID="Ziptxt" runat="server" Width="57px" Text='<%# Bind("KC_Zip") %>'></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label for="rdcmbDesg">Designation *</label>
                        <telerik:RadComboBox TabIndex="5" ID="rdcmbDesg" DataSourceID="dsTitles" DataTextField="Name"
                            DataValueField="ID" runat="server" Width="65%" EnableEmbeddedSkins="false" SkinID="planInfoCombo"
                            Skin="pathfinder" AppendDataBoundItems="true">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td class="rn">
                        <label for="Ph1txt">Phone</label>
                        <asp:TextBox TabIndex="16" ID="Ph1txt" runat="server" Width="111px" Text='<%# Bind("KC_Phone") %>'></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label for="Titletxt">Title</label>
                        <asp:TextBox TabIndex="6" ID="Titletxt" runat="server" Width="330px" Text='<%# Bind("KC_Role") %>'></asp:TextBox>
                    </td>
                    <td class="rn">
                        <label for="MobileTxt">Mobile</label>
                        <asp:TextBox TabIndex="17" ID="MobileTxt" runat="server" Width="111px" Text='<%# Bind("KC_Mobile") %>'></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label for="Email1txt">Email</label>
                        <asp:TextBox TabIndex="7" ID="Email1txt" runat="server" Width="331px" Text='<%# Bind("KC_Email") %>'></asp:TextBox>
                    </td>
                    <td class="rn">
                        <label for="Faxtxt">Fax</label>
                        <asp:TextBox TabIndex="18" ID="Faxtxt" runat="server" Width="111px" Text='<%# Bind("KC_Fax") %>'></asp:TextBox>
                    </td>
                </tr>
            </table>
                    

                    <div class="tileContainerHeader" >
                    <div class="title">
                <asp:Literal runat="server" ID="Literal2" Text='Assistant Details' />
            </div>
                    
                    </div>
            <table class="genTable" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td><label for="AsstNmtxt">Name</label>
                            <asp:TextBox TabIndex="8" ID="AsstNmtxt" runat="server" Width="237px" Text='<%# Bind("KC_Admin_Name") %>'></asp:TextBox>
                        </td>
                        <td rowspan="3">
                        <label for="Cmtstxt">Comments</label>
                            <asp:TextBox TabIndex="19" ID="Cmtstxt" runat="server" Height="50px" Rows="2" TextMode="MultiLine" Width="305px" Text='<%# Bind("KC_Comments") %>'></asp:TextBox>
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
                    
            <asp:requiredfieldvalidator ID="Requiredfieldvalidator4" controltovalidate="FNametxt" display="none" errormessage='<%$ Resources:Resource, Message_Required_FirstName %>' runat="server" />

            <asp:requiredfieldvalidator ID="Requiredfieldvalidator5" controltovalidate="LNametxt" display="none" errormessage='<%$ Resources:Resource, Message_Required_LastName %>' runat="server" />
              
              <asp:requiredfieldvalidator ID="Requiredfieldvalidator6" controltovalidate="rdcmbDesg" display="none" errormessage='<%$ Resources:Resource, Message_Required_Designation %>' runat="server" />
            <div>&nbsp;</div>              
            <div style="height:15px">
                <asp:ValidationSummary runat="server" ID="validationSummary" DisplayMode="SingleParagraph" />
            </div>
            
            <div class="modalFormButtons">               
                 <pinso:CustomButton TabIndex="20" ID="Addbtn" runat="server" Text="Add" Visible="true" onclick="Addbtn_Click"/>
                 <pinso:CustomButton TabIndex="21" ID="Resetbtn" runat="server" Text="Reset" OnClientClick="javascript:ClearForm(); return false;"/>                        
            </div>
           
        </InsertItemTemplate>
    </asp:FormView>
</div>
    
    
    <asp:EntityDataSource runat="server" ID="dsTitles" ConnectionString="name=PathfinderEntities" DefaultContainerName="PathfinderEntities" EntitySetName="TitleSet" OrderBy="it.Name"></asp:EntityDataSource>   
  
    <asp:EntityDataSource runat="server" ID="dsStates" ConnectionString="name=PathfinderEntities" DefaultContainerName="PathfinderEntities" EntitySetName="StateSet"></asp:EntityDataSource>   

    <asp:EntityDataSource ID="dsKeyContacts" runat="server" EntitySetName="KeyContactSet" Include="Title" ConnectionString="name=PathfinderEntities" DefaultContainerName="PathfinderEntities" 
        AutoGenerateWhereClause="true">
        <WhereParameters>       
            <asp:QueryStringParameter QueryStringField="PlanID" Name="Plan_ID" Type="Int32" ConvertEmptyStringToNull="true"/>
            <asp:QueryStringParameter QueryStringField="KCID" Name="KC_ID" Type="Int32" ConvertEmptyStringToNull="true" />
        </WhereParameters>
        <InsertParameters>
            <asp:QueryStringParameter QueryStringField="PlanID" Name="Plan_ID" Type="Int32" ConvertEmptyStringToNull="true" DefaultValue="1"/>
        </InsertParameters>
        <UpdateParameters>
            <asp:QueryStringParameter QueryStringField="PlanID" Name="Plan_ID" Type="Int32" ConvertEmptyStringToNull="true"/>
            <asp:QueryStringParameter QueryStringField="KCID" Name="KC_ID" Type="Int32" ConvertEmptyStringToNull="true" />
        </UpdateParameters> 
    </asp:EntityDataSource>    
    
    <asp:Label ID="Msglbl" runat="server" Text="" Visible="false"></asp:Label>
    <br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />
    <br /><br /><br /><br /><br /><br /><br /><br /><br /><br />
    <div align="left">
        &nbsp;&nbsp;<asp:Label align="left" ID="CloseMsglbl" runat="server" ForeColor="Red" Text="Please note that this window will close in 4 seconds...." Visible="false"></asp:Label>
    </div>      
    <telerik:radwindowmanager EnableEmbeddedSkins="false" Skin="pathfinder" id="RadWindowManager1" runat="server" DestroyOnClose="true" Modal="true" 
           Behaviors="Close" VisibleTitlebar="false">    
    </telerik:radwindowmanager>  
</asp:Content>