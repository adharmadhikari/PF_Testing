using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderModel;
using PathfinderClientModel;
using Pinsonault.Web;

public partial class standardreports_controls_FilterReportType : System.Web.UI.UserControl
{
    public string ContainerID { get; set; }
    
    public standardreports_controls_FilterReportType()
    {
        ContainerID = "moduleOptionsContainer";
    }

    protected override void OnLoad(EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Rank.ClientID, null, ContainerID);       

        using (PathfinderEntities context = new PathfinderEntities())
        {
            //Check if Rank Types are available by client
            IList<RankTypes> q = context.GetReportRankTypes(Pinsonault.Web.Session.ClientID);

            if (q.Count() == 0) //If not, get generic Rank Types
                q = context.GetReportRankTypes(null);

            Rank.DataSource = q;
            Rank.DataBind();
        }

        using (PathfinderClientEntities clientContext = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
        {
            int channel = Convert.ToInt32(Request.QueryString["channel"]);

            var q = (from p in clientContext.PlanSearchSet
                     where p.Section_ID == channel
                     orderby p.Name
                     select p).ToList().Select(p => new GenericListItem { ID = p.ID.ToString(), Name = p.Name });
            if (q != null)
            {               
                Pinsonault.Web.Support.RegisterGenericListVariable(this.Page, q.ToArray(), "allPlans");
            }
        }
        
        string frag = "rankIndexChanged(s, a)";
        Rank.OnClientSelectedIndexChanged = "function(s,a){" + frag + "}";
        Rank.OnClientLoad = "function(s,a){var data = clientManager.get_SelectionData();}";      

        base.OnLoad(e);
    }

}
