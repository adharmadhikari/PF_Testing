using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using Persits.PDF;
using System.IO;
using PathfinderClientModel;
using Pinsonault.Web;

public partial class custom_labopharm_sellsheets_sellsheetreview :  InputFormBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Int32 SheetID = 0;

        //Get selected Sell_Sheet_ID
        SheetID = Convert.ToInt32(HttpContext.Current.Request.QueryString["Sell_Sheet_ID"]);

        string requestUrl = HttpContext.Current.Request.Url.ToString().Replace("sellsheetreview.aspx", "GenerateSellSheetPDF.aspx");

        string templateName = "_portrait";

        using ( PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString) )
        {
            //Select the Copay and TemplateName fields
            SellSheetMast ssMast = null;
            ssMast = (from d in context.SellSheetMastSet
                      where d.Sell_Sheet_ID == SheetID
                      select d).First();

            if (!String.IsNullOrEmpty(ssMast.Template_Name))
                templateName = ssMast.Template_Name.ToString();
        }

        //Generate random number so iFrame content does not cache on refresh
        Random r = new Random();
        double num = ((r.NextDouble()) * 101);

        if (templateName.IndexOf("_portrait") > 0)
            ssPreview.Text = String.Format("<iframe id='iframeSS' allowtransparency='true' width='262' height='670' id='ifPreview' src='{0}&Preview=True&rnd={1}'></iframe>", requestUrl, num);
        else
            ssPreview.Text = String.Format("<iframe id='iframeSS' allowtransparency='true' width='424' height='670' id='ifPreview' src='{0}&Preview=True&rnd={1}'></iframe>", requestUrl, num);

        litExport.Text = String.Format("<a id='ExportSellSheet' href='javascript:openPDFPreview({0})' runat='server' ><img src='content/images/pdf-icon.gif'/></a>", SheetID);
    }

    protected override bool IsRequestValid()
    {
        return true;
    }
  
}
