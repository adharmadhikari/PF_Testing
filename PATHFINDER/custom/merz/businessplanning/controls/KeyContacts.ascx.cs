using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderClientModel;
using Pathfinder;
using Pinsonault.Web;

public partial class custom_merz_businessplanning_controls_KeyContacts : System.Web.UI.UserControl
{
    
    protected override void OnInit(EventArgs e)
    {
       // AllKCEntity.ConnectionString = Pinsonault.Web.Session.ClientConnectionString;

    }

    protected override void OnLoad(EventArgs e)
    {
        Int32 Section_ID = Convert.ToInt32(HttpContext.Current.Request.QueryString["Section_ID"]);
        Int32 Plan_ID = Convert.ToInt32(HttpContext.Current.Request.QueryString["Plan_ID"]);

        using (PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
        {
            //GetKeyContactByPlanID: gets list of key contacts based on selected Section_ID and Plan_ID.
            GridviewKC.DataSource = context.GetKeyContactByPlanID(Section_ID, Plan_ID).ToList ();
            GridviewKC.DataBind();
        }

        base.OnLoad(e);
    }
}
