<%@ Page Title="" Language="C#" EnableViewState="true" MasterPageFile="~/custom/MasterPages/SellSheetStep.master" AutoEventWireup="true" CodeFile="marketandformularyselection.aspx.cs" Inherits="custom_pinso_sellsheets_marketandformularyselection" %>
<%@ MasterType VirtualPath="~/custom/MasterPages/SellSheetStep.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="scriptContainer" Runat="Server">
    <script type="text/javascript" >
        function updateTextbox(chk, txt)
        {
            var txtVal = $("#" + txt).val();

            if (chk.checked == true) 
                $("#" + txt).val(txtVal + chk.id);
            else 
                $("#" + txt).val(txtVal.replace(chk.id, ""));
        }

        function validate()
        {
            var t = $("#" + txtSegmentID).val();

            if (t == "")
                $("#segmentInvalid").addClass("sellsheetinvalid");
            else
                $("#segmentInvalid").removeClass("sellsheetinvalid");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="StepBody" Runat="Server">
<div id="divIndent">
        <asp:HiddenField runat="server" ID="txtSegment" />
        <pinso:ClientValidator ID="vldSegment" runat="server" Text="Please select at least one Segment" Target="txtSegment" Required="true" />
        <div class="spacedMarketRows">
            <span class="ssBold">Segment</span><span class="requiredRed">*</span> 
        </div>
        <div class="spacedMarketRows">
            <div id="segmentInvalid" style="display: inline" >
                <asp:CheckBox ID="chkSegmentCP" runat="server" Text="Commercial" OnClick="updateTextbox(this, txtSegmentID );" Visible="false" CssClass="listItemWidth"/>
                <asp:CheckBox ID="chkSegmentMD" runat="server" Text="Medicare Part D (Plan Product)" OnClick="updateTextbox(this, txtSegmentID);" Visible="false" CssClass="listItemWidth"/>
            </div>
        </div>
        <div class="spacedMarketRows">
            <span class="ssBold">Status Type</span><span class="requiredRed">*</span>
        </div>
        <div>
            <asp:RadioButtonList ID="rblStatusType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="listItemWidth"
                DataSourceID="dsSellSheetType" DataTextField="Type_Name" 
                DataValueField="Type_ID" AppendDataBoundItems="true">
            </asp:RadioButtonList>
        </div>
        <pinso:ClientValidator ID="vldStatusType" runat="server" Text="Please select a Status Type" Target="rblStatusType" Required="true" /><br />
        <asp:HiddenField runat="server" ID="txtRestrictions" />
        <div class="spacedMarketRows">
            <span class="ssBold">Which Restrictions Do You Wish To Include?</span>
        </div>
        <div class="spacedMarketRows">
            <asp:CheckBox ID="chkPA" runat="server" Text="PA" OnClick="updateTextbox(this, txtRestrictionsID);" CssClass="listItemWidth"/>
            <asp:CheckBox ID="chkQL" runat="server" Text="QL" OnClick="updateTextbox(this, txtRestrictionsID);" CssClass="listItemWidth"/>
            <asp:CheckBox ID="chkST" runat="server" Text="ST" OnClick="updateTextbox(this, txtRestrictionsID);" CssClass="listItemWidth"/><br />
        </div>
        <div class="spacedMarketRows">
            <span class="ssBold">Do You Wish To Include Co-Pay?</span>
        </div>
        <div>
            <asp:RadioButtonList ID="rblCopay" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="listItemWidth">
                <asp:ListItem Text="Yes" Value="True" />
                <asp:ListItem Text="No" Value="False" Selected="True" />
            </asp:RadioButtonList>
        </div>
</div>
            <input type="button" class="btnPrev"
        onclick="clientManager.get_ApplicationManager().back(clientManager)"  
        value="Back" />
    <asp:Button ID="btnNext" runat="server" Text="Next" CssClass="postback validate btnNext" 
        onclick="btnNext_Click" onclientclick="javascript: validate();" />
        <asp:EntityDataSource ID="dsSellSheetType" runat="server" EntitySetName="SellSheetTypeSet" DefaultContainerName="PathfinderClientEntities" EnableUpdate="true">
        </asp:EntityDataSource>
</asp:Content>

