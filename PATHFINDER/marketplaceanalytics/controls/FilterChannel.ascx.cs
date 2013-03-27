using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderModel;

public partial class todaysanalytics_controls_FilterChannel : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Request.QueryString["Module"] == "tiercoveragecomparison")
        //{
        //    Section_ID.DataBind();
        //    //remove state medicaid option from drop down if module is tier coverage
        //    if (Section_ID.FindItemByValue("9") != null)
        //    {
        //        Section_ID.FindItemByValue("9").Remove();
        //    }
        //}

        using (PathfinderEntities context = new PathfinderEntities())
        {
            int clientID = Pinsonault.Web.Session.ClientID;

            var channels = from c in context.ClientApplicationAccessSet
                           join s in context.SectionSet on
                           c.SectionID equals s.ID
                           where c.ClientID == clientID
                           where c.ApplicationID == 2
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
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Plan_ID.ClientID, null, "moduleOptionsContainer");

        string frag = "switch(parseInt(gt,10)) {case 4: l.control.set_visible(true);break; default: l.control.set_visible(false);break; }";
        
        //Disable Rollup Type if Combined (-1) channel is selected
        //onSectionListChanged is located in FiltersContainerScript.ascx
        Section_ID.OnClientSelectedIndexChanged = "function(s,a){onSectionListChanged(s,a);var comboRollup = $find('ctl00_partialPage_filtersContainer_Rollup_Rollup_Type'); var comboGeoRegion = $find('ctl00_partialPage_filtersContainer_Geography_Geography_Type'); var comboGeoState = $find('ctl00_partialPage_filtersContainer_Geography_State_ID'); if(s.get_value() == -1){if (comboRollup.get_value() != 4){comboRollup.findItemByValue(1).select(); comboRollup.disable();} } else comboRollup.enable(); if(s.get_value() == 4){comboGeoRegion.findItemByValue(1).select(); comboGeoRegion.disable(); comboGeoState.disable();}else{comboGeoRegion.enable(); comboGeoState.enable();}}";
        Plan_ID.OnClientLoad = "function(s,a){ var l=s.get_element();var gt = $find('ctl00_partialPage_filtersContainer_Rollup_Rollup_Type').get_value(); " + frag + "}";
    }
}
