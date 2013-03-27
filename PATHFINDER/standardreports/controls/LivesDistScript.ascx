<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LivesDistScript.ascx.cs" Inherits="standardreports_controls_LivesDistScript" %>
<%@ OutputCache Shared="true" VaryByParam="None" Duration="100" %>

<script type="text/javascript">

    clientManager.add_pageInitialized(pageInitialized);
    clientManager.add_pageUnloaded(pageUnloaded);

    function pageInitialized() {
        var gridLivesDist = $find("ctl00_Tile3_gridLivesDistribution_gridLDReport").GridWrapper;
        gridLivesDist.add_dataBound(gridLDReport_onDataBound);

    }

    function pageUnloaded() {
        clientManager.remove_pageInitialized(pageInitialized);
        clientManager.remove_pageUnloaded(pageUnloaded);
    }

    function gridLDReport_onDataBound(sender, args)
    {
        var channel = 0;
        var data = clientManager.get_SelectionData();
        var curChannel = 0;

        if (typeof (data["Section_ID"]) != "undefined")
        {
            channel = data["Section_ID"].value;

        }
        var isAllSelected = false;
        var channelcnt = 0;
        var show = ".Plan_Name,.Geography_Name,.Total_Covered,.Total_Pharmacy";
        //Commercial columns
        var hide = ",.Total_Commercial_Lives,.Commercial_Pharmacy_Lives,.HMO_Lives,.PPO_Lives,.POS_Lives,.CDH_Lives,.Self_Insured_Lives";
        //Managed Medicaid columns
        hide += ",.Managed_Medicaid_Medical_Lives,.Managed_Medicaid_Lives";
        //Medicare Part-D lives
        hide += ",.Medicare_PartD_Lives,.MAPD_Lives,.PDP_Lives,.MAPD_LIS_Lives,.PDP_LIS_Lives";
        //PBM columns
        hide += ",.Employer_Lives,.Employer_Formulary,.Health_Plan_Lives,.Health_Plan_Processor,.Health_Plan_Formulary,.Health_Plan_Commercial,.Health_Plan_Medicare_Part_D,.Health_Plan_Managed_Medicaid";
        //State Medicaid columns
        hide += ",.Medicaid_Enrollment,.Medicaid_Mcare_Enrollment,.FFS_Lives";
        

        if ($.isArray(channel))
        {
            //Loop through selected channels
            for (var c = 0; c < channel.length; c++)
            {
                curChannel = parseInt(channel[c].toString(), 10);
                switch (curChannel)
                {
                    case 1: //Commercial
                        show += ",.Total_Commercial_Lives,.Commercial_Pharmacy_Lives,.HMO_Lives,.PPO_Lives,.POS_Lives,.CDH_Lives,.Self_Insured_Lives";
                        hide = hide.replace(",.Total_Commercial_Lives,.Commercial_Pharmacy_Lives,.HMO_Lives,.PPO_Lives,.POS_Lives,.CDH_Lives,.Self_Insured_Lives", "");
                        break;
                    case 4: //PBM
                        show += ",.Employer_Lives,.Employer_Formulary,.Health_Plan_Lives,.Health_Plan_Processor,.Health_Plan_Formulary,.Health_Plan_Commercial,.Health_Plan_Medicare_Part_D,.Health_Plan_Managed_Medicaid";
                        hide = hide.replace(",.Employer_Lives,.Employer_Formulary,.Health_Plan_Lives,.Health_Plan_Processor,.Health_Plan_Formulary,.Health_Plan_Commercial,.Health_Plan_Medicare_Part_D,.Health_Plan_Managed_Medicaid", "");
                        break;
                    case 6: //Managed Medicaid
                        show += ",.Managed_Medicaid_Medical_Lives,.Managed_Medicaid_Lives";
                        hide = hide.replace(",.Managed_Medicaid_Medical_Lives,.Managed_Medicaid_Lives", "");
                        break;
                    case 9: //state medicaid
                        show += ",.Medicaid_Enrollment,.Medicaid_Mcare_Enrollment,.FFS_Lives";
                        hide = hide.replace(",.Medicaid_Enrollment,.Medicaid_Mcare_Enrollment,.FFS_Lives", "");
                        break;
                    case 17: // Medicare Part D
                        show += ",.Medicare_PartD_Lives,.MAPD_Lives,.PDP_Lives,.MAPD_LIS_Lives,.PDP_LIS_Lives";
                        hide = hide.replace(",.Medicare_PartD_Lives,.MAPD_Lives,.PDP_Lives,.MAPD_LIS_Lives,.PDP_LIS_Lives", "");
                        break;
                } //end of switch

                if (curChannel != 11 && curChannel != 12)
                    channelcnt++;
            } //End of for loop
        } //End of if
        else
        {
            curChannel = parseInt(channel.toString(), 10);
            switch (curChannel)
            {
                case 0: //All
                    isAllSelected = true;
                    //Commercial columns
                    show += ",.Total_Commercial_Lives,.Commercial_Pharmacy_Lives,.HMO_Lives,.PPO_Lives,.POS_Lives,.CDH_Lives,.Self_Insured_Lives";
                    //Managed Medicaid columns
                    show += ",.Managed_Medicaid_Medical_Lives,.Managed_Medicaid_Lives";
                    //Medicare Part-D lives
                    show += ",.Medicare_PartD_Lives,.MAPD_Lives,.PDP_Lives,.MAPD_LIS_Lives,.PDP_LIS_Lives";
                    //PBM columns
                    show += ",.Employer_Lives,.Employer_Formulary,.Health_Plan_Lives,.Health_Plan_Processor,.Health_Plan_Formulary,.Health_Plan_Commercial,.Health_Plan_Medicare_Part_D,.Health_Plan_Managed_Medicaid";
                    //State Medicaid columns
                    show += ",.Medicaid_Enrollment,.Medicaid_Mcare_Enrollment,.FFS_Lives";
                    hide = "";
                    break;
                case 1: //Commercial
                    show += ",.Total_Commercial_Lives,.Commercial_Pharmacy_Lives,.HMO_Lives,.PPO_Lives,.POS_Lives,.CDH_Lives,.Self_Insured_Lives";
                    hide = hide.replace(",.Total_Commercial_Lives,.Commercial_Pharmacy_Lives,.HMO_Lives,.PPO_Lives,.POS_Lives,.CDH_Lives,.Self_Insured_Lives", "");
                    break;
                case 4: //PBM
                    show += ",.Employer_Lives,.Employer_Formulary,.Health_Plan_Lives,.Health_Plan_Processor,.Health_Plan_Formulary,.Health_Plan_Commercial,.Health_Plan_Medicare_Part_D,.Health_Plan_Managed_Medicaid";
                    hide = hide.replace(",.Employer_Lives,.Employer_Formulary,.Health_Plan_Lives,.Health_Plan_Processor,.Health_Plan_Formulary,.Health_Plan_Commercial,.Health_Plan_Medicare_Part_D,.Health_Plan_Managed_Medicaid", "");
                    break;
                case 6: //Managed Medicaid                   
                    show += ",.Managed_Medicaid_Medical_Lives,.Managed_Medicaid_Lives";                 
                    hide = hide.replace(",.Managed_Medicaid_Medical_Lives,.Managed_Medicaid_Lives", "");
                    break;
                case 9: //state medicaid
                    show = show.replace(",.Total_Pharmacy", "");
                    show += ",.Medicaid_Enrollment,.Medicaid_Mcare_Enrollment,.FFS_Lives";
                    hide += ",.Total_Pharmacy";
                    hide = hide.replace(",.Medicaid_Enrollment,.Medicaid_Mcare_Enrollment,.FFS_Lives", "");
                    break;
                case 11://VA
                    show = show.replace(",.Total_Pharmacy", "");
                    hide += ",.Total_Pharmacy";
                    break;
                case 12://DoD
                    show = show.replace(",.Total_Pharmacy", "");
                    hide += ",.Total_Pharmacy";
                    break;
                case 17: // Medicare Part D
                    show += ",.Medicare_PartD_Lives,.MAPD_Lives,.PDP_Lives,.MAPD_LIS_Lives,.PDP_LIS_Lives";
                    hide = hide.replace(",.Medicare_PartD_Lives,.MAPD_Lives,.PDP_Lives,.MAPD_LIS_Lives,.PDP_LIS_Lives", "");
                    break;
            } //end of switch
            channelcnt = 1;
        }

        //---------------------------------------------------------------------
        if (isAllSelected)
        {
            var byAll = isAllSelected ? "ByAll" : "";

            if (byAll)
            {
                var gridLivesDist = $find("ctl00_Tile3_gridLivesDistribution_gridLDReport");
                //If dataset is different then only refresh the grid.
                if (gridLivesDist.ClientSettings.DataBinding.DataService.TableName != String.format("LivesDistribution{0}Set", byAll))
                {
                    gridLivesDist.ClientSettings.DataBinding.DataService.TableName = String.format("LivesDistribution{0}Set", byAll);
                    gridLivesDist.get_masterTableView().rebind();
                }
            }
        }
        else
        {
            var gridLivesDist = $find("ctl00_Tile3_gridLivesDistribution_gridLDReport");
            //If dataset is different then only refresh the grid.
            if (gridLivesDist.ClientSettings.DataBinding.DataService.TableName != String.format("LivesDistribution{0}Set", byAll))
            {
                gridLivesDist.ClientSettings.DataBinding.DataService.TableName = String.format("LivesDistribution{0}Set", byAll);
                gridLivesDist.get_masterTableView().rebind();
            }
        }
        //---------------------------------------------------------------------

        var dynamicPercent = 100;
        var visible = show.split(",").length;
        var hiddencols = hide.split(",").length;

        $(show).removeClass("StdRepHideShowCols");
        $(hide).addClass("StdRepHideShowCols");

        if (channelcnt > 1)
        {
            $(show).css('width', "150px")
            
        }
        else
        {
           
            $('.rgMasterTable').css('width', "100%");
            $(show).width(dynamicPercent / visible + "%");

        }

        //----If All is selected-----------------------
        if ((isAllSelected) || (channelcnt >= 4))
        {
            $(show).css('width', "100px")
            $('.Plan_Name').css('width', "200px");
        }
    
    }

</script>