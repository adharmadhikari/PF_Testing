using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderModel;
using Pinsonault.Web;

public partial class restrictionsreport_controls_FilterMarketSegment : System.Web.UI.UserControl
{
    protected override void OnPreRender(EventArgs e)
    {


        //creates the drop down list for Market Segment
        using (PathfinderEntities context = new PathfinderEntities())
        {
            int clientID = Pinsonault.Web.Session.ClientID;
            var q = (from a in context.ClientApplicationAccessSet
                     join s in context.SectionSet on
                     a.SectionID equals s.ID
                     orderby s.Name
                     where a.ApplicationID == 21 &&
                            a.ClientID == clientID //&&
                     //a.SectionID != 0
                     select s).ToList().Select(p => new GenericListItem { ID = p.ID.ToString(), Name = p.Name });
            if (q != null)
            {
                string module = Request.QueryString["module"];
                //remove All selection for regular restrictions report
                if (module == "deyrestrictionsreport")
                {
                    q = q.Where(item => item.ID != "0");
                }
                Pinsonault.Web.Support.RegisterGenericListVariable(this.Page, q.OrderBy(p => p.Name).ToArray(), "marketSegment");
            }
        }
        base.OnPreRender(e);
    }


    protected override void OnLoad(EventArgs e)
    {
        Pinsonault.Web.Support.RegisterTierScriptVariable(this.Page);
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Section_ID.ClientID, null, "moduleOptionsContainer");
        //Section_ID.OnClientLoad = "function(s,a){$createCheckboxDropdown(s.get_id(), 'MarketSegmentIDOptionList', marketSegment, {'defaultText': '--All Market Segments--', 'multiItemText': '" + Resources.Resource.Label_Multiple_Selection + "' }, null, 'moduleOptionsContainer'); var market_segment_id = $get('MarketSegmentIDOptionList').control; $loadPinsoListItems(market_segment_id, marketSegment, null, -1);}";
        if (Request.QueryString["module"] == "deyrestrictionsreport")
        {
            Section_ID.OnClientLoad = "function(s,a){$createCheckboxDropdown(s.get_id(), 'MarketSegmentIDOptionList', marketSegment, {'defaultText': '--No Selection--' }, {itemClicked:onMarketSegmentClicked}, 'moduleOptionsContainer'); var market_segment_id = $get('MarketSegmentIDOptionList').control; $loadPinsoListItems(market_segment_id, marketSegment, null, -1);}";
        }
        else
        {
            //remove validator from markup
            client_validator.InnerHtml = "";
            Section_ID.OnClientLoad = "function(s,a){$createCheckboxDropdown(s.get_id(), 'MarketSegmentIDOptionList', marketSegment, {'defaultText': '--No Selection--' }, {itemClicked:onMarketSegmentClicked}, 'moduleOptionsContainer'); var market_segment_id = $get('MarketSegmentIDOptionList').control; $loadPinsoListItems(market_segment_id, marketSegment, null, 0);}";
        }
        base.OnLoad(e);
    }

}
