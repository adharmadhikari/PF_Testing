<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Popup.master" AutoEventWireup="true" CodeFile="MktPopup.aspx.cs" Inherits="prescriberreporting_all_MktPopup" %>
<%@ Register src="~/prescriberreporting/controls/MktGrid.ascx" tagname="PrescriberGrid" tagprefix="pinso" %>
<%@ Register src="~/prescriberreporting/controls/MktReportScript.ascx" tagname="PrescriberReportScript" tagprefix="pinso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="scriptContainer" Runat="Server">
 <style type="text/css">
    #infoPopup .tools
    {
        padding: 0px!important;
    }
 </style>
 <pinso:PrescriberReportScript ID="PrescriberReportScript" runat="server" /> 

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" Runat="Server">
<asp:Label ID="lblPrescriberName" runat="server" Text=""></asp:Label>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="tools" Runat="Server">
    <pinso:TileOptionsMenu runat="server" ID="optionsMenu" UserRole="export" ContainerID="infoPopup" Module="prescribertrendingpopup" ExportHandler="window.top.customExport"  />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="main" runat="Server">
    <table class="genTable" cellpadding="0" cellspacing="0" >
        <tr>
            <td>
            <br />
                <telerik:RadComboBox runat="server" ID="Rollup_Type" Skin="pathfinder" 
                    EnableEmbeddedSkins="false" MaxHeight="200px" OnClientSelectedIndexChanged="TopN_Product_Changed" Width="100px">
                    <Items>
                        <telerik:RadComboBoxItem runat="server" Value="1" Text="All" />
                        <telerik:RadComboBoxItem runat="server" Value="2" Text="Top 10 Plans" />
                        <telerik:RadComboBoxItem runat="server" Value="3" Text="Top 20 Plans" />
                    </Items>        
                </telerik:RadComboBox>
            </td>
            <td>
                <label style="width: auto">
                    Rank by Product</label><br />
                <telerik:RadComboBox runat="server" ID="rcbProduct" Skin="pathfinder" EnableEmbeddedSkins="false"
                    OnClientSelectedIndexChanged="TopN_Product_Changed" Width="150px">
                    <Items>
                    </Items>
                </telerik:RadComboBox>
            </td>
            <td>
            <label style="width: auto">Channel</label><br />
                <telerik:RadComboBox runat="server" ID="Section_ID"  DataTextField="Name" DataValueField="ID"  
                AppendDataBoundItems="true" Skin="pathfinder" EnableEmbeddedSkins="false" DropDownWidth="185px"
                OnClientSelectedIndexChanged="TopN_Product_Changed" Width="200px">
                    <Items>
                        <telerik:RadComboBoxItem Text="All" Value="all" Selected="false"/> 
                        <telerik:RadComboBoxItem Text="Combined (Commercial + Part D)" Value="-1" Selected="false"/> 
                    </Items>
                </telerik:RadComboBox>
            </td>
        </tr>
        
    </table>
    <div id="physGrid">
    <pinso:PrescriberGrid runat="server" ID="PrescriberReport" ContainerID="infoPopup" />
    </div>
<pinso:ThinGrid ID="ThinGrid1" runat="server" AutoLoad="false" StaticHeader="true"
    ContainerID="infoPopup" Target="physGrid" LoadSelector=".grid" Url="~/prescriberreporting/all/MktGrid.aspx" />
</asp:Content>
