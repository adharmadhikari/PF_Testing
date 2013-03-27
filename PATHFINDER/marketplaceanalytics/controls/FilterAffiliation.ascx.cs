using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pinsonault.Web.UI;

public partial class marketplaceanalytics_controls_FilterAffiliation : System.Web.UI.UserControl 
{
    public marketplaceanalytics_controls_FilterAffiliation()
    {
        ContainerID = "moduleOptionsContainer";
    }

    public string ContainerID { get; set; }

    protected override void OnPreRender(EventArgs e)
    {   
        Section_ID.DataBind();
        
        if (!Page.IsPostBack)
        {
            //make commercial section selected by default
            Section_ID.SelectedIndex = Section_ID.Items.IndexOf(Section_ID.Items.FindItemByValue("1"));
        }

        base.OnPreRender(e);
    }

    protected override void OnLoad(EventArgs e)
    {

        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Section_ID.ClientID, null, ContainerID);
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Plan_ID.ClientID, null, ContainerID);
      
        base.OnLoad(e);
    }   
}
