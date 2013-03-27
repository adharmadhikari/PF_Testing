using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;

public partial class controls_CoveredLivesCombined : System.Web.UI.UserControl
{
    public string ContainerID { get; set; }

    public string CoveredLivesEntitySet { get; set; }

    public bool ShowTotalCoveredLives { get; set; }
    public bool ShowPharmLives { get; set; }
    public bool ShowSectionDisclaimer { get; set; }
    public bool IsDoD { get; set; }
    public bool IsVA { get; set; }

    public controls_CoveredLivesCombined()
    {
        CoveredLivesEntitySet = "V_CoveredLivesSet";
        ShowTotalCoveredLives = true;
        ShowPharmLives = true;
        ShowSectionDisclaimer = true;
        IsDoD = false;
    }

    protected override void OnLoad(EventArgs e)
    {
        dsCoveredLives.EntitySetName = CoveredLivesEntitySet;

        NameValueCollection nvc = new NameValueCollection(Request.QueryString);

        //Filter Lives Distribution with Plan ID
        dsCoveredLives.Where = "it.Plan_ID = @Plan_ID";

        //Filter Lives Distribution based on original section selected
        if (!string.IsNullOrEmpty(nvc["Original_Section"]))
        {
            string[] sections = nvc["Original_Section"].Split(',');

            List<int> coveredLivesType = new List<int>();

            bool hasCommercial = sections.Contains("1");
            bool hasPartD = sections.Contains("17");
            bool hasManagedMedicaid = sections.Contains("6");

            //Restrict Commercial records
            if (!hasCommercial)
            {
                coveredLivesType.Add(4);
                coveredLivesType.Add(5);
                coveredLivesType.Add(6);
                coveredLivesType.Add(7);
                coveredLivesType.Add(9);
                coveredLivesType.Add(16);
            }

            //Restrict Part D records
            if (!hasPartD)
            {
                coveredLivesType.Add(10);
                coveredLivesType.Add(11);
                coveredLivesType.Add(12);
                coveredLivesType.Add(19);
                coveredLivesType.Add(20);
            }

            //Restrict Managed Medicaid records
            if (!hasManagedMedicaid)
            {
                coveredLivesType.Add(8);
            }

            if (coveredLivesType.Count > 0)
            {
                dsCoveredLives.Where += " AND it.Covered_Lives_Type_ID NOT IN {" + string.Join(",", coveredLivesType.Select(x => x.ToString()).ToArray()) + "}";
            }
        }

        string prodID = Request.QueryString["Prod_ID"];
        
        if ( string.IsNullOrEmpty(prodID) )
        {
            gridCoveredLives.Visible = true;

            Pinsonault.Web.Support.RegisterComponentWithClientManager(this.Page, gridCoveredLives.ClientID, "{}", ContainerID);

        }

        base.OnLoad(e);
    }

}
