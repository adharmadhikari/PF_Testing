using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderClientModel;
using Pinsonault.Web;
using Pinsonault.Application.Reckitt;


public partial class custom_reckitt_businessplanning_all_RemoveGoal : PageBase 
{
    protected override void OnInit(EventArgs e)
    {
        string strText = Request.QueryString["linkremove"].ToString();
        titleText.Text = string.Format("Remove Selected {0}", strText);
        base.OnInit(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["linkremove"].ToString() == "goal")
            FormRemoveGoal.Visible = true;
        else if (Request.QueryString["linkremove"].ToString() == "tactic")
            FormRemoveTactic.Visible = true;

    }
    protected void Yesbtn_Click(object sender, EventArgs e)
    {

        int iGoalID = Convert.ToInt32(Request.QueryString["Goal_ID"]);
        using (PathfinderReckittEntities context = new PathfinderReckittEntities())
        {
            int iGoalCount = (from g in context.BusinessPlanGoalSet
                              where g.Goal_ID == iGoalID
                              select g).Count();

            if (iGoalCount > 0)
            {
                var gToDeleteQuery = from gDelete in context.BusinessPlanGoalSet
                                     where gDelete.Goal_ID == iGoalID
                                     select gDelete;
                foreach (var gToDelete in gToDeleteQuery.Select(d => d))
                {
                    context.DeleteObject(gToDelete);
                }
                context.SaveChanges();
                this.FormRemoveGoal.Visible = false;
                this.Msglbl.Text = "<div>Selected Goal has been removed successfully.</div>";
                this.Msglbl.Visible = true;
            }
        }
        //Calls Javascript function RefreshMySSList() to refresh sell sheet grids.
        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "RefreshGoalList", "RefreshGoalList();", true);
    }

    protected void YesTacticbtn(object sender, EventArgs e)
    {
        int iTacticID = Convert.ToInt32(Request.QueryString["Tactic_ID"]);
        using (PathfinderReckittEntities context = new PathfinderReckittEntities())
        {
            int iCount = (from t in context.BusinessPlanTacticSet
                          where t.Tactic_ID == iTacticID
                          select t).Count();

            if (iCount > 0)
            {
                var tToDeleteQuery = from tDelete in context.BusinessPlanTacticSet
                                     where tDelete.Tactic_ID == iTacticID
                                     select tDelete;
                foreach (var tToDelete in tToDeleteQuery.Select(d => d))
                {
                    context.DeleteObject(tToDelete);
                }
                context.SaveChanges();
            }
        }
        //Calls Javascript function RefreshMySSList() to refresh sell sheet grids.
        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "RefreshGoalList", "RefreshGoalList();", true);
    }
}
