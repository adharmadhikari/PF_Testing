using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class controls_MMBenefitDesign : System.Web.UI.UserControl
{
    public string ContainerID { get; set; }
    public bool AllowSorting { get; set; }
    public string OnClientRowSelected { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {

        Pinsonault.Web.Support.RegisterComponentWithClientManager(this.Page, gridMMBenefitDesg.ClientID, "{}", ContainerID);
        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "_mmbdpagevars", string.Format("var gridMMBenefitDesignID = '{0}'; var gridMedDBenefitDesignID = {1}", gridMMBenefitDesg.ClientID, null), true);

        // based on Plan_Mast (3 additional fields: Has_Commercial_Business, Has_Medicare_partD_Business, Has_Managed_Medicaid_Business)
        // show/hide Grid

        bool hasMM = false;
        
        string id = Request.QueryString["Plan_ID"];
        int planID = 0;
        if (int.TryParse(id, out planID))
        {
            using (PathfinderModel.PathfinderEntities context = new PathfinderModel.PathfinderEntities())
            {
            
                var p = (from q in context.PlanMasterSet
                         where q.Plan_ID == planID
                         select q.Has_Managed_Medicaid_Business).FirstOrDefault();
                if (p == true)
                    hasMM = true;
                
            }
        }

        if (hasMM)
        {
            BDHeader3.Visible = true;
            gridMMBenefitDesg.Visible = true;
        }
        else
        {

            BDHeader3.Visible = false;
            gridMMBenefitDesg.Visible = false;

        }

        gridMMBenefitDesg.AllowSorting = AllowSorting;
        gridMMBenefitDesg.MasterTableView.AllowSorting = AllowSorting;



        if (!string.IsNullOrEmpty(OnClientRowSelected))
        {
            //uncomment following code
            //If formulary is enabled for the user then show drilldown grid else hide it.
            if (Context.User.IsInRole("frmly_6"))
            {
                gridMMBenefitDesg.ClientSettings.Selecting.AllowRowSelect = true;
                gridMMBenefitDesg.ClientSettings.ClientEvents.OnRowSelected = OnClientRowSelected;
            }
        }

        //formularyDate.Visible = gridCommBenefitDesg.Visible;
    }
}
