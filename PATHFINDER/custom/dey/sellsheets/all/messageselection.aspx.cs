using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using PathfinderClientModel;
using Pinsonault.Web;
using System.IO;

public partial class custom_unitedthera_sellsheets_messageselection : InputFormBase
{
    protected string PageModule { get; set; }

    protected override void OnInit(EventArgs e)
    {
        PageModule = Path.GetFileNameWithoutExtension(Request.Url.AbsolutePath);
        dsSellSheetMessages.ConnectionString = Pinsonault.Web.Session.ClientConnectionString;
        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Int32 ssid = Convert.ToInt32(Request.QueryString["Sell_Sheet_ID"]);

            using (PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
            {
                SellSheet ss = null; 
                ss = (from d in context.SellSheetSet
                      where d.Sell_Sheet_ID == ssid
                      select d).FirstOrDefault();

                //if (ss.Message_ID != null) rblMessage.SelectedValue = Convert.ToString(ss.Message_ID);

                chkGeo.Checked = ss.Include_Territory_Name;
                //chkHighlightProd.Checked = ss.Is_Highlighted;
            }
        }
    }

    protected override bool IsRequestValid()
    {
        return true;
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        try
        {
            Int32 ssid = Convert.ToInt32(Request.QueryString["Sell_Sheet_ID"]);
            
            using (PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
            {
                SellSheet ssheet = null;
                ssheet = (from d in context.SellSheetSet
                          where d.Sell_Sheet_ID == ssid
                          select d).FirstOrDefault();
                //ssheet.Message_ID = Convert.ToInt32(rblMessage.SelectedValue);
                ssheet.Modified_BY = Pinsonault.Web.Session.FullName;
                ssheet.Modified_DT = DateTime.UtcNow;
                ssheet.Territory_ID = Pinsonault.Web.Session.TerritoryID;
                ssheet.Include_Territory_Name = chkGeo.Checked;
                //ssheet.Is_Highlighted = chkHighlightProd.Checked; 

                //Logic below is to check if current step needs to be updated
                if (Master.RequestedStep == Master.CurrentStep)
                {
                    string nextStep = (from ss in context.SellSheetStepSet
                                       where ss.Step_Order == (Master.RequestedStep + 1)
                                       select ss.Step_Key).FirstOrDefault();
                    ssheet.Current_Step = nextStep;
                }

                context.SaveChanges();
            }
            PostBackResult.Success = true;
        }
        catch
        {
            PostBackResult.Success = false;
        }
    }
}
