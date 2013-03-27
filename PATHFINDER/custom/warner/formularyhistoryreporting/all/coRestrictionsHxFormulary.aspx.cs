using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dundas.Charting.WebControl;
using System.Collections.Specialized;
using Telerik.Web.UI;
using Pinsonault.Application.MarketplaceAnalytics;
using Pinsonault.Application.FormularyHistoryReporting;
using Pinsonault.Data;
using System.ComponentModel;
using System.Data;
using PathfinderModel;

public partial class custom_warner_formularyhistoryreporting_all_coRestrictionsHxFormulary : PageBase 
{
    static IList<CoverageStatus> _coverageStatus = null;
    public static IList<CoverageStatus> coverageStatus
    {
        get
        {
            if (_coverageStatus == null)
            {
                using (PathfinderModel.PathfinderEntities context = new PathfinderModel.PathfinderEntities())
                {
                    _coverageStatus = context.CoverageStatusSet.Where(e => e.ID != 4).OrderBy(cs => cs.ID).ToList(); //EXCLUDE Data Not Available
                }
            }

            return _coverageStatus;
        }
    }
     
       
    protected void Page_Load(object sender, EventArgs e)
    {
        //Find Grids and Charts
        Chart chart1 = (Chart)chart.FindControl("chartDisplay1").FindControl("chart");
        RadGrid grid1 = (RadGrid)chart.FindControl("grid1").FindControl("gridTemplate");

        Chart chart2 = (Chart)chart.FindControl("chartDisplay2").FindControl("chart");
        RadGrid grid2 = (RadGrid)chart.FindControl("grid2").FindControl("gridTemplate");
        //chart1.Visible = false;

        NameValueCollection queryValues = new NameValueCollection(Request.QueryString);

        string dataField = null;
        string tableName = null;

        string msdataField = null;
        string mstableName = null;

        IList<int> timeFrame = new List<int>();
        bool isMonth;

        IEnumerable<GenericDataRecord> g = null;
        IEnumerable<GenericDataRecord> g1 = null;

        queryValues.Add("Rollup_Type", "4"); //Rollup by Account
        queryValues.Add("Calendar_Rolling", "Calendar"); //Timeframe is always calendar based        

        //If Med-D, timeframe is month based
        if (queryValues["Monthly_Quarterly"] == "M")
        {
            //The timeframe is month based
            isMonth = true;
            msdataField = "TimeFrame";
            dataField = "Data_Year_Month";

            mstableName = "MS_Monthly";
            tableName = "V_GetPlanProductFormularyHistory";
        }
        else
        {
            //The timeframe is quarter based
            isMonth = false;
            msdataField = "TimeFrame";
            dataField = "Data_Year_Quarter";

            mstableName = "MS_Quarterly";
            tableName = "V_GetPlanProductFormularyHistory";
        }

        FHXProvider fhx = new FHXProvider();
        int geographyType = Convert.ToInt32(queryValues["Geography_Type"]);


        //string geographyID = "US";

        //if (geographyType == 2)
        //    geographyID = queryValues["Region_ID"];
        //if (geographyType == 3)
        //    geographyID = queryValues["Territory_ID"];
        //if ((geographyType == 1 || geographyType == 2) && !string.IsNullOrEmpty(queryValues["State_ID"]))
        //    geographyID = queryValues["State_ID"];
        string geographyID = queryValues["Geography_ID"];

        timeFrame.Add(Convert.ToInt32(queryValues["TimeFrame1"]));
        timeFrame.Add(Convert.ToInt32(queryValues["TimeFrame2"]));
        //dataField = "TimeFrame";

        //Add keys for query
        IList<string> keys = new List<string>();
        keys.Add("Drug_ID");
        keys.Add("Drug_Name");
        keys.Add("Product_ID");
        keys.Add("Product_Name");
        //keys.Add("Thera_ID");
        keys.Add("Geography_ID");
        keys.Add("Coverage_Status_ID");
        g = fhx.GetCoverageStatusDataEx(timeFrame, geographyID, tableName, dataField, keys, queryValues);

        timeFrame.Clear();
        timeFrame.Add(Convert.ToInt32(queryValues["TimeFrame1"]));
        timeFrame.Add(Convert.ToInt32(queryValues["TimeFrame2"]));  

        if (g.Count() > 0)
        {
            ProcessGrid(grid1, isMonth, g, timeFrame, queryValues);
            ProcessChart(timeFrame, chart1, isMonth, g, queryValues);
        }

        //Add keys for TRx query
        IList<string> keys_trx = new List<string>();
        keys_trx.Add("Drug_ID");
        keys_trx.Add("Drug_Name");
        keys_trx.Add("Product_ID");
        keys_trx.Add("Product_Name");
        keys_trx.Add("Thera_ID");
        keys_trx.Add("Geography_ID");
        g1 = fhx.GetTierTRXData(timeFrame, geographyID, mstableName, msdataField, keys_trx, queryValues);

        timeFrame.Clear();
        timeFrame.Add(Convert.ToInt32(queryValues["TimeFrame1"]));
        timeFrame.Add(Convert.ToInt32(queryValues["TimeFrame2"]));  

        if (g1.Count() > 0)
        {
            ProcessGrid_Trx(grid2, isMonth, g1, timeFrame, queryValues);
            ProcessChart_Trx(timeFrame, chart2, isMonth, g1, queryValues);
        }
    }

    public void ProcessGrid(RadGrid grid, bool isMonth, IEnumerable<GenericDataRecord> g, IList<int> timeFrameVals, NameValueCollection queryVals)
    {
        grid.Columns.Clear();
        List<int> timeFrame = timeFrameVals.ToList();

        //BoundField boundCol;
        Telerik.Web.UI.GridBoundColumn boundCol;
        Telerik.Web.UI.GridHyperLinkColumn Col;
        string previousTimeFrame = GetHeaderText(isMonth, timeFrame[0], queryVals);
        string currentTimeFrame = GetHeaderText(isMonth, timeFrame[1], queryVals);

        boundCol = new Telerik.Web.UI.GridBoundColumn();
        boundCol.DataField = "Coverage_Status_Name";
        boundCol.HeaderText = "Coverage Status";
        grid.Columns.Add(boundCol);

        string[] productIDarr = queryVals["Product_ID"].Split(',');
        int c = productIDarr.Count();
        string drugName = "";
        int i = 0;
        //for (int i = 0; i < c; i++)
        //{
            string Drug = "Drug" + (i + 1);
            string Tier = "Drug" + (i + 1) + "_CoverageStatusID";
            string[] urlFields = { Drug, Tier };
            drugName = g.FirstOrDefault().GetValue(g.FirstOrDefault().GetOrdinal("Drug" + (i + 1) + "_Name")).ToString();

            boundCol = new Telerik.Web.UI.GridBoundColumn();
            boundCol.DataField = "Drug" + (i + 1);
            boundCol.Visible = false;
            grid.Columns.Add(boundCol);

            boundCol = new Telerik.Web.UI.GridBoundColumn();
            boundCol.DataField = "Drug" + (i + 1) + "_Name";
            boundCol.HeaderText = "Drug";           
            grid.Columns.Add(boundCol);

            Col = new Telerik.Web.UI.GridHyperLinkColumn();
             if (string.Compare(queryVals["Trx_Mst"], "mst", true) == 0)
            {
                Col.DataTextField = "Drug" + (i + 1) + "_Percent1";
                Col.DataTextFormatString = "{0:N2}%";
            }
            else
            {
                Col.DataTextField = "Drug" + (i + 1) + "_TotalCovered1";
                Col.DataTextFormatString = "{0:n0}";
            }
            //Col.HeaderText = string.Format("{0}, {1}", previousTimeFrame, drugName);
            Col.HeaderText = previousTimeFrame;
            Col.DataNavigateUrlFields = urlFields;                     
            Col.DataNavigateUrlFormatString = "javascript:toggleGridRowSelection('" + grid.ClientID + "',{0},{1},0)"; //TierTimeIndex = 0, so filter for coverage_statusname0
            Col.ItemStyle.CssClass = "alignRight";
            grid.Columns.Add(Col);

            Col = new Telerik.Web.UI.GridHyperLinkColumn();
            if (string.Compare(queryVals["Trx_Mst"], "mst", true) == 0)
            {
                Col.DataTextField = "Drug" + (i + 1) + "_Percent2";
                Col.DataTextFormatString = "{0:N2}%";
            }
            else
            {
                Col.DataTextField = "Drug" + (i + 1) + "_TotalCovered2";
                Col.DataTextFormatString = "{0:n0}";
            }
            //Col.HeaderText = string.Format("{0}, {1}", currentTimeFrame, drugName);
            Col.HeaderText = currentTimeFrame;
            Col.DataNavigateUrlFields = urlFields;
            Col.DataNavigateUrlFormatString = "javascript:toggleGridRowSelection('" + grid.ClientID + "',{0},{1},1)";//TierTimeIndex = 1, so filter for coverage_statusname1
            Col.ItemStyle.CssClass = "alignRight";
            grid.Columns.Add(Col);
        //}


        grid.DataSource = g;
        grid.DataBind();       

    }

    public void ProcessGrid_Trx(RadGrid grid, bool isMonth, IEnumerable<GenericDataRecord> g, IList<int> timeFrameVals, NameValueCollection queryVals)
    {
        grid.Columns.Clear();
        List<int> timeFrame = timeFrameVals.ToList();

        Telerik.Web.UI.GridBoundColumn boundCol;
        Telerik.Web.UI.GridHyperLinkColumn Col;

        string previousTimeFrame = GetHeaderText(isMonth, timeFrame[0], queryVals);
        string currentTimeFrame = GetHeaderText(isMonth, timeFrame[1], queryVals);

        boundCol = new Telerik.Web.UI.GridBoundColumn();
        boundCol.DataField = "Product_Name";
        boundCol.HeaderText = "Product";
        grid.Columns.Add(boundCol);

        string[] urlFields = { "Product_ID" };
        //col1 trx\mst
        Col = new Telerik.Web.UI.GridHyperLinkColumn();
        if (string.Compare(queryVals["Trx_Mst"], "trx", true) == 0)
        {
            Col.DataTextField = "Product_TRx0";
            Col.HeaderText = previousTimeFrame + " Trx";
            Col.DataTextFormatString = "{0:N0}";
        }
        else
        {
            Col.DataTextField = "Product_Mst0";
            Col.HeaderText = previousTimeFrame + " Mst";
            Col.DataTextFormatString = "{0:N3}%";
        }
        Col.ItemStyle.CssClass = "alignRight";
        Col.DataNavigateUrlFields = urlFields;
        Col.DataNavigateUrlFormatString = "javascript:toggleGridRowSelection('" + grid.ClientID + "',{0})";         
        grid.Columns.Add(Col);

        //col2 trx\mst
        Col = new Telerik.Web.UI.GridHyperLinkColumn();
        if (string.Compare(queryVals["Trx_Mst"], "trx", true) == 0)
        {
            Col.DataTextField = "Product_TRx1";
            Col.HeaderText = currentTimeFrame + " Trx";
            Col.DataTextFormatString = "{0:N0}";
        }
        else
        {
            Col.DataTextField = "Product_Mst1";
            Col.HeaderText = currentTimeFrame + " Mst";
            Col.DataTextFormatString = "{0:N3}%";
        }
        Col.ItemStyle.CssClass = "alignRight";
        Col.DataNavigateUrlFields = urlFields;
        Col.DataNavigateUrlFormatString = "javascript:toggleGridRowSelection('" + grid.ClientID + "',{0})";        
        grid.Columns.Add(Col);


        grid.DataSource = g;
        grid.DataBind();
    }

    public void ProcessChart(IList<int> timeFrameVals, Chart chart, bool isMonth, IEnumerable<GenericDataRecord> g1, NameValueCollection queryVals)
    {
        List<int> timeFrame = timeFrameVals.ToList();

        string Drug_Name = null;
        //string Product_ID = "";
        string[] productlist = queryVals["Product_ID"].Split(',');

        DataTable dt = ToDataTable(g1.ToList());
        DataTable pivotTable = new DataTable();

        for (int i = 1; i <= productlist.Count(); i++)
        {
            pivotTable = dt.Select("Drug1 = " + productlist[i - 1]).CopyToDataTable();
            int index = 0;
            IList<CoverageStatus> coverageStatusList = coverageStatus.Reverse().ToList();

            //foreach (DataRow y in pivotTable.Rows)
            //{
            foreach (CoverageStatus coveragestatus in coverageStatusList)
            {
                DataRow y = pivotTable.Select("Coverage_Status_ID = " + coveragestatus.ID).SingleOrDefault();
                string Coverage_Status_Name = coveragestatus.Name;
                int Coverage_Status_ID = coveragestatus.ID;
                int Total_Covered = 0;               
                Drug_Name = pivotTable.Rows[0]["Drug1_Name"].ToString();

                for (int j = 1; j <= timeFrame.Count(); j++)
                {   
                    string columnExtension = "_TotalCovered"; //column for volume - default
                    if (string.Compare(queryVals["Trx_Mst"], "mst", true) == 0)
                    {
                        columnExtension = "_Percent";
                    }
                    string tc_ordinal = "Drug1" + columnExtension + j.ToString();                   

                    if (y != null)
                    {
                        Total_Covered = Convert.ToInt32(y[tc_ordinal]);                       
                    }
                    else
                    {
                        Total_Covered = 0;                      
                    }
                    if (!string.IsNullOrEmpty(Coverage_Status_Name))
                        addPoint(chart, Convert.ToInt16(productlist[i - 1]), Coverage_Status_ID, Coverage_Status_Name, Drug_Name, Total_Covered, index, queryVals, timeFrameVals, isMonth, timeFrameVals[j - 1]);
                }
                index++;
            }
        }
    }
            

    public void ProcessChart_Trx(IList<int> timeFrameVals, Chart chart, bool isMonth, IEnumerable<GenericDataRecord> g1, NameValueCollection queryVals)
    {
        List<int> timeFrame = timeFrameVals.ToList();
        string[] productlist = queryVals["Product_ID"].Split(',');

        chart.Attributes["_title"] = Resources.Resource.Label_Covered_Lives_Report;//for exporter

        string Product_Name = null;
        int Product_trx = 0;
        int Product_ID = 0;

        foreach (var y in g1)
        {
            Product_Name = y.GetValue(y.GetOrdinal("Product_Name")).ToString();
            Product_ID = Convert.ToInt32(y.GetValue(y.GetOrdinal("Product_ID")));
            int index = 0;
            for (int j = 0; j < timeFrame.Count(); j++)
            {
                string trx_ordinal = string.Format("{0}_{1}{2}", "Product", queryVals["Trx_Mst"], j.ToString());
                if (y.GetValue(y.GetOrdinal(trx_ordinal)) != DBNull.Value)
                    Product_trx = Convert.ToInt32(y.GetValue(y.GetOrdinal(trx_ordinal)));
                else
                    Product_trx = 0;
                string lbl = GetHeaderText(isMonth, timeFrame[j], queryVals);
                addPoint_Trx(chart, Product_Name, Product_trx, index, lbl, Product_ID, queryVals, timeFrameVals, isMonth, timeFrame[j]);
                index++;
            }

        }

    }

    void addPoint(Chart chart, int productID, int tierID, string Coverage_Status_Name, string drugName, int lives, int index, NameValueCollection queryVals, IList<int> timeFrameVals, bool isMonth, int timeFrame)
    {
        chart.Series[index].ShowInLegend = true;
        int pointIndex;

        if (string.Compare(queryVals["Trx_Mst"], "trx", true) == 0)
        {
            chart.ChartAreas[0].AxisY.LabelStyle.Format = "{0:N0}";
            pointIndex = chart.Series[index].Points.AddY(lives / 1000000.0);
        }
        else
        {
            chart.ChartAreas[0].AxisY.LabelStyle.Format = "{0:P0}"; //for percentage
            chart.ChartAreas[0].AxisY.Title = "Pharmacy Lives (%)";
            chart.ChartAreas[0].AxisY.Maximum = 100.00;
            pointIndex = chart.Series[index].Points.AddY(lives); //now volume is percent
        }    

        chart.Series[index].CustomAttributes = "DrawingStyle=Cylinder";
        chart.Series[index]["MaxPixelPointWidth"] = "45";

        chart.Series[index].Name = Coverage_Status_Name;
        chart.Series[index].Points[pointIndex].AxisLabel = string.Format("{0}/{1}", GetHeaderText(isMonth, timeFrame, queryVals), drugName);

        int TierTimeIndex = 0;
        if (timeFrame == Convert.ToInt32(queryVals["TimeFrame2"]))
            TierTimeIndex = 1;

        //show the drilldown same way as it is from grid        
        chart.Series[index].Points[pointIndex].Href = string.Format("javascript:showHideGrids(1, {0}, {1},{2})", productID, tierID, TierTimeIndex); //TierTimeIndex = 0, so filter for Tier_Name0
    }

    void addPoint_Trx(Chart chart, string drugName, int Product_trx, int index, string lblName, int productID, NameValueCollection queryVals, IList<int> timeFrameVals, bool isMonth, int timeFrame)
    {
        int pointIndex = chart.Series[index].Points.AddY(Product_trx);

        //Format numbers based on Trx/Mst or Nrx/Msn
        if ((string.Compare(queryVals["Trx_Mst"], "trx", true) == 0) || (string.Compare(queryVals["Trx_Mst"], "nrx", true) == 0))
            chart.ChartAreas[0].AxisY.LabelStyle.Format = "{0:N0}";
        else
            chart.ChartAreas[0].AxisY.LabelStyle.Format = "{0:P0}";

        chart.ChartAreas[0].AxisY.Title = queryVals["Trx_Mst"];
        chart.Series[index].CustomAttributes = "DrawingStyle=Cylinder";
        chart.Series[index].Name = lblName;
        //chart.Series[index].Points[pointIndex].Label = label;
        chart.Series[index].Points[pointIndex].AxisLabel = drugName;
                
        chart.Series[index].Points[pointIndex].Href = string.Format("javascript:showHideGrids(2, {0})", productID);
    }

    public string GetHeaderText(bool isMonth, int num, NameValueCollection queryVals)
    {
        string headerText;
        MarketplaceAnalyticsProvider ma = new MarketplaceAnalyticsProvider();

        if (queryVals["Monthly_Quarterly"] == "M")
            //headerText = GetTimeFrameName(num, "tr.Lkp_MS_MonthYears", "MonthYear");            
            headerText = string.Format("{0} {1}", ma.GetTimeFrameName(Convert.ToInt32(num.ToString().Substring(4)), "month"), Convert.ToInt32(num.ToString().Substring(0, 4)));
        else
            //headerText = GetTimeFrameName(num, "tr.Lkp_MS_QuarterYears", "QuarterYear");
            headerText = string.Format("{0} {1}", ma.GetTimeFrameName(Convert.ToInt32(num.ToString().Substring(4)), "quarter"), Convert.ToInt32(num.ToString().Substring(0, 4)));

        return headerText;
    }
    public static DataTable ToDataTable<T>(IList<T> data)
    {
        //PropertyDescriptorCollection props = data.GetProperties(); //TypeDescriptor.GetProperties(typeof(T));
        PropertyDescriptorCollection properties = null; ;
        PropertyDescriptor property;
        DataTable table = new DataTable();
        DataTable returnTable = new DataTable();

        foreach (object o in data)
        {
            if (table.Columns.Count <= 0)
            {
                properties = ((ICustomTypeDescriptor)o).GetProperties();
                property = properties["Drug1"];

                for (int i = 0; i < properties.Count; i++)
                {
                    PropertyDescriptor prop = properties[i];
                    table.Columns.Add(prop.Name, prop.PropertyType);
                    returnTable.Columns.Add(prop.Name, prop.PropertyType);
                }
            }
        }

        object[] values = new object[properties.Count];
        foreach (T item in data)
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = properties[i].GetValue(item);
            }
            table.Rows.Add(values);
        }
        string[] productlist = HttpContext.Current.Request.QueryString["Product_ID"].Split(',');

        for (int i = 1; i < productlist.Count(); i++)
        {
            returnTable.Columns.Add("Drug" + i + 1);
        }
        return table;
    }
}
