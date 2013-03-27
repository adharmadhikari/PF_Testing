using System.Collections.Generic;
using System.Linq;
using Dundas.Charting.WebControl;
using PathfinderModel;
using Pinsonault.Data.Reports;
using System.Data.Common;
using Pinsonault.Web;
using System;
using System.Web.UI;

public partial class standardreports_controls_tiercoveragechart : System.Web.UI.UserControl, IReportChart
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

        //this.pchartlink.HRef = string.Format("javascript:OpenTCPieChartViewer('{0}','{1}','{2}',{3},{4})", SectionIDs, null, null, 500, 400);

        //imgpchart.Src = "./content/images/chart.gif";
        base.OnLoad(e);
    }
    
    public void ProcessChart(IEnumerable<DbDataRecord> data, string chartTitle, string region)
    {
        string chart1="";
        Chart chart;

        IList<Tier> tiers = ReportPageLoader.Tiers.Reverse().ToList();

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

        string drillDownRegion = region;

        int noColumn = 0;
        if ( data.Count() > 0 )
        {
            int index = 0;
            string id;

            int drugID;
            string drugName;
            int drillDownSectionID = 0; //all section, by default            

            foreach ( DbDataRecord record in data )
            {
                drugID = record.GetInt32(record.GetOrdinal("Drug_ID"));
                if (!string.IsNullOrEmpty(Request.QueryString["Section_ID"]))
                    drillDownSectionID = record.GetInt32(record.GetOrdinal("Section_ID"));
                
                //if section is present in querystring i.e. then replace drug name with section name and append drug name in chart title
                // because for multiple sections only one drug will be selected
                if (!string.IsNullOrEmpty(Request.QueryString["Section_ID"]) && Request.QueryString["Section_ID"].Contains(","))
                {
                    if (chart1=="")
                    chart1 = string.Format("{0}",record.GetString(record.GetOrdinal("Section_Name")));
                    else
                        chart1 = string.Format("{0}, {1}", chart1, record.GetString(record.GetOrdinal("Section_Name")));
                   
                    drugName = record.GetString(record.GetOrdinal("Section_Name"));
                    if (drugName == "PBM")
                        drugName = "PBM (Employer)";
                    if (!chartTitle.Contains(record.GetString(record.GetOrdinal("Drug_Name"))))
                        chartTitle = string.Format("{0} - {1}", chartTitle, record.GetString(record.GetOrdinal("Drug_Name")));
                }
                else
                {
                    drugName = record.GetString(record.GetOrdinal("Drug_Name"));
                    if (!string.IsNullOrEmpty(Request.QueryString["Section_ID"]))
                    {
                        if ((Request.QueryString["Section_ID"] == "17") && (Request.QueryString["Segment_ID"] == "8"))
                            chart1 = string.Format("{0} (LIS)", record.GetString(record.GetOrdinal("Section_Name")));
                        else
                            chart1 = string.Format("{0}", record.GetString(record.GetOrdinal("Section_Name")));
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Request.QueryString["onlyPBM"]))
                            chart1 = "PBM";
                        else
                            chart1 = "All";
                    }
                    }

                index = 0;
                chart.PaletteCustomColors = new System.Drawing.Color[tiers.Count];

                foreach (Tier tier in tiers)
                {
                    bool IsColumnVisible = false;
                    foreach (DbDataRecord recordCheck in data)
                    {
                        decimal tierPercent = recordCheck.GetDecimal(recordCheck.GetOrdinal(string.Format("T{0}", tier.ID)));
                        if (tierPercent > 0) IsColumnVisible = true;
                    }
                    if (IsColumnVisible) //if all the records for a column are having value > 0 then add the column else don't add it
                    {
                        id = string.Format("T{0}", tier.ID);

                        //Color fix
                        int tierID = tier.ID;
                        if (tierID == 20) //M Tier
                            tierID = 10;
                        if (tierID == 21) //S Tier
                            tierID = 9;
                        if (tierID == 0) //0 Tier
                            tierID = 8;

                        chart.PaletteCustomColors[index] = ReportColors.StandardReports.GetColor(tierID - 1);
                        if (options.Count == 0 || (options.ContainsKey(id) && options[id] == "true"))
                            addPoint(chart, drillDownRegion, drugID, drugName, tier.Name, tier.ID, record.GetInt32(record.GetOrdinal(id + "_Lives")), index++, noColumn, drillDownSectionID);
                    }
                }

                    //additional tiers 20 and 21 for M and S, so total 9 tiers
                    //hide the rest
                    while (index < 10)
                    {
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

    void addPoint(Chart chart, string region, int drugID, string drugName, string tierName, int tierID, int lives, int index, int noColumn, int drillDownSectionID)
    {
        chart.Series[index].ShowInLegend = true;
        chart.Series[index].Points.AddY(lives / 1000000.0);
        chart.Series[index].CustomAttributes = "DrawingStyle=Cylinder";
        chart.Series[index]["MaxPixelPointWidth"] = "45";
        chart.Series[index].Name = tierName;        
        chart.Series[index].Points[noColumn].AxisLabel = drugName;

        //Color fix
        int tierIDfix = tierID;
        if (tierIDfix == 20) //M Tier
            tierIDfix = 10;
        if (tierIDfix == 21) //S Tier
            tierIDfix = 9;
        if (tierIDfix == 0) //0 Tier
            tierIDfix = 8;
        chart.Series[index].Points[noColumn].Color = ReportColors.StandardReports.GetColor(tierIDfix - 1);
        chart.Series[index].Points[noColumn].Href = string.Format("javascript:tierCoverageDrilldown('{0}',{1},{2},'{3}','{4}',{5})", region, drugID, tierID, drugName, tierName, drillDownSectionID);
    }

}

