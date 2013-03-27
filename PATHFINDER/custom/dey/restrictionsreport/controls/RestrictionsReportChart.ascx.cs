using System.Collections.Generic;
using System.Linq;
using Dundas.Charting.WebControl;
using PathfinderModel;
using Pinsonault.Data.Reports;
using System.Data.Common;
using Pinsonault.Web;
using System.Collections.Specialized;
using System;

public partial class restrictionsreport_controls_MedicalPharmacyCoverageChart : System.Web.UI.UserControl, IReportChart
{
    public void ProcessChart(IEnumerable<DbDataRecord> data, string chartTitle, string region)
    {
        Chart chart;

        //IList<Tier> tiers = ReportPageLoader.Tiers.Reverse().ToList();

        if ( string.Compare("US", region, true) == 0 )
        {
            chart = chartNational.HostedChart;
            chartNational.Visible = true;
        }
        else
        {
            chart = chartRegional.HostedChart;
            chartRegional.Visible = true;
        }

        Dictionary<string, string> options = new Dictionary<string, string>();
        string opts = Request.QueryString["__options"];

        if ( !string.IsNullOrEmpty(opts) )
        {
            string[] a = opts.Trim('{', '}').Split(',');
            string[] vals;
            string name;
            foreach ( string s in a )
            {
                vals = s.Split(':');
                name = vals[0].Trim('"').ToUpper();
                if ( string.Compare(name, "T0", true) != 0 )
                    options.Add(name, vals[1].Trim('"').ToLower());
            }
        }

        chart.Titles[0].Text = chartTitle;
        chart.Attributes["_title"] = chartTitle;//for exporter

        string drillDownRegion = region;

        int noColumn = 0;
        if ( data.Count() > 0 )
        {
            int index = 0;
            string id = string.Empty;

            int drugID;
            string drugName;
            int sectionID;
            string sectionName;

            using (PathfinderEntities context = new PathfinderEntities())
            {
                //Get all restriction criteria
                var q = (from p in context.LkpRestrictionCriteriaCombinationsSet
                               where p.Client_ID == Pinsonault.Web.Session.ClientID
                         select new { p.Criteria_Id, p.Criteria_Name }).Distinct().AsEnumerable().OrderByDescending(p => p.Criteria_Id);

                NameValueCollection nvc = new NameValueCollection(Request.QueryString);

                //If multiple sections selected, display chart by Section, otherwise display by drug
                bool multipleSections = false;

                if (nvc["Section_ID"].Contains(','))
                    multipleSections = true;

                foreach ( DbDataRecord record in data )
                {
                    drugID = record.GetInt32(record.GetOrdinal("Drug_ID"));
                    drugName = record.GetString(record.GetOrdinal("Drug_Name"));
                    sectionID = record.GetInt32(record.GetOrdinal("Section_ID"));
                    sectionName = record.GetString(record.GetOrdinal("Section_Name"));

                    index = 0;
                    chart.PaletteCustomColors = new System.Drawing.Color[q.Count()];

                    foreach (var r in q)
                    {
                        id = string.Format("P{0}",r.Criteria_Id.ToString());

                        chart.PaletteCustomColors[index] = ReportColors.StandardReports.GetColor(r.Criteria_Id - 1);

                        addPoint(chart, drillDownRegion, drugID, drugName, r.Criteria_Name, r.Criteria_Id, record.GetInt32(record.GetOrdinal(id + "_Lives")), index++, noColumn, multipleSections, sectionID, sectionName);
                    }

                    //hide the rest
                    while (index < 4)
                    {
                        chart.Series[index].ShowInLegend = false;
                        index++;
                    }

                    noColumn++;
                 
                }
            }
        }
        else
            chart.Visible = false;

    }

    void addPoint(Chart chart, string region, int drugID, string drugName, string criteriaName, int criteriaID, int lives, int index, int noColumn, bool multipleSections, int sectionID, string sectionName)
    {
        chart.Series[index].ShowInLegend = true;

        chart.Series[index].YAxisType = AxisType.Primary;
        chart.Series[index].Points.AddY(lives / 1000000.0);
        chart.Series[index].Name = criteriaName;

        chart.Series[index].CustomAttributes = "DrawingStyle=Cylinder";
        chart.Series[index]["MaxPixelPointWidth"] = "45";
        
        //chart.Series[index].Points[noColumn].ToolTip = chart.Series[index].Name + ": " + String.Format("{0:N0}", lives);
        string axisLabel = string.Empty;

        if (multipleSections)
            axisLabel = sectionName;
        else
            axisLabel = drugName;

        chart.Series[index].Points[noColumn].AxisLabel = axisLabel;        

        chart.Series[index].Points[noColumn].Color = ReportColors.StandardReports.GetColor(criteriaID - 1);
        chart.Series[index].Points[noColumn].Href = string.Format("javascript:restrictionsReportDrilldown('{0}',{1},{2},'{3}','{4}',{5},'{6}','{7}')", region, drugID, criteriaID, drugName, criteriaName, sectionID, sectionName, multipleSections);
    }

}

