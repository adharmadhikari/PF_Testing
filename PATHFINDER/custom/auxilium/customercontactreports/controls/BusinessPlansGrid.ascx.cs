using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class custom_auxilium_customercontactreports_controls_BusinessPlansGrid : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
         gridCCDocuments.ClientSettings.DataBinding.Location = "~/custom/" + Pinsonault.Web.Session.ClientKey + "/customercontactreports/services/PathfinderDataService.svc";
        //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "maxAge", "<script language='javascript'>var maxAge='" + ConfigurationManager.AppSettings["CCRDocumentFolder"].ToString() + "';</script>");
        //ClientScript.RegisterClientScriptBlock(this.GetType(), "maxAge", "<script language='javascript'>var maxAge='" + ConfigurationManager.AppSettings["test"].ToString() + "';</script>");

    }
}
