<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Popup.master" AutoEventWireup="true" CodeFile="PrescriberPopup.aspx.cs" Inherits="marketplaceanalytics_all_PrescriberPopup" %>
<%@ Register src="~/marketplaceanalytics/controls/PrescriberGrid.ascx" tagname="PrescriberGrid" tagprefix="pinso" %>
<%@ Register src="~/marketplaceanalytics/controls/PrescriberReportScript.ascx" tagname="PrescriberReportScript" tagprefix="pinso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="scriptContainer" Runat="Server">

 <pinso:PrescriberReportScript ID="PrescriberReportScript" runat="server" /> 

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" Runat="Server">
<asp:Label ID="popupPlanName" runat="server" Text=""></asp:Label>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="tools" Runat="Server">
    <pinso:TileOptionsMenu runat="server" ID="optionsMenu" UserRole="export" ContainerID="infoPopup" Module="prescribers" ExportHandler="window.top.customExport"  />

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="main" runat="Server">
    <%--<div class="filterGeo">
        <asp:Literal runat="server" ID="filterLabel" Text="Top Prescribers" />
    </div>--%>
    <table class="genTable" cellpadding="0" cellspacing="0" width="800px">
        <%--<tr>
            <td>
                &nbsp;
            </td>
            <td>
                <label style="width: auto">
                    Rank by Product</label>
            </td>
            <td>
                <label style="width: auto">
                    Region</label>
            </td>
            <td>
                <label style="width: auto">
                    District</label>
            </td>
            <td>
                <label style="width: auto">
                    Territory</label>
            </td>
        </tr>--%>
        <tr>
            <td>
            <br />
                <telerik:RadComboBox runat="server" ID="TopN" DataTextField="Prescribers_TopN_Name"
                    DataValueField="Prescribers_TopN_ID" Skin="pathfinder" EnableEmbeddedSkins="false"
                    OnClientSelectedIndexChanged="TopN_Product_Changed" Width="70px">
                    <Items>
                        <%--<telerik:RadComboBoxItem Text="Top 50 Prescribers" Value="50" Selected="true" />
                    <telerik:RadComboBoxItem Text="Top 100 Prescribers" Value="100" />
                    <telerik:RadComboBoxItem Text="Top 500 Prescribers" Value="500" />--%>
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
            <%--   Region, State, Territory Filters --%>
            <td>
            <asp:Label ID="lblRegion" runat="server" Text=""  style="width: auto"></asp:Label><br />
                 <%--<label style="width: auto">
                    Region</label><br />--%>
                <telerik:RadComboBox runat="server" ID="Region_ID" DataValueField="Region_Name" DataTextField="Region_Name"
                    Skin="pathfinder" EnableEmbeddedSkins="false" Width="100px">
                </telerik:RadComboBox>
            </td>
            <td>
            <asp:Label ID="lblDistrict" runat="server" Text=""  style="width: auto"></asp:Label><br />
               <%-- <label style="width: auto">
                    District</label><br />--%>
                <telerik:RadComboBox runat="server" ID="District_ID" DataValueField="District_Name"
                    DataTextField="District_Name" Skin="pathfinder" EnableEmbeddedSkins="false" Width="150px">
                </telerik:RadComboBox>
            </td>
            <td>
            <asp:Label ID="lblTerritory" runat="server" Text=""  style="width: auto"></asp:Label><br />
           <%-- <label style="width: auto">
                    Territory</label><br />--%>
                <telerik:RadComboBox runat="server" ID="Territory_ID" DataValueField="Territory_Name"
                    DataTextField="Territory_Name" Skin="pathfinder" EnableEmbeddedSkins="false" Width="150px">
                </telerik:RadComboBox>
            </td>
        </tr>
       <tr><td colspan="5"><label style="height: 10px"></label></td></tr>
        
    </table>
<%--    
 <div class="clearAll">
        </div>--%>

    <div id="physGrid">
    <pinso:PrescriberGrid runat="server" ID="PrescriberReport" ContainerID="infoPopup" />
    </div>
<pinso:ThinGrid ID="ThinGrid1" runat="server" AutoLoad="false" StaticHeader="true"
    ContainerID="infoPopup" Target="physGrid" LoadSelector=".grid" Url="~/marketplaceanalytics/all/PrescriberGrid.aspx" />
   
</asp:Content>
