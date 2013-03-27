using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Collections;
using System.Data.Objects;
using System.Collections.Specialized;
using Pinsonault.Data;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Data.Common;
using System.Drawing;
using PathfinderModel;
using System.Globalization;
using Pinsonault.Application.FormularyHistoryReporting;
using System.Reflection;

public partial class custom_pinso_formularyhistoryreporting_controls_formularycomparison : System.Web.UI.UserControl
{  
    string dataField = null;
    string tableName = null;
    string planLevelTableName = null;
    IList<string> keys = new List<string>();
    List<int> timeFrame = null;
    int totalperpage = 30;

    /// <summary>
    /// Following values are derived from fhr.Lkp_Display_Options table
    /// </summary>
    public enum LkpDisplayOptions
    {
        Tier = 1,
        F_Status = 2,
        PA = 3,
        QL = 4,
        ST = 5,
        CoPay = 6
    }
    public GridView HostedGrid
    {
        get { return gridFHX; }
    }
   
    protected void Page_Load(object sender, EventArgs e)
    {
        NameValueCollection queryValues = new NameValueCollection(Request.QueryString);      

        //if (!string.IsNullOrEmpty(queryValues["Plan_ID"]) && !string.IsNullOrEmpty(queryValues["Drug_ID"]))
        if (!string.IsNullOrEmpty(queryValues["Section_ID"]) && !string.IsNullOrEmpty(queryValues["Drug_ID"]))
        {           
            IEnumerable<GenericDataRecord> g = null;
            IList<int> timeFrameVals = new List<int>();
            IList<int> originalTimeFrameVals = new List<int>();
            IList<int> display_optionsVals = new List<int>();

            //Add keys for query, used for inner join in pivot condition 
            keys.Add("Plan_ID");
            keys.Add("Drug_ID");
            keys.Add("Pinso_Formulary_ID");   //changed column name from Formulary_ID to Pinso_Formulary_ID  
            keys.Add("Segment_ID");          
            keys.Add("Plan_Product_ID");


            // check to make sure calendar is selected
          
            int iSectionID = Convert.ToInt32(queryValues["Section_ID"]);
            FHXProvider fhr = new FHXProvider();

            //Time Frame will be converted to Max Month if originally quarterly based
            timeFrameVals = fhr.GetTimeFrameVals(queryValues, iSectionID, false);
            display_optionsVals = fhr.GetDisplayOptionList(queryValues, iSectionID);

            //Add original time frame vals for label processing
            //If Timeframe is originally quarterly based, it is converted to Max Month for that particular quarter
            originalTimeFrameVals = fhr.GetTimeFrameVals(queryValues, iSectionID, true);

            planLevelTableName = "V_GetFHXData_Geography"; //"V_GetPlanProductFormularyData";

            //string PlanID = queryValues["Plan_ID"];
            string DrugID = queryValues["Drug_ID"];
            string SectionID = queryValues["Section_ID"];

            string geography_id = "us";
            string dataField = "";

            bool isMonth = false;
            //if (iSectionID == 17) isMonth = true;
            if (queryValues["Monthly_Quarterly"] == "M") isMonth = true;

            if (timeFrameVals.Count > 0)
            {
                //Check if page count is needed
                NameValueCollection n = null;               

                if (!string.IsNullOrEmpty(queryValues["RequestPageCount"]) && queryValues["RequestPageCount"] == "true")
                {
                    if (string.IsNullOrEmpty(queryValues["StartPage"]))
                        queryValues.Add("StartPage", "1");

                    n = new NameValueCollection(queryValues);
                    n["PagingEnabled"] = "false";
                    IEnumerable<GenericDataRecord> gTotal = null;
                    gTotal = fhr.GetData(timeFrameVals, geography_id, planLevelTableName, dataField, keys, n, display_optionsVals);
                    gridCount.Text = gTotal.FirstOrDefault().GetValue(0).ToString();
                }                    

                if(string.IsNullOrEmpty(queryValues["TotalPerPage"]))
                    queryValues.Add("TotalPerPage", Convert.ToString(totalperpage)); //queryValues.Add("TotalPerPage", Convert.ToString(Math.Round(g.Count() * 1.00 / totalperpage)));
                
                if(string.IsNullOrEmpty(queryValues["pagingEnabled"]))
                    queryValues.Add("pagingEnabled", "true");
                
                g = fhr.GetData(timeFrameVals, geography_id, planLevelTableName, dataField, keys, queryValues, display_optionsVals);

                if (g.Count() > 0)
                {
                    //Process grid with original unprocessed timeframe values for correct header text values
                    ProcessGrid(gridFHX, isMonth, g, display_optionsVals, queryValues, originalTimeFrameVals);

                    fhrColorLegend.Visible = true;
                    divNoRecords.Visible = false;
                }
                else
                {
                    divNoRecords.Visible = true;

                    //Temporary fix to add 'No Records Text' until GridWrapper has the functionality
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Plan_Name");
                    dt.Columns.Add("Plan_State_ID");
                    dt.Columns.Add("Plan_Pharmacy_Lives");
                    dt.Columns.Add("Product_Name");
                    dt.Columns.Add("Formulary_Name");
                    dt.Columns.Add("Formulary_Lives");
                    dt.Columns.Add("Drug_Name");
                    dt.Rows.Add(dt.NewRow());

                    gridFHX.RowCreated -= new GridViewRowEventHandler(gridFHX_RowCreated);
                    gridFHX.DataBound -= new EventHandler(gridFHX_DataBound);
                    gridFHX.RowDataBound -= new GridViewRowEventHandler(gridFHX_RowDataBound);
                    gridFHX.DataSource = dt;                    
                    gridFHX.DataBind();
                    int totalColumns = gridFHX.Rows[0].Cells.Count;
                    gridFHX.Rows[0].Cells.Clear();
                    gridFHX.Rows[0].Cells.Add(new TableCell());
                    gridFHX.Rows[0].Cells[0].ColumnSpan = totalColumns;
                    gridFHX.Rows[0].Cells[0].Text = "No Records Found";
                }        
            }
        }
    }      

    protected void gridFHX_RowCreated(object sender, GridViewRowEventArgs e)
    {
        IList<int> display_optionsVals = new List<int>();
        FHXProvider fhr = new FHXProvider();
        display_optionsVals = fhr.GetDisplayOptionList(Request.QueryString, Convert.ToInt32(Request.QueryString["Section_ID"]));

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int iChangeColIndex = 0;
            int i = 0;
            int istatic = 6; //index of static columns-  Account name,State,Pharmacy Lives,Prod name, benefit design,formulary lives,Drug Name
 
            foreach (int idisp in display_optionsVals)
            {
                i = display_optionsVals.IndexOf(idisp) + 1;
                iChangeColIndex = istatic + 2 * i;
                switch (idisp)
                {
                    case 1:                            
                        if ((((Pinsonault.Data.GenericDataRecord)(e.Row.DataItem))).GetValue((((Pinsonault.Data.GenericDataRecord)(e.Row.DataItem))).GetOrdinal("Is_Tier_Name_Changed")).ToString() == "1")
                            e.Row.Cells[iChangeColIndex].BackColor = Color.Yellow;
                        break;
                    case 2:                           
                        if ((((Pinsonault.Data.GenericDataRecord)(e.Row.DataItem))).GetValue((((Pinsonault.Data.GenericDataRecord)(e.Row.DataItem))).GetOrdinal("Is_Formulary_Status_Name_Changed")).ToString() == "1")
                            e.Row.Cells[iChangeColIndex].BackColor = Color.Yellow;
                        break;
                    case 3:                            
                        if ((((Pinsonault.Data.GenericDataRecord)(e.Row.DataItem))).GetValue((((Pinsonault.Data.GenericDataRecord)(e.Row.DataItem))).GetOrdinal("Is_PA_Changed")).ToString() == "1")
                            e.Row.Cells[iChangeColIndex].BackColor = Color.Yellow;
                        break;
                    case 4:
                        if ((((Pinsonault.Data.GenericDataRecord)(e.Row.DataItem))).GetValue((((Pinsonault.Data.GenericDataRecord)(e.Row.DataItem))).GetOrdinal("Is_QL_Changed")).ToString() == "1")
                            e.Row.Cells[iChangeColIndex].BackColor = Color.Yellow;
                        break;
                    case 5:
                        if ((((Pinsonault.Data.GenericDataRecord)(e.Row.DataItem))).GetValue((((Pinsonault.Data.GenericDataRecord)(e.Row.DataItem))).GetOrdinal("Is_ST_Changed")).ToString() == "1")
                            e.Row.Cells[iChangeColIndex].BackColor = Color.Yellow;
                        break;
                    case 6:
                        if ((((Pinsonault.Data.GenericDataRecord)(e.Row.DataItem))).GetValue((((Pinsonault.Data.GenericDataRecord)(e.Row.DataItem))).GetOrdinal("Is_Co_Pay_Changed")).ToString() == "1")
                            e.Row.Cells[iChangeColIndex].BackColor = Color.Yellow;
                        break;
                }
                iChangeColIndex = istatic; //reset to static col index
            }
        }
    }    

    public void ProcessGrid(GridView grid, bool isMonth, IEnumerable<GenericDataRecord> g, IList<int> displayoptionsVals, NameValueCollection queryVals, IList<int> timeFrameVals)
    {
        List<int> displayoptions = displayoptionsVals.ToList();
        
        for (int x = 0; x < displayoptions.Count; x++)
        {
            switch (displayoptions[x])
            {
                case (int)LkpDisplayOptions.Tier:
                    addGridColumn(grid, "Tier_Name", timeFrameVals,isMonth);
                    break;
                case (int)LkpDisplayOptions.CoPay:
                    addGridColumn(grid, "Co_Pay", timeFrameVals,isMonth);
                    break;
                case (int)LkpDisplayOptions.F_Status:
                    addGridColumn(grid, "Formulary_Status_Name", timeFrameVals, isMonth);
                    break;
                case (int)LkpDisplayOptions.PA:
                    addGridColumn(grid, "PA", timeFrameVals, isMonth);
                    break;
                case (int)LkpDisplayOptions.QL:
                    addGridColumn(grid, "QL", timeFrameVals, isMonth);
                    break;
                case (int)LkpDisplayOptions.ST:
                    addGridColumn(grid, "ST", timeFrameVals, isMonth);
                    break;
            }
        }

        grid.DataSource = g;
        //rename "Benefit Design" as "Formulary" for Med D , field name used - "Formulary_Name"
        if(Request.QueryString["Section_ID"] == "17")
            grid.Columns[5].HeaderText = "Formulary";

        grid.DataBind();

    }

    public void addGridColumn(GridView grid,string ColName,IList<int> timeframe,bool isMonth)
    {
        // comparative report there would be always two time frame values       

        for (int i = 0; i < timeframe.Count; i++)
        {
            BoundField boundCol = new BoundField();            
            boundCol.ItemStyle.CssClass = "alignRight";
            boundCol.DataField = string.Format("{0}{1}", ColName, i);

            //get the month short name for Med D for Header Text else Q
            if (isMonth)
                boundCol.HeaderText = string.Format("{0} {1}", CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(Convert.ToInt32(timeframe[i].ToString().Substring(4))), timeframe[i].ToString().Substring(0, 4));
            else
                boundCol.HeaderText = string.Format("Q{0} {1}", timeframe[i].ToString().Substring(5), timeframe[i].ToString().Substring(0, 4));

            grid.Columns.Add(boundCol);              
        }
    }

    protected void gridFHX_RowDataBound(object sender, GridViewRowEventArgs e)
    {        
        IList<int> displayoptions = new List<int>();
        FHXProvider fhp = new FHXProvider();
        displayoptions = fhp.GetDisplayOptionList(Request.QueryString, Convert.ToInt32(Request.QueryString["Section_ID"]));

        int iStatic = 6; //static col index

        if (e.Row.RowType == DataControlRowType.Header)
        {
            //For each desired row, create a new FormatCells List
            //FormatCells.Add(<column number>,<header name,colspan,rowspan>)
            SortedList FormatCells = new SortedList();
            //up to 6th column or 7th col (Med D), the header columns are constant
            
            if (Request.QueryString["Section_ID"] == "17")
                iStatic = 7;

            for (int iCol = 1; iCol <= iStatic; iCol++)
            {
                FormatCells.Add(iCol, ",1,1");
            }        

            //Add Header rows pass in this order - column index, required no of column, rows
            
            for (int i = 0; i < displayoptions.Count; i++)
            {
                int j = iStatic + i + 1 ;
                string col1 = GetDisplayOptionName(displayoptions[i]);//displayoptions[i].ToString();               

                FormatCells.Add(j, string.Format("{0},2,1", col1));
            }

            AddDualRowHeader(e, FormatCells);
        }

        if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
        {

            for (int i = 7 ; i < e.Row.Cells.Count; i = i + 2)
            {             
                    e.Row.Cells[i].CssClass = "fhrBorderLeft";                   
            }             
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int j = 0; j < 7; j++)
            {
                e.Row.Cells[j].BackColor = System.Drawing.Color.White;
            }   
        }
    }

    public void AddDualRowHeader(GridViewRowEventArgs e, SortedList GetCels)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridViewRow row = new GridViewRow(-1, -1, DataControlRowType.Header, DataControlRowState.Normal);
            IDictionaryEnumerator enumCels = GetCels.GetEnumerator();            
            while (enumCels.MoveNext())
            {

                string[] count = enumCels.Value.ToString().Split(Convert.ToChar(","));
                TableHeaderCell Cell = new TableHeaderCell();
                Cell.RowSpan = Convert.ToInt16(count[2].ToString());
                Cell.ColumnSpan = Convert.ToInt16(count[1].ToString());
                Cell.Controls.Add(new LiteralControl(count[0].ToString()));
                Cell.HorizontalAlign = HorizontalAlign.Center;
                if (string.IsNullOrEmpty(count[0].ToString()))
                    Cell.CssClass = "doubleHeader";
                else
                    Cell.CssClass = "doubleHeader fhrBorderLeft";                
                row.Cells.Add(Cell);
            }

            e.Row.Parent.Controls.AddAt(0, row);

        }        
    }
    private string GetDisplayOptionName(int displayid)
    {
        string displayname = string.Empty;
        switch (displayid)
        {
            case 1:
                displayname = "Tier";
                break;
            case 2:
                displayname = "Status";
                break;
            case 3:
                displayname = "PA";
                break;
            case 4:
                displayname = "QL";
                break;
            case 5:
                displayname = "ST";
                break;
            case 6:
                displayname = "Copay";
                break;
        }      

        return displayname;
    }

    protected void gridFHX_DataBound(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        FHXProvider fhp = new FHXProvider();

        //fhp.GroupRows(gv, 6);
        fhp.GroupRows(gv, 5);
        fhp.GroupRows(gv, 4);
        fhp.GroupRows(gv, 3);
        fhp.GroupRows(gv, 2);
        fhp.GroupRows(gv, 1);
        fhp.GroupRows(gv, 0);

        //show product name only in case of Med D
        if (Request.QueryString["Section_ID"] == "17")       
            gv.Columns[3].Visible = true;       
        else
            gv.Columns[3].Visible = false;
    }
}
