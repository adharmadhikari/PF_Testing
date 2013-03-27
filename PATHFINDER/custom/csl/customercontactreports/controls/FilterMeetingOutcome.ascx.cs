using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderClientModel;
using Pinsonault.Web;

public partial class custom_controls_FilterMeetingOctcome : System.Web.UI.UserControl
{
    public custom_controls_FilterMeetingOctcome()
    {
        ContainerID = "moduleOptionsContainer";
        //MaxMeetingOutCome = 5;
    }
    public string ContainerID { get; set; }
    //public int MaxMeetingOutCome { get; set; }

    protected override void OnPreRender(EventArgs e)
    {
        //creates the drop down list for Products Discussed based on the Drug_Name
        using (PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
        {
            var q = (from p in context.LkpMeetingOutcomeSet
                    orderby p.Sort_Index 
                     select p).ToList().Select(p => new GenericListItem { ID = p.Meeting_Outcome_ID.ToString(), Name = p.Meeting_Outcome_Name });
              if (q != null)
              {
                  Pinsonault.Web.Support.RegisterGenericListVariable(this.Page, q.ToArray(), "meetingOutcome");
              }
        }
        base.OnPreRender(e);
    }
    protected override void OnLoad(EventArgs e)
    {
        Pinsonault.Web.Support.RegisterTierScriptVariable(this.Page);
        Pinsonault.Web.Support.RegisterComponentWithClientManager(this.Page, Meeting_Outcome_ID.ClientID, null, "moduleOptionsContainer");
       // Meeting_Outcome_ID.OnClientLoad = "function(s,a){$createCheckboxDropdown(s.get_id(), 'MeetingOutcomeIDOptionList', meetingOutcome, {'maxItems':" + MaxMeetingOutCome.ToString() + ",'defaultText':'" + Resources.Resource.Label_No_Selection + "'}, {'error':function(){$alert('Maximum " + MaxMeetingOutCome.ToString() + " meeting outcome can be selected.');}}, 'moduleOptionsContainer');}";
        Meeting_Outcome_ID.OnClientLoad = "function(s,a){$createCheckboxDropdown(s.get_id(), 'MeetingOutcomeIDOptionList', meetingOutcome , {'defaultText': '--All outcomes--', 'multiItemText': '" + Resources.Resource.Label_Multiple_Selection + "' }, null, 'moduleOptionsContainer'); var Meeting_Outcome_ID = $get('MeetingOutcomeIDOptionList').control; $loadPinsoListItems(Meeting_Outcome_ID, meetingOutcome, null, -1);}";
        
        base.OnLoad(e);
    }
}
