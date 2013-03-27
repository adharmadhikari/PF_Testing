<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FormularyStatusData.ascx.cs" Inherits="standardreports_controls_FormularyStatusData" %>
<%@ Register src="~/standardreports/controls/FormularyStatusDataTemplate.ascx" tagname="FormularyStatusTemplate" tagprefix="pinso" %>
<div class="formularystatus">
    <pinso:FormularyStatusTemplate runat="server" Visible="false" ID="gridNational" />
    <pinso:FormularyStatusTemplate runat="server" Visible="false" ID="gridRegional" />
</div>