using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using PathfinderClientModel;
using Pinsonault.Web;
using Pinsonault.Application.Reckitt;

public partial class custom_reckitt_businessplanning_all_GoalTactics : PageBase
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        bool isValidUser = false;
        int iPlanID = Convert.ToInt32(Request.QueryString["Plan_ID"]);
        using (PathfinderModel.PathfinderEntities clientContext = new PathfinderModel.PathfinderEntities())//(Pinsonault.Web.Session.ClientConnectionString))
        {
            isValidUser = clientContext.CheckUserAlignment(iPlanID, Pinsonault.Web.Session.UserID);
        }
        divGoalButtons.Visible = isValidUser;
        divTacticButton.Visible = isValidUser;
        if (rgGoals.SelectedIndexes.Count > 0)
        {
            btnEditGoal.Visible = true;
            btnDeleteGoal.Visible = true;
        }
        if (rgTactics.SelectedIndexes.Count > 0)
        {
            btnEditTactics.Visible = true;
            btnDeleteTactic.Visible = true;
        }
    }
   
    protected override void OnPreRenderComplete(EventArgs e)
    {
        int goalID = 0;
        int tacticID = 0;
        if ((int.TryParse(hdnGoal_ID.Value, out goalID)) && goalID > 0)
        {
            if (rgGoals.MasterTableView.FindItemByKeyValue("Goal_ID", goalID) != null)
                rgGoals.MasterTableView.FindItemByKeyValue("Goal_ID", goalID).Selected = true;
        }
        if ((int.TryParse(hdnTactic_ID.Value, out tacticID)) && tacticID > 0)
        {
            if (rgTactics.MasterTableView.FindItemByKeyValue("Tactic_ID", tacticID) != null)
                rgTactics.MasterTableView.FindItemByKeyValue("Tactic_ID", tacticID).Selected = true;
        }

        base.OnPreRenderComplete(e);
    }
   
    # region"Goal"
    protected void rgGoals_SelectedIndexChanged(object sender, EventArgs e)  
     {        
         formGoals.Visible = false;
         formTactics.Visible = false;
     }  

    /// <summary>
    /// changes the form view in insert mode
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddGoal_Click(object sender, EventArgs e)
    {
        formGoals.Visible = true;
        litGoal.Text = "Add Goal";
        frmSectedGoal.ChangeMode(FormViewMode.Insert);
    }
    /// <summary>
    /// changes the formview in edit mode
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnEditGoal_Click(object sender, EventArgs e)
    {
        if (rgGoals.SelectedIndexes.Count > 0)
        {
            formGoals.Visible = true;
            litGoal.Text = "Edit Goal";
            frmSectedGoal.ChangeMode(FormViewMode.Edit);
        }
        
    }    
   
    /// <summary>
    /// Updates the record in Business_Plan_Goals table
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void UpdateGoal(object sender, EntityDataSourceChangingEventArgs e)
    {
        int iStatusID = Convert.ToInt32(((RadioButtonList)(frmSectedGoal.FindControl("Goal_Status"))).SelectedValue);
        ((Pinsonault.Application.Reckitt.BusinessPlanGoal)e.Entity).Status = ((PathfinderReckittEntities)e.Context).BusinessPlanStatusSet.FirstOrDefault(s => s.Status_ID == iStatusID);
        formGoals.Visible = false;
    }
    /// <summary>
    /// Adds a goal
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void AddGoal(object sender, EntityDataSourceChangingEventArgs e)
    {
        int iBPID = Convert.ToInt32(Request.QueryString["Business_Plan_ID"]);
        int iStatusID = Convert.ToInt32(((RadioButtonList)(frmSectedGoal.FindControl("Goal_Status"))).SelectedValue);
        //add a goal with FK BPID and status_id
        using (PathfinderReckittEntities context = new PathfinderReckittEntities())
        {
            ((Pinsonault.Application.Reckitt.BusinessPlanGoal)e.Entity).BusinessPlan = ((Pinsonault.Application.Reckitt.PathfinderReckittEntities)e.Context).BusinessPlanSet.FirstOrDefault(s => s.Business_Plan_ID == iBPID);
            ((Pinsonault.Application.Reckitt.BusinessPlanGoal)e.Entity).Status = ((Pinsonault.Application.Reckitt.PathfinderReckittEntities)e.Context).BusinessPlanStatusSet.FirstOrDefault(s => s.Status_ID == iStatusID);                        
        }
        formGoals.Visible = false;
    }
    #endregion

    #region "Tactics"
    protected void rgTactics_SelectedIndexChanged(object sender, EventArgs e)
    {      
        formGoals.Visible = false;
        formTactics.Visible = false;
    } 

    /// <summary>
    /// changes the form view in insert mode
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddTactics_Click(object sender, EventArgs e)
    {
        if (rgGoals.SelectedIndexes.Count > 0)
        {
            formTactics.Visible = true;
            litTactic.Text = "Add Tactic";
            frmTactics.ChangeMode(FormViewMode.Insert);
        }
    }
    /// <summary>
    /// changes the form view in Edit mode
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnEditTactics_Click(object sender, EventArgs e)
    {
        if (rgTactics.SelectedIndexes.Count > 0)
        {                        
            formTactics.Visible = true;
            litTactic.Text = "Edit Tactic";
            frmTactics.ChangeMode(FormViewMode.Edit);
        }
    }
    
    /// <summary>
    /// Adds a Tactic
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void AddTactic(object sender, EntityDataSourceChangingEventArgs e)
    {
        int iGoalID = Convert.ToInt32(hdnGoal_ID.Value);
        int iStatusID = Convert.ToInt32(((RadioButtonList)(frmTactics.FindControl("Tactic_Status"))).SelectedValue);
        //add a tactic with FK BPID and status_id
        using (PathfinderReckittEntities context = new PathfinderReckittEntities())
        {
            ((Pinsonault.Application.Reckitt.BusinessPlanTactic)e.Entity).BusinessPlanGoal = ((Pinsonault.Application.Reckitt.PathfinderReckittEntities)e.Context).BusinessPlanGoalSet.FirstOrDefault(s => s.Goal_ID == iGoalID);
            ((Pinsonault.Application.Reckitt.BusinessPlanTactic)e.Entity).Status = ((Pinsonault.Application.Reckitt.PathfinderReckittEntities)e.Context).BusinessPlanStatusSet.FirstOrDefault(s => s.Status_ID == iStatusID);
        }
        formTactics.Visible = false;
    }
    /// <summary>
    /// Updates a Tactic
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void UpdateTactic(object sender, EntityDataSourceChangingEventArgs e)
    {
        int iStatusID = Convert.ToInt32(((RadioButtonList)(frmTactics.FindControl("Tactic_Status"))).SelectedValue);
        ((Pinsonault.Application.Reckitt.BusinessPlanTactic)e.Entity).Status = ((Pinsonault.Application.Reckitt.PathfinderReckittEntities)e.Context).BusinessPlanStatusSet.FirstOrDefault(s => s.Status_ID == iStatusID);                        
        formTactics.Visible = false;
    }
    #endregion
    
}
