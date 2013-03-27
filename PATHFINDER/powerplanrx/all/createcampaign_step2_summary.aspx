<%@ Page Language="C#" MasterPageFile="~/powerplanrx/MasterPages/MasterPage.master" AutoEventWireup="true" Title="PowerPlanRx - Step 2 Summary" Theme="impact" CodeFile="createcampaign_step2_summary.aspx.cs" Inherits="createcampaign_step2_summary" %>
<%@ Register Src="~/powerplanrx/controls/Goals.ascx" TagName="Goals" TagPrefix="pinso" %>
<%@ Register Src="~/powerplanrx/controls/Tactics.ascx" TagName="Tactics" TagPrefix="pinso" %>
<%--    <%@ Register Src="~/powerplanrx/controls/Messages.ascx" TagName="Messages" TagPrefix="pinso" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" Runat="Server">
    
    <asp:FormView runat="server" ID="formView" DataSourceID="dsCampaignInfo" Width="100%">
        <ItemTemplate>
        <div class="tileContainerHeader">
            <div class="CampaignInfo">
                <b>Campaign Name:</b> <asp:Label runat="server" ID="lblPlanName" Text='<%# Eval("Campaign_Name") %>' />        
            </div>
        </div>
        <div>
            <table style="width:80%;margin:auto auto auto auto;">
                <tr>
                    <td class="alignRight" style="width:25%;font-weight:bold;padding-right:3px;">Plan Name:</td>
                    <td><asp:Label runat="server" ID="Label1" Text='<%# Eval("Plan_Name") %>' /></td>
                    
                    <td class="alignRight" style="width:25%;font-weight:bold;padding-right:3px;">Total Lives:</td>
                    <td style="width:25%"><asp:Label runat="server" ID="lblTotalLives" Text='<%# Eval("Total_Lives", "{0:n0}") %>' />&nbsp;</td>
                </tr>
                <tr>
                    <td class="alignRight" style="width:25%;font-weight:bold;padding-right:3px;">Formulary Change:</td>
                    <td style="width:25%"><asp:Label runat="server" ID="lblFormularyChange" Text='<%# (bool)Eval("Formulary_Change_Status") ? "Yes" : "No" %>' />&nbsp;</td>

                    <td class="alignRight" style="width:25%;font-weight:bold;padding-right:3px;">Pharmacy Lives:</td>
                    <td style="width:25%"><asp:Label runat="server" ID="lblPharmacyLives" Text='<%# Eval("Pharmacy_Lives", "{0:n0}") %>' />&nbsp;</td>                    
                </tr>                
                <tr>
                    <td class="alignRight" style="width:25%;font-weight:bold;padding-right:3px;">Formulary Change Effective Date:</td>
                    <td style="width:25%"><asp:Label runat="server" ID="lblFormularyChangeDate" Text='<%# Eval("Formulary_Change_Eff_Date", "{0:d}") %>' />&nbsp;</td>
                    
                    <td class="alignRight" style="width:25%;font-weight:bold;padding-right:3px;">Plan Penetration Region:</td>
                    <td style="width:25%"><asp:Label runat="server" ID="lblPlanPenetration" Text='<%# Eval("Plan_Penetrate_Region") %>' />&nbsp;</td>
                </tr>                                
                <tr>
                    <td></td>
                    <td></td>
                    
                    <td  style="width:25%;font-weight:bold;padding-right:3px;"class="alignRight">Contract Share Goal:</td>
                    <td style="width:25%"><asp:Label runat="server" ID="lblContractShareGoal" Text='<%# Eval("Contract_Share_Goal") %>' />&nbsp;</td>
                </tr>                   
            </table>
        </div>
        </ItemTemplate>
    </asp:FormView>
    <asp:SqlDataSource ID="dsCampaignInfo" runat="server" ConnectionString="<%$ ConnectionStrings:PathfinderClientDB_Format %>" SelectCommand="pprx.usp_GetCampaignSummaryInfoByCampaignID" SelectCommandType="StoredProcedure" >
        <SelectParameters>
            <asp:QueryStringParameter Name="Campaign_ID" QueryStringField="id" />
        </SelectParameters>      
    </asp:SqlDataSource>     
    
    
    <pinso:Tactics runat="server" id="tactics" />
  
   <%-- <pinso:Messages runat="server" id="messages" />--%>
    
    <pinso:Goals runat="server" ID="goals" ShowCampaignInformation="false" ShowSummary="true" />
    
</asp:Content>

