using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
//using Impact;

public partial class controls_subtab : System.Web.UI.UserControl
{
    //enum derived from Lkp_Status table for Campaign Status
    public enum CampaignStatus
    {
        Pending = 1,
        Approved = 2,
        Completed = 3,
        Cancelled = 4
    }
    /// <summary>
    /// This enum corresponds to table Lkp_Phase. It's values correspond to Phase_ID from Lkp_Phase table.
    /// </summary>
    public enum PhaseID
    {
        Campaign_Created = 1,
        Profile = 2,
        Timeline = 3,
        Goal_Setup = 4,
        Team_Setup = 5,
        Tactics = 6,
        Messages = 7,
        Approval = 8,
        Feedback = 9,
        All_Steps_Completed = 10
    }


    public string PreviousPhaseName { get; protected set; }
    public int PreviousPhaseID { get; protected set; }
    public string CurrentPhaseName { get; protected set; }
    public int CurrentPhaseID { get; protected set; }
    public int RequestedPhaseID { get; protected set; }
    public int PreviousStep { get; protected set; }
    public int CurrentStep { get; protected set; }
    public int StatusID { get; protected set; }
    public string CTerritoryID { get; protected set; }

    /// <summary>
    /// If true it means the page the user is requesting is not valid for the selected campaign because they have not proceeded far enough in the process to be viewing the requested step.
    /// </summary>
    public bool InvalidStep { get; protected set; }

    /// <summary>
    /// If true it means the page the user is requesting is not valid for the selected campaign because they have not proceeded far enough in the process to be viewing the requested phase.
    /// </summary>
    public bool InvalidPhase { get; protected set; }

    /// <summary>
    /// The submit button will be displayed on the page based on ShowSubmit value
    /// </summary>
    public bool ShowSubmit { get; protected set; }

    /// <summary>
    /// The edit button can be displayed on the page
    /// </summary>
    public bool ShowEdit { get; protected set; }
    

    protected override void OnLoad(EventArgs e)
    {
        string pageName = Path.GetFileName(this.Request.PhysicalPath);
               
        tabCampaigns.Visible = pageName.StartsWith("campaigns", StringComparison.InvariantCultureIgnoreCase);
        if ( tabCampaigns.Visible )
        {
            c1.Attributes["class"] = pageName.StartsWith("campaigns_current", StringComparison.InvariantCultureIgnoreCase) ? "selected" : "default";
            c2.Attributes["class"] = pageName.StartsWith("campaigns_archived", StringComparison.InvariantCultureIgnoreCase) ? "selected" : "default";
            
        }

        tabMyCampaigns.Visible = pageName.StartsWith("mycampaigns", StringComparison.InvariantCultureIgnoreCase);
        if ( tabMyCampaigns.Visible)
        {
            mc1.Attributes["class"] = pageName.StartsWith("mycampaigns_current", StringComparison.InvariantCultureIgnoreCase) ? "selected" : "default";
            mc2.Attributes["class"] = pageName.StartsWith("mycampaigns_opportunities", StringComparison.InvariantCultureIgnoreCase) ? "selected" : "default";
        }

        tabSteps.Visible = pageName.StartsWith("createcampaign", StringComparison.InvariantCultureIgnoreCase);
        if ( tabSteps.Visible )
        {            
            statusBox.Visible = true;
        }

        base.OnLoad(e);
    }

    public void GetCampaignStatus(string id, string pageName)
    {
        int step = pageName.StartsWith("createcampaign_step1", StringComparison.InvariantCultureIgnoreCase) ? 1 :
                    pageName.StartsWith("createcampaign_step2", StringComparison.InvariantCultureIgnoreCase) ? 2 :
                    pageName.StartsWith("createcampaign_step3", StringComparison.InvariantCultureIgnoreCase) ? 3 :
                    pageName.StartsWith("createcampaign_step4", StringComparison.InvariantCultureIgnoreCase) ? 4 : 0;

        step0.Attributes["class"] = string.Format("step0 {0}", step == 0 ? "selected" : "default");
        step1.Attributes["class"] = string.Format("step1 {0}", step == 1 ? "selected" : "default");
        step2.Attributes["class"] = string.Format("step2 {0}", step == 2 ? "selected" : "default");
        step3.Attributes["class"] = string.Format("step3 {0}", step == 3 ? "selected" : "default");
        step4.Attributes["class"] = string.Format("step4 {0}", step == 4 ? "selected" : "default");


        LoadStatusBox();

        InvalidStep = (CurrentStep < step);

        bool bIsPageApproval = false;

        int iRequestedPhaseID = (int)PhaseID.Campaign_Created;

        switch (step)
        {
            case 1:
                step1Tabs.Visible = true;
                if (pageName.StartsWith("createcampaign_step1_profile", StringComparison.InvariantCultureIgnoreCase))
                {
                    s1Profile.Attributes["class"] = "selected";
                    iRequestedPhaseID = (int)PhaseID.Profile;
                }
                else if (pageName.StartsWith("createcampaign_step1_timeline", StringComparison.InvariantCultureIgnoreCase))
                {
                    s1Timeline.Attributes["class"] = "selected";
                    iRequestedPhaseID = (int)PhaseID.Timeline;
                }
                else if (pageName.StartsWith("createcampaign_step1_goals", StringComparison.InvariantCultureIgnoreCase))
                {
                    s1Goals.Attributes["class"] = "selected";
                    iRequestedPhaseID = (int)PhaseID.Goal_Setup;
                }
                else if (pageName.StartsWith("createcampaign_step1_team", StringComparison.InvariantCultureIgnoreCase))
                {
                    s1Team.Attributes["class"] = "selected";
                    iRequestedPhaseID = (int)PhaseID.Team_Setup;
                }
                break;

            case 2:
                step2Tabs.Visible = true;

                if (pageName.StartsWith("createcampaign_step2_summary", StringComparison.InvariantCultureIgnoreCase))
                {
                    s2Summary.Attributes["class"] = "selected";
                    //iRequestedPhaseID = (int)PhaseID.?                       
                }
                else if (pageName.StartsWith("createcampaign_step2_tactics", StringComparison.InvariantCultureIgnoreCase))
                {
                    s2Tactics.Attributes["class"] = "selected";
                    iRequestedPhaseID = (int)PhaseID.Tactics;
                }

                //uncomment if messages tab is required 
                else if (pageName.StartsWith("createcampaign_step2_messages", StringComparison.InvariantCultureIgnoreCase))
                {
                    s2Messages.Attributes["class"] = "selected";
                    iRequestedPhaseID = (int)PhaseID.Messages;
                }

                break;

            case 3:
                step3Tabs.Visible = true;

                if (pageName.StartsWith("createcampaign_step3_approval", StringComparison.InvariantCultureIgnoreCase))
                {
                    s3Approval.Attributes["class"] = "selected";
                    iRequestedPhaseID = (int)PhaseID.Approval;
                    bIsPageApproval = true;
                }
                
               
                break;

            case 4:
                step4Tabs.Visible = true;
                if (pageName.StartsWith("createcampaign_step4_results", StringComparison.InvariantCultureIgnoreCase))
                {
                    s4Results.Attributes["class"] = "selected";
                    iRequestedPhaseID = CurrentPhaseID >= (int)PhaseID.Approval ? 0 : Int32.MaxValue; // (int)PhaseID.Approval; //Just has to be approved to see results (int)PhaseID.Feedback;
                }
                else if (pageName.StartsWith("createcampaign_step4_bestpractices", StringComparison.InvariantCultureIgnoreCase))
                {
                    s4BestPractices.Attributes["class"] = "selected";
                    iRequestedPhaseID = StatusID == (int)CampaignStatus.Completed ? (int)PhaseID.Feedback : int.MaxValue; //Must be completed be valid
                }
                break;

        }

        //
        ValidateSelectedPhase(CurrentPhaseID, iRequestedPhaseID);
        
        //set property so it can be used by master page
        RequestedPhaseID = iRequestedPhaseID;

        // sl
        if (iRequestedPhaseID == (int)PhaseID.Tactics || iRequestedPhaseID == (int)PhaseID.Messages)
        {
            GetCampaignShowSubmit_TacticsMessages(bIsPageApproval, iRequestedPhaseID);

        }   // sl end

        else
        {

            GetCampaignShowSubmit(bIsPageApproval, iRequestedPhaseID);
        }     
    }
 
    /// <summary>
    /// Sets properties on the control related to the current campaign's Step, Phase, and Territory
    /// </summary>
    public void LoadStatusBox()
    {
        string id = Request.QueryString["id"];
        if ( !string.IsNullOrEmpty(id) )
        {
            int cid = 0;
            if ( Int32.TryParse(id, out cid) )
            {
                using ( SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString))
                { 
                    using (SqlCommand cmd = new SqlCommand("pprx.usp_Get_Campaign_Status", cn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", cid);
                        cn.Open();

                        using ( SqlDataReader rdr = cmd.ExecuteReader() )
                        {
                            if ( rdr.Read() )
                            {
                                PreviousPhaseID = rdr.GetInt32(rdr.GetOrdinal("Previous_Phase_ID"));
                                PreviousPhaseName = rdr.GetString(rdr.GetOrdinal("Previous_Phase_Name"));
                                CurrentPhaseID = rdr.GetInt32(rdr.GetOrdinal("Current_Phase_ID"));
                                CurrentPhaseName = rdr.GetString(rdr.GetOrdinal("Current_Phase_Name"));
                                PreviousStep = rdr.GetInt32(rdr.GetOrdinal("Previous_Step"));
                                CurrentStep = rdr.GetInt32(rdr.GetOrdinal("Current_Step"));
                                StatusID = rdr.GetInt32(rdr.GetOrdinal("Status_ID"));
                                CTerritoryID = rdr.GetString(rdr.GetOrdinal("Territory_ID"));
                            }
                        }
                    }
                }
            }
        }

    }

    /// <summary>
    /// Checks if current campaign phase can be viewed user.  Selected phase must be less than or equal to Campaign phase otherwise it is invalid.
    /// </summary>
    /// <param name="CurrentPhase">Phase ID of the campaign</param>
    /// <param name="RequestedPhase">Phase ID of the page being loaded.</param>
    void ValidateSelectedPhase(int CurrentPhase, int RequestedPhase)
    {
        InvalidPhase = (CurrentPhase < RequestedPhase);
       
    }

    /// <summary>
    /// sl: This function applies to Tactics & Messages only
    /// 
    /// </summary>
    /// <param name="bIsPageApproval"></param>
    void GetCampaignShowSubmit_TacticsMessages(bool bIsPageApproval, int iRequestedPhaseID)
    {
        if (!InvalidStep && !InvalidPhase && ((StatusID == (int)CampaignStatus.Pending)) || (StatusID == (int)CampaignStatus.Approved))
        {
            if (HttpContext.Current.User.IsInRole("pprx_CVA") && CTerritoryID == Session["territoryID"].ToString())
            {
                ShowSubmit = false;
                ShowEdit = true;
                if (PreviousPhaseID + 1 == iRequestedPhaseID)
                {
                    ShowSubmit = true;
                    ShowEdit = false;
                }
            }
        }
    }


    /// <summary>
    /// This functions sets the show submit status for Campaign creation pages. 
    /// It checks for user rights and campaign status and accordingly sets the ShowSubmit value for the page
    /// </summary>
    /// <param name="bIsPageApproval"></param>
    void GetCampaignShowSubmit(bool bIsPageApproval, int iRequestedPhaseID)
    {           
        //show edit if valid step and phase - campaign must also have status of Pending or Completed when looking at Feedback
        if (!InvalidStep && !InvalidPhase && ((StatusID == (int)CampaignStatus.Pending) || (iRequestedPhaseID == (int)PhaseID.Feedback && StatusID == (int)CampaignStatus.Completed)))
        {
            if ((HttpContext.Current.User.IsInRole("pprx_CVA") && (CTerritoryID == Session["territoryID"].ToString())) || (bIsPageApproval))
            {
                ShowSubmit = false;
                ShowEdit = true;
                if (PreviousPhaseID + 1 == iRequestedPhaseID)
                {
                    ShowSubmit = true;
                    ShowEdit = false;
                }
            }
        }
    }
    
}
