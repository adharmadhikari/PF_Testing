<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ModuleSelection.ascx.cs" Inherits="pathfinder_controls_ModuleSelection" %>

<%@ Register Namespace="Pathfinder" TagPrefix="pinso" %>
<div id="channelSelectorContainer" class="channelSelectorContainer">
    <div class="moduleSelector" id="channelSelector" style="display:none"></div>
    <pinso:Menu runat="server" ID="menuControl1" Target="channelSelector" CssClass="coreBtn" SelectedCssClass="selected" />
</div>
<div class="clearAll"></div>
<div id="moduleSubHeader" class="moduleSubHeader tileContainerHeader">
    <div class="title"></div>
    <div class="clearAll"></div>
</div>
<div class="moduleSelector" id="moduleSelector"></div>
<pinso:Menu runat="server" ID="menuControl2" Target="moduleSelector" CssClass="coreBtn" SelectedCssClass="selected" />
<div id="moduleOptionsContainer" ></div>