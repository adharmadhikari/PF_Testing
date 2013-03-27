﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Data.Common;
using Dundas.Charting.WebControl;
using Telerik.Web.UI;
using Pathfinder;
using Pinsonault.Data;
using Pinsonault.Data.Reports;
using Pinsonault.Application.StandardReports;
using PathfinderModel;

public partial class standardreports_all_formularystatus : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //ReportPageLoader.LoadReport<PathfinderClientModel.ReportsFormularyStatus, FormularyStatusQueryDefinition>(Request.QueryString, formularystatuschart1, gridFDSummary);
        ReportPageLoader.LoadReport<PathfinderModel.ReportsFormularyStatusSummary, FormularyStatusQueryDefinition>(Request.QueryString, formularystatuschart1, gridFDSummary);
    }

    //public void ProcessGrid(Telerik.Web.UI.RadGrid grid, IList<DbDataRecord> FStatusFilter)
    //{
    //    using (PathfinderClientModel.PathfinderClientEntities clientContext = new PathfinderClientModel.PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
    //    {
    //        grid.MasterTableView.DataSource = FStatusFilter;
    //        grid.MasterTableView.DataBind();
    //    }
    //}


    //public void ProcessChart(Chart chart, string region, string CoveredWithRestrictionsColumnName, List<DbDataRecord> FStatusFilter)
    //{

    //    string chartTitle;

    //    using (PathfinderModel.PathfinderEntities context = new PathfinderModel.PathfinderEntities())
    //    {
    //        IList<PathfinderModel.CoverageStatus> coverageStatus = context.CoverageStatusSet.OrderBy(cs => cs.ID).ToList();

    //        if (region == "US")
    //        {
    //            chartTitle = Resources.Resource.Label_National;
               
    //        }
    //        else
    //        {
    //            if (context.StateSet.Count(s => s.ID == region) > 0)  //State
    //            {
    //                chartTitle = context.StateSet.FirstOrDefault(s => s.ID == region).Name;
    //            }
    //            else  //Territory
    //            {
    //                using (PathfinderClientModel.PathfinderClientEntities clientContext = new PathfinderClientModel.PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
    //                {
    //                    PathfinderClientModel.Territory territory = clientContext.TerritorySet.FirstOrDefault(t => t.ID == region);

    //                    chartTitle = territory != null ? territory.Name : "";

    //                }
    //            }

    //        }


    //        chart.Titles[0].Text = chartTitle;
    //        chart.Attributes["_title"] = chartTitle; //for exporter



    //        /////

    //        //int noColumn = 0;

    //        int index = 0;
    //        string livesPropName;
    //        double total;
    //        int lives;
    //        string label;

    //        // var queryCoverageStatusSet = context.CoverageStatusSet.OrderBy(cs => cs.ID);

    //        foreach (var r in FStatusFilter)
    //        {
    //            // .GetOrdinal returns position of data so should be interger
    //            int drugID = r.GetOrdinal("Drug_ID");
    //            int drugName = r.GetOrdinal("Drug_Name");

    //            int dID = Convert.ToInt32(r.GetValue(drugID));
    //            string dName = Convert.ToString(r.GetValue(drugName));

    //            index = 0;

    //            int iF1_Lives = Convert.ToInt32(r.GetValue(r.GetOrdinal("F1_Lives")));
    //            int iF2_Lives = Convert.ToInt32(r.GetValue(r.GetOrdinal("F2_Lives")));
    //            int iF3_Lives = Convert.ToInt32(r.GetValue(r.GetOrdinal("F3_Lives")));

    //            total = iF1_Lives + iF2_Lives + iF3_Lives;
    //            foreach (CoverageStatus cs in coverageStatus)
    //            {

    //                if (cs.ID != 4)   // donot include Coverage_Status_ID=4(Not Available)
    //                {
    //                    livesPropName = string.Format("F{0}_Lives", cs.ID);
    //                    lives = Convert.ToInt32(r.GetValue(r.GetOrdinal(livesPropName)));

    //                    //lives = (int)fs.GetType().GetProperty(livesPropName).GetValue(fs, null);

    //                    if (lives > 0)
    //                        label = string.Format("{0:p}", lives / total);
    //                    else
    //                        label = "";

    //                    //addPoint(chart, dID, dName, cs.Name, CoveredWithRestrictionsColumnName, lives, index, label);
    //                    addPoint(chart, dID, dName, cs.Name, cs.ID, lives, index, label);

    //                    index++;
    //                }
    //            }

    //            //hide the rest
    //            while (index < 3)
    //            {
    //                chart.Series[index].ShowInLegend = false;
    //                index++;
    //            }

    //            //noColumn++;
    //        }
    //    }

    //}


    //void addPoint(Chart Chart1, int drugID, string drugName, string coverageName, int coverageID, int lives, int index, string label)
    //{
    //    int pointIndex = Chart1.Series[index].Points.AddY(lives / 1000000.0);
    //    Chart1.Series[index].CustomAttributes = "DrawingStyle=Cylinder";
    //    Chart1.Series[index].Name = coverageName;
    //    Chart1.Series[index].Points[pointIndex].Label = label;
    //    Chart1.Series[index].Points[pointIndex].AxisLabel = drugName;
    //    Chart1.Series[index].Points[pointIndex].Href = string.Format("javascript:gridFSDrilldown_setfilter({0}, '{1}', {2}, '{3}')", drugID, drugName, coverageID, coverageName);
    //}
}