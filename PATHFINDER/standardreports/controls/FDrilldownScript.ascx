<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FDrilldownScript.ascx.cs" Inherits="standardreports_controls_FDrilldownScript" %>
<%@ OutputCache Shared="true" VaryByParam="None" Duration="100" %>

<script type="text/javascript">

//    var RadGrid1;
//    function GetGridObject() {
//        RadGrid1 = this;
//    }
    
   
    clientManager.add_pageInitialized(pageInitialized);
    clientManager.add_pageUnloaded(pageUnloaded);

    function pageInitialized() {

        var gridDrilldown = $find("ctl00_Tile3_fdrilldowndata_gridF").GridWrapper; 
      
        gridDrilldown.add_dataBound(gridF_onDataBound);
    }

    function pageUnloaded() {
        clientManager.remove_pageInitialized(pageInitialized);
        clientManager.remove_pageUnloaded(pageUnloaded);
    }

    function gridF_onDataBound(sender, args) {
        var cellIndex;
        cellIndex = 15;

        // information icon
        DisableLinks_FormularyDrilldownReport(cellIndex);
    }

    function DisableLinks_FormularyDrilldownReport(cellIndex) {
        //adjust column width
//        $(".planName").width("9%");
//        $(".geogName").width("8%");
//        $(".pharmacyLives").width("7%");
//        $(".copayRange").width("6%");
//        $(".commentsCell").width("3%");
//        $(".statusName").width("5%");
//        
    
        var grid = $find("ctl00_Tile3_fdrilldowndata_gridF");
        var masterTable = grid.get_masterTableView();
        var comment = cellIndex;   // Comments field position: CP, PBM, Managed Medicaid, Part-D - 12,   SM- 10
        
        
        
        for (var i = 0; i <= (masterTable.get_dataItems().length - 1); i++) {
            var cell;
            var href; //href for link to reassign to img tag of comments
            var j; //temp jQuery object

            var item = masterTable.get_dataItems()[i];
            var dataItem = item.get_dataItem();
 
            if (dataItem != null) {
                //Comments
                cell = item.get_element().cells[comment];

                // testing
                //cell = grid.MasterTableView.GetCellByColumnUniqueName(grid.MasterTableView.Rows[i], "Comments");
                
                
                j = $(cell).find("a");

                if (j.length) {
                    // testing
                    //alert(dataItem.Comments);

                    if (dataItem.Comments != null && $.trim(dataItem.Comments) != '')
                    {
                        href = j.attr("href").replace("javascript:", "");
                        j.html("<img src='content/images/information.png' onmouseout=\"$('#infoPopup').hide()\" onmouseover='" + href + "' />");
                    }
                    else {
                        j.html("&nbsp;"); //clean up if new data 
                    }
                }
            }
        }
    }
    //
    function OpenNotesViewer(Plan_ID, Drug_ID, FormularyID, SegmentID, Type, x, y, width, height) {
        //var app = clientManager.get_ApplicationManager();
        //var url = app.getUrl("all", clientManager.get_Module(), "OpenNotes.aspx");
        
        var url = "todaysaccounts/all/OpenNotes.aspx";
        url = url + "?Plan_ID=" + Plan_ID + "&Drug_ID=" + Drug_ID + "&FormularyID=" + FormularyID + "&SegmentID=" + SegmentID + "&Type=" + Type;
        
        //var mt = $get(gridCLDrillDownID).control.get_masterTableView();
        var mt = $find("ctl00_Tile3_fdrilldowndata_gridF").get_masterTableView();
        var cell;

        //Get the list of dataitems which matches the selected Drug_ID.
        var list = $.grep(mt.get_dataItems(), function(i) {
            if (i.get_dataItem()) {
                return i.get_dataItem().Drug_ID == Drug_ID && i.get_dataItem().Plan_ID == Plan_ID && i.get_dataItem().Formulary_ID == FormularyID;
            }
            else
                return false;
        }, false);

        //Get the cellIndex for selected cell.
        if (list && list.length > 0) {
            var col;
            var rect;

//            if (Type == "ST") {
//                col = mt.getColumnByUniqueName("ST_Restrictions");
//            }
//            else if (Type == "QL") {
//                col = mt.getColumnByUniqueName("QL_Restrictions");
//            }
//            else if (Type == "comments") {

            if (Type == "comments") {
                col = mt.getColumnByUniqueName("Comments");
            }

            if (col) {
                cell = list[0].get_element().cells[col.get_element().cellIndex];
            }
            else
                throw new Error("Cannot find column 'Comments'");
        }

        //Getting cell bounds.
        rect = Sys.UI.DomElement.getBounds(cell);

        //alert(rect.y);

        var rect_Y;
        if (rect.y < 0) {
            rect_Y = -(rect.y);
            //alert(rect_Y);
            clientManager.openViewer(url, rect.x - width, rect_Y, width, height);
        }
        else {

            //Open pop-up window with calculated co-ordinates.
            clientManager.openViewer(url, rect.x - width, rect.y, width, height);
        }
    }  
 
  
</script>

