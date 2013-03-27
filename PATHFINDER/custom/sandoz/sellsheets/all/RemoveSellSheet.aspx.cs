using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderClientModel;
using Pinsonault.Web; 

public partial class custom_pinso_all_RemoveSellSheet :  PageBase
{
    protected override void OnInit(EventArgs e)
    {
        dsSellSheets.ConnectionString = Pinsonault.Web.Session.ClientConnectionString;
        //titleText.Text = "Remove " + Request.QueryString["SellSheetDesc"] + " Sell Sheet";
        titleText.Text = "Remove Selected Sell Sheet";
        base.OnInit(e);
    }

      protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.formRemoveSellSheet.Visible = true;
            this.Msglbl.Visible = false;
        }
    }

    //Called when "Yes" button is clicked.
    protected void Yesbtn_Click(object sender, EventArgs e)
    {
        //Update sell sheet status from active(1) to inactive(3). It doesn't delete the sellsheet physically from the database.
        using ( PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString) )
        {
            ////Updates Sell_Sheet_Mast.Status_ID field to 3(i.e. Inactive).
            if (CustomDataService.RemoveSellSheet(System.Convert.ToInt32(Page.Request.QueryString["Sell_Sheet_ID"]), context))
            {
                //Confirmation message is displayed after Delete.
                this.formRemoveSellSheet.Visible = false;
                this.Msglbl.Text = "<div>Selected sell sheet has been removed successfully.</div>";
                this.Msglbl.Visible = true;
            }
        }

        //Calls Javascript function RefreshMySSList() to refresh sell sheet grids.
        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "RefreshMySSList", "RefreshMySSList();", true);  
    }

}
