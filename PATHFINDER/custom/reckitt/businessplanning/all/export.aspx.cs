using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using Persits.PDF;


public partial class custom_reckitt_businessplanning_all_export : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ////create the PDF file
        //string strCookieName = "", strCookieValue = "";
        //strCookieName = Request.Cookies[".ASPXAUTH"].Name;
        //strCookieValue = Request.Cookies[".ASPXAUTH"].Value;

        //string strServerName = Request.ServerVariables["LOCAL_ADDR"];
        //string strPort = Request.ServerVariables["SERVER_PORT"];

        ////Setup URL Page which will be converted to PDF.
        //string strUrl = string.Format("https://www.pathfinderrx.com{0}?{1}"                                        
        //                                , Request.Url.AbsolutePath.Replace("export.aspx","businessplanning_pdf.aspx")
        //                                , Request.QueryString);
        //ExportToPDF(strUrl, strCookieName, strCookieValue, "business_planning", 36, true);


        Pinsonault.Web.PDFSupport.ExportPageToPDF(Request, "businessplanning_pdf.aspx", "business_planning", true, 36, true);
    }
    //public static void ExportToPDF(string strUrl, string strCookieName, string strCookieValue, string strFileName, int Margin, bool disclaimer)
    //{
    //    PdfManager objPdf = new PdfManager();
    //    PdfDocument objDoc = objPdf.CreateDocument();

    //    objDoc.ImportFromUrl(strUrl, string.Format("landscape=true,LeftMargin={0},RightMargin={0},TopMargin={0},BottomMargin={0}", Margin), "Cookie:" + strCookieName, strCookieValue);

    //    if (disclaimer)
    //    {
    //        foreach (PdfPage page in objDoc.Pages)
    //        {
    //            page.Canvas.DrawText("FOR INTERNAL USE ONLY.  NOT FOR DISTRIBUTION OR DISSEMINATION.", objPdf.CreateParam(string.Format("Alignment=2; Angle=90; x={0}; y=0; width={1}", page.Width - 13, page.Height)), objDoc.Fonts[0]);
    //        }
    //    }

    //    objDoc.SaveHttp("attachment;filename=" + strFileName + ".pdf");
    //}  
}
