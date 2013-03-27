using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderModel;
using Pinsonault.Web;
using Telerik.Web.UI;

public partial class controls_planinfo : System.Web.UI.UserControl
{
    protected override void OnLoad(EventArgs e)
    {
        gridPlanInfo.Style[HtmlTextWriterStyle.Visibility] = "hidden";
        base.OnLoad(e);
    }

    protected void rdlSections_PreRender(object sender, EventArgs e)
    {
        RadComboBox combo = sender as RadComboBox;
        using (PathfinderEntities context = new PathfinderEntities())
        {
            int clientID = Pinsonault.Web.Session.ClientID;
            int applicationID = Identifiers.TodaysAccounts;

            var channels = (from c in context.ClientApplicationAccessSet
                            join s in context.SectionSet on c.SectionID equals s.ID
                            where c.ClientID == clientID
                            where c.ApplicationID == applicationID
                            where s.ID < 100 && s.ID != 0  
                            orderby s.Name
                            select s).ToList().Select(s => new GenericListItem { ID = s.ID.ToString(), Name = s.Name.ToString() });
            if (channels != null)
            {
                combo.DataSource = channels;
                combo.DataBind();

            }
        }


    }
}
