using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class standardreports_controls_FilterAccountType : System.Web.UI.UserControl
{
    public string ContainerID { get; set; }
    
    public standardreports_controls_FilterAccountType()
    {

        ContainerID = "moduleOptionsContainer";

    }
    protected override void OnLoad(EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Class_Partition.ClientID, null, ContainerID);
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Segment_ID.ClientID, null, ContainerID);
        
        string frag = "SetGeography(s,a)";

        Class_Partition.OnClientLoad = "function(s,a){" + frag + "}";
        Class_Partition.OnClientSelectedIndexChanged = "function(s,a){" + frag + "}";

        base.OnLoad(e);
    }
}
