using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderClientModel;
using PathfinderModel;
using Pinsonault.Web;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;
using Telerik.Web.UI;
using Pinsonault.Application.MarketplaceAnalytics;

public partial class prescriberreporting_controls_FilterGeography : UserControl//, IFilterControl 
{
    string lblReg, lblDist, lblTerr;

    public prescriberreporting_controls_FilterGeography()
    {
        ContainerID = "moduleOptionsContainer";
    }

    public string ContainerID { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        using (PathfinderMarketplaceAnalyticsEntities context = new PathfinderMarketplaceAnalyticsEntities(Pinsonault.Web.Session.GetClientApplicationConnectionString("MarketplaceAnalytics")))
        {
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
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "TerrLevelScript", jScript, false);

            }

        }

    }

    protected override void OnInit(EventArgs e)
    {
        //dsAcctMgr.ConnectionString = Pinsonault.Web.Session.ClientConnectionString;

        base.OnInit(e);
    }

    protected override void OnLoad(EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Region_ID.ClientID, null, ContainerID);
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, District_ID.ClientID, null, ContainerID);
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Territory_ID.ClientID, null, ContainerID);

        if (!Page.IsPostBack)
        {
            //Get all available Regions
            using (PathfinderMarketplaceAnalyticsEntities context = new PathfinderMarketplaceAnalyticsEntities(Pinsonault.Web.Session.GetClientApplicationConnectionString("MarketplaceAnalytics")))
            {               
                var regionQ = context.PrescribersRegionDistrictTerrSet.Select(o => new { o.Region_ID, o.Region_Name})
                        .Distinct().OrderBy(o => o.Region_Name).ToList()
                        .Select(d => new GenericListItem { ID = d.Region_ID, Name = d.Region_Name }).OrderBy(o => o.Name);


                if (regionQ != null)
                {
                    Pinsonault.Web.Support.RegisterGenericListVariable(this.Page, regionQ.Distinct().ToArray(), "allRegions");
                }

                //District
                var districtQ = context.PrescribersRegionDistrictTerrSet.Select(o => new { o.District_ID, o.District_Name })
                        .Distinct().OrderBy(o => o.District_Name).ToList()
                        .Select(d => new GenericListItem { ID = d.District_ID, Name = d.District_Name }).OrderBy(o => o.Name);

                if (districtQ != null)
                {
                    Pinsonault.Web.Support.RegisterGenericListVariable(this.Page, districtQ.ToArray(), "allDistricts");
                }

                //Territory
                var terrQ = context.PrescribersRegionDistrictTerrSet.Select(o => new { o.Territory_ID, o.Territory_Name })
                        .Distinct().OrderBy(o => o.Territory_Name).ToList()
                        .Select(d => new GenericListItem { ID = d.Territory_ID, Name = d.Territory_Name }).OrderBy(o => o.Name);

                if (terrQ != null)
                {
                    Pinsonault.Web.Support.RegisterGenericListVariable(this.Page, terrQ.ToArray(), "allTerritories");
                }


                //Region-District or Region-Territory
                var regionQry = context.PrescribersRegionDistrictTerrSet.Select(o => new { o.Region_ID, o.Region_Name})
                        .Distinct().ToList().OrderBy(o => o.Region_Name)
                        .Select(d => new GenericListItem { ID = d.Region_ID, Name = d.Region_Name }).OrderBy(o => o.Name);

                int regionCount = regionQry.Count();
                StringBuilder sbDistrict = new StringBuilder();

                Type[] types = { typeof(GenericListItem[]), typeof(GenericListItem) };
                DataContractJsonSerializer serializer = serializer = new DataContractJsonSerializer(typeof(GenericListItem), "root", types);

                //Create JSON object: District related to Region
                sbDistrict.Append("var districts = {");

                int a = 1;  //District count
                foreach (GenericListItem li in regionQry)
                {
                    sbDistrict.AppendFormat("\"{0}\":", li.ID);

                    var districtQuery = context.PrescribersRegionDistrictTerrSet
                                         .Where(o => o.Region_ID == li.ID)
                                         .Select(o => new { o.District_ID, o.District_Name})
                                         .OrderBy(o => o.District_Name).Distinct().ToList()
                                         .Select(d => new GenericListItem { ID = d.District_ID, Name = d.District_Name }).OrderBy(o => o.Name);

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

                //Ccreate JSON object: Territory related to Region
                StringBuilder sbTerr2 = new StringBuilder();

                sbTerr2.Append("var territories2 = {");

                int b = 1;  //District count
                foreach (GenericListItem li in regionQry)
                {
                    sbTerr2.AppendFormat("\"{0}\":", li.ID);

                    var terr2Qry = context.PrescribersRegionDistrictTerrSet
                                   .Where(o => o.Region_ID == li.ID)
                                   .Select(o => new { o.Territory_ID, o.Territory_Name })
                                   .OrderBy(o => o.Territory_Name).Distinct().ToList()
                                   .Select(d => new GenericListItem { ID = d.Territory_ID, Name = d.Territory_Name }).OrderBy(o => o.Name);

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


                //District-Territory
                var districtQry = context.PrescribersRegionDistrictTerrSet
                                  .Select(o => new { o.District_ID, o.District_Name })
                                  .OrderBy(o => o.District_Name).Distinct().ToList()
                                  .Select(d => new GenericListItem { ID = d.District_ID, Name = d.District_Name }).OrderBy(o => o.Name);

                int districtCount = districtQry.Count();
                StringBuilder sbTerr = new StringBuilder();


                //Create JSON object: Territory related to District
                sbTerr.Append("var territories = {");
                int x = 1;  // district count
                foreach (GenericListItem li in districtQry)
                {
                    sbTerr.AppendFormat("\"{0}\":", li.ID);

                    var terrQry = context.PrescribersRegionDistrictTerrSet
                                  .Where(o => o.District_ID == li.ID)
                                  .Select(o => new { o.Territory_ID, o.Territory_Name })
                                  .OrderBy(o => o.Territory_Name).Distinct().ToList()
                                  .Select(d => new GenericListItem { ID = d.Territory_ID, Name = d.Territory_Name }).OrderBy(o => o.Name);

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

                sbTerr.Append("};");

                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "districts", sbDistrict.ToString(), true);
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "territories", sbTerr.ToString(), true);
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "territories2", sbTerr2.ToString(), true);
                
                Region_ID.OnClientLoad = "function (s, a) {var d = s.get_element(); $loadListItems(d, allRegions, {value:'all',text:lblValR});}";
                District_ID.OnClientLoad = "function (s, a) {var d = s.get_element(); $loadListItems(d, allDistricts, {value:'all',text:lblValD});}";
                Territory_ID.OnClientLoad = "function(s,a){var t = s.get_element(); $loadListItems(t, allTerritories, {value:'all',text:lblValT});}";
                
                Region_ID.OnClientSelectedIndexChanged = "function(s,a){var l=$get('" + District_ID.ClientID + "'); if(!l || !l.control) return; var t=$get('" + Territory_ID.ClientID + "'); if(!t || !t.control) return; var rcbR = a.get_item().get_value(); if (rcbR == 'all') {$loadListItems(l, allDistricts, {value:'all',text:lblValD}); $loadListItems(t, allTerritories, {value:'all',text:lblValT});} else {$loadListItems(l, districts[rcbR], {value:'all',text:lblValD});  $loadListItems(t, territories2[rcbR], {value:'all',text:lblValT});}; }";
                District_ID.OnClientSelectedIndexChanged = "function(s,a){var l=$get('" + Territory_ID.ClientID + "'); if(!l || !l.control) return; var rcbD = a.get_item().get_value();var rcbR = $find('" + Region_ID.ClientID + "').get_value();if (rcbD == 'all'){ if (rcbR == 'all') {$loadListItems(l, allTerritories, {value:'all',text:lblValT});} else {$loadListItems(l, territories2[rcbR], {value:'all',text:lblValT});}} else { $loadListItems(l, territories[rcbD], {value:'all',text:lblValT});};}";
                Territory_ID.OnClientSelectedIndexChanged = "function(){}";
            }
        }

        base.OnLoad(e);
    }

    #region IFilterControl Members

    string _defaultValue; 
    public string DefaultValue 
    { 
        get
        {
            if ( _defaultValue == null ) 
                return "US";

            return _defaultValue;
        }
        set
        {
            _defaultValue = value;
        }    
    }
    
    #endregion
}
