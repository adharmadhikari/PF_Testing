using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pinsonault.Application.Dey;


public partial class custom_dey_sellsheets_EditAddress : PageBase
{
    public int SheetID { get; set; }
    public int RepID { get; set; }

    protected override void OnInit(EventArgs e)
    {
        //dsLkpStates.ConnectionString = Pinsonault.Web.Session.ClientConnectionString;
        base.OnInit(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        SheetID = Convert.ToInt32(Request.QueryString["sheet_ID"]);
        RepID = Convert.ToInt32(Request.QueryString["Rep_ID"]);
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        using (PathfinderDeyEntities context = new PathfinderDeyEntities())
        {
            Pinsonault.Application.Dey.SellSheetOrders ss_order = new Pinsonault.Application.Dey.SellSheetOrders()
            {
                Rep_ID = RepID,
                Sell_Sheet_ID = SheetID,
                Ship_Address1 = Addr1.Text,
                Ship_Address2 = Addr2.Text,
                Ship_City = City.Text,
                //Ship_State = rdcmbState.SelectedValue,
                Ship_Zip = Zip.Text,
                Created_BY = Pinsonault.Web.Session.FullName,
                Created_DT = DateTime.UtcNow
            };
            context.AddToSellSheetOrdersSet(ss_order);
            context.SaveChanges();
            int test = ss_order.Order_ID;
        }
    }
}
