using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Telerik.Web.UI;

public partial class custom_merz_businessplanning_controls_CoveredLives_MC : System.Web.UI.UserControl
{
    public string ContainerID { get; set; }

    public string CoveredLivesEntitySet { get; set; }

    public bool ShowTotalCoveredLives { get; set; }
    public bool ShowPharmLives { get; set; }

    public custom_merz_businessplanning_controls_CoveredLives_MC()
    {
        CoveredLivesEntitySet = "V_CoveredLivesSet";
        ShowTotalCoveredLives = true;
        ShowPharmLives = true;
    }
    
    protected override void OnLoad(EventArgs e)
    {

        dsCoveredLives.EntitySetName = CoveredLivesEntitySet;

        string prodID = Request.QueryString["Prod_ID"];
        
        if ( string.IsNullOrEmpty(prodID) )
        {
            gridCoveredLives.Visible = true;
            gridProductCoveredLives.Visible = false;

            Pinsonault.Web.Support.RegisterComponentWithClientManager(this.Page, gridCoveredLives.ClientID, "{}", ContainerID);

        }
        else
        {
            gridCoveredLives.Visible = false;
            gridProductCoveredLives.Visible = true;

            Pinsonault.Web.Support.RegisterComponentWithClientManager(this.Page, gridProductCoveredLives.ClientID, "{}", ContainerID);
        }

        base.OnLoad(e);
    }
}
