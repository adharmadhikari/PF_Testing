using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pinsonault.Application.Alcon;
using System.Text;
using System.Collections.Generic;

public partial class custom_controls_FilterMeetingActivity : System.Web.UI.UserControl
{
    protected override void OnLoad(EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Activity_Type_ID.ClientID, null, "moduleOptionsContainer");

        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "_activityTypeSet", getActivityTypeScript(), true);

        Activity_Type_ID.OnClientLoad = "function(s,a){ $createCheckboxDropdown(s.get_id(), 'ActivityTypeIDList', activityTypeSet, null, null, 'moduleOptionsContainer'); }";

        base.OnLoad(e);
    }

    static string getActivityTypeScript()
    {
        StringBuilder sb = new StringBuilder("[{");
        sb.AppendFormat("id:0,text:\"{0}\"", "All");
        sb.Append("}");

        using (PathfinderAlconEntities context = new PathfinderAlconEntities())
        {
            int ClientID = Pinsonault.Web.Session.ClientID;
            Dictionary<Int32, String> activityType = null;

            if (!Pinsonault.Web.Session.Admin) //get regions (User_Level null (old) or User_Level == 2)
                activityType = context.DailyActivityTypeSet.Select(t => new { ID = t.Activity_Type_ID, Name = t.Activity_Type_Name }).OrderBy(t => t.Name).ToDictionary(s => s.ID, s => s.Name);

            foreach (KeyValuePair<Int32, String> entry in activityType)
            {   
                sb.Append(",");

                sb.AppendFormat("{0}id:{1},text:\"{2}\"{3}", "{", entry.Key, entry.Value, "}");                    
            }
        }
        sb.Append("]");

        return string.Format("var activityTypeSet={0};", sb.ToString());
    }
}
