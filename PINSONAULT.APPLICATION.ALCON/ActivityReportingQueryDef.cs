using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pinsonault.Data;
using PathfinderClientModel;
using System.Collections.Specialized;

namespace Pinsonault.Application.Alcon
{
    public class ActivityReportingQueryDef : QueryDefinition
    {
        public ActivityReportingQueryDef(string EntityTypeName, NameValueCollection queryString) : base(EntityTypeName, queryString) { }
        public ActivityReportingQueryDef(NameValueCollection queryString) : base(queryString) { }
        
        public override string Aggregate
        {
            get { return "Sum(Activity_Hours), PercentSum(Activity_Hours)"; }
        }

        public override string Select
        {
            get { return "Activity_Type_ID, Activity_Type_Name"; }
        }

        public override string Sort
        {
            get { return "Activity_Type_Name"; }
        }

        protected override void Preprocess(NameValueCollection queryString)
        {
            base.Preprocess(queryString);

            ////Hack lookups to search detail records
            //using ( PathfinderClientEntities clientContext = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString) )
            //{
            //    if ( clientContext != null )
            //    {
            //        string lookupIDs = queryString["Followup_Notes_ID"];
            //        IList<int> ccrIDs = null;
            //        bool mustRestrict = false;

            //        if ( !string.IsNullOrEmpty(lookupIDs) )
            //        {
            //            ccrIDs = clientContext.ContactReportFollowupNotesSet.Where(Generic.GetFilterForList<ContactReportFollowupNotes, int>(lookupIDsToInt(lookupIDs), "Followup_ID")).Select(crfn => crfn.Contact_Report_ID).ToList();
            //            mustRestrict = true;
            //        }

            //        lookupIDs = queryString["Meeting_Outcome_ID"];
            //        if ( !string.IsNullOrEmpty(lookupIDs) )
            //        {
            //            IList<int> ccrIDs2 = clientContext.ContactReportOutcomeSet.Where(Generic.GetFilterForList<ContactReportOutcome, int>(lookupIDsToInt(lookupIDs), "Outcome_ID")).Select(crfn => crfn.Contact_Report_ID).ToList();
            //            if ( ccrIDs != null )
            //                ccrIDs = ccrIDs.Intersect(ccrIDs2).ToList();
            //            else
            //                ccrIDs = ccrIDs2;

            //            mustRestrict = true;
            //        }

            //        if ( mustRestrict )
            //        {
            //            if ( ccrIDs.Count > 0 )
            //                queryString["Contact_Report_ID"] = string.Join(",", ccrIDs.Select(i => i.ToString()).ToArray());
            //            else
            //                queryString["Contact_Report_ID"] = "-1";
            //        }
            //    }
            //}

            //queryString.Remove("Followup_Notes_ID");
            //queryString.Remove("Meeting_Outcome_ID");
            ////
        }


        IEnumerable<int> lookupIDsToInt(string lookupIDs)
        {
            return lookupIDs.Replace(" ", "").Split(',').Select(s => Convert.ToInt32(s));
        }
    }

}
