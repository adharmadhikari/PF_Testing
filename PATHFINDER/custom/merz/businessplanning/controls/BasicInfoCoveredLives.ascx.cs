using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class custom_merz_businessplanning_controls_BasicInfoCoveredLives : System.Web.UI.UserControl
{
    public string ContainerID { get; set; }

    public bool Export { get; set; }

    protected override void OnLoad(EventArgs e)
    {
        PlanCL1.ContainerID = ContainerID;
        CLlivesMC.ContainerID = ContainerID;

        if (Export)
        {
            BasicInfoDiv.Attributes["class"] = "leftBPTile divborder leftSmPDFTile";
            CLMedicareDiv.Attributes["class"] = "leftBPTile divborder leftSmPDFTile";
            CLCommDiv.Attributes["class"] = "leftBPTile divborder leftSmPDFTile";
        }
        else
        {
            BasicInfoDiv.Attributes["class"] = "leftBPTile divborder leftSmTile";
            CLMedicareDiv.Attributes["class"] = "leftBPTile divborder leftSmTile";
            CLCommDiv.Attributes["class"] = "leftBPTile divborder leftSmTile";
        }

        base.OnLoad(e);
    }
}
