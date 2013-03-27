using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Data;
using PathfinderModel;
using PathfinderClientModel;
using System.Collections;
using Pinsonault.Application.MarketplaceAnalytics;
using Pinsonault.Data;
using System.Text;
using Telerik.Web.UI;
using System.Configuration;
using Pinsonault.Web;
using System.Runtime.Serialization.Json;
using System.IO;


public partial class marketplaceanalytics_all_formularyhistoryreporting_popup : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        GridView grid1 = (GridView)fhrGrid.FindControl("gridTemplate");
        IList<string> keys = new List<string>();
        NameValueCollection queryValues = new NameValueCollection(Request.QueryString);
        bool isMonth;
        string dataField;
        string tableName;        
        IEnumerable<GenericDataRecord> g = null;
        IList<int> timeFrameVals = new List<int>();

        //Hardcode specific queryValues for compatibility with 'GetData' function
        queryValues.Add("Rollup_Type", "4"); //Rollup by Account
        queryValues.Add("Geography_Type", "1"); //Geography is always 'US'
        queryValues.Add("Calendar_Rolling", "Calendar"); //Timeframe is always calendar based        

        //Add keys for query
        keys.Add("Product_ID");
        keys.Add("Drug_ID");
        keys.Add("Thera_ID");
        keys.Add("Geography_ID");

        string timeFrame = queryValues["Timeframe"];

        //Add year to keys
        //keys.Add("Data_Year");

        //If Med-D, timeframe is month based
        if (queryValues["Monthly_Quarterly"] == "M")
        {
            //The timeframe is month based
            isMonth = true;
            dataField = "Data_Year_Month";
            tableName = "MS_Monthly";
        }
        else
        {
            //The timeframe is quarter based
            isMonth = false;
            dataField = "Data_Year_Quarter";
            tableName = "MS_Quarterly";
        }

        MarketplaceAnalyticsProvider ma = new MarketplaceAnalyticsProvider();

        popupPlanName.Text = ma.GetPlanName(Convert.ToInt32(queryValues["Plan_ID"]));
        pnlColor.BackColor = Pinsonault.Data.Reports.ReportColors.FormularyHistoryReporting.GetColor(Convert.ToInt32(queryValues["Field_Index"]));

        if (!string.IsNullOrEmpty(timeFrame))
        {
            //Add time frame values to a list for processing
            timeFrameVals.Add(Convert.ToInt32(timeFrame));

            g = ma.GetFHData(timeFrameVals, isMonth, tableName, dataField, keys, queryValues);

            if (g.Count() > 0)
            {
                queryValues.Add("IsFormularyHistory", "true");
                ma.ProcessGrid(grid1, isMonth, g, timeFrameVals, queryValues);
            }      
        }

        string htmlColor = string.Format("#{0:X2}{1:X2}{2:X2}", Pinsonault.Data.Reports.ReportColors.FormularyHistoryReporting.GetColor(Convert.ToInt32(queryValues["Field_Index"])).R, Pinsonault.Data.Reports.ReportColors.FormularyHistoryReporting.GetColor(Convert.ToInt32(queryValues["Field_Index"])).G, Pinsonault.Data.Reports.ReportColors.FormularyHistoryReporting.GetColor(Convert.ToInt32(queryValues["Field_Index"])).B);

        Page.ClientScript.RegisterStartupScript(typeof(Page), "_fhpagevars", string.Format("setBGColor('{0}');", htmlColor), true);
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
    }   
}
