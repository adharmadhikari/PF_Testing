using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class todaysaccounts_all_OpenQL : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int iDrugID = Convert.ToInt32(System.Web.HttpContext.Current.Request.QueryString["DrugID"]);

        Pinsonault.Web.Support.RegisterComponentWithClientManager(this, rgCriteriaDetails.ClientID, null, "infoPopup");
        
        using (PathfinderModel.PathfinderEntities context = new PathfinderModel.PathfinderEntities())
        {
            var d = (from drugName in context.DrugSet
                     where drugName.ID == iDrugID
                     select drugName.Name).FirstOrDefault();           

            if (d.Count() > 0)
            {
                // check the type and parse the notes accordingly
                this.titleText.Text = "Drug Name: " + d;
            }

        }
    }
}
