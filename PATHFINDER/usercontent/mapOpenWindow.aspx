<%@ Page Language="C#" Theme="pathfinder" MasterPageFile="~/MasterPages/Modal.master"  AutoEventWireup="true" CodeFile="mapOpenWindow.aspx.cs" Inherits="mapOpenWindow" %>

<%@ Register Src="~/controls/map.ascx" TagName="map" TagPrefix="map" %>
<asp:Content runat="server" ID="content1" ContentPlaceHolderID="main">

     <map:map ID="map1" runat="server" ContainerFileName="mapContainer.swf" ChangePath="../" />

<script type="text/javascript">
    var clientManager = window.top.clientManager;
    
    var c = $get("fmASEngine")
    if (c && c.initializeMap)
        c.initializeMap("areas/maptheme.ashx?s=" + clientManager.get_CurrentUIStateAsText());
    //
</script>
</asp:Content>