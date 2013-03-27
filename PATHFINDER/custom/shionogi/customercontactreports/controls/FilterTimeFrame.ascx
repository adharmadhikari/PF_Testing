<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterTimeFrame.ascx.cs" Inherits="custom_controls_FilterTimeFrame" %>
<%--<telerik:RadCalendar ID="hs" runat="server"></telerik:RadCalendar>--%>
<%--<telerik:RadDatePicker ID="newwew" runat="server"></telerik:RadDatePicker>--%>
<div class="filterGeo">
    <asp:Literal runat="server" ID="Timeselect" Text="Choose Time Frame " />  
</div>
<telerik:RadComboBox ID="rdlTF" runat="server" CssClass="notfilter" Skin="pathfinder" EnableEmbeddedSkins="false" MaxHeight="200px"  >
 <Items>
            <telerik:RadComboBoxItem runat="server" Value="1" Text="Current Month" />
            <telerik:RadComboBoxItem runat="server" Value="2" Text="Year To Date" />
            <telerik:RadComboBoxItem runat="server" Value="3" Text="Time Frame" />
 </Items>  
</telerik:RadComboBox>
<%--<div id ="YTDate" runat="server" style="display: none">
   <div class="filterGeo">
       <asp:Literal runat="server" ID="Literal1" Text="Date" />  
   </div>
   <asp:TextBox runat="server" id="YTD" SkinID="datePicker" Width="80px" /> 
   <asp:HiddenField runat="server" ID="hdnYTD" Value="" />
</div>--%>
<div id ="timeFrame" style="display: none">
<div class="filterGeo">
    <input id="txtFrom" name="Contact_Date" class="datePicker" style="width:70px" /> - <input id="txtTo" name="Contact_Date" class="datePicker" style="width:70px" />
</div>
   <%--<div>
    
  </div>--%>
  <pinso:ClientValidator runat="server" id="ClientValidator1" target="txtFrom" DataType="Date" Text='Please enter a valid start date.' />
  <pinso:ClientValidator runat="server" id="ClientValidator2" target="txtTo" DataType="Date" Text='Please enter a valid end date.' />
  <%--<div>
    <pinso:ClientValidator runat="server" id="ClientValidator2" target="txtTo" DataType="Date" Required="true" Text='Please enter a valid end date.' />
  </div>--%>
</div>
