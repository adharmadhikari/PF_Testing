using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using PathfinderModel;


public partial class custom_controls_FilterRegion : System.Web.UI.UserControl
{
    public custom_controls_FilterRegion()
    {
        ContainerID = "moduleOptionsContainer";
    }
    public string ContainerID { get; set; }
    protected override void OnLoad(EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Territory_ID.ClientID, null, ContainerID);

        int ClientID = Pinsonault.Web.Session.ClientID;

        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "_regionSet", getRegionScript(), true);
        
        Territory_ID.OnClientLoad = "function(s,a){ $createCheckboxDropdown(s.get_id(), 'GeographyIDList', regionSet, null, null, 'moduleOptionsContainer'); }";

        base.OnLoad(e);
    }

    static string getRegionScript()
    {
        StringBuilder sb = new StringBuilder("[{");
        sb.AppendFormat("id:0,text:\"{0}\"", "All");
        sb.Append("}");

        using (PathfinderEntities context = new PathfinderEntities())
        {
            int ClientID = Pinsonault.Web.Session.ClientID;
            Dictionary<string, string> territories = null;

            if (!Pinsonault.Web.Session.Admin) //get regions (User_Level null (old) or User_Level == 2)
                territories = context.TerritorySet.Where(t => (t.User_Level == null || t.User_Level == 2) && t.Client_ID == ClientID).Select(t => new { ID = t.Territory_ID, Name = t.Territory_Name }).ToDictionary(s => s.ID, s => s.Name);
            
            foreach (KeyValuePair<String,String> entry in territories)
            {
                sb.Append(",");

                sb.AppendFormat("{0}id:\"{1}\",text:\"{2}\"{3}", "{", entry.Key, entry.Value, "}");
            }
        }
        sb.Append("]");

        return string.Format("var regionSet={0};", sb.ToString());
    }

    

    #region IFilterControl Members

    string _defaultValue;
    public string DefaultValue
    {
        get
        {
            if (_defaultValue == null)
                return string.Empty;

            return _defaultValue;
        }
        set
        {
            _defaultValue = value;
        }
    }

    #endregion
}
