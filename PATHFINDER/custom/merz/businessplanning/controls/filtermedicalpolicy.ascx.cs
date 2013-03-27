using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class custom_merz_businessplanning_controls_filtermedicalpolicy : System.Web.UI.UserControl
{
    public custom_merz_businessplanning_controls_filtermedicalpolicy()
    {
        ContainerID = "moduleOptionsContainer";
        IncludeAll = true;
    }
    public string ContainerID { get; set; }

    public bool IncludeAll { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {        
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, DocumentType.ClientID, null, ContainerID);

        //if (IncludeAll)
        //{
        //    RadComboBoxItem itemDT = new RadComboBoxItem("All");
        //    DocumentType.Items.Add(itemDT);
        //    DocumentType.SelectedItem.Text = "All";
        //}
    }
    
}
