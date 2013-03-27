<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BasicInfoCoveredLives.ascx.cs" Inherits="custom_merz_businessplanning_controls_BasicInfoCoveredLives" %>
<%@ Register src="~/custom/merz/businessplanning/controls/PlanInfoAddress.ascx" tagname="PlanInfo" tagprefix="BasicInfo" %>
<%@ Register src="~/custom/merz/businessplanning/controls/CoveredLives.ascx" tagname="CLlives" tagprefix="BasicInfo" %>
<%@ Register src="~/custom/merz/businessplanning/controls/CoveredLives_MC.ascx" tagname="CLlives_MC" tagprefix="BasicInfo" %>
<div id="BasicInfoDiv" runat="server" class="leftBPTile divborder leftSmPDFTile leftSmTile">
    <div class="tileContainerHeader"><div class="title">Basic Information</div></div>
    <div><BasicInfo:PlanInfo ID="Planaddress1" runat="server"/></div>
</div>
<div runat="server" id="CLCommDiv" class="leftBPTile divborder leftSmPDFTile leftSmTile">
    <div class="tileContainerHeader"><div class="title">Covered Lives - Commercial</div></div>
    <div><BasicInfo:CLlives ID="PlanCL1" runat="server"/></div>
</div>
<div id="CLMedicareDiv" runat="server" class="leftBPTile divborder leftSmPDFTile leftSmTile">
    <div class="tileContainerHeader"><div class="title">Covered Lives - Medicare</div></div>
    <div><BasicInfo:CLlives_MC ID="CLlivesMC" runat="server"/></div>
</div> 
