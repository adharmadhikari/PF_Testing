using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pinsonault.Application.Genzyme;
using Pinsonault.Web;

public partial class custom_controls_FilterMeetingActivity : System.Web.UI.UserControl
{
    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Meeting_Activity_ID.ClientID, null, "moduleOptionsContainer");
    //    using (PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
    //    {
    //        Meeting_Activity_ID.DataSource = context.LkpMeetingActivitySet;
    //        Meeting_Activity_ID.DataBind();

    //    }
    //}

    public custom_controls_FilterMeetingActivity()
    {
        ContainerID = "moduleOptionsContainer";
    }
    public string ContainerID { get; set; }
    
    protected override void OnPreRender(EventArgs e)
    {
        //creates the drop down list for Products Discussed based on the Drug_Name
        using (PathfinderGenzymeEntities context = new PathfinderGenzymeEntities())
        {
            var q = (from m in context.LkpMeetingActivitySet
                    orderby m.Meeting_Activity_Name 
                    select m).ToList().Select( m => new GenericListItem { ID = m.Meeting_Activity_ID.ToString(), Name = m.Meeting_Activity_Name });
              if (q != null)
              {
                  //List<GenericListItem> list = q.ToList();
                  //list.Insert(0, new GenericListItem { ID = "0", Name = "All" });
                  Pinsonault.Web.Support.RegisterGenericListVariable(this.Page, q.ToArray(), "meetingActivity");
              }
        }
        base.OnPreRender(e);
    }
    protected override void OnLoad(EventArgs e)
    {
        Pinsonault.Web.Support.RegisterTierScriptVariable(this.Page);
        Pinsonault.Web.Support.RegisterComponentWithClientManager(this.Page, Meeting_Activity_ID.ClientID, null, "moduleOptionsContainer");
        Meeting_Activity_ID.OnClientLoad = "function(s,a){$createCheckboxDropdown(s.get_id(), 'MeetActivityIDOptionList', meetingActivity , {'defaultText': '--All Meeting Activities--', 'multiItemText': '" + Resources.Resource.Label_Multiple_Selection + "' }, null, 'moduleOptionsContainer'); var Meeting_Activity_ID = $get('MeetActivityIDOptionList').control; $loadPinsoListItems(Meeting_Activity_ID, meetingActivity, null, -1);}";
        base.OnLoad(e);
    }
}
