<%@ Page Title="Campaign Team" Language="C#" MasterPageFile="~/powerplanrx/MasterPages/Popup.master" Theme="impact" AutoEventWireup="true" CodeFile="email_campaign_team.aspx.cs" Inherits="email_campaign_team" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style>
    body, html {
    	background: none !important;
    		margin:0; 
	padding: 0;

}
.tileSubHeader 
{
    padding:5px;
}
.formPlanInfo td
{
    padding:5px;
}
.left 
{
    text-align:right;
}
.coreBtn {
	background-position:0 0;
	padding-left:7px;
}

.coreBtn, .coreBtn * {
	cursor:pointer;
	display:inline-block;
	height:18px;
}
.coreBtn .bg {
	background-position: top right;
	background-repeat:no-repeat;
	padding-right:8px;
}
.coreBtn .bg .bg2 span{
	height:17px;

}
.coreBtn .bg2 {
	background-position:0 -52px;
	background-repeat:repeat-x;
}
.coreBtn input
{
	border:none;
	background:none;
}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="partialPage" Runat="Server">


<asp:FormView ID="frmCampaignMeetingEmail" runat="server" DataSourceID="dsCampaignMeetingEmail" Width="100%">
    <ItemTemplate>    
      
       <div class="tileSubHeader">Campaign Name: 
       <asp:label ID="lblCampaignName" runat="server"  Text ='<%# Eval("Campaign_Name")%>'></asp:label> /
       <span>Brand:</span>
       <asp:label ID="lblBrandName" runat="server"  Text ='<%# Eval("Brand_Name")%>'></asp:label></div>
     </ItemTemplate>        
</asp:FormView>

<asp:FormView ID="frmGetEmailAddresses" runat="server" DataSourceID="dsGetEmailAddresses" Width="100%">
    <ItemTemplate> 
        <table class="formPlanInfo" cellpadding="0" cellspacing="0">
            <tr>
                <td style="width:25%" class="left">From:
                </td>
                <td class="alignLeft" style="width:75%"><%= Session["FirstName"] %>
                </td>
            </tr>
            <tr>
                <td class="left">To:  
                </td>
                <td><div class="tip">When entering multiple email addresses below please separate them with a comma</div>
                <asp:TextBox Width="550px" ID="txtEmail" runat="server" Text='<%# Eval("EmailAddresses") %>'  ></asp:TextBox>       
       <asp:RegularExpressionValidator id="RegularExpressionValidator1" 
          runat="server" ControlToValidate="txtEmail" 
          ErrorMessage="You must enter valid email addresses separated by commas and with no spaces" 
          ValidationExpression="^(([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}){1,25})+([,.](([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}){1,25})+)*$">
        </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td class="left">CC:
                </td>
                <td><asp:TextBox Width="550px" ID="txtCC" runat="server" Text='<%# Eval("CCEmailAddresses") %>' ></asp:TextBox> 
       <asp:RegularExpressionValidator id="RegularExpressionValidator2" 
          runat="server" ControlToValidate="txtCC" 
          ErrorMessage="You must enter valid email addresses separated by commas and with no spaces" 
          ValidationExpression="^(([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}){1,25})+([,.](([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}){1,25})+)*$">
        </asp:RegularExpressionValidator>
                </td>
            </tr>
        </table>
        
      </ItemTemplate>        
</asp:FormView>

<asp:FormView ID="frmCampaignMeeting" runat="server" DataSourceID="dsCampaignMeetingEmail" Width="100%">
    <ItemTemplate> 
    <table class="formPlanInfo" cellpadding="0" cellspacing="0">
           <tr>
                <td class="left" style="width:25%">Subject:
                </td>
                <td class="alignLeft" style="width:75%"> Pull-Through Campaign Planning for <asp:Label ID="lblSubject" runat="server" Text='<%# Eval("Campaign_Name") %>' />
                </td>
            </tr>
            <tr>
                <td class="left">Meeting Date:
                </td>
                <td class="alignLeft"><asp:TextBox ID="txtMeetingDate" runat="server"   MaxLength="10"></asp:TextBox>                  
        <ajax:CalendarExtender runat="server" ID="calMeetingDate" TargetControlID="txtMeetingDate" />
        <asp:CompareValidator ID="cvalDate" runat="server"  ErrorMessage="Invalid Date!" ControlToValidate="txtMeetingDate" 
        Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td class="left">Meeting Time:
                </td>
                <td class="alignLeft"><asp:TextBox ID="txtMeetingTime" runat="server"   ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="left">Meeting Location:
                </td>
                <td class="alignLeft"><asp:TextBox ID="txtMeetingLocation" runat="server"   ></asp:TextBox>
                </td class="alignLeft">
            </tr>
            <tr>
                <td class="left">Dial In Number:</td>
                <td class="alignLeft"><asp:TextBox ID="txtDialInNumber" runat="server"   ></asp:TextBox></td>
            </tr>
            <tr>
                <td class="left">Participant Code:</td>
                <td class="alignLeft"> <asp:TextBox ID="txtParticipantCode" runat="server"   ></asp:TextBox></td>
            </tr>
    </table>
    <div style="padding:5px; text-align:right" >
    <span class="coreBtn"><span class="bg"><span class="bg2">
       <asp:Button ID="btnEmailTeam" runat="server" Text="Send Email" OnClick="EmailTeam" />
       </span></span></span></div>
       
       
       
       <asp:BulletedList ID="meetingAgendaList" runat="server" BulletStyle="Numbered" ItemStyle-CssClass="hiddencolumn" Visible="False">
       <asp:ListItem>Review the market opportunity and rationale for this campaign</asp:ListItem>
       <asp:ListItem>Review key information about the Health Plan</asp:ListItem>
       <asp:ListItem>Review the Campaign Timeline to understand key dates and milestones</asp:ListItem>
       <asp:ListItem>Define the share and volume goals to be achieved</asp:ListItem>
       <asp:ListItem>Define the tactics and messages that will be used to execute the campaign</asp:ListItem>
       </asp:BulletedList>
                  
   </ItemTemplate>        
</asp:FormView>
<asp:Panel runat="server" ID="pnlMessage" Visible="false">
    Message has been sent to the team.
</asp:Panel>

<asp:SqlDataSource ID="dsCampaignMeetingEmail" runat="server" ConnectionString="<%$ ConnectionStrings:PathfinderClientDB_Format %>"
    SelectCommand="pprx.usp_GetCampaignEmailInfo_By_Campaign_Id" 
    SelectCommandType="StoredProcedure" >
    <SelectParameters>
        <asp:QueryStringParameter Name="Campaign_ID" QueryStringField="id" />
    </SelectParameters> 
</asp:SqlDataSource> 

<asp:SqlDataSource ID="dsGetEmailAddresses" runat="server" ConnectionString="<%$ ConnectionStrings:PathfinderClientDB_Format %>" 
    SelectCommand="pprx.usp_GetCampaignEmailAddresses_By_Campaign_Id" 
    SelectCommandType="StoredProcedure" >
    <SelectParameters>
        <asp:QueryStringParameter Name="Campaign_ID" QueryStringField="id" />
    </SelectParameters>
</asp:SqlDataSource>

</asp:Content>

