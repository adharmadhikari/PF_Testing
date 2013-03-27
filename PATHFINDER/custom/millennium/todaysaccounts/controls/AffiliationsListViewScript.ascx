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
        $clearGridFilter(sender, "Plan_ID");

        //Pull VISN from PlanInfo grid's selected row.
        var mt = clientManager.get_PlanInfoGrid().get_masterTableView();
        if (mt.get_virtualItemCount() > 0)
        {
            var items = mt.get_selectedItems();
            var data = {};
            if (items != null && items.length > 0)
            {
                var names = mt.get_clientDataKeyNames();

                for (var i = 0; i < names.length; i++)
                {
                    data[names[i]] = items[0].get_dataItem()[names[i]];
                }
            }
        }

        //Add VISN filter to get affiliations data.
        var iVISN = 0;
        if (data)
            iVISN = data.VISN;

        var name = 'VISN';
        var value = iVISN;
        var dataType = 'System.int32';

        var e = $get("ctl00_Tile3_AffiliationsListView_gridAffiliations");
        if (e && e.control)
        {
            var grid = e.control.get_masterTableView();

            if (grid)
            {
                if (value != null && value != "")
                    $setGridFilter(grid, name, value, null, dataType);
                else if (name)
                    $clearGridFilter(grid, name);
            }
        }
    }

    function clearSelection(sender, args) {
        $(sender.get_masterTableView().get_element()).find(".rgSelectedRow").removeClass("rgSelectedRow");
    }

      
</script>