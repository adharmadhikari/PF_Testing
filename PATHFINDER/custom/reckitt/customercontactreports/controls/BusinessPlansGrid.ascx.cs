using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class custom_controls_BusinessPlansGrid : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        gridCCDocuments.ClientSettings.DataBinding.Location = "~/custom/" + Pinsonault.Web.Session.ClientKey + "/customercontactreports/services/PathfinderDataService.svc";
        //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "maxAge", "<script language='javascript'>var maxAge='" + ConfigurationManager.AppSettings["CCRDocumentFolder"].ToString() + "';</script>");
        //ClientScript.RegisterClientScriptBlock(this.GetType(), "maxAge", "<script language='javascript'>var maxAge='" + ConfigurationManager.AppSettings["test"].ToString() + "';</script>");
    }
}
