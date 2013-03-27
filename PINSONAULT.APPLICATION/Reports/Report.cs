using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;
using PathfinderModel;
using System.Collections.Specialized;
using System.Drawing;
using System.Reflection;
using System.Text;

namespace Pinsonault.Data.Reports
{

    /// <summary>
    /// Summary description for PathfinderReport
    /// </summary>
    public abstract class Report
    {
        public Report()
        {            
        }

        public Report(string Title) : this()
        {
            this.Title = Title;
        }

        public virtual string Title { get; set; }

        public IList<NameValueCollection> FilterSets { get; private set; }
        public IList<NameValueCollection> Images { get; private set; }
        public NameValueCollection OriginalQueryString { get; private set; }
        public string ReportKey { get; set; }

        public PathfinderEntities BaseObjectContext { get; private set; }
        public ObjectContext QueryContext { get; private set; }

        public virtual void Initialize(PathfinderEntities baseContext, NameValueCollection queryString, bool custom)
        {
            OriginalQueryString = new NameValueCollection(queryString);
            FilterSets = ExtractFilterSetsFromRequest(queryString);
            Images = ExtractImagesFromRequest(queryString);


            BaseObjectContext = baseContext;
            QueryContext = CreateObjectContext(baseContext, custom);

            ReportDefinitions = new List<ReportDefinition>();
            
            BuildReportDefinitions();
        }

        protected abstract void BuildReportDefinitions();
    
        public IList<ReportDefinition> ReportDefinitions { get; private set; }

        /// <summary>
        /// Checks all filter sets for a specific QueryString parameter.  If there is at least 1 parameter that was specified with a value that is not null or empty then true is returned.
        /// </summary>
        /// <param name="Name">Name of the QueryString parameter to search.</param>
        /// <returns></returns>
        public bool HasValue(string Name)
        {
            return HasValue(FilterSets, Name);
        }

        /// <summary>
        /// Checks all filter sets for a specific name and value combination.
        /// </summary>
        /// <param name="Name">Name of the QueryString parameter to search.</param>
        /// <param name="Value">Value to search for.</param>
        /// <returns></returns>
        public bool HasValue(string Name, string Value)
        {
            return HasValue(FilterSets, Name, Value);
        }

        /// <summary>
        /// Returns all values from all FilterSets for the specified QueryString parameter.
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public IEnumerable<string> FindValues(string Name)
        {
            return FindValues(FilterSets, Name);
        }

        public static bool HasValue(IList<NameValueCollection> filterSets, string Name)
        {
            return FindValues(filterSets, Name).Count(v => !string.IsNullOrEmpty(v)) > 0;
        }

        public static bool HasValue(IList<NameValueCollection> filterSets, string Name, string Value)
        {
            return FindValues(filterSets, Name).Count(v => string.Compare(v, Value, true) ==0) > 0;
        }
        
        public static IEnumerable<string> FindValues(IList<NameValueCollection> filterSets, string Name)
        {
            return filterSets.Select(fs => fs[Name]);
        }

        protected bool IsGeographyNationOrState(PathfinderEntities context, string GeographyID)
        {
            bool national = string.IsNullOrEmpty(GeographyID) || string.Compare(GeographyID, "US", true) == 0;

            if ( national || context.StateSet.Count(s => s.ID == GeographyID) > 0 )
                return true;
            
            return false;
        }

        protected virtual ObjectContext CreateObjectContext(PathfinderEntities context, bool IsCustom)
        {

            string geographyID = FindValues(FilterSets, "Geography_ID").FirstOrDefault();

            if ( !IsCustom )
                return CreateObjectContextByGeography(context, geographyID);
            else
            {
                //determine if using custom model
                Type customContextType = Type.GetType(string.Format("Pinsonault.Application.{0}.Pathfinder{0}Entities, Pinsonault.Application.{0}", Pinsonault.Web.Session.ClientKey), false, true);
                if ( customContextType != null )
                {
                    return customContextType.GetConstructor(Type.EmptyTypes).Invoke(null) as ObjectContext;
                }
                else //fall back to regular client model although it might not work
                    return CreateClientContext();
            }
        }

        protected ObjectContext CreateClientContext()
        {
            return new PathfinderClientModel.PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString);
        }

        protected ObjectContext CreateObjectContextByGeography(PathfinderEntities context, string GeographyID)
        {
            if ( IsGeographyNationOrState(context, GeographyID) )
                return context; //continue using current context
            else
                return new PathfinderClientModel.PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString);
        }

        public static IList<NameValueCollection> ExtractFilterSetsFromRequest(NameValueCollection request)
        {
            List<NameValueCollection> data = new List<NameValueCollection>();

            string formData = null;
            int count = 10; //max data sets - probably not best way but for now should be sufficient
            bool found = false;
            for ( int i = 0; i < count; i++ )
            {
                formData = request[string.Format("_data{0}", i)];

                if ( !string.IsNullOrEmpty(formData) )
                {
                    found = true;

                    ////% Fix for "Contains/Like" queries
                    //formData = formData.Replace("%", "%25");

                    ////Ampersand Fix
                    //int ampCount = formData.Count(f => f == '&');
                    //int equalCount = formData.Count(f => f == '=');

                    ////Equal count should always be 1 greater than Ampersand count, if not, fix 'double ampersand'
                    //if ( (equalCount - 1) != ampCount )
                    //{
                    //    string[] splitQuery = formData.Split('=');
                    //    List<string> fixedQuery = new List<string>();

                    //    foreach (string s in splitQuery)
                    //    {
                    //        string fixedString = s;

                    //        while (fixedString.Count(f => f == '&') > 1)
                    //        {
                    //            int firstAmp = fixedString.IndexOf('&');

                    //            fixedString = fixedString.Remove(firstAmp, 1);
                    //            fixedString = fixedString.Insert(firstAmp, "%26");
                    //        }
                    //        fixedQuery.Add(fixedString);
                    //    }

                    //    formData = string.Join("=", fixedQuery.ToArray());
                    //}
                    ////End Ampersand Fix

                    data.Add(HttpUtility.ParseQueryString(formData));
                }
                else
                {
                    if ( !found ) //add empty set as place holder
                        data.Add(new NameValueCollection());
                    else //break
                        break; //no more sets so break (don't allow empty sets after one set found)
                }
            }

            if ( !found )
            {
                data.Clear();//no actual filters sent
                data.Add(new NameValueCollection()); //but have to return something because it is expected - something to consider as process is improved
            }
            return data;
        }

        public static IList<NameValueCollection> ExtractImagesFromRequest(NameValueCollection request)
        {
            List<NameValueCollection> data = new List<NameValueCollection>();
            NameValueCollection col;

            string formData = null;
            int i = 0;

            while ( i == 0 || formData != null )
            {
                formData = request[string.Format("_img{0}", i)];
                if ( !string.IsNullOrEmpty(formData) )
                {
                    col = HttpUtility.ParseQueryString(formData);
                    if ( !string.IsNullOrEmpty(col["chartid"]) )
                    {
                        //if ( !string.IsNullOrEmpty(col["url"]) )
                        //{
                        //    uri = new Uri(col["url"]);
                        //col["path"] = HttpContext.Current.Server.MapPath(uri.LocalPath);
                        //col["url"] = string.Format("usercontent/chart.ashx?id={0}", col["chartid"]);
                        col["path"] = System.IO.Path.Combine(Pinsonault.Web.Support.GetClientTempFolder("charts"), string.Format("{0}.jpeg", col["chartid"])); //chart id is file name
                        //}
                        if ( string.IsNullOrEmpty(col["Width"]) )
                            col["Width"] = "800";
                        if ( string.IsNullOrEmpty(col["Height"]) )
                            col["Height"] = "600";

                        data.Add(col);
                    }
                }
                i++;
            }

            return data;
        }
        
        static NameValueCollection prepFilterSet(NameValueCollection filterSet, string sort)
        {
            NameValueCollection filters = new NameValueCollection(filterSet);

            // Set the Client_ID filter from session (if it's not already set.)
            const String clientIDKey = "Client_ID";
            if ( String.IsNullOrEmpty(filters[clientIDKey]) )
                filters.Add(clientIDKey, Pinsonault.Web.Session.ClientID.ToString());

            filters["__sort"] = sort;            

            filters.Remove("report"); // Report ID
            filters.Remove("tile"); // Tile ID
            filters.Remove("type"); // Export type
            filters.Remove("undefined"); // Bug in JS code

            return filters;
        }

        public IList<ReportSubsection> CreateReportSections()
        {
            List<ReportSubsection> reportSubsections = new List<ReportSubsection>();

            int index = 0;
            int imageIndex = 0;

            ReportDefinition def;
            IList<ReportDefinition> selectedDefs;
            NameValueCollection image;

            foreach ( NameValueCollection filters in FilterSets )
            {
                def = index < ReportDefinitions.Count ? ReportDefinitions[index] : null;

                if ( def != null && (!def.RequiresFilters || filters.Count > 0) )
                {
                    if ( def.ReportDefinitions.Count == 0 )
                    {
                        selectedDefs = new List<ReportDefinition>();
                        selectedDefs.Add(def);
                    }
                    else
                        selectedDefs = def.ReportDefinitions;

                    foreach ( ReportDefinition selectedDef in selectedDefs )
                    {
                        //processedFilters = selectedDef(filters);
                        selectedDef.ApplyFilters(prepFilterSet(filters, selectedDef.Sort));

                        selectedDef.LoadColumnMap<ReportColumn>(BaseObjectContext);

                        int reportKeyLength = ReportKey.Length;
                        if ( reportKeyLength >= 26 )
                            reportKeyLength = 26;

                        if ( selectedDef.Visible )
                        {
                            image = imageIndex < Images.Count ? Images[imageIndex] : null;

                            reportSubsections.Add(
                                new ReportSubsection
                                {
                                    Name = (string.IsNullOrEmpty(selectedDef.SectionTitle) ? string.Format("{0} - {1}", ReportKey.Substring(0, reportKeyLength), index) : selectedDef.SectionTitle),
                                    //Criteria = filters,
                                    CriteriaItems = LoadCriteriaItems(selectedDef.QueryDefinition.Values),
                                    Data = selectedDef.QueryDefinition.CreateQuery(QueryContext),
                                    ColumnMap = selectedDef.ColumnMapping,
                                    ReportDefinition = selectedDef,
                                    ImageUrl = image != null ? image["url"] : null,
                                    ImagePath = image != null ? image["path"] : null,
                                    ChartID = image != null ? image["chartid"] : null,
                                    Width = image != null ? Convert.ToSingle(image["Width"]) : 0,
                                    Height = image != null ? Convert.ToSingle(image["Height"]) : 0
                                }
                            );


                            imageIndex++;
                        }
                    }
                    index++;
                }
                else
                    index++;
            }

            return reportSubsections;
        }


        static int[] SplitIDs(String idSet)
        {
            IList<int> results = new List<int>();
            String[] ids = idSet.Split(',');
            foreach ( String id in ids )
                results.Add(int.Parse(id.Trim()));

            return results.ToArray();
        }

        protected virtual Dictionary<string, CriteriaItem> LoadCriteriaItems(NameValueCollection filters)
        {            
            return LoadCriteriaItems(BaseObjectContext, filters);
        }

        Dictionary<string, CriteriaItem> LoadCriteriaItems(PathfinderEntities context, NameValueCollection filters)
        {
            Dictionary<string, CriteriaItem> list = new Dictionary<string, CriteriaItem>();

            try
            {
                CriteriaItem[] items = new CriteriaItem[] {
                        new CriteriaItem("PA",              "PA Restriction",   val => val == "PA" ? "Yes" : "No"),
                        new CriteriaItem("QL",              "QL Restriction",   val => val == "QL" ? "Yes" : "No"),
                        new CriteriaItem("ST",              "Step Therapy",    val => val == "ST" ? "Yes" : "No"),
                        new CriteriaItem("Market_Basket_ID", "Market Basket",   val => CrackNames<TherapeuticClassMaster>(SplitIDs(val), context.TherapeuticClassMasterSet, "ID", "Name") ),
                        new CriteriaItem("Drug_ID",         "Drug",             val => CrackNames<Drug>(SplitIDs(val), context.DrugSet, "ID", "Name")),
                        new CriteriaItem("Tier_ID",         "Tier",             val => CrackNames<Tier>(SplitIDs(val), context.TierSet, "ID", "Name")),
                        //new CriteriaItem("Segment_ID",      "Segment",          val => val),
                        new CriteriaItem("Section_ID",      "Section",          val => CrackNames<Section>(SplitIDs(val), context.SectionSet, "ID", "Name")),
                        new CriteriaItem("Geography_ID",    "Geography",        val => val),
                        new CriteriaItem("Is_Predominant",  "Benefit Design",   val => val == "true" ? "Predominant" : "All"),
                        new CriteriaItem("Rank",            "Rank",             val => "Top " + val),
                        new CriteriaItem("Plan_ID",         "Account Name(s)",  val => CrackNames<PlanMaster>(SplitIDs(val), context.PlanMasterSet, "Plan_ID", "Plan_Name")),
                        new CriteriaItem("Coverage_Status_ID", "Coverage Status", val=> CrackNames<CoverageStatus>(SplitIDs(val), context.CoverageStatusSet, "ID","Name")),
                        new CriteriaItem("MarketSegmentId",   "Market Segment", val => CrackNames<CoveredLivesSummary>(SplitIDs(val), context.CoveredLivesSummarySet, "MarketSegmentId", "MarketSegmentName")),
                        new CriteriaItem("Formulary_Status_ID",         "Formulary Status",             val => CrackNames<FormularyStatus>(SplitIDs(val), context.FormularyStatusSet, "ID", "Name"))
                    };

                CriteriaItem item;
                foreach ( String key in filters.Keys )
                {
                    item = items.Where(i => i.Key == key).FirstOrDefault();
                    if ( item != null )
                    {
                        item.Evaluate(filters[key]);
                        list.Add(item.Key.ToLower(), item);
                    }
                }
            }
            catch ( Exception ex )
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Exporter failed to initialize CriteriaItems: {0}", ex.Message));
            }
            
            return list;
        }

        protected string CrackNames<T>(int[] ids, IEnumerable<T> source, string idPropertyName, string namePropertyName)
        {
            IEnumerable<T> results = source.Where<T>(delegate(T item)
            {
                PropertyInfo pi = typeof(T).GetProperty(idPropertyName);
                foreach ( int id in ids )
                {
                    if ( (int)pi.GetValue(item, null) == id )
                        return true;
                }
                return false;
            });

            StringBuilder sb = new StringBuilder();
            foreach ( T result in results )
            {
                sb.Append((string)typeof(T).GetProperty(namePropertyName).GetValue(result, null));
                sb.Append(", ");
            }
            sb.Remove(sb.Length - 2, 2);
            return sb.ToString();
        }
    }
}