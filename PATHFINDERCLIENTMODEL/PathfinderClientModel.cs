using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using PathfinderModel;
using Pinsonault.Data;
namespace PathfinderClientModel
{
    public partial class PathfinderClientEntities
    {
        partial void OnContextCreated()
        {
            this.SavingChanges += new EventHandler(OnSavingChanges);

            //can't apply this fix if using with data service and/or possibly EntityDataSource control
            ////if ( !string.IsNullOrEmpty(Connection.ConnectionString) )
            ////{
            ////    if ( string.Compare(Connection.ConnectionString, "name=PathfinderClientEntities", true) == 0 )
            ////        throw new ApplicationException("Invalid client connection string.  Please set using a call to Pinsonault.Web.Session.ClientConnectionString.");
            ////}
        }

        void OnSavingChanges(object sender, EventArgs e)
        {
            if ( string.Compare(Connection.ConnectionString, "name=PathfinderClientEntities", true) == 0 )
                throw new ApplicationException("Invalid client connection string.  Please set using a call to Pinsonault.Web.Session.ClientConnectionString.");
        }

        public IQueryable<KeyContactSearch> GetKeyContactByPlanID(int Section_ID, int Plan_ID)
        {
            //for VA and DoD we need to pull data from base plan view from PF db, as these sections aren't custom sections
            using (PathfinderEntities pfentity = new PathfinderEntities())
            {
                //For VA
                if (Section_ID == 11)
                {
                    //Get the VISN for the selected plan.
                    int? iVISN = (from p in pfentity.PlanInfoListViewSet
                                  where p.Plan_ID == Plan_ID
                                  select p.VISN).FirstOrDefault();

                    //Pull Plan_ID for current VISN
                    int? iVAPlan_ID = (from p in pfentity.PlanInfoListViewSet
                                       where p.Plan_Type_ID == 19 && p.VISN == iVISN
                                       select p.Plan_ID).FirstOrDefault();

                    List<int> ids = new List<int>();
                    if (iVAPlan_ID != null)
                        ids.Add(iVAPlan_ID.Value);
                    ids.Add(Plan_ID);

                    return KeyContactSearchSet.Where(Generic.GetFilterForList<KeyContactSearch, int>(ids, "Plan_ID"));
                }
                else if (Section_ID == 12)//For DoD
                {
                    //Get all plan ids for DODHeadquarters + selected planid.

                    List<int> ids = pfentity.GetDODHeadQuarters().ToList();
                    ids.Add(Plan_ID);
                    //GetFilterForList: dynamically creates a where condition based on the list of ids provided. 
                    //for eg: where condition would be: Plan_ID == x && Plan_ID == y && Plan_ID == z….  
                    return KeyContactSearchSet.Where(Generic.GetFilterForList<KeyContactSearch, int>(ids, "Plan_ID"));

                }
                else //For Other
                {
                    return from kc in KeyContactSearchSet
                           where kc.Plan_ID == Plan_ID
                           select kc;
                }
            }
         }
    }
}

