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

public partial class custom_controls_FilterAccount : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        searchlist.ServiceUrl = string.Format("custom/{0}/customercontactreports/services/GSKDataService.svc/PlanSearchSet", Pinsonault.Web.Session.ClientKey);
    }
}

    
