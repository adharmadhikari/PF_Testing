using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class custom_controls_FilterDrugSelection : System.Web.UI.UserControl
{
    public custom_controls_FilterDrugSelection()
    {
        ContainerID = "moduleOptionsContainer";        
    }

    public string ContainerID { get; set; }
    public int MaxDrugs { get; set; }

    protected override void OnLoad(EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Thera_ID.ClientID, null, ContainerID);
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Drug_ID.ClientID, null, ContainerID);
        //Thera_ID.SelectedIndex = 0;
        base.OnLoad(e);
    }
}
