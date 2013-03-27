using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class controls_CommBenefitDesign : System.Web.UI.UserControl
{
    public string ContainerID { get; set; }
    public bool AllowSorting { get; set; }
    public string OnClientRowSelected { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(this.Page, gridCommBenefitDesg.ClientID, "{}", ContainerID);
        //Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "_commbdpagevars", string.Format("var gridCommBenefitDesignID = '{0}';", gridCommBenefitDesg.ClientID), true);


        gridCommBenefitDesg.AllowSorting = AllowSorting;
        gridCommBenefitDesg.MasterTableView.AllowSorting = AllowSorting;

        //For Channel = Commercial, based on selected plan's Segment_ID, hide or show Commercial Grid.
        //SegmentID = 1(Show only Commercial Grid)
        //SegmentID = 2(Show only Medicare Part D grid)
        //SegmentID = 3(Show both grids, Commercial and Medicare Part D)

        // sl 3/22/2012 : Business Rule changed
        // based on Plan_Mast (3 additional fields: Has_Commercial_Business, Has_Medicare_partD_Business, Has_Managed_Medicaid_Business)
        // show/hide Grid


        bool hasComm = false;
        bool recExists = false;
        string id = Request.QueryString["Plan_ID"];
        int planID = 0;
        if (int.TryParse(id, out planID))
        {
            using (PathfinderModel.PathfinderEntities context = new PathfinderModel.PathfinderEntities())
            {
                //segmentID = context.PlanMasterSet.Where(p => p.Plan_ID == planID).Select(p=>p.Segment_ID).FirstOrDefault();                

                var p = (from q in context.PlanMasterSet
                         where q.Plan_ID == planID
                         select q.Has_Commercial_Business).FirstOrDefault();
                if (p == true)
                    hasComm = true;

                // to check if record exists
                var d = (from r in context.BenefitDesignCommercialSet
                         where r.Plan_ID == planID
                         select r);
                if (d.Count() > 0)
                {
                    recExists = true;
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "_commbdpagevars", string.Format("var CommRecExists = true;"), true);


                }
                else
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "_commbdpagevars", string.Format("var CommRecExists = false;"), true);


            }
        }

        gridCommBenefitDesg.AllowSorting = AllowSorting;
        gridCommBenefitDesg.MasterTableView.AllowSorting = AllowSorting;

        if (hasComm)
        {
            BDHeader1.Visible = true;
            gridCommBenefitDesg.Visible = true;
        }
        else
        {
            BDHeader1.Visible = false;
            gridCommBenefitDesg.Visible = false;

        }


        //string SegID = null;
        //HiddenField segment = SegIDCommCovLives.FindControl("Seg_ID") as HiddenField;
        //if ( segment != null )
        //    SegID = segment.Value;

        //if (SegID == "2")
        //{
        //    BDHeader1.Visible = false;
        //    gridCommBenefitDesg.Visible = false;
        //}
        //else
        //{
        //    BDHeader1.Visible = true;
        //    gridCommBenefitDesg.Visible = true;
        //}


        if (!string.IsNullOrEmpty(OnClientRowSelected))
        {
            //uncomment following code
            //If formulary is enabled for the user then show drilldown grid else hide it.
            if (Context.User.IsInRole("frmly_1"))
            {
                gridCommBenefitDesg.ClientSettings.Selecting.AllowRowSelect = true;
                gridCommBenefitDesg.ClientSettings.ClientEvents.OnRowSelected = OnClientRowSelected;
            }
        }

        //formularyDate.Visible = gridCommBenefitDesg.Visible;
    }
}
