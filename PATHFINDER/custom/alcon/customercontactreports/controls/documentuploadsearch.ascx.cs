using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class custom_Alcon_customercontactreports_Controls_documentuploadsearch : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        gridCCDocuments.ClientSettings.DataBinding.Location = "~/custom/" + Pinsonault.Web.Session.ClientKey + "/customercontactreports/services/AlconDataService.svc";
        //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "maxAge", "<script language='javascript'>var maxAge='" + ConfigurationManager.AppSettings["CCRDocumentFolder"].ToString() + "';</script>");
        //ClientScript.RegisterClientScriptBlock(this.GetType(), "maxAge", "<script language='javascript'>var maxAge='" + ConfigurationManager.AppSettings["test"].ToString() + "';</script>");
    }

}
