
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Data.Objects;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.Serialization.Json;
using PathfinderModel;
using Pathfinder;

public partial class custom_controls_ccrMeetingActivityDrillDown : System.Web.UI.UserControl
{

    protected void Page_Load(object sender, EventArgs e)
    {
        gridCcrMeetingActivityDrillDown.ClientSettings.DataBinding.Location = "~/custom/" + Pinsonault.Web.Session.ClientKey + "/customercontactreports/services/PathfinderDataService.svc";
    }

    
}
