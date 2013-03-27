using System;
using System.Data.Services;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using PathfinderModel;
using System.Text;
using System.Linq.Expressions;
using Pinsonault.Web.Services;
using System.Web;

namespace Pinsonault.Application.StandardReports
{
    public class StandardReportsDataService : PathfinderDataServiceBase<PathfinderEntities>
    {
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(IDataServiceConfiguration config)
        {
            Pinsonault.Web.Support.InitializeService(config);

            //Important: Only list Entity Sets that should be accessible through the web service.  If client side queries are not required to populate a grid or another control do not make it public     
            config.SetEntitySetAccessRule("CoveredLivesListViewSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("ReportsFormularyStatusDrilldownSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("ReportsFormularyDrilldownSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("ReportsTierCoverageDrilldownSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("LivesDistributionSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("LivesDistributionByAllSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("AffiliationsFormularyParentPlanSet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("AffiliationsFormularySet", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("FormularyCoverageDrilldownSet", EntitySetRights.AllRead);            
            //        
        }


        /// <summary>
        /// Returns text that can be used in javascript to construct an array of report options available to the current user based on their client id.
        /// </summary>
        /// <returns></returns>
        [WebGet]
        public override string GetModuleOptions()
        {
            return CurrentDataSource.GetUserReportOptionsAsJSON(Pinsonault.Web.Session.UserID, 3);
        }

        //public static Expression<Func<T, bool>> FormularyRoleFilter<T>()
        //{
        //    int sectionID = Pinsonault.Data.Generic.ParseServiceRequestForIdentifier<int>(HttpContext.Current.Request.QueryString["$filter"], "Section_ID");


        //    string role = string.Format("frmly_{0}", sectionID);
        //    bool hasRole = HttpContext.Current.User.IsInRole(role);
        //    return f => hasRole;

        //}        

        #region Query Interceptors

        [QueryInterceptor("ReportsFormularyStatusDrilldownSet")]
        public Expression<Func<ReportsFormularyStatusDrilldown, bool>> FilterReportsFormularyStatusDrilldownByRank()
        {
            string val = System.Web.HttpContext.Current.Request["Rank"];           
            
            if (!string.IsNullOrEmpty(val))
            {
                int rank;
                if (int.TryParse(val, out rank))
                {
                    return e => e.Rank <= rank;
                }
            }

            return e => true; //get all - no rank filtering
        }

        [QueryInterceptor("ReportsFormularyStatusDrilldownSet")]
        public Expression<Func<ReportsFormularyStatusDrilldown, bool>> FilterReportsFormularyStatusDrilldownByClient()
        {
            int clientID = Pinsonault.Web.Session.ClientID;
            return e => e.Client_ID == clientID;
        }

        [QueryInterceptor("ReportsFormularyStatusDrilldownSet")]
        public Expression<Func<ReportsFormularyStatusDrilldown, bool>> FilterReportsFormularyStatusDrilldownByPredominant()
        {
            bool IsPredominant = false;
            if (System.Web.HttpContext.Current.Request["Is_Predominant"] == "true")
                IsPredominant = true;

            if (IsPredominant)
                return e => e.Is_Predominant == IsPredominant;
            else
                return e => true;
        }

        [QueryInterceptor("ReportsFormularyStatusDrilldownSet")]
        public Expression<Func<ReportsFormularyStatusDrilldown, bool>> FilterReportsFormularyStatusDrilldownByRestrictions()
        {
            string PA = System.Web.HttpContext.Current.Request["PA"];
            string QL = System.Web.HttpContext.Current.Request["QL"];
            string ST = System.Web.HttpContext.Current.Request["ST"];

            if( !string.IsNullOrEmpty(PA) || !string.IsNullOrEmpty(QL) || !string.IsNullOrEmpty(ST))
                return e => e.PA == PA ||
                            e.QL == QL ||
                            e.ST == ST ;
            else
                return e => true;
         
        }
        //[QueryInterceptor("ReportsFormularyStatusDrilldownSet")]
        //public Expression<Func<ReportsFormularyStatusDrilldown, bool>> FilterReportsFormularyStatusDrilldownByExcludedSection()
        //{
        //    string excludePBM = System.Web.HttpContext.Current.Request.QueryString["excludePBM"];

        //    if (!string.IsNullOrEmpty(excludePBM))
        //        return e => e.Section_ID != 4;
        //    else
        //        return e => true;
        //}

        [QueryInterceptor("ReportsFormularyStatusDrilldownSet")]
        public Expression<Func<ReportsFormularyStatusDrilldown, bool>> FilterReportsFormularyStatusDrilldownByOnlyPBMSelection()
        {
            //if only PBM is selected, then show all plans which have PBM name associated with it
            string onlyPBM = System.Web.HttpContext.Current.Request.QueryString["onlyPBM"];

            if (!string.IsNullOrEmpty(onlyPBM))
                return e => e.PBM_Id > 0;
            else
                return e => true;
        }


        [QueryInterceptor("ReportsFormularyStatusDrilldownSet")]
        public Expression<Func<ReportsFormularyStatusDrilldown, bool>> FilterReportsFormularyStatusDrilldownByExcludedSegment()
        {
            string excludeSegment = System.Web.HttpContext.Current.Request.QueryString["excludeSegment"];

            if (!string.IsNullOrEmpty(excludeSegment))
            {
                int Segment_id = Convert.ToInt32(excludeSegment);
                return e => e.Segment_ID != Segment_id;
            }
            else
                return e => true;
        }

        [QueryInterceptor("ReportsFormularyDrilldownSet")]
        public Expression<Func<ReportsFormularyDrilldown, bool>> FilterReportsFormularyDrilldownByExcludedSegment()
        {
            string excludeSegment = System.Web.HttpContext.Current.Request.QueryString["excludeSegment"];

            if (!string.IsNullOrEmpty(excludeSegment))
            {
                int Segment_id = Convert.ToInt32(excludeSegment);
                return e => e.Segment_ID != Segment_id;
            }
            else
                return e => true;
        }

        [QueryInterceptor("ReportsFormularyDrilldownSet")]
        public Expression<Func<ReportsFormularyDrilldown, bool>> FilterFormularyDrilldownByRank()
        {
            string val = System.Web.HttpContext.Current.Request["Rank"];
            if (!string.IsNullOrEmpty(val))
            {
                int rank;
                if (int.TryParse(val, out rank))
                {
                    return e => e.Rank <= rank;
                }
            }

            return e => true; //get all - no rank filtering
        }

        [QueryInterceptor("ReportsFormularyDrilldownSet")]
        public Expression<Func<ReportsFormularyDrilldown, bool>> FilterFormularyDrilldownByPredominant()
        {
            bool IsPredominant = false;
            if (System.Web.HttpContext.Current.Request["Is_Predominant"] == "true")
                IsPredominant = true;

            if (IsPredominant)
                return e => e.Is_Predominant == IsPredominant;
            else
                return e => true;
        }

        [QueryInterceptor("ReportsFormularyDrilldownSet")]
        public Expression<Func<ReportsFormularyDrilldown, bool>> FilterReportsFormularyDrilldownSetByOnlyPBMSelection()
        {
            //if only PBM is selected, then show all plans which have PBM name associated with it
            string onlyPBM = System.Web.HttpContext.Current.Request.QueryString["onlyPBM"];

            if (!string.IsNullOrEmpty(onlyPBM))
                return e => e.PBM_Id > 0;
            else
                return e => true;
        }

        [QueryInterceptor("ReportsFormularyDrilldownSet")]
        public Expression<Func<ReportsFormularyDrilldown, bool>> FilterFormularyDrilldownByRole()
        {
            bool cp = HttpContext.Current.User.IsInRole("frmly_1");
            bool medd = HttpContext.Current.User.IsInRole("frmly_17");
            bool sm = HttpContext.Current.User.IsInRole("frmly_9");
            bool dod = HttpContext.Current.User.IsInRole("frmly_12");
            bool pbm = HttpContext.Current.User.IsInRole("frmly_4");
            bool mm = HttpContext.Current.User.IsInRole("frmly_6");
            bool va = HttpContext.Current.User.IsInRole("frmly_11");

            return f => (((f.Section_ID == 1) && f.Segment_ID == 1 && cp)
                || (((f.Segment_ID == 2) || (f.Segment_ID == 8)) && medd)
                || (f.Section_ID == 4 && pbm)
                || (f.Section_ID == 6 && mm)
                || (f.Section_ID == 9 && sm)
                || (f.Section_ID == 12 && dod)
                || (f.Section_ID == 11 && va)
                );
        }

        [QueryInterceptor("ReportsFormularyDrilldownSet")]
        public Expression<Func<ReportsFormularyDrilldown, bool>> FilterReportsFormularyDrilldownByClient()
        {
            int clientID = Pinsonault.Web.Session.ClientID;
            return e => e.Client_ID == clientID;
        }

        [QueryInterceptor("ReportsFormularyDrilldownSet")]
        public Expression<Func<ReportsFormularyDrilldown, bool>> FilterReportsFormularyDrilldownByRestrictions()
        {
            string PA = System.Web.HttpContext.Current.Request["PA"];
            string QL = System.Web.HttpContext.Current.Request["QL"];
            string ST = System.Web.HttpContext.Current.Request["ST"];

            if (!string.IsNullOrEmpty(PA) || !string.IsNullOrEmpty(QL) || !string.IsNullOrEmpty(ST))
                return e => e.PA == PA ||
                            e.QL == QL ||
                            e.ST == ST;
            else
                return e => true;
        }

        [QueryInterceptor("ReportsTierCoverageDrilldownSet")]
        public Expression<Func<ReportsTierCoverageDrilldown, bool>> FilterReportsTierCoverageDrilldownByRank()
        {
            string val = System.Web.HttpContext.Current.Request["Rank"];
            if (!string.IsNullOrEmpty(val))
            {
                int rank;
                if (int.TryParse(val, out rank))
                {
                    return e => e.Rank <= rank;
                }
            }

            return e => true; //get all - no rank filtering
        }

        [QueryInterceptor("ReportsTierCoverageDrilldownSet")]
        public Expression<Func<ReportsTierCoverageDrilldown, bool>> FilterReportsTierCoverageDrilldownByPredominant()
        {
            bool IsPredominant = false;
            if (System.Web.HttpContext.Current.Request["Is_Predominant"] == "true")
                IsPredominant = true;

            if (IsPredominant)
                return e => e.Is_Predominant == IsPredominant;
            else
                return e => true;
        }

        [QueryInterceptor("ReportsTierCoverageDrilldownSet")]
        public Expression<Func<ReportsTierCoverageDrilldown, bool>> FilterReportsTierCoverageDrilldownByClient()
        {
            int clientID = Pinsonault.Web.Session.ClientID;
            return e => e.Client_ID == clientID;
        }

        [QueryInterceptor("ReportsTierCoverageDrilldownSet")]
        public Expression<Func<ReportsTierCoverageDrilldown, bool>> FilterReportsTierCoverageDrilldownByRestrictions()
        {
            string PA = System.Web.HttpContext.Current.Request["PA"];
            string QL = System.Web.HttpContext.Current.Request["QL"];
            string ST = System.Web.HttpContext.Current.Request["ST"];

            if (!string.IsNullOrEmpty(PA) || !string.IsNullOrEmpty(QL) || !string.IsNullOrEmpty(ST))
                return e => e.PA == PA ||
                            e.QL == QL ||
                            e.ST == ST;
            else
                return e => true;
        }
        //[QueryInterceptor("ReportsTierCoverageDrilldownSet")]
        //public Expression<Func<ReportsTierCoverageDrilldown, bool>> FilterReportsTierCoverageDrilldownByExcludedSection()
        //{
        //    string excludePBM = System.Web.HttpContext.Current.Request.QueryString["excludePBM"];

        //    if (!string.IsNullOrEmpty(excludePBM))
        //        return e => e.Section_ID != 4;
        //    else
        //        return e => true;
        //}

        [QueryInterceptor("ReportsTierCoverageDrilldownSet")]
        public Expression<Func<ReportsTierCoverageDrilldown, bool>> FilterReportsTierCoverageDrilldownByOnlyPBMSelection()
        {
            //if only PBM is selected, then show all plans which have PBM name associated with it
            string onlyPBM = System.Web.HttpContext.Current.Request.QueryString["onlyPBM"];

            if (!string.IsNullOrEmpty(onlyPBM))
                return e => e.PBM_Id > 0;
            else
                return e => true;
        }


        [QueryInterceptor("ReportsTierCoverageDrilldownSet")]
        public Expression<Func<ReportsTierCoverageDrilldown, bool>> FilterReportsTierCoverageDrilldownByExcludedSegment()
        {
            string excludeSegment = System.Web.HttpContext.Current.Request.QueryString["excludeSegment"];
            
            if (!string.IsNullOrEmpty(excludeSegment))
            {
                int Segment_id = Convert.ToInt32(excludeSegment);
                return e => e.Segment_ID != Segment_id;
            }
            else
                return e => true;
        }

        [QueryInterceptor("LivesDistributionSet")]
        public Expression<Func<LivesDistribution, bool>> FilterLivesDistributionByRank()
        {
            string val = System.Web.HttpContext.Current.Request["Rank"];
            if (!string.IsNullOrEmpty(val))
            {
                int rank;
                if (int.TryParse(val, out rank))
                {
                    return e => e.Rank <= rank;
                }
            }

            return e => true; //get all - no rank filtering
        }

        [QueryInterceptor("LivesDistributionSet")]
        public Expression<Func<LivesDistribution, bool>> FilterLivesDistributionByClient()
        {
            int clientID = Pinsonault.Web.Session.ClientID;
            return e => e.Client_ID == clientID;
        }

        [QueryInterceptor("LivesDistributionByAllSet")]
        public Expression<Func<LivesDistributionByAll, bool>> FilterLivesDistributionByAllByClient()
        {
            int clientID = Pinsonault.Web.Session.ClientID;
            return e => e.Client_ID == clientID;
        }

        [QueryInterceptor("AffiliationsFormularyParentPlanSet")]
        public Expression<Func<AffiliationsFormularyParentPlan, bool>> AffiliationsFormularyParentPlan()
        {
            int clientID = Pinsonault.Web.Session.ClientID;
            return e => e.Client_ID == clientID;
        }

        [QueryInterceptor("AffiliationsFormularySet")]
        public Expression<Func<AffiliationsFormulary, bool>> AffiliationsFormulary()
        {
            int clientID = Pinsonault.Web.Session.ClientID;
            return e => e.Client_ID == clientID;
        }

        #region Formulary coverage Query Interceptors
        [QueryInterceptor("FormularyCoverageDrilldownSet")]
        public Expression<Func<FormularyCoverageDrilldown, bool>> FilterFormularyCoverageDrilldownByRank()
        {
            string val = System.Web.HttpContext.Current.Request["Rank"];

            if (!string.IsNullOrEmpty(val))
            {
                int rank;
                if (int.TryParse(val, out rank))
                {
                    return e => e.Rank <= rank;
                }
            }

            return e => true; //get all - no rank filtering
        }
        [QueryInterceptor("FormularyCoverageDrilldownSet")]
        public Expression<Func<FormularyCoverageDrilldown, bool>> FilterFormularyCoverageDrilldownByClient()
        {
            int clientID = Pinsonault.Web.Session.ClientID;
            return e => e.Client_ID == clientID;
        }

        [QueryInterceptor("FormularyCoverageDrilldownSet")]
        public Expression<Func<FormularyCoverageDrilldown, bool>> FilterFormularyCoverageDrilldownByPredominant()
        {
            bool IsPredominant = false;
            if (System.Web.HttpContext.Current.Request["Is_Predominant"] == "true")
                IsPredominant = true;

            if (IsPredominant)
                return e => e.Is_Predominant == IsPredominant;
            else
                return e => true;
        }

        [QueryInterceptor("FormularyCoverageDrilldownSet")]
        public Expression<Func<FormularyCoverageDrilldown , bool>> FilterFormularyCoverageDrilldownByRestrictions()
        {
            string PA = System.Web.HttpContext.Current.Request["PA"];
            string QL = System.Web.HttpContext.Current.Request["QL"];
            string ST = System.Web.HttpContext.Current.Request["ST"];

            if (!string.IsNullOrEmpty(PA) || !string.IsNullOrEmpty(QL) || !string.IsNullOrEmpty(ST))
                return e => e.PA == PA ||
                            e.QL == QL ||
                            e.ST == ST;
            else
                return e => true;

        }
        //[QueryInterceptor("FormularyCoverageDrilldownSet")]
        //public Expression<Func<FormularyCoverageDrilldown, bool>> FilterReportsFormularyCoverageDrilldownByExcludedSection()
        //{
        //    string excludePBM = System.Web.HttpContext.Current.Request.QueryString["excludePBM"];

        //    if (!string.IsNullOrEmpty(excludePBM))
        //        return e => e.Section_ID != 4;
        //    else
        //        return e => true;
        //}

        [QueryInterceptor("FormularyCoverageDrilldownSet")]
        public Expression<Func<FormularyCoverageDrilldown, bool>> FilterReportsFormularyCoverageDrilldownByOnlyPBMSelection()
        {
            //if only PBM is selected, then show all plans which have PBM name associated with it
            string onlyPBM = System.Web.HttpContext.Current.Request.QueryString["onlyPBM"];

            if (!string.IsNullOrEmpty(onlyPBM))
                return e => e.PBM_Id > 0;
            else
                return e => true;
        }


        [QueryInterceptor("FormularyCoverageDrilldownSet")]
        public Expression<Func<FormularyCoverageDrilldown, bool>> FilterReportsFormularyCoverageDrilldownByExcludedSegment()
        {
            string excludeSegment = System.Web.HttpContext.Current.Request.QueryString["excludeSegment"];

            if (!string.IsNullOrEmpty(excludeSegment))
            {
                int Segment_id = Convert.ToInt32(excludeSegment);
                return e => e.Segment_ID != Segment_id;
            }
            else
                return e => true;
        }

        #endregion


        #endregion

    }
}