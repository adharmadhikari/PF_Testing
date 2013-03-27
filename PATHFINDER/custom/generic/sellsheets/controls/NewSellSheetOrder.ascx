<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NewSellSheetOrder.ascx.cs" Inherits="custom_controls_NewSellSheetOrder" %>
   <div id="NewOrder" class="newOrder" style="display:none;">
        <div id="HideSelectedSheetID" style="display: none;" >
            <%-- This div is used to store the Selected Sell_Sheet_ID --%>
            <input id="SelectedSheetID" type="text" value="" /> 
            <input id="SelectedOrderID" type="text" value="" /> 
        </div>
    <div class="tileContainerHeader">
        <div ID="BDHeader1" class="title" runat="server">New Order</div>
        <div class="tools">
           <a href="javascript:void(0)" onclick="clientManager.get_ApplicationManager().newSellSheetOrder()" runat="server" id="SaveNewOrder">Save</a>
           <span class="pipe">|</span><a id="ClearNewOrder" href="javascript:void(0)" runat="server" onclick="$resetContainer('NewOrder'); return false;">Clear</a>
        </div> 
        <div class="clearAll"></div>
    </div>
   
    <table class="newSSOrder">
     <tr>
     <td width="50%" valign="top">
        <table>
        <tr>
        <td><span class="ssBold">Quantity</span><span class="requiredRed">*</span></td>
        <td>
           <asp:TextBox ID="NoSellSheets" runat="server" Text="" MaxLength="3" Width="35px"></asp:TextBox>  
        </td>
        </tr>
        <tr>
        <td><span class="ssBold">Printer Email</span><span class="requiredRed">*</span></td>
        <td><asp:TextBox ID="PrinterEmail" runat="server" Text="" MaxLength="50" Width="150px" ></asp:TextBox></td>
        </tr>
        <tr>
        <td><span class="ssBold">Printer Fax</span><span class="requiredRed">*</span></td>
        <td><asp:TextBox ID="PrinterFax" runat="server" Text="" MaxLength="20" Width="150px"></asp:TextBox></td>
        </tr>
        <tr>
        <td valign="top"><span class="ssBold">Print Instructions</span></td>
        <td><asp:TextBox runat="server" ID="PrintInsttxt" TextMode="MultiLine" Width="150px" rows="5"></asp:TextBox></td>
        </tr>
        </table>
     </td>
        
     <td width="50%">
        <table>
            <tr style="width: 175px">
            <td ><span class="ssBold">Ship Location</span><span class="requiredRed">*</span></td>
            <td style="padding-left:10px"> 
                <telerik:RadComboBox ID="rdcmbShipLocation" runat="server" Width="65%" DropDownWidth="150px" EnableEmbeddedSkins="false" SkinID="planInfoCombo" Skin="pathfinder" AppendDataBoundItems="true" DataSourceID="dsLkpShipLocations" DataTextField="Location_Name" DataValueField="Location_ID"  >
                </telerik:RadComboBox>
            </td>
            </tr>
            <tr>
            <td ><span class="ssBold">Address 1</span><span class="requiredRed">*</span></td>
            <td colspan="5"><asp:TextBox ID="Addr1" runat="server" Text="" MaxLength="50" Width="150px" ></asp:TextBox></td>
            </tr>
            <tr>
            <td ><span class="ssBold">Address 2</span></td>
            <td  colspan="5"><asp:TextBox ID="Addr2" runat="server" Text="" MaxLength="50" Width="150px" ></asp:TextBox></td>
            </tr>
            <tr>
            <td ><span class="ssBold">City</span><span class="requiredRed">*</span></td>
            <td><asp:TextBox ID="City" runat="server" Text="" MaxLength="50" Width="150px" ></asp:TextBox></td>
            </tr>
            <tr>
            <td ><span class="ssBold">State</span><span class="requiredRed">*</span></td>
            <td style="padding-left:10px">
                <telerik:RadComboBox ID="rdcmbState" runat="server" Width="155px" AppendDataBoundItems="true" DropDownWidth="155px" EnableEmbeddedSkins="false" SkinID="planInfoCombo" Skin="pathfinder" DataSourceID="dsLkpStates" DataTextField="Name" DataValueField="ID"  >
                    <Items>
                        <telerik:RadComboBoxItem Value="" Text="[Select a State]" Selected="true" />  
                    </Items>
                </telerik:RadComboBox>
            </td>
            </tr>
            <tr>
            <td ><span class="ssBold">Zip</span><span class="requiredRed">*</span></td>
            <td><asp:TextBox ID="Zip" runat="server" Text="" MaxLength="50" Width="30px" ></asp:TextBox> <span class="ssBold">Phone</span><asp:TextBox ID="Phone" runat="server" Text="" MaxLength="50" Width="65px"></asp:TextBox></td>

            </tr>
            <tr>
            <td><span class="ssBold">Email</span><span class="requiredRed">*</span><div style="height: 0px; width: 65px; "></div></td>
            <td ><asp:TextBox ID="Email" runat="server" Text="" MaxLength="50" Width="150px"></asp:TextBox></td>

            </tr>
        </table>
     </td>
     </tr>
    </table>

    <pinso:ClientValidator ID="ClientValidator1" Target="NoSellSheets" Text ="Please enter valid No. of Sell Sheets." Required="true" runat="server" RegExp="^\d{1,3}$"  />  
    <pinso:ClientValidator ID="ClientValidator2" Target="rdcmbShipLocation" Text ='Please select the ship location' Required="true" runat="server" />  
    <pinso:ClientValidator ID="ClientValidator3" Target="Addr1" Text ='Please enter address1' Required="true" runat="server"  RegExp="[0-9a-zA-Z]" />  
    <%--<pinso:ClientValidator ID="ClientValidator5" Target="Addr2" Text ='Please enter address2' Required="true" runat="server" RegExp="[0-9a-zA-Z]"/>--%>  
    <pinso:ClientValidator ID="ClientValidator6" Target="City" Text ='Please enter the city.' Required="true" runat="server" RegExp="[a-zA-Z]"/>  
    <pinso:ClientValidator ID="ClientValidator7" Target="rdcmbState" Text ='Please select the State' Required="true" runat="server" />  
    <pinso:ClientValidator ID="ClientValidator8" Target="Zip" Text ='Please enter the zipcode.' Required="true" runat="server"  RegExp="[0-9]"/>  
    <pinso:ClientValidator ID="ClientValidator9" Target="Email" Text ="Please enter email address." Required="true" runat="server"  RegExp="[\w!#$%*/?|\^\{\}'~\.]+@(\w+\.)*\w{2,3}" />  
    <pinso:ClientValidator ID="ClientValidator4" Target="PrinterEmail" Text ='Please enter valid printer email.' Required="true" runat="server" RegExp="[\w!#$%*/?|\^\{\}'~\.]+@(\w+\.)*\w{2,3}"/>  
    <pinso:ClientValidator ID="ClientValidator10" Target="PrinterFax" Text ='Please enter valid printer fax.' Required="true" runat="server" RegExp="[0-9]"/>  
 </div>
   
<asp:EntityDataSource ID="dsLkpShipLocations" runat="server" EntitySetName="LkpShipLocationsSet" DefaultContainerName="PathfinderClientEntities"
         AutoGenerateWhereClause="true">
</asp:EntityDataSource>

<asp:EntityDataSource ID="dsLkpStates" runat="server" EntitySetName="StateSet" DefaultContainerName="PathfinderClientEntities" 
         AutoGenerateWhereClause="true">
</asp:EntityDataSource>
