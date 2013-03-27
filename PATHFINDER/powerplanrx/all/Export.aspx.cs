using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Reflection;
using System.Configuration;
using System.Data;
using Pinsonault.Application.PowerPlanRx;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;
using System.Collections.Specialized;

using Pinsonault.Web;
using Pathfinder;
using System.EnterpriseServices;

/// <summary>
/// This page is being used for excel export and pdf export functionality. 
/// </summary>

public partial class Export : System.Web.UI.Page
{   
    public enum ExcelReportType
    {
        Summary = 1,
        Details = 2,
        PhysicianList = 3,
        RegionSingleBrand = 4,
        DistrictSingleBrand = 5,
        RegionGroup = 6,
        DistrictGroup = 7
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpContext.Current.Response.AddHeader("p3p", "CP=\"IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT\"");

        try
        {
            string strFileName = string.Empty;
          
            string strPagename = Request.QueryString["page"].ToString().ToLower();
            string strExportType = Request.QueryString["type"].ToString().ToLower();

            //TODO: change the code in common class and uncomment this code later
            switch (strPagename)
            {
            //    case "districtprofiletrxchart":
            //        {
            //            if (strExportType == "pdf")
            //            {
            //                strFileName = "BrandSharedVolume";                          
            //                PDFSupport.ExportPageToPDF(Request, "DistrictProfileTrxChartPDF.aspx", strFileName);
            //            }
            //        }
            //        break;


            //    case "districtregionbrandreport":
            //        {
            //            if (strExportType == "pdf")
            //            {
            //                strFileName = "DistrictRegionProfileReport";                           
            //                PDFSupport.ExportPageToPDF(Request, "DistrictRegionBrandReportPDF.aspx", strFileName);
            //            }
            //            else if (strExportType == "excel")
            //            {
            //                string strReportName = "District Region Profile Report";
            //                string strHeader = string.Empty;
            //                SortedList slReportCriteria = new SortedList();

            //                DataSet dsReport = Campaign.dsSVBaseByRegionDistrictSegmentBrandID(Request.QueryString["reporttype"].ToString(), Request.QueryString["regionid"].ToString(), Request.QueryString["dist"].ToString(), Convert.ToInt32((Request.QueryString["brandid"]).ToString()), Convert.ToInt32((Request.QueryString["segment"]).ToString()));
            //                HttpContext ctx = HttpContext.Current;

            //                DataRow drReport = dsReport.Tables[0].Rows[0];

            //                if (Request.QueryString["dist"].ToString() == "0")
            //                {
            //                    slReportCriteria.Add("Region", drReport["Region_ID"].ToString() + " " + drReport["Region_Name"].ToString());
            //                }
            //                else
            //                {
            //                    slReportCriteria.Add("District", drReport["District_ID"].ToString() + " " + drReport["District_Name"].ToString());
            //                }
            //                slReportCriteria.Add("Brand", drReport["PP_Brand_Name"].ToString());
            //                slReportCriteria.Add("Segment", drReport["Segment_Name"].ToString());

            //                strHeader = ExporterBase.GetPPRXCriteriaDetails("District Region Profile Report", Pinsonault.Data.Reports.ReportType.Excel, slReportCriteria);

            //                if (Request.QueryString["reporttype"].ToString() == "1")
            //                {
            //                    if (Request.QueryString["dist"].ToString() == "0")
            //                    {

            //                        ExportExcelForReport(ctx, strReportName, dsReport, strHeader, Request.QueryString, null, (int)ExcelReportType.RegionSingleBrand, null);
            //                    }
            //                    else
            //                    {
            //                        ExportExcelForReport(ctx, strReportName, dsReport, strHeader, Request.QueryString, null, (int)ExcelReportType.DistrictSingleBrand, null);
            //                    }
            //                }
            //                else
            //                {
            //                    if (Request.QueryString["dist"].ToString() == "0")
            //                    {
            //                        ExportExcelForReport(ctx, strReportName, dsReport, strHeader, Request.QueryString, null, (int)ExcelReportType.RegionGroup, null);
            //                    }
            //                    else
            //                    {
            //                        ExportExcelForReport(ctx, strReportName, dsReport, strHeader, Request.QueryString, null, (int)ExcelReportType.DistrictGroup, null);
            //                    }
            //                }

            //            }
            //        }
            //        break;

                case "physicians":
                    {
                        if (strExportType == "pdf")
                        {
                            strFileName = "PhysiciansList";                           
                            PDFSupport.ExportPageToPDF(Request, "PhysiciansPDF.aspx", strFileName);                           
                        }
                        else if (strExportType == "excel")
                        {
                            string strReportName = "Physicians List";
                            string strHeader = string.Empty;
                            SortedList slReportCriteria = new SortedList();

                            DataSet dsPhys = Campaign.GetPhysList(Request.QueryString["dist"].ToString(), Convert.ToInt32(Request.QueryString["id"]));
                            HttpContext ctx = HttpContext.Current;

                            DataRow drPhys = dsPhys.Tables[0].Rows[0];
                            slReportCriteria.Add("District", drPhys["District_ID"].ToString() + " " + drPhys["District_Name"].ToString());
                            slReportCriteria.Add("Brand", drPhys["Brand_Name"].ToString());
                            slReportCriteria.Add("Plan Name", drPhys["Plan_Name"].ToString());

                            slReportCriteria.Add("Data Month", string.Format("{0}/{1}", drPhys["Data_Month"].ToString(), drPhys["Data_Year"].ToString()));

                            strHeader = ExporterBase.GetPPRXCriteriaDetails("Physician List", Pinsonault.Data.Reports.ReportType.Excel, slReportCriteria);

                            ExportExcelForReport(ctx, strReportName, dsPhys, strHeader, Request.QueryString, null, (int)ExcelReportType.PhysicianList, null);
                        }
                    }
                    break;
            }
           
        }
        catch(Exception ex)
        {
            //handle exception
            throw ex;
        }
          
    }  
   /// <summary>
   /// for excel export
   /// </summary>
   /// <param name="context"></param>
   /// <param name="strReportName"></param>
   /// <param name="dsReportSet"></param>
   /// <param name="strHeader"></param>
   /// <param name="QueryString"></param>
   /// <param name="MonthListDesc"></param>
   /// <param name="iReportType"></param>
   /// <param name="dtReportSummary"></param>
    public static void ExportExcelForReport(HttpContext context, string strReportName, DataSet dsReportSet, string strHeader, NameValueCollection QueryString, IList<int> MonthListDesc, int iReportType, DataTable dtReportSummary)
    {
        HttpRequest request = context.Request;
        HttpResponse response = context.Response;

        try
        {
            //TODO: uncomment it later- commented to remove build error
            using (ExporterBase exporter = ExporterBase.CreateInstance<StandardExcelExporter>(strReportName, dsReportSet))
            {
                exporter.ProtectOutputFile = true;
                String outputFile = exporter.ExportToFileWithFormat(strHeader, strReportName, dsReportSet, QueryString, MonthListDesc, iReportType, dtReportSummary);

                // Send response data and headers
                response.ContentType = "application/x-excel";
                response.AddHeader("Content-Disposition",
                    String.Format("attachment; filename=\"{0}-{1}.xls\"",
                        strReportName, Guid.NewGuid().ToString()));
                response.AddHeader("Pragma", "no-cache");
                response.TransmitFile(outputFile);

                // Direct calls to File.Delete can cause hangs.
                AppShutdownExecutor.Enqueue(
                    delegate()
                    {
                        File.Delete(outputFile);
                    }
                );
            }
        }
        catch (Exception ex)
        {
            //handle exception
        }

        finally { }
    }
}
