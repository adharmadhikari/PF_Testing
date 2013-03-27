using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using Persits.PDF;
using PathfinderClientModel;
using Pinsonault.Web;
using System.IO;

public partial class custom_sandoz_sellsheets_all_GenerateSellSheetPDF : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Int32 SheetID = Convert.ToInt32(HttpContext.Current.Request.QueryString["Sell_Sheet_ID"]);
        
        //create the PDF file
        string strCookieName = "", strCookieValue = "";
        strCookieName = Request.Cookies[".ASPXAUTH"].Name;
        strCookieValue = Request.Cookies[".ASPXAUTH"].Value;

        //string strServerName = "www.pathfinderrx.com";// Request.ServerVariables["LOCAL_ADDR"];
        //string strPort = "80";// Request.ServerVariables["SERVER_PORT"];

        //Setup URL Page which will be converted to PDF.
        //string strUrl = string.Format("https://{0}{1}?{2}"
        //                                , strServerName
        //                               // , strPort
        //                                , Request.Url.AbsolutePath.Replace("GenerateSellSheetPDF.aspx", "SellSheetCreatePreview.aspx")
        //                                , Request.QueryString);

        string url = Pinsonault.Web.PDFSupport.ExportPDFUrl(Request, "SellSheetCreatePreview.aspx");

        string templateName = null;

        //Select the TemplateName to determine PDF orientation
        using ( PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString) )
        {
            templateName = (from d in context.SellSheetMastSet
                      where d.Sell_Sheet_ID == SheetID
                      select d.Template_Name).FirstOrDefault();

            if ( !string.IsNullOrEmpty(templateName) )
                templateName = Server.MapPath(string.Format("../templates/{0}", templateName));
        }

        //PDF filename
        string sFileName = Server.MapPath("../sspdfs/" + SheetID + "_preview");

        SellSheetToPDF s = new SellSheetToPDF();
        s.ExportToPDF(url, strCookieName, strCookieValue, sFileName, templateName, true);
    }    
}
