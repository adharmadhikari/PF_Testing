using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderClientModel;
using Telerik.Web.UI;
using Pinsonault.Application.Dey;
using Pinsonault.Web;

public partial class custom_dey_sellsheets_all_New_SellSheetOrder : System.Web.UI.Page
{
    public int SheetID { get; set; }
    public string RepID { get; set; }
    public string OrderID { get; set; }

    protected override void OnInit(EventArgs e)
    {
        Pinsonault.Web.Session.CheckSessionState();
        SheetID = Convert.ToInt32(Request.QueryString["SellSheetID"]);
        RepID = Request.QueryString["RepID"];

        ds_Rep.ConnectionString = Pinsonault.Web.Session.ClientDBConnectionString;
        if (!string.IsNullOrEmpty(RepID))
        {
            string sqlcmd = "Select * from V_Sell_Sheet_Territory_Reps where ID in (" + RepID + ")";
            ds_Rep.SelectCommand = sqlcmd;
            ds_Rep.SelectCommandType = SqlDataSourceCommandType.Text;
        }
        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Selected_SheetID.Value = SheetID.ToString();
        gridRep.DataSource = ds_Rep;
        gridRep.DataBind();
    }

    private string GetData(int ssid)
    {
        String SellSheetNM = "";
        String CreatedByNM = "";
        String UserEmailAddr = "";
        String PDFFileNM = ssid.ToString() + "_preview.pdf";

        using (PathfinderClientModel.PathfinderClientEntities clientContext = new PathfinderClientModel.PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
        {
            //Select the Copay and TemplateName fields
            SellSheetMast ssMast = null;
            ssMast = (from d in clientContext.SellSheetMastSet
                      where d.Sell_Sheet_ID == ssid
                      select d).First();

            if (ssMast != null)
            {
                SellSheetNM = ssMast.Sell_Sheet_Name.ToString();
                if (!String.IsNullOrEmpty(ssMast.Created_BY))
                    CreatedByNM = ssMast.Created_BY.ToString();
            }
        }

        UserEmailAddr = HttpContext.Current.User.Identity.Name;

        string strMain = "";
        string orders = Order_ID.Value;
        string[] a = orders.Split(',');
        int i = 0;
        //Generate email body.
        strMain = "<table style='font-family : Arial; font-size: 12px; color:#2d58a7;' cellpadding='10' border='1'>";
        strMain = string.Concat(strMain, "<tr><td colspan='4' style='background-color: #bbbfc2;'><b>Print Reorder Request</b></td></tr>");
        strMain = string.Concat(strMain, "<tr><th>Order ID</th><th>Sales Rep</th><th>Quantity</th><th>Address</th></tr>");

        foreach (GridDataItem item in gridRep.MasterTableView.Items)
        {
            int Rep_ID = Convert.ToInt32(item["ID"].Text);
            DropDownList ddl = (DropDownList)item.FindControl("ddl_quantity");
            int Quantity = Convert.ToInt32(ddl.SelectedItem.Value);
            string Rep_Name = item["Name"].Text;
            string Rep_Address = item["RepFullAddress"].Text;
            strMain = string.Concat(strMain, "<tr><td>" + a[i] + "</td>");
            strMain = string.Concat(strMain, "<td>" + Rep_Name + "</td>");
            strMain = string.Concat(strMain, "<td>" + Quantity + "</td>");
            strMain = string.Concat(strMain, "<td>" + Rep_Address + "</td></tr>");
            i++;
        }
        strMain = string.Concat(strMain, "</table>");
        return strMain;
    }
    
    protected void lb_order_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem item in gridRep.MasterTableView.Items)
        {

            int Rep_ID = Convert.ToInt32(item["ID"].Text);
            DropDownList ddl = (DropDownList)item.FindControl("ddl_quantity");
            int Quantity = Convert.ToInt32(ddl.SelectedItem.Value);

            using (PathfinderDeyEntities context = new PathfinderDeyEntities())
            {
                var rep_data = (from c in context.SellSheetRepsSet
                                where c.Rep_ID == Rep_ID
                                select c).First();

                Pinsonault.Application.Dey.SellSheetOrders ss_order = new Pinsonault.Application.Dey.SellSheetOrders()
                {
                    Rep_ID = Rep_ID,
                    Sell_Sheet_ID = SheetID,
                    Sell_Sheet_Copies = Quantity,
                    Ship_Address1 = rep_data.Address,
                    Ship_City = rep_data.City,
                    Ship_State = rep_data.State,
                    Ship_Zip = rep_data.Zip,
                    Ship_Phone = rep_data.Rep_Cell,
                    Ship_Email = rep_data.Rep_Email,
                    Created_BY = Pinsonault.Web.Session.FullName,
                    Created_DT = DateTime.UtcNow
                };
                context.AddToSellSheetOrdersSet(ss_order);
                context.SaveChanges();
                if (!string.IsNullOrEmpty(Order_ID.Value))
                {
                    Order_ID.Value = Order_ID.Value + "," + ss_order.Order_ID.ToString();
                }
                else
                    Order_ID.Value = ss_order.Order_ID.ToString();
                bool test = SendEmail(SheetID, ss_order.Order_ID.ToString());
            }
        }
        gridRep.DataSource = new string[] { };
        gridRep.Rebind();
        Page.ClientScript.RegisterStartupScript(typeof(Page), "RefreshOrders", "RefreshOrders();", true); 
        //Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "RefreshOrders", "RefreshOrders();", true);
        //Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "UpdateOrder", "OpenOrderDtls();", true);           


    }

    private bool SendEmail(int ssid, string order)
    {
        bool emailSuccess = true;

        List<string> attachments = new List<string>();

        string strTemplateName = "_landscape";
        string ssName = "";
        string footertemplateName = null;

        //Select the TemplateName to determine PDF orientation

        using (PathfinderClientModel.PathfinderClientEntities clientContext = new PathfinderClientModel.PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
        {
            //Select the Copay and TemplateName fields
            SellSheetMast ssMast = null;
            ssMast = (from d in clientContext.SellSheetMastSet
                      where d.Sell_Sheet_ID == ssid
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
                    //footertemplateName = strTemplateName.Replace(".jpg", "_footer.jpg");
                    //string[] a = strTemplateName.Split('.jpg');
                    //footertemplateName = Server.MapPath(string.Format("../templates/{0}", a[0].ToString() + "_footer.jpg"));
                }
                if (!String.IsNullOrEmpty(ssMast.Sell_Sheet_Name))
                    ssName = ssMast.Sell_Sheet_Name.ToString();
            }
        }

        if (!String.IsNullOrEmpty(strTemplateName))
        {
            //create the PDF file
            string strCookieName = "", strCookieValue = "";
            strCookieName = HttpContext.Current.Request.Cookies[".ASPXAUTH"].Name;
            strCookieValue = HttpContext.Current.Request.Cookies[".ASPXAUTH"].Value;

            string strServerName = Request.ServerVariables["LOCAL_ADDR"];
            string strPort = Request.ServerVariables["SERVER_PORT"];


            //string urlReplace = string.Format("custom/{0}/sellsheets/all/SellSheetCreatePreview.aspx?Sell_Sheet_ID={1}", Pinsonault.Web.Session.ClientKey, ssid.ToString());
            //string strUrl = Request.Url.AbsoluteUri.Replace(urlReplace, string.Format("content/images/{0}", strTemplateName.ToString()));

            //Setup URL Page which will be converted to PDF.
            //to fix production bug because url contains https 
            //string strUrl = string.Format("http://{0}:{1}{2}?{3}"
            //                                , strServerName
            //                                , strPort
            //                                , HttpContext.Current.Request.Url.AbsolutePath.Replace("New_SellSheetOrder.aspx", "SellSheetCreatePreview.aspx")
            //                                , "Sell_Sheet_ID=" + SheetID.ToString());

            string strUrl = PDFSupport.ExportPDFUrl(Request, "SellSheetCreatePreview.aspx");
            strUrl = strUrl.Remove(strUrl.IndexOf('?')) + "?Sell_Sheet_ID=" + Request.QueryString["SellSheetID"];

            string fileName = String.Format("{0}_{1}", order , ssName);
            string safeName = System.Text.RegularExpressions.Regex.Replace(fileName, @"\W", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            //If sell sheet name is too long then use first 25 characters to build the PDF filename.
            //if (safeName.Length > 25)
            //safeName = safeName.Remove(25);

            fileName = String.Format("{0}.pdf", safeName);

            string path = String.Format("~/custom/dey/sellsheets/sspdfs/{0}",safeName);

            //PDF filename
            string sFileName = HttpContext.Current.Server.MapPath(String.Format("../sspdfs/{0}", safeName));

            //Generate PDF file.
            SellSheetToPDF sspdf = new SellSheetToPDF();
            sspdf.Dey_ExportToPDFwithFooter(strUrl, strCookieName, strCookieValue, sFileName, strTemplateName, false, footertemplateName);
        //    //Add the attachment.
        //    attachments.Add(HttpContext.Current.Server.MapPath(path));

        //    //Generate email 
        //    string strBody = GetData(ssid);
        //    //string strBody = "testing";
        //    string strFrom = "mbewalder@pinsonault.com";
        //    string strCC = "DYalamanchili@pinsonault.com";
        //    string strTo = "DYalamanchili@pinsonault.com";
        //    string strSub = "Formulary Sell Sheet: Print Reorder Request";

        //    //Send an email to Printer's Email address.
        //    emailSuccess = Pinsonault.Web.Support.SendAttachmentEmail(strFrom, strTo, strCC, strSub, strBody, true, attachments);
        }
        return emailSuccess;
    }

}
