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

public partial class standardreports_controls_FilterSection : System.Web.UI.UserControl, IFilterControl
{
    public standardreports_controls_FilterSection()
    {
        ContainerID = "moduleOptionsContainer";
        MaxSection = 7;
    }

    public string ContainerID { get; set; }
    public int MaxSection { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {

        using (PathfinderEntities context = new PathfinderEntities())
        {
            int clientID = Pinsonault.Web.Session.ClientID;
            int applicationID = Identifiers.StandardReports;
            string module = Request.QueryString["module"];

            var channels = (from c in context.ClientApplicationAccessSet
                            join s in context.SectionSet on
                            c.SectionID equals s.ID
                            where c.ClientID == clientID
                            where c.ApplicationID == applicationID
                            orderby s.Sort_Order
                            select s).ToList().Select(s => new GenericListItem { ID = s.ID.ToString(), Name = s.Name.ToString() });

            if (MaxSection == 1)
                channels = channels.Where(c => c.ID != "0").ToList();

            //if (module == "geographiccoverage")
            //    channels = channels.Where(c => c.ID != "4").ToList();
            if (module == "affiliationsformulary")
            {
                channels = channels.Where(c => c.ID != "9").ToList();
                channels = channels.Where(c => c.ID != "11").ToList();
                channels = channels.Where(c => c.ID != "12").ToList();
            }

            if (channels != null)
            {
                Pinsonault.Web.Support.RegisterGenericListVariable(this.Page, channels.ToArray(), "channelsList");
            }
            Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Section_ID.ClientID, null, "moduleOptionsContainer");

            //if Max section =1 i.e a single selection dropdown else multiselect check box dropdown
            if (MaxSection > 1)
                Section_ID.OnClientLoad = "function(s,a){$createCheckboxDropdown(s.get_id(), 'MarketSegmentIDOptionList', channelsList, {'maxItems':" + MaxSection.ToString() + ", 'multiItemText': '" + Resources.Resource.Label_Multiple_Selection + "' }, {'error':function(){$alert('Maximum " + MaxSection.ToString() + " sections should be selected.', 'Report Filters');}},{itemClicked:onMarketSegmentItemClicked}, 'moduleOptionsContainer');resetSectionSelection('MarketSegmentIDOptionList',0);}";
            else if (MaxSection == 1)
            {
                Section_ID.OnClientLoad = "function(s,a){$loadListItems($find('" + Section_ID.ClientID + "'), channelsList,null, channelsList.length>1 ? -1 : channelsList[0].ID);LoadParentPlanListBySection(s,a);}";
                Section_ID.OnClientSelectedIndexChanged = "function(s,a){LoadParentPlanListBySection(s,a);}";
            }
            
        }
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
