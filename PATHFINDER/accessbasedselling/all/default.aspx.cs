using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class accessbasedselling_all_default : PageBase
{
    protected override void OnLoad(EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(this, rcbTopPayers.ClientID);
        //Pinsonault.Web.Support.RegisterComponentWithClientManager(this, rcbTopPrescribers.ClientID);

        gridPrescribers.Columns[0].Visible = Pinsonault.Web.Session.UserID != 46;
        gridCompAnalysis.Columns[0].Visible = Pinsonault.Web.Session.UserID != 46;


        base.OnLoad(e);
    }
}
