using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Dundas.Charting.WebControl;
using System.Collections.Specialized;
using System.Data;
using PathfinderModel;
using PathfinderClientModel;
using Pinsonault.Application.FormularyHistoryReporting;
using Pinsonault.Data;
using System.Data.SqlClient;
using System.Data.Common;
using Pinsonault.Data.Reports;
using System.Drawing;
using Pinsonault.Application.MarketplaceAnalytics;

public partial class custom_warner_formularyhistoryreporting_all_detailgrid : PageBase
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        NameValueCollection queryValues = new NameValueCollection(Request.QueryString);
        MarketplaceAnalyticsProvider ma = new MarketplaceAnalyticsProvider();

        string dataField = null;
        string tableName = null;

        string msdataField = null;
        string mstableName = null;

        IList<int> timeFrame = new List<int>();
        bool isMonth = false;

        IList<string> keys_drilldown = new List<string>();
     
        keys_drilldown.Add("Plan_ID");
        keys_drilldown.Add("Drug_ID");
        keys_drilldown.Add("Product_ID");
        keys_drilldown.Add("Product_Name");
        keys_drilldown.Add("Pinso_Formulary_ID");   //changed column name from Formulary_ID to Pinso_Formulary_ID  
        keys_drilldown.Add("Segment_ID");
        keys_drilldown.Add("Plan_Product_ID");

        IList<string> mskeys_drilldown = new List<string>();
        mskeys_drilldown.Add("Product_ID");
        mskeys_drilldown.Add("Product_Name");
        mskeys_drilldown.Add("Drug_ID");
        mskeys_drilldown.Add("Thera_ID");
        mskeys_drilldown.Add("Segment_ID");
        mskeys_drilldown.Add("Segment_Name");
        mskeys_drilldown.Add("Geography_ID");
        mskeys_drilldown.Add("Plan_ID");
      
        IList<string> keys_fhr = new List<string>();
        keys_fhr.Add("Plan_ID");
        keys_fhr.Add("Drug_ID");
        keys_fhr.Add("Product_ID");
        keys_fhr.Add("Pinso_Formulary_ID");   //changed column name from Formulary_ID to Pinso_Formulary_ID  
        keys_fhr.Add("Segment_ID");
        keys_fhr.Add("Plan_Product_ID");
        keys_fhr.Add("Geography_ID");
        keys_fhr.Add("Geography_Name");

        if (queryValues["Monthly_Quarterly"] == "M")
        {
            //The timeframe is month based
            isMonth = true;
          
            msdataField = "TimeFrame";
            dataField = "Data_Year_Month";

            mstableName = "MS_Monthly";
            tableName = "V_GetPlanProductFormularyByProduct";
        }
        else
        {
            //The timeframe is quarter based
            isMonth = false;
           
            msdataField = "TimeFrame";
            dataField = "Data_Year_Quarter";

            mstableName = "MS_Quarterly";
            tableName = "V_GetPlanProductFormularyByProduct";
        }

        FHXProvider fhx = new FHXProvider();

        string geographyID = queryValues["Geography_ID"];     

        timeFrame.Add(Convert.ToInt32(queryValues["TimeFrame1"]));
        timeFrame.Add(Convert.ToInt32(queryValues["TimeFrame2"]));

        //Check if page count is needed
        bool getPageCount = false;
        NameValueCollection n = null;
       
        IEnumerable<GenericDataRecord> g = null;
        if ((!string.IsNullOrEmpty(queryValues["RequestPageCount"])) && (Convert.ToBoolean(queryValues["RequestPageCount"]) == true))
        {
            n = new NameValueCollection(queryValues);
            getPageCount = true;
            n["PagingEnabled"] = "false";
        }
    
        //check if grid 1 is checked, show Volume drilldown; if 2 then show Rx Lives drilldown data
        if (string.Compare(queryValues["Selection_Clicked"], "1") == 0)
        {
            keys_drilldown.Add("Plan_Name");
            keys_drilldown.Add("Plan_State_ID");
            keys_drilldown.Add("Plan_Pharmacy_Lives");
            keys_drilldown.Add("Formulary_Name");
            keys_drilldown.Add("Formulary_Lives");
            keys_drilldown.Add("Drug_Name");
            keys_drilldown.Add("Plan_Classification_ID");

            g = fhx.GetTierOrCoverageDrilldownData(timeFrame, geographyID, tableName, dataField, keys_drilldown, queryValues);

            timeFrame.Clear();
            timeFrame.Add(Convert.ToInt32(queryValues["TimeFrame1"]));
            timeFrame.Add(Convert.ToInt32(queryValues["TimeFrame2"]));

            ProcessTierOrCoverageDrilldownGeid(detailedGrid, isMonth, g, timeFrame, queryValues);
            
            detailedGrid.Visible = true;
            detailedGridTrx.Visible = false;

            if (getPageCount)
                g = fhx.GetTierOrCoverageDrilldownData(timeFrame, geographyID, tableName, dataField, keys_drilldown, n);
           
            gridCount.Text = g.FirstOrDefault().GetValue(0).ToString();
        }        
         
        if (string.Compare(queryValues["Selection_Clicked"], "2") == 0)
        {            
            
            g = fhx.GetTierTRXDrilldownData(timeFrame, geographyID, tableName, dataField, keys_fhr, queryValues, mstableName, mskeys_drilldown,msdataField);

            timeFrame.Clear();
            timeFrame.Add(Convert.ToInt32(queryValues["TimeFrame1"]));
            timeFrame.Add(Convert.ToInt32(queryValues["TimeFrame2"]));

            ProcessTrxDrilldownGrid(detailedGridTrx, isMonth, g, timeFrame, queryValues);

            detailedGridTrx.Visible = true;
            detailedGrid.Visible = false;

            if (getPageCount)                
                g = fhx.GetTierTRXDrilldownData(timeFrame, geographyID, tableName, dataField, keys_fhr, n, mstableName, mskeys_drilldown, msdataField);   
           
            gridCount.Text = g.FirstOrDefault().GetValue(0).ToString();          
           
        }
       
       
    }
    /// <summary>
    /// for binding and creating columns for Tier or Coverage status drilldown
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="isMonth"></param>
    /// <param name="g"></param>
    /// <param name="timeFrameVals"></param>
    /// <param name="queryVals"></param>
    public void ProcessTierOrCoverageDrilldownGeid(GridView grid, bool isMonth, IEnumerable<GenericDataRecord> g, IList<int> timeFrameVals, NameValueCollection queryVals)
    {

        string drugName = g.FirstOrDefault().GetValue(g.FirstOrDefault().GetOrdinal("Product_Name")).ToString();

        BoundField boundCol;
        string colName = "Tier_Name";
        if (!string.IsNullOrEmpty(queryVals["Coverage_Status_ID"]))
        {
            colName = "Coverage_Status_Name";
        }

        for (int iCompareCol = 0; iCompareCol < 2; iCompareCol++)
        {
            boundCol = new BoundField();
            boundCol.DataField = string.Format("{0}{1}", colName, iCompareCol);
            boundCol.HeaderText = GetHeaderText(isMonth, timeFrameVals[iCompareCol], queryVals);//string.Format("{0}, {1}", GetHeaderText(isMonth, timeFrameVals[iCompareCol], queryVals), drugName);
            grid.Columns.Add(boundCol);
        }
        grid.DataSource = g;
        grid.DataBind();
    }

   /// <summary>
   /// for Trx drilldown grid
   /// </summary>
   /// <param name="grid"></param>
   /// <param name="isMonth"></param>
   /// <param name="g"></param>
   /// <param name="timeFrameVals"></param>
   /// <param name="queryVals"></param>
    public void ProcessTrxDrilldownGrid(GridView grid, bool isMonth, IEnumerable<GenericDataRecord> g, IList<int> timeFrameVals, NameValueCollection queryVals)
    {
        List<int> timeFrame = timeFrameVals.ToList();

        BoundField boundCol;

        string trxMst = queryVals["Trx_Mst"];

        if (string.IsNullOrEmpty(trxMst))
            trxMst = "Trx";
        string prevTimeFrame = GetHeaderText(isMonth, timeFrame[0], queryVals);
        string nextTimeFrame = GetHeaderText(isMonth, timeFrame[1], queryVals);

        //tier rename header by column index
        int istaticcolindex = 4;
        grid.Columns[istaticcolindex+1].HeaderText = string.Format("Tier ({0})", prevTimeFrame);
        grid.Columns[istaticcolindex+2].HeaderText = string.Format("Tier ({0})", nextTimeFrame);

        grid.Columns[istaticcolindex+3].HeaderText = string.Format("CoPay ({0})", prevTimeFrame);
        grid.Columns[istaticcolindex+4].HeaderText = string.Format("CoPay ({0})", nextTimeFrame);

        grid.Columns[istaticcolindex+5].HeaderText = string.Format("Restrictions ({0})", prevTimeFrame);
        grid.Columns[istaticcolindex + 6].HeaderText = string.Format("Restrictions ({0})", nextTimeFrame);

        for (int x = 0; x < timeFrame.Count; x++)
        {
            boundCol = new BoundField();
            boundCol.ItemStyle.CssClass = "alignRight";
            boundCol.DataField = string.Format("Product_{0}{1}", trxMst, x);
            boundCol.HeaderText = string.Format("{0}-{1}", trxMst, GetHeaderText(isMonth, timeFrame[x], queryVals));

            //Check if client/user has decimal formatting enabled
            if (HttpContext.Current.User.IsInRole("mpa_decimal"))
            {
                if ((string.Compare(trxMst, "trx", true) == 0) || (string.Compare(trxMst, "nrx", true) == 0))
                    boundCol.DataFormatString = "{0:N2}";
                else
                    boundCol.DataFormatString = "{0:N3}%";
            }
            else
            {
                if ((string.Compare(trxMst, "trx", true) == 0) || (string.Compare(trxMst, "nrx", true) == 0))
                    boundCol.DataFormatString = "{0:N0}";
                else
                    boundCol.DataFormatString = "{0:N0}%";
            }

            grid.Columns.Add(boundCol);
        }

        grid.DataSource = g;
        grid.DataBind();
    }

    protected void detailedGrid_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;

        //This replaces <td> with <th> and adds the scope attribute
        gv.UseAccessibleHeader = true;

        //This will add the <thead> and <tbody> elements
        GridViewRow hdr = gv.HeaderRow;

        if (hdr != null)
            gv.HeaderRow.TableSection = TableRowSection.TableHeader;
    }

    protected void detailedGrid_DataBound(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;        
    }

    protected string CheckRestriction(object PA, object QL, object ST)
    {
        IList<string> restrictions = new List<string>();

        if ((!string.IsNullOrEmpty(PA.ToString())))
            restrictions.Add("PA");
        if ((!string.IsNullOrEmpty(QL.ToString())))
            restrictions.Add("QL");
        if ((!string.IsNullOrEmpty(ST.ToString())))
            restrictions.Add("ST");

        string concatRestrictions = "";

        if (restrictions.Count > 0)
            concatRestrictions = string.Join(",", restrictions.ToArray());

        return concatRestrictions;
     
    }

    protected void detailedGridTrx_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;

        //This replaces <td> with <th> and adds the scope attribute
        gv.UseAccessibleHeader = true;

        //This will add the <thead> and <tbody> elements
        GridViewRow hdr = gv.HeaderRow;

        if (hdr != null)
            gv.HeaderRow.TableSection = TableRowSection.TableHeader;
    }

    protected void detailedGridTrx_DataBound(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;
        MarketplaceAnalyticsProvider ma = new MarketplaceAnalyticsProvider();

        //Specify column index that needs merging
        //ma.GroupRows(gv, 3);
        ma.GroupRows(gv, 2);
        ma.GroupRows(gv, 1);
        ma.GroupRows(gv, 0);
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

    
}
