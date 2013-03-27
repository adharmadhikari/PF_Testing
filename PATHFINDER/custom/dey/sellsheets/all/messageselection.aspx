<%@ Page Title="" Language="C#" EnableViewState="true" MasterPageFile="~/custom/MasterPages/SellSheetStep.master" AutoEventWireup="true" CodeFile="messageselection.aspx.cs" Inherits="custom_unitedthera_sellsheets_messageselection" %>
<%@ MasterType VirtualPath="~/custom/MasterPages/SellSheetStep.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="scriptContainer" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="StepBody" Runat="Server">
<div id="divIndent">
<%--    <asp:RadioButtonList ID="rblMessage" runat="server" RepeatLayout="Flow" CssClass="listItemWidth" DataSourceID="dsSellSheetMessages" 
        DataTextField="Message_Name" DataValueField="Message_ID" RepeatDirection="Vertical" CellSpacing="10">
        </asp:RadioButtonList>
        <pinso:ClientValidator ID="vldMessage" runat="server" Text="Please select a Message" Target="rblMessage" Required="true" />
    <br /><br />--%>
    <asp:CheckBox runat="server" Checked="false" ID="chkGeo" CssClass="listItemWidth" text="Include Geography Name"/>   
    <br /><br />
    <%--<asp:CheckBox runat="server" Checked="false" ID="chkHighlightProd" CssClass="listItemWidth" text="Highlight Client's Product"/>   --%>
</div>
        <input type="button" class="btnPrev"
        onclick="clientManager.get_ApplicationManager().back(clientManager)"  
        value="Back" />
    <asp:Button ID="btnNext" runat="server" Text="Next" CssClass="postback validate btnNext" 
        onclick="btnNext_Click" />
        <asp:EntityDataSource ID="dsSellSheetMessages" runat="server" EntitySetName="SellSheetMessagesSet" DefaultContainerName="PathfinderClientEntities" EnableUpdate="true">
        </asp:EntityDataSource>
</asp:Content>

