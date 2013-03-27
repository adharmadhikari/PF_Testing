using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class controls_CoveredLivesMedDState : System.Web.UI.UserControl
{
    public string ContainerID { get; set; }

    public bool ShowTotalCoveredLives { get; set; }

    public controls_CoveredLivesMedDState()
    {
        ShowTotalCoveredLives = true;
    }

    protected override void OnLoad(EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(this.Page, gridProductCoveredLives.ClientID, "{}", ContainerID);
        base.OnLoad(e);
    }
}
