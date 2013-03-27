using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Data.Common;
using Pathfinder;
using Pinsonault.Data;
using PathfinderModel;

public interface IReportChart
{
    void ProcessChart(IEnumerable<DbDataRecord> Data, string chartTitle, string region);
}

public interface IReportGrid
{
    void ProcessGrid(IList<DbDataRecord> Data, string Title, string region);
}

/// <summary>
/// Summary description for ReportPageLoader
/// </summary>
public class ReportPageLoader
{
    public ReportPageLoader()
    {
    }


    static IList<Tier> _tiers = null;
    public static IList<Tier> Tiers
    {
        get
        {
            if ( _tiers == null )
            {
                using ( PathfinderEntities context = new PathfinderEntities() )
                {
                    _tiers = context.TierSet.OrderBy(t => t.ID).ToList();
                }

            }

            return _tiers;
        }
    }
    static IList<FormularyStatus> _formularystatus = null;
    public static IList<FormularyStatus> FormularyStatus
    {
        get
        {
            if (_formularystatus == null)
            {
                //exclude "NA" option from formulary status
                using (PathfinderEntities context = new PathfinderEntities())
                {
                    _formularystatus = context.FormularyStatusSet.Where(e=> e.Abbr != "NA").OrderBy(t => t.ID).ToList();
                }

            }

            return _formularystatus;
        }
    }

    public static void LoadReport<T, QT>(NameValueCollection QueryString, IReportChart chartControl) where QT : QueryDefinition
    {
        LoadReport<T, QT>(QueryString, chartControl, null, false, false, null);
    }

    public static void LoadReport<T, QT>(NameValueCollection QueryString, IReportGrid gridControl) where QT : QueryDefinition
    {
        LoadReport<T, QT>(QueryString, null, gridControl, false, false, null);
    }

    public static void LoadReport<T, QT>(NameValueCollection QueryString, IReportChart chartControl, IReportGrid gridControl) where QT : QueryDefinition 
    {
        LoadReport<T, QT>(QueryString, chartControl, gridControl, true, false, null);
    }

    public static void LoadReport<T, QT>(NameValueCollection QueryString, IReportChart chartControl, IReportGrid gridControl, bool includeNational) where QT : QueryDefinition
    {
        LoadReport<T, QT>(QueryString, chartControl, gridControl, includeNational, false, null);
    }

    public static void LoadReport<T, QT>(NameValueCollection QueryString, IReportChart chartControl, IReportGrid gridControl, bool includeNational, bool useClientContext, System.Data.Objects.ObjectContext clientContext) where QT : QueryDefinition 
    {
        using ( PathfinderClientModel.PathfinderClientEntities context = new PathfinderClientModel.PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString) )
        {
            NameValueCollection queryValues = new NameValueCollection(QueryString);

            queryValues.Add("Client_ID", Pinsonault.Web.Session.ClientID.ToString());

            string val = queryValues["Geography_ID"];

            bool regionalChart = false;
            if ( string.Compare(val, "US", true) != 0 && queryValues["Section_ID"] != "4" )
            {
                regionalChart = true;
                if ( includeNational )
                    queryValues["Geography_ID"] = string.Format("US, {0}", queryValues["Geography_ID"]);
            }

            string chartTitle; //region or state title

            IList<DbDataRecord> q = null;

            if (useClientContext)
                q = GetReportData<T, QT>(typeof(T).Name, clientContext, val, queryValues, useClientContext, out chartTitle);
            else
                q = GetReportData<T, QT>(typeof(T).Name, context, val, queryValues, useClientContext, out chartTitle);

            IEnumerable<DbDataRecord> listUS = q.Where(r => r.GetString(r.GetOrdinal("Geography_ID")).ToUpper() == "US");
            IEnumerable<DbDataRecord> listRegion = q.Where(r => r.GetString(r.GetOrdinal("Geography_ID")).ToLower() == val.ToLower());

            if ( chartControl != null )
            {
                if ( includeNational || !regionalChart )
                    chartControl.ProcessChart(listUS, Resources.Resource.Label_National, "US");
                
                if ( regionalChart )
                    chartControl.ProcessChart(listRegion, chartTitle, val);
            }

            if ( gridControl != null )
            {
                if ( includeNational || !regionalChart )
                    gridControl.ProcessGrid(listUS.ToList(), Resources.Resource.Label_National, "US");

                if ( regionalChart )
                    gridControl.ProcessGrid(listRegion.ToList(), chartTitle, val);
            }
        }
    }

    public static IList<System.Data.Common.DbDataRecord> GetReportData<T, QT>(string EntityTypeName, System.Data.Objects.ObjectContext context, string region, NameValueCollection queryValues, bool useClientContext, out string chartTitle) where QT : QueryDefinition
    {
        //using ( PathfinderEntities context = new PathfinderEntities() )
        //{
        //no geo id or US or a State then use master db - otherwise use client db for territory mappings
        using (PathfinderClientModel.PathfinderClientEntities c = new PathfinderClientModel.PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
        {
            bool national = string.Compare(region, "US", true) == 0;
            PathfinderClientModel.State state = c.StateSet.FirstOrDefault(s => s.ID == region);
            string stateName = state != null ? state.Name : "";
            bool isState = !string.IsNullOrEmpty(stateName);

            if (national)
                chartTitle = Resources.Resource.Label_National;
            else
            {
                if (isState)
                {
                    chartTitle = stateName;
                }

                else if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["rcbGeographyType"]) && System.Web.HttpContext.Current.Request.QueryString["rcbGeographyType"].ToString() == "4")
                {
                    string Territory_ID = System.Web.HttpContext.Current.Request.QueryString["Geography_ID"].ToString();
                    //get the AM name for selected Geography_ID

                    var q = (from p in c.AccountManagersByTerritorySet
                             where p.Territory_ID == Territory_ID
                             select p.FullName).FirstOrDefault();

                    chartTitle = q;

                }
                else
                {
                    PathfinderClientModel.Territory territory = c.TerritorySet.FirstOrDefault(t => t.ID == region);
                    chartTitle = territory != null ? territory.Name : "";
                }
            }


            //if(!byUser) //regular query by region (other than state)
            //    return PathfinderApplication.Data.Generic.CreateGenericEntityQuery<PathfinderClientModel.TierCoverageByPlan>(clientContext, new TierCoverageQueryDefinition(queryValues), true).ToList();
            //else if(national || isState) //regular query by user (national or state)
            //    return PathfinderApplication.Data.Generic.CreateGenericEntityQuery<PathfinderClientModel.TierCoverageByUser>(clientContext, new TierCoverageQueryDefinition(queryValues), true).ToList();
            //else //query by user for a region (other than a state)
            //    return PathfinderApplication.Data.Generic.CreateGenericEntityQuery<PathfinderClientModel.TierCoverageByUserAndRegion>(clientContext, new TierCoverageQueryDefinition(queryValues), true).ToList();
            // return Generic.CreateGenericEntityQuery<T>(context, typeof(QT).GetConstructor(new Type[] { typeof(string), typeof(NameValueCollection) }).Invoke(new object[] { EntityTypeName, queryValues }) as QueryDefinition, true).ToList();
            if (EntityTypeName == "ReportsFormularyStatusSummary" || EntityTypeName == "ReportsTierCoverage" || EntityTypeName == "FormularyCoverageSummary")
            {
                PathfinderModel.PathfinderEntities ctx = new PathfinderModel.PathfinderEntities();
                return Generic.CreateGenericEntityQuery<T>(ctx, typeof(QT).GetConstructor(new Type[] { typeof(string), typeof(NameValueCollection) }).Invoke(new object[] { EntityTypeName, queryValues }) as QueryDefinition, true).ToList();
            }
            else if (useClientContext)
                return Generic.CreateGenericEntityQuery<T>(context, typeof(QT).GetConstructor(new Type[] { typeof(string), typeof(NameValueCollection) }).Invoke(new object[] { EntityTypeName, queryValues }) as QueryDefinition, true).ToList();
            else
                return Generic.CreateGenericEntityQuery<T>(c, typeof(QT).GetConstructor(new Type[] { typeof(string), typeof(NameValueCollection) }).Invoke(new object[] { EntityTypeName, queryValues }) as QueryDefinition, true).ToList();

            //}


            //}
        }
    }
}
