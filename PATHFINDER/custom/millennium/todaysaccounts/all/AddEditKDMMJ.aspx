<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Modal.master" AutoEventWireup="true" CodeFile="AddEditKDMMJ.aspx.cs" Inherits="custom_millennium_todaysaccounts_all_AddEditKDMMJ" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register src="~/custom/millennium/todaysaccounts/controls/AddEditKDMScript.ascx" tagname="KDMScript" tagprefix="pinso" %>
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

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" runat="Server">
    <asp:Literal runat="server" ID="titleText" />
    <pinso:KDMScript ID ="KDMScript1" runat="server" />    
</asp:Content><asp:Content ID="Content3" ContentPlaceHolderID="tools" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="main" runat="Server">
    <asp:HiddenField ID="PlanNameHdn" runat="server" Value="" Visible="false" />
    <asp:HiddenField ID="hdnMeetOutcome" runat="server" Value="" Visible="true" />
    <asp:HiddenField ID="hdnCredentialsOutcome" runat="server" Value="" Visible="true" /> 
    <asp:HiddenField ID="hdnCACOutcome" runat="server" Value="" Visible="true" />    
    <asp:HiddenField Id="hdnStatesCovered" runat="server" Value="" Visible="true" />
    <asp:HiddenField ID="hdnMJ" runat="server" Value="" Visible="true" />   
    <asp:FormView runat="server" ID="formKDMView" CellPadding="0" Width="100%" DataSourceID="dsKDM">
        <InsertItemTemplate>
            <table class="genTable">
            <tr>
                <td valign="top">
                    <table align="left" >
                    <tr align="left">
                        <td><label for="CAC_CMD">CAC / CMD *</label>
                            <telerik:RadComboBox ID="rdlCAC_CMD" runat="server" AppendDataBoundItems="true" 
                             TabIndex="1" Skin="pathfinder" Width="120px" DropDownWidth="120px"
                                        EnableEmbeddedSkins="false" AllowCustomText="True"  SelectedValue='<%# Bind("CAC_CMD") %>' >
                               <Items>
                                        <telerik:RadComboBoxItem Text="-Select CAC / CMD-" Value="" Selected="true" />
                                        <telerik:RadComboBoxItem Text="CAC" Value="CAC" />
                                        <telerik:RadComboBoxItem Text="CMD" Value="CMD" />
                               </Items>
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr align="left">
                        <td><label for="FNametxt">First Name *</label>
                            <asp:TextBox TabIndex="2" ID="FNametxt" runat="server" Width="237px" Text='<%# Bind("KDM_F_Name") %>'></asp:TextBox>
                        </td>
                    </tr>
                    <tr>    
                        <td><label for="LNametxt">Last Name *</label>
                            <asp:TextBox TabIndex="3" ID="LNametxt" runat="server" Width="237px" Text='<%# Bind("KDM_L_Name") %>'></asp:TextBox>
                        
                        </td>
                    </tr>
                    <tr>
                        <td><label for="Emailtxt">Email</label>
                            <asp:TextBox TabIndex="4" ID="Emailtxt" runat="server" Width="331px" Text='<%# Bind("KDM_Email") %>'></asp:TextBox>
                        </td>
                    </tr>
                    <tr align="left"> 
                        <td><label for="Titletxt">Title</label>
                            <telerik:RadComboBox ID="rdlCustomTitle" runat="server" AppendDataBoundItems="true" DataSourceID="dsCustomsTitle"
                                        TabIndex="5" Skin="pathfinder" Width="343px" DropDownWidth="343px"  DataTextField ="Title_Name"
                                        EnableEmbeddedSkins="false" DataValueField="Title_ID" AllowCustomText="True" Text="-Select Custom Title-" 
                                         OnClientDropDownClosed="setCustomTitle" OnClientLoad="setCustomTitle">
                                        <ItemTemplate>
                                            <span id='<%# String.Format("m{0}",Eval("Title_ID")) %>'>
                                                <asp:CheckBox ID="CheckBox1" runat="server" Text='<%#Eval ("Title_Name") %>' 
                                                onclick='<%# string.Format("CustomTitleChanged(this,{0})", Eval("Title_ID")) %>' />
                                            </span>
                                        </ItemTemplate>
                             </telerik:RadComboBox>
                             <div id="spanCustomTitle"></div>
                        </td>
                    </tr> 
                    <tr align="left"> 
                        <td><label for="CACtxt">CAC Type</label>
                            <telerik:RadComboBox ID="rdlCustomCAC" runat="server" AppendDataBoundItems="true" DataSourceID="dsCustomCAC"
                                        TabIndex="6" Skin="pathfinder" Width="343px" DropDownWidth="343px"  DataTextField ="CAC_Name"
                                        EnableEmbeddedSkins="false" DataValueField="CAC_ID" AllowCustomText="True" Text="-Select Custom CAC-" 
                                         OnClientDropDownClosed="setCustomCAC" OnClientLoad="setCustomCAC">
                                        <ItemTemplate>
                                            <span id='<%# String.Format("b{0}",Eval("CAC_ID")) %>'>
                                                <asp:CheckBox ID="CheckBox3" runat="server" Text='<%#Eval ("CAC_Name") %>' 
                                                onclick='<%# string.Format("CustomCACChanged(this,{0})", Eval("CAC_ID")) %>' />
                                            </span>
                                        </ItemTemplate>
                             </telerik:RadComboBox>
                             <div id="spanCustomCAC"></div>
                        </td>
                    </tr>                   
                    </table>
                </td>
                <td valign="top">
                    <table>
                         <tr>                        
                            <td><label for="Phtxt">Phone</label>
                              <asp:TextBox TabIndex="7" ID="Phtxt" runat="server" Width="111px" Text='<%# Bind("KDM_Phone") %>'></asp:TextBox>
                             </td>
                        </tr>
                        <tr> 
                            <td><label for="Faxtxt">Fax</label>
                                <asp:TextBox ID="Faxtxt" TabIndex="8" runat="server" Width="111px" Text='<%# Bind("KDM_Fax") %>'></asp:TextBox>
                            </td>
                        </tr> 
                        <tr> 
                            <td><label for="Faxtxt">Organization Name</label>
                                <asp:TextBox ID="Aff_MJ_Org_Name" TabIndex="9" runat="server" Width="330px" Text='<%# Bind("Aff_MJ_Org_Name") %>'></asp:TextBox>
                            </td>
                        </tr> 
                        <tr>
                            <td>
                                <label for="Titletxt">Credentials</label>
                                <telerik:RadComboBox ID="rdlCredentials" runat="server" AppendDataBoundItems="true" DataSourceID="dsCustomCredentials"
                                        TabIndex="10" Skin="pathfinder" Width="343px" DropDownWidth="343px"  DataTextField ="Credentials_Name"
                                        EnableEmbeddedSkins="false" DataValueField="Credentials_ID" AllowCustomText="True" Text="-Select Custom Credentials-" 
                                         OnClientDropDownClosed="setCustomCredentials" OnClientLoad="setCustomCredentials">
                                        <ItemTemplate>
                                            <span id='<%# String.Format("c{0}",Eval("Credentials_ID")) %>'>
                                                <asp:CheckBox ID="CheckBox2" runat="server" Text='<%#Eval ("Credentials_Name") %>'
                                                 onclick='<%# string.Format("CustomCredentialsChanged(this,{0})", Eval("Credentials_ID")) %>' />
                                            </span>
                                        </ItemTemplate>
                                </telerik:RadComboBox>
                             <div id="spanCredentials"></div>
                            </td>                            
                        </tr> 
                    </table>
                </td>    
            </tr>        
            </table>
            
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="&bull;&nbsp;Select CAC / CMD." Display="None"  InitialValue="-Select CAC / CMD-" ControlToValidate="rdlCAC_CMD"></asp:RequiredFieldValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="&bull;&nbsp;First Name Required."  ControlToValidate="FNametxt" Display="None" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="&bull;&nbsp;Last Name Required."  ControlToValidate="LNametxt"  Display="None" />            
            <asp:RegularExpressionValidator ID="regexpName" runat="server"   ErrorMessage="&bull;&nbsp;Valid Email Required." Display="None"  ControlToValidate="Emailtxt" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"   ErrorMessage="&bull;&nbsp;Valid Phone Required." Display="None"  ControlToValidate="Phtxt" ValidationExpression="^((([1]{1})*[- .(]*([0-9]{3})[- .)]*[0-9]{3}[- .]*[0-9]{4}))$"/>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"   ErrorMessage="&bull;&nbsp;Valid Fax Required." Display="None"  ControlToValidate="Faxtxt" ValidationExpression="^((([1]{1})*[- .(]*([0-9]{3})[- .)]*[0-9]{3}[- .]*[0-9]{4}))$"/>
            <div style="height:15px">
                        <asp:ValidationSummary runat="server" ID="validationSummary" DisplayMode="SingleParagraph" />
             </div>
             <div class="modalFormButtons"  style="margin-top: -10px;">                
                <pinso:CustomButton TabIndex="11" ID="btnEdit" runat="server" Text="Add" Visible="true" OnClick="Addbtn_Click" />
                <pinso:CustomButton ID="btnReset" TabIndex="12" runat="server" Text="Reset" OnClientClick="javascript:ClearForm(); return false;" />                       
            </div>        
        </InsertItemTemplate>
        <EditItemTemplate>
            <table class="genTable">
            <tr>
                <td valign="top">
                    <table align="left" >
                    <tr align="left">
                        <td><label for="CAC_CMD">CAC /CMD *</label>
                            <telerik:RadComboBox ID="rdlCAC_CMD" runat="server" AppendDataBoundItems="true" 
                             TabIndex="1" Skin="pathfinder" Width="120px" DropDownWidth="120px"
                                        EnableEmbeddedSkins="false" AllowCustomText="True"  SelectedValue='<%# Bind("CAC_CMD") %>' >
                               <Items>
                                        <telerik:RadComboBoxItem Text="-Select CAC / CMD-" Value="" Selected="true" />
                                        <telerik:RadComboBoxItem Text="CAC" Value="CAC" />
                                        <telerik:RadComboBoxItem Text="CMD" Value="CMD" />
                               </Items>
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr align="left">
                        <td><label for="FNametxt">First Name *</label>
                            <asp:TextBox TabIndex="2" ID="FNametxt" runat="server" Width="237px" Text='<%# Bind("KDM_F_Name") %>'></asp:TextBox>
                        </td>
                    </tr>
                    <tr>    
                        <td><label for="LNametxt">Last Name *</label>
                            <asp:TextBox TabIndex="3" ID="LNametxt" runat="server" Width="237px" Text='<%# Bind("KDM_L_Name") %>'></asp:TextBox>
                        
                        </td>
                    </tr>
                    <tr>
                        <td><label for="Emailtxt">Email</label>
                            <asp:TextBox TabIndex="4" ID="Emailtxt" runat="server" Width="331px" Text='<%# Bind("KDM_Email") %>'></asp:TextBox>
                        </td>
                    </tr>
                    <tr align="left"> 
                        <td><label for="Titletxt">Title</label>
                            <telerik:RadComboBox ID="rdlCustomTitle" runat="server" AppendDataBoundItems="true" DataSourceID="dsCustomsTitle"
                                        TabIndex="5" Skin="pathfinder" Width="343px" DropDownWidth="343px"  DataTextField ="Title_Name"
                                        EnableEmbeddedSkins="false" DataValueField="Title_ID" AllowCustomText="True" Text="-Select Custom Title-" 
                                         OnClientDropDownClosed="setCustomTitle" OnClientLoad="setCustomTitle">
                                        <ItemTemplate>
                                            <span id='<%# String.Format("m{0}",Eval("Title_ID")) %>'>
                                                <asp:CheckBox ID="CheckBox1" runat="server" Text='<%#Eval ("Title_Name") %>' 
                                                onclick='<%# string.Format("CustomTitleChanged(this,{0})", Eval("Title_ID")) %>' />
                                            </span>
                                        </ItemTemplate>
                             </telerik:RadComboBox>
                             <div id="spanCustomTitle"></div>
                        </td>
                    </tr> 
                    <tr align="left"> 
                        <td><label for="CACtxt">CAC Type</label>
                            <telerik:RadComboBox ID="rdlCustomCAC" runat="server" AppendDataBoundItems="true" DataSourceID="dsCustomCAC"
                                        TabIndex="6" Skin="pathfinder" Width="343px" DropDownWidth="343px"  DataTextField ="CAC_Name"
                                        EnableEmbeddedSkins="false" DataValueField="CAC_ID" AllowCustomText="True" Text="-Select Custom CAC-" 
                                         OnClientDropDownClosed="setCustomCAC" OnClientLoad="setCustomCAC">
                                        <ItemTemplate>
                                            <span id='<%# String.Format("b{0}",Eval("CAC_ID")) %>'>
                                                <asp:CheckBox ID="CheckBox3" runat="server" Text='<%#Eval ("CAC_Name") %>' 
                                                onclick='<%# string.Format("CustomCACChanged(this,{0})", Eval("CAC_ID")) %>' />
                                            </span>
                                        </ItemTemplate>
                             </telerik:RadComboBox>
                             <div id="spanCustomCAC"></div>
                        </td>
                    </tr>                   
                    </table>
                </td>
                <td valign="top">
                    <table>
                         <tr>                        
                            <td><label for="Phtxt">Phone</label>
                              <asp:TextBox TabIndex="7" ID="Phtxt" runat="server" Width="111px" Text='<%# Bind("KDM_Phone") %>'></asp:TextBox>
                             </td>
                        </tr>
                        <tr> 
                            <td><label for="Faxtxt">Fax</label>
                                <asp:TextBox ID="Faxtxt" TabIndex="8" runat="server" Width="111px" Text='<%# Bind("KDM_Fax") %>'></asp:TextBox>
                            </td>
                        </tr>  
                        <tr> 
                            <td><label for="Faxtxt">Organization Name</label>
                                <asp:TextBox ID="Aff_MJ_Org_Name" TabIndex="9" runat="server" Width="330px" Text='<%# Bind("Aff_MJ_Org_Name") %>'></asp:TextBox>
                            </td>
                        </tr> 
                        <tr>
                            <td>
                                <label for="Titletxt">Credentials</label>
                                <telerik:RadComboBox ID="rdlCredentials" runat="server" AppendDataBoundItems="true" DataSourceID="dsCustomCredentials"
                                        TabIndex="10" Skin="pathfinder" Width="343px" DropDownWidth="343px"  DataTextField ="Credentials_Name"
                                        EnableEmbeddedSkins="false" DataValueField="Credentials_ID" AllowCustomText="True" Text="-Select Custom Credentials-" 
                                         OnClientDropDownClosed="setCustomCredentials" OnClientLoad="setCustomCredentials">
                                        <ItemTemplate>
                                            <span id='<%# String.Format("c{0}",Eval("Credentials_ID")) %>'>
                                                <asp:CheckBox ID="CheckBox2" runat="server" Text='<%#Eval ("Credentials_Name") %>' 
                                                onclick='<%# string.Format("CustomCredentialsChanged(this,{0})", Eval("Credentials_ID")) %>' />
                                            </span>
                                        </ItemTemplate>
                                </telerik:RadComboBox>
                             <div id="spanCredentials"></div>
                            </td>                            
                        </tr> 
                    </table>
                </td>    
            </tr>        
            </table>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="&bull;&nbsp;Select CAC / CMD." Display="None"  InitialValue="-Select CAC / CMD-" ControlToValidate="rdlCAC_CMD"></asp:RequiredFieldValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="&bull;&nbsp;First Name Required."  ControlToValidate="FNametxt" Display="None" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="&bull;&nbsp;Last Name Required."  ControlToValidate="LNametxt"  Display="None" />            
            <asp:RegularExpressionValidator ID="regexpName" runat="server"   ErrorMessage="&bull;&nbsp;Valid Email Required." Display="None"  ControlToValidate="Emailtxt" ValidationExpression="^[a-zA-Z0-9._]+@[a-zA-Z0-9._]+?\.[a-zA-Z]{2,4}$" />
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"   ErrorMessage="&bull;&nbsp;Valid Phone Required." Display="None"  ControlToValidate="Phtxt" ValidationExpression="^((([1]{1})*[- .(]*([0-9]{3})[- .)]*[0-9]{3}[- .]*[0-9]{4}))$"/>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"   ErrorMessage="&bull;&nbsp;Valid Fax Required." Display="None"  ControlToValidate="Faxtxt" ValidationExpression="^((([1]{1})*[- .(]*([0-9]{3})[- .)]*[0-9]{3}[- .]*[0-9]{4}))$"/>
            <div style="height:15px">
                        <asp:ValidationSummary runat="server" ID="validationSummary" DisplayMode="SingleParagraph" />
             </div>
            <div class="modalFormButtons" style="margin-top: -10px;">              
                  <%--<pinso:CustomButton TabIndex="20" ID="Updbtn" runat="server" Text="Update" width="50px" Visible="true" CommandName="Editbtn_Click" />--%>               
                  <pinso:CustomButton TabIndex="11" ID="Delbtn" runat="server" Text="Update" width="50px" Visible="true" onclick="Editbtn_Click" />                           
                  <pinso:CustomButton TabIndex="12" ID="Nobtn" width="50px" runat="server" Text="Cancel" OnClientClick="javascript:CloseWin(); return false;"/>  
            </div>
        </EditItemTemplate>
        <ItemTemplate>
            <table class="genTable">
            <tr>
                <td valign="top">
                    <table align="left" >
                    <tr align="left">
                        <td><label for="CAC_CMD">CAC /CMD *</label>
                            <telerik:RadComboBox ID="rdlCAC_CMD" runat="server" AppendDataBoundItems="true" 
                             TabIndex="1" Skin="pathfinder" Width="120px" DropDownWidth="120px"
                                        EnableEmbeddedSkins="false" AllowCustomText="True"  SelectedValue='<%# Bind("CAC_CMD") %>' >
                               <Items>
                                        <telerik:RadComboBoxItem Text="-Select CAC / CMD-" Value="" Selected="true" />
                                        <telerik:RadComboBoxItem Text="CAC" Value="CAC" />
                                        <telerik:RadComboBoxItem Text="CMD" Value="CMD" />
                               </Items>
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr align="left">
                        <td><label for="FNametxt">First Name *</label>
                            <asp:TextBox TabIndex="2" ID="FNametxt" runat="server" Width="237px" Text='<%# Eval("KDM_F_Name") %>' ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>    
                        <td><label for="LNametxt">Last Name *</label>
                            <asp:TextBox TabIndex="3" ID="LNametxt" runat="server" Width="237px" Text='<%# Eval("KDM_L_Name") %>' ReadOnly="true"></asp:TextBox>
                        
                        </td>
                    </tr>
                    <tr>
                        <td><label for="Emailtxt">Email</label>
                            <asp:TextBox TabIndex="4" ID="Emailtxt" runat="server" Width="331px" Text='<%# Eval("KDM_Email") %>' ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr align="left"> 
                        <td><label for="Titletxt">Title</label>
                            <telerik:RadComboBox ID="rdlCustomTitle" runat="server" AppendDataBoundItems="true" DataSourceID="dsCustomsTitle"
                                        TabIndex="5" Skin="pathfinder" Width="343px" DropDownWidth="343px"  DataTextField ="Title_Name"
                                        EnableEmbeddedSkins="false" DataValueField="Title_ID" AllowCustomText="True" Text="-Select Custom Title-" 
                                         OnClientDropDownClosed="setCustomTitle" OnClientLoad="setCustomTitle">
                                        <ItemTemplate>
                                            <span id='<%# String.Format("m{0}",Eval("Title_ID")) %>'>
                                                <asp:CheckBox ID="CheckBox1" runat="server" Text='<%#Eval ("Title_Name") %>' onclick='<%# string.Format("CustomTitleChanged(this,{0})", Eval("Title_ID")) %>' />
                                            </span>
                                        </ItemTemplate>
                             </telerik:RadComboBox>
                             <div id="spanCustomTitle"></div>
                        </td>
                    </tr> 
                    <tr align="left"> 
                        <td><label for="CACtxt">CAC Type</label>
                            <telerik:RadComboBox ID="rdlCustomCAC" runat="server" AppendDataBoundItems="true" DataSourceID="dsCustomCAC"
                                        TabIndex="6" Skin="pathfinder" Width="343px" DropDownWidth="343px"  DataTextField ="CAC_Name"
                                        EnableEmbeddedSkins="false" DataValueField="CAC_ID" AllowCustomText="True" Text="-Select Custom CAC-" 
                                         OnClientDropDownClosed="setCustomCAC" OnClientLoad="setCustomCAC">
                                        <ItemTemplate>
                                            <span id='<%# String.Format("b{0}",Eval("CAC_ID")) %>'>
                                                <asp:CheckBox ID="CheckBox3" runat="server" Text='<%#Eval ("CAC_Name") %>' 
                                                onclick='<%# string.Format("CustomCACChanged(this,{0})", Eval("CAC_ID")) %>' />
                                            </span>
                                        </ItemTemplate>
                             </telerik:RadComboBox>
                             <div id="spanCustomCAC"></div>
                        </td>
                    </tr>                   
                    </table>
                </td>
                <td valign="top">
                    <table>
                         <tr>                        
                            <td><label for="Phtxt">Phone</label>
                              <asp:TextBox TabIndex="7" ID="Phtxt" runat="server" Width="111px" Text='<%# Eval("KDM_Phone") %>' ReadOnly="true"></asp:TextBox>
                             </td>
                        </tr>
                        <tr> 
                            <td><label for="Faxtxt">Fax</label>
                                <asp:TextBox ID="Faxtxt" TabIndex="8" runat="server" Width="111px" Text='<%# Eval("KDM_Fax") %>' ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr> 
                            <td><label for="Faxtxt">Organization Name</label>
                                <asp:TextBox ID="Aff_MJ_Org_Name" TabIndex="9" runat="server" Width="330px" Text='<%# Bind("Aff_MJ_Org_Name") %>'></asp:TextBox>
                            </td>
                        </tr> 
                        <tr>
                            <td>
                                <label for="Titletxt">Credentials</label>
                                <telerik:RadComboBox ID="rdlCredentials" runat="server" AppendDataBoundItems="true" DataSourceID="dsCustomCredentials"
                                        TabIndex="10" Skin="pathfinder" Width="343px" DropDownWidth="343px"  DataTextField ="Credentials_Name"
                                        EnableEmbeddedSkins="false" DataValueField="Credentials_ID" AllowCustomText="True" Text="-Select Custom Credentials-" 
                                         OnClientDropDownClosed="setCustomCredentials" OnClientLoad="setCustomCredentials">
                                        <ItemTemplate>
                                            <span id='<%# String.Format("c{0}",Eval("Credentials_ID")) %>'>
                                                <asp:CheckBox ID="CheckBox2" runat="server" Text='<%#Eval ("Credentials_Name") %>' onclick='<%# string.Format("CustomCredentialsChanged(this,{0})", Eval("Credentials_ID")) %>' />
                                            </span>
                                        </ItemTemplate>
                                </telerik:RadComboBox>
                             <div id="spanCredentials"></div>
                            </td>                            
                        </tr> 
                    </table>
                </td>    
            </tr>        
            </table>
             <br />
                    <div>
                    <table width="100%" style="margin-top:-10px;"> 
                    <tr>
                         <td class="style1" colspan="2" style="color:Red;" align="center">Are you sure you want to delete this key contact?</td>
                    </tr>                        
                    </table>
            <div class="modalFormButtons" style="margin-top: -10px;">
                  <pinso:CustomButton TabIndex="11" ID="CustomButton1" runat="server" Text="Yes" width="50px" Visible="true" onclick="Delbtn_Click"/>                     
                  <pinso:CustomButton TabIndex="12" ID="Nobtn" width="50px" runat="server" Text="No" OnClientClick="javascript:CloseWin(); return false;"/>                   
            </div>
                    
        </ItemTemplate>
    </asp:FormView>
    <asp:EntityDataSource runat="server" ID="dsKDM" ConnectionString="name=PathfinderMillenniumEntities" DefaultContainerName="PathfinderMillenniumEntities" 
          EntitySetName="KDMDetailSet" AutoGenerateWhereClause="true">
        <WhereParameters>
            <asp:QueryStringParameter QueryStringField="KDM_ID" Name="KDM_ID" Type="Int32" ConvertEmptyStringToNull="true" />     
            <asp:QueryStringParameter QueryStringField="PlanID" Name="Plan_ID" Type="Int32" ConvertEmptyStringToNull="true" />       
        </WhereParameters>
        <InsertParameters>
            <asp:QueryStringParameter QueryStringField="KDM_ID" Name="KDM_ID" Type="Int32" ConvertEmptyStringToNull="true" />
            <asp:QueryStringParameter QueryStringField="PlanID" Name="Plan_ID" Type="Int32" ConvertEmptyStringToNull="true" />
        </InsertParameters>
        <UpdateParameters>
            <asp:QueryStringParameter QueryStringField="KDM_ID" Name="KDM_ID" Type="Int32" ConvertEmptyStringToNull="true" />
            <asp:QueryStringParameter QueryStringField="PlanID" Name="Plan_ID" Type="Int32" ConvertEmptyStringToNull="true" />           
        </UpdateParameters>
    </asp:EntityDataSource>
    <asp:EntityDataSource runat="server" ID="dsCustomsTitle" ConnectionString="name=PathfinderMillenniumEntities" DefaultContainerName="PathfinderMillenniumEntities" 
        EnableInsert="true" EntitySetName="CustomTitleSet" Where="it.Section_ID=105" OrderBy="it.Title_Name ">     
    </asp:EntityDataSource>  
    <asp:EntityDataSource runat="server" ID="dsCustomCredentials" ConnectionString="name=PathfinderMillenniumEntities" DefaultContainerName="PathfinderMillenniumEntities" 
        EnableInsert="true" EntitySetName="CustomCredentialsSet" Where="it.Section_ID=105" OrderBy="it.Credentials_Name">     
    </asp:EntityDataSource>    
    <asp:EntityDataSource runat="server" ID="dsCustomCAC" ConnectionString="name=PathfinderMillenniumEntities" DefaultContainerName="PathfinderMillenniumEntities" 
       AutoGenerateWhereClause="true" EnableInsert="true" EntitySetName="CustomCACSet">     
    </asp:EntityDataSource> 
    <div>
        <asp:Label ID="Msglbl" runat="server" Text="" Visible="false"></asp:Label>
        <asp:Label ID="Label2" runat="server" Text='<%= Request.Form["PlanID"]%>' Visible="false"></asp:Label>
    </div>
</asp:Content>