using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderClientModel;
using Pinsonault.Application.Millennium;
using System.IO;
using Pinsonault.Data;
using System.Linq.Expressions;

public partial class custom_Millennium_customercontactreports_controls_deleteplan : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        this.Msglbl.Visible = false;
    }
    protected void Yesbtn_Click(object sender, EventArgs e)
    {
       
        using (PathfinderMillenniumEntities context = new PathfinderMillenniumEntities())
        {

            if (DeletePlan(System.Convert.ToInt32(Page.Request.QueryString["Plan_ID"])))
            {
                
                this.Msglbl.Text = "<div>Selected Plan has been deleted successfully.</div>";
                this.Msglbl.Visible = true;

               Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "RefreshPlan", "RefreshPlan();", true);
            }

        }
    }
       

    public bool DeletePlan(int Plan_ID)
    {


        if (Plan_ID != null)
        {
            using (PathfinderMillenniumEntities context = new PathfinderMillenniumEntities())
            {
                PlansClient customplan;
                customplan = context.PlansClientSet.FirstOrDefault(p => p.Plan_ID == Plan_ID);
                customplan.Status = false;
                customplan.Modified_DT = DateTime.UtcNow;
                customplan.Modified_BY = Pinsonault.Web.Session.FullName;
                 context.SaveChanges();
           
            }
            
            return true;
        }

        return false;

      
    }
}
