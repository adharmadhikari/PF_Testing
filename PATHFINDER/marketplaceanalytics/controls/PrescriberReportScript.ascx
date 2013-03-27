<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PrescriberReportScript.ascx.cs" Inherits="marketplaceanalytics_controls_PrescriberReportScript" %>
<%@ OutputCache Shared="true" VaryByParam="None" Duration="100" %>


<script type="text/javascript">
    
    $(document).ready(function()
    {
        //IE6 Hacks
        if (ie6)
        {
            $("#physGrid").height(200);
            setTimeout("resizeStaticHeaders('physGrid')", 1000);

            $("#infoPopup .close").mouseup(function()
            {
                $("#timeFrameLeft1").width("40.5%");
            });
        }

    });

    
    function TopN_Product_Changed(sender, args) {
        var data = clientManager.getContextValue("physQuery");
        //var topN = args.get_item().get_value();


        var c1 = $find("ctl00_ctl00_partialPage_main_TopN");
        var c2 = $find("ctl00_ctl00_partialPage_main_rcbProduct");
        
        // 1/7/2011 Region testing
        var cR = $find("ctl00_ctl00_partialPage_main_Region_ID");
        var cD = $find("ctl00_ctl00_partialPage_main_District_ID");
        var cT = $find("ctl00_ctl00_partialPage_main_Territory_ID");
        //alert(cR.get_value() + "~" + cD.get_value() + "~" + cT.get_value());
       
        //Region
        if (data["Phy_Region_ID"] && cR.get_value() == 'all')
            delete (data["Phy_Region_ID"]);
        
        if (cR.get_value() != 'all')
            data["Phy_Region_ID"] = cR.get_value();

        //District
        if (data["Phy_District_ID"] && cD.get_value() == 'all')
            delete (data["Phy_District_ID"]);

        if (cD.get_value() != 'all')
            data["Phy_District_ID"] = cD.get_value();


        //Territory
        if (data["Phy_Territory_ID"] && cT.get_value() == 'all')
            delete (data["Phy_Territory_ID"]);

        if (cT.get_value() != 'all')
            data["Phy_Territory_ID"] = cT.get_value();
       
        /////////////////////////////////

        var c1_val = c1.get_value();
        var c2_val = c2.get_value();
        
        //data["TopN"] = topN;     //topN is selected value in droplist
        data["TopN"] = c1_val;
        data["SelectedProduct"] = c2_val;

        //alert(data["TopN"]);
        //alert(data["SelectedProduct"]);

        var grid = $find("physGrid");

        //alert(grid);
        
        if (grid) {
            grid.set_params(data);
            grid.dataBind();
        }
    }
    
    function customExport(type, module)
    {
        //custom logic here..
        var data = clientManager.getContextValue("physQuery"); 
                            
        //window.top.clientManager.exportView(type, false, module, data); //2nd param should false if not using confirmation
        $exportModule(type, true, 2, 0, module, data);
    }
   

</script>

