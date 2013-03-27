using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Collections.Specialized;

namespace Pinsonault.Application.MarketplaceAnalytics
{
    public partial class PathfinderMarketplaceAnalyticsEntities
    {
        public DateTime GetDataDate()
        {
            var q = (from m in MSMonthSet
                     orderby m.Data_Year descending, m.Data_Month descending
                     select m);

            MSMonth month = q.FirstOrDefault();
            if(month != null)
                return new DateTime(month.Data_Year, month.Data_Month, 1);
            return
                DateTime.MinValue;
        }

        partial void OnContextCreated()
        {
            //can't apply this fix if using with data service and/or possibly EntityDataSource control
            //if ( string.Compare(this.Connection.ConnectionString, Pinsonault.Web.Session.GetClientApplicationConnectionString("MarketplaceAnalytics"), true) != 0 )
            //    throw new ApplicationException("Invalid client connection string.  Please set using a call to Pinsonault.Web.Session.GetClientApplicationConnectionString(\"MarketplaceAnalytics\")");
        }

        /// <summary>
        /// Returns the Plan IDs of the top N plans based on market share.
        /// </summary>
        /// <param name="TopN">How many plans should be returned.</param>
        /// <param name="SectionID">Section ID to filter by.</param>
        /// <param name="GeographyID">ID of region to filter by.</param>
        /// <param name="MarketBasketID">ID of market basket to filter by.</param>
        /// <returns></returns>
        public IQueryable<int> GetTopPlansByMarketshare(int TopN, int SectionID, string GeographyID, int MarketBasketID)
        {
            return (from d in MSRankSet
                    where (d.Section_ID == SectionID)
                            && (d.Geography_ID == GeographyID)
                            && (d.Thera_ID == MarketBasketID)
                     orderby d.Item_Rank
                    select d.Plan_ID).Take(TopN);            
        }

        /// <summary>
        /// Returns the Plan IDs of the top N plans based on market share and territory
        /// </summary>
        /// <param name="TopN">How many plans should be returned.</param>
        /// <param name="SectionID">Section ID to filter by.</param>
        /// <param name="GeographyID">ID of region to filter by.</param>
        /// <param name="MarketBasketID">ID of market basket to filter by.</param>
        /// <param name="TerritoryID">ID of territory to filter by.</param>
        /// <returns></returns>
        public IQueryable<int> GetTopPlansByMarketshareTerritory(int TopN, int SectionID, int MarketBasketID, string TerritoryID)
        {
            return (from d in MSRankByTerritorySet
                    where (d.Section_ID == SectionID)
                            && (d.Thera_ID == MarketBasketID)
                            && (d.Territory_ID == TerritoryID)
                    orderby d.Item_Rank
                    select d.Plan_ID).Take(TopN);
        }

        /// <summary>
        /// Returns the Physician IDs of the top N Prescribers based on market share and geography
        /// </summary>
        /// <param name="TopN">How many plans should be returned.</param>
        /// <param name="SectionID">Section ID to filter by.</param>
        /// <param name="GeographyID">ID of region to filter by.</param>
        /// <param name="MarketBasketID">ID of market basket to filter by.</param>
        /// <param name="TerritoryID">ID of territory to filter by.</param>
        /// <returns></returns>
        public IQueryable<int> GetTopPrescribersByGeography(int TopN, int SectionID, int MarketBasketID, string TerritoryID)
        {
            return (from d in MSRankByTerritorySet
                    where (d.Section_ID == SectionID)
                            && (d.Thera_ID == MarketBasketID)
                            && (d.Territory_ID == TerritoryID)
                    orderby d.Item_Rank
                    select d.Plan_ID).Take(TopN);
        }

        /// <summary>
        /// Returns a list of changes per timeframe/drug
        /// </summary>
        /// <param name="queryVals">Current query string.</param>
        /// <param name="dataField">Timeframe column to select by.</param>
        /// <returns></returns>
        public List<FHRChanges> GetFHRChanges(NameValueCollection queryVals, string dataField)
        {
            using (PathfinderMarketplaceAnalyticsEntities context = new PathfinderMarketplaceAnalyticsEntities(Pinsonault.Web.Session.GetClientApplicationConnectionString("MarketplaceAnalytics")))
            {
                int planID = Convert.ToInt32(queryVals["Plan_ID"]);
                int sectionID = Convert.ToInt32(queryVals["Section_ID"]);
                string query = "SELECT VALUE O FROM ProductFormularyHistoryDataSet AS O WHERE O.Product_ID IN {" + queryVals["Product_ID"] + "} AND O." + dataField + " IN {" + queryVals["Timeframe"] + "}";

                ObjectQuery<ProductFormularyHistoryData> o = new ObjectQuery<ProductFormularyHistoryData>(query, context);

                o = (ObjectQuery<ProductFormularyHistoryData>)o
                    .Where(d => d.Plan_ID == planID);

                //Only query on SectionID if not Med-D
                if (sectionID != 17)
                    o = (ObjectQuery<ProductFormularyHistoryData>)o
                        .Where(d => d.Section_ID == sectionID);

                int segmentID = 1;

                if (Convert.ToInt32(queryVals["Section_ID"]) == 17) //Med-D
                    segmentID = 2;
                else if (Convert.ToInt32(queryVals["Section_ID"]) == 9) //State Medicaid
                    segmentID = 3;

                o = (ObjectQuery<ProductFormularyHistoryData>)o
                        .Where(d => d.Segment_ID == segmentID);

                if (sectionID == 17) //If Med D, select Data_Year_Month
                    return (List<FHRChanges>)o.Select(d => new FHRChanges { Timeframe = d.Data_Year_Month, Changed = d.Changed_Overall, ID = d.Product_ID }).ToList().AsEnumerable();
                else //All Else, select Data_Year_Quarter
                    return (List<FHRChanges>)o.Select(d => new FHRChanges { Timeframe = d.Data_Year_Quarter, Changed = d.Changed_Overall, ID = d.Product_ID }).ToList().AsEnumerable();
            }
        }
    }
}

public class FHRChanges
{
    public FHRChanges() { }
    public int ID { get; set; }
    public int? Changed { get; set; }
    public int? Timeframe { get; set; }
}

