<%@ Page EnableViewState="true" Language="C#" AutoEventWireup="true" CodeFile="ReportViewer.aspx.cs" Inherits="powerplanrx_all_ReportViewer" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>

<body style="margin: 0 0 0 0;">
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager AsyncPostBackTimeout="900" ID="ScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Path="https://ajax.microsoft.com/ajax/jquery/jquery-1.3.2.min.js" />
            </Scripts>
        </asp:ScriptManager>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" ProcessingMode="Remote" Width="100%" AsyncRendering="False">        
            <ServerReport  />
        </rsweb:ReportViewer>  
        <script language="javascript" type="text/javascript">

            ResizeReportViewer();
            function ResizeReportViewer()
            {
                var rptviewer = document.getElementById("<%= ReportViewer1.ClientID %>");
                var htmlheight = document.documentElement.clientHeight;
                var htmlwidth = document.documentElement.clientWidth;
                //the following code will center report within the report viewer control
                //get report cell
                var cell = $('td[id$="oReportCell"]');
                //remove blank cell near report
                 $(cell).next('td').remove();;              
                //get parent table
                 var table = $(cell).parent('tr').parent('tbody').parent('table')
                //center table
                 $(table).css("margin-left", "auto").css("margin-right", "auto");
                //get parent div
                var div = $(table).parent('div');
                // $(div).css("text-align", "center");
              

                if ($.browser.msie) {
                    $(div).css("height", (htmlheight - 35));        
                    rptviewer.style.height = (htmlheight - 35) + "px";
                } else {
                    $(div).css("height", (htmlheight -85));
                    rptviewer.style.height = (htmlheight - 85) + "px";
                }
                rptviewer.style.width = htmlwidth;
                //allow scrolling
                $(div).css("overflow", "auto");
                parent.dialogModule.close();
            }
            window.onresize = function resize() { ResizeReportViewer(); } 
        </script> 
    </div>
    </form>
</body>
</html>
