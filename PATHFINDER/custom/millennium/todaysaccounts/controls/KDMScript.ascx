<%@ Control Language="C#" AutoEventWireup="true" CodeFile="KDMScript.ascx.cs" Inherits="custom_millennium_todaysaccounts_controls_KDMScript" %>
<%@ OutputCache Shared="true" VaryByParam="None" Duration="100" %>



<script type="text/javascript">
    clientManager.add_pageInitialized(KDMDetailsVARPageInitialized);
    clientManager.add_pageUnloaded(KDMDetailsVARPageUnloaded);
    
    function KDMDetailsVARPageInitialized()
    {
        
        var e = $get("ctl00_Tile3_KDMDetailsVAR_gridKDMDetailsVAR");
        if (e && e.control)
        {
            var grid = e.control.get_masterTableView();

            if (grid)
            {
                $clearGridFilter(grid, "VISN");
            }
        }

        var e = $get("ctl00_Tile3_KDMAddressVAR1_gridKDMAddress");
        if (e && e.control)
        {
            var grid = e.control.get_masterTableView();

            if (grid)
            {
                $clearGridFilter(grid, "VISN");
            }
        }
        
    }

    function KDMDetailsVARPageUnloaded() {
        clientManager.remove_pageInitialized(KDMDetailsVARPageInitialized);
        clientManager.remove_pageUnloaded(KDMDetailsVARPageUnloaded);

    }

 
</script>