using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dundas.Charting.WebControl;
using System.Data;
using System.Collections;
using System.Data.Objects;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.Serialization.Json;
using PathfinderModel;
using Pathfinder;
using System.Data.Common;
using Pinsonault.Application.MarketplaceAnalytics;

public partial class marketplace_controls_Chart : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //populate plan name in second chart section if the requested report is affiliations report
        if (!string.IsNullOrEmpty(Request.QueryString["Plan_ID"]) && Path.GetFileName(Request.FilePath) == "ma_affiliations.aspx")
        {          
            lbl2.Text = Resources.Resource.Label_Plan_Level;
        }
        
    }
}

