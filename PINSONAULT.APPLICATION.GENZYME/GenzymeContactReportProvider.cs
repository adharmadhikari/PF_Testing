
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dundas.Charting.WebControl;
using System.Collections.Specialized;
using System.Data.Common;
using Pinsonault.Data.Reports;

namespace Pinsonault.Application.Genzyme
{
    public class GenzymeContactReportProvider
    {
        //Used for Meeting Type and Meeting Activity report
        public void ProcessGenzymeChart(Chart chart, List<DbDataRecord> meetingActivityFilter, int prodID, string reportType, int chartNum)
        {
            using (PathfinderGenzymeEntities clientContext = new PathfinderGenzymeEntities())
            {
                var drugInfo = meetingActivityFilter.FirstOrDefault();
                int drugOrdinal = drugInfo.GetOrdinal("Drug_Name");
                string drugNameText = drugInfo.GetValue(drugOrdinal).ToString();

                if (chartNum < 4) //Non national chart
                {
                    chart.Titles[0].Text = drugNameText;
                    chart.Attributes["_title"] = drugNameText;//for exporter
                }
                else //National chart
                {
                    chart.Titles[0].Text = string.Format("{0} - National", drugNameText);
                    chart.Attributes["_title"] = string.Format("{0} - National", drugNameText); ;//for exporter
                }

                if (chartNum == 1) //Product 1
                    chart.Titles[0].Color = System.Drawing.Color.LightSeaGreen;
                else if (chartNum == 2) //Product 2
                    chart.Titles[0].Color = System.Drawing.Color.Orange;
                else if (chartNum == 3) //Product 3
                    chart.Titles[0].Color = System.Drawing.Color.YellowGreen;
                else if (chartNum == 4) //Product 1 National Comparison
                    chart.Titles[0].Color = System.Drawing.Color.DarkSeaGreen;
                else if (chartNum == 5) //Product 2 National Comparison
                    chart.Titles[0].Color = System.Drawing.Color.SandyBrown;
                else if (chartNum == 6) //Product 3 National Comparison
                    chart.Titles[0].Color = System.Drawing.Color.SeaGreen;

                int noColumn = 0;
                int index = 0;

                foreach (var y in meetingActivityFilter)
                {
                    //Locate index of all fields by ordinals
                    int mtgIDOrdinal = 0;
                    int mtgNameOrdinal = 0;

                    if (reportType == "meetingactivity")
                    {
                        mtgIDOrdinal = y.GetOrdinal("Meeting_Activity_ID");
                        mtgNameOrdinal = y.GetOrdinal("Meeting_Activity_Name");
                    }
                    else if (reportType == "meetingtype")
                    {
                        mtgIDOrdinal = y.GetOrdinal("Meeting_Type_ID");
                        mtgNameOrdinal = y.GetOrdinal("Meeting_Type_Name");
                    }

                    int numOfCallsOrdinal = y.GetOrdinal("RecordCount");
                    int percentOfCallsOrdinal = y.GetOrdinal("User_ID_Percent");

                    chart.PaletteCustomColors = new System.Drawing.Color[meetingActivityFilter.Count()];

                    //Call addCRPoint and get values of fields by index
                    addCRPoint(chart, prodID, drugNameText, y.GetValue(mtgIDOrdinal), y.GetValue(mtgNameOrdinal),
                        y.GetValue(numOfCallsOrdinal), y.GetValue(percentOfCallsOrdinal), index, noColumn, reportType);

                    noColumn++;

                    //Fix for 1 point bug
                    if (meetingActivityFilter.Count == 1)
                    {
                        chart.Series[index].Points.AddY(y.GetValue(numOfCallsOrdinal));
                        chart.Series[index].Points[noColumn].Color = ReportColors.CustomerContactReports.GetColor(Convert.ToInt32(y.GetValue(mtgIDOrdinal)) - 1);
                        chart.Series[index].Points[noColumn].Href = string.Format("javascript:ccr{0}Drilldown({1},{2},{3},'{4}')", reportType, prodID, y.GetValue(mtgIDOrdinal), (noColumn - 1), chart.ClientID);
                    }
                }

                //chart.Serializer.SerializableContent = "*.*";
                //chart.Serializer.Save("C:\\chart.xml");

            }
        }

        //Used for Products Discussed report
        public void ProcessGenzymeChart(Chart chart, IList<DbDataRecord> meetingActivityFilter, string[] contactDate, int chartNum)
        {
            using (PathfinderGenzymeEntities clientContext = new PathfinderGenzymeEntities())
            {
                string chartName = string.Format("{0} to {1}", contactDate[0], contactDate[1]);
                chart.Titles[0].Text = chartName;
                chart.Attributes["_title"] = chartName;//for exporter

                if (chartNum == 1)
                    chart.Titles[0].Color = System.Drawing.Color.LightSeaGreen;
                else if (chartNum == 2)
                    chart.Titles[0].Color = System.Drawing.Color.Orange;

                int noColumn = 0;
                int index = 0;

                foreach (var y in meetingActivityFilter)
                {
                    //Locate index of all fields by ordinals
                    int prodIDOrdinal = y.GetOrdinal("Products_Discussed_ID");
                    int prodNameOrdinal = y.GetOrdinal("Drug_Name");
                    int numOfCallsOrdinal = y.GetOrdinal("RecordCount");
                    int percentOfCallsOrdinal = y.GetOrdinal("User_ID_Percent");

                    chart.PaletteCustomColors = new System.Drawing.Color[meetingActivityFilter.Count()];

                    //Call addCRPoint and get values of fields by index
                    addCRPoint(chart, y.GetValue(prodIDOrdinal), y.GetValue(prodNameOrdinal), y.GetValue(numOfCallsOrdinal),
                        y.GetValue(percentOfCallsOrdinal), index, noColumn);

                    noColumn++;

                    //Fix for 1 point bug
                    if (meetingActivityFilter.Count == 1)
                    {
                        chart.Series[index].Points.AddY(y.GetValue(numOfCallsOrdinal));
                        chart.Series[index].Points[noColumn].Color = ReportColors.CustomerContactReports.GetColor(Convert.ToInt32(y.GetValue(prodIDOrdinal)) - 1);
                        chart.Series[index].Points[noColumn].Href = string.Format("javascript:ccrProductsDiscussedDrilldown({0},{1},'{2}')", y.GetValue(prodIDOrdinal), (noColumn - 1), chart.ClientID);
                    }
                }
            }
        }

        //Used for Meeting Type and Meeting Activity report
        void addCRPoint(Chart chart, Int32 productId, string drugNameText, object meetingID, object meetingName, object Number_Of_Calls, object Percent_Of_Calls, int index, int noColumn, string reportType)
        {
            chart.Series[index].Type = SeriesChartType.Pie;
            //chart.Series[index].ShowInLegend = true;
            chart.Series[index].ShowInLegend = false;
            chart.Series[index].Points.AddY(Number_Of_Calls);
            chart.Series[index].Name = drugNameText;
            chart.Series[index]["PieDrawingStyle"] = "Concave";

            //switch (HttpContext.Current.Request.QueryString["dataFormat"])
            //{ //REM: Chart displays percentage
            //    case "1":
            //        chart.Series[index].Points[noColumn].AxisLabel = String.Format("{0:P}", Percent_Of_Calls);
            //        break;
            //    //REM: Chart displays numbers
            //    case "2":
            //        chart.Series[index].Points[noColumn].AxisLabel = Number_Of_Calls.ToString();
            //        break;
            //    default:
            //        chart.Series[index].Points[noColumn].AxisLabel = Number_Of_Calls.ToString();
            //        break;
            //}

            //chart.Series[index].Points[noColumn].LegendText = meetingName.ToString();

            chart.Series[index].Points[noColumn].Color = ReportColors.CustomerContactReports.GetColor(Convert.ToInt32(meetingID) - 1);

            chart.Series[index].Points[noColumn].Href = string.Format("javascript:ccr{0}Drilldown({1},{2},{3},'{4}')", reportType, productId, meetingID, noColumn, chart.ClientID);
        }

        //Used for Products Discussed report
        void addCRPoint(Chart chart, object productId, object productName, object Number_Of_Calls, object Percent_Of_Calls, int index, int noColumn)
        {
            chart.Series[index].Type = SeriesChartType.Pie;
            //chart.Series[index].ShowInLegend = true;
            chart.Series[index].ShowInLegend = false;
            chart.Series[index].Points.AddY(Number_Of_Calls);
            chart.Series[index].Name = productName.ToString();
            chart.Series[index]["PieDrawingStyle"] = "Concave";

            //switch (HttpContext.Current.Request.QueryString["dataFormat"])
            //{ //REM: Chart displays percentage
            //    case "1":
            //        chart.Series[index].Points[noColumn].AxisLabel = String.Format("{0:P}", Percent_Of_Calls);
            //        break;
            //    //REM: Chart displays numbers
            //    case "2":
            //        chart.Series[index].Points[noColumn].AxisLabel = Number_Of_Calls.ToString();
            //        break;
            //    default:
            //        chart.Series[index].Points[noColumn].AxisLabel = Number_Of_Calls.ToString();
            //        break;
            //}

            chart.Series[index].Points[noColumn].LegendText = productName.ToString();
            chart.Series[index].Points[noColumn].Color = ReportColors.CustomerContactReports.GetColor(Convert.ToInt32(productId) - 1);
            chart.Series[index].Points[noColumn].Href = string.Format("javascript:ccrProductsDiscussedDrilldown({0},{1},'{2}')", productId, noColumn, chart.ClientID);
        }

        public void ProcessGenzymeGrid(Telerik.Web.UI.RadGrid grid, IList<DbDataRecord> meetingActivityFilter)
        {
            using (PathfinderGenzymeEntities clientContext = new PathfinderGenzymeEntities())
            {
                grid.MasterTableView.DataSource = meetingActivityFilter;
                grid.MasterTableView.DataBind();
            }
        }

        public string[] GetCRProductsDiscussedID()
        {
            string prod = Convert.ToString(HttpContext.Current.Request.QueryString["Products_Discussed_ID"]);
            if (prod.IndexOf(',') >= 0)
            {
                string[] productID = prod.Split(',');
                return productID;
            }
            else
            {
                string[] productID = { prod };
                return productID;
            }
        }

        public string[] GetCRProductsDiscussedID(string prod)
        {
            if (prod.IndexOf(',') >= 0)
            {
                string[] productID = prod.Split(',');
                return productID;
            }
            else
            {
                string[] productID = { prod };
                return productID;
            }
        }
    }

}



/// <summary>
/// Provides functionality for reporting in Customer Contact Reports
/// </summary>
/// 

