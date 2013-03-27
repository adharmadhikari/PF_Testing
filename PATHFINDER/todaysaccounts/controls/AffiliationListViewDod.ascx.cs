using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class todaysaccounts_controls_AffiliationListViewDod : System.Web.UI.UserControl
{
    public string ContainerID { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(this.Page, gridAffiliations.ClientID, "{}", ContainerID);

        string plan_id = Request.QueryString["plan_Id"];
    }
}
