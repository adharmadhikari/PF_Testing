using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderClientModel;
using Pinsonault.Web;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;
using Telerik.Web.UI;
using Pinsonault.Application.MarketplaceAnalytics;

public partial class marketplaceanalytics_controls_FilterTimeFrameComparisonSelector : System.Web.UI.UserControl
{
    protected override void OnInit(EventArgs e)
    {
        //dsRollingQuarters.ConnectionString = Pinsonault.Web.Session.GetClientApplicationConnectionString("MarketplaceAnalytics");
        
        base.OnInit(e);
    }

    protected override void OnLoad(EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Year.ClientID, null, "timeframeArea");
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Month_Quarter.ClientID, null, "timeframeArea");
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Month_Quarter_Selection.ClientID, null, "timeframeArea");
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Rolling_Quarter_Selection.ClientID, null, "timeframeArea");

        //Load page
        if (!Page.IsPostBack)
        {
            using ( PathfinderMarketplaceAnalyticsEntities context = new PathfinderMarketplaceAnalyticsEntities(Pinsonault.Web.Session.GetClientApplicationConnectionString("MarketplaceAnalytics")) )
            {
                //Get available years, quarters and months to create JSON variable to dynamically populate TimeFrame container options
                var yearQuery =
                    (from d in context.LkpMarketplaceYearsSet
                     orderby d.Year
                     select d.Year);

                //Get available rolling quarters
                string sbRollingQuarter = "";

                var rollingQuarters =
                    (from d in context.MSRollingQuarterlyBaseSet
                     select d.Data_Quarter).Distinct();

                var rollingQtrNames = context.LkpMarketplaceShortLongRollingQuarterNamesSet.Where(Pinsonault.Data.Generic.GetFilterForList<LkpMarketplaceShortLongRollingQuarterNames, int>(rollingQuarters.ToList(), "Number"));

                var rollingQuarterQuery =
                    (from d in rollingQtrNames
                     orderby d.Number
                     select d).ToList().Select(d => new GenericListItem { ID = d.Number.ToString(), Name = d.ShortName });

                Type[] types = { typeof(GenericListItem[]), typeof(GenericListItem) };
                DataContractJsonSerializer serializer = serializer = new DataContractJsonSerializer(typeof(GenericListItem), "root", types);

                using (MemoryStream ms = new MemoryStream())
                {
                    serializer.WriteObject(ms, rollingQuarterQuery.ToArray());
                    string script = UTF8Encoding.UTF8.GetString(ms.ToArray());
                    sbRollingQuarter = string.Format("var rollingQtrIdName = {0};", script);
                }

                List<int> years = yearQuery.ToList();

                Year.DataSource = years;
                Year.DataBind();

                int yearCount = years.Count();
                StringBuilder sbQuarter = new StringBuilder();
                StringBuilder sbMonth = new StringBuilder();                

                sbQuarter.Append("var yrQtr = {");
                sbMonth.Append("var yrMth = {");

                int x = 1;
                foreach (int y in years)
                {
                    sbMonth.AppendFormat("{0}:", y);

                    var monthQuery = (from d in context.LkpMarketplaceMonthYearsSet
                                      join n in context.LkpMarketplaceShortLongMonthNamesSet on
                                      d.Data_Month equals n.Number                                      
                                      where d.Data_Year == y
                                      orderby n.Number
                                      select n).ToList().Select(p => new GenericListItem { ID = p.Number.ToString(), Name = p.ShortName });

                    using (MemoryStream ms = new MemoryStream())
                    {
                        serializer.WriteObject(ms, monthQuery.ToArray());
                        string script = UTF8Encoding.UTF8.GetString(ms.ToArray());
                        sbMonth.Append(script);
                    }

                    sbQuarter.AppendFormat("{0}:", y);

                    var quarterQuery = (from d in context.LkpMarketplaceQuarterYearsSet
                                        join n in context.LkpMarketplaceShortLongQuarterNamesSet on
                                        d.Data_Quarter equals n.Number                                        
                                        where d.Data_Year == y
                                        orderby n.Number
                                        select n).ToList().Select(p => new GenericListItem { ID = p.Number.ToString(), Name = p.ShortName });

                    using (MemoryStream ms = new MemoryStream())
                    {
                        serializer.WriteObject(ms, quarterQuery.ToArray());
                        string script = UTF8Encoding.UTF8.GetString(ms.ToArray());
                        sbQuarter.Append(script);
                    }

                    if (x != yearCount) //only add a comma if it is not the last item
                    {
                        sbMonth.Append(",");
                        sbQuarter.Append(",");
                    }

                    x++;
                }

                sbMonth.Append("};");
                sbQuarter.Append("};");

                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "yrMth", sbMonth.ToString(), true);
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "yrQtr", sbQuarter.ToString(), true);
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "rollingQtrIdName", sbRollingQuarter, true);

                //Doing this in FilterTimeFrameComparisonScript instead because below not working
                //Year.OnClientLoad = "function(s,a){ var l=$get('" + Month_Quarter_Selection.ClientID + "');var year=$find('"+ Year.ClientID +"').get_value();var moYr=$find('"+ Month_Quarter.ClientID +"');if(moYr){var moYrVal=moYr.get_value();switch(parseInt(moYrVal,10)){case 1:$loadListItems(l,yrQtr[year]);break;case 2:$loadListItems(l,yrMth[year]);break}}}";

                Year.OnClientSelectedIndexChanged = "function(s,a){ var l=$get('" + Month_Quarter_Selection.ClientID + "');if(!l||!l.control)return;var year=a.get_item().get_value();var moYr=$find('" + Month_Quarter.ClientID + "');var moYrVal=moYr.get_value();switch(parseInt(moYrVal,10)){case 1:$loadListItems(l,yrQtr[year]);break;case 2:$loadListItems(l,yrMth[year]);break}}";

                Month_Quarter.OnClientSelectedIndexChanged = "function(s,a){ var l=$get('" + Month_Quarter_Selection.ClientID + "');if(!l||!l.control)return;var year=$find('" + Year.ClientID + "').get_value();var moYrVal=a.get_item().get_value();switch(parseInt(moYrVal,10)){case 1:$loadListItems(l,yrQtr[year]);break;case 2:$loadListItems(l,yrMth[year]);break}}";
            }
        }
    }
}
