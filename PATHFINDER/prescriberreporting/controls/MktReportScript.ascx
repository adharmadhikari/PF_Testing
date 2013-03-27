<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MktReportScript.ascx.cs" Inherits="prescriberreporting_controls_PrescriberReportScript" %>
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


    function TopN_Product_Changed(sender, args) 
    {
        var data = clientManager.getContextValue("physTrendingQuery");

        var rollupCtrl = $find("ctl00_ctl00_partialPage_main_Rollup_Type");
        var productCtrl = $find("ctl00_ctl00_partialPage_main_rcbProduct");
        var channelCtrl = $find("ctl00_ctl00_partialPage_main_Section_ID");
        
                
        data["Rollup_Type"] = rollupCtrl.get_value();
        data["Product_ID"] = productCtrl.get_value();

        if (channelCtrl.get_value() != 'all')
            data["Section_ID"] = channelCtrl.get_value();
        else
        {
            delete (data["Section_ID"]);
            delete (data["Segment_ID"]);
        }
        
        var grid = $find("physGrid");

        if (grid) {
            grid.set_params(data);
            grid.dataBind();
        }
    }
    
    function customExport(type, module)
    {
        //custom logic here..
        var data = clientManager.getContextValue("physTrendingQuery"); 
                            
        //window.top.clientManager.exportView(type, false, module, data); //2nd param should false if not using confirmation
        $exportModule(type, true, 15, 0, module, data);
    }
   

</script>

