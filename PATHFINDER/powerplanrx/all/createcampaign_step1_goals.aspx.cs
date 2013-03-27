using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
/* using Pinsonault.Data; */
using System.Data;
using System.Text;
using Telerik.Web.UI;
using Pinsonault.Application.PowerPlanRx;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;

public partial class createcampaign_step1_goals : System.Web.UI.Page, IEditPage
{
    public bool _isEdit = false;
    protected override void OnLoad(EventArgs e)
    {
        
        //goals.AllowEdit = ((MasterPage)Master).PhaseID > 3;
        //goals.ShowSummary = goals.AllowEdit;
        //goals.dbEditMode = ((MasterPage)Master).PhaseID > 3;   // Goals already saved

        if (string.IsNullOrEmpty(Request.QueryString["id"]))
        {
            Response.Redirect(Request.ServerVariables["HTTP_REFERER"]);
        }

        using (SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString))
        {
            cn.Open();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = cn;
                cmd.CommandText = "select Current_Step_ID from pprx.Campaign_Mast where Campaign_ID=" + Convert.ToInt32(Request.QueryString["id"]);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                        _isEdit = (dr.GetInt32(0) == 3);

                }
            }
        }
 
        if (!Page.IsPostBack)
        {
            goals.IsEdit = _isEdit;  //If Goals have not been submitted once then force edit mode
                            
            if (Convert.ToBoolean(Request.QueryString["qEdit"]))
            {
                if (divSubmitBtns.Visible == true)
                {
                    divEditBtns.Visible = true;
                    divSubmitBtns.Visible = false;
                }
                else
                {
                    divEditBtns.Visible = false;
                    divSubmitBtns.Visible = true;
                }


            }
            else  // 10/27/2012  
            {
                if (_isEdit)  // current step id: 3 (goto Edit mode directly)
                {
                    divEditBtns.Visible = false;
                    divSubmitBtns.Visible = true;
                }
            }
        }
        else
            goals.IsEdit = true;

        //goals.SelectedDistricts = SelectedDistricts;
        base.OnLoad(e);
    }

    protected void jsCall(string alert)
    {
        string script = @"alert('" + alert + "');";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "jsCall", script, true);
    }


    protected void btn_submit_Click(object sender, EventArgs e)
    {
        int baseMonth = 0;
        int baseYear = 0;
        string baseYearMonth = string.Empty;

        int duration = 0;
        int campaignID = Convert.ToInt32(Request.QueryString["id"]);
        string planName = string.Empty;
        DateTime start = DateTime.MinValue;
        int userID = Pinsonault.Web.Session.UserID;
        Goals.getCampaignBaselineYearMonth(campaignID, out baseMonth, out baseYear, out baseYearMonth, out duration);
        Goals.getCampaignTimeline(campaignID, out start, out duration, out planName);
        ///////////////////////////////////////////////
        //TODO: validation
        //TextBox _txtValidator = (TextBox)goals.TextValidator;
        
        HiddenField _hValidator = (HiddenField)goals.HiddenValidator;
        decimal _hValue = Convert.ToDecimal(_hValidator.Value);
        //if (_hValue != 0)
        //{
            
        //}

        //TODO: save goal %
        string pValues = string.Empty;

        //goal1 = Convert.ToDecimal(Request.Form["goals.GoalPercent1"]);
        HtmlTable _table = (HtmlTable)goals.TableGoalPercent;
        for (int s = 1; s <= duration; s++)
        {
            string _percentMonthID = "percentGrowth_month" + s;
            TextBox _txtGoal = (TextBox)_table.FindControl(_percentMonthID);
            pValues = pValues + _txtGoal.Text + ",";
        }

        // Goal Percent: hide months
        int toHideMonth_Begin = Convert.ToInt32(duration + 1);
        for (int x = toHideMonth_Begin; x <= 12; x++)   // max: 12 month
        {
            pValues = pValues + "0,";
        }
        string[] _p = pValues.Split(',');

        using (SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString))
        {
            cn.Open();
            using (SqlCommand cmd = new SqlCommand("pprx.usp_CampaignGoals_IncrementPercent_InsertUpdate", cn))
            {
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Campaign_ID", campaignID);
                cmd.Parameters.AddWithValue("@Goal1", Convert.ToDecimal(_p[0]));
                cmd.Parameters.AddWithValue("@Goal2", Convert.ToDecimal(_p[1]));
                cmd.Parameters.AddWithValue("@Goal3", Convert.ToDecimal(_p[2]));
                cmd.Parameters.AddWithValue("@Goal4", Convert.ToDecimal(_p[3]));
                cmd.Parameters.AddWithValue("@Goal5", Convert.ToDecimal(_p[4]));
                cmd.Parameters.AddWithValue("@Goal6", Convert.ToDecimal(_p[5]));
                cmd.Parameters.AddWithValue("@Goal7", Convert.ToDecimal(_p[6]));
                cmd.Parameters.AddWithValue("@Goal8", Convert.ToDecimal(_p[7]));
                cmd.Parameters.AddWithValue("@Goal9", Convert.ToDecimal(_p[8]));
                cmd.Parameters.AddWithValue("@Goal10", Convert.ToDecimal(_p[9]));
                cmd.Parameters.AddWithValue("@Goal11", Convert.ToDecimal(_p[10]));
                cmd.Parameters.AddWithValue("@Goal12", Convert.ToDecimal(_p[11]));
                cmd.Parameters.AddWithValue("@User_ID", userID);

                // cmd.Parameters.AddWithValue("@UserName", Impact.User.FullName);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

            }
        }

        ///////////////////// RadGrid ///////////////////



        DateTime dt;
        int dataMonth = 0;
        int dataYear = 0;


        //// Region
        int rowCount_R = goals.DistrictGrid.MasterTableView.Items.Count;
        for (int a = 0; a < rowCount_R; a++)
        {
            GridDataItem dataItemR = goals.DistrictGrid.MasterTableView.Items[a];
            Int32 geoLevelID = 1;  // Region   
            if (dataItemR != null && dataItemR is GridDataItem)
            {

                string regionID = dataItemR["Region_ID"].Text;
                if (regionID != "Non-Targeted")
                {

                    // string _brand = dataItemR["Brand_Name"].Text;  //GridTemplateColumn is not working

                    // Region: Baseline TRx, MST - runs only once
                    string controlID, controlID2;
                    Decimal _TRx = 0;
                    Decimal _MST = 0;
                    controlID = "R_TRx";
                    controlID2 = "R_MST";
                    Utility.get_cellValue_fromLabel(dataItemR, controlID, controlID2, ref _TRx, ref _MST);


                    // Region: Goal TRx, MST - runs based on duration
                    for (int x = 0; x <= duration; x++)
                    {

                        string R_controlID, R_controlID2;
                        Decimal R_TRx = 0;
                        Decimal R_MST = 0;


                        if (x == 0)  // to save Baseline TRx, MST in month0 (Campaign_Month_Indicator: 0 in Campaign_Goals
                        {
                            Utility.InsertUpdateProcess(campaignID, baseMonth, baseYear, regionID, 1, x, _TRx, _MST);
                            Utility.InsertBaselineProcess(campaignID, baseMonth, baseYear, regionID, 1, x, _TRx, _MST);

                        }
                        else
                        {
                            dt = start.AddMonths(x - 1);
                            dataYear = Convert.ToInt32(string.Format("{0:yyyy}", dt));
                            dataMonth = Convert.ToInt32(string.Format("{0:MM}", dt));

                            R_controlID = "R_TRx" + x;
                            R_controlID2 = "R_MST" + x;

                            Utility.get_cellValue_fromTextbox(dataItemR, R_controlID, R_controlID2, ref R_TRx, ref R_MST);

                            Utility.InsertUpdateProcess(campaignID, dataMonth, dataYear, regionID, 1, x, R_TRx, R_MST);
                        }
                    }
                }
            }


            /////// district
            if (goals.DistrictGrid.MasterTableView.Items[a].ChildItem.NestedTableViews[0].Name == "DistrictDetails")
            {
                // itemsD = goals.DistrictGrid.MasterTableView.Items[0].ChildItem.NestedTableViews[0].GetItems(GridItemType.Item);
                int rowCount_D = goals.DistrictGrid.MasterTableView.Items[a].ChildItem.NestedTableViews[0].Items.Count;

                //geoLevelID = 2;  // District 
                for (int b = 0; b < rowCount_D; b++)
                {
                    GridDataItem dataItemD = goals.DistrictGrid.MasterTableView.Items[a].ChildItem.NestedTableViews[0].Items[b];                // itemsR[i] as GridDataItem;
                    string DistrictID = dataItemD["District_ID"].Text;

                    //DateTime dt;
                    //int dataMonth = 0;
                    //int dataYear = 0;

                    // District: Baseline TRx, MST - runs only once
                    string cID, cID2;
                    Decimal base_d_TRx = 0;
                    Decimal base_d_MST = 0;
                    cID = "D_TRx";
                    cID2 = "D_MST";
                    Utility.get_cellValue_fromLabel(dataItemD, cID, cID2, ref base_d_TRx, ref base_d_MST);


                    // District: Goal TRx, MST - runs based on duration
                    for (int y = 0; y <= duration; y++)
                    {
                        //dt = goals.Start.AddMonths(x);
                        //name = string.Format("{0:yyyy}{0:MM}", dt);

                        string D_controlID, D_controlID2;
                        Decimal D_TRx = 0;
                        Decimal D_MST = 0;


                        if (y == 0)  // to save Baseline TRx, MST in month0 (Campaign_Month_Indicator: 0 in Campaign_Goals
                        {
                            Utility.InsertUpdateProcess(campaignID, baseMonth, baseYear, DistrictID, 2, y, base_d_TRx, base_d_MST);
                            Utility.InsertBaselineProcess(campaignID, baseMonth, baseYear, DistrictID, 2, y, base_d_TRx, base_d_MST);

                        }
                        else
                        {
                            dt = start.AddMonths(y - 1);
                            dataYear = Convert.ToInt32(string.Format("{0:yyyy}", dt));
                            dataMonth = Convert.ToInt32(string.Format("{0:MM}", dt));

                            D_controlID = "D_TRx" + y;
                            D_controlID2 = "D_MST" + y;
                            Utility.get_cellValue_fromTextbox(dataItemD, D_controlID, D_controlID2, ref D_TRx, ref D_MST);

                            Utility.InsertUpdateProcess(campaignID, dataMonth, dataYear, DistrictID, 2, y, D_TRx, D_MST);
                        }
                    }


                    ///// territory
                    if (goals.DistrictGrid.MasterTableView.Items[a].ChildItem.NestedTableViews[0].Items[b].ChildItem.NestedTableViews[0].Name == "TerritoryDetails")
                    {
                        int rowCount_T = goals.DistrictGrid.MasterTableView.Items[a].ChildItem.NestedTableViews[0].Items[b].ChildItem.NestedTableViews[0].Items.Count;
                        //geoLevelID = 3;  // Territory

                        for (int t = 0; t < rowCount_T; t++)
                        {

                            GridDataItem dataItemT = goals.DistrictGrid.MasterTableView.Items[a].ChildItem.NestedTableViews[0].Items[b].ChildItem.NestedTableViews[0].Items[t];                // itemsR[i] as GridDataItem;
                            string TerritoryID = dataItemT["Territory_ID"].Text;

                            //DateTime dt;
                            //string name;

                            // Territory: Baseline TRx, MST - runs only once
                            string ctID, ctID2;
                            Decimal base_t_TRx = 0;
                            Decimal base_t_MST = 0;
                            ctID = "T_TRx";
                            ctID2 = "T_MST";
                            Utility.get_cellValue_fromLabel(dataItemT, ctID, ctID2, ref base_t_TRx, ref base_t_MST);



                            for (int z = 0; z <= duration; z++)
                            {
                                //dt = goals.Start.AddMonths(x);
                                //name = string.Format("{0:yyyy}{0:MM}", dt);

                                string T_controlID, T_controlID2;
                                Decimal T_TRx = 0;
                                Decimal T_MST = 0;



                                if (z == 0)  // to save Baseline TRx, MST in month0 (Campaign_Month_Indicator: 0 in Campaign_Goals
                                {
                                    Utility.InsertUpdateProcess(campaignID, baseMonth, baseYear, TerritoryID, 3, z, base_t_TRx, base_t_MST);

                                    Utility.InsertBaselineProcess(campaignID, baseMonth, baseYear, TerritoryID, 3, z, base_t_TRx, base_t_MST);

                                }
                                else
                                {
                                    dt = start.AddMonths(z - 1);
                                    dataYear = Convert.ToInt32(string.Format("{0:yyyy}", dt));
                                    dataMonth = Convert.ToInt32(string.Format("{0:MM}", dt));

                                    T_controlID = "T_TRx" + z;
                                    T_controlID2 = "T_MST" + z;
                                    Utility.get_cellValue_fromTextbox(dataItemT, T_controlID, T_controlID2, ref T_TRx, ref T_MST);

                                    Utility.InsertUpdateProcess(campaignID, dataMonth, dataYear, TerritoryID, 3, z, T_TRx, T_MST);
                                }
                            }
                        }

                    }
                    //////////territory: end 

                }
                ///// district: for - end
            }
            ///// district: end

        }


        //// Plan
        //int rowCount_Plan = goals.PlanGoalGrid.MasterTableView.Items.Count;
        GridItem[] planItem = goals.PlanGoalGrid.MasterTableView.GetItems(GridItemType.Item);
        //for (int p = 0; p < rowCount_Plan; p++)
        if (planItem.Length > 0)
        {
            GridDataItem dataItemPlan = goals.PlanGoalGrid.MasterTableView.Items[0];
            if (dataItemPlan != null && dataItemPlan is GridDataItem)
            {

                string planID = dataItemPlan["Plan_ID"].Text;
                // string _brand = dataItemPlan["Brand_Name"].Text;  //GridTemplateColumn is not working

                // Region: Baseline TRx, MST - runs only once
                string PlanControlID, PlanControlID2;
                Decimal base_Plan_TRx = 0;
                Decimal base_Plan_MST = 0;
                PlanControlID = "TRx";
                PlanControlID2 = "MST";
                Utility.get_cellValue_fromLabel(dataItemPlan, PlanControlID, PlanControlID2, ref base_Plan_TRx, ref base_Plan_MST);


                // Region: Goal TRx, MST - runs based on duration
                for (int k = 0; k <= duration; k++)
                {
                    string Plan_controlID, Plan_controlID2;
                    Decimal Plan_TRx = 0;
                    Decimal Plan_MST = 0;




                    if (k == 0)  // to save Baseline TRx, MST in month0 (Campaign_Month_Indicator: 0 in Campaign_Goals
                    {
                        Utility.InsertUpdateProcess_Plan(campaignID, baseMonth, baseYear, k, base_Plan_TRx, base_Plan_MST);
                        Utility.InsertBaselineProcess_Plan(campaignID, baseMonth, baseYear, k, base_Plan_TRx, base_Plan_MST);

                    }
                    else
                    {
                        dt = start.AddMonths(k - 1);
                        dataYear = Convert.ToInt32(string.Format("{0:yyyy}", dt));
                        dataMonth = Convert.ToInt32(string.Format("{0:MM}", dt));

                        Plan_controlID = "TRx" + k;
                        Plan_controlID2 = "MST" + k;

                        Utility.get_cellValue_fromTextbox(dataItemPlan, Plan_controlID, Plan_controlID2, ref Plan_TRx, ref Plan_MST);
                        Utility.InsertUpdateProcess_Plan(campaignID, dataMonth, dataYear, k, Plan_TRx, Plan_MST);
                    }
                }
            }
        }  //plan


        // status changed to 4(Goals saved)
        using (SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString))
        {
            cn.Open();

            //Only save status the first time Goals are saved
            //if (((MasterPage)Master).PhaseID == 3)
            if (_isEdit)
            {
                using (SqlCommand cmd = new SqlCommand("pprx.usp_Campaign_UpdateGoals", cn))
                {
                    cmd.Parameters.AddWithValue("@Campaign_ID", campaignID);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();
                }
            }
        }



        Response.Redirect("createcampaign_step1_goals.aspx?id=" + Request.QueryString["id"]);
    }

    protected void btn_edit_Click(object sender, EventArgs e)
    {
      //  Response.Redirect("createcampaign_step1_goals.aspx?id=" + Request.QueryString["id"]);
       
  
    }

    #region IEditPage Members
    
    public bool Save()
    {
        /*
        int baseMonth = 0;
        int baseYear = 0;
        string baseYearMonth = string.Empty;

        int duration = 0;
        int campaignID = Convert.ToInt32(Request.QueryString["id"]);
        string planName = string.Empty;
        DateTime start = DateTime.MinValue;
        int userID = Pinsonault.Web.Session.UserID;
        Goals.getCampaignBaselineYearMonth(campaignID, out baseMonth, out baseYear, out baseYearMonth, out duration);
        Goals.getCampaignTimeline(campaignID, out start, out duration, out planName);
        ///////////////////////////////////////////////
        //TODO: validation
        

        //TODO: save goal %
        string pValues = string.Empty;
        
        //goal1 = Convert.ToDecimal(Request.Form["goals.GoalPercent1"]);
        HtmlTable _table = (HtmlTable)goals.TableGoalPercent;
        for (int s = 1; s <= duration; s++)
        {
            string _percentMonthID = "percentGrowth_month" + s;
            TextBox _txtGoal = (TextBox)_table.FindControl(_percentMonthID);
            pValues = pValues + _txtGoal.Text + ",";
        }

        // Goal Percent: hide months
        int toHideMonth_Begin = Convert.ToInt32(duration + 1);
        for (int x = toHideMonth_Begin; x <= 12; x++)   // max: 12 month
        {
            pValues = pValues + "0,";
        }
        string[] _p =pValues.Split(',');

        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["impact"].ConnectionString))
        {
            cn.Open();
            using (SqlCommand cmd = new SqlCommand("usp_CampaignGoals_IncrementPercent_InsertUpdate", cn))
            {
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Campaign_ID", campaignID);
                cmd.Parameters.AddWithValue("@Goal1", Convert.ToDecimal(_p[0]));
                cmd.Parameters.AddWithValue("@Goal2", Convert.ToDecimal(_p[1]));
                cmd.Parameters.AddWithValue("@Goal3", Convert.ToDecimal(_p[2]));
                cmd.Parameters.AddWithValue("@Goal4", Convert.ToDecimal(_p[3]));
                cmd.Parameters.AddWithValue("@Goal5", Convert.ToDecimal(_p[4]));
                cmd.Parameters.AddWithValue("@Goal6", Convert.ToDecimal(_p[5]));
                cmd.Parameters.AddWithValue("@Goal7", Convert.ToDecimal(_p[6]));
                cmd.Parameters.AddWithValue("@Goal8", Convert.ToDecimal(_p[7]));
                cmd.Parameters.AddWithValue("@Goal9", Convert.ToDecimal(_p[8]));
                cmd.Parameters.AddWithValue("@Goal10", Convert.ToDecimal(_p[9]));
                cmd.Parameters.AddWithValue("@Goal11", Convert.ToDecimal(_p[10]));
                cmd.Parameters.AddWithValue("@Goal12", Convert.ToDecimal(_p[11]));
                cmd.Parameters.AddWithValue("@User_ID", userID);

                // cmd.Parameters.AddWithValue("@UserName", Impact.User.FullName);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

            }
        }

        ///////////////////// RadGrid ///////////////////
 
        

        DateTime dt;
        int dataMonth = 0;
        int dataYear = 0;


        //// Region
        int rowCount_R = goals.DistrictGrid.MasterTableView.Items.Count;
        for (int a = 0; a < rowCount_R; a++)
        {
            GridDataItem dataItemR = goals.DistrictGrid.MasterTableView.Items[a];
            Int32 geoLevelID = 1;  // Region   
            if (dataItemR != null && dataItemR is GridDataItem)
            {

                string regionID = dataItemR["Region_ID"].Text;
                // string _brand = dataItemR["Brand_Name"].Text;  //GridTemplateColumn is not working

                // Region: Baseline TRx, MST - runs only once
                string controlID, controlID2;
                Decimal _TRx = 0;
                Decimal _MST = 0;
                controlID = "R_TRx";
                controlID2 = "R_MST";
                Utility.get_cellValue_fromLabel(dataItemR, controlID, controlID2, ref _TRx, ref _MST);


                // Region: Goal TRx, MST - runs based on duration
                for (int x = 0; x <= duration; x++)
                {

                    string R_controlID, R_controlID2;
                    Decimal R_TRx = 0;
                    Decimal R_MST = 0;


                    if (x == 0)  // to save Baseline TRx, MST in month0 (Campaign_Month_Indicator: 0 in Campaign_Goals
                    {
                        Utility.InsertUpdateProcess(campaignID, baseMonth, baseYear, regionID, 1, x, _TRx, _MST);
                        Utility.InsertBaselineProcess(campaignID, baseMonth, baseYear, regionID, 1, x, _TRx, _MST);

                    }
                    else
                    {
                        dt = start.AddMonths(x - 1);
                        dataYear = Convert.ToInt32(string.Format("{0:yyyy}", dt));
                        dataMonth = Convert.ToInt32(string.Format("{0:MM}", dt));

                        R_controlID = "R_TRx" + x;
                        R_controlID2 = "R_MST" + x;

                        Utility.get_cellValue_fromTextbox(dataItemR, R_controlID, R_controlID2, ref R_TRx, ref R_MST);

                        Utility.InsertUpdateProcess(campaignID, dataMonth, dataYear, regionID, 1, x, R_TRx, R_MST);
                    }
                }
            }


            /////// district
            if (goals.DistrictGrid.MasterTableView.Items[a].ChildItem.NestedTableViews[0].Name == "DistrictDetails")
            {
                // itemsD = goals.DistrictGrid.MasterTableView.Items[0].ChildItem.NestedTableViews[0].GetItems(GridItemType.Item);
                int rowCount_D = goals.DistrictGrid.MasterTableView.Items[a].ChildItem.NestedTableViews[0].Items.Count;

                //geoLevelID = 2;  // District 
                for (int b = 0; b < rowCount_D; b++)
                {
                    GridDataItem dataItemD = goals.DistrictGrid.MasterTableView.Items[a].ChildItem.NestedTableViews[0].Items[b];                // itemsR[i] as GridDataItem;
                    string DistrictID = dataItemD["District_ID"].Text;

                    //DateTime dt;
                    //int dataMonth = 0;
                    //int dataYear = 0;

                    // District: Baseline TRx, MST - runs only once
                    string cID, cID2;
                    Decimal base_d_TRx = 0;
                    Decimal base_d_MST = 0;
                    cID = "D_TRx";
                    cID2 = "D_MST";
                    Utility.get_cellValue_fromLabel(dataItemD, cID, cID2, ref base_d_TRx, ref base_d_MST);


                    // District: Goal TRx, MST - runs based on duration
                    for (int y = 0; y <= duration; y++)
                    {
                        //dt = goals.Start.AddMonths(x);
                        //name = string.Format("{0:yyyy}{0:MM}", dt);

                        string D_controlID, D_controlID2;
                        Decimal D_TRx = 0;
                        Decimal D_MST = 0;


                        if (y == 0)  // to save Baseline TRx, MST in month0 (Campaign_Month_Indicator: 0 in Campaign_Goals
                        {
                            Utility.InsertUpdateProcess(campaignID, baseMonth, baseYear, DistrictID, 2, y, base_d_TRx, base_d_MST);
                            Utility.InsertBaselineProcess(campaignID, baseMonth, baseYear, DistrictID, 2, y, base_d_TRx, base_d_MST);

                        }
                        else
                        {
                            dt = start.AddMonths(y - 1);
                            dataYear = Convert.ToInt32(string.Format("{0:yyyy}", dt));
                            dataMonth = Convert.ToInt32(string.Format("{0:MM}", dt));

                            D_controlID = "D_TRx" + y;
                            D_controlID2 = "D_MST" + y;
                            Utility.get_cellValue_fromTextbox(dataItemD, D_controlID, D_controlID2, ref D_TRx, ref D_MST);

                            Utility.InsertUpdateProcess(campaignID, dataMonth, dataYear, DistrictID, 2, y, D_TRx, D_MST);
                        }
                    }


                    ///// territory
                    if (goals.DistrictGrid.MasterTableView.Items[a].ChildItem.NestedTableViews[0].Items[b].ChildItem.NestedTableViews[0].Name == "TerritoryDetails")
                    {
                        int rowCount_T = goals.DistrictGrid.MasterTableView.Items[a].ChildItem.NestedTableViews[0].Items[b].ChildItem.NestedTableViews[0].Items.Count;
                        //geoLevelID = 3;  // Territory

                        for (int t = 0; t < rowCount_T; t++)
                        {

                            GridDataItem dataItemT = goals.DistrictGrid.MasterTableView.Items[a].ChildItem.NestedTableViews[0].Items[b].ChildItem.NestedTableViews[0].Items[t];                // itemsR[i] as GridDataItem;
                            string TerritoryID = dataItemT["Territory_ID"].Text;

                            //DateTime dt;
                            //string name;

                            // Territory: Baseline TRx, MST - runs only once
                            string ctID, ctID2;
                            Decimal base_t_TRx = 0;
                            Decimal base_t_MST = 0;
                            ctID = "T_TRx";
                            ctID2 = "T_MST";
                            Utility.get_cellValue_fromLabel(dataItemT, ctID, ctID2, ref base_t_TRx, ref base_t_MST);



                            for (int z = 0; z <= duration; z++)
                            {
                                //dt = goals.Start.AddMonths(x);
                                //name = string.Format("{0:yyyy}{0:MM}", dt);

                                string T_controlID, T_controlID2;
                                Decimal T_TRx = 0;
                                Decimal T_MST = 0;



                                if (z == 0)  // to save Baseline TRx, MST in month0 (Campaign_Month_Indicator: 0 in Campaign_Goals
                                {
                                    Utility.InsertUpdateProcess(campaignID, baseMonth, baseYear, TerritoryID, 3, z, base_t_TRx, base_t_MST);

                                    Utility.InsertBaselineProcess(campaignID, baseMonth, baseYear, TerritoryID, 3, z, base_t_TRx, base_t_MST);

                                }
                                else
                                {
                                    dt = start.AddMonths(z - 1);
                                    dataYear = Convert.ToInt32(string.Format("{0:yyyy}", dt));
                                    dataMonth = Convert.ToInt32(string.Format("{0:MM}", dt));

                                    T_controlID = "T_TRx" + z;
                                    T_controlID2 = "T_MST" + z;
                                    Utility.get_cellValue_fromTextbox(dataItemT, T_controlID, T_controlID2, ref T_TRx, ref T_MST);

                                    Utility.InsertUpdateProcess(campaignID, dataMonth, dataYear, TerritoryID, 3, z, T_TRx, T_MST);
                                }
                            }
                        }

                    }
                    //////////territory: end 

                }
                ///// district: for - end
            }
            ///// district: end

        }


        //// Plan
        //int rowCount_Plan = goals.PlanGoalGrid.MasterTableView.Items.Count;
        GridItem[] planItem = goals.PlanGoalGrid.MasterTableView.GetItems(GridItemType.Item);
        //for (int p = 0; p < rowCount_Plan; p++)
        if (planItem.Length > 0)
        {
            GridDataItem dataItemPlan = goals.PlanGoalGrid.MasterTableView.Items[0];
            if (dataItemPlan != null && dataItemPlan is GridDataItem)
            {

                string planID = dataItemPlan["Plan_ID"].Text;
                // string _brand = dataItemPlan["Brand_Name"].Text;  //GridTemplateColumn is not working

                // Region: Baseline TRx, MST - runs only once
                string PlanControlID, PlanControlID2;
                Decimal base_Plan_TRx = 0;
                Decimal base_Plan_MST = 0;
                PlanControlID = "TRx";
                PlanControlID2 = "MST";
                Utility.get_cellValue_fromLabel(dataItemPlan, PlanControlID, PlanControlID2, ref base_Plan_TRx, ref base_Plan_MST);


                // Region: Goal TRx, MST - runs based on duration
                for (int k = 0; k <= duration; k++)
                {
                    string Plan_controlID, Plan_controlID2;
                    Decimal Plan_TRx = 0;
                    Decimal Plan_MST = 0;




                    if (k == 0)  // to save Baseline TRx, MST in month0 (Campaign_Month_Indicator: 0 in Campaign_Goals
                    {
                        Utility.InsertUpdateProcess_Plan(campaignID, baseMonth, baseYear, k, base_Plan_TRx, base_Plan_MST);
                        Utility.InsertBaselineProcess_Plan(campaignID, baseMonth, baseYear, k, base_Plan_TRx, base_Plan_MST);

                    }
                    else
                    {
                        dt = start.AddMonths(k - 1);
                        dataYear = Convert.ToInt32(string.Format("{0:yyyy}", dt));
                        dataMonth = Convert.ToInt32(string.Format("{0:MM}", dt));

                        Plan_controlID = "TRx" + k;
                        Plan_controlID2 = "MST" + k;

                        Utility.get_cellValue_fromTextbox(dataItemPlan, Plan_controlID, Plan_controlID2, ref Plan_TRx, ref Plan_MST);
                        Utility.InsertUpdateProcess_Plan(campaignID, dataMonth, dataYear, k, Plan_TRx, Plan_MST);
                    }
                }
            }
        }  //plan


        // status changed to 4(Goals saved)
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["impact"].ConnectionString))
        {
            cn.Open();

            //Only save status the first time Goals are saved
            //if (((MasterPage)Master).PhaseID == 3)
            if (_isEdit)
            {
                using (SqlCommand cmd = new SqlCommand("usp_Campaign_UpdateGoals", cn))
                {
                    cmd.Parameters.AddWithValue("@Campaign_ID", campaignID);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();
                }
            }
        }




        ///////////////
        goals.IsEdit = false;
        goals.AllowEdit = true;
        Response.Redirect("createcampaign_step1_goals.aspx?id=" + Request.QueryString["id"]);
        */
        return true;
    }


    public void Reset()
    {
        //don't reset if PhaseID is 3 (meaning AE has not submitted Goals once)
        //goals.IsEdit = ((MasterPage)Master).PhaseID == 3;
        Response.Redirect("createcampaign_step1_goals.aspx?id=" + Request.QueryString["id"]);
  
    }

    public void Edit()
    {
        goals.IsEdit = true;

        //need to rebind to support dynamically generated template columns
        //Response.Redirect("createcampaign_step1_goals.aspx?id=" + Request.QueryString["id"] + "&postback=true");
       
        //goals.PlanGrid.DataBind();
        //goals.DistrictGrid.DataBind();
    }

    #endregion

}
