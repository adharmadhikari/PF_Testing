using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Data;
using PathfinderModel;
using PathfinderClientModel;
using System.Collections;
using Pinsonault.Application.MarketplaceAnalytics;
using Pinsonault.Data;
using System.Text;
using Telerik.Web.UI;
using System.Configuration;
using Pinsonault.Web;
using System.Runtime.Serialization.Json;
using System.IO;


public partial class prescriberreporting_all_MktPopup : PageBase
{
    bool TerrFlag = false;
    string lblReg, lblDist, lblTerr;

    protected void Page_Load(object sender, EventArgs e)
    {

        Pinsonault.Web.Support.RegisterComponentWithClientManager(this, Rollup_Type.ClientID, null, "infoPopup");
        Pinsonault.Web.Support.RegisterComponentWithClientManager(this, rcbProduct.ClientID, null, "infoPopup");
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Section_ID.ClientID, null, "infoPopup");

        //Bind Channel Selection
        using (PathfinderEntities context = new PathfinderEntities())
        {
            int clientID = Pinsonault.Web.Session.ClientID;

            var channels = from c in context.ClientApplicationAccessSet
                           join s in context.SectionSet on
                           c.SectionID equals s.ID
                           where c.ClientID == clientID
                           where c.ApplicationID == 2
                           where c.SectionID != 0
                           select new
                           {
                               s.ID,
                               s.Name
                           };

            Section_ID.DataSource = channels.OrderBy(s => s.Name);
            Section_ID.DataBind();

            //Preselect Commercial
            Section_ID.Items[0].Selected = true;

        }
    }

    protected override void OnLoad(EventArgs e)
    {
 
        NameValueCollection queryValues = new NameValueCollection(Request.QueryString);

        string val = queryValues["Physician_ID"];
        if (!string.IsNullOrEmpty(val))
        {
            // to get Plan_Name
            string prescriberName;
            using (PathfinderMarketplaceAnalyticsEntities context = new PathfinderMarketplaceAnalyticsEntities(Pinsonault.Web.Session.GetClientApplicationConnectionString("MarketplaceAnalytics")))
            {
                //step 1: delete all the records for this business plan key contacts
                //PlanInfo plan = new PlanInfo();
                prescriberName = (from p in context.LkpPhysiciansSet
                                  where p.Physician_ID == val
                                    select p.Full_Name).FirstOrDefault();
            }
            lblPrescriberName.Text = prescriberName;



            // To get Product_Name
            string drugIDs = queryValues["Product_ID"];

            using (PathfinderClientModel.PathfinderClientEntities clientContext = new PathfinderClientModel.PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
            {
                string[] drugIDarr = drugIDs.Split(',');
                int[] drugs = drugIDarr.Select(s => Convert.ToInt32(s)).ToArray();
                Get_rcbProduct_List(clientContext, drugs, queryValues["Default_Product_ID"]);
            }
        }
        else
        {
            throw new HttpException(500, "Invalid request");
        }

        //optionsMenu.Module = "prescribers";

        //optionsMenu.ExportHandler = "window.top.customExport";
        //optionsMenu.ExportHandler = "customExport";

        base.OnLoad(e);
    }

  
    private void Get_rcbProduct_List(PathfinderClientModel.PathfinderClientEntities clientContext, int[] drugIDs, string defaultProduct)
    {
        var q = clientContext.DrugProductsSet.Where(Generic.GetFilterForList<DrugProducts, int>(drugIDs, "Product_ID"));

        RadComboBoxItem item;
        foreach (DrugProducts product in q)
        {
            //fill rcbProduct combo
            item = new RadComboBoxItem();
            item.Text = product.Product_Name;
            item.Value = product.Product_ID.ToString();
            rcbProduct.Items.Add(item);
            if (item.Value == defaultProduct)
                item.Selected = true;
        }
    }

   
}
