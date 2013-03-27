using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderModel;

public partial class custom_pinso_formularyhistoryreporting_controls_FilterDisplayOptions : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Display_ID.ClientID, null, "moduleOptionsContainer");
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, chk_ViewChangesOnly.ClientID, null, "moduleOptionsContainer");
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Is_Predominant.ClientID, null, "moduleOptionsContainer");
        
    }
}
