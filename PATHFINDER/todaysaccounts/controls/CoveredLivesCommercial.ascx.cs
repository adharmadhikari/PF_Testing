using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class controls_CoveredLivesCommercial : System.Web.UI.UserControl
{
    public string ContainerID { get; set; }

    public string CoveredLivesEntitySet { get; set; }

    public bool ShowTotalCoveredLives { get; set; }
    public bool ShowPharmLives { get; set; }
    public bool ShowSectionDisclaimer { get; set; }
    public bool IsDoD { get; set; }
    public bool IsVA { get; set; }

    public controls_CoveredLivesCommercial()
    {
        CoveredLivesEntitySet = "CoveredLivesCommercialSet";
        ShowTotalCoveredLives = true;
        ShowPharmLives = true;
        ShowSectionDisclaimer = true;
        IsDoD = false;
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

        if (IsDoD == true || IsVA == true)
        {
            gridCoveredLives.Visible = false;
        }
        else
        {
            gridCoveredLives.Visible = true;
        }

        base.OnLoad(e);
    }

}
