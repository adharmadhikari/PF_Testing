<%@ Control Language="C#" %>
<%@ OutputCache Shared="true" VaryByParam="None" Duration="100" %>

<script type="text/javascript">

    clientManager.add_pageInitialized(affiliations_pageInitialized);
    clientManager.add_pageUnloaded(affiliations_pageUnloaded);

    function affiliations_pageInitialized(sender, args)
    {
        var g = $find("ctl00_Tile3_AffiliationsListView_gridAffiliations$GridWrapper");
        if (g) g.add_dataBinding(affiliation_onDataBinding);
    }

    function affiliations_pageUnloaded(sender, args)
    {
        clientManager.remove_pageInitialized(affiliations_pageInitialized);
        clientManager.remove_pageUnloaded(affiliations_pageUnloaded);
    }

    function affiliation_onDataBinding(sender, args)
    {
        $clearGridFilter(sender, "Original_Section");
    }

    function onAffiliationsRowSelected(sender, args) 
    {
        var row = args.get_item().get_element();
        var rect = Sys.UI.DomElement.getBounds(row);
        var data = args.get_gridDataItem().get_dataItem();

        var app = clientManager.get_ApplicationManager();
        var channel = clientManager.getChannelUrlName(1, data["Section_ID"]);
        var url = app.getUrl(channel, clientManager.get_Module(), "planinfodetailspopup.aspx");

        var showKeyContactList = "false";
        var opts = $.grep(clientManager.getModuleOptionsByApp(clientManager.get_Application()), function(i, x) { return i.ID == "contacts"; });
        if (opts && opts.length > 0)
            showKeyContactList = "true";

        url = url + "?plan_id=" + data["Child_Plan_ID"] + "&Plan_Name=" + encodeURIComponent(data["Plan_Name"]) + "&showKeyContact=" + showKeyContactList;
        clientManager.openToolTipViewerWithQueryString(url, 200, null, null, null, data["Plan_Name"], sender.get_masterTableView().get_element().id);

    }
    //on row selection of MED D plans from affiliations list view.
    function onMedDAffiliationsRowSelected(sender, args) {
        var row = args.get_item().get_element();
        var rect = Sys.UI.DomElement.getBounds(row);
        var data = args.get_gridDataItem().get_dataItem();
        //start of code for coloring the selected rows as per the selected plan_id.

        $(sender.get_masterTableView().get_element()).find(".rgSelectedRow").removeClass("rgSelectedRow");
        var planid = data.Child_Plan_ID;

        var grid = $find("ctl00_Tile3_AffiliationsListView_gridAffiliations");

        var masterTable = grid.get_masterTableView();

        for (var i = 0; i <= (masterTable.get_dataItems().length - 1); i++) {

            var item = masterTable.get_dataItems()[i];
            var dataItem = item.get_dataItem();
            if (dataItem != null) {
            //highlight the row if the selected plan_id = child_plan_id of the record (i.e. affiliated plan id)
                if (dataItem.Child_Plan_ID == planid) {
                    $(item.get_element()).addClass("rgSelectedRow");
                }
            }
        }
        //end of code addition for coloring / highlighting the selected rows.  
        var title = data["Plan_Name"];
        var app = clientManager.get_ApplicationManager();
        var channel = clientManager.getChannelUrlName(1, 17);
        var url = app.getUrl(channel, clientManager.get_Module(), "planinfodetailspopup.aspx");

        var showKeyContactList = "false";
        var opts = $.grep(clientManager.getModuleOptionsByApp(clientManager.get_Application()), function(i, x) { return i.ID == "contacts"; });
        if (opts && opts.length > 0)
            showKeyContactList = "true";

        url = url + "?plan_id=" + data["Child_Plan_ID"] + "&Prod_State=" + data["Plan_State"] + "&Plan_Name=" + encodeURIComponent(data["Plan_Name"]) + "&showKeyContact=" + showKeyContactList;
        clientManager.openToolTipViewerWithQueryString(url, 200, null, null, null, data["Plan_Name"], sender.get_masterTableView().get_element().id);
    }
    function clearSelection(sender, args) {
        $(sender.get_masterTableView().get_element()).find(".rgSelectedRow").removeClass("rgSelectedRow");
    }
    function onDOD_RowDataBound(sender, args) {
        var test = args.get_dataItem()["Mid_Parent_Plan_ID"];
        if (test == 0)
            $find("ctl00_Tile3_AffiliationsListView_gridAffiliations").get_masterTableView().hideColumn(0);
        else
            $find("ctl00_Tile3_AffiliationsListView_gridAffiliations").get_masterTableView().showColumn(0);
    }
      
</script>