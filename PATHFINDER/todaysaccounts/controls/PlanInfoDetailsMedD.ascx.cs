using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class todaysaccounts_controls_PlanInfoMedD : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {        
        string strProdID = Request.QueryString["Prod_ID"];

        if (string.IsNullOrEmpty(strProdID))
        {                    
            BDHeader2.Visible = false;
            formView1.Visible = false;
        }
        else
        {         
            BDHeader2.Visible = true;
            formView1.Visible = true;           
        }
    }
      
}
