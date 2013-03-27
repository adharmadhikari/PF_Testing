<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleSection.master" AutoEventWireup="true" CodeFile="toppayersbystate.aspx.cs" Inherits="custom_leo_reports_all_toppayersbystate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptContainer" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function() {
            $('#ddlTheraID').change(function() {
                if ($(this).val() == 53) {
                    $('#1').hide();
                    $('#83').hide();
                    $('#53').show();
                }
                else if ($(this).val() == 83) {
                    $('#1').hide();
                    $('#53').hide();
                    $('#83').show();
                }
                else {
                    $('#53').hide();
                    $('#83').hide();
                    $('#1').show();
                }
            });

            $('#ddlTheraID').change();

        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Tile3Title" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Tile3Tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Tile3" Runat="Server">
    Please select a Market Basket: <select id="ddlTheraID">
        <option value="53">Dermatological (Dermatoses)</option>
        <option value="83">Actinic Keratosis</option>
        <option value="1">Both</option>
    </select>
    <ul id="53">
        <li>
            <a target="_blank" href="custom/leo/reports/all/toppayersbystate.ashx?channel=1&topN=5&theraID=53">Top 5 Commercial Payers by State</a>
        </li>
        <li>
            <a target="_blank" href="custom/leo/reports/all/toppayersbystate.ashx?channel=1&topN=10&theraID=53">Top 10 Commercial Payers by State</a>
        </li>
        <li>
            <a target="_blank" href="custom/leo/reports/all/toppayersbystate.ashx?channel=17&topN=5&theraID=53">Top 5 Medicare Part D Payers by State</a>
        </li>
        <li>
            <a target="_blank" href="custom/leo/reports/all/toppayersbystate.ashx?channel=17&topN=10&theraID=53">Top 10 Medicare Part D Payers by State</a>
        </li>
        <li>
            <a target="_blank" href="custom/leo/reports/all/toppayersbystate.ashx?channel=17&topN=20&theraID=53">Top Accounts Medicare Part D</a>
        </li>
        <li>
            <a target="_blank" href="custom/leo/reports/all/toppayersbystate.ashx?channel=0&topN=0&theraID=53">Top Accounts Formulary Report</a>
        </li>
        
        
    </ul>
    <ul id="83">
        <li>
            <a target="_blank" href="custom/leo/reports/all/toppayersbystate.ashx?channel=1&topN=5&theraID=83">Top 5 Commercial Payers by State</a>
        </li>
        <li>
            <a target="_blank" href="custom/leo/reports/all/toppayersbystate.ashx?channel=1&topN=10&theraID=83">Top 10 Commercial Payers by State</a>
        </li>
        <li>
            <a target="_blank" href="custom/leo/reports/all/toppayersbystate.ashx?channel=17&topN=5&theraID=83">Top 5 Medicare Part D Payers by State</a>
        </li>
        <li>
            <a target="_blank" href="custom/leo/reports/all/toppayersbystate.ashx?channel=17&topN=10&theraID=83">Top 10 Medicare Part D Payers by State</a>
        </li>
        <li>
            <a target="_blank" href="custom/leo/reports/all/toppayersbystate.ashx?channel=17&topN=20&theraID=83">Top Accounts Medicare Part D</a>
        </li>       
        <li>
            <a target="_blank" href="custom/leo/reports/all/toppayersbystate.ashx?channel=0&topN=99&theraID=83">Top Accounts Formulary Report</a>
        </li>       
        
    </ul>
    <ul id="1">
        <li>
            <a target="_blank" href="custom/leo/reports/all/toppayersbystate.ashx?channel=17&topN=20&theraID=53&theraID=83">Top Accounts Medicare Part D</a>
        </li>
         <li>
            <a target="_blank" href="custom/leo/reports/all/toppayersbystate.ashx?channel=0&topN=98&theraID=53&theraID=83">Top Accounts Formulary Report</a>
        </li>
    </ul>

    

</asp:Content>

