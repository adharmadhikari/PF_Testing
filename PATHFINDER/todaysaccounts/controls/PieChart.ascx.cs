using System.Collections.Generic;
using System.Linq;
using Dundas.Charting.WebControl;
using PathfinderModel;
using Pinsonault.Data.Reports;
using System.Data.Common;
using System.Web;
using System;
using System.Drawing;

public partial class todaysaccounts_controls_PieChart : System.Web.UI.UserControl
{
    public void ProcessChart(IEnumerable<DbDataRecord> data, string chartTitle, string region, string originalSection)
    {

        String SelectedSectionIDs = originalSection;
        Boolean AllSectionFlag = false;

        if (SelectedSectionIDs == "0")
            AllSectionFlag = true; //All section is selected.
        else
            AllSectionFlag = false; //All section is not selected.


        string[] selSectionIds = new string[] { };
        selSectionIds = new string[20];

        if (SelectedSectionIDs.IndexOf(",") > 0)
        {
            //Split the data by comma 
            selSectionIds = SelectedSectionIDs.ToString().Split(new Char[] { ',' });
        }
        else
        {
            selSectionIds[0] = SelectedSectionIDs;
        }

        Chart Chart1;
        Chart1 = piechartFCoverage1.HostedChart;

        //Legend for inner series
        Legend legendInner = new Legend();
        legendInner.Name = "legendInner";
        legendInner.Font = new Font("Arial", 6);
        legendInner.BackColor = Color.Transparent;
        legendInner.Docking = LegendDocking.Bottom;
        legendInner.LegendStyle = LegendStyle.Table;
        legendInner.TableStyle = LegendTableStyle.Wide;
        Chart1.Legends.Add(legendInner);
        Chart1.Legends["legendInner"].Alignment = StringAlignment.Center;

        Chart1.Series[0].Type = SeriesChartType.Pie;
        Chart1.Series[0]["PieDrawingStyle"] = "Concave";


        Chart1.Series[0]["PieLabelStyle"] = "Outside";
        Chart1.Series[0]["PieOutsideLabelAlignment"] = "Radial";
        Chart1.Series[0]["LabelsHorizontalLineSize"] = "1";
        Chart1.Series[0]["LabelsRadialLineSize"] = "1";
        Chart1.Series[0]["PieLineColor"] = "Black";


        int noColumn = 0;
        if (data.Count() > 0)
        {
            int index = 0;
            string sectionName;
            int drillDownSectionID = 0;
            string SectionID;//all section, by default

            double OtherFSLives = 0;
            Int32 iCnt = 0, iOtherCnt = 0;
            foreach (DbDataRecord record in data)
            {
                //If the dataset-section_id doesn't exists in the actual_selected_sections array
                //then consider it as "Other".
                 if ((Array.IndexOf(selSectionIds, record.GetInt32(record.GetOrdinal("Section_ID")).ToString()) == -1) && (AllSectionFlag != true))
                {
                    var s = record.GetValue(record.GetOrdinal("Total_Pharmacy_Percent"));

                    string percent = s.ToString();
                    if (!string.IsNullOrEmpty(percent) && Convert.ToDouble(string.Format("{0:0.00}", Convert.ToDouble(percent))) > 0)
                    {
                        //Add up formulary_lives to get the total lives for 'Other'
                        OtherFSLives = OtherFSLives + Convert.ToDouble(percent);
                        iOtherCnt++; //total number of sections considered as 'Other'
                    }
                }

                iCnt++;
            } //end of for loop

            foreach (DbDataRecord record in data)
            {
                Chart1.PaletteCustomColors = new System.Drawing.Color[data.Count()];

                SectionID = Request.QueryString["Section_ID"];

                sectionName = record.GetString(record.GetOrdinal("Section_Name"));
                drillDownSectionID = record.GetInt32(record.GetOrdinal("Section_ID"));

                chartTitle = "Lives Distribution";//string.Format("{0} - {1}", chartTitle, record.GetString(record.GetOrdinal("Section_Name")));

                var s = record.GetValue(record.GetOrdinal("Total_Pharmacy_Percent"));

                string percent = s.ToString();
                 //If the dataset-sectionid exists in the actual_selected_sections then plot the points on piechart.
                if ((Array.IndexOf(selSectionIds, Convert.ToString(drillDownSectionID)) != -1) || (AllSectionFlag))
                {
                    if (!string.IsNullOrEmpty(percent) && Convert.ToDouble(string.Format("{0:0.00}", Convert.ToDouble(percent))) > 0)
                    {

                        //For each section add a point
                        Chart1.Series[0].Points.AddY(percent);
                        Chart1.Series[0].Points[index].Name = sectionName;
                        Chart1.Series[0].Points[index].ToolTip = sectionName + ": " + string.Format("{0:0.00}%", Convert.ToDouble(percent));
                        Chart1.Series[0].Points[index].Label = sectionName + ":\n " + string.Format("{0:0.00}%", Convert.ToDouble(percent));
                        Chart1.Series[0].Points[index].LegendText = sectionName;
                        Chart1.Series[0].Points[index].ShowInLegend = true;

                        Chart1.Series[0].Points[index].ShowLabelAsValue = true;

                        Chart1.Series[0].Points[index].BorderWidth = 1;
                        Chart1.Series[0].Points[index].Color = ReportColors.StandardReports.GetColor(index);

                        noColumn++;
                        index++;
                    }
                }

            } //End of FOR loop

             ///////////Plot the graph for "Other" sections///////////////////////////////////////////////
            if (OtherFSLives > 0)
            {
                sectionName = "Other"; //Label it as 'Other'.
                var s = OtherFSLives;

                string percent = s.ToString();
                if (!string.IsNullOrEmpty(percent) && Convert.ToDouble(string.Format("{0:0.00}", Convert.ToDouble(percent))) > 0)
                {

                    //For each section add a point
                    Chart1.Series[0].Points.AddY(percent);
                    Chart1.Series[0].Points[index].Name = sectionName;
                    Chart1.Series[0].Points[index].ToolTip = sectionName + ": " + string.Format("{0:0.00}%", Convert.ToDouble(percent));
                    Chart1.Series[0].Points[index].Label = sectionName + ":\n " + string.Format("{0:0.00}%", Convert.ToDouble(percent));
                    Chart1.Series[0].Points[index].LegendText = sectionName;
                    Chart1.Series[0].Points[index].ShowInLegend = true;

                    Chart1.Series[0].Points[index].ShowLabelAsValue = true;

                    Chart1.Series[0].Points[index].BorderWidth = 1;
                    Chart1.Series[0].Points[index].Color = ReportColors.StandardReports.GetColor(index);

                    noColumn++;
                    index++;
                }
            }
            //////////////////////////////////////////////////////////////////////////////////////////////
        }
        else
            Chart1.Visible = false;


        Chart1.Titles[0].Text = string.Format("{0} - {1}", chartTitle, region);
        Chart1.Attributes["_title"] = string.Format("{0} - {1}", chartTitle, region);//for exporter
    }
}
