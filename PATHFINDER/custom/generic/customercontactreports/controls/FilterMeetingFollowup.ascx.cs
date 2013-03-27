using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderClientModel;
using Pinsonault.Web;

public partial class custom_controls_FilterMeetingFollowup : System.Web.UI.UserControl
{    
    public custom_controls_FilterMeetingFollowup()
    {
        ContainerID = "moduleOptionsContainer";
        //MaxNotes = 5;
    }
    public string ContainerID { get; set; }
    //public int MaxNotes { get; set; }

    protected override void OnPreRender(EventArgs e)
    {
        //creates the drop down list for Products Discussed based on the Drug_Name
        using (PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
        {
            var q = (from p in context.LkpFollowupNotesSet
                    orderby p.Followup_Notes_Name 
                     select p).ToList().Select(p => new GenericListItem { ID = p.Followup_Notes_ID.ToString(), Name = p.Followup_Notes_Name });
              if (q != null)
              {
                  Pinsonault.Web.Support.RegisterGenericListVariable(this.Page, q.ToArray(), "followupNotes");
              }
        }
        base.OnPreRender(e);
    }
    protected override void OnLoad(EventArgs e)
    {
        Pinsonault.Web.Support.RegisterTierScriptVariable(this.Page);
        Pinsonault.Web.Support.RegisterComponentWithClientManager(this.Page, Followup_Notes_ID.ClientID, null, "moduleOptionsContainer");
        //Followup_Notes_ID.OnClientLoad = "function(s,a){$createCheckboxDropdown(s.get_id(), 'FollowupNotesIDOptionList', followupNotes, {'maxItems':" + MaxNotes.ToString() + ",'defaultText':'" + Resources.Resource.Label_No_Selection + "'}, {'error':function(){$alert('Maximum " + MaxNotes.ToString() + " follow up notes can be selected.');}}, 'moduleOptionsContainer');}";
        Followup_Notes_ID.OnClientLoad = "function(s,a){$createCheckboxDropdown(s.get_id(), 'FollowupNotesIDOptionList', followupNotes , {'defaultText': '--All Notes--', 'multiItemText': '" + Resources.Resource.Label_Multiple_Selection + "' }, null, 'moduleOptionsContainer'); var Followup_Notes_ID = $get('FollowupNotesIDOptionList').control; $loadPinsoListItems(Followup_Notes_ID, followupNotes, null, -1);}";

        base.OnLoad(e);
    }
}
