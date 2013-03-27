<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterMedicalPolicyWithAll.ascx.cs" Inherits="custom_merz_businessplanning_controls_FilterMedicalPolicyWithAll" %>
<%@ Register src="~/custom/merz/businessplanning/controls/FilterMedicalPolicy.ascx" tagname="medicalpolicy" tagprefix="pinso" %>
<pinso:medicalpolicy runat="server" ID="medicalPolicy" IncludeAll="true"   />