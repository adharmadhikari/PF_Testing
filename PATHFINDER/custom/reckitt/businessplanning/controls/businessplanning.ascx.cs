using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class custom_reckitt_businessplanning_controls_businessplanning : System.Web.UI.UserControl
{
    private Boolean MktSegmentChanged;
    public custom_reckitt_businessplanning_controls_businessplanning()
    {
        ContainerID = "moduleOptionsContainer";
    }
    public string ContainerID { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Segment_ID.ClientID, null, ContainerID);
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Plan_ID.ClientID, null, ContainerID);

        Dictionary<string, PathfinderModel.ClientApplicationAccess> access = Pinsonault.Web.Session.ClientApplicationAccess;
        //Select all available Section IDs for Todays Accounts
        Segment_ID.DataSource = access.Where(i => i.Value.ApplicationID == 1 && i.Value.Section != null && ((i.Value.SectionID == 1) || (i.Value.SectionID == 4) || (i.Value.SectionID == 9) || (i.Value.SectionID == 11) || (i.Value.SectionID == 12))).Select(i => new { ID = i.Value.SectionID, Name = i.Value.Section.Name });
        Segment_ID.DataBind();
        


    }    

}
