using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using Persits.PDF;
using PathfinderClientModel;
using Pinsonault.Web;

public partial class custom_dey_sellsheets_all_EmailSellSheet : PageBase
{
    protected override void OnPreInit(EventArgs e)
    {
        this.Response.Cache.SetNoStore();
        base.OnPreInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            txtSubject.Text = String.Format("{0} - {1}", Request.QueryString["Sell_Sheet_Name"], Request.QueryString["Sell_Sheet_Date"]);
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Int32 SheetID = Convert.ToInt32(HttpContext.Current.Request.QueryString["Sell_Sheet_ID"]);
        List<string> attachments = new List<string>();

        string fileName = String.Format("{0}_{1}", Pinsonault.Web.Session.FullName, Request.QueryString["Sell_Sheet_Name"]);
        string safeName = System.Text.RegularExpressions.Regex.Replace(fileName, @"\W", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        if (safeName.Length > 25)
            safeName = safeName.Remove(25);

        fileName = String.Format("{0}.pdf", safeName);
        string path = String.Format("~/custom/{0}/sellsheets/sspdfs/{1}", Pinsonault.Web.Session.ClientKey, fileName);
        string fullPath = HttpContext.Current.Server.MapPath(path);

        string strTemplateName = "_landscape";
        string footertemplateName = null;

        //Select the TemplateName to determine PDF orientation
        using (PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
        {            
            SellSheetMast ssMast = null;
            ssMast = (from d in context.SellSheetMastSet
                      where d.Sell_Sheet_ID == SheetID
                      select d).First();
         
            if (ssMast != null)
            {
                if (!String.IsNullOrEmpty(ssMast.Template_Name))
                    strTemplateName = HttpContext.Current.Server.MapPath(string.Format("../templates/{0}", ssMast.Template_Name.ToString()));
                if (!string.IsNullOrEmpty(strTemplateName))
                {
                    if (ssMast.Segment_CP == true)
                    {
                        switch (ssMast.Type_ID)
                        {
                            case 1:
                                footertemplateName = strTemplateName.Replace(".jpg", "_4009_footer.jpg");
                                break;
                            case 2:
                                footertemplateName = strTemplateName.Replace(".jpg", "_4009A_footer.jpg");
                                break;
                            case 3:
                                footertemplateName = strTemplateName.Replace(".jpg", "_4009B_footer.jpg");
                                break;
                        }
                    }
                    if (ssMast.Segment_MD == true)
                    {
                        switch (ssMast.Type_ID)
                        {
                            case 1:
                                footertemplateName = strTemplateName.Replace(".jpg", "_4009C_footer.jpg");
                                break;
                            case 2:
                                footertemplateName = strTemplateName.Replace(".jpg", "_4009D_footer.jpg");
                                break;
                            case 3:
                                footertemplateName = strTemplateName.Replace(".jpg", "_4009E_footer.jpg");
                                break;
                        }
                    }
                }
            }
        }

        ////create the PDF file
        string strCookieName = "", strCookieValue = "";
        strCookieName = Request.Cookies[".ASPXAUTH"].Name;
        strCookieValue = Request.Cookies[".ASPXAUTH"].Value;

        string strServerName = Request.ServerVariables["LOCAL_ADDR"];
        string strPort = Request.ServerVariables["SERVER_PORT"];

        ////Setup URL Page which will be converted to PDF.
       //fix url error

        string url = Pinsonault.Web.PDFSupport.ExportPDFUrl(Request, "SellSheetCreatePreview.aspx");

        //PDF filename
        string sFileName = Server.MapPath(String.Format("../sspdfs/{0}", safeName));
        
        SellSheetToPDF s = new SellSheetToPDF();
        s.Dey_ExportToPDFwithFooter(url, strCookieName, strCookieValue, sFileName, strTemplateName, false, footertemplateName);

        attachments.Add(HttpContext.Current.Server.MapPath(path));

        //fix from error
        //bool emailSuccess = Pinsonault.Web.Support.SendAttachmentEmail(Pinsonault.Web.Support.UserEmail, txtEmail.Text, "Formulary Sell Sheet", txtComments.Text, false, attachments);
        bool emailSuccess = Pinsonault.Web.Support.SendAttachmentEmail(Pinsonault.Web.Support.CustomerSupportEmail, txtEmail.Text, string.Format("Formulary Sell Sheet from - {0}", Pinsonault.Web.Support.UserEmail), txtComments.Text, false, attachments);

        divEmail.Visible = false;

        if (emailSuccess)
            lblMessage.Text = "Email successfully sent.";
        else
            lblMessage.Text = "Email failed.";

        lblMessage.Visible = true;

        //Calls Javascript function RefreshPlanSelection() to refresh plan selection list parent grid.
        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "CloseWinDelay", "CloseWinDelay();", true);
    }
}
