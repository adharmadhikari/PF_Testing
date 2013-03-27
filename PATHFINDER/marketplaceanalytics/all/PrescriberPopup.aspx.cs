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


public partial class marketplaceanalytics_all_PrescriberPopup : PageBase
{
    bool TerrFlag = false;
    string lblReg, lblDist, lblTerr;

    protected void Page_Load(object sender, EventArgs e)
    {

        Pinsonault.Web.Support.RegisterComponentWithClientManager(this, TopN.ClientID, null, "infoPopup");
        Pinsonault.Web.Support.RegisterComponentWithClientManager(this, rcbProduct.ClientID, null, "infoPopup");
        Pinsonault.Web.Support.RegisterComponentWithClientManager(this, Region_ID.ClientID, null, "infoPopup");
        Pinsonault.Web.Support.RegisterComponentWithClientManager(this, District_ID.ClientID, null, "infoPopup");
        Pinsonault.Web.Support.RegisterComponentWithClientManager(this, Territory_ID.ClientID, null, "infoPopup");


        using (PathfinderMarketplaceAnalyticsEntities context = new PathfinderMarketplaceAnalyticsEntities(Pinsonault.Web.Session.GetClientApplicationConnectionString("MarketplaceAnalytics")))
        {
            TopN.DataSource = context.PrescribersTopNSet;
            TopN.DataBind();


            // based on client, Territory_Levels are different (only 3 used out of Territory/District/Region/Area
            // table: tr.Lkp_MS_Territory_Levels

            var tQ = from q in context.LkpMSTerritoryLevelsSet
                     orderby q.MS_Territory_Level_ID
                     select new { tID = q.MS_Territory_Level_ID, tName = q.MS_Territory_Level_Name };

            if (tQ != null)
            {
                foreach (var i in tQ)
                {
                    try
                    {
                        if (i.tID == 1)
                        {
                            lblRegion.Text = i.tName;
                            lblReg = i.tName;
                        }
                        if (i.tID == 2)
                        {
                            lblDistrict.Text = i.tName;
                            lblDist = i.tName;

                        }
                        if (i.tID == 3)
                        {
                            lblTerritory.Text = i.tName;
                            lblTerr = i.tName;
                        }

                    }
                    catch (Exception ex)
                    {
                        throw new HttpException(500, ex.Message);
                    }
                }

                //   'Select Region/District/Territory' dropdown default label should come from database table so register the value 
                string jScript = "<script language=JavaScript>\n" +
                    " var lblValR ='Select " + lblReg + "'; var lblValD ='Select " + lblDist + "'; var lblValT ='Select " + lblTerr + "' ;\n" +
                   "</script>\n";

                Type csType = this.GetType();
                if (!Page.ClientScript.IsClientScriptBlockRegistered(csType, "TerrLevelScript"))
                    RegisterClientScriptBlock("TerrLevelScript", jScript);

            }

        }

    }

    protected override void OnLoad(EventArgs e)
    {
 
        NameValueCollection queryValues = new NameValueCollection(Request.QueryString);

        string val = queryValues["Plan_ID"];
        if (!string.IsNullOrEmpty(val))
        {
            // to get Plan_Name
            string planName;
            using (PathfinderEntities context = new PathfinderEntities())
            {
                int PlanID = Convert.ToInt32(val);
                //step 1: delete all the records for this business plan key contacts
                //PlanInfo plan = new PlanInfo();
                planName = (from p in context.PlanMasterSet
                            where p.Plan_ID == PlanID
                            select p.Plan_Name).FirstOrDefault();
            }
            popupPlanName.Text = planName;



            // to get Product_Name
            string drugIDs = queryValues["Product_ID"];

            using (PathfinderClientModel.PathfinderClientEntities clientContext = new PathfinderClientModel.PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
            {
                string[] drugIDarr = drugIDs.Split(',');
                int[] drugs = drugIDarr.Select(s => Convert.ToInt32(s)).ToArray();
                Get_rcbProduct_List(clientContext, drugs, queryValues["Default_Product_ID"]);
            }

            
                            /////////////////// testing

                            // Region, District, Territory

                            //Get all available Regions
            using (PathfinderMarketplaceAnalyticsEntities context = new PathfinderMarketplaceAnalyticsEntities(Pinsonault.Web.Session.GetClientApplicationConnectionString("MarketplaceAnalytics")))
            {

                //using (PathfinderClientModel.PathfinderClientEntities context = new PathfinderClientModel.PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
                //{

                //Region
                //var regionQ = (from r in context.PrescribersRegionDistrictTerrSet
                //               orderby r.Region_Name
                //               select r).Distinct().ToList().Select(d => new GenericListItem { ID = d.Region_ID.ToString(), Name = d.Region_Name });

                var regionQ = (from r in context.PrescribersRegionDistrictTerrSet
                               select r.Region_Name).Distinct().OrderBy(o => o).ToList().Select(d => new GenericListItem { ID = d, Name = d });


                if (regionQ != null)
                {
                    Pinsonault.Web.Support.RegisterGenericListVariable(this.Page, regionQ.Distinct().ToArray(), "allRegions");
                }

                //District
                var districtQ = (from d in context.PrescribersRegionDistrictTerrSet
                                select d.District_Name).Distinct().OrderBy(o => o).ToList().Select(d => new GenericListItem { ID = d, Name = d });

                if (districtQ != null)
                {
                    Pinsonault.Web.Support.RegisterGenericListVariable(this.Page, districtQ.ToArray(), "allDistricts");
                }

                //Territory
                var terrQ = (from t in context.PrescribersRegionDistrictTerrSet
                            select t.Territory_Name).Distinct().OrderBy(o => o).ToList().Select(d => new GenericListItem { ID = d, Name = d });

                if (terrQ != null)
                {
                    Pinsonault.Web.Support.RegisterGenericListVariable(this.Page, terrQ.ToArray(), "allTerritories");
                }


                ////////////////////   Region-District   OR    Region-Territory /////////////////////
                var regionQry = (from r in context.PrescribersRegionDistrictTerrSet
                                 orderby r.Region_Name
                                 select r.Region_Name).Distinct().ToList().Select(d => new GenericListItem { ID = d, Name = d });

                int regionCount = regionQry.Count();
                StringBuilder sbDistrict = new StringBuilder();

                Type[] types = { typeof(GenericListItem[]), typeof(GenericListItem) };
                DataContractJsonSerializer serializer = serializer = new DataContractJsonSerializer(typeof(GenericListItem), "root", types);

                //////// create JSON object: District related to Region
                sbDistrict.Append("var districts = {");
                
                int a = 1;  // district count
                foreach (GenericListItem li in regionQry)
                {
                    sbDistrict.AppendFormat("\"{0}\":", li.ID);

                    var districtQuery = (from t in context.PrescribersRegionDistrictTerrSet
                                         where t.Region_Name == li.ID
                                         select t.District_Name).OrderBy(o => o).Distinct().ToList().Select(d => new GenericListItem { ID = d, Name = d });

                    using (MemoryStream ms = new MemoryStream())
                    {
                        serializer.WriteObject(ms, districtQuery.ToArray());
                        string script = UTF8Encoding.UTF8.GetString(ms.ToArray());
                        sbDistrict.Append(script);
                    }

                    if (a != regionCount) //only add a comma if it is not the last item
                        sbDistrict.Append(",");

                    a++;
                }
                sbDistrict.Append("};");

                //////// create JSON object: Territory related to Region
                StringBuilder sbTerr2 = new StringBuilder();

                sbTerr2.Append("var territories2 = {");

                int b = 1;  // district count
                foreach (GenericListItem li in regionQry)
                {
                    sbTerr2.AppendFormat("\"{0}\":", li.ID);

                    var terr2Qry = (from t in context.PrescribersRegionDistrictTerrSet
                                    where t.Region_Name == li.ID
                                    select t.Territory_Name).OrderBy(o => o).Distinct().ToList().Select(d => new GenericListItem { ID = d, Name = d });

                    using (MemoryStream ms = new MemoryStream())
                    {
                        serializer.WriteObject(ms, terr2Qry.ToArray());
                        string script = UTF8Encoding.UTF8.GetString(ms.ToArray());
                        sbTerr2.Append(script);
                    }

                    if (b != regionCount) //only add a comma if it is not the last item
                        sbTerr2.Append(",");

                    b++;
                }
                sbTerr2.Append("};");


                ////////////////////   District-Territory
                var districtQry = (from d in context.PrescribersRegionDistrictTerrSet
                                  select d.District_Name).OrderBy(o => o).Distinct().ToList().Select(d => new GenericListItem { ID = d, Name = d });

                int districtCount = districtQry.Count();
                StringBuilder sbTerr = new StringBuilder();


                /////// create JSON object: Territory related to District
                sbTerr.Append("var territories = {");
                int x = 1;  // district count
                foreach (GenericListItem li in districtQry)
                {
                    sbTerr.AppendFormat("\"{0}\":", li.ID);

                    var terrQry = (from t in context.PrescribersRegionDistrictTerrSet
                                   where t.District_Name == li.ID
                                   select t.Territory_Name).OrderBy(o => o).Distinct().ToList().Select(d => new GenericListItem { ID = d, Name = d });
                    
                    using (MemoryStream ms = new MemoryStream())
                    {
                        serializer.WriteObject(ms, terrQry.ToArray());
                        string script = UTF8Encoding.UTF8.GetString(ms.ToArray());
                        sbTerr.Append(script);
                    }

                    if (x != districtCount) //only add a comma if it is not the last item
                        sbTerr.Append(",");

                    x++;
                }

                //}

                sbTerr.Append("};");

               
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "districts", sbDistrict.ToString(), true);
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "territories", sbTerr.ToString(), true);
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "territories2", sbTerr2.ToString(), true);
 

                Region_ID.OnClientLoad = "function (s, a) {var d = s.get_element(); $loadListItems(d, allRegions, {value:'all',text:lblValR});}";
                District_ID.OnClientLoad = "function (s, a) {var d = s.get_element(); $loadListItems(d, allDistricts, {value:'all',text:lblValD});}";
                Territory_ID.OnClientLoad = "function(s,a){var t = s.get_element(); $loadListItems(t, allTerritories, {value:'all',text:lblValT});}";

      
                Region_ID.OnClientSelectedIndexChanged = "function(s,a){var l=$get('" + District_ID.ClientID + "'); if(!l || !l.control) return; var t=$get('" + Territory_ID.ClientID + "'); if(!t || !t.control) return; var rcbR = a.get_item().get_value(); if (rcbR == 'all') {$loadListItems(l, allDistricts, {value:'all',text:lblValD}); $loadListItems(t, allTerritories, {value:'all',text:lblValT});} else {$loadListItems(l, districts[rcbR], {value:'all',text:lblValD});  $loadListItems(t, territories2[rcbR], {value:'all',text:lblValT});}; }";
                District_ID.OnClientSelectedIndexChanged = "function(s,a){var l=$get('" + Territory_ID.ClientID + "'); if(!l || !l.control) return; var rcbD = a.get_item().get_value();var rcbR = $find('" + Region_ID.ClientID + "').get_value();if (rcbD == 'all'){ if (rcbR == 'all') {$loadListItems(l, allTerritories, {value:'all',text:lblValT});} else {$loadListItems(l, territories2[rcbR], {value:'all',text:lblValT});}} else { $loadListItems(l, territories[rcbD], {value:'all',text:lblValT});};}";
                Territory_ID.OnClientSelectedIndexChanged = "function(){TopN_Product_Changed();}";

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
