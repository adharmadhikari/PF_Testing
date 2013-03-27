using System.Data.Services;
using System.ServiceModel.Web;
using System.Text;
using PathfinderClientModel;
using System.Linq;
using System.Web;
using System;
using System.Data.Objects;
using System.Linq.Expressions;
using System.Collections.Specialized;
using System.Collections.Generic;
using ASPPDFLib;
using Pinsonault.Web.Services;
using System.Reflection;

public class CustomDataService : PathfinderClientDataServiceBase<PathfinderClientEntities>
{
    // This method is called only once to initialize service-wide policies.
    public static void InitializeService(IDataServiceConfiguration config)
    {
        Pinsonault.Web.Support.InitializeService(config);
        
        //Important: Only list Entity Sets that should be accessible through the web service.  If client side queries are not required to populate a grid or another control do not make it public     
        config.SetEntitySetAccessRule("SellSheetSet", EntitySetRights.AllRead);
        config.SetEntitySetAccessRule("PlanSearchSet", EntitySetRights.AllRead);
        config.SetEntitySetAccessRule("DraftedSellSheetsSet", EntitySetRights.AllRead);
        config.SetEntitySetAccessRule("CompletedSellSheetsSet", EntitySetRights.AllRead);
        config.SetEntitySetAccessRule("SellSheetOrdersSet", EntitySetRights.AllRead);
        config.SetEntitySetAccessRule("SSPlanSelectionListSet", EntitySetRights.AllRead);
        config.SetEntitySetAccessRule("SellSheetPlansSet", EntitySetRights.AllRead);
        config.SetEntitySetAccessRule("SellSheetMastSet", EntitySetRights.AllRead);
        config.SetEntitySetAccessRule("SellSheetAdditionalPlansSet", EntitySetRights.AllRead);
        config.SetEntitySetAccessRule("PlanDocumentsViewSet", EntitySetRights.AllRead);
        config.SetEntitySetAccessRule("ContactReportProductsDiscussedViewSet", EntitySetRights.AllRead);
      
        config.SetEntitySetAccessRule("PlanInfoListViewSet", EntitySetRights.AllRead);
        config.SetEntitySetAccessRule("ContactReportDataSet", EntitySetRights.AllRead);
        config.SetEntitySetAccessRule("PlansSectionViewSet", EntitySetRights.AllRead);
        config.SetEntitySetAccessRule("PlansGridSet", EntitySetRights.AllRead);

        config.MaxResultsPerCollection = 5000;
    }

    #region Query Interceptors

    //Gets drafted sellsheets data for logged on userid.
    [QueryInterceptor("DraftedSellSheetsSet")]
    public Expression<Func<DraftedSellSheets, bool>> FilterDraftedSellSheetsByUser()
    {
        int userID = Pinsonault.Web.Session.UserID;
        return e => e.User_ID == userID;
    }

    //Gets completed sellsheets data for logged on userid.
    [QueryInterceptor("CompletedSellSheetsSet")]
    public Expression<Func<CompletedSellSheets, bool>> FilterCompletedSellSheetsByUser()
    {
        int userID = Pinsonault.Web.Session.UserID;
        return e => e.User_ID == userID;
    }

    //Gets completed sellsheets data for logged on userid.
    [QueryInterceptor("SSPlanSelectionListSet")]
    public Expression<Func<SSPlanSelectionList, bool>> FilterSSPlanSelectionListByUser()
    {
        int userID = Pinsonault.Web.Session.UserID;
        return e => e.User_ID == userID;
    }

    [QueryInterceptor("PlansSectionViewSet")]
    public Expression<Func<PlansSectionView, bool>> FilterPlansByTerritory()
    {
        string terrID = Pinsonault.Web.Session.TerritoryID;

        if ( !string.IsNullOrEmpty(terrID) )
            return p => p.Territory_ID == terrID;
        else //if user does not have a terr id then return True since they may not have plan alignments (THIS IS DIFFERENT THAN TA WHERE FALSE IS DEFAULT)
            return p => true;
    }

    [QueryInterceptor("PlansSectionViewSet")]
    public Expression<Func<PlansSectionView, bool>> FilterPlans()
    {
        return Pinsonault.Web.Data.LINQHelper.GenFilterPlansByClientSections<PlansSectionView>(Pinsonault.Web.Identifiers.TodaysAccounts);
    }

    [QueryInterceptor("PlansGridSet")]
    public Expression<Func<PlansGrid, bool>> FilterPlanByUser()
    {
        if (Pinsonault.Web.Session.ClientID == 17) { return e => true; }
        else
        {
            int userID = Pinsonault.Web.Session.UserID;
            return e => e.AE_UserID == userID;
        }
    }
    #endregion

    [WebGet]
    public string GetCCRModuleOptions()
    {
        using (PathfinderModel.PathfinderEntities context = new PathfinderModel.PathfinderEntities())
        {
            return context.GetUserReportOptionsAsJSON(Pinsonault.Web.Session.UserID, Pinsonault.Web.Identifiers.CustomerContactReports);
        }
    }

    [WebGet]
    public string GetFormularyHistoryReportingModuleOptions()
    {
        using (PathfinderModel.PathfinderEntities context = new PathfinderModel.PathfinderEntities())
        {
            return context.GetUserReportOptionsAsJSON(Pinsonault.Web.Session.UserID, Pinsonault.Web.Identifiers.FormularyHistoryReporting);
        }
    }

    [WebGet]
    public string GetBusinessPlanningOptions()
    {
        using (PathfinderModel.PathfinderEntities context = new PathfinderModel.PathfinderEntities())
        {
            return context.GetUserReportOptionsAsJSON(Pinsonault.Web.Session.UserID, Pinsonault.Web.Identifiers.BusinessPlanning);
        }
    }
    [WebGet]
    public string GetSellSheetReportingOptions()
    {
        using (PathfinderModel.PathfinderEntities context = new PathfinderModel.PathfinderEntities())
        {
            return context.GetUserReportOptionsAsJSON(Pinsonault.Web.Session.UserID, Pinsonault.Web.Identifiers.SellSheetReporting);
        }
    }


    
    [WebGet]
    public int GetPlanInfoListViewCount(string where)
    {
        if ( String.IsNullOrEmpty(where) )
            return CurrentDataSource.PlanInfoListViewSet.Count();
        else
            return CurrentDataSource.PlanInfoListViewSet.Where(where).Count();
    }
    [WebGet]
    public int GetPlanSectionViewCount(string where)
    {
        if (String.IsNullOrEmpty(where))
            return CurrentDataSource.PlansSectionViewSet.Where(FilterPlansByTerritory()).Where(FilterPlans()).Count();
        else
            return CurrentDataSource.PlansSectionViewSet.Where(where).Where(FilterPlansByTerritory()).Where(FilterPlans()).Count();
    }
   


    [WebGet]
    public int GetDocumentCount(string where)
    {

        if ( string.IsNullOrEmpty(where) )
            return CurrentDataSource.PlanDocumentsViewSet.Count();
        else
            return CurrentDataSource.PlanDocumentsViewSet.Where(where).Count();
    }

    [WebGet]
    public int GetCRDrillDownCount(string where)
    {

        if ( string.IsNullOrEmpty(where) )
                return CurrentDataSource.ContactReportDataSet.Count();
        else
                return CurrentDataSource.ContactReportDataSet.Where(where.Replace("%27", "'")).Count();
    }
   
    [WebGet]
    public int GetCCReportCount(string where)
    {
        if ( string.IsNullOrEmpty(where) )
            return CurrentDataSource.ContactReportProductsDiscussedViewSet.Count();
        else
            return CurrentDataSource.ContactReportProductsDiscussedViewSet.Where(where).Count();
    }

   

    [WebGet]
    public int CreateSellSheet(string Created)
    {
        DateTime created;
        if ( DateTime.TryParse(Created, out created) )
        {
            SellSheet sellSheet = new SellSheet();
            sellSheet.Sell_Sheet_Name = string.Format("Draft - {0:d}", created);
            sellSheet.Status_ID = 1;
            sellSheet.Current_Step = "classandtemplateselection";
            sellSheet.Territory_ID = Pinsonault.Web.Session.TerritoryID;
            sellSheet.Include_Territory_Name = true;
            //Set type = "Tier Status" by default
            sellSheet.Type_ID = 1;
            sellSheet.Created_BY = Pinsonault.Web.Session.FullName;
            sellSheet.Created_DT = DateTime.UtcNow;
            sellSheet.Modified_DT = sellSheet.Created_DT;
            sellSheet.Modified_BY = sellSheet.Created_BY;
            CurrentDataSource.AddToSellSheetSet(sellSheet);
            CurrentDataSource.SaveChanges();

            return sellSheet.Sell_Sheet_ID;
        }

        return 0;
    }

    //Updates the Template ID in DB for selected sell sheet from Template Selector sidebar
    [WebInvoke]
    public bool UpdateSellSheetTemplate()
    {
        NameValueCollection form = HttpContext.Current.Request.Form;
        int ssID = Convert.ToInt32(form["sellSheetID"]);
        int templateID = Convert.ToInt32(form["tempID"]);

        SellSheet ss = CurrentDataSource.SellSheetSet.FirstOrDefault(s => s.Sell_Sheet_ID == ssID);

        if (ss != null)
        {
            ss.Template_ID = templateID;
            CurrentDataSource.SaveChanges();
            return true;
        }
        return false;
    }

    //Checks for a duplicate Geography Name entry for Step 2 in Sell Sheets
    [WebInvoke]
    public int CheckGeoNameDuplicates()
    {
        NameValueCollection form = HttpContext.Current.Request.Form;
        string geoName = form["geographyName"];
        int ssID = Convert.ToInt32(form["sellSheetID"]);

        return CurrentDataSource.SellSheetSet.Where(s => s.Geography_Name.Equals(geoName, StringComparison.CurrentCultureIgnoreCase)).Where(s => s.Sell_Sheet_ID != ssID).Count();
    }

    //Inserts new order into DB for selected sell sheet.
    [WebInvoke]
    public bool NewSellSheetOrder()
    {
        tblSellSheetOrders sellSheet = new tblSellSheetOrders();
        NameValueCollection form = HttpContext.Current.Request.Form;

        //Get form data for new sell sheet order and save it to db.
        sellSheet.Sell_Sheet_ID = Convert.ToInt32(form["SelectedSheetID"]);
        sellSheet.Ship_Address1 = form["ctl00$Tile3$NewSellSheetOrder1$Addr1"];
        sellSheet.Ship_Address2 = form["ctl00$Tile3$NewSellSheetOrder1$Addr2"];
        sellSheet.Ship_Location_ID = Convert.ToInt32(form["ctl00$Tile3$NewSellSheetOrder1$rdcmbShipLocation"]);
        sellSheet.Sell_Sheet_Copies = Convert.ToInt32(form["ctl00$Tile3$NewSellSheetOrder1$NoSellSheets"]);
        sellSheet.Ship_City = form["ctl00$Tile3$NewSellSheetOrder1$City"];
        sellSheet.Ship_State = form["ctl00$Tile3$NewSellSheetOrder1$rdcmbState"];
        sellSheet.Ship_Zip = form["ctl00$Tile3$NewSellSheetOrder1$Zip"];
        sellSheet.Ship_Phone = form["ctl00$Tile3$NewSellSheetOrder1$Phone"];
        sellSheet.Ship_Email = form["ctl00$Tile3$NewSellSheetOrder1$Email"];
        sellSheet.Printer_Email = form["ctl00$Tile3$NewSellSheetOrder1$PrinterEmail"];
        sellSheet.Printer_Fax = form["ctl00$Tile3$NewSellSheetOrder1$PrinterFax"];
        sellSheet.Created_DT = DateTime.UtcNow;
        sellSheet.Created_BY = Pinsonault.Web.Session.FullName;
        sellSheet.Modified_DT = DateTime.UtcNow;
        sellSheet.Modified_BY = Pinsonault.Web.Session.FullName;   
        CurrentDataSource.AddTotblSellSheetOrdersSet(sellSheet);
        //CurrentDataSource.SaveChanges();
       
        //Send an email to Printer's Email containing order and shipping details.
        bool emailSuccess = SendEmail();

        //save changes if email has been sent successfully.
        if(emailSuccess)
            CurrentDataSource.SaveChanges();

        return emailSuccess;
    }

    //Used to send an email for new sell sheet order. 
    private bool SendEmail()
    {
        NameValueCollection form = HttpContext.Current.Request.Form;
        bool emailSuccess = true ;

        Int32 SheetID = Convert.ToInt32(form["SelectedSheetID"]);
        List<string> attachments = new List<string>();

        string strTemplateName = "_landscape";
        string ssName = "";
        //Select the TemplateName to determine PDF orientation
        SellSheetMast ss = CurrentDataSource.SellSheetMastSet.FirstOrDefault(s => s.Sell_Sheet_ID == SheetID);
        if (ss != null)
        {
            if (!String.IsNullOrEmpty(ss.Template_Name))
                strTemplateName = HttpContext.Current.Server.MapPath(string.Format("../templates/{0}", ss.Template_Name.ToString()));

            if (!String.IsNullOrEmpty(ss.Sell_Sheet_Name))
                ssName = ss.Sell_Sheet_Name.ToString();  
        }

        //create the PDF file
        string strCookieName = "", strCookieValue = "";
        strCookieName = HttpContext.Current.Request.Cookies[".ASPXAUTH"].Name;
        strCookieValue = HttpContext.Current.Request.Cookies[".ASPXAUTH"].Value;

        string strServerName = HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
        string strPort = HttpContext.Current.Request.ServerVariables["SERVER_PORT"];

        //Setup URL Page which will be converted to PDF.
        string strUrl = string.Format("http://{0}:{1}{2}?{3}"
                                        , strServerName
                                        , strPort
                                        , HttpContext.Current.Request.Url.AbsolutePath.Replace("services/pathfinderdataservice.svc/NewSellSheetOrder", "all/SellSheetCreatePreview.aspx")
                                        , "Sell_Sheet_ID=" + SheetID.ToString ());

        //PDF filename
        string fileName = String.Format("{0}_{1}", Pinsonault.Web.Session.FullName, ssName);
        string safeName = System.Text.RegularExpressions.Regex.Replace(fileName, @"\W", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        //If sell sheet name is too long then use first 25 characters to build the PDF filename.
        if (safeName.Length > 25)
            safeName = safeName.Remove(25);

        fileName = String.Format("{0}.pdf", safeName);

        string path = String.Format("~/custom/{0}/sellsheets/sspdfs/{1}", Pinsonault.Web.Session.ClientKey, fileName);

        //PDF filename
        string sFileName = HttpContext.Current.Server.MapPath(String.Format("../sspdfs/{0}", safeName));
        //Generate PDF file.
        SellSheetToPDF sspdf = new SellSheetToPDF();
        sspdf.ExportToPDF(strUrl, strCookieName, strCookieValue, sFileName, strTemplateName, false);
      
        //Add the attachment.
        attachments.Add(HttpContext.Current.Server.MapPath(path));

        //Generate email 
        string strBody = GetData();
        string strFrom = Pinsonault.Web.Support.UserEmail;
        string strCC = form["ctl00$Tile3$NewSellSheetOrder1$Email"];
        string strTo = form["ctl00$Tile3$NewSellSheetOrder1$PrinterEmail"];
        string strSub = "Formulary Sell Sheet: Print Reorder Request";

        //Send an email to Printer's Email address.
        emailSuccess = Pinsonault.Web.Support.SendAttachmentEmail(strFrom, strTo,strCC, strSub, strBody, true, attachments);

        return emailSuccess;
    }

    //Gets form data and builds email message.
    private string GetData()
    {
        NameValueCollection form = HttpContext.Current.Request.Form;
        Int32 SheetID = Convert.ToInt32(form["SelectedSheetID"]); 
        String SellSheetNM ="";
        String CreatedByNM = "";
        String UserEmailAddr = "";
        String PDFFileNM = SheetID.ToString()  + "_preview.pdf";

        SellSheet ss = CurrentDataSource.SellSheetSet.FirstOrDefault(s => s.Sell_Sheet_ID == SheetID);
        if (ss != null)
        {
            SellSheetNM = ss.Sell_Sheet_Name.ToString();
            if(!String.IsNullOrEmpty(ss.Created_BY))
                CreatedByNM = ss.Created_BY.ToString();  
        }
        UserEmailAddr = HttpContext.Current.User.Identity.Name;
         
        string strMain = "";
        
        //Generate email body.
        strMain = "<table style='font-family : Arial; font-size: 12px; color:#2d58a7;'><tr><td colspan='3' style='background-color: #bbbfc2;'><b>Print Reorder Request</b></td></tr><tr><td colspan='3' >&nbsp;</td></tr>";
        strMain = string.Concat(strMain, "<tr><td colspan='3' style='background-color: #bbbfc2;'><b>Requestor Information:</b></td></tr>");
        strMain = string.Concat(strMain, "<tr><td colspan='3' ><b> Name: </b>", CreatedByNM + "</td></tr>");
        strMain = string.Concat(strMain, "<tr><td colspan='3' ><b> Email Address: </b>", UserEmailAddr + "</td></tr>");
        strMain = string.Concat(strMain, "<tr><td colspan='3' style='background-color: #bbbfc2;'><b>Order Details</b></td></tr>");
        strMain = string.Concat(strMain, "<tr><td colspan='3' ><b> Name of the File: </b>", SellSheetNM + "</td></tr>");
        strMain = string.Concat(strMain, "<tr><td colspan='3' ><b> Quantity to Print: </b>", form["ctl00$Tile3$NewSellSheetOrder1$NoSellSheets"] + "</td></tr>");
        strMain = string.Concat(strMain, "<tr><td colspan='3' ><b> Print Instructions: </b>", form["ctl00$Tile3$NewSellSheetOrder1$PrintInsttxt"] + "</td></tr>");
        strMain = string.Concat(strMain, "<tr><td  colspan='3' style='background-color: #bbbfc2;'><b>Shipping Details</b></td></tr>");
        strMain = string.Concat(strMain, "<tr><td colspan='3' ><b> Address1: </b>", form["ctl00$Tile3$NewSellSheetOrder1$Addr1"] + "</td></tr>");
        strMain = string.Concat(strMain, "<tr><td colspan='3' ><b> Address2: </b>", form["ctl00$Tile3$NewSellSheetOrder1$Addr2"] + "</td></tr>");
        strMain = string.Concat(strMain, "<tr><td width='20%'><b> City: </b>", form["ctl00$Tile3$NewSellSheetOrder1$City"] + "</td>");
        strMain = string.Concat(strMain, "<td width='20%'><b> State: </b>", form["ctl00$Tile3$NewSellSheetOrder1$rdcmbState"] + "</td><td width='60%'>&nbsp;</td></tr>");
        strMain = string.Concat(strMain, "<tr><td colspan='3' ><b> Zip: </b>", form["ctl00$Tile3$NewSellSheetOrder1$Zip"] + "</td></tr>");
        if (!String.IsNullOrEmpty(form["ctl00$Tile3$NewSellSheetOrder1$Phone"]))
            strMain = string.Concat(strMain, "<tr><td colspan='3' ><b> Phone: </b>", form["ctl00$Tile3$NewSellSheetOrder1$Phone"] + "</td></tr>");
        strMain = string.Concat(strMain, "</table>");
        return strMain;
    }

    private static void ExportToPDF(string strUrl, string strCookieName, string strCookieValue, string strFileName, string strTemplateName)
    {
        String strPDFFile = strFileName + ".pdf";
        PdfManager objPdf = new PdfManager();
        IPdfDocument objDoc = objPdf.CreateDocument(Missing.Value);

        if (strTemplateName.IndexOf("_portrait") > 0)
            objDoc.ImportFromUrl(strUrl, "landscape=false,LeftMargin=0,RightMargin=0,TopMargin=-7,BottomMargin=0", "Cookie:" + strCookieName, strCookieValue);
        else
            objDoc.ImportFromUrl(strUrl, "landscape=true,LeftMargin=-7,RightMargin=0,TopMargin=0,BottomMargin=0", "Cookie:" + strCookieName, strCookieValue);
        
        objDoc.Save(strPDFFile, true);
        objDoc = null;
        objPdf = null;
    }

    //To change the status of sell sheets from active to deactive hence removing it from the list.
    public static bool RemoveSellSheet(int Sell_Sheet_ID, PathfinderClientModel.PathfinderClientEntities context)
    {
        PathfinderClientModel.SellSheet sheet = context.SellSheetSet.FirstOrDefault(s => s.Sell_Sheet_ID == Sell_Sheet_ID);
        if (sheet!= null)
        {
            sheet.Status_ID = 3;
            context.SaveChanges();
            return true;
        }
        return false;
    }

    [WebGet]
    public int GetDraftedSellSheetCount(string where)
    {
        if ( String.IsNullOrEmpty(where) )
            return CurrentDataSource.DraftedSellSheetsSet.Count(FilterDraftedSellSheetsByUser());
        else
            return CurrentDataSource.DraftedSellSheetsSet.Where(where).Count(FilterDraftedSellSheetsByUser());
    }

    [WebGet]
    public int GetCompletedSellSheetCount(string where)
    {
        if ( String.IsNullOrEmpty(where) )
            return CurrentDataSource.CompletedSellSheetsSet.Count(FilterCompletedSellSheetsByUser());
        else
            return CurrentDataSource.CompletedSellSheetsSet.Where(where).Count(FilterCompletedSellSheetsByUser());
    }

    [WebGet]
    public int GetSellSheetOrdersCount(string where)
    {
        if ( String.IsNullOrEmpty(where) )
            return CurrentDataSource.SellSheetOrdersSet.Count();
        else
            return CurrentDataSource.SellSheetOrdersSet.Where(where).Count();
    }

    [WebGet]
    public int GetSSPlanSelectionListCount(string where)
    {
        if ( String.IsNullOrEmpty(where) )
            return CurrentDataSource.SSPlanSelectionListSet.Count(FilterSSPlanSelectionListByUser());
        else
            return CurrentDataSource.SSPlanSelectionListSet.Where(where).Count(FilterSSPlanSelectionListByUser());
    }

    [WebGet]
    public string GetExecutiveReportsOptions()
    {
        using (PathfinderModel.PathfinderEntities context = new PathfinderModel.PathfinderEntities())
        {
            return context.GetUserReportOptionsAsJSON(Pinsonault.Web.Session.UserID, Pinsonault.Web.Identifiers.ExecutiveReports);
        }
    }

    [WebGet]
    public string GetRestrictionsReportOptions()
    {
        using (PathfinderModel.PathfinderEntities context = new PathfinderModel.PathfinderEntities())
        {
            return context.GetUserReportOptionsAsJSON(Pinsonault.Web.Session.UserID, Pinsonault.Web.Identifiers.RestrictionsReport);
        }
    }
 

}
