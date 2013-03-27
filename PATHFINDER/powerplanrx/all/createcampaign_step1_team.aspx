<%@ Page Title="Campaign Team" Language="C#" MasterPageFile="~/powerplanrx/MasterPages/MasterPage.master" Theme="impact" AutoEventWireup="true" CodeFile="createcampaign_step1_team.aspx.cs" Inherits="createcampaign_step1_team" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" Runat="Server">
 
<asp:FormView ID="frmCampaignInfo" runat="server" DataSourceID="dsCampaignTeam" HorizontalAlign="Center" Width="100%">
    <ItemTemplate>        
        <div class="tileContainerHeader">
            <div class="CampaignInfo">Campaign Name: <asp:label ID="lblCampaignName" runat="server"  Text ='<%# Eval("Campaign_Name")%>'></asp:label>
            </div>
        </div>
    </ItemTemplate>       
</asp:FormView>


   <div class="tileSubHeader"><div class="labelSubHeader">Campaign Team</div>
    <div class="toolTipControls">
        <%--<asp:Button ID="btnEmailTeam" runat="server" CssClass="btnEmailTeam" OnClick="btnEmailTeam_Click" Visible="false" /> --%>
        <asp:Button ID="btnEmailTeam" runat="server" CssClass="btnEmailTeam" OnClientClick="return showEmailModal()" Visible="false" />
        <asp:Button ID="Button1" runat="server" CssClass="btnInfo"  /> </div>
        <div id="instructions1" class="toolTipInfo">Please select a function area and employee name from the drop down lists and click "Add to Team" button.  You can add multiple members for each function area. </div>
         <div id="instructions2" class="toolTipInfo">Send to Team and Sr. Management </div>
  </div>  

<asp:FormView ID="frmCampaignTeam" runat="server" DataSourceID="dsCampaignTeam" Width="100%" Visible="false">
    <ItemTemplate>
    
        <table class="CampaignTeam" cellpadding="0" cellspacing="0">
            <tr>
                <td class="label"><b>Function Area:</b></td><td> <telerik:RadComboBox  EnableEmbeddedSkins="false"  SkinID="impactGen" ID="drpFunctionArea" runat="server" DataSourceID="dsFunctionArea" DataTextField="Title_Name" 
        DataValueField="Title_ID" AutoPostBack="true" AppendDataBoundItems="true" 
        Width="180">
            <Items>
                <telerik:RadComboBoxItem  Value="0" Text="-- Select a Function Area --"  Selected="true" />
            </Items>
        </telerik:RadComboBox>        
                </td>
                <td class="label"><b>Employee Name:</b></td><td> 
                <telerik:RadComboBox EnableEmbeddedSkins="false"  SkinID="impactGen" ID="drpEmpName" runat="server" DataSourceID="dsEmpNames" DataTextField="Full_Name"
        DataValueField="Territory_ID"  AppendDataBoundItems="true" Width="200">
            <Items>
                <telerik:RadComboBoxItem  Value="0" Text="-- Select Employee Name --"  Selected="true"/>
            </Items>
        </telerik:RadComboBox> </td>
                <td> <asp:Button SkinID="addButton" ID="btnAddToTeam" runat="server" Text="Add To Team" OnClick="btnAddToTeam_Click" /></td>
            </tr>
        </table>
        
        
                  
        
        
                           
                
  </ItemTemplate>
</asp:FormView>

<asp:Panel ID="pnlTeamAlert" runat="server" Visible="false">
    <div class="FunctionAreas"></div>
    <asp:Label ID="lblTeamAlert" runat="server" Text="Please select the Function Area and Employee Name for the new Team Member." >
 </asp:Label>
           
</asp:Panel>

<asp:GridView ID="grvCampaignTeam" DataSourceID="dsCampaignTeam" runat="server" Width="100%" AutoGenerateColumns="False" CssClass="FunctionAreas"  DataKeyNames="Territory_ID" AllowSorting="True"  EmptyDataText="No Campaign Team members have been added">
   <Columns>
        <asp:BoundField DataField="Territory_ID" Visible="false" />
        <asp:BoundField DataField="Title_Name" HeaderText="Function Area" SortExpression="Title_Name" />
        <asp:BoundField DataField="Full_Name" HeaderText="Name" SortExpression="Full_Name" />
        <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
        <asp:BoundField DataField="Phone" HeaderText="Phone" SortExpression="Phone" />
        <asp:TemplateField HeaderText="Remove From Team" Visible="false">
            <EditItemTemplate>
            </EditItemTemplate>
            <ItemTemplate>
            <pinso:CustomButton runat="server" ID="btnDelete" Text="Delete" 
                Visible='<%# ((Eval("Title_ID") != "1") && (Eval("Title_ID") != "4") && (Eval("Title_ID") != "8")) %>' 
                CommandName="delete" OnClientClick="return confirm('Are you sure you want to delete this record?');" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns> 
</asp:GridView>

    

    <div class="tileSubHeader upperSpace">Ad Hoc / Extended Team</div>
   
<asp:FormView ID="frmAdHoc" runat="server" DataSourceID="dsCampaignTeam" 
        Width="100%" Visible="true">
    <ItemTemplate>  
  
       <table class="CampaignTeam" cellpadding="0" cellspacing="0">
           <tr>
                <td class="label"><b>Function Area:</b></td><td>
        <telerik:RadComboBox  EnableEmbeddedSkins="false"  SkinID="impactGen" runat="server" ID="drpAdHocFunctionArea" DataSourceID="dsFunctionArea" 
        DataTextField="Title_Name" DataValueField="Title_ID" AppendDataBoundItems="true" Width="180" >
            <Items>
                <telerik:RadComboBoxItem Value="0" Text="-- Select a Function Area --" />
            </Items>
        </telerik:RadComboBox></td>
                <td><b>Name:</b></td><td>
        <asp:TextBox ID="txtName" runat="server"></asp:TextBox>         
        </td>
                <td><b>Email:</b></td><td>                 
                <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>            
        </td>
                <td><b>Phone:</b></td>
                <td>                  
                    <asp:TextBox ID="txtPhone" runat="server" MaxLength="12"></asp:TextBox></td>
                <td>                 
                    <asp:Button  SkinID="addButton"  ID="btnAddAdHoc" runat="server" Text="Add To Team" OnClick="btnAddAdHoc_Click" ValidationGroup="adhoc" /> </td>
           </tr>
           <tr>
                <td>&nbsp;</td>
                <td>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" InitialValue="-- Select a Function Area --" ControlToValidate="drpAdHocFunctionArea" ErrorMessage="Functional Area is required." ValidationGroup="adhoc" />
                </td>
               <td>
                </td>                
                <td>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtName" ErrorMessage="Name is required." ValidationGroup="adhoc" Display="Dynamic" />
                </td>
                <td>
                </td>
                <td>
                    <asp:RegularExpressionValidator runat="server" ControlToValidate="txtEmail" ID="reg1" ErrorMessage="Email is invalid." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="adhoc" Display="Dynamic" />
                    <asp:RequiredFieldValidator runat="server" ID="req1" ControlToValidate="txtEmail" ErrorMessage="Email is required." ValidationGroup="adhoc" Display="Dynamic" />
                </td>
                <td>
                </td>
                <td>
                    <asp:RegularExpressionValidator runat="server" ControlToValidate="txtPhone" ID="RegularExpressionValidator1" ValidationGroup="adhoc" ErrorMessage="Phone is invalid." ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}" Display="Dynamic" />
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtPhone" ErrorMessage="Phone is required." ValidationGroup="adhoc" Display="Dynamic" />
                </td>                
           </tr>
       </table>        
      
        
    </ItemTemplate>
</asp:FormView>

<div class="FunctionAreas"></div>
<asp:GridView ID="grvAdHocCampaign" CssClass="FunctionAreas" DataSourceID="dsAdHocCampaignTeam" runat="server" 
    Width="100%" AutoGenerateColumns="False"  
        DataKeyNames="Adhoc_ID" AllowSorting="True" 
        RowStyle-Height="25" HorizontalAlign="Center" EmptyDataText="No Ad Hoc Team members have been added">
   <Columns>
        <asp:BoundField DataField="Adhoc_ID" Visible="false" />
        <asp:BoundField DataField="Title_ID" Visible="false" />
        <asp:BoundField DataField="Title_Name" HeaderText="Function Area" SortExpression="Title_Name" />
        <asp:BoundField DataField="User_Name" HeaderText="Name" SortExpression="User_Name" />
        <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
        <asp:BoundField DataField="Phone" HeaderText="Phone" SortExpression="Phone" />
        <asp:TemplateField HeaderText="Remove From Team" Visible="false" >
            <ItemTemplate>
                <SPAN class=coreBtn><SPAN class=bg><SPAN class=bg2><asp:Button runat="server" ID="btnDeleteAdHoc" Text="Delete"  CommandName="delete" 
                OnClientClick="return confirm('Are you sure you want to delete this record?');" /></SPAN></SPAN></SPAN>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>    

</asp:GridView>

<asp:SqlDataSource ID="dsCampaignTeam" runat="server" ConnectionString="<%$ ConnectionStrings:PathfinderClientDB_Format %>"
    SelectCommand="pprx.usp_GetCampaignTeam_By_Campaign_Id" 
    DeleteCommand="pprx.usp_DeleteCampaignTeam_By_Territory_Id"
    SelectCommandType="StoredProcedure" DeleteCommandType="StoredProcedure">
    <SelectParameters>
        <asp:QueryStringParameter Name="Campaign_ID" QueryStringField="id" />
    </SelectParameters> 
    <DeleteParameters>
        <asp:Parameter Name="Territory_ID" Type="String"  />
    </DeleteParameters>      
</asp:SqlDataSource> 

<asp:SqlDataSource ID="dsAdHocCampaignTeam" runat="server" ConnectionString="<%$ ConnectionStrings:PathfinderClientDB_Format %>"
    SelectCommand="pprx.usp_GetAdHocCampaignTeam_By_Campaign_Id" 
    DeleteCommand="pprx.usp_DeleteAdHocCampaignTeam_By_Id"
    SelectCommandType="StoredProcedure" DeleteCommandType="StoredProcedure">
    <SelectParameters>
        <asp:QueryStringParameter Name="Campaign_ID" QueryStringField="id" />
    </SelectParameters> 
    <DeleteParameters>
        <asp:Parameter Name="Adhoc_ID" Type="Int32" />
    </DeleteParameters>
</asp:SqlDataSource> 

<asp:SqlDataSource ID="dsFunctionArea" runat="server" 
    ConnectionString="<%$ ConnectionStrings:PathfinderClientDB_Format %>" 
    SelectCommand="pprx.usp_GetFunctionArea" SelectCommandType="StoredProcedure">
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsEmpNames" runat="server"
    ConnectionString="<%$ ConnectionStrings:PathfinderClientDB_Format %>" 
     SelectCommandType="StoredProcedure"   >
   <%-- <SelectParameters>
        <asp:ControlParameter ControlID="ctl00$main$frmCampaignTeam$drpFunctionArea" Name="Title_ID" PropertyName="SelectedValue" DefaultValue="0" DbType="Int32" ConvertEmptyStringToNull="true" />
        <asp:QueryStringParameter Name="Campaign_ID" QueryStringField="id" />
    </SelectParameters>--%>
</asp:SqlDataSource>

    <div id="dial" style="display:none;">
        <iframe frameborder=0 style="width:100%;height:99%; overflow:scroll;"></iframe>
    </div>

 <script type="text/javascript">
     $(document).ready(function()
     {
         $("#dial").dialog({ autoOpen: false, resizable: false, draggable: false, modal: true, width: 800, height: 400, title: "" });
     });

     function showEmailModal()
     {
         $("#dial").dialog('option', 'title', ' E-Invitation for Team Meeting').dialog('open').find("iframe").attr("src", "email_campaign_team.aspx" + document.location.search);

         return false;
     }
    </script>
</asp:Content>

