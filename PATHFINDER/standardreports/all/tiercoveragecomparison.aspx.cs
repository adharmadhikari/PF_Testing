using System;
using PathfinderClientModel;
using Pinsonault.Application.StandardReports;

//using System.Collections.Specialized;
//using System.Collections.Generic;
//using System.Data.Common;
//using System.Data.SqlClient;
//using Pinsonault.Data;


public partial class standardreports_all_tiercoveragecomparison : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ReportPageLoader.LoadReport<PathfinderModel.ReportsTierCoverage, TierCoverageQueryDefinition>(Request.QueryString, Tiercoveragechart1, tiercoveragedata1);
        
        //NameValueCollection queryValues = new NameValueCollection(Request.QueryString);
       
        //bool includeNational = true;

        //string chartTitle = Resources.Resource.Label_National;
        //IEnumerable<GenericDataRecord> listUS = null;
        //IEnumerable<GenericDataRecord> listRegion = null;

        //string val = queryValues["Geography_ID"];

        //bool regionalChart = false;
        //if (string.Compare(val, "US", true) != 0 && queryValues["Section_ID"] != "4")
        //{
        //    regionalChart = true;          
        //    listRegion = ReportPageLoader.GetTierCoverageData(val);
        //    chartTitle = ReportPageLoader.getChartTitle(val);
        //}      

        //listUS = ReportPageLoader.GetTierCoverageData("US"); //geography_id = 'US' for National        

        //if (Tiercoveragechart1 != null)
        //{
        //    if (includeNational || !regionalChart)
        //        Tiercoveragechart1.ProcessChart(listUS, Resources.Resource.Label_National, "US");

        //    if (regionalChart)
        //    {                
        //        Tiercoveragechart1.ProcessChart(listRegion, chartTitle, val);
        //    }
        //}

        //if (tiercoveragedata1 != null)
        //{
        //    if (includeNational || !regionalChart)
        //        tiercoveragedata1.ProcessGrid(listUS, Resources.Resource.Label_National, "US");

        //    if (regionalChart)
        //    {                
        //        tiercoveragedata1.ProcessGrid(listRegion, chartTitle, val);
        //    }
        //}
    }  
}
