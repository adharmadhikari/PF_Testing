using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class standardreports_controls_filtertieraffiliationtype : System.Web.UI.UserControl
{
    
    public standardreports_controls_filtertieraffiliationtype()
    {
        ContainerID = "moduleOptionsContainer";
    }

    public string ContainerID { get; set; }       

    protected override void OnInit(EventArgs e)
    {
        
        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Parent_ID.ClientID, null, ContainerID);
        Parent_ID.OnClientLoad = "function(s,a){refreshParentPlanList(-1);}"; //parent plans are populated from ParentPlanSet in PathfinderEntities

    }

}
