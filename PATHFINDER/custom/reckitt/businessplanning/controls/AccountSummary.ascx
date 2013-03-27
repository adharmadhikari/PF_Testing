<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountSummary.ascx.cs" Inherits="custom_reckitt_businessplanning_controls_AccountSummary" %>
<style type="text/css">


</style>
<asp:FormView  runat="server" ID="frmBPAccountSummary" DefaultMode="ReadOnly"
           DataSourceID="dsBusinessPlans" CellPadding="0" CellSpacing="0" Width="100%" valign="top">
        <ItemTemplate>
            <table cellpadding="0"  cellspacing="0" class="genBP_AccountSummaryTable">
                <tr>
                    <td colspan="4">
                        <asp:Label ID="Acct_Summary" runat="server" 
                            text='<%# ConvertLineBreaks(Convert.ToString(Eval("Acct_Summary")))%>' Wrap="true"></asp:Label>
                        </td>
                </tr>
                <tr>
                    <td >
                        Does the Organization offer OTC coverage?</td>
                    <td>                       
                        <pinso:RadiobuttonValueList ID="OTC_Coverage_YN" runat="server" BorderStyle="None" RepeatDirection="Horizontal" SelectedVal='<%# Eval("OTC_Coverage_YN") %>'>
                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem> 
                            <asp:ListItem Text="No" Value="2"></asp:ListItem> 
                            <asp:ListItem Text="Not Available" Value="0" Selected="True"></asp:ListItem> 
                        </pinso:RadiobuttonValueList>
                    </td>
                    <td>
                        Is P &amp; T review required?</td>
                    <td >                        
                         <pinso:RadiobuttonValueList ID="PT_Review_YN" runat="server" BorderStyle="None" RepeatDirection="Horizontal" SelectedVal='<%# Eval("PT_Review_YN") %>'>
                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem> 
                            <asp:ListItem Text="No" Value="2"></asp:ListItem> 
                            <asp:ListItem Text="Not Available" Value="0" Selected="True"></asp:ListItem> 
                        </pinso:RadiobuttonValueList>
                    </td>
                </tr>
                <tr>
                    <td >
                        Do Opportunities exist to participate in OTC patient and/or provider educational 
                        programs? </td>
                    <td>          
                        <pinso:RadiobuttonValueList ID="OTC_Opportunities_YN" runat="server" BorderStyle="None" RepeatDirection="Horizontal" SelectedVal='<%# Eval("OTC_Opportunities_YN") %>'>
                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem> 
                            <asp:ListItem Text="No" Value="2"></asp:ListItem> 
                            <asp:ListItem Text="Not Available" Value="0" Selected="True"></asp:ListItem> 
                        </pinso:RadiobuttonValueList>
                    </td>
                    <td>
                        If no, explain.</td>
                    <td>
                        <asp:Label ID="NO_PT_Explanation" runat="server" MaxLength="50" 
                            TextMode="MultiLine" Text='<%# Eval("NO_PT_Explanation") %>' Wrap="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td rowspan="3">
                        Opportunities</td>
                    <td rowspan="2">
                        <asp:CheckBox id="O_Cough_Cold_Kits_YN" runat="server" Text="Cough/Cold Kits" Checked='<%# Eval("O_Cough_Cold_Kits_YN") %>' /><br />
                        <asp:CheckBox id="O_Health_Fairs_YN" runat="server" Text="Health Fairs" Checked='<%# Eval("O_Health_Fairs_YN") %>' /><br />
                        <asp:CheckBox id="O_Education_Brochures_YN" runat="server" Text="Educational Brochures" Checked='<%# Eval("O_Education_Brochures_YN") %>' /><br />
                        <asp:CheckBox id="O_Other_YN" runat="server" Text="Other" Checked='<%# Eval("O_Other_YN") %>' /><br />
                    </td>
                    <td rowspan="2">
                        How quickly can a new product be scheduled?</td>
                    <td >                        
                         <pinso:RadiobuttonValueList ID="Product_Review_Period" runat="server" BorderStyle="None" RepeatDirection="Horizontal" SelectedVal='<%# Eval("Product_Review_Period") %>'>
                            <asp:ListItem Value="1" Text="0-1 months"></asp:ListItem>
                            <asp:ListItem Value="2" Text="2-3 months"></asp:ListItem>
                            <asp:ListItem Value="3" Text="4-6 months"></asp:ListItem>
                            <asp:ListItem Value="4" Text="7-12 months"></asp:ListItem>
                            <asp:ListItem Value="5" Text="Not Available"></asp:ListItem>
                        </pinso:RadiobuttonValueList>
                    </td>
                </tr>
                <tr>
                    <td >
                        <asp:Label ID="NA_Schedule_Period" runat="server" MaxLength="50" 
                            Text='<%# Eval("NA_Schedule_Period") %>' Wrap="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="O_Other_Explanation" runat="server" MaxLength="50" 
                             Text='<%# Eval("O_Other_Explanation") %>' Wrap="true"></asp:Label>
                    </td>
                    <td>
                        Can the time frame be reduced for review?</td>
                    <td >
                         <pinso:RadiobuttonValueList ID="Timeframe_Reduced_YN" runat="server" BorderStyle="None" RepeatDirection="Horizontal" SelectedVal='<%# Eval("Timeframe_Reduced_YN") %>'>
                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem> 
                            <asp:ListItem Text="No" Value="2"></asp:ListItem> 
                            <asp:ListItem Text="Not Available" Value="0" Selected="True"></asp:ListItem> 
                        </pinso:RadiobuttonValueList>
                    </td>
                </tr>
            </table>
        </ItemTemplate> 
        
</asp:FormView> 

<asp:EntityDataSource ID="dsBusinessPlans" runat="server" 
    ConnectionString="name=PathfinderReckittEntities" 
    DefaultContainerName="PathfinderReckittEntities" 
    EntitySetName="BusinessPlanSet" AutoGenerateWhereClause="true">
        <WhereParameters>                
                   <asp:QueryStringParameter QueryStringField="Plan_ID" Name="Plan_ID" Type="Int32"/> 
        </WhereParameters>  
</asp:EntityDataSource>


