using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderClientModel;
using Pinsonault.Web;
using System.IO;

public partial class custom_reckitt_businessplanning_all_businessplanning_pdf : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int SegmentID = Convert.ToInt32(Request.QueryString["Segment_ID"]);
        
        if (SegmentID == 17)
        {
            coveredLives.ShowTotalCoveredLives = false;
            coveredLives.ShowPharmLives = false;
            coveredLives.CoveredLivesEntitySet = "CoveredLivesMedDSet";
        }
        else if (SegmentID == 9)
        {
            pnlStateMedicaid.Visible = true;
            pnlCoveredLives.Visible = false;
        }
        lblPlanName.Text = Request.QueryString["Plan_Name"].ToString();
    }
}
