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

public partial class custom_auxilium_customercontactreports_controls_CCRGridView : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //PathfinderApplication.Support.RegisterComponentWithClientManager(Page, gridCCReports.ClientID);
        gridCCReports.ClientSettings.DataBinding.Location = "~/custom/" + Pinsonault.Web.Session.ClientKey + "/customercontactreports/services/PathfinderDataService.svc";
    }
}
