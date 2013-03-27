using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using PathfinderClientModel;

public partial class custom_pinso_sellsheets_SaveSellSheet : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            String strSheetName = "";

            Int32 SheetID = Convert.ToInt32(Request.QueryString["Sell_Sheet_ID"]);
            using (PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
            {
                SellSheet ssMast = null;
                ssMast = (from d in context.SellSheetSet
                          where d.Sell_Sheet_ID == SheetID
                          select d).First();

                if (!String.IsNullOrEmpty(ssMast.Sell_Sheet_Name))
                    strSheetName = ssMast.Sell_Sheet_Name.ToString();

            }

          //  txtSSName.Text = strSheetName.ToString();
            frmDiv.Visible = true;
            Msglbl.Visible = true;
            Msglbl.Text = "";
            Msglbl.Style.Add("color", "none"); 
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Int32 SheetID = Convert.ToInt32(HttpContext.Current.Request.QueryString["Sell_Sheet_ID"]);

        if (String.IsNullOrEmpty(txtSSName.Text.ToString()))
        {
            Msglbl.Style.Add("color","RED"); 
            Msglbl.Text = "Please enter sell sheet name.";
            Msglbl.Visible = true;
        }
        else
        {
            using (PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
            {
                SellSheet ssSheet = null;
                ssSheet = (from d in context.SellSheetSet
                           where d.Sell_Sheet_ID == SheetID 
                           select d).FirstOrDefault();
                
                ssSheet.Sell_Sheet_Name =  txtSSName.Text.ToString();
                ssSheet.Status_ID = 2; //For completed status. 
                ssSheet.Modified_DT = DateTime.Now;
                ssSheet.Modified_BY = Pinsonault.Web.Session.FullName;
                context.SaveChanges(); 
            }

            frmDiv.Visible = false;
            Msglbl.Text = "Sell Sheet has been successfully saved to My Sell Sheets.";
            Msglbl.Style.Add("color", "none"); 
            Msglbl.Visible = true;

            //Calls Javascript function RefreshPlanSelection() to refresh plan selection list parent grid.
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "RefreshPage", "RefreshPage();", true);

        }
    }

}
