using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class standardreports_controls_filterbenefitdesign : System.Web.UI.UserControl
{
     public string ContainerID { get; set; }

     public standardreports_controls_filterbenefitdesign()
    {

        ContainerID = "moduleOptionsContainer";

    }

    protected override void OnLoad(EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Is_Predominant.ClientID, null, ContainerID);
        base.OnLoad(e);
    }
}
