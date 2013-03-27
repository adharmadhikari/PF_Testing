<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Modal.master" AutoEventWireup="true"
    CodeFile="AddEditPlanInfoMJ.aspx.cs" Inherits="custom_millennium_todaysaccounts_all_AddEditPlanInfo" %>
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
    <asp:HiddenField ID="hdnMJ" runat="server" Value="" Visible="true" />
     
    <div id="AddPlanInfoMain" class="PlanInfoContainer" style="margin-top : 15px !important;">
    <asp:FormView runat="server" ID="formViewPlanInfo" CellPadding="0" Width="100%" DataSourceID="dsPlanInfo">
        <ItemTemplate>
            <table width="100%" >
                <tr>
                    <td valign="top">
                        <table align="left" >
                        
                            <tr align="left">
                                <td width="130px">
                                    <label id="OrganizationName"> Organization Name *</label>
                                </td>                                
                           
                                <td colspan="2" style="padding-left: 5px;padding-bottom : 2px;">                                   
                                     <asp:TextBox id="txtOrganizationName" MaxLength="200"  runat="server" Width="213px" TabIndex="1"
                                      Text='<%# Eval("Plan_Name") %>' />
                              </td>
                            </tr>
                            
                            <tr align="left">
                                <td width="130px"> 
                                    <label for="rdlMAC">MAC</label>
                                </td>
                                <td>
                                
                                    <telerik:RadComboBox ID="rdlMAC" runat="server" AppendDataBoundItems="true"
                                         Skin="pathfinder" Width="220px" SelectedValue='<%# (Boolean.Parse(Eval("MAC").ToString()))?"1" : "0" %>'
                                        TabIndex="2" DropDownWidth="220px"  EnableEmbeddedSkins="false">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="No" Value="0" Selected="true"/>
                                        <telerik:RadComboBoxItem Text="Yes" Value="1"/>
                                    </Items>   
                                    </telerik:RadComboBox>
                                </td>
                            </tr> 
                           
                            <tr align="left">
                                <td colspan="2"><label>State Covered *</label></td>                                
                            </tr>
                            <tr align="left">
                                <td colspan="2" style="padding-left: 5px;">
                                    <telerik:RadComboBox runat="server" ID="rdlStateCovered" DataSourceID="dsState"
                                         TabIndex="3" Skin="pathfinder" Width="343px" DropDownWidth="343px"
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
                                <td colspan="2">
                                    <label>Jurisditions Covered</label>
                                </td>
                            </tr>
                            <tr align="left">
                                <td colspan="2" style="padding-left: 5px;">
                                    <telerik:RadComboBox runat="server" ID="rdlJC" DataSourceID="dsJC"
                                         TabIndex="4" Skin="pathfinder" Width="343px" DropDownWidth="343px" 
                                        EnableEmbeddedSkins="false" DataTextField ="Jurisdiction_Name" DataValueField="Jurisdiction_ID"
                                        AppendDataBoundItems="true" AllowCustomText="True" Text="-Select Jurisditions Covered-"
                                         OnClientDropDownClosed="setJCText" OnClientLoad="setJCText">
                                        <ItemTemplate>
                                           <span id='<%# String.Format("m{0}",Eval("Jurisdiction_ID")) %>'>
                                           <asp:CheckBox runat="server" ID="chkMeetingOutcome" Text ='<%# Eval("Jurisdiction_Name") %>'
                                            onclick='<%# string.Format("JurisditionsCoveredChanged(this,{0})", Eval("Jurisdiction_ID")) %>' />
                                           </span>
                                        </ItemTemplate>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                           
                            <tr align="left">
                                <td colspan="2" style="padding-left: 5px;">
                                    <div id="spanJurisditionsCovered">                            
                                   </div>
                                </td>
                            </tr>
                               
                        </table>          
                    </td>
                    <td valign="top">
                        <table cellpadding="2" cellspacing="2" >
                         <%--<tr align="left">
                                <td width="130px">
                                    <label id="Label3"> RAM</label>
                                </td>                                
                           
                                <td colspan="2" style="padding-left: 5px;padding-bottom : 2px;">                                   
                                     <asp:TextBox id="TextBox1" MaxLength="200"  runat="server" Width="213px" ReadOnly="true"
                                      Text='<%# Eval("User_Name") %>' />
                              </td>
                            </tr>--%>
                        <tr align="left">
                                <td width="130px">
                                    <label id="Label1">Website</label>
                                </td>                                
                           
                                <td colspan="2" style="padding-left: 5px;padding-bottom : 2px;">                                   
                                     <asp:TextBox id="TxtWebsite"  MaxLength="150"  runat="server" Width="213px" TabIndex="5"
                                     Text='<%# Eval("Website") %>' />
                              </td>
                            </tr>    
                             <tr align="left">
                                <td width="130px">
                                    <label id="Address1">Address1</label>
                                </td>                                
                           
                                <td colspan="2" style="padding-left: 5px;padding-bottom : 2px;">                                   
                                     <asp:TextBox id="TxtAddress1"  MaxLength="150"  runat="server" Width="213px" TabIndex="6"
                                     Text='<%# Eval("Address1") %>' />
                              </td>
                            </tr>    
                             <tr align="left">
                                <td width="130px">
                                    <label id="Address2">Address2</label>
                                </td>                                
                            
                                <td colspan="2" style="padding-left: 5px;padding-bottom : 2px;">                                   
                                     <asp:TextBox id="TxtAddress2"  MaxLength="150"  runat="server" Width="213px" TabIndex="7"
                                     Text='<%# Eval("Address2") %>' />
                              </td>
                            </tr> 
                            <tr align="left">
                                <td width="130px">
                                    <label id="City">City</label>
                                </td>                                
                          
                                <td colspan="2" style="padding-left: 5px;padding-bottom : 2px;">                                   
                                     <asp:TextBox id="TxtCity"  MaxLength="100"  runat="server" Width="213px" TabIndex="8"
                                     Text='<%# Eval("City") %>' />
                              </td>
                            </tr>  
                                         
                            <tr align="left">
                                <td >
                                    <label for="rdlState" >State *</label><br />                                    
                                </td>
                                <td >
                                   <telerik:RadComboBox ID="rdlState" runat="server" AppendDataBoundItems="true" Width="220px"
                                        DropDownWidth="220px"  
                                        DataSourceID="dsState" TabIndex="9" Skin="pathfinder" EnableEmbeddedSkins="false" DataTextField="StateName"
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
                                     <asp:TextBox id="TxtZip"  MaxLength="5"  runat="server" Width="213px" TabIndex="10"
                                     Text='<%# Eval("Zip") %>' />
                              </td>
                            </tr>  
                            <tr align="left">
                                <td width="130px">
                                    <label id="Zip4">Zip+4</label>
                                </td>                                
                           
                                <td colspan="2" style="padding-left: 5px;padding-bottom : 2px;">                                   
                                     <asp:TextBox id="TxtZip4"  MaxLength="4"  runat="server" Width="213px" TabIndex="11"
                                     Text='<%# Eval("Zip_4") %>' />
                              </td>
                            </tr>        
                           <%--<tr align="left">
                                <td width="130px">
                                    <label id="Customer_Master_ID">Customer Master ID</label>
                                </td>                                
                           
                                <td colspan="2" style="padding-left: 5px;padding-bottom : 2px;">                                   
                                     <asp:TextBox id="Txt_Customer_Master_ID"  MaxLength="30" ReadOnly="true"  runat="server" Width="213px"
                                     Text='<%# Eval("Customer_Master_ID") %>' />
                              </td>
                            </tr>  --%> 
                    
                        </table>
                    </td>                  
                </tr>                
            </table>    
            <asp:requiredfieldvalidator ID="Requiredfieldvalidator6" controltovalidate="txtOrganizationName" display="none" InitialValue="" errormessage="Organization Name is Required" runat="server" />            
            <asp:requiredfieldvalidator ID="Requiredfieldvalidator2" controltovalidate="rdlStateCovered" display="none" InitialValue="-Select State Covered-" errormessage="State Covered is Required" runat="server" />
            <asp:requiredfieldvalidator ID="Requiredfieldvalidator3" controltovalidate="rdlState" display="none" InitialValue="-Select State-" errormessage="State is Required" runat="server" />
               
             <div style="height:15px">
                <asp:ValidationSummary runat="server" ID="validationSummary1" DisplayMode="List" />
            </div>
            <div class="modalFormButtons">
                 <pinso:CustomButton TabIndex="12" ID="CustomButton1" runat="server" Text="Update" Visible="true" OnClick="Editbtn_Click" />
                 <pinso:CustomButton ID="CustomButton2" TabIndex="13" runat="server" Text="Reset" OnClientClick="javascript:ClearForm(); return false;" />                     
            </div>
        </ItemTemplate>
        <InsertItemTemplate>
          <table width="100%" >
                <tr>
                    <td valign="top">
                        <table align="left" >
                       
                            <tr align="left">
                                <td width="130px">
                                    <label id="OrganizationName"> Organization Name *</label>
                                </td>                                
                           
                                <td colspan="2" style="padding-left: 5px;padding-bottom : 2px;">                                   
                                     <asp:TextBox id="txtOrganizationName" MaxLength="200"  runat="server" Width="213px" TabIndex="1"
                                      Text='<%# Eval("Plan_Name") %>' />
                              </td>
                            </tr>
                           
                            <tr align="left">
                                <td width="130px"> 
                                    <label for="rdlMAC">MAC</label>
                                </td>
                                <td>
                                
                                    <telerik:RadComboBox ID="rdlMAC" runat="server" AppendDataBoundItems="true"
                                         Skin="pathfinder" Width="220px" TabIndex="2"
                                        DropDownWidth="220px"  EnableEmbeddedSkins="false">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="No" Value="0" Selected="true"/>
                                        <telerik:RadComboBoxItem Text="Yes" Value="1"/>
                                    </Items>   
                                    </telerik:RadComboBox>
                                </td>
                            </tr> 
                           
                            <tr align="left">
                                <td colspan="2"><label>State Covered *</label></td>                                
                            </tr>
                            <tr align="left">
                                <td colspan="2" style="padding-left: 5px;">
                                    <telerik:RadComboBox runat="server" ID="rdlStateCovered" DataSourceID="dsState"
                                         TabIndex="3" Skin="pathfinder" Width="343px" DropDownWidth="343px"
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
                                <td colspan="2">
                                    <label>Jurisditions Covered</label>
                                </td>
                            </tr>
                            <tr align="left">
                                <td colspan="2" style="padding-left: 5px;">
                                    <telerik:RadComboBox runat="server" ID="rdlJC" DataSourceID="dsJC"
                                         TabIndex="4" Skin="pathfinder" Width="343px" DropDownWidth="343px" 
                                        EnableEmbeddedSkins="false" DataTextField ="Jurisdiction_Name" DataValueField="Jurisdiction_ID"
                                        AppendDataBoundItems="true" AllowCustomText="True" Text="-Select Jurisditions Covered-"
                                         OnClientDropDownClosed="setJCText" OnClientLoad="setJCText">
                                        <ItemTemplate>
                                           <span id='<%# String.Format("m{0}",Eval("Jurisdiction_ID")) %>'>
                                           <asp:CheckBox runat="server" ID="chkMeetingOutcome" Text ='<%# Eval("Jurisdiction_Name") %>'
                                            onclick='<%# string.Format("JurisditionsCoveredChanged(this,{0})", Eval("Jurisdiction_ID")) %>' />
                                           </span>
                                        </ItemTemplate>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                           
                            <tr align="left">
                                <td colspan="2" style="padding-left: 5px;">
                                    <div id="spanJurisditionsCovered">                            
                                   </div>
                                </td>
                            </tr>
                               
                        </table>          
                    </td>
                    <td valign="top">
                        <table cellpadding="2" cellspacing="2" >
                        <tr align="left">
                                <td width="130px">
                                    <label id="Label1">Website</label>
                                </td>                                
                           
                                <td colspan="2" style="padding-left: 5px;padding-bottom : 2px;">                                   
                                     <asp:TextBox id="TxtWebsite"  MaxLength="150"  runat="server" Width="213px" TabIndex="5"
                                     Text='<%# Eval("Website") %>' />
                              </td>
                            </tr>    
                             <tr align="left">
                                <td width="130px">
                                    <label id="Address1">Address1</label>
                                </td>                                
                           
                                <td colspan="2" style="padding-left: 5px;padding-bottom : 2px;">                                   
                                     <asp:TextBox id="TxtAddress1"  MaxLength="150"  runat="server" Width="213px" TabIndex="6"
                                     Text='<%# Eval("Address1") %>' />
                              </td>
                            </tr>    
                             <tr align="left">
                                <td width="130px">
                                    <label id="Address2">Address2</label>
                                </td>                                
                            
                                <td colspan="2" style="padding-left: 5px;padding-bottom : 2px;">                                   
                                     <asp:TextBox id="TxtAddress2"  MaxLength="150"  runat="server" Width="213px" TabIndex="7"
                                     Text='<%# Eval("Address2") %>' />
                              </td>
                            </tr> 
                            <tr align="left">
                                <td width="130px">
                                    <label id="City">City</label>
                                </td>                                
                          
                                <td colspan="2" style="padding-left: 5px;padding-bottom : 2px;">                                   
                                     <asp:TextBox id="TxtCity"  MaxLength="100"  runat="server" Width="213px" TabIndex="8"
                                     Text='<%# Eval("City") %>' />
                              </td>
                            </tr>  
                                         
                            <tr align="left">
                                <td >
                                    <label for="rdlState" >State *</label><br />                                    
                                </td>
                                <td >
                                   <telerik:RadComboBox ID="rdlState" runat="server" AppendDataBoundItems="true" Width="220px"
                                        DropDownWidth="220px"  
                                        DataSourceID="dsState" TabIndex="9" Skin="pathfinder" EnableEmbeddedSkins="false" DataTextField="StateName"
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
                                     <asp:TextBox id="TxtZip"  MaxLength="5"  runat="server" Width="213px" TabIndex="10"
                                     Text='<%# Eval("Zip") %>' />
                              </td>
                            </tr>  
                            <tr align="left">
                                <td width="130px">
                                    <label id="Zip4">Zip+4</label>
                                </td>                                
                           
                                <td colspan="2" style="padding-left: 5px;padding-bottom : 2px;">                                   
                                     <asp:TextBox id="TxtZip4"  MaxLength="4"  runat="server" Width="213px" TabIndex="11"
                                     Text='<%# Eval("Zip_4") %>' />
                              </td>
                            </tr>        
                          
                    
                        </table>
                    </td>                  
                </tr>                
            </table>  
             <asp:requiredfieldvalidator ID="Requiredfieldvalidator6" controltovalidate="txtOrganizationName" display="none" InitialValue="" errormessage="Organization Name is Required" runat="server" />            
            <asp:requiredfieldvalidator ID="Requiredfieldvalidator2" controltovalidate="rdlStateCovered" display="none" InitialValue="-Select State Covered-" errormessage="State Covered is Required" runat="server" />
            <asp:requiredfieldvalidator ID="Requiredfieldvalidator3" controltovalidate="rdlState" display="none" InitialValue="-Select State-" errormessage="State is Required" runat="server" />
                         
            <div style="height:15px">
                <asp:ValidationSummary runat="server" ID="validationSummary" DisplayMode="List" />
            </div>
            <div class="modalFormButtons">                
                <pinso:CustomButton TabIndex="12" ID="btnEdit" runat="server" Text="Add" Visible="true" OnClick="Editbtn_Click" />
                <pinso:CustomButton ID="btnReset" TabIndex="13" runat="server" Text="Reset" OnClientClick="javascript:ClearForm(); return false;" />                       
            </div>
        </InsertItemTemplate>
    </asp:FormView>
    </div>
    <asp:EntityDataSource runat="server" ID="dsState" ConnectionString="name=PathfinderMillenniumEntities"
        DefaultContainerName="PathfinderMillenniumEntities" EntitySetName="LkpStateSet"
        EntityTypeFilter="LkpState" OrderBy="it.StateName" >
    </asp:EntityDataSource>
    <asp:EntityDataSource runat="server" ID="dsJC" ConnectionString="name=PathfinderMillenniumEntities"
        DefaultContainerName="PathfinderMillenniumEntities" 
        EntitySetName="LkpCustomJurisdictionSet" >
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
