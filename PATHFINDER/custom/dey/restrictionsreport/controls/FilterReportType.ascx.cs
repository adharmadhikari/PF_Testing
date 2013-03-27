using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderModel;
using PathfinderClientModel;
using Pinsonault.Web;

public partial class restrictionsreport_controls_FilterReportType : System.Web.UI.UserControl
{
    public string ContainerID { get; set; }

    public restrictionsreport_controls_FilterReportType()
    {

        ContainerID = "moduleOptionsContainer";

    }

    protected override void OnLoad(EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Rank.ClientID, null, ContainerID);
       // Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Plan_ID.ClientID, null, ContainerID);

        using (PathfinderEntities context = new PathfinderEntities())
        {
            //Check if Rank Types are available by client
            IList<RankTypes> q = context.GetReportRankTypes(Pinsonault.Web.Session.ClientID);

            if (q.Count() == 0) //If not, get generic Rank Types
            {
                q = context.GetReportRankTypes(null);
            }
            //remove Top 10 and Top 20
            var qFilter = q.Where(item => item.Rank_Value != 10 && item.Rank_Value != 20);
            Rank.DataSource = qFilter;
            Rank.DataBind();
        }

        //using (PathfinderClientEntities clientContext = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
        //{
        //    int channel = Convert.ToInt32(Request.QueryString["channel"]);

        //    var q = (from p in clientContext.PlanSearchSet
        //             where p.Section_ID == channel
        //             orderby p.Name
        //             select p).ToList().Select(p => new GenericListItem { ID = p.ID.ToString(), Name = p.Name });
        //    if (q != null)
        //    {
        //        Pinsonault.Web.Support.RegisterGenericListVariable(this.Page, q.ToArray(), "allPlans");
        //    }
        //}

        string frag = "rankIndexChanged(s, a)";
        Rank.OnClientSelectedIndexChanged = "function(s,a){" + frag + "}";
        Rank.OnClientLoad = "function(s,a){var data = clientManager.get_SelectionData();}";

        base.OnLoad(e);
    }


    //protected void Page_Load(object sender, EventArgs e)
    //{

    //}
}
