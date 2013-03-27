<%@ Page Title="PowerPlanRx - Step 3 Approval" Language="C#" MasterPageFile="~/powerplanrx/MasterPages/MasterPage.master" AutoEventWireup="true" Theme="impact" CodeFile="createcampaign_step3_approval.aspx.cs" Inherits="createcampaign_step2_approval" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" Runat="Server">
    <div class="tileContainerHeader">
<div class="CampaignInfo">Team Approval Status</div>
</div> 
 <telerik:RadGrid ID="RadGrid1" runat="server" EnableEmbeddedSkins="false" SkinID="table1"  AllowSorting="True" DataSourceID="SqlDataSource1" OnItemDataBound="RadGrid1_ItemDataBound" >
<MasterTableView autogeneratecolumns="False" datasourceid="SqlDataSource1">
<RowIndicatorColumn>
<HeaderStyle Width="20px"></HeaderStyle>
</RowIndicatorColumn>
<ExpandCollapseColumn>
<HeaderStyle Width="20px"></HeaderStyle>
</ExpandCollapseColumn>
    <Columns>
        <telerik:GridBoundColumn DataField="Campaign_ID" DataType="System.Int32"  Visible="false"
            HeaderText="Campaign_ID" SortExpression="Campaign_ID" UniqueName="Campaign_ID">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Territory_ID" Visible="false"
            HeaderText="Territory_ID" SortExpression="Territory_ID" UniqueName="Territory_ID">
        </telerik:GridBoundColumn> 
        
                <telerik:GridBoundColumn DataField="Approval_Status_Indicator" Visible="false"
            HeaderText="Approval_Status_Indicator" SortExpression="Territory_ID" UniqueName="Approval_Status_Indicator">
        </telerik:GridBoundColumn>
               
        <telerik:GridBoundColumn DataField="UName" 
            HeaderText="Name" SortExpression="UName" 
            UniqueName="UName">
        </telerik:GridBoundColumn>        
        <telerik:GridBoundColumn DataField="Title_Name" HeaderText="Function Area" 
            SortExpression="Title_Name" UniqueName="Title_Name">
        </telerik:GridBoundColumn>
        
        <telerik:GridBoundColumn DataField="Status" HeaderText="Approval Status" 
            SortExpression="Status" UniqueName="Status">
        </telerik:GridBoundColumn>        
        <telerik:GridBoundColumn DataField="Approved_DT" DataFormatString = "{0:d}"
            HeaderText="Date" SortExpression="Approved_DT" UniqueName="Approved_DT">
        </telerik:GridBoundColumn>
        <telerik:GridTemplateColumn HeaderText="Decision Needed">            
            <ItemTemplate> 
            <pinso:CustomButton runat="server" ID="Approval_Button" OnClick="On_Approval_Button_Click" Text="Approve" Visible="false" /> 
            <pinso:CustomButton ID="Denial_Button" OnClick="On_Denial_Button_Click" runat="server" Text="Deny" Visible="false" />  
                
            </ItemTemplate>             
        </telerik:GridTemplateColumn> 
        
    </Columns>
</MasterTableView>

<FilterMenu EnableTheming="True">
<CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
</FilterMenu>
        <ClientSettings Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="false" Selecting-AllowRowSelect="false" />

        </telerik:RadGrid>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:PathfinderClientDB_Format %>"  
            SelectCommand="pprx.usp_Get_Team_Approval_Status_By_CampaignID" 
            SelectCommandType="StoredProcedure">            
            <SelectParameters>
                <asp:QueryStringParameter Name="Campaign_ID" QueryStringField="id" />
            </SelectParameters>
        </asp:SqlDataSource>
    
</asp:Content>