//Print functionality; DivPageContentID = div ID containing print page content ; DocTitle = Document Title to be displayed
function PrintPage(DivPageContentID, DocTitle) {
    var Display_Setting = "toolbar=yes,location=no,directories=yes,menubar=yes,";
    Display_Setting += "scrollbars=yes,width=650, height=600, left=100, top=25";
    var Print_Content = document.getElementById(DivPageContentID).innerHTML;

    var docprint = window.open("", "", Display_Setting);
    docprint.document.open();
    docprint.document.write('<html><head><title>');
    docprint.document.write(DocTitle);
    docprint.document.write('</title>');
    docprint.document.write('<style type="text/css">.headerList{padding:5px;background:#215fa0;font-weight:bold;color:#fff;}.physList{width:100%;margin-bottom:20px;font-weight:bold;}div.tileSubHeader{background:#215FA0;color:#fff;font-weight:bold;}.itemList{padding:5px;font-weight:normal;}</style>');
    docprint.document.write('</head><body onLoad="self.print()"><center>');
    docprint.document.write(Print_Content);
    docprint.document.write('</center></body></html>');
    docprint.document.close();
    docprint.focus();
}

function GoalsLoaded()
{
    var element = document.getElementById('ctl00_loadgoals');
    element.style.cssText = 'display:none;';
}

//for export functionality
//hiddenfield = export page url with required querystring
function exportFile(hiddenfield,chart1,chart2,pagename,type)
    {
        var urlvalueNew ="";
        var urlvalue = "";
        if (document.getElementById(hiddenfield) != null) {
            urlvalueNew = document.getElementById(hiddenfield).value;
        }
        //append chart image url in querystring
        if (document.getElementById(chart1) != null) {
            var chart1url = "url=" + document.getElementById(chart1).src;
            urlvalueNew = urlvalueNew + '&_img0=' + chart1url;
        }
        if (document.getElementById(chart2) != null) {
            var chart2url = "url=" + document.getElementById(chart2).src;
            urlvalueNew = urlvalueNew + '&_img1=' + chart2url;
        }
        if (urlvalueNew != "") {
            urlvalueNew = urlvalueNew + '&page=' + pagename + '&type=' + type;
            //redirect the page to export page
            window.location = urlvalueNew;
        }
    }

    //for pdf export functionality with single chart
    //hiddenfield = export page url with required querystring
    function exportPDF(hiddenfield, chart1, pagename, type) {
        var urlvalueNew = "";
        if (document.getElementById(hiddenfield) != null) {
            urlvalueNew = document.getElementById(hiddenfield).value;
        }
        //append chart image url in querystring
        var data = "";
        var dataItem;
        var items = 0;
        $(" .chartThumb IMG").each(function()
        {
            dataItem = "chartid=" + this.getAttribute("_chartid") + "&height=" + this.getAttribute("_height") + "&width=" + this.getAttribute("_width") + "&title=" + (this.getAttribute("_title") ? this.getAttribute("_title") : "");

            data += ("&_img" + items + "=" + encodeURIComponent(dataItem));

            items++;
        });
        urlvalueNew = urlvalueNew + data;  
        if (urlvalueNew != "") {
            urlvalueNew = urlvalueNew + '&page=' + pagename + '&type=' + type;
            //redirect the page to export page
            window.location = urlvalueNew;
        }
    }
   