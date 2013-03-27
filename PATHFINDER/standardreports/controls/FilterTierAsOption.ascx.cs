using System;
using System.Text;
using System.Web.UI;
using PathfinderModel;

public partial class standardreports_controls_FilterTierAsOption : System.Web.UI.UserControl
{
    protected override void OnLoad(EventArgs e)
    {
        Pinsonault.Web.Support.RegisterTierScriptVariable(this.Page);

        Pinsonault.Web.Support.RegisterComponentWithClientManager(this.Page, Tier_ID.ClientID, null, "moduleOptionsContainer");


        base.OnLoad(e);
    }
}
