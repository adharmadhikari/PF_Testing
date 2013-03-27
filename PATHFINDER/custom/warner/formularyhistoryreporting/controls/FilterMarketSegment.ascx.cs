using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderModel;
using Pinsonault.Web;
using Telerik.Web.UI;
using Pinsonault.Web.UI;

public partial class custom_warner_formularyhistoryreporting_controls_FilterMarketSegment : System.Web.UI.UserControl, IFilterControl
{
    protected void Page_Load(object sender, EventArgs e)
    {        

        using (PathfinderEntities context = new PathfinderEntities())
        {
            int clientID = Pinsonault.Web.Session.ClientID;

            //get the list of Sections for a selected client for App_id = 14 -- formulary history reporting

            var channels = from c in context.ClientApplicationAccessSet
                           join s in context.SectionSet on
                           c.SectionID equals s.ID
                           where c.ClientID == clientID
                           where c.ApplicationID == 14
                           where c.SectionID != 0                           
                           select new
                           {
                               s.ID,
                               s.Name
                           };

            Section_ID.DataSource = channels.OrderBy(s => s.Name);
            Section_ID.DataBind();
        }
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Section_ID.ClientID, null, "moduleOptionsContainer");
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, rcbGeographyType.ClientID, null, "moduleOptionsContainer");
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Geography_ID.ClientID, null, "moduleOptionsContainer");

        //script to change the PlanList based on section and geography selection
        
        Geography_ID.OnClientLoad = "function(s, a){GeoLoad(s, a)}";
        rcbGeographyType.OnClientLoad = "function(s,a) {GeoTypeLoad(s,a)}";
        rcbGeographyType.OnClientSelectedIndexChanged = "function(s, a){geoSelectedIndexChanged (s, a)}";
        //Geography_ID.OnClientSelectedIndexChanged = "function(s, a){geoIDSelectedIndexChanged (s,a)}";

        RadComboBoxItem item = rcbGeographyType.Items.FindItemByValue("2");
        if (item != null)
        {
            item.Visible = Context.User.IsInRole("sr_rgns");
        }

        string strSectionLoad = "{get_fhr_SectionLoad(s,a)}";

        Section_ID.OnClientLoad = string.Format("function(s,a){0}", strSectionLoad);
        
        Section_ID.OnClientSelectedIndexChanged = string.Format("function(s,a){0}", strSectionLoad);
       
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
