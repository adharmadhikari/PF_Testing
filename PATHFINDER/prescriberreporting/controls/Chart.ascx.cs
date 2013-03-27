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
        using (PathfinderMarketplaceAnalyticsEntities context = new PathfinderMarketplaceAnalyticsEntities(Pinsonault.Web.Session.GetClientApplicationConnectionString("MarketplaceAnalytics")))
        {
            // based on client, Territory_Levels are different (only 3 used out of Territory/District/Region/Area
            // table: tr.Lkp_MS_Territory_Levels

            var tQ = from q in context.LkpMSTerritoryLevelsSet
                     orderby q.MS_Territory_Level_ID
                     select new { tID = q.MS_Territory_Level_ID, tName = q.MS_Territory_Level_Name };

            if (tQ != null)
            {
                foreach (var i in tQ)
                {
                    try
                    {
                        if (i.tID == 1)
                            lbl1.Text = i.tName;
                        if (i.tID == 2)
                            lbl2.Text = i.tName;
                        if (i.tID == 3)
                            lbl3.Text = i.tName;
                    }
                    catch (Exception ex)
                    {
                        throw new HttpException(500, ex.Message);
                    }
                }
            }
        }        
    }
}

