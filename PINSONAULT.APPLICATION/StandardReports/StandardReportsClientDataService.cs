using System;
using System.Data.Services;
using System.Linq;
using System.Linq.Expressions;
using System.ServiceModel.Web;
using PathfinderClientModel;
using Pinsonault.Web.Services;
using System.Web;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace Pinsonault.Application.StandardReports
{
    public class StandardReportsClientDataService : PathfinderClientDataServiceBase<PathfinderClientEntities>
    {
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(IDataServiceConfiguration config)
        {
            Pinsonault.Web.Support.InitializeService(config);

            //Important: Only list Entity Sets that should be accessible through the web service.  If client side queries are not required to populate a grid or another control do not make it public     
            config.SetEntitySetAccessRule("PlanSearchSet", EntitySetRights.AllRead);
            //config.SetEntitySetAccessRule("LivesDistributionSet", EntitySetRights.AllRead);
            //config.SetEntitySetAccessRule("ChildFormularyListSet", EntitySetRights.AllRead);
            //config.SetEntitySetAccessRule("ParentPlanFormularyViewSet", EntitySetRights.AllRead);

            //config.SetEntitySetAccessRule("ReportsTierCoverageDrilldownSet", EntitySetRights.AllRead);
            //config.SetEntitySetAccessRule("ReportsFormularyStatusDrilldownSet", EntitySetRights.AllRead);
            //config.SetEntitySetAccessRule("ReportsFormularyDrilldownSet", EntitySetRights.AllRead);
            //config.SetEntitySetAccessRule("*", EntitySetRights.AllRead);
            config.UseVerboseErrors = true;
            //
        }

        public static Expression<Func<T, bool>> FormularyRoleFilter<T>()
        {
            int sectionID = Pinsonault.Data.Generic.ParseServiceRequestForIdentifier<int>(HttpContext.Current.Request.QueryString["$filter"], "Section_ID");


            string role = string.Format("frmly_{0}", sectionID);
            bool hasRole = HttpContext.Current.User.IsInRole(role);
            return f => hasRole;

        }

        #region Query Interceptors

        //[QueryInterceptor("LivesDistributionSet")]
        //public Expression<Func<LivesDistribution, bool>> FilterLivesDistributionByRank()
        //{
        //    string val = System.Web.HttpContext.Current.Request["Rank"];
        //    if (!string.IsNullOrEmpty(val))
        //    {
        //        int rank;
        //        if (int.TryParse(val, out rank))
        //        {
        //            return e => e.Rank <= rank;
        //        }
        //    }

        //    return e => true; //get all - no rank filtering
        //}

        //[QueryInterceptor("ReportsFormularyDrilldownSet")]
        //public Expression<Func<ReportsFormularyDrilldown, bool>> FilterFormularyDrilldownByRank()
        //{
        //    string val = System.Web.HttpContext.Current.Request["Rank"];
        //    if (!string.IsNullOrEmpty(val))
        //    {
        //        int rank;
        //        if (int.TryParse(val, out rank))
        //        {
        //            return e => e.Rank <= rank;
        //        }
        //    }

        //    return e => true; //get all - no rank filtering
        //}

        //[QueryInterceptor("ReportsFormularyDrilldownSet")]
        //public Expression<Func<ReportsFormularyDrilldown, bool>> FilterFormularyDrilldownByRole()
        //{
        //    return FormularyRoleFilter<ReportsFormularyDrilldown>();
        //}

        //[QueryInterceptor("ReportsFormularyDrilldownSet")]
        //public Expression<Func<ReportsFormularyDrilldown, bool>> FilterFormularyDrilldownByRestrictions()
        //{
        //    NameValueCollection queryString = HttpContext.Current.Request.QueryString;
        //    string pa = queryString["PA"];
        //    string ql = queryString["QL"];
        //    string st = queryString["ST"];

        //    //only applies when sending custom restrictions - Covered With Restrictions
        //    if (!string.IsNullOrEmpty(pa) || !string.IsNullOrEmpty(ql) || !string.IsNullOrEmpty(st))
        //        return e => e.PA == pa || e.QL == ql || e.ST == st;

        //    //not customizing restrction filter so just return true;
        //    return e => true;
        //}

        //[QueryInterceptor("ReportsTierCoverageDrilldownSet")]
        //public Expression<Func<ReportsTierCoverageDrilldown, bool>> FilterTierCoverageDrilldownByRestrictions()
        //{
        //    NameValueCollection queryString = HttpContext.Current.Request.QueryString;
        //    string pa = queryString["PA"];
        //    string ql = queryString["QL"];
        //    string st = queryString["ST"];

        //    //only applies when sending custom restrictions - Covered With Restrictions
        //    if (!string.IsNullOrEmpty(pa) || !string.IsNullOrEmpty(ql) || !string.IsNullOrEmpty(st))
        //        return e => e.PA == pa || e.QL == ql || e.ST == st;

        //    //not customizing restrction filter so just return true;
        //    return e => true;
        //}

        //[QueryInterceptor("ReportsTierCoverageDrilldownSet")]
        //public Expression<Func<ReportsTierCoverageDrilldown, bool>> FilterReportsTierCoverageDrilldownByRank()
        //{
        //    string val = System.Web.HttpContext.Current.Request["Rank"];
        //    if (!string.IsNullOrEmpty(val))
        //    {
        //        int rank;
        //        if (int.TryParse(val, out rank))
        //        {
        //            return e => e.Rank <= rank;
        //        }
        //    }

        //    return e => true; //get all - no rank filtering
        //}


        //[QueryInterceptor("ReportsFormularyStatusDrilldownSet")]
        //public Expression<Func<ReportsFormularyStatusDrilldown, bool>> FilterReportsFormularyStatusDrilldownByRank()
        //{
        //    string val = System.Web.HttpContext.Current.Request["Rank"];
        //    if (!string.IsNullOrEmpty(val))
        //    {
        //        int rank;
        //        if (int.TryParse(val, out rank))
        //        {
        //            return e => e.Rank <= rank;
        //        }
        //    }

        //    return e => true; //get all - no rank filtering
        //}


        //[QueryInterceptor("ReportsTierCoverageDrilldownSet")]
        //public Expression<Func<ReportsTierCoverageDrilldown, bool>> FilterReportsTierCoverageDrilldownByClient()
        //{
        //    int clientID = Pinsonault.Web.Session.ClientID;
        //    return e => e.Client_ID == clientID;
        //}

        //[QueryInterceptor("ReportsTierCoverageDrilldownSet")]
        //public Expression<Func<ReportsTierCoverageDrilldown, bool>> FilterReportsTierCoverageDrilldownByRestrictions()
        //{
        //    NameValueCollection queryString = HttpContext.Current.Request.QueryString;
        //    string pa = queryString["PA"];
        //    string ql = queryString["QL"];
        //    string st = queryString["ST"];

        //    //only applies when sending custom restrictions - Covered With Restrictions
        //    if (!string.IsNullOrEmpty(pa) || !string.IsNullOrEmpty(ql) || !string.IsNullOrEmpty(st))
        //        return e => e.PA == pa || e.QL == ql || e.ST == st;

        //    //not customizing restrction filter so just return true;
        //    return e => true;
        //}

        //[QueryInterceptor("ReportsFormularyStatusDrilldownSet")]
        //public Expression<Func<ReportsFormularyStatusDrilldown, bool>> FilterReportsFormularyStatusDrilldownByClient()
        //{
        //    int clientID = Pinsonault.Web.Session.ClientID;
        //    return e => e.Client_ID == clientID;
        //}

        //[QueryInterceptor("ReportsFormularyStatusDrilldownSet")]
        //public Expression<Func<ReportsFormularyStatusDrilldown, bool>> FilterReportsFormularyStatusDrilldownByRestrictions()
        //{            
        //    NameValueCollection queryString = HttpContext.Current.Request.QueryString;
        //    string pa = queryString["PA"];
        //    string ql = queryString["QL"];
        //    string st = queryString["ST"];

        //    //only applies when sending custom restrictions - Covered With Restrictions
        //    if (!string.IsNullOrEmpty(pa) || !string.IsNullOrEmpty(ql) || !string.IsNullOrEmpty(st))
        //        return e => e.PA == pa || e.QL == ql || e.ST == st;

        //    //not customizing restrction filter so just return true;
        //    return e => true;
        //}

        //[QueryInterceptor("ReportsFormularyDrilldownSet")]
        //public Expression<Func<ReportsFormularyDrilldown, bool>> FilterReportsFormularyDrilldownByClient()
        //{
        //    int clientID = Pinsonault.Web.Session.ClientID;
        //    return e => e.Client_ID == clientID;
        //}


        //[QueryInterceptor("ChildFormularyListSet")]
        //public Expression<Func<ChildFormularyList, bool>> ChildFormularyList()
        //{
        //    int clientID = Pinsonault.Web.Session.ClientID;
        //    return e => e.Client_ID == clientID;
        //}

        //[QueryInterceptor("ParentPlanFormularyViewSet")]
        //public Expression<Func<ParentPlanFormularyView, bool>> ParentPlanFormularyView()
        //{
        //    int clientID = Pinsonault.Web.Session.ClientID;
        //    return e => e.Client_ID == clientID;
        //}
        #endregion


        //[WebGet]
        //public int GetReportsFormularyDrilldownCount(string where)
        //{
        //    if (String.IsNullOrEmpty(where))
        //        return CurrentDataSource.ReportsFormularyDrilldownSet.Where(this.FilterReportsFormularyDrilldownByClient()).Count();
        //    else
        //        return CurrentDataSource.ReportsFormularyDrilldownSet.Where(where).Where(this.FilterReportsFormularyDrilldownByClient()).Count();
        //}

        //[WebGet]
        //public int GetReportsTierCoverageDrilldownCount(string where)
        //{
        //    if (string.IsNullOrEmpty(where))
        //        return CurrentDataSource.ReportsTierCoverageDrilldownSet.Count(FilterReportsTierCoverageDrilldownByClient());
        //    else
        //        return CurrentDataSource.ReportsTierCoverageDrilldownSet.Where(where).Count(FilterReportsTierCoverageDrilldownByClient());
        //}

        //[WebGet]
        //public int GetReportsFormularyStatusDrilldownCount(string where)
        //{
        //    if (string.IsNullOrEmpty(where))
        //        return CurrentDataSource.ReportsFormularyStatusDrilldownSet.Count(FilterReportsFormularyStatusDrilldownByClient());
        //    else
        //        return CurrentDataSource.ReportsFormularyStatusDrilldownSet.Where(where).Count(FilterReportsFormularyStatusDrilldownByClient());
        //}

    }
}