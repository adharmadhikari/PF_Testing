using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class custom_merz_businessplanning_controls_filteraccountmanagers : System.Web.UI.UserControl
{
    public custom_merz_businessplanning_controls_filteraccountmanagers()
    {
        ContainerID = "moduleOptionsContainer";
    }
    public string ContainerID { get; set; }

    public bool IncludeAll { get; set; }

    protected override void OnInit(EventArgs e)
    {
        dsAcctMgr.ConnectionString = Pinsonault.Web.Session.ClientConnectionString;

        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, User_ID.ClientID, null, ContainerID);

        if (IncludeAll)
        {
            RadComboBoxItem itemAM = new RadComboBoxItem("--All Account Managers--");
            User_ID.Items.Add(itemAM);
        }
    }
    
}
