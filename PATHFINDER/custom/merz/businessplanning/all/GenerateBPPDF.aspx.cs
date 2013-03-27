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

public partial class custom_merz_businessplanning_all_GenerateBPPDF : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
       //Pass querystring variable "report=2" when exporting the file.
        //create the PDF file
        int Plan_ID = Convert.ToInt32(HttpContext.Current.Request.QueryString["Plan_ID"]);
        int Thera_ID = Convert.ToInt32(HttpContext.Current.Request.QueryString["Thera_ID"]);
        int Section_ID = Convert.ToInt32(HttpContext.Current.Request.QueryString["Section_ID"]);

        string bpName = Plan_ID.ToString();

        string fileName = String.Format("{0}_{1}", Pinsonault.Web.Session.FullName, bpName);
        string safeName = System.Text.RegularExpressions.Regex.Replace(fileName, @"\W", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        //fileName = string.Format("{0}.pdf", safeName);
        fileName = safeName;

        Pinsonault.Web.PDFSupport.ExportPageToPDF(Request, "BusinessPlanning.aspx", fileName, true);
    }    
}
