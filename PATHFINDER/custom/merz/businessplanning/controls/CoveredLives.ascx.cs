using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class controls_CoveredLives : System.Web.UI.UserControl
{
    public string ContainerID { get; set; }

    public string CoveredLivesEntitySet { get; set; }

    public bool ShowTotalCoveredLives { get; set; }
    public bool ShowPharmLives { get; set; }

    public controls_CoveredLives()
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
