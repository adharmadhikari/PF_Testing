using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using PathfinderModel;

public partial class standardreports_controls_FilterTierSelection : System.Web.UI.UserControl
{
    protected override void OnLoad(EventArgs e)
    {
        Pinsonault.Web.Support.RegisterTierScriptVariable(this.Page);

        Pinsonault.Web.Support.RegisterComponentWithClientManager(this.Page, Tier_ID.ClientID, null, "moduleOptionsContainer");


        base.OnLoad(e);
    }
}
