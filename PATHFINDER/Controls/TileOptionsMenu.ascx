<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TileOptionsMenu.ascx.cs" Inherits="Controls_TileOptionsMenu" %>

<telerik:RadMenu ID="tileOptionsMenu" runat="server" SkinID="options" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false"
    OnClientItemClicked="onExportMenuItemClicked" ClickToOpen="true">
        <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
        <Items>
            <telerik:RadMenuItem runat="server" Text="Options">
                <Items>
                    <%--Codebehind updates Value property to JSON value to support custom modules.  Value should be set to "type" in markup.  It is used in code. --%>
                    <telerik:RadMenuItem runat="server" SkinID="print" Value='print' Text="Print" />
                    <telerik:RadMenuItem runat="server" SkinID="excel" Value='excel' Text="Excel" />
                </Items>
            </telerik:RadMenuItem>
        </Items>
</telerik:RadMenu>
