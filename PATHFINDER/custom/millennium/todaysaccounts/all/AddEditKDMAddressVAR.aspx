<%@ Page Language="C#" MasterPageFile="~/MasterPages/Modal.master"  AutoEventWireup="true" CodeFile="AddEditKDMAddressVAR.aspx.cs" Inherits="custom_millennium_todaysaccounts_all_AddEditKDMAddressVAR" %>



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
    <asp:HiddenField Id="hdnStatesCovered" runat="server" Value="" Visible="true" />
    <asp:HiddenField ID="hdnMJ" runat="server" Value="" Visible="true" />   
    <asp:FormView runat="server" ID="formKDMView" DefaultMode="Insert" CellPadding="0" Width="100%" DataSourceID="dsKDM">
        <InsertItemTemplate>
             <table class="genTable">
            <tr>
                <td valign="top">
                    <table align="left" >
                    <tr align="left">
                        <td><label for="Address1txt">Address 1 *</label>
                            <asp:TextBox TabIndex="1" ID="Address1txt" runat="server" MaxLength="150" Width="331px" Text='<%# Bind("KDM_Address1") %>'></asp:TextBox>
                        </td>
                    </tr>
                    <tr>    
                        <td><label for="Address2txt">Address 2</label>
                            <asp:TextBox TabIndex="2" ID="Address2txt" runat="server" MaxLength="150" Width="331px" Text='<%# Bind("KDM_Address2") %>'></asp:TextBox>
                        
                        </td>
                    </tr>
                    <tr>
                        <td><label for="Citytxt">City</label>
                            <asp:TextBox TabIndex="3" ID="Citytxt" runat="server" MaxLength="100" Width="237px" Text='<%# Bind("KDM_City") %>'></asp:TextBox>
                        </td>
                    </tr>              
                    </table>
                </td>
                <td valign="top">
                    <table>
                         <tr>
                             <td><label for="rdlState">State *</label>
                              <%--<asp:TextBox TabIndex="16" ID="Statetxt" runat="server" Width="111px" Text='<%# Bind("KDM_State") %>'></asp:TextBox>--%>
                             <telerik:RadComboBox ID="rdlState" runat="server" AppendDataBoundItems="true" Width="220px"
                                        DropDownWidth="220px"  
                                        DataSourceID="dsState" TabIndex="4" Skin="pathfinder" EnableEmbeddedSkins="false" DataTextField="StateName"
                                        DataValueField="StateAbbrev" SelectedValue='<%# Bind("KDM_State") %>' >
                                    <Items>
                                        <telerik:RadComboBoxItem Text="-Select State-" Value="" Selected="true" />
                                    </Items>   
                             </telerik:RadComboBox>
                             </td>
                             </td>
                        </tr>
                        <tr> 
                            <td><label for="Ziptxt">Zip *</label>
                                <asp:TextBox ID="Ziptxt" TabIndex="5" runat="server" MaxLength="5" Width="111px" Text='<%# Bind("KDM_Zip") %>'></asp:TextBox>
                            </td>
                        </tr> 
                        <tr> 
                            <td><label for="Zip4txt">Zip + 4</label>
                                <asp:TextBox ID="Zip4txt" TabIndex="6" runat="server" MaxLength="4" Width="111px" Text='<%# Bind("KDM_Zip_4") %>'></asp:TextBox>
                            </td>
                        </tr> 

                    </table>
                </td>    
            </tr>        
            </table>    
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="&bull;&nbsp;Address 1 Required" ControlToValidate="Address1txt" Display="None" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="&bull;&nbsp;State Required" Display="None"  InitialValue="-Select State-" ControlToValidate="rdlState"></asp:RequiredFieldValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="&bull;&nbsp;Zip Required" ControlToValidate="Ziptxt" Display="None" />
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"   ErrorMessage="&bull;&nbsp;Valid Zip Required." Display="None"  ControlToValidate="Ziptxt" ValidationExpression="^([0-9]{5})$" />
            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"   ErrorMessage="&bull;&nbsp;Valid Zip + 4  Required." Display="None"  ControlToValidate="Zip4txt" ValidationExpression="^([0-9]{4})$" />
             <div style="height:15px">
                        <asp:ValidationSummary runat="server" ID="validationSummary" DisplayMode="SingleParagraph" />
             </div> 
             <div class="modalFormButtons">                
                <pinso:CustomButton TabIndex="7" ID="btnEdit" runat="server" Text="Add" Visible="true" OnClick="Addbtn_Click" />
                <pinso:CustomButton ID="btnReset" TabIndex="8" runat="server" Text="Reset" OnClientClick="javascript:ClearForm(); return false;" />                       
            </div>        
        </InsertItemTemplate>
        <EditItemTemplate>
            <table class="genTable">
            <tr>
                <td valign="top">
                    <table align="left" >
                    <tr align="left">
                        <td><label for="Address1txt">Address 1 *</label>
                            <asp:TextBox TabIndex="1" ID="Address1txt" runat="server" MaxLength="150" Width="331px" Text='<%# Bind("KDM_Address1") %>'></asp:TextBox>
                        </td>
                    </tr>
                    <tr>    
                        <td><label for="Address2txt">Address 2</label>
                            <asp:TextBox TabIndex="2" ID="Address2txt" runat="server" MaxLength="150" Width="331px" Text='<%# Bind("KDM_Address2") %>'></asp:TextBox>
                        
                        </td>
                    </tr>
                    <tr>
                        <td><label for="Citytxt">City</label>
                            <asp:TextBox TabIndex="3" ID="Citytxt" runat="server" MaxLength="100" Width="237px" Text='<%# Bind("KDM_City") %>'></asp:TextBox>
                        </td>
                    </tr>              
                    </table>
                </td>
                <td valign="top">
                    <table>
                         <tr>                        
                            <td><label for="rdlState">State *</label>
                              <%--<asp:TextBox TabIndex="16" ID="Statetxt" runat="server" Width="111px" Text='<%# Bind("KDM_State") %>'></asp:TextBox>--%>
                             <telerik:RadComboBox ID="rdlState" runat="server" AppendDataBoundItems="true" Width="220px"
                                        DropDownWidth="220px"  
                                        DataSourceID="dsState" TabIndex="4" Skin="pathfinder" EnableEmbeddedSkins="false" DataTextField="StateName"
                                        DataValueField="StateAbbrev" SelectedValue='<%# Bind("KDM_State") %>' >
                                    <Items>
                                        <telerik:RadComboBoxItem Text="-Select State-" Value="" Selected="true" />
                                    </Items>   
                             </telerik:RadComboBox>
                             </td>
                        </tr>
                        <tr> 
                            <td><label for="Ziptxt">Zip *</label>
                                <asp:TextBox ID="Ziptxt" TabIndex="5" runat="server" MaxLength="5" Width="111px" Text='<%# Bind("KDM_Zip") %>'></asp:TextBox>
                            </td>
                        </tr> 
                        <tr> 
                            <td><label for="Zip4txt">Zip + 4</label>
                                <asp:TextBox ID="Zip4txt" TabIndex="6" runat="server" MaxLength="4" Width="111px" Text='<%# Bind("KDM_Zip_4") %>'></asp:TextBox>
                            </td>
                        </tr> 

                    </table>
                </td>    
            </tr>        
            </table>  
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="&bull;&nbsp;Address 1 Required" ControlToValidate="Address1txt" Display="None" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="&bull;&nbsp;State Required" Display="None"  InitialValue="-Select State-" ControlToValidate="rdlState"></asp:RequiredFieldValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="&bull;&nbsp;Zip Required" ControlToValidate="Ziptxt" Display="None" />
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"   ErrorMessage="&bull;&nbsp;Valid Zip Required." Display="None"  ControlToValidate="Ziptxt" ValidationExpression="^([0-9]{5})$" />
            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"   ErrorMessage="&bull;&nbsp;Valid Zip + 4  Required." Display="None"  ControlToValidate="Zip4txt" ValidationExpression="^([0-9]{4})$" />
             <div style="height:15px">
                        <asp:ValidationSummary runat="server" ID="validationSummary" DisplayMode="SingleParagraph" />
             </div>
            <div class="modalFormButtons">              
                  <%--<pinso:CustomButton TabIndex="20" ID="Updbtn" runat="server" Text="Update" width="50px" Visible="true" CommandName="Editbtn_Click" />--%>               
                  <pinso:CustomButton TabIndex="7" ID="Delbtn" runat="server" Text="Update" width="50px" Visible="true" onclick="Editbtn_Click"/>                     
                  <pinso:CustomButton TabIndex="8" ID="Nobtn" width="50px" runat="server" Text="Cancel" OnClientClick="javascript:CloseWin(); return false;"/>                   
            </div>
        </EditItemTemplate>
        <ItemTemplate>
            <table class="genTable">
            <tr>
                <td valign="top">
                    <table align="left" >
                    <tr align="left">
                        <td><label for="Address1txt">Address 1 *</label>
                            <asp:TextBox TabIndex="1" ID="Address1txt" runat="server" Width="331px" Text='<%# Bind("KDM_Address1") %>'></asp:TextBox>
                        </td>
                    </tr>
                    <tr>    
                        <td><label for="Address2txt">Address 2</label>
                            <asp:TextBox TabIndex="2" ID="Address2txt" runat="server" Width="237px" Text='<%# Bind("KDM_Address2") %>'></asp:TextBox>
                        
                        </td>
                    </tr>
                    <tr>
                        <td><label for="Citytxt">City</label>
                            <asp:TextBox TabIndex="3" ID="Citytxt" runat="server" Width="237px" Text='<%# Bind("KDM_City") %>'></asp:TextBox>
                        </td>
                    </tr>              
                    </table>
                </td>
                <td valign="top">
                    <table>
                         <tr>                        
                            <td><label for="rdlState">State *</label>
                              <%--<asp:TextBox TabIndex="16" ID="Statetxt" runat="server" Width="111px" Text='<%# Bind("KDM_State") %>'></asp:TextBox>--%>
                             <telerik:RadComboBox ID="rdlState" runat="server" AppendDataBoundItems="true" Width="220px"
                                        DropDownWidth="220px"  
                                        DataSourceID="dsState" TabIndex="4" Skin="pathfinder" EnableEmbeddedSkins="false" DataTextField="StateName"
                                        DataValueField="StateAbbrev" SelectedValue='<%# Bind("KDM_State") %>' >
                                    <Items>
                                        <telerik:RadComboBoxItem Text="-Select State-" Value="" Selected="true" />
                                    </Items>   
                             </telerik:RadComboBox>
                             </td>
                        </tr>
                        <tr> 
                            <td><label for="Ziptxt">Zip *</label>
                                <asp:TextBox ID="Ziptxt" TabIndex="5" runat="server" Width="111px" Text='<%# Bind("KDM_Zip") %>'></asp:TextBox>
                            </td>
                        </tr> 
                        <tr> 
                            <td><label for="Zip4txt">Zip + 4</label>
                                <asp:TextBox ID="Zip4txt" TabIndex="6" runat="server" Width="111px" Text='<%# Bind("KDM_Zip_4") %>'></asp:TextBox>
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
                         <td class="style1" colspan="2" style="color:Red;" align="center">Are you sure you want to delete this Address?</td>
                    </tr>

                        
                    </table>
            <div class="modalFormButtons">              
                                 
                  <pinso:CustomButton TabIndex="7" ID="CustomButton1" runat="server" Text="Yes" width="50px" Visible="true" onclick="Delbtn_Click"/>                     
                  <pinso:CustomButton TabIndex="8" ID="Nobtn" width="50px" runat="server" Text="No" OnClientClick="javascript:CloseWin(); return false;"/>                   
            </div>
                    
        </ItemTemplate>
    </asp:FormView>
    <asp:EntityDataSource runat="server" ID="dsKDM" ConnectionString="name=PathfinderMillenniumEntities" DefaultContainerName="PathfinderMillenniumEntities" 
          EntitySetName="KDMAddressSet" AutoGenerateWhereClause="true">
        <WhereParameters>
        <asp:QueryStringParameter QueryStringField="KDM_ADD_ID" Name="ID" Type="Int32" ConvertEmptyStringToNull="true" />
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
    <asp:EntityDataSource runat="server" ID="dsState" ConnectionString="name=PathfinderMillenniumEntities"
        DefaultContainerName="PathfinderMillenniumEntities" EntitySetName="LkpStateSet"
        EntityTypeFilter="LkpState" OrderBy="it.StateName" >
    </asp:EntityDataSource>
    
    
    <div>
        <asp:Label ID="Msglbl" runat="server" Text="" Visible="false"></asp:Label>
        <asp:Label ID="Label2" runat="server" Text='<%= Request.Form["PlanID"]%>' Visible="false"></asp:Label>
    </div>
</asp:Content>