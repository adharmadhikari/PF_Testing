using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderModel;


public partial class custom_warner_formularyhistoryreporting_controls_ReportScript : System.Web.UI.UserControl
{
    public string DrillDownGridUrl
    {
        get;
        set;
    }
    public string ShowPhysList
    {
        get
        {
            //IMPORTANT - this is not intended as true security and used just to disable click action - the same check is applied on actual page's grid control to block access if the user is not assigned module.             
            using (PathfinderEntities context = new PathfinderEntities())
            {
                return context.CheckUserModule(Pinsonault.Web.Session.UserID, 2, "prescribers").ToString().ToLower();
            }
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }
}
