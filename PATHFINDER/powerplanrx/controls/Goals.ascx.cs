using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.Configuration;
using System.Text;
//using Impact.Utility;
using System.Web.UI.HtmlControls;
using System.Data;
using Pinsonault.Application.PowerPlanRx;

public partial class controls_GoalsByDistrict : System.Web.UI.UserControl
{
    public Telerik.Web.UI.RadGrid PlanGoalGrid
    {
        get { return gridView; }
    }

    public Telerik.Web.UI.RadGrid DistrictGrid
    {
        get { return gridViewDistricts; }
    }

    //public DropDownList TopNDistricts
    //{
    //    get { return topNDistricts; }
    //}

    //public string DistrictGoalsQuery
    //{
    //    get { return dsDGoals.SelectCommand; }
    //}

    //USED BY TEMPLATE CONTROLS TO DEFAULT ZEROS
    public static string GetFormattedGoalValue(object value, string format)
    {
        if (value == null || typeof(DBNull).Equals(value.GetType()))
            return string.Format(format, 0);
        else
            return string.Format(format, value);
    }


    /// <summary>
    /// Indicates that the page should show the "actual" values along with goals (summary information is also assumed then)
    /// </summary>
    public bool ShowResults { get; set; }
    /// <summary>
    /// Indicates that the stored summary information should be displayed for baseline 
    /// </summary>
    public bool ShowSummary { get; set; }

    public bool PDFExport { get; set; }

    /// <summary>
    /// Indicates that the control is being used on the main Goals page and it is possible to update targeting and summary data (not the same as IsEdit which is actual mode of page).
    /// </summary>
    public bool AllowEdit { get; set; }

    /// <summary>
    /// Indicates if the district grid should show in "edit" mode meaning the user can update which districts are targeted.
    /// </summary>
    public bool IsEdit
    {
        get
        {
            object val = ViewState["isEdit"];
            if (val != null)
                return Convert.ToBoolean(val);

            return false;
        }
        set
        {
            ViewState["isEdit"] = value;
        }
    }


    public int Duration
    {
        get;
        set;
    }

    public DateTime Start
    {
        get;
        set;
    }

    public List<int> Range = new List<int>();
    public bool dbEditMode
    {
        get
        {
            object val = ViewState["dbEditMode"];
            if (val != null)
                return Convert.ToBoolean(val);

            return false;
        }
        set
        {
            ViewState["dbEditMode"] = value;
        }
    }

    public bool _isEdit2 = false;
    public string Name { get; set; }
    public HtmlTable TableGoalPercent { get { return tblGoalPercent; } }
    public TextBox TextValidator { get { return txtDifference; } }
    //public Label LabelValidatorMessage { get { return lblValidatorMessage; } }
    public HiddenField HiddenValidator { get { return hValidator; } }

    /// <summary>
    /// Indicates if Campaign info section should be visible (name, brand, etc)
    /// </summary>
    public bool ShowCampaignInformation
    {
        get { return panelCampaignInformation.Visible; }
        set { panelCampaignInformation.Visible = value; }
    }

    public void ResetCommands()
    {

        //first time in set dynamic grid columns which are constructed based on timeline.  ViewState properties are also set so values obtained by this method call are retained accross postbacks.
        List<int> range = buildDynamicOutput(AllowEdit, ShowSummary, ShowResults);

        //always need to reset commands they may be needed in the event of sorting or paging a grid
        setDataSourceCommands(range, AllowEdit, ShowSummary, ShowResults);

    }

    protected override void OnInit(EventArgs e)
    {

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
                        _isEdit2 = (dr.GetInt32(0) == 3);

                }
            }
        }

        if (!IsPostBack)
        {
            this.gridViewDistricts.NeedDataSource += new GridNeedDataSourceEventHandler(this.gridViewDistricts_NeedDataSource);
            this.gridViewDistricts.DetailTableDataBind += new GridDetailTableDataBindEventHandler(this.gridViewDistricts_DetailTableDataBind);
                
        }
        else
        {
            if (!Convert.ToBoolean(Request.QueryString["qEdit"]) && !_isEdit2)
                Response.Redirect("createcampaign_step1_goals.aspx?id=" + Request.QueryString["id"] + "&qEdit=true");
        }

        base.OnInit(e);
    }

    protected override void OnLoad(EventArgs e)
    {

        //if (!IsPostBack)
        //{
            //ResetCommands();

            // plan
            List<int> range = buildDynamicOutput(AllowEdit, ShowSummary, ShowResults);
            //if (dbEditMode)
                gridView.DataSource = Plan_DataSource_EditMode();
            //else
            //    Plan_DataSource_InsertMode();

        //}

        base.OnLoad(e);
    }


    protected override void OnPreRender(EventArgs e)
    {
 
        if (IsEdit || (Convert.ToBoolean(Request.QueryString["qEdit"])))
        {
            
            string _pGoal = string.Empty;
            //get Goal Percent
            using (SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("pprx.usp_CampaignGoals_IncrementPercent_Select", cn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Campaign_ID", Convert.ToInt32(Request.QueryString["id"]));
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                            _pGoal = rdr.GetString(0);
                    }
                }
            }

            string[] _p = _pGoal.Split(',');

            // Goal Percent 
            divVolumeGrowth.Visible = true;
            // Goal Percent: header month label
            for (int a = 1; a <= Duration; a++)
            {
                DateTime dt;
                string name;
                dt = Start.AddMonths(a - 1);
                name = string.Format("{0:MMM yyyy}", dt);
                string _headerID = "GoalIncrement" + a;
                string _textID ="percentGrowth_month" + a;

                Label _lblMonth = (Label)tblGoalPercent.FindControl(_headerID);
                _lblMonth.Text = name;

                TextBox _txtPercent = (TextBox)tblGoalPercent.FindControl(_textID);
                _txtPercent.Text = _p[a-1];

            }

            // Goal Percent: hide months
            int toHideMonth_Begin = Convert.ToInt32(Duration + 1);
            for (int x = toHideMonth_Begin; x <= 12; x++)   // max: 12 month
            {
                string _tdHeaderID = "tdGoalHeader" + x;
                string _tdPercentID = "tdGoalPercent" + x;

                HtmlTableCell td = (HtmlTableCell)tblGoalPercent.FindControl(_tdHeaderID);
                HtmlTableCell td2 = (HtmlTableCell)tblGoalPercent.FindControl(_tdPercentID);
                td.Style.Add("display", "none");
                td2.Style.Add("display", "none");
            }

        }
        else
        {
            divVolumeGrowth.Visible = false;
            spanIndicator.Visible = false;
        }


        //rowCount.Visible = true;             // HttpContext.Current.IsDebuggingEnabled;
        base.OnPreRender(e);
    }

    void processGridHeaders(RadGrid grid, DateTime start)
    {
        Literal tableCell;
        int i = 0;
        foreach (Control control in grid.Controls[1].Controls[0].Controls[0].Controls[0].Controls)
        {
            if (control.Controls.Count > 0)
            {
                tableCell = control.Controls[0].FindControl("monthHeader") as Literal;
                if (tableCell != null)
                {
                    tableCell.Text = string.Format("{0:MMM yyyy}", start.AddMonths(i));
                    i++;
                }
            }
        }
    }

    //returns timeline information about a campaign
    void getCampaignTimeline(int id, out DateTime start, out int duration, out string planName)
    {
        start = DateTime.MinValue;
        duration = 0;
        planName = string.Empty;

        Goals.getCampaignTimeline(Convert.ToInt32(Request.QueryString["id"]), out start, out duration, out planName);

        Duration = duration;
        Start = start;

        //ViewState["startDate"] = start;
        //ViewState["duration"] = duration;
        //ViewState["planName"] = planName;
    }

    public Dictionary<string, string> SelectedDistricts
    {
        get;
        set;
    }

    /// <summary>
    /// Determines the date range for the campaign and updates the grids with appropriate columns.  BaselineDate, Range, & HeaderRowDetails page properties are set.
    /// </summary>
    List<int> buildDynamicOutput(bool allowEdit, bool showAsSummary, bool showCurrent)
    {
        DateTime start = DateTime.MinValue;
        int duration = 0;
        string planName;
        int baseMonth = 0;
        int baseYear = 0;
        string baseYearMonth = string.Empty;

        getCampaignTimeline(Convert.ToInt32(Request.QueryString["id"]), out start, out duration, out planName);
        Goals.getCampaignBaselineYearMonth(Convert.ToInt32(Request.QueryString["id"]), out baseMonth, out baseYear, out baseYearMonth, out duration);

        lblPlanName.Text = planName;

        //hide checkbox when on summary or results
        if (!allowEdit && (showAsSummary || showCurrent))
        {
            //gridViewDistricts.Columns[0].Visible = false;
            gridViewDistricts.ClientSettings.Scrolling.FrozenColumnsCount = 4;
        }
        else
            gridViewDistricts.ClientSettings.Scrolling.FrozenColumnsCount = 5;

        List<int> values = new List<int>();

        if (duration > 0)
        {
            hDuration.Value = Convert.ToString(duration);


            //GridTemplateColumn planCol = null;
            //GridTemplateColumn districtCol = null;

            DateTime dt;
            string name;

            for (int i = 0; i <= 12; i++)   // avoid error, pivoting 12 month
            {

                if (i == 0)  // when pivoting, month0 holds baseline year & month 
                {
                    values.Add(Convert.ToInt32(baseYearMonth));
                    name = baseYearMonth;
                }
                else if (i <= duration)
                {
                    dt = start.AddMonths(i - 1);   // if i= 1, then start.AddMonths(0) will be in month1 when pivoting 
                    values.Add((dt.Year * 100) + dt.Month);
                    name = string.Format("{0:yyyy}{0:MM}", dt);
                    Name = name;

                    //if(i==0)  sl
                    if (i == 1)
                        Page.ClientScript.RegisterHiddenField("startDateKey", name);
                }

            }
        }

        return values;
    }

    protected void gridView_PreRender(object sender, EventArgs e)
    {
        int toHideMonth_Begin = Convert.ToInt32(Duration + 1);

        //// hide GridTemplateColumn based on duration
        //int rowCount_plan = gridView.MasterTableView.Items.Count;

        //if (rowCount_plan > 0)
        //{
        for (int x = toHideMonth_Begin; x <= 12; x++)   // max: 12 month
            gridView.MasterTableView.GetColumn("month" + x).Visible = false;
        //}
    }

    protected void gridViewDistricts_PreRender(object sender, EventArgs e)
    {

        int toHideMonth_Begin = Convert.ToInt32(Duration + 1);

        //// hide GridTemplateColumn based on duration: working
        int rowCount_R = gridViewDistricts.MasterTableView.Items.Count;

        if (rowCount_R > 0)
        {

            for (int x = toHideMonth_Begin; x <= 12; x++)  // max: 12 month
                gridViewDistricts.MasterTableView.GetColumn("R_month" + x).Visible = false;

            for (int a = 0; a < rowCount_R; a++)
            {
                // district
                if (gridViewDistricts.MasterTableView.Items[a].ChildItem.NestedTableViews[0].Name == "DistrictDetails")
                {
                    int rowCount_D = gridViewDistricts.MasterTableView.Items[a].ChildItem.NestedTableViews[0].Items.Count;

                    //donot show header
                    if (rowCount_D == 0)
                    {
                        GridTableView childTV = gridViewDistricts.MasterTableView.Items[a].ChildItem.NestedTableViews[0];
                        childTV.ShowHeader = false;

                    }

                    for (int y = toHideMonth_Begin; y <= 12; y++)  // max: 12 month
                        gridViewDistricts.MasterTableView.Items[a].ChildItem.NestedTableViews[0].GetColumn("D_month" + y).Visible = false;

                    for (int b = 0; b < rowCount_D; b++)
                    {
                        // territory
                        if (gridViewDistricts.MasterTableView.Items[a].ChildItem.NestedTableViews[0].Items[b].ChildItem.NestedTableViews[0].Name == "TerritoryDetails")
                        {
                            for (int z = toHideMonth_Begin; z <= 12; z++)  // max: 12 month
                                gridViewDistricts.MasterTableView.Items[a].ChildItem.NestedTableViews[0].Items[b].ChildItem.NestedTableViews[0].GetColumn("T_month" + z).Visible = false;
                        }
                    }
                }
            }
        }
        else  // no Regions:  still hide if toHideMonth_Begin <= 12  
        {
            for (int x = toHideMonth_Begin; x <= 12; x++)  // max: 12 month
                gridViewDistricts.MasterTableView.GetColumn("R_month" + x).Visible = false;

        }

        if (!IsPostBack && !_isEdit2 && !Convert.ToBoolean(Request.QueryString["qEdit"]))
        {
            
            foreach (GridDataItem item in gridViewDistricts.MasterTableView.Items)
            {
                item.Expanded = true;
                foreach (GridDataItem item2 in item.ChildItem.NestedTableViews[0].Items)
                {
                    item2.Expanded = false;
                }
            }
        }
        else
        {
            
            foreach (GridDataItem item in gridViewDistricts.MasterTableView.Items)
            {
                item.Expanded = true;
                foreach (GridDataItem item2 in item.ChildItem.NestedTableViews[0].Items)
                {
                    item2.Expanded = true;
                }
            }
        }

    }


    public int RecordType(object value)
    {
        return value != null && !value.Equals(DBNull.Value) ? (int)value : 1;
    }

    protected bool IsTargetedDistrict(object value)
    {
        string districtName = value != null && !value.Equals(DBNull.Value) ? value.ToString() : "";

        return string.Compare(districtName, "Non-Targeted", true) != 0;
    }

    protected bool IsTargeted(object value)
    {
        return value != null && !value.Equals(DBNull.Value) ? (bool)value : false;
    }

    /// <summary>
    /// Dynamically generates appropriate sql statements for both Plan and District level goals based on timeline.  Appropriate SQLDataSource objects will be updated by this function.
    /// </summary>
    /// <param name="allowEdit"></param>
    /// <param name="showAsSummary"></param>
    /// <param name="showCurrent"></param>
    void setDataSourceCommands(List<int> range, bool allowEdit, bool showAsSummary, bool showCurrent)
    {

        //if (dbEditMode)
        //{
            //Region_DataSource_EditMode();   // edit mode: goals already saved in database
            gridViewDistricts.DataSource = Region_DataSource_EditMode();
            gridView.DataSource = Plan_DataSource_EditMode();

        //}

        //else
        //{
        //    Region_DataSource_InsertMode(1);   // Region: insert mode


        //    /////// Plan: .SelectCommand ////////////////
        //    Plan_DataSource_InsertMode();
        //}
    }

    private void Plan_DataSource_InsertMode()
    {
        using (SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand("pprx.usp_CampaignGoals_GetData_InsertMode", cn))
            {
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Campaign_ID", Convert.ToInt32(Request.QueryString["id"]));
                cmd.Parameters.AddWithValue("@Geography_Level_ID", 0);   // 0: plan

                //cmd.Parameters.AddWithValue("@Is_Targeted", 1);
                cmd.Parameters.AddWithValue("@Ranks", 50);
                cmd.Parameters.AddWithValue("@Geo_ID", null);
                cmd.CommandType = CommandType.StoredProcedure;

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cn.Open();
                da.Fill(dt);
                gridView.DataSource = dt;

            }
            //cn.Close();
        }
    }

    private void Region_DataSource_InsertMode(int geoLevelID)
    {
        using (SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand("pprx.usp_CampaignGoals_GetData_InsertMode", cn))
            {
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Campaign_ID", Convert.ToInt32(Request.QueryString["id"]));
                cmd.Parameters.AddWithValue("@Geography_Level_ID", geoLevelID);

                //cmd.Parameters.AddWithValue("@Is_Targeted", 1);
                cmd.Parameters.AddWithValue("@Ranks", 50);
                cmd.Parameters.AddWithValue("@Geo_ID", null);
                cmd.CommandType = CommandType.StoredProcedure;

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cn.Open();
                da.Fill(dt);
                gridViewDistricts.DataSource = dt;

            }
            //cn.Close();
        }
    }


    public DataTable Plan_DataSource_EditMode()
    {
        string mainViewForPlan;
        mainViewForPlan = "Campaign_Plan_EditMode";
        List<int> range = new List<int>();
        int duration = Duration;

        for (int i = 0; i <= duration; i++)
            range.Add(i);

        SQLPivotQuery<int> query = SQLPivotQuery<int>.Create(mainViewForPlan, new string[] { "Campaign_ID", "Plan_ID", "Plan_Name", "Brand_ID", "Brand_Name", "Start_Data_Key", "End_Data_Key" }, "Campaign_Month_Indicator", range)
              .Pivot(SQLFunction.MAX, "TRx")
            //.Pivot(SQLFunction.MAX, "MB_TRx")
              .Pivot(SQLFunction.MAX, "MST");

        //query.Where("Campaign_ID", "Campaign_ID", SQLOperator.EqualTo);

        StringBuilder sbPlan = new StringBuilder();

        sbPlan.Append("Select distinct *, TRx0 as TRx, MST0 as MST, (TRx0 * 100 / MST0) - TRx0 as Comp_Plan_TRx, (100 - MST0) as Comp_Plan_MST ");

        // 12 month pivot data needed even if campaign duration 3 month or ....  later hide extra month
        for (int j = duration + 1; j <= 12; j++)
        {
            sbPlan.AppendFormat(", 0 as {0}, 0 as {1} ", "TRx" + j, "MST" + j);
        }


        //sbPlan.Append(", case when Geography_Name <> 'Non-Targeted' then MST else case when MB_TRx = 0 then 0 else (convert(float,TRx)/convert(float,MB_TRx))*100 end end as MS");
        //sbPlan.Append(" from ( Select *");

        //sbPlan.AppendFormat(" from ({0}) as resultsTable", query);
        sbPlan.AppendFormat(" from ({0} where pivotTable0.Campaign_ID = " + Request.QueryString["id"] + ") as resultsTable", query);
        
        SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);
        SqlCommand cmd = new SqlCommand();
        sbPlan.Replace("Campaign_Plan_EditMode", "pprx.Campaign_Plan_EditMode");
        cmd.Connection = cn;
        cmd.CommandText = sbPlan.ToString();
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        cn.Open();
        da.Fill(dt);
        cn.Close();

        return dt;

    }

    public DataTable Region_DataSource_EditMode()
    {
        SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = cn;

        StringBuilder sbR = new StringBuilder();
        StringBuilder sbR_Targeted = new StringBuilder();
        StringBuilder sbR_NonTargeted = new StringBuilder();

        sbR_Targeted = Region_PivotPart(cmd, true);
        sbR_NonTargeted = Region_PivotPart(cmd, false);

        //sbR.AppendFormat(" from ({0} where pivotTable0.Campaign_ID = " + Request.QueryString["id"] + ") as resultsTable ", query);

        sbR.Append("Select * from (" + sbR_Targeted + "  Union " + sbR_NonTargeted + " ) finalT order by sortorder, R_TRx0 desc ");

        cmd.CommandText = sbR.ToString();
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        cn.Open();
        da.Fill(dt);
        cn.Close();

        return dt;

    }

    private StringBuilder Region_PivotPart(SqlCommand cmd, bool targeted)
    {
        string mainViewForRegion;
        mainViewForRegion = "hierarchy_Campaign_Region_EditMode";
        List<int> range = new List<int>();
        int duration = Duration;

        for (int i = 0; i <= duration; i++)
            range.Add(i);

        SQLPivotQuery<int> query = SQLPivotQuery<int>.Create(mainViewForRegion, new string[] { "Campaign_ID", "Plan_ID", "Region_ID", "Region_Name", "Brand_ID", "Brand_Name", "Start_Data_Key", "End_Data_Key" }, "Campaign_Month_Indicator", range)
              .Pivot(SQLFunction.MAX, "R_TRx")
            //.Pivot(SQLFunction.MAX, "R_MB_TRx")
              .Pivot(SQLFunction.MAX, "R_MST");

        //query.Where("Campaign_ID", "Campaign_ID", SQLOperator.EqualTo);

        StringBuilder sbR = new StringBuilder();

        //sl 1/13/2013  to add Competitor's TRx, MST
        if (targeted)
            sbR.Append("Select distinct 0 as sortorder,*, R_TRx0 as R_TRx, R_MST0 as R_MST, (R_TRx0 * 100 / R_MST0) - R_TRx0 as Comp_R_TRx, (100 - R_MST0) as Comp_R_MST ");
        else
            sbR.Append("Select distinct 1 as sortorder,*, R_TRx0 as R_TRx, R_MST0 as R_MST, (R_TRx0 * 100 / R_MST0) - R_TRx0 as Comp_R_TRx, (100 - R_MST0) as Comp_R_MST ");

        // 12 month pivot data needed even if campaign duration 3 month or ....  later hide extra month
        for (int j = duration + 1; j <= 12; j++)
        {
            sbR.AppendFormat(", 0 as {0}, 0 as {1} ", "R_TRx" + j, "R_MST" + j);
        }


        //sbR.Append(", case when Geography_Name <> 'Non-Targeted' then MST else case when MB_TRx = 0 then 0 else (convert(float,TRx)/convert(float,MB_TRx))*100 end end as MS");
        //sbR.Append(" from ( Select *");

        //sbR.AppendFormat(" from ({0}) as resultsTable", query);
        if (targeted)
            sbR.AppendFormat(" from ({0} where pivotTable0.Campaign_ID = " + Request.QueryString["id"] + " and pivotTable0.Region_ID <> 'Non-Targeted' ) as resultsTable ", query);


        else
            sbR.AppendFormat(" from ({0} where pivotTable0.Campaign_ID = " + Request.QueryString["id"] + " and pivotTable0.Region_ID = 'Non-Targeted' ) as resultsTable ", query);

        sbR.Replace("hierarchy_Campaign_Region_EditMode", "pprx.hierarchy_Campaign_Region_EditMode");
        return sbR;


    }

    protected void gridViewDistricts_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        List<int> range = buildDynamicOutput(AllowEdit, ShowSummary, ShowResults);
        if (!e.IsFromDetailTable)
        {
            //if (dbEditMode)
                //Region_DataSource_EditMode();  // edit mode: goals already saved in database
                gridViewDistricts.DataSource = Region_DataSource_EditMode();

            //else
            //    Region_DataSource_InsertMode(1);  // insert mode

        }
    }

    protected void gridViewDistricts_DetailTableDataBind(object source, Telerik.Web.UI.GridDetailTableDataBindEventArgs e)
    {
        GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;

        //List<int> range = buildDynamicOutput(AllowEdit, ShowSummary, ShowResults);
        int Campaign_ID = Convert.ToInt32(Request.QueryString["id"]);
        //if (dbEditMode) // edit mode: goals already saved in database

            switch (e.DetailTableView.Name)
            {
                case "DistrictDetails":
                    {
                        string RegionID;


                        GridTableView detailtable1 = (GridTableView)e.DetailTableView;
                        GridDataItem parentItem = (GridDataItem)detailtable1.ParentItem;
                        RegionID = parentItem.GetDataKeyValue("Region_ID").ToString();

                        //RegionID = dataItem.GetDataKeyValue("Region_ID").ToString();
                        //District_DataSource_EditMode(e, RegionID);
                        e.DetailTableView.DataSource = District_DataSource_EditMode(RegionID);
                        break;
                    }
                case "TerritoryDetails":
                    {
                        //Territory_DataSource_EditMode(); 
                        string DistrictID;
                        //string RegionID;
                        //RegionID = dataItem.GetDataKeyValue("Region_ID").ToString();
                        //DistrictID = dataItem.GetDataKeyValue("District_ID").ToString();

                        GridTableView detailtable2 = (GridTableView)e.DetailTableView;
                        GridDataItem parentItem = (GridDataItem)detailtable2.ParentItem;
                        DistrictID = parentItem.GetDataKeyValue("District_ID").ToString();


                        Territory_DataSource_EditMode(e, DistrictID);

                        break;
                    }
            }

       
    }

    private void Territory_DataSource_EditMode(Telerik.Web.UI.GridDetailTableDataBindEventArgs e, string DistrictID)
    {
        string mainView;
        mainView = "hierarchy_Campaign_Territory_EditMode";
        List<int> range = new List<int>();
        int duration = Duration;

        for (int i = 0; i <= duration; i++)
            range.Add(i);

        SQLPivotQuery<int> query = SQLPivotQuery<int>.Create(mainView, new string[] { "Campaign_ID", "Plan_ID", "District_ID", "Territory_ID", "Territory_Name", "Brand_ID", "Brand_Name", "Start_Data_Key", "End_Data_Key" }, "Campaign_Month_Indicator", range)
              .Pivot(SQLFunction.MAX, "T_TRx")
            //.Pivot(SQLFunction.MAX, "T_MB_TRx")
              .Pivot(SQLFunction.MAX, "T_MST");

        //query.Where("Campaign_ID", "Campaign_ID", SQLOperator.EqualTo);

        StringBuilder sbT = new StringBuilder();

        sbT.Append("Select distinct *, T_TRx0 as T_TRx, T_MST0 as T_MST ");

        // 12 month pivot data needed even if campaign duration 3 month or ....  later hide extra month
        for (int j = duration + 1; j <= 12; j++)
        {
            sbT.AppendFormat(", 0 as {0}, 0 as {1} ", "T_TRx" + j, "T_MST" + j);
        }


        //sbT.Append(", case when Geography_Name <> 'Non-Targeted' then MST else case when MB_TRx = 0 then 0 else (convert(float,TRx)/convert(float,MB_TRx))*100 end end as MS");
        //sbT.Append(" from ( Select *");
        sbT.AppendFormat(" from ({0} where pivotTable0.Campaign_ID = " + Request.QueryString["id"] + " and pivotTable0.District_ID = '" + DistrictID + "' ) as resultsTable order by T_TRx0 desc", query);

        sbT.Replace("hierarchy_Campaign_Territory_EditMode", "pprx.hierarchy_Campaign_Territory_EditMode");

        using (SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString))
        {
            cn.Open();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = cn;
                cmd.CommandText = sbT.ToString();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);
                e.DetailTableView.DataSource = dt;

            }
            //cn.Close();
        }
    }

    public DataTable District_DataSource_EditMode(string RegionID)
    {
        string mainView;
        mainView = "hierarchy_Campaign_District_EditMode";
        List<int> range = new List<int>();
        int duration = Duration;

        for (int i = 0; i <= duration; i++)
            range.Add(i);

        SQLPivotQuery<int> query = SQLPivotQuery<int>.Create(mainView, new string[] { "Campaign_ID", "Plan_ID", "Region_ID", "District_ID", "District_Name", "Brand_ID", "Brand_Name", "Start_Data_Key", "End_Data_Key" }, "Campaign_Month_Indicator", range)
              .Pivot(SQLFunction.MAX, "D_TRx")
              //.Pivot(SQLFunction.MAX, "D_MB_TRx")
              .Pivot(SQLFunction.MAX, "D_MST");

        //query.Where("Campaign_ID", "Campaign_ID", SQLOperator.EqualTo);

        StringBuilder sbD = new StringBuilder();

        sbD.Append("Select distinct *, D_TRx0 as D_TRx, D_MST0 as D_MST, (D_TRx0 * 100 / D_MST0) - D_TRx0 as Comp_D_TRx, (100 - D_MST0) as Comp_D_MST ");

        // 12 month pivot data needed even if campaign duration 3 month or ....  later hide extra month
        for (int j = duration + 1; j <= 12; j++)
        {
            sbD.AppendFormat(", 0 as {0}, 0 as {1} ", "D_TRx" + j, "D_MST" + j);
        }


        //sbD.Append(", case when Geography_Name <> 'Non-Targeted' then MST else case when MB_TRx = 0 then 0 else (convert(float,TRx)/convert(float,MB_TRx))*100 end end as MS");
        //sbD.Append(" from ( Select *");
        sbD.AppendFormat(" from ({0} where pivotTable0.Campaign_ID = " + Request.QueryString["id"] + " and pivotTable0.Region_ID = '" + RegionID + "' ) as resultsTable order by D_TRx0 desc", query);

        sbD.Replace("hierarchy_Campaign_District_EditMode", "pprx.hierarchy_Campaign_District_EditMode");

        SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);
        SqlCommand cmd = new SqlCommand();

        cmd.Connection = cn;
        cmd.CommandText = sbD.ToString();
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        cn.Open();
        da.Fill(dt);
        cn.Close();

        return dt;


    }

    public DataTable GetDataTable(int CampaignID, int GeoLevelID, string RegionID)
    {
        DataTable dt = new DataTable();
        SqlConnection cn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);
        SqlCommand cmd = new SqlCommand("pprx.usp_CampaignGoals_GetData_InsertMode", cn);

        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@Campaign_ID", CampaignID);
        cmd.Parameters.AddWithValue("@Geography_Level_ID", GeoLevelID);
        //cmd.Parameters.AddWithValue("@Is_Targeted", 1);
        cmd.Parameters.AddWithValue("@Ranks", 50);
        cmd.Parameters.AddWithValue("@Geo_ID", RegionID);
        cmd.CommandType = CommandType.StoredProcedure;

        SqlDataAdapter da = new SqlDataAdapter(cmd);
        cn.Open();
        da.Fill(dt);
        cn.Close();

        return dt;
    }


    protected void gridView_ItemCreated(object sender, GridItemEventArgs e)
    {


    }

    protected void gridView_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DateTime dt;
        string name;

        if (e.Item.OwnerTableView == gridView.MasterTableView && e.Item is GridHeaderItem)
        {
            GridHeaderItem item = (GridHeaderItem)e.Item;

            for (int x = 1; x <= Duration; x++)
            {
                dt = Start.AddMonths(x - 1);
                name = string.Format("{0:MMM yyyy}", dt);

                // header month dynamically based on Campaign timeline
                if (e.Item.ItemType == GridItemType.Header)
                {
                    Label _header = item.FindControl("header_month" + x) as Label;
                    if (_header != null)
                        _header.Text = name;
                }
            }
        }

    }


    protected void gridViewDistricts_OnItemDataBound(object sender, GridItemEventArgs e)
    {
        //if ( !PDFExport )
        //{
        DateTime dt;
        string name;

        if (e.Item.OwnerTableView.Name == "DistrictDetails" && e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            GridDataItem parentItem = (GridDataItem)e.Item.OwnerTableView.ParentItem;

            string hierarchyIndex = item.ItemIndexHierarchical;
            int itemIndex = item.ItemIndex;

            // baseline TRx, MST
            Label base_D_TRx = item.FindControl("D_TRx") as Label;
            Label base_D_MST = item.FindControl("D_MST") as Label;

            for (int x = 1; x <= Duration; x++)
            {
                dt = Start.AddMonths(x);
                name = string.Format("{0:yyyy}{0:MM}", dt);

                string ctl_D_TRx, ctl_D_MST;
                string parentControlID;

                // defined in .aspx
                ctl_D_TRx = "D_TRx" + x;
                ctl_D_MST = "D_MST" + x;
                RadTextBox obj_D_TRx = item.FindControl(ctl_D_TRx) as RadTextBox;
                RadTextBox obj_D_MST = item.FindControl(ctl_D_MST) as RadTextBox;

                // parent
                parentControlID = "R_TRx" + x;
                RadTextBox parentObj1 = parentItem.FindControl(parentControlID) as RadTextBox;

                //get parent Region ID
                string parentRegionID = parentItem.GetDataKeyValue("Region_ID").ToString();
 
                //if (obj_D_TRx != null)
                //    obj_D_TRx.Attributes.Add("onClick", "return GetCurrentValue('" + obj_D_TRx.ClientID + "')");

                if (obj_D_TRx != null && obj_D_MST != null && base_D_TRx != null && base_D_MST != null)
                {
                    if (IsEdit || (Convert.ToBoolean(Request.QueryString["qEdit"])))
                    {
                        obj_D_TRx.ReadOnly = false;
                        obj_D_TRx.CssClass = "UserInputBoxEdit";
                    }
                    else
                        obj_D_TRx.ReadOnly = true;




                    decimal currTRxValue = Convert.ToDecimal(obj_D_TRx.Text);
                    decimal currMSTValue = Convert.ToDecimal(obj_D_MST.Text);

                    //if 0, then % increment is not set (can't reallocate district value)
                    string isPercentSaved = "N";
                    if (currMSTValue != 0 && currTRxValue != 0)
                        isPercentSaved = "Y";



                    obj_D_TRx.Attributes.Add("onFocusout", "return ReAllocation('" + parentRegionID + "','" + isPercentSaved + "'," + currTRxValue + "," + currMSTValue + ", '" + base_D_TRx.ClientID + "', '" + base_D_MST.ClientID + "', '" + obj_D_TRx.ClientID + "', '" + obj_D_MST.ClientID + "', " + Duration + "," + x + ", '" + hierarchyIndex + "', " + itemIndex + ")");

                    //obj_D_TRx.Attributes.Add("onBlur", "HighlightChanged('" + obj_D_TRx.ClientID + "');");
                    //obj_D_TRx.Attributes.Add("onload", "loadHandler('" + obj_D_TRx + "');");
                
                }
            }
        }

        if (e.Item.OwnerTableView == gridViewDistricts.MasterTableView && e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            //get Region ID
            string regionID =item.GetDataKeyValue("Region_ID").ToString();
            if (regionID == "Non-Targeted")
            {
                //change row color
                item.CssClass = "nontargetedrow";
                

            }
 

        }

        if (e.Item.OwnerTableView == gridViewDistricts.MasterTableView && e.Item is GridHeaderItem)
        {

            GridHeaderItem item = (GridHeaderItem)e.Item;

            for (int x = 1; x <= Duration; x++)
            {
                dt = Start.AddMonths(x - 1);
                name = string.Format("{0:MMM yyyy}", dt);

                // header month dynamically based on Campaign timeline
                if (e.Item.ItemType == GridItemType.Header)
                {
                    Label _header = item.FindControl("R_header_month" + x) as Label;
                    if (_header != null)
                        _header.Text = name;
                }
            }
        }

        if (e.Item.OwnerTableView.Name == "DistrictDetails" && e.Item is GridHeaderItem)
        {
            GridHeaderItem item = (GridHeaderItem)e.Item;

            for (int x = 1; x <= Duration; x++)
            {
                dt = Start.AddMonths(x - 1);
                name = string.Format("{0:MMM yyyy}", dt);

                // header month dynamically based on Campaign timeline
                if (e.Item.ItemType == GridItemType.Header)
                {
                    Label _header = item.FindControl("D_header_month" + x) as Label;
                    if (_header != null)
                        _header.Text = name;
                }
            }
        }

        if (e.Item.OwnerTableView.Name == "TerritoryDetails" && e.Item is GridHeaderItem)
        {
            GridHeaderItem item = (GridHeaderItem)e.Item;

            for (int x = 1; x <= Duration; x++)
            {
                dt = Start.AddMonths(x - 1);
                name = string.Format("{0:MMM yyyy}", dt);

                // header month dynamically based on Campaign timeline
                if (e.Item.ItemType == GridItemType.Header)
                {
                    item.CssClass = "territoryHeader";
                    Label _header = item.FindControl("T_header_month" + x) as Label;
                    if (_header != null)
                        _header.Text = name;
                }
            }
        }

        ///////////////
        //}
    }

    protected void textBox_TextChanged(object sender, EventArgs e)
    {

        RadTextBox textBox = sender as RadTextBox;
        if (textBox != null)
        {
            textBox.CssClass = "UserInputBoxHighlight";
        }
    }


    protected void OnPageIndexChanged(object sender, EventArgs e)
    {
        //gridView.DataBind();
    }


    protected void gridViewDistricts_ItemCommand(object source, GridCommandEventArgs e)
    {
       

    }
    protected void gridViewDistricts_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
    {
        if (IsEdit || (Convert.ToBoolean(Request.QueryString["qEdit"])))
        {
            //gridViewDistricts.MasterTableView.ExpandCollapseColumn.Visible = false;
            if (e.Column is GridExpandColumn)
            {
                e.Column.HeaderStyle.Width = new Unit(1);
                e.Column.ItemStyle.Width = new Unit(1);
                //(e.Column as GridExpandColumn).Visible = false;
                
            }
        }
        else
        {
            if (e.Column is GridExpandColumn)
            {
                (e.Column as GridExpandColumn).ButtonType = GridExpandColumnType.ImageButton;

                (e.Column as GridExpandColumn).ExpandImageUrl = "../content/images/arwDwn.png";
                (e.Column as GridExpandColumn).CollapseImageUrl = "../content/images/arwRt.png";
            }
        }
    }
   
}
