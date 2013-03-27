using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class custom_merz_businessplanning_controls_MedicalPolicy : System.Web.UI.UserControl
{
    public int BusinessPlanID { get; set; }
    public bool AlignmentStatus { get; set; }

    public string ContainerID { get; set; }

    public bool Export { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        //If logged on user is aligned to selected plan then show "upload files" link. else hide it.
        if (AlignmentStatus == true)
        {
            A1.Visible = true;
            separator3.Visible = true;
            A2.Visible = true;
            separator31.Visible = true;
        }
        else
        {
            A1.Visible = false;
            separator3.Visible = false;
            A2.Visible = false;
            separator31.Visible = false;
        }

        RadGridbppOLICY.ClientSettings.DataBinding.Location = string.Format("~/custom/merz/businessplanning/services/MerzDataService.svc/BusinessPlansSet({0})", BusinessPlanID);
        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "_clddpagevars", string.Format("var gridBPMedPolicyID = '{0}';", RadGridbppOLICY.ClientID), true);

        radGridWrapper.ContainerID = ContainerID;

        if (Export == true)
        {
            MedPolicyDiv.Attributes["class"] = "rightBPTile divborder rightMedPDFTile";
            //MedPolicyHeader.Attributes["class"] = "leftBPTile rightBgTile";
            //MedPolicyPager.Attributes["class"] = "rightBPTile leftSmTile";  
        }
        else
        {
            MedPolicyDiv.Attributes["class"] = "rightMedTile rightBPTile divborder";
            //MedPolicyHeader.Attributes["class"] = "leftBPTile leftSmTile";
            //MedPolicyPager.Attributes["class"] = "rightBPTile rightBgTile";
        }
    }
}
