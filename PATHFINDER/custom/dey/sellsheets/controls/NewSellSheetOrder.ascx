<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NewSellSheetOrder.ascx.cs" Inherits="custom_controls_NewSellSheetOrder" %>
   <div id="NewOrder" class="newOrder">
    <div id="HideSelectedSheetID" style="display: none;" >
            <%-- This div is used to store the Selected Sell_Sheet_ID --%>
            <input id="SelectedSheetID" type="text" value="" /> 
            <input id="SelectedOrderID" type="text" value="" /> 
        </div>
    <div class="tileContainerHeader">
        <div ID="BDHeader1" class="title" runat="server">New Order</div>
        <div class="tools">
           <a href="javascript:void(0)" onclick="clientManager.get_ApplicationManager().newSellSheetOrder()" runat="server" id="SaveNewOrder">Save</a>
           <span class="pipe">|</span><a id="ClearNewOrder" href="javascript:void(0)" runat="server" onclick="reset_sellsheetordercontainer()">Clear</a>
        </div> 
        <div class="clearAll"></div>
    </div>
    <table class="newSSOrder"  width="100%" cellpadding="5">
        <tr>
        <td> 
            <table cellpadding="5" cellspacing="5">
                <tr>
                    <td><span class="ssBold">Region:</span></td>
                    <td>
                         <telerik:RadComboBox ID="rdcmbRegion" runat="server"  AppendDataBoundItems="true"
                            EnableEmbeddedSkins="false" Skin="pathfinder" MaxHeight="250px"
                            OnClientDropDownClosed="rdcmbRegion_DropDownClosed" >
                            </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
        </td>
        <td>
            <table cellpadding="5" cellspacing="5">
                <tr>
                    <td><span class="ssBold">District:</span></td>
                    <td>
                         <telerik:RadComboBox ID="rdcmbDistrict" runat="server"  AppendDataBoundItems="true"
                            EnableEmbeddedSkins="false" Skin="pathfinder" Height="160px"
                            OnClientDropDownClosed="rdcmbDistrict_DropDownClosed" >
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
        </td>
        </tr>
        <tr>
        <td align="left" colspan="2" >
            <table>
                <tr>
                    <td><span class="ssBold">Sales Rep:</span></td>
                    <td>
                         <telerik:RadComboBox ID="rdcmbRep" runat="server"  AppendDataBoundItems="true"
                         EnableEmbeddedSkins="false" Skin="pathfinder" Height="160px" 
                         OnClientDropDownClosed="rdcmbRep_DropDownClosed">
                            </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
            
           
        </td>
        </tr>
        <tr>
            <td colspan="2" width="100%">
                <telerik:RadGrid SkinID="radTable" runat="server" ID="gridRep" AllowSorting="False"  
                    AllowFilteringByColumn="false" AutoGenerateColumns="False" AllowPaging="True" 
                    EnableEmbeddedSkins="False" GridLines="None" >
                 <MasterTableView AutoGenerateColumns="False" DataKeyNames="ID" >
                    <Columns>
                        <telerik:GridBoundColumn DataField="ID" Display="false" />
                        <telerik:GridBoundColumn DataField="Name"  HeaderText="Sales Rep" DataType="System.String" 
                             HeaderStyle-Width="30%" ItemStyle-Width="30%"/>
                        <telerik:GridBoundColumn DataField="RepFullAddress" HeaderText="Address" DataType="System.String" 
                            HeaderStyle-Width="40%" ItemStyle-Width="40%"/>
                        <telerik:GridTemplateColumn UniqueName="Quantity" HeaderText="Quantity"
                            HeaderStyle-Width="30%" ItemStyle-Width="30%" >
                            <ItemTemplate>
                                 <asp:TextBox ID="txt_quantity"  runat="server" MaxLength="3" Width="35px" />
                                 <pinso:ClientValidator ID="clientValidator1" runat="server"
                                   CompareOperator="DataTypeCheck" DataType="Integer"  Required="true"
                                    Target="txt_quantity"  Text="Please enter a integer value" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings>    
                     <DataBinding Location="~/custom/dey/sellsheets/services/DeyDataService.svc" DataService-TableName="SellSheetTerritoryRepsSet" />
                     <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="150px"/>
                     <Selecting AllowRowSelect="true" /> 
                </ClientSettings>
                </telerik:RadGrid>
                <pinso:RadGridWrapper runat="server"  ID="radGridWrapper" ShowNumberOfRecords="true" 
                    Target="gridRep" CustomPaging="false"  MergeRows="false"    
                    RequiresFilter="true" AutoUpdate="false" AutoLoad="true" />

            </td>
        </tr>
    </table>

 </div>
   
