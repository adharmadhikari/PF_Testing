using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dundas.Charting.WebControl;
using System.Data;
using System.Collections;
using System.Data.Objects;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.Serialization.Json;
using PathfinderModel;
using Pathfinder;
using Pinsonault.Data;
using Pinsonault.Data.Reports;
using System.Data.Common;
using Pinsonault.Web;
using System.Web.UI;

public partial class standardreports_controls_FormularyStatusChart : UserControl, IReportChart
{
    public bool GeographicCoverage
    {
        get { return chartRegional.GeographicCoverage; }
        set
        {
            chartRegional.GeographicCoverage = value;
            chartNational.GeographicCoverage = value;
        }
    }

    IList<PathfinderModel.CoverageStatus> coverageStatus = null;

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

       // this.pchartlink.HRef = string.Format("javascript:OpenFSPieChartViewer('{0}','{1}','{2}',{3},{4})", SectionIDs, null, null, 500, 400);

       // imgpchart.Src = "./content/images/chart.gif";
        base.OnLoad(e);
    }

    #region IReportChart Members

    public void ProcessChart(IEnumerable<DbDataRecord> Data, string chartTitle, string region)
    {
        string chart1 = "";
        if ( coverageStatus == null )
        {
            using ( PathfinderModel.PathfinderEntities context = new PathfinderModel.PathfinderEntities() )
            {
                coverageStatus = context.CoverageStatusSet.OrderBy(cs => cs.ID).ToList();
            }
        }

        Chart chart = null;

        if ( string.Compare(region, "US", true) == 0 )
        {
            chartNational.Visible = true;
            chart = chartNational.HostedChart;
        }
        else
        {
            chartRegional.Visible = true;
            chart = chartRegional.HostedChart;
        }
        int index = 0;
        string livesPropName;
        double total;
        int lives;
        string label;
        int noColumn = 0;
        int drillDownSectionID = 0;

        if ( Data.Count() > 0 )
        {
            foreach ( var r in Data )
            {
                // .GetOrdinal returns position of data so should be interger
                int drugID = r.GetOrdinal("Drug_ID");
                int drugName = r.GetOrdinal("Drug_Name");

                int dID = Convert.ToInt32(r.GetValue(drugID));
                string dName;
                //string dName = Convert.ToString(r.GetValue(drugName));

                if (!string.IsNullOrEmpty(Request.QueryString["Section_ID"]))
                {
                    string Section_ID = Request.QueryString["Section_ID"];
                    string[] SID = Section_ID.Split(',');
                    if (SID.Length > 1)
                    {
                        if (chart1 == "")
                            chart1 = string.Format("{0}", r.GetString(r.GetOrdinal("Section_Name")));
                        else
                            chart1 = string.Format("{0}, {1}", chart1, r.GetString(r.GetOrdinal("Section_Name")));

                        dName = r.GetString(r.GetOrdinal("Section_Name"));
                        if (dName == "PBM")
                            dName = "PBM (Employer)";
                        drillDownSectionID = r.GetInt32(r.GetOrdinal("Section_ID"));
                        if (!chartTitle.Contains(r.GetString(r.GetOrdinal("Drug_Name"))))
                            chartTitle = string.Format("{0} - {1}", chartTitle, r.GetString(r.GetOrdinal("Drug_Name")));
                    }
                    else
                    {
                        if ((Request.QueryString["Section_ID"] == "17") && (Request.QueryString["Segment_ID"] == "8"))
                            chart1 = string.Format("{0} (LIS)", r.GetString(r.GetOrdinal("Section_Name")));
                        else
                            chart1 = string.Format("{0}", r.GetString(r.GetOrdinal("Section_Name")));
                        drillDownSectionID = r.GetInt32(r.GetOrdinal("Section_ID"));
                        dName = r.GetString(r.GetOrdinal("Drug_Name"));
                    }

                }
                else
                {
                    dName = r.GetString(r.GetOrdinal("Drug_Name"));
                    if (!string.IsNullOrEmpty(Request.QueryString["onlyPBM"]))
                        chart1 = "PBM";
                    else
                        chart1 = "All";
                }

                index = 0;

                int iF1_Lives = Convert.ToInt32(r.GetValue(r.GetOrdinal("F1_Lives")));
                int iF2_Lives = Convert.ToInt32(r.GetValue(r.GetOrdinal("F2_Lives")));
                int iF3_Lives = Convert.ToInt32(r.GetValue(r.GetOrdinal("F3_Lives")));
                int iF5_Lives = Convert.ToInt32(r.GetValue(r.GetOrdinal("F5_Lives")));
                //SL: 10/13/2010
                int iOther_Lives = 0;// Convert.ToInt32(r.GetValue(r.GetOrdinal("Other_Lives")));

                string coverageStatusLabel;
                IList<string> restrictionList = Pinsonault.Application.StandardReports.FormularyStatusQueryDefinition.GetRestrictionsFromRequest(Request.QueryString);
                string restrictions = string.Join(", ", restrictionList.ToArray());

                total = iF1_Lives + iF2_Lives + iF3_Lives + iOther_Lives + iF5_Lives;
                foreach (CoverageStatus cs in coverageStatus)
                {
                    //if (cs.ID > 3) continue;//TEMP WORKAROUND TO UNDO CHANGES FOR "OTHER" RESTRICTIONS

                    coverageStatusLabel = cs.Name;
                    if (cs.ID == 2)//with restriction
                        coverageStatusLabel = string.Format("{0} ({1})", coverageStatusLabel, restrictions);

                    if (cs.ID != 4)   // donot include Coverage_Status_ID=4(Not Available)
                        livesPropName = string.Format("F{0}_Lives", cs.ID);

                    else  //if Coverage_Status_ID=4 then use 'Other_Lives' field
                    {
                        livesPropName = "Other_Lives";
                   
                        // to get other restrictions

                        string[] restrictionOther = new[] { "PA", "QL", "ST" };
                        restrictionOther = restrictionOther.Except(restrictionList).ToArray();


                        if (restrictionOther.Length > 0)
                            coverageStatusLabel = string.Format("{0} ({1})", "Covered With Unselected Restrictions", string.Join(", ", restrictionOther));
                            //coverageStatusLabel = string.Format("Unselected Restrictions");
                        else
                            coverageStatusLabel = "N/A";
                    }

                    lives = Convert.ToInt32(r.GetValue(r.GetOrdinal(livesPropName)));
                    //lives = (int)fs.GetType().GetProperty(livesPropName).GetValue(fs, null);

                    if (lives > 0)
                        label = string.Format("{0:p}", lives / total);
                    else
                        label = "";
                     bool IsColumnVisible = false;
                     foreach (DbDataRecord record in Data)
            {
                if (cs.ID != 4)
                {
                    decimal statusPercent = Convert.ToDecimal(record.GetValue(record.GetOrdinal(string.Format("F{0}_Lives", cs.ID))));
                    if (statusPercent > 0) IsColumnVisible = true;
                }
                else
                {
                    decimal statusPercent = Convert.ToDecimal(record.GetValue(record.GetOrdinal(string.Format("Other_Lives"))));
                    if (statusPercent > 0) IsColumnVisible = true;
                }
                
            }
                     if (IsColumnVisible) //if all the records for a column are having value > 0 then add the column else don't add it
                     {
                         //addPoint(chart, dID, dName, cs.Name, CoveredWithRestrictionsColumnName, lives, index, label);
                         addPoint(chart, dID, dName, coverageStatusLabel, cs.ID, lives, index, label, region, drillDownSectionID);

                        
                     }
                     else
                     
                         chart.Series[index].ShowInLegend = false;
                         index++;
                     
                }
               // int iCWURLives = Convert.ToInt32(r.GetValue(r.GetOrdinal("F5_Lives")));
                //if (iF5_Lives > 0)
                //    label = string.Format("{0:p}", iF5_Lives / total);
                //    else
                //        label = "";
                //addPoint(chart, dID, dName, "Covered with Unselected Restrictions", 5, iF5_Lives, index, label, region);
                //index++;
                //hide the rest
                while ( index < 4 )
                {
                    chart.Series[index].ShowInLegend = false;
                    index++;
                }

                //noColumn++;
            }
        }
        else
        {
            chart.Visible = false;
        }
        chart.Titles[0].Text = chartTitle;
        chart.Titles[1].Text = chart1;
        chart.Attributes["_title"] = chartTitle; //for exporter
    }

    #endregion


    void addPoint(Chart chart, int drugID, string drugName, string coverageName, int coverageID, int lives, int index, string label, string region, int drillDownSectionID)
    {

        int pointIndex = chart.Series[index].Points.AddY(lives / 1000000.0);
        chart.Series[index].CustomAttributes = "DrawingStyle=Cylinder";
        chart.Series[index].Name = coverageName;
        //chart.Series[index].Points[pointIndex].Label = label;
        chart.Series[index].Points[pointIndex].AxisLabel = drugName;
        chart.Series[index].Points[pointIndex].Href = string.Format("javascript:gridFSDrilldown_setfilter({0}, '{1}', {2}, '{3}', '{4}',{5})", drugID, drugName, coverageID, coverageName, region ,drillDownSectionID);

    }
    
}

