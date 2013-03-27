using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using PathfinderClientModel;
using System.Text;

public partial class custom_Alcon_activityreporting_controls_filteraccountmanagers : System.Web.UI.UserControl
{
    public custom_Alcon_activityreporting_controls_filteraccountmanagers()
    {
        ContainerID = "moduleOptionsContainer";
    }
    public string ContainerID { get; set; }

    protected override void OnInit(EventArgs e)
    {
        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "_accountManagersSet", getAccountManagersScript(), true);

        User_ID.OnClientLoad = "function(s,a){ $createCheckboxDropdown(s.get_id(), 'UserIDList', accountManagersSet, null, null, 'moduleOptionsContainer'); }";
        
        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, User_ID.ClientID, null, ContainerID);
       
    }

    static string getAccountManagersScript()
    {
        StringBuilder sb = new StringBuilder("[{");
        sb.AppendFormat("id:0,text:\"{0}\"", "All");
        sb.Append("}");

        using (PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
        {
            int ClientID = Pinsonault.Web.Session.ClientID;
            Dictionary<Int32, String> accountManagers = null;

            accountManagers = context.AccountManagerSet.OrderBy(t => t.User_L_Name).Select(t => new { ID = t.User_ID, Name = t.FullName }).ToDictionary(s => s.ID, s => s.Name);

            foreach (KeyValuePair<Int32, String> entry in accountManagers)
            {
                sb.Append(",");

                sb.AppendFormat("{0}id:{1},text:\"{2}\"{3}", "{", entry.Key, entry.Value, "}");
            }
        }
        sb.Append("]");

        return string.Format("var accountManagersSet={0};", sb.ToString());
    }
}
