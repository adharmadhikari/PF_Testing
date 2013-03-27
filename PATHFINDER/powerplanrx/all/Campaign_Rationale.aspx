<%@ Page Title="Campaign Rationale" Language="C#" MasterPageFile="~/powerplanrx/MasterPages/Popup.master" Theme="impact" AutoEventWireup="true" CodeFile="Campaign_Rationale.aspx.cs" Inherits="Campaign_Rationale" %>
<%@ OutputCache NoStore="true" Duration="1" VaryByParam="None" %>
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
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">

<asp:FormView runat="server" ID="frmPlanBrandInfo" DataSourceID="dsPlanBrandInfo" Width="100%">
            <ItemTemplate><div class="tileSubHeader">            
                <asp:Label runat="server" ID="Label1" Text="Plan Name:" />
                <asp:Label runat="server" ID="lblDistrict" Text='<%# Eval("Plan_Name") %>' />
              /
                <asp:Label runat="server" ID="Label2" Text="Brand:" />
                <asp:Label runat="server" ID="lblBrandName" Text='<%# Eval("Brand_Name") %>' />  
                </div>             
            </ItemTemplate>            
</asp:FormView>
<asp:Panel ID="pnlRationale" runat="server" CssClass="rationale">
    <table><tr>
        <td colspan="2" align="center">
            <asp:Label ID="lblTitle" runat="server" Text="Campaign Rationale"></asp:Label>
            <span class="requiredRed">*</span>
        </td>
    </tr></table> 
    <asp:DataList ID="dlrationale" runat="server" DataSourceID="dsRationale">
        <ItemTemplate>
           <asp:CheckBox ID="chkRationaleID" Visible="false" runat="server" Text='<%#Eval("Rationale_ID")%>' Checked='<%# (System.Convert.ToBoolean(Eval("ActiveRationale")))%>' />
           <asp:CheckBox ID="chkRationale" runat="server" Text='<%#Eval("Rationale_Name")%>' Checked='<%# (System.Convert.ToBoolean(Eval("ActiveRationale")))%>' />
        </ItemTemplate>
    </asp:DataList>
    
     <div class="impactBtns" id="div1" runat="server">
            <span class="coreBtn"><span class="bg"><span class="bg2">
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="OnSubmit" />
            </span></span></span><span class="coreBtn"><span class="bg"><span class="bg2">
                <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="OnReset" />
            </span></span></span>
     </div>
    <asp:Label ID="lblErrorMessage" runat="server" Text= "The request could not be processed this time, please try later." Visible="false"></asp:Label>
    <asp:Label ID="lblReqdRationals" runat="server" Text= "Please select a rationale for the Campaign." CssClass="requiredRed"  Visible="false"></asp:Label>
    
    <asp:SqlDataSource ID="dsRationale" runat="server" ConnectionString="<%$ ConnectionStrings:PathfinderClientDB_Format %>"
        SelectCommand="pprx.usp_GetRationaleByBrandID" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter Name="Brand_ID" QueryStringField="Brand_ID" />
            <asp:QueryStringParameter Name="Campaign_ID" QueryStringField="id" DefaultValue="0" />
        </SelectParameters>                    
    </asp:SqlDataSource> 
  </asp:Panel>
 <asp:Panel ID="pnlAlert" runat="server">
    <table><tr>
        <td colspan="2" align="center">
            <asp:Label ID="lblMessage" runat="server" Text="The campaign for this plan and brand already exists. Please contact the home office for creating this campaign."></asp:Label>
        </td>
        
    </tr></table> 
    <div class="impactBtns" id="divButtons" runat="server">
            <span class="coreBtn"><span class="bg"><span class="bg2">
                <asp:Button ID="btnCreateCampaign" runat="server" Text="Notify Admin" OnClick="NotifyAdmin" />
            </span></span></span><span class="coreBtn"><span class="bg"><span class="bg2">
                <asp:Button ID="btnUpdateCampaign" runat="server" Text="Update Campaign" OnClick="UpdateCampaign" />
            </span></span></span>
    </div>
    
  </asp:Panel>
   <asp:SqlDataSource ID="dsPlanBrandInfo" runat="server" ConnectionString="<%$ ConnectionStrings:PathfinderClientDB_Format %>"
        SelectCommand="pprx.usp_GetPlanBrandInfo" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter Name="Plan_ID" QueryStringField="Plan_ID" />
            <asp:QueryStringParameter Name="Brand_ID" QueryStringField="Brand_ID" />           
        </SelectParameters>                    
    </asp:SqlDataSource> 
</asp:Content>


