using System.Collections.Generic;
using System.Linq;
using Dundas.Charting.WebControl;
using PathfinderModel;
using Pinsonault.Data.Reports;
using System.Data.Common;
using System.Web;
using System;
using System.Drawing;

public partial class standardreports_controls_TierCoveragePieChart : System.Web.UI.UserControl, IReportChart
{
    public void ProcessChart(IEnumerable<DbDataRecord> data, string chartTitle, string region)
    {
        String SelectedSectionIDs = HttpContext.Current.Request.QueryString["Selected_Section_ID"].ToString();
        Boolean AllSectionFlag = false;
        Boolean NoDataFlag = true;

        if (SelectedSectionIDs == "0")
            AllSectionFlag = true; //All section is selected.
        else
            AllSectionFlag = false; //All section is not selected.


        string[] selSectionIds = new string[] { };
        selSectionIds = new string[6];

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
        IList<Tier> tiers = ReportPageLoader.Tiers.ToList();

        Chart1 = piechartTierCoverage1.HostedChart;
        SeriesChartType ct = SeriesChartType.Pie;

        Chart1.ChartAreas.Clear();
        Chart1.Series.Clear();
        
        //Create the outer area
        Single outerSize = 60; //percent
        ChartArea outerArea = new ChartArea();
        outerArea.Name = "OUTER_AREA";
        //Set the plot position to the middle of the area depending on the size
        outerArea.InnerPlotPosition = new ElementPosition((100 - outerSize) / 2, (100 - outerSize) / 2, outerSize, outerSize);
        outerArea.BackColor = Color.Transparent;

        //Create the inner area
        Single innerSize = 80; //percent
        ChartArea innerArea = new ChartArea();
        innerArea.Name = "INNER_AREA";
        innerArea.InnerPlotPosition = new ElementPosition((100 - innerSize) / 2, (100 - innerSize) / 2, innerSize, innerSize);
        innerArea.BackColor = Color.Transparent;

        //Create the outer series
        Series outerSeries = new Series("OUTER_SERIES");
        outerSeries.ChartArea = "OUTER_AREA";
        outerSeries.Type = SeriesChartType.Doughnut;
        //outerSeries.Palette = ChartColorPalette.Pastel; //ChartColorPalette.SeaGreen;

        //Create the inner series
        Series innerSeries = new Series("INNER_SERIES");
        innerSeries.ChartArea = "INNER_AREA";
        innerSeries.Type =  ct;
        innerSeries.Palette = ChartColorPalette.Pastel;

        //Legend for outer series
        Legend legendOuter = new Legend();
        legendOuter.Name = "legendOuter";
        legendOuter.Font = new Font("Arial", 6);
        legendOuter.Position.Auto = false;
        legendOuter.Position.X = 95;
        legendOuter.Position.Y = 1;
        legendOuter.Position.Width = 9;
        legendOuter.Position.Height = 30;
        legendOuter.BackColor = Color.Transparent;
        legendOuter.Docking = LegendDocking.Right;
        legendOuter.LegendStyle = LegendStyle.Column;
        Chart1.Legends.Add(legendOuter);

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

        if (data.Count() > 0)
        {
            if (AllSectionFlag != true)
            {
                foreach (DbDataRecord record in data)
                {
                    //If the dataset-section_id doesn't exists in the actual_selected_sections array
                    //then consider it as "Other".
                    if (NoDataFlag == true)
                    {
                        if ((Array.IndexOf(selSectionIds, record.GetInt32(record.GetOrdinal("Section_ID")).ToString()) == -1))
                            NoDataFlag = true;
                        else
                            NoDataFlag = false;
                    }
                }
            }
            else
                NoDataFlag = false;

            if (NoDataFlag == false)
            {
                int index = 0;
                int innerSeriesColor = 0;
                int i = 0;
                int icolor = 0;
                int drugID;
                string drugName;
                string Drug_Name;
                String sTC = "";
                int drillDownSectionID = 0;
                string SectionID;//all section, by default
                double OtherFSLives = 0;
                double[] Other_Percents = new double[] { };
                Other_Percents = new double[tiers.Count];

                //Calculating formulary lives for plotting dummypoint---------------------------------------
                Double smFS = 0;
                Int32 iCnt = 0, iTC = 0, iOtherCnt = 0;
                Double dummyPointLives = 0;
                foreach (DbDataRecord record in data)
                {
                    if(iCnt == 0)
                        smFS = Convert.ToDouble(record.GetInt32(record.GetOrdinal("Formulary_Lives")));
                    else
                    {
                        if (smFS > Convert.ToDouble(record.GetInt32(record.GetOrdinal("Formulary_Lives"))))
                            smFS = Convert.ToDouble(record.GetInt32(record.GetOrdinal("Formulary_Lives")));
                    }

                    iTC = 0;
                    //If the dataset-section_id doesn't exists in the actual_selected_sections array
                    //then consider it as "Other".
                    if ((Array.IndexOf(selSectionIds, record.GetInt32(record.GetOrdinal("Section_ID")).ToString()) == -1) && (AllSectionFlag != true))
                    {
                        //Add up formulary_lives to get the total lives for 'Other'
                        OtherFSLives = OtherFSLives + Convert.ToDouble(record.GetInt32(record.GetOrdinal("Formulary_Lives")));
                        foreach (Tier tier in tiers)
                        {
                            sTC = string.Format("T{0}", tier.ID);

                            if (iOtherCnt == 0)
                                Other_Percents[iTC] = Convert.ToDouble(record[sTC]);
                            else
                                Other_Percents[iTC] = Other_Percents[iTC] + Convert.ToDouble(record[sTC]);

                            iTC++;
                        } //end of for each
                        iOtherCnt++; //total number of sections considered as 'Other'
                    }

                    iCnt++;
                } //end of for loop

                //if there are more sections 
                //then calculate dummypoint as 40% of the smallest lives.
                if (iCnt >= 4) 
                    dummyPointLives = Convert.ToDouble((smFS * 50) / 100);
                else //else 30% of the smallest lives.
                    dummyPointLives = Convert.ToDouble((smFS * 30) / 100);
                //--------------------------------------------------------------------------------------------

                foreach (DbDataRecord record in data)
                {
                    drugID = record.GetInt32(record.GetOrdinal("Drug_ID"));
                    Drug_Name = record.GetString(record.GetOrdinal("Drug_Name"));
                    if (!string.IsNullOrEmpty(Request.QueryString["Section_ID"]))
                    {
                        SectionID = Request.QueryString["Section_ID"];
                        if (SectionID.Contains(","))
                        {
                            drugName = record.GetString(record.GetOrdinal("Section_Name"));
                            drillDownSectionID = record.GetInt32(record.GetOrdinal("Section_ID"));
                            if (!chartTitle.Contains(record.GetString(record.GetOrdinal("Drug_Name"))))
                                chartTitle = string.Format("{0} - {1}", chartTitle, record.GetString(record.GetOrdinal("Drug_Name")));
                        }
                        else
                        {
                            drillDownSectionID = record.GetInt32(record.GetOrdinal("Section_ID"));
                            drugName = record.GetString(record.GetOrdinal("Drug_Name"));
                            if (!chartTitle.Contains(record.GetString(record.GetOrdinal("Section_Name"))))
                                chartTitle = string.Format("{0} - {1}", chartTitle, record.GetString(record.GetOrdinal("Section_Name")));

                        }
                    }
                    else
                        drugName = record.GetString(record.GetOrdinal("Drug_Name"));

                    //If the dataset-sectionid exists in the actual_selected_sections then plot the points on piechart.
                    if ((Array.IndexOf(selSectionIds, Convert.ToString(drillDownSectionID)) != -1) || (AllSectionFlag))
                    {
                        if (iCnt > 1) //if there are more than one points in the pie chart then only add a dummy point.
                        {
                            //Add a dummy section point everytime to show the explode effect
                            innerSeries.Points.AddY(Convert.ToDouble(dummyPointLives));
                            innerSeries.Points[index].Name = "DummySection" + Convert.ToString(index);
                            innerSeries.Points[index].Color = Color.White;
                            innerSeries.Points[index].ShowInLegend = false;
                            innerSeries.Points[index].ShowLabelAsValue = false;
                            innerSeries.Points[index].BorderColor = Color.White;
                            innerSeries.Points[index].BorderWidth = 0;
                            index++;
                            //---------------------------------------------------------
                        }

                        //For each section add a point
                        innerSeries.Points.AddY(Convert.ToDouble(record.GetInt32(record.GetOrdinal("Formulary_Lives"))));
                        innerSeries.Points[index].Name = drugName;
                        innerSeries.Points[index].ToolTip = drugName + ": " + String.Format("{0:N0}", Convert.ToDouble(record.GetInt32(record.GetOrdinal("Formulary_Lives"))));
                        innerSeries.Points[index].LegendText = drugName;
                        innerSeries.Points[index].ShowInLegend = true;
                        innerSeries.Points[index].ShowLabelAsValue = true;
                        innerSeries.Points[index].Label = drugName + ": " + String.Format("{0:N0}", Convert.ToDouble(record.GetInt32(record.GetOrdinal("Formulary_Lives"))));
                        innerSeries.Points[index].BorderWidth = 1;
                        innerSeries.Points[index].BorderColor = Color.Black;
                        innerSeries.Points[index].Color = ReportColors.StandardReportsPieLight.GetColor(innerSeriesColor);
                        
                        icolor = 0; //Datapoint color counter
                        foreach (Tier tier in tiers)
                        {
                            //add a dummy formulary status point with white color 
                            //to distinguish between each section's formulary statues.
                            if (icolor == 0)
                            {
                                if (iCnt > 1) //if there are more than one points in the pie chart then only add a dummy point.
                                {
                                    outerSeries.Points.AddY((Convert.ToDouble(dummyPointLives) * Convert.ToDouble(100)) / 100);
                                    outerSeries.Points[i].Name = "DummyFS" + Convert.ToString(i);
                                    outerSeries.Points[i].Color = Color.White;
                                    outerSeries.Points[i].ShowInLegend = false;
                                    outerSeries.Points[i].BorderColor = Color.White;
                                    outerSeries.Points[i].BorderWidth = 0;
                                    i++;
                                }
                            }
                            //-----------------------------------------------------------

                            //Add each formulary status datapoint per section.
                            sTC = string.Format("T{0}", tier.ID);
                            outerSeries.Points.AddY((Convert.ToDouble(record.GetInt32(record.GetOrdinal("Formulary_Lives"))) * Convert.ToDouble(record[sTC])) / 100);
                            outerSeries.Points[i].Name = tier.Name;
                            outerSeries.Points[i].Color = ReportColors.StandardReportsPieDark.GetColor(icolor);
                           
                            //Show percentages on tooltip.
                            outerSeries.Points[i].ToolTip = drugName + ", " + tier.Name + ": " + String.Format("{0:0.#}", Convert.ToDouble(record[sTC])) + "%";

                            //show the legend and label for formulary status only for the firsttime.
                            if (index <= 1)
                            {
                                outerSeries.Points[i].LegendText = tier.Name;
                                outerSeries.Points[index].Label = tier.Name;
                            }
                            else
                            {
                                //Hide the legend and Label formulary status for others.
                                outerSeries.Points[i].ShowInLegend = false;
                                outerSeries.Points[index].ShowLabelAsValue = false;
                            }

                            outerSeries.Points[i].BorderWidth = 1;
                            outerSeries.Points[i].BorderColor = Color.Black;

                            i++;
                            icolor++; 

                        } //end of for loop
                        index++;
                        innerSeriesColor++;
                    } //End of if loop
                } //End of FOR loop


                ///////////Plot the graph for "Other" sections///////////////////////////////////////////////
                if (OtherFSLives > 0)
                {
                    //calculate the percentage based on total number of 'Other' sections.
                    Int32 percentCalc = (100 * iOtherCnt);

                    iTC = 0;
                    drugName = "Other"; //Label it as 'Other'.

                    //Add a dummy section point everytime to show the explode effect
                    innerSeries.Points.AddY(Convert.ToDouble(dummyPointLives));
                    innerSeries.Points[index].Name = "DummySection" + Convert.ToString(index);
                    innerSeries.Points[index].Color = Color.White;
                    innerSeries.Points[index].ShowInLegend = false;
                    innerSeries.Points[index].ShowLabelAsValue = false;
                    innerSeries.Points[index].BorderColor = Color.White;
                    innerSeries.Points[index].BorderWidth = 0;
                    index++;
                    //---------------------------------------------------------

                    //For each section add a point
                    innerSeries.Points.AddY(OtherFSLives);
                    innerSeries.Points[index].Name = drugName;
                    innerSeries.Points[index].ToolTip = drugName + ": " + String.Format("{0:N0}", OtherFSLives);
                    innerSeries.Points[index].LegendText = drugName;
                    innerSeries.Points[index].ShowInLegend = true;
                    innerSeries.Points[index].ShowLabelAsValue = true;
                    innerSeries.Points[index].Label = drugName + ": " + String.Format("{0:N0}", OtherFSLives);
                    innerSeries.Points[index].BorderWidth = 1;
                    innerSeries.Points[index].BorderColor = Color.Black;
                    innerSeries.Points[index].Color = ReportColors.StandardReportsPieLight.GetColor(innerSeriesColor);

                    icolor = 0; //Datapoint color counter
                    foreach (Tier tier in tiers)
                    {
                        //add a dummy formulary status point with white color 
                        //to distinguish between each section's formulary statues.
                        if (icolor == 0)
                        {
                            outerSeries.Points.AddY((Convert.ToDouble(dummyPointLives) * Convert.ToDouble(100)) / 100);
                            outerSeries.Points[i].Name = "DummyFS" + Convert.ToString(i);
                            outerSeries.Points[i].Color = Color.White;
                            outerSeries.Points[i].ShowInLegend = false;
                            outerSeries.Points[i].BorderColor = Color.White;
                            outerSeries.Points[i].BorderWidth = 0;
                            i++;
                        }
                        //-----------------------------------------------------------

                        //Add each formulary status datapoint per section.

                        //Calculate the percentage for each tier coverage point in 100.
                        double OtherPercentin100 = (Convert.ToDouble(Other_Percents[iTC] * 100) / percentCalc);

                        sTC = string.Format("T{0}", tier.ID);
                        outerSeries.Points.AddY(Convert.ToDouble(OtherFSLives * OtherPercentin100) / 100);
                        outerSeries.Points[i].Name = tier.Name;
                        outerSeries.Points[i].Color = ReportColors.StandardReportsPieDark.GetColor(icolor);

                        //Show percentages on tooltip.
                        outerSeries.Points[i].ToolTip = drugName + ", " + tier.Name + ": " + String.Format("{0:0.#}", OtherPercentin100) + "%";

                        //show the legend and label for formulary status only for the firsttime.
                        if (index == 1)
                        {
                            outerSeries.Points[i].LegendText = tier.Name;
                            outerSeries.Points[index].Label = tier.Name;
                        }
                        else
                        {
                            //Hide the legend and Label formulary status for others.
                            outerSeries.Points[i].ShowInLegend = false;
                            outerSeries.Points[index].ShowLabelAsValue = false;
                        }

                        outerSeries.Points[i].BorderWidth = 1;
                        outerSeries.Points[i].BorderColor = Color.Black;

                        i++;
                        icolor++;

                        iTC++;
                    } //end of inner for loop
                    index++;
                    innerSeriesColor++;
                } //end of if 
                ////////////////////////////////////////////////////////////////////////

                //Add the formulary status series to the chart
                Chart1.Series.Add(outerSeries);
                Chart1.Series["OUTER_SERIES"].ChartArea = "OUTER_AREA";

                //Add section series
                Chart1.Series.Add(innerSeries);
                Chart1.Series["INNER_SERIES"].ChartArea = "INNER_AREA";

                Chart1.ChartAreas.Add(outerArea);
                Chart1.ChartAreas.Add(innerArea);

                Chart1.Titles[0].Text = chartTitle;
                Chart1.Attributes["_title"] = chartTitle;//for exporter

                //Format section labels for inner series.
                Chart1.Series["OUTER_SERIES"]["PieLabelStyle"] = "Disabled";
                Chart1.Series["INNER_SERIES"]["PieLabelStyle"] = "Outside";
                Chart1.Series["INNER_SERIES"]["PieOutsideLabelAlignment"] = "Radial";
                Chart1.Series["INNER_SERIES"]["LabelsHorizontalLineSize"] = "2";
                Chart1.Series["INNER_SERIES"]["LabelsRadialLineSize"] = "2";
                Chart1.Series["INNER_SERIES"]["PieLineColor"] = "Black";

                //Add custom legends to series.
                Chart1.Series["INNER_SERIES"].Legend = "legendInner";
                Chart1.Series["OUTER_SERIES"].Legend = "legendOuter";

                //Disable default Legend.
                Chart1.Legend.Enabled = false;

                //-----------Arrange the chart areas as grouped pie chart-------
                Chart1.ChartAreas["INNER_AREA"].BackColor = Color.Transparent;
                Chart1.ChartAreas["INNER_AREA"].Position.X = 25;
                Chart1.ChartAreas["INNER_AREA"].Position.Y = 25;
                Chart1.ChartAreas["INNER_AREA"].Position.Width = 50;
                Chart1.ChartAreas["INNER_AREA"].Position.Height = 50;

                Chart1.ChartAreas["INNER_AREA"].AlignWithChartArea = "OUTER_AREA";
                Chart1.ChartAreas["OUTER_AREA"].Position.X = 5;
                Chart1.ChartAreas["OUTER_AREA"].Position.Y = 5;
                Chart1.ChartAreas["OUTER_AREA"].Position.Width = 90;
                Chart1.ChartAreas["OUTER_AREA"].Position.Height = 90;

                Chart1.Series["INNER_SERIES"].BorderColor = Color.Black;
                Chart1.Series["OUTER_SERIES"].BorderColor = Color.Black;
                Chart1.Series["OUTER_SERIES"]["DoughnutRadius"] = "40";
                //---------------------------------------------------------------
            } //End of nodataflag checking if
            else
                Chart1.Visible = false; //if no data then don't display the chart.
        }
        else
            Chart1.Visible = false;


        
    }
}
