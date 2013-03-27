using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dundas.Charting.WebControl;
using Pinsonault.Application.TodaysAccounts;
using System.Collections.Specialized;
using System.Data.Common;
using PathfinderModel;
using Pinsonault.Data;


public partial class todaysaccounts_all_OpenPieChart : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        NameValueCollection queryValues = new NameValueCollection(Request.QueryString);        

        using (PathfinderModel.PathfinderEntities context = new PathfinderModel.PathfinderEntities())
        {
            
            string originalSections = queryValues["Section_ID"];

            string[] sections = (from s in context.ClientApplicationAccessSet
                             where s.ApplicationID == 1
                             && s.ClientID == Pinsonault.Web.Session.ClientID
                             && s.SectionID != 99
                             select s.SectionID).ToArray().Select( s => s.ToString()).ToArray();

            queryValues["Section_ID"] = string.Join(",", sections);

            IList<DbDataRecord> q = null;

            //Filter by user territory if selecting 'My Accounts'
            if (!string.IsNullOrEmpty(queryValues["ByTerritory"]))
            {
                queryValues["Territory_ID"] = Pinsonault.Web.Session.TerritoryID;
                q = Generic.CreateGenericEntityQuery<PathfinderModel.PlanInfoListViewByTerritoryModal>(context, typeof(TodaysAccountsQueryDefinition).GetConstructor(new Type[] { typeof(string), typeof(NameValueCollection) }).Invoke(new object[] { typeof(PathfinderModel.PlanInfoListViewByTerritoryModal).Name, queryValues }) as QueryDefinition, true).ToList();
            }
            else
                q = Generic.CreateGenericEntityQuery<PathfinderModel.PlanInfoListViewModal>(context, typeof(TodaysAccountsQueryDefinition).GetConstructor(new Type[] { typeof(string), typeof(NameValueCollection) }).Invoke(new object[] { typeof(PathfinderModel.PlanInfoListViewModal).Name, queryValues }) as QueryDefinition, true).ToList();

            string geography = "US";

            if (!string.IsNullOrEmpty(queryValues["Plan_State"]))
                geography = queryValues["Plan_State"];

            PieChart1.ProcessChart(q, "Lives Distribution", geography, originalSections);
        }
    }


}
