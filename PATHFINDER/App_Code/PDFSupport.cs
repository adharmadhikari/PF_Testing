using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASPPDFLib;
using System.Reflection;


namespace Pinsonault.Web
{
    /// <summary>
    /// Summary description for PDFSupport
    /// </summary>
    public static class PDFSupport
    {
        public static string ExportPDFUrl(HttpRequest Request, string ExportPageName)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\/\w+\.aspx\?", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            return regex.Replace(Request.Url.AbsoluteUri, string.Format("/{0}?", ExportPageName));
        }

        public static void ExportPageToPDF(HttpRequest Request, string ExportPageName, string OutputFileName)
        {
            ExportPageToPDF(Request, ExportPageName, OutputFileName, false, 36, false);
        }

        public static void ExportPageToPDF(HttpRequest Request, string ExportPageName, string OutputFileName, bool Landscape)
        {
            ExportPageToPDF(Request, ExportPageName, OutputFileName, Landscape, 36, false);
        }

        public static void ExportPageToPDF(HttpRequest Request, string ExportPageName, string OutputFileName, bool Landscape, int Margin, bool Disclaimer)
        {
            //create the PDF file
            string cookieName = "";
            string cookieValue = "";

            cookieName = Request.Cookies[".ASPXAUTH"].Name;
            cookieValue = Request.Cookies[".ASPXAUTH"].Value;

            ExportToPDF(ExportPDFUrl(Request, ExportPageName), cookieName, cookieValue, OutputFileName, Landscape, Margin, Disclaimer);
        }

        static void ExportToPDF(string strUrl, string cookieName, string cookieValue, string outputFileName, bool Landscape, int Margin, bool disclaimer)
        {
            PdfManager objPdf = new PdfManager();
            IPdfDocument objDoc = objPdf.CreateDocument(Missing.Value);

            objDoc.ImportFromUrl(strUrl, string.Format("landscape={0},LeftMargin={1},RightMargin={1},TopMargin={1},BottomMargin={1}", Landscape, Margin), "Cookie:" + cookieName, cookieValue);

            if ( disclaimer )
            {
                string pdfParam;
                foreach ( IPdfPage page in objDoc.Pages )
                {
                    if ( Landscape )
                        pdfParam = string.Format("Alignment=2; Angle=90; x={0}; y=0; width={1}", page.Width - 13, page.Height);
                    else
                        pdfParam = string.Format("Alignment=2; x=0; y=0; width={0}", page.Width - 13);

                    page.Canvas.DrawText("FOR INTERNAL USE ONLY.  NOT FOR DISTRIBUTION OR DISSEMINATION.", objPdf.CreateParam(pdfParam), objDoc.Fonts[0, Missing.Value]);
                }
            }

            objDoc.SaveHttp("attachment;filename=" + outputFileName + ".pdf", Missing.Value);
        }

    }

}