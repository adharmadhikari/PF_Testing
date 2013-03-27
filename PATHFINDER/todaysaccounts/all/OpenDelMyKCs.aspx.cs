using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class todaysaccounts_all_OpenDelMyKCs : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.formDelKC.Visible = true;
            this.Msglbl.Visible = false;

            //Updates Plan Name hidden variable based on Querystring.
            this.PlanNameHdn.Value = System.Web.HttpContext.Current.Request.QueryString["PlanName"];

            //Sets the form header with PlanName included.
            this.titleText.Text = this.PlanNameHdn.Value + " - " + this.titleText.Text;
        }
    }

    //Called when "Yes" button is clicked.
    protected void Yesbtn_Click(object sender, EventArgs e)
    {
        //Update contact status from active to inactive. It doesn't delete the contact physically from the database.
        using (PathfinderClientModel.PathfinderClientEntities context = new PathfinderClientModel.PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
        {
            //Updates Key_Contacts.Status field to False.
            if (Pinsonault.Application.TodaysAccounts.TodaysAccountsClientDataService.DeleteContact(System.Convert.ToInt32(Page.Request.QueryString["KCID"]), context))
            {

                //Confirmation message is displayed after Delete.
                this.formDelKC.Visible = false;
                this.Msglbl.Text = "<br/><b>Key contact has been deleted successfully.</b><br/><br/>";
                this.Msglbl.Visible = true;
            }
        }

        //Calls Javascript function RefreshMyKCs() to refresh my key contacts parent grid.
        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "RefreshMyKCs", "RefreshMyKCs();", true);  
    }
}
