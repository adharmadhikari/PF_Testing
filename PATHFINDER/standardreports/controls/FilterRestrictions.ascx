<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterRestrictions.ascx.cs" Inherits="standardreports_controls_FilterRestrictions" %>
<%@ Register Namespace="Pathfinder" TagPrefix="pinso" %>
<div class="filterGeo">
    <asp:Literal runat="server" ID="filterLabel" Text='<%$ Resources:Resource, Label_Restrictions %>'  />
    </div>
<pinso:CheckboxValueList CssClass="chkBoxDiv" runat="server" ID="restrictions" ItemCssClass="queryExt"  DataSourceID="dsRestrictions" DataTextField="ID" HasAllOption="false" TrueFalse="false" TrueValue="Y" DataValueField="ID" RepeatDirection="Horizontal" RepeatColumns="3"  />
<pinso:CheckboxListClientControl runat="server" ID="restrictionsList" Target="restrictions" ContainerID="moduleOptionsContainer" />

<asp:EntityDataSource runat="server" ID="dsRestrictions" DefaultContainerName="PathfinderEntities" ConnectionString="name=PathfinderEntities" EntitySetName="RestrictionSet" />
