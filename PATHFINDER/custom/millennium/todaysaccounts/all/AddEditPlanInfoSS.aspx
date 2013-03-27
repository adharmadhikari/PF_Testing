<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Modal.master" AutoEventWireup="true"
    CodeFile="AddEditPlanInfoSS.aspx.cs" Inherits="custom_millennium_todaysaccounts_all_AddEditPlanInfo" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register src="~/custom/millennium/todaysaccounts/controls/AddEditPlanInfoScript.ascx" tagname="PlanInfoScript" tagprefix="pinso" %>
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
    <pinso:PlanInfoScript ID ="PlanInfoScript1" runat="server" />    
</asp:Content><asp:Content ID="Content3" ContentPlaceHolderID="tools" runat="server">  
     </asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="main" runat="Server">
    <asp:HiddenField ID="PlanNameHdn" runat="server" Value="" Visible="false" />
    <asp:HiddenField Id="hdnStatesCovered" runat="server" Value="" Visible="true" />
    
      
    <div id="AddPlanInfoMain" class="PlanInfoContainer" style="margin-top : 15px !important;">
    <asp:FormView runat="server" ID="formViewPlanInfo" CellPadding="5" Width="100%" DataSourceID="dsPlanInfo">
        <ItemTemplate>
            <table width="100%" >
                <tr>
                    <td valign="top">
                        <table align="left" >
                       
                       <%-- <tr align="left">
                                <td width="130px">
                                    <label id="Label3"> RAM/FAM/NAE</label>
                                </td>                                
                           
                                <td colspan="2">                                   
                                     <asp:TextBox id="TextBox1" MaxLength="200"  runat="server" Width="177px" ReadOnly="true"
                                      Text='<%# Eval("User_Name") %>' />
                              </td>
                            </tr>--%>
                            <tr align="left">
                                <td width="130px">
                                    <label id="OrganizationName"> Society Name *</label>
                                </td>                                
                           
                                <td colspan="2" >                                   
                                     <asp:TextBox id="txtOrganizationName" MaxLength="200"  runat="server" Width="177px" TabIndex="1"
                                      Text='<%# Eval("Plan_Name") %>' />
                              </td>
                            </tr>
                           
                             <tr align="left">
                                <td colspan="2"><label>State Covered *</label></td>                                
                            </tr>
                            <tr align="left">
                                <td colspan="2" style="padding-left: 5px;">
                                    <telerik:RadComboBox runat="server" ID="rdlStateCovered" DataSourceID="dsState"
                                         TabIndex="2" Skin="pathfinder" Width="343px" DropDownWidth="343px"
                                         EnableEmbeddedSkins="false" DataTextField="StateName" DataValueField="StateFips"
                                         AppendDataBoundItems="true" AllowCustomText="True" Text="-Select State Covered-" 
                                         OnClientDropDownClosed="setStatesCoveredText" OnClientLoad="setStatesCoveredText">
                                         <ItemTemplate>
                                            <span id='<%# String.Format("p{0}",Eval("StateFips")) %>'>
                                               <asp:CheckBox runat="server" ID="chkStateCovered" Text ='<%# Eval("StateName") %>' 
                                                             onclick='<%# string.Format("StatesCoveredChanged(this,{0})", Eval("StateFips")) %>' 
                                                              />
                                               </span>
                                         </ItemTemplate>
                                      </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr align="left" valign="top">
                                <td colspan="2" style="padding-left: 5px; ">
                                    <div id="spanState">                                                
                                    </div>
                                </td>
                            </tr>  
                            <tr align="left">
                                <td width="130px">
                                    <label id="Website">Website</label>
                                </td>                                
                           
                                <td colspan="2" >                                   
                                     <asp:TextBox id="TxtWebsite"  MaxLength="150"  runat="server" Width="177px" TabIndex="3"
                                     Text='<%# Eval("Website") %>' />
                              </td>
                            </tr>            
                        </table>          
                    </td>
                    <td valign="top">
                        <table cellpadding="2" cellspacing="2" >
                             <tr align="left">
                                <td width="130px">
                                    <label id="Address1">Address1</label>
                                </td>                                
                           
                                <td colspan="2" style="padding-left: 5px;padding-bottom : 2px;">                                   
                                     <asp:TextBox id="TxtAddress1"  MaxLength="150"  runat="server" Width="213px" TabIndex="4"
                                     Text='<%# Eval("Address1") %>' />
                              </td>
                            </tr>    
                             <tr align="left">
                                <td width="130px">
                                    <label id="Address2">Address2</label>
                                </td>                                
                            
                                <td colspan="2" style="padding-left: 5px;padding-bottom : 2px;">                                   
                                     <asp:TextBox id="TxtAddress2"  MaxLength="150"  runat="server" Width="213px" TabIndex="5"
                                     Text='<%# Eval("Address2") %>' />
                              </td>
                            </tr> 
                            <tr align="left">
                                <td width="130px">
                                    <label id="City">City</label>
                                </td>                                
                          
                                <td colspan="2" style="padding-left: 5px;padding-bottom : 2px;">                                   
                                     <asp:TextBox id="TxtCity"  MaxLength="100"  runat="server" Width="213px" TabIndex="6"
                                     Text='<%# Eval("City") %>' />
                              </td>
                            </tr>  
                                         
                            <tr align="left">
                                <td >
                                    <label for="rdlMeetingType" >State *</label><br />                                    
                                </td>
                                <td >
                                   <telerik:RadComboBox ID="rdlState" runat="server" AppendDataBoundItems="true" Width="220px"
                                        DropDownWidth="220px"  
                                        DataSourceID="dsState" TabIndex="7" Skin="pathfinder" EnableEmbeddedSkins="false" DataTextField="StateName"
                                        DataValueField="StateAbbrev" SelectedValue='<%# Eval("State") %>' >
                                    <Items>
                                        <telerik:RadComboBoxItem Text="-Select State-" Value="" Selected="true" />
                                    </Items>   
                                    </telerik:RadComboBox>
                                </td>
                            </tr> 
                             <tr align="left">
                                <td width="130px">
                                    <label id="Zip">Zip</label>
                                </td>                                
                           
                                <td colspan="2" style="padding-left: 5px;padding-bottom : 2px;">                                   
                                     <asp:TextBox id="TxtZip"  MaxLength="5"  runat="server" Width="213px" TabIndex="8"
                                     Text='<%# Eval("Zip") %>' />
                              </td>
                            </tr>  
                            <tr align="left">
                                <td width="130px">
                                    <label id="Zip4">Zip+4</label>
                                </td>                                
                           
                                <td colspan="2" style="padding-left: 5px;padding-bottom : 2px;">                                   
                                     <asp:TextBox id="TxtZip4"  MaxLength="4"  runat="server" Width="213px" TabIndex="9"
                                     Text='<%# Eval("Zip_4") %>' />
                              </td>
                            </tr>        
                          <%-- <tr align="left">
                                <td width="130px">
                                    <label id="Customer_Master_ID">Customer Master ID</label>
                                </td>                                
                           
                                <td colspan="2" style="padding-left: 5px;padding-bottom : 2px;">                                   
                                     <asp:TextBox id="Txt_Customer_Master_ID"  MaxLength="30" ReadOnly="true"  runat="server" Width="213px"
                                     Text='<%# Eval("Customer_Master_ID") %>' />
                              </td>
                            </tr>   --%>
                     <%--<tr align="left">
                                <td width="130px">
                                    <label id="TerritoryAlignment">Territory Alignment </label>
                                </td>                                
                           
                                <td colspan="2" style="padding-left: 5px;padding-bottom : 2px;">                                   
                                     <asp:TextBox id="TxtTerritoryAlignment"  MaxLength="30" ReadOnly="true"  runat="server" Width="213px"
                                     Text='<%# Eval("Territory_Name") %>' />
                              </td>
                            </tr> --%>
                        </table>
                    </td>                  
                </tr>                
            </table>    
            <asp:requiredfieldvalidator ID="Requiredfieldvalidator6" controltovalidate="txtOrganizationName" display="none" InitialValue="" errormessage="Society Name is Required" runat="server" />            
            <asp:requiredfieldvalidator ID="Requiredfieldvalidator2" controltovalidate="rdlStateCovered" display="none" InitialValue="-Select State Covered-" errormessage="State Covered is Required" runat="server" />
            <asp:requiredfieldvalidator ID="Requiredfieldvalidator3" controltovalidate="rdlState" display="none" InitialValue="-Select State-" errormessage="State is Required" runat="server" />
             <br /><br />        
             <div style="height:15px">
                <asp:ValidationSummary runat="server" ID="validationSummary1" DisplayMode="List" />
            </div>
            <div class="modalFormButtons">
                 <pinso:CustomButton TabIndex="10" ID="CustomButton1" runat="server" Text="Update" Visible="true" OnClick="Editbtn_Click" />
                 <pinso:CustomButton ID="CustomButton2" TabIndex="11" runat="server" Text="Reset" OnClientClick="javascript:ClearForm(); return false;" />                     
            </div>
        </ItemTemplate>
        <InsertItemTemplate>
           <table width="100%" >
                <tr>
                    <td valign="top">
                        <table align="left" >
                       
                            <tr align="left">
                                <td width="130px">
                                    <label id="OrganizationName"> Society Name *</label>
                                </td>                                
                           
                                <td colspan="2">                                   
                                     <asp:TextBox id="txtOrganizationName" MaxLength="200"  runat="server" Width="177px"
                                      TabIndex="1" Text='<%# Eval("Plan_Name") %>' />
                              </td>
                            </tr>
                           
                           
                            <tr align="left">
                                <td colspan="2"><label>State Covered *</label></td>                                
                            </tr>
                            <tr align="left">
                                <td colspan="2" style="padding-left: 5px;">
                                    <telerik:RadComboBox runat="server" ID="rdlStateCovered" DataSourceID="dsState"
                                         TabIndex="2" Skin="pathfinder" Width="343px" DropDownWidth="343px"
                                         EnableEmbeddedSkins="false" DataTextField="StateName" DataValueField="StateFips"
                                         AppendDataBoundItems="true" AllowCustomText="True" Text="-Select State Covered-" 
                                         OnClientDropDownClosed="setStatesCoveredText" OnClientLoad="setStatesCoveredText">
                                         <ItemTemplate>
                                            <span id='<%# String.Format("p{0}",Eval("StateFips")) %>'>
                                               <asp:CheckBox runat="server" ID="chkStateCovered" Text ='<%# Eval("StateName") %>' 
                                                             onclick='<%# string.Format("StatesCoveredChanged(this,{0})", Eval("StateFips")) %>' 
                                                              />
                                               </span>
                                         </ItemTemplate>
                                      </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr align="left" valign="top">
                                <td colspan="2" style="padding-left: 5px; ">
                                    <div id="spanState">                                                
                                    </div>
                                </td>
                            </tr>  
                          
                            <tr align="left">
                                <td width="130px">
                                    <label id="Website">Website</label>
                                </td>                                
                           
                                <td colspan="2">                                   
                                     <asp:TextBox id="TxtWebsite"  MaxLength="150"  runat="server" Width="177px"
                                     TabIndex="3" Text='<%# Eval("Website") %>' />
                              </td>
                            </tr>            
                        </table>          
                    </td>
                    <td valign="top">
                        <table cellpadding="2" cellspacing="2" >
                             <tr align="left">
                                <td width="130px">
                                    <label id="Address1">Address1</label>
                                </td>                                
                           
                                <td colspan="2" style="padding-left: 5px;padding-bottom : 2px;">                                   
                                     <asp:TextBox id="TxtAddress1"  MaxLength="150"  runat="server" Width="213px"
                                     TabIndex="4" Text='<%# Eval("Address1") %>' />
                              </td>
                            </tr>    
                             <tr align="left">
                                <td width="130px">
                                    <label id="Address2">Address2</label>
                                </td>                                
                            
                                <td colspan="2" style="padding-left: 5px;padding-bottom : 2px;">                                   
                                     <asp:TextBox id="TxtAddress2"  MaxLength="150"  runat="server" Width="213px"
                                     TabIndex="5" Text='<%# Eval("Address2") %>' />
                              </td>
                            </tr> 
                            <tr align="left">
                                <td width="130px">
                                    <label id="City">City</label>
                                </td>                                
                          
                                <td colspan="2" style="padding-left: 5px;padding-bottom : 2px;">                                   
                                     <asp:TextBox id="TxtCity"  MaxLength="100"  runat="server" Width="213px"
                                     TabIndex="6" Text='<%# Eval("City") %>' />
                              </td>
                            </tr>  
                                         
                            <tr align="left">
                                <td >
                                    <label for="rdlMeetingType" >State *</label><br />                                    
                                </td>
                                <td >
                                   <telerik:RadComboBox ID="rdlState" runat="server" AppendDataBoundItems="true" Width="220px"
                                        DropDownWidth="220px"  
                                        DataSourceID="dsState" TabIndex="7" Skin="pathfinder" EnableEmbeddedSkins="false" DataTextField="StateName"
                                        DataValueField="StateAbbrev" SelectedValue='<%# Eval("State") %>' >
                                    <Items>
                                        <telerik:RadComboBoxItem Text="-Select State-" Value="" Selected="true" />
                                    </Items>   
                                    </telerik:RadComboBox>
                                </td>
                            </tr> 
                             <tr align="left">
                                <td width="130px">
                                    <label id="Zip">Zip</label>
                                </td>                                
                           
                                <td colspan="2" style="padding-left: 5px;padding-bottom : 2px;">                                   
                                     <asp:TextBox id="TxtZip"  MaxLength="5"  runat="server" Width="213px"
                                     TabIndex="8" Text='<%# Eval("Zip") %>' />
                              </td>
                            </tr>  
                            <tr align="left">
                                <td width="130px">
                                    <label id="Zip4">Zip+4</label>
                                </td>                                
                           
                                <td colspan="2" style="padding-left: 5px;padding-bottom : 2px;">                                   
                                     <asp:TextBox id="TxtZip4"  MaxLength="4"  runat="server" Width="213px"
                                     TabIndex="9" Text='<%# Eval("Zip_4") %>' />
                              </td>
                            </tr>        
                          
                        </table>
                    </td>                  
                </tr>                
            </table>   
            <asp:requiredfieldvalidator ID="Requiredfieldvalidator6" controltovalidate="txtOrganizationName" display="none" InitialValue="" errormessage="Society Name is Required" runat="server" />            
            <asp:requiredfieldvalidator ID="Requiredfieldvalidator2" controltovalidate="rdlStateCovered" display="none" InitialValue="-Select State Covered-" errormessage="State Covered is Required" runat="server" />
            <asp:requiredfieldvalidator ID="Requiredfieldvalidator3" controltovalidate="rdlState" display="none" InitialValue="-Select State-" errormessage="State is Required" runat="server" />
            <br /><br />
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
    <asp:EntityDataSource runat="server" ID="dsState" ConnectionString="name=PathfinderMillenniumEntities"
        DefaultContainerName="PathfinderMillenniumEntities" EntitySetName="LkpStateSet"
        EntityTypeFilter="LkpState" OrderBy="it.StateName">
    </asp:EntityDataSource>
    
   
    <asp:EntityDataSource runat="server" ID="dsPlanInfo" ConnectionString="name=PathfinderMillenniumEntities"
        DefaultContainerName="PathfinderMillenniumEntities" 
        EntitySetName="PlansClientViewSet" AutoGenerateWhereClause="True">
        <WhereParameters>
            <asp:QueryStringParameter QueryStringField="PlanID" Name="Plan_ID" Type="Int32" ConvertEmptyStringToNull="true" />
            
        </WhereParameters>
        <InsertParameters>
            <asp:QueryStringParameter QueryStringField="PlanID" Name="Plan_ID" Type="Int32" ConvertEmptyStringToNull="true" />
        </InsertParameters>
        <UpdateParameters>
            <asp:QueryStringParameter QueryStringField="PlanID" Name="Plan_ID" Type="Int32" ConvertEmptyStringToNull="true" />
           
        </UpdateParameters>
    </asp:EntityDataSource>
    
    
    <div>
        <asp:Label ID="Msglbl" runat="server" Text="" Visible="false"></asp:Label>
        <asp:Label ID="Label2" runat="server" Text='<%= Request.Form["PlanID"]%>' Visible="false"></asp:Label>
    </div>
</asp:Content>
