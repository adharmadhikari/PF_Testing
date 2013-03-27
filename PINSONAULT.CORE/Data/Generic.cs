using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Diagnostics;
using System.Data.Objects.DataClasses;
using System.Text.RegularExpressions;

namespace Pinsonault.Data
{
    /// <summary>
    /// Summary description for Pinsonault.Web
    /// </summary>
    public static class Generic
    {
        #region Helper Functions

        public static string CollectionToQueryString(NameValueCollection col)
        {
            return string.Join("&", col.AllKeys.Select(k => string.Format("{0}={1}", k, col[k])).ToArray());
        }

        /// <summary>
        /// Creates an expression that will evaluate to true if the value returned by targetExp equals any item in the list.  Ex: trg.Plan_State = "NY" OR trg.Plan_State = "PA" OR trg.Plan_State = "NJ"
        /// </summary>
        /// <param name="items">List of items that the target can equal.</param>
        /// <param name="targetExp">Expression that returns a value to match.</param>
        /// <returns>Expression</returns>
        public static Expression GetExpForList<T>(IEnumerable<T> items, Expression targetExp)
        {
            if ( items.Count() > 0 )
            {
                Expression valueExp = Expression.Constant(items.First(), typeof(T));
                return Expression.Or(Expression.Equal(targetExp, valueExp), GetExpForList<T>(items.Skip(1).ToArray(), targetExp));
            }

            //just return false to create last OR condition that will never be true (only previous ones added will have affect on results)
            return Expression.Constant(false);
        }

        public static Expression GetNotExpForList<T>(IEnumerable<T> items, Expression targetExp)
        {
            if ( items.Count() > 0 )
            {
                Expression valueExp = Expression.Constant(items.First(), typeof(T));
                return Expression.And(Expression.NotEqual(targetExp, valueExp), GetNotExpForList<T>(items.Skip(1).ToArray(), targetExp));
            }

            //just return true to create last AND condition that will never be false (only previous ones added will have affect on results)
            return Expression.Constant(true);
        }

        public static Expression<Func<eT, bool>> GetFilterForList<eT, iT>(IEnumerable<iT> items, string PropertyName)
        {
            return GetFilterForList<eT, iT>(items, PropertyName, false);
        }

        public static Expression<Func<eT, bool>> GetNotFilterForList<eT, iT>(IEnumerable<iT> items, string PropertyName)
        {
            return GetFilterForList<eT, iT>(items, PropertyName, true);
        }

        static Expression<Func<eT, bool>> GetFilterForList<eT, iT>(IEnumerable<iT> items, string PropertyName, bool Not)
        {
            //Expression that creates a parameter for the current entity of type T
            ParameterExpression entity = Expression.Parameter(typeof(eT), "trg");

            if ( items.Count() > 0 )
            {
                //Expression that returns the PropertyName property value for a single entity of type T;
                Expression targetExp = Expression.Property(entity, PropertyName);

                //Construct the necessary OR conditions for the list
                Expression body;

                if ( !Not )
                    body = Generic.GetExpForList<iT>(items, targetExp);
                else
                    body = Generic.GetNotExpForList<iT>(items, targetExp);

                return Expression.Lambda<Func<eT, bool>>(body, entity);
            }

            return Expression.Lambda<Func<eT, bool>>(Expression.Constant(true), entity);
        }

        /// <summary>
        /// helper function for parsing DataService url's $filter for values - if this becomes hard to manage consider just appending query string with regular values as if it were page request (in addition to $filter required by data service). Only works for single values not an 'or' or 'in' clause
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static T ParseServiceRequestForIdentifier<T>(string query, string propertyName)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(string.Format(@".*{0} eq ", propertyName));
            string val = null;
            if ( reg.IsMatch(query) )
            {
                val = reg.Replace(query, "");
                reg = new System.Text.RegularExpressions.Regex(@" .*");
                val = reg.Replace(val, "").Trim(new char[] { '\'' });

                //Fix for 'or' clause, ie: (Section_ID eq 1 or Section_ID eq 17)
                //Remove last parenthesis
                val = val.Replace(")", "");
            }
            if ( !string.IsNullOrEmpty(val) )
                return (T)Convert.ChangeType(val, typeof(T));
            else
                return default(T);
        }



        #endregion


        /// <summary>
        /// Returns a Entity Framework Query for a specified entity set.  Where clause is constructed using QueryParameters.
        /// </summary>
        /// <typeparam name="T">Entity Set type that is being queried.</typeparam>
        /// <param name="context">Data context that will be used as the datasource.</param>
        /// <param name="QueryParameters">Collection of values that will be applied to the query to construct the Where clause.  The names of each parameter in the collection must match a property on the entity.</param>
        /// <returns>ObjectQuery that can be used to request data from the specified context (additional conditions can be applied by caller if needed).</returns>
        public static ObjectQuery<T> CreateEntityQuery<T>(System.Data.Objects.ObjectContext context, QueryDefinition QueryParameters)
        {
            return CreateEntityQuery<T, T>(context, QueryParameters, false);
        }

        public static ObjectQuery<T> CreateEntityQuery<T>(System.Data.Objects.ObjectContext context, QueryDefinition QueryParameters, bool ignorePageSize)
        {
            return CreateEntityQuery<T, T>(context, QueryParameters, ignorePageSize);
        }

        public static ObjectQuery<System.Data.Common.DbDataRecord> CreateGenericEntityQuery<T>(System.Data.Objects.ObjectContext context, QueryDefinition QueryParameters)
        {
            return CreateEntityQuery<T, System.Data.Common.DbDataRecord>(context, QueryParameters, false);
        }

        public static ObjectQuery<System.Data.Common.DbDataRecord> CreateGenericEntityQuery<T>(System.Data.Objects.ObjectContext context, QueryDefinition QueryParameters, bool ignorePageSize)
        {
            return CreateEntityQuery<T, System.Data.Common.DbDataRecord>(context, QueryParameters, ignorePageSize);
        }

        static ObjectQuery<RT> CreateEntityQuery<T, RT>(System.Data.Objects.ObjectContext context, QueryDefinition QueryParameters, bool ignorePageSize)
        {
            string value;

            context.MetadataWorkspace.LoadFromAssembly(typeof(T).Assembly);

            NameValueCollection queryParameters =
                QueryParameters == null ? 
                    new NameValueCollection() :
                    QueryParameters.Values;

            int pageSize = 50;
            int pageIndex = 0;
            string sort = null;

            //Get Paging & Sorting parameters from collection and remove so they are not added to query
            if ( !ignorePageSize )
            {
                value = queryParameters["__pagesize"];
                if ( !string.IsNullOrEmpty(value) )
                {
                    int.TryParse(value, out pageSize);
                }
            }
            else
                pageSize = 0;
            //queryParameters.Remove("__pagesize");

            value = queryParameters["__pageindex"];
            if ( !string.IsNullOrEmpty(value) )
            {
                int.TryParse(value, out pageIndex);                
            }
            //queryParameters.Remove("__pageindex");

            string aggr = queryParameters["__aggr"];
            //queryParameters.Remove("__aggr");

            string select = queryParameters["__select"];
            //queryParameters.Remove("__select");

            string expr = queryParameters["__expr"];
            //queryParameters.Remove("__expr");

            Dictionary<string, string> selectFields = new Dictionary<string,string>();
            if (!String.IsNullOrEmpty(select))
            {
                select = select.Replace(" ", "");
                selectFields = select.Split(',').ToDictionary(s => s);
            }

            sort = queryParameters["__sort"];
            if ( string.IsNullOrEmpty(sort) )
                sort = typeof(T).GetProperties()[0].Name;
            //queryParameters.Remove("__sort");

            //queryParameters.Remove("__options");
            //

            //Construct Query
            StringBuilder sb = new StringBuilder("Select ");
            StringBuilder groupBy = new StringBuilder();


            if ( string.IsNullOrEmpty(aggr) && string.IsNullOrEmpty(expr) )
            {
                IEnumerable<PropertyInfo> props = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.DeclaredOnly).Where(p => p.PropertyType.Name != "EntityReference`1");//.Where(p => p.GetCustomAttributes(typeof(EdmRelationshipNavigationPropertyAttribute), true).Count() == 0);
                //PropertyInfo prop;
                //for(int i=0;i<props.Length;i++)
                //{
                //    prop = props[i];
                //    if ( i > 0 ) sb.Append(",");
                //    sb.AppendFormat("entity.{0}", prop.Name);
                //}
                ////sb.Append("VALUE entity ");
                sb.Append(string.Join(",", props.Select(p => string.Format("entity.{0}", p.Name)).ToArray()));
            }
            else
            {
                if ( !string.IsNullOrEmpty(expr) )
                    expr = string.Format(" {0} ", expr); //pad string for parsing later.

                string[] fields;
                string field;
                string func;
                //convert sum(myfield) to sum:myfield and then split string
                if ( aggr == null ) aggr = "";

                string[] aggrs = aggr.Replace(" ", "").Replace("(", ":").Replace(")", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                bool started = false;
                PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.DeclaredOnly);
                foreach ( PropertyInfo prop in props )
                {
                    field = string.Format(":{0}", prop.Name);
                    fields = aggrs.Where(i => i.EndsWith(field)).ToArray();

                    if ( !string.IsNullOrEmpty(expr) )
                    {
                        Regex reg = new Regex(string.Format(@"[^(\s+as\s+)](\W){0}(\W)", prop.Name));
                        MatchCollection matches = reg.Matches(expr);
                        foreach ( Match match in matches )
                        {                            
                            expr = expr.Replace(match.Value, match.Value.Replace(prop.Name, string.Format("entity.[{0}]", prop.Name)));
                        }                        
                    }

                    if ( fields.Length == 0 )//not aggragate
                    {
                        if ( selectFields.Count == 0 || selectFields.ContainsKey(prop.Name) )
                        {
                            if ( started ) sb.Append(",");

                            started = true;
                            field = string.Format("entity.{0}", prop.Name);
                            sb.Append(field);
                            if ( groupBy.Length > 0 )
                                groupBy.Append(",");
                            groupBy.Append(field);
                        }
                    }
                    else
                    {
                        if ( started ) sb.Append(",");

                        started = false;
                        foreach ( string item in fields )
                        {
                            if ( started ) sb.Append(",");

                            func = item.Split(':')[0];
                            if ( string.Compare(func, "COUNT", true) == 0 )
                            {
                                sb.AppendFormat("COUNT(entity.{0}) as RecordCount", prop.Name);

                            }
                            else if ( !func.StartsWith("PERCENT", StringComparison.InvariantCultureIgnoreCase) )
                                sb.AppendFormat("{0}(entity.{1}) as {1}", func, prop.Name);
                            else if ( string.Compare(func, "PERCENTSUM", true) == 0 )
                                sb.AppendFormat("100 * (case when sum(entity.{0}) == 0 then 0 else Cast(sum(entity.{0}) as System.Decimal) / case when Cast(anyelement (select sum(subentity.{0}) as allrowsum from {1}Set as subentity ###COUNTWHERE###).allrowsum as System.Decimal) > 0 then Cast(anyelement (select sum(subentity.{0}) as allrowsum from {1}Set as subentity ###COUNTWHERE###).allrowsum as System.Decimal) else sum(entity.{0}) end end) as {0}_Percent", prop.Name, typeof(T).Name);
                            else if ( string.Compare(func, "PERCENTCOUNT", true) == 0 )
                                sb.AppendFormat("100 * (Cast(count(0) as System.Decimal) / Cast(anyelement (select count(0) as allrowcount from {1}Set as subentity ###COUNTWHERE###).allrowcount as System.Decimal)) as {0}_Percent", prop.Name, typeof(T).Name);
                            else
                                throw new ApplicationException("Invalid function");

                            started = true;
                        }
                        started = true;
                    }
                }

                if ( !string.IsNullOrEmpty(expr) )
                {
                    sb.Append(",").Append(expr);
                }
            }

            sb.AppendFormat(" from {0}Set as entity", typeof(T).Name);


            List<ObjectParameter> parameters = new List<ObjectParameter>();

            int index;
            int count;
            string[] vals;
            string paramName;
            bool includeAnd = false;
            bool includeOr = false;
            bool firstProp = false;           
            PropertyInfo property;
            Type propertyType;
            Type[] genargs;
            string referenceName;
            object propertyValue;
            string filterOperator;

            //Where Clause
            StringBuilder sb2 = new StringBuilder();
            string[] keyGroup;
            if ((!string.IsNullOrEmpty(QueryParameters.Values.Get("PA")) || !string.IsNullOrEmpty(QueryParameters.Values.Get("QL")) || !string.IsNullOrEmpty(QueryParameters.Values.Get("ST"))) && string.IsNullOrEmpty(QueryParameters.Values.Get("_others")))
            {
                string PA = "''";
                if (!string.IsNullOrEmpty(QueryParameters.Values.Get("PA")))
                    PA = QueryParameters.Values["PA"];

                string QL = "''";
                if (!string.IsNullOrEmpty(QueryParameters.Values.Get("QL")))
                    QL = QueryParameters.Values["QL"];

                string ST = "''";
                if (!string.IsNullOrEmpty(QueryParameters.Values.Get("ST")))
                    ST = QueryParameters.Values["ST"];

                sb2.AppendFormat("((entity.PA = '{0}') OR (entity.QL = '{1}') OR (entity.ST = '{2}')) and ",PA,QL,ST);   
            }           
           
            foreach ( string filterKey in QueryParameters.FilterKeys )
            {
                if ((filterKey != "PA" && filterKey != "QL" && filterKey != "ST") || !string.IsNullOrEmpty(QueryParameters.Values.Get("_others")))
                {
                keyGroup = filterKey.Replace(" ", "").Split(',');
                includeOr = false;

                foreach (string key in keyGroup)
                {

                    referenceName = string.Empty;

                    property = typeof(T).GetProperty(key);
                    //If prop is null then first check references for match - otherwise skip because it is invalid for entity 
                    if (property == null)
                    {
                        IEnumerable<PropertyInfo> referenceProps = typeof(T).GetProperties().Where(p => p.GetCustomAttributes(typeof(EdmRelationshipNavigationPropertyAttribute), true).Count() > 0);
                        foreach (PropertyInfo referenceProp in referenceProps)
                        {
                            property = referenceProp.PropertyType.GetProperty(key);
                            if (property != null)
                            {
                                referenceName = string.Format("{0}.", referenceProp.Name);
                                break;
                            }
                        }

                    }

                    if (property != null)
                    {
                        firstProp = true; //for tracking ands

                        if (property.PropertyType.IsGenericType)
                        {
                            genargs = property.PropertyType.GetGenericArguments();
                            if (genargs.Length > 0)
                                propertyType = genargs[0];
                            else //shouldn't happen but just in case default to string if type cannot be determined.
                                propertyType = typeof(string);
                        }
                        else
                            propertyType = property.PropertyType;

                        if (includeAnd)
                        {
                            sb2.Append(" and (");
                            includeAnd = false; //not true again until we are done with keyGroup
                        }
                        else if (includeOr)
                        {
                            sb2.Append(" or ");
                        }
                        else
                            sb2.Append(" (");
                        
                        value = queryParameters[key];

                        if (!string.IsNullOrEmpty(value)) //missing values are added as "IS NULL" filter so don't send key if it shouldn't be part of query
                        {
                            if (string.Compare(value, "#NULL", true) == 0)
                            {
                                sb2.AppendFormat("entity.{0}{1} IS NULL", referenceName, key);
                            }
                            else if (!typeof(DateTime).Equals(propertyType)) //not datetime 
                            {
                                filterOperator = QueryParameters.GetQueryCondition(key, propertyType);

                                vals = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                                if (string.Compare(filterOperator, "like", true) == 0 || vals.Length == 1)
                                {
                                    sb2.AppendFormat("entity.{0}{1} {2} @{1}", referenceName, key, filterOperator);

                                    propertyValue = Convert.ChangeType(value, propertyType);

                                    parameters.Add(new ObjectParameter(key, propertyValue));
                                }
                                else
                                {
                                    index = 0;
                                    count = vals.Length;
                                    sb2.AppendFormat("entity.{0}{1} in ", referenceName, key);
                                    sb2.Append("{");
                                    foreach (string val in vals)
                                    {
                                        paramName = string.Format("{0}{1}", key, index);
                                        sb2.AppendFormat("@{0}{1}", paramName, (index < count - 1 ? "," : ""));
                                        parameters.Add(new ObjectParameter(paramName, Convert.ChangeType(val.Trim(), propertyType)));
                                        index++;
                                    }
                                    sb2.Append("}");
                                }
                            }
                            else //date/time
                            {

                                value = value.ToLower().Replace(" am", "am").Replace(" pm", "pm");

                                vals = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                                if (vals.Length == 1 || (vals.Length == 2 && vals[1].IndexOf(':') != -1)) //only one value (no other date or time) or one date with time specified  (could be 1/1/2009 or 1/1/2009 11:00am)
                                {
                                    value = string.Join(" ", vals);
                                    sb2.AppendFormat("entity.{0}{1}=@{1}", referenceName, key);
                                    parameters.Add(new ObjectParameter(key, Convert.ChangeType(value, propertyType)));
                                }
                                else
                                {
                                    string date1 = vals[0];  //start out expecting just 2 dates (1/1/2009 3/1/2009)
                                    string date2 = vals[1];

                                    if (vals.Length > 2) //check if time is part of param 
                                    {
                                        if (vals[1].IndexOf(":") != -1) //2nd value is time
                                        {
                                            date1 += " " + vals[1];
                                            date2 = vals[2] + (vals.Length == 4 ? " " + vals[3] : "");   //3rd val is date and 4th could be time or not set
                                        }
                                        else //2nd value is date and not time so if there are 3 vals than 2nd and 3rd make up date/time
                                            date2 = vals[1] + " " + vals[2];
                                    }
                                    //could use between operator but generated query for getting count uses >= and <= so keeping the same
                                    sb2.AppendFormat("entity.{0}{1} >= @{1}1 and entity.{0}{1} <= @{1}2", referenceName, key);
                                    parameters.Add(new ObjectParameter(key + "1", Convert.ToDateTime(date1)));
                                    parameters.Add(new ObjectParameter(key + "2", Convert.ToDateTime(date2)));
                                }
                            }                            
                        }
                        else //query string param sent with no value - assume null check
                        {
                            sb2.AppendFormat("entity.{0}{1} is null", referenceName, key);
                        }

                        includeOr = true;
                    }                    
                }
                includeAnd = firstProp;
                if(includeOr) //something was set so close out
                    sb2.Append(")");
                }
            }
                    
            if ( queryParameters.Count > 0 && sb2.Length > 0 )
            {
                sb.Append(" where ");


                //sb2.Append("and entity.Plan_ID in {").Append(Get;
                //if (!string.IsNullOrEmpty(QueryParameters.Values.Get("excludePBM")) && QueryParameters.Values.Get("excludePBM") == "true")
                //    sb2.AppendFormat(" and entity.Section_ID != {0}", 4);  //exclude pbm

                //only PBM  is selected in filters
                if (!string.IsNullOrEmpty(QueryParameters.Values.Get("onlyPBM")) && QueryParameters.Values.Get("onlyPBM") == "true")
                    sb2.AppendFormat(" and entity.PBM_Id > {0}", 0);  //only pbm will display all plans where PBM_ID > 0

                if (!string.IsNullOrEmpty(QueryParameters.Values.Get("excludeSegment")))
                    sb2.AppendFormat(" and entity.Segment_ID != {0}", QueryParameters.Values.Get("excludeSegment"));  //exclude segment

                sb.Append(sb2);
                sb.Replace("###COUNTWHERE###", string.Format(" where {0}", sb2.Replace("entity.", "subentity.").ToString()));
            }
            else
                sb.Replace("###COUNTWHERE###", "");


            //Grouping
            if ( groupBy.Length > 0 )
            {
                sb.Append(" Group By ").Append(groupBy);
            }


            //Sorting
            string[] sortFields = sort.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (sortFields.Length > 0)
            {
                sb.Append(" order by ");
                bool includeComma = false;
                foreach (string sortField in sortFields)
                {
                    sb.AppendFormat("{0}entity.{1}", (includeComma ? ", " : ""), sortField.Trim());
                    includeComma = true;
                }
            }

            //Paging Directives
            if (pageSize > 0)
                sb.AppendFormat(" skip({0}) limit({1})", pageSize * pageIndex, pageSize);

            String eqlQuery = sb.ToString();
            Debug.WriteLine("Generic EQL: " + eqlQuery);
            ObjectQuery<RT> objectQuery = context.CreateQuery<RT>(eqlQuery, parameters.ToArray());

            objectQuery.MergeOption = MergeOption.NoTracking;

            context.CommandTimeout = 300;

            return objectQuery;
        }
    }
}