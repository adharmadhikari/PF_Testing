<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Print.master" AutoEventWireup="true" CodeFile="Print.aspx.cs" Inherits="usercontent_Print" %>
<%@ OutputCache VaryByParam="None"  Duration="1" NoStore="true" %>
<%@ Register src="~/Controls/printexport.ascx" tagname="print" tagprefix="pinso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PrintTitle" Runat="Server">
    <asp:Literal runat="server" ID="titleText" Text='<%$ Resources:Resource, Assembly_ApplicationName %>'></asp:Literal>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptContainer" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PrintContents" Runat="Server">
    <pinso:print runat="server" ID="print" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="initializationScript" Runat="Server">
    <script type="text/javascript">
    
        //Hide print icon when document is printed
        if (typeof(window.onbeforeprint) != 'undefined')
        {
            window.onbeforeprint = HidePrintIcon;
            window.onafterprint = ShowPrintIcon;
        }
        else
        {
            //The browser is not IE so attach a print style to append the link's text when printed
            $('head').append('<style type="text/css" media="print">#printIcon { display:none!important; }</style>');
        }

        $('#printIcon').click(function()
        {
            window.print();
        });

        //Limit width of chart images to 700 if greater than 700
        $('.printContent img').each(function()
        {
            if ($(this).width() > 700)
                $(this).width(700);
        });
        
        function ShowPrintIcon()
        {
            $("#printIcon").show();
        }

        function HidePrintIcon()
        {
            $("#printIcon").hide();
        }        
         
    </script>
</asp:Content>

