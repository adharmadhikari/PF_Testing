using System.Collections.Generic;
using System.Linq;
using Dundas.Charting.WebControl;
using PathfinderModel;
using Pinsonault.Data.Reports;
using System.Data.Common;
using Pinsonault.Web;
using System;
using System.Web.UI ;

public partial class standardreports_controls_formularycoveragechart : System.Web.UI.UserControl, IReportChart
{
    protected override void OnLoad(EventArgs e)
    {
        string SectionIDs = string.Empty;
        using (PathfinderEntities context = new PathfinderEntities())
        {
            int clientID = Pinsonault.Web.Session.ClientID;
            int applicationID = Identifiers.StandardReports;
           
            var channels = (from c in context.ClientApplicationAccessSet
                            join s in context.SectionSet on
                            c.SectionID equals s.ID
                            where c.ClientID == clientID
                            where c.ApplicationID == applicationID 
                            && (s.ID != 0)
                            orderby s.Name
                            select s).ToList().Select(s => string.Format("{0}", s.ID.ToString()));
            SectionIDs = string.Join(",", channels.ToArray());
        }

        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "_srpagevars", string.Format("var repSecIDs = '{0}';", SectionIDs), true);

        //this.pchartlink.HRef = string.Format("javascript:OpenPieChartViewer('{0}','{1}','{2}',{3},{4})", SectionIDs,null,null,500,400);
        
        //imgpchart.Src = "./content/images/chart.gif";
        base.OnLoad(e);
    }

    public void ProcessChart(IEnumerable<DbDataRecord> data, string chartTitle, string region)
    {
        string chart1 = "";
        Chart chart;

        IList<FormularyStatus> formularystatus = ReportPageLoader.FormularyStatus;

        if (string.Compare("US", region, true) == 0)
        {
            chart = chartNational.HostedChart;
            chartNational.Visible = true;
        }
        else
        {
            chart = chartRegional.HostedChart;
            chartRegional.Visible = true;
        }




        string drillDownRegion = region;

        //add the chart series dynamically
        foreach (FormularyStatus fstatus in formularystatus)
        {
            chart.Series.Add(fstatus.Name);
        }


        int noColumn = 0;
        if (data.Count() > 0)
        {
            int index = 0;
            int drugID;
            string drugName;
            string Drug_Name;
            int drillDownSectionID = 0;
            string SectionID;//all section, by default
            foreach (DbDataRecord record in data)
            {
                drugID = record.GetInt32(record.GetOrdinal("Drug_ID"));
                Drug_Name = record.GetString(record.GetOrdinal("Drug_Name"));
                if (!string.IsNullOrEmpty(Request.QueryString["Section_ID"]))
                {
                    SectionID = Request.QueryString["Section_ID"];
                    if (SectionID.Contains(","))
                    {
                        if (chart1 == "")
                            chart1 = string.Format("{0}", record.GetString(record.GetOrdinal("Section_Name")));
                        else
                            chart1 = string.Format("{0}, {1}", chart1, record.GetString(record.GetOrdinal("Section_Name")));

                        drugName = record.GetString(record.GetOrdinal("Section_Name"));
                        if (drugName == "PBM")
                            drugName = "PBM (Employer)";
                        drillDownSectionID = record.GetInt32(record.GetOrdinal("Section_ID"));
                        if (!chartTitle.Contains(record.GetString(record.GetOrdinal("Drug_Name"))))
                            chartTitle = string.Format("{0} - {1}", chartTitle, record.GetString(record.GetOrdinal("Drug_Name")));
                    }
                    else
                    {

                        if ((Request.QueryString["Section_ID"] == "17") && (Request.QueryString["Segment_ID"] == "8"))
                            chart1 = string.Format("{0} (LIS)", record.GetString(record.GetOrdinal("Section_Name")));
                        else
                            chart1 = string.Format("{0}", record.GetString(record.GetOrdinal("Section_Name")));
                        drillDownSectionID = record.GetInt32(record.GetOrdinal("Section_ID"));
                        drugName = record.GetString(record.GetOrdinal("Drug_Name"));
                        //if (!chartTitle.Contains(record.GetString(record.GetOrdinal("Section_Name"))))
                        //    chartTitle = string.Format("{0} - {1}", chartTitle, record.GetString(record.GetOrdinal("Section_Name")));

                    }
                }
                else
                {
                    drugName = record.GetString(record.GetOrdinal("Drug_Name"));
                    if (!string.IsNullOrEmpty(Request.QueryString["onlyPBM"]))
                        chart1 = "PBM";
                    else
                        chart1 = "All";
                }
                index = 0;
                chart.PaletteCustomColors = new System.Drawing.Color[formularystatus.Count];

                foreach (FormularyStatus fstatus in formularystatus)
                {
                    bool IsColumnVisible = false;
                    foreach (DbDataRecord recordCheck in data)
                    {
                        decimal statusPercent = recordCheck.GetDecimal(recordCheck.GetOrdinal(string.Format("{0}_Percent", fstatus.Abbr)));
                        if (statusPercent > 0) IsColumnVisible = true;
                    }
                    if (IsColumnVisible) //if all the records for a column are having value > 0 then add the column else don't add it
                    {
                        chart.PaletteCustomColors[index] = ReportColors.StandardReports.GetColor(fstatus.ID - 1);
                        addPoint(chart, drillDownRegion, drugID, drugName, fstatus.Name, fstatus.ID, record.GetInt32(record.GetOrdinal(fstatus.Abbr)), index, noColumn, drillDownSectionID);
                        
                    }
                    else
                    chart.Series[index].ShowInLegend = false;
                    index++;
                }
               
                    noColumn++;
                
            }
        }
        else

            chart.Visible = false;



        chart.Titles[0].Text = chartTitle;
        chart.Titles[1].Text = chart1;
        chart.Attributes["_title"] = chartTitle;//for exporter
    }

    void addPoint(Chart chart, string region, int drugID, string drugName, string formularyStatusName, int formularyStatusID, int lives, int index, int noColumn, int drillDownSectionID)
    {
        chart.Series[index].ShowInLegend = true;
        chart.Series[index].Points.AddY(lives / 1000000.0);
        chart.Series[index].CustomAttributes = "DrawingStyle=Cylinder";

        chart.Series[index].Name = formularyStatusName;
        chart.Series[index].Points[noColumn].AxisLabel = drugName;

        //assign colors for series
        chart.Series[index].Points[noColumn].Color = ReportColors.StandardReports.GetColor(formularyStatusID - 1);

        chart.Series[index].Points[noColumn].Href = string.Format("javascript:formularyCoverageDrilldown('{0}',{1},{2},'{3}','{4}',{5})", region, drugID, formularyStatusID, drugName, formularyStatusName, drillDownSectionID);
    }

}

