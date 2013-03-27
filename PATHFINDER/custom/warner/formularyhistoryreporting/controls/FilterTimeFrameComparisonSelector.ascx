<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterTimeFrameComparisonSelector.ascx.cs" Inherits="custom_warner_formularyhistoryreporting_controls_FilterTimeFrameComparisonSelector" %>
<asp:Panel runat="server" id="timeFrameCalendar">
    <table width="100%">
        <tr>
            <td width="25%">
                Year
            </td>
            <td width="25%">
                <telerik:RadComboBox ID="Year" runat="server" AppendDataBoundItems="true" Width="100%"
                    DropDownWidth="100px" Skin="pathfinder" EnableEmbeddedSkins="false">
                    <Items>
                    </Items>   
                </telerik:RadComboBox>
            </td>
            <td width="25%">
                <telerik:RadComboBox ID="Month_Quarter" runat="server" AppendDataBoundItems="true" Width="100%"
                    DropDownWidth="100px" Skin="pathfinder" EnableEmbeddedSkins="false">
                    <Items>
                        <telerik:RadComboBoxItem Text="Quarter" Value="1" Selected="true" />
                        <telerik:RadComboBoxItem Text="Month" Value="2" />
                    </Items>   
                </telerik:RadComboBox>
            </td>
            <td width="25%">
                <telerik:RadComboBox ID="Month_Quarter_Selection" runat="server" AppendDataBoundItems="true" Width="100%"
                    DropDownWidth="100px" Skin="pathfinder" EnableEmbeddedSkins="false">
                    <Items>
                    </Items>   
                </telerik:RadComboBox>
            </td>
        </tr>
    </table>
</asp:Panel>
