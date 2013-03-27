using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class standardreports_controls_FilterFormularyType : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["Module"] == "tiercoveragecomparison")
        {
            Section_ID.DataBind();
            //remove state medicaid option from drop down if module is tier coverage
            if (Section_ID.FindItemByValue("9") != null)
            {
                Section_ID.FindItemByValue("9").Remove();
            }
        }
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Section_ID.ClientID, null, "moduleOptionsContainer");
        
    }
}
