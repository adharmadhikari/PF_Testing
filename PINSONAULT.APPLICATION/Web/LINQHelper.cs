using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Pinsonault.Web.Data
{
    public class LINQHelper
    {
        /// <summary>
        /// Helper function to automatically generate a filter condition to apply to a Data Service Query Interceptor - The purpose of the filter is to restrict results to Channels purchased by client.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ApplicationID">ID of application to get list of available channels</param>
        /// <returns>Expression</returns>
        public static Expression<Func<T, bool>> GenFilterPlansByClientSections<T>(int ApplicationID)
        {
            Dictionary<string, PathfinderModel.ClientApplicationAccess> access = Pinsonault.Web.Session.ClientApplicationAccess;
            //Select all available Section IDs for Todays Accounts
            IEnumerable<int> q = access.Where(i => i.Value.ApplicationID == ApplicationID).Select(i => i.Value.SectionID);

            //Expression that creates a parameter for the current entity of type KeyContactSearch
            ParameterExpression entity = Expression.Parameter(typeof(T), "trg");

            //Expression that returns the Section_ID property value for a single entity of type KeyContactSearch;
            Expression targetExp = Expression.Property(entity, "Section_ID");

            //Construct the necessary OR conditions for the list of regions
            Expression body = Pinsonault.Data.Generic.GetExpForList<int>(q.ToArray(), targetExp);

            return Expression.Lambda<Func<T, bool>>(body, entity);
        }
    }
}
