using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Data.Objects;
using System.Collections;
using System.Reflection;

namespace Pinsonault.Data
{
    /// <summary>
    /// Provides information on how to generate a dynamic query based on an entity 
    /// </summary>
    public class QueryDefinition
    {
        readonly NameValueCollection _queryString = null;

        /// <summary>
        /// Optional list of comma separated entity Properties.  This is an alternative to specifying "__select" in a query string and takes precidence over the query string value.
        /// </summary>
        public virtual string Select { get { return string.Empty; } }
        /// <summary>
        /// Optional list of comma separated entity Properties wrapped in an aggregate function such as "SUM(My_Property)".  This is an alternative to specifying "__aggr" in a query string and takes precidence over the query string value.
        /// </summary>
        public virtual string Aggregate { get { return string.Empty; } }
        /// <summary>
        /// Optional list of comma separated expressions that provide custom calculations on an entity Property or Properties.  This should be used if aggregate functions are not sufficient.  There is no query string alternative and any expressions listed are not checked for safety so user input should not be allowed.
        /// </summary>
        public virtual string Expressions { get { return string.Empty; } }
        /// <summary>
        /// Optional list of comma separated entity Properties and corrisponding sort direction.  This is an alternative to specifying "__sort" in a query string and takes precidence over the query string value.
        /// </summary>
        public virtual string Sort { get { return string.Empty; } }
        /// <summary>
        /// Indicates if the dynamically generated query should ignore the page size directive passed in the query string ("__pagesize").  Typically when generating queries for report exports it is necessary to ignore page size but in some cases limiting results to the Top N records may be necessary.  In those cases override this property and return false.
        /// </summary>
        public virtual bool IgnorePageSizeOnExport { get { return true; } }

        public string EntityTypeName { get; private set; }

        public QueryDefinition(NameValueCollection queryString)
        {
            NameValueCollection col = new NameValueCollection(queryString);

            if ( !string.IsNullOrEmpty(Select) )
                col["__select"] = Select;

            if ( !string.IsNullOrEmpty(Aggregate) )
                col["__aggr"] = Aggregate;

            if ( !string.IsNullOrEmpty(Sort) )
                col["__sort"] = Sort;

            //never allow from caller!
            if (!string.IsNullOrEmpty(Expressions))
                col["__expr"] = Expressions;
            else
                col.Remove("__expr");

            Preprocess(col);

            //now ok to assign
            _queryString = col;
        }

        public QueryDefinition(string EntityTypeName, NameValueCollection queryString)
            : this(queryString)
        {
            this.EntityTypeName = EntityTypeName;
        }

        public NameValueCollection Values
        {
            get { return new NameValueCollection(_queryString); }
        }

        /// <summary>
        /// Returns a list of fields that are requested for filtering by the user.  Fields that begin with an underscore are stripped because they are assumed to have special meaning and not intended to be applied to WHERE clause.  Override to hack the values.
        /// </summary>
        public virtual IEnumerable<string> FilterKeys
        {
            get { return _queryString.AllKeys.Where(s=>!s.StartsWith("_")); }
        }

        /// <summary>
        /// Indicates that Preprocess has been called on base class which is a requirement to call CreateQuery.
        /// </summary>
        protected bool CanCreateQuery { get; private set; }

        /// <summary>
        /// Indicates whether the results of the esql query must be a DbDataRecord.  This value is automatically set to true if __select, __aggr, or __expr query string values have been set.  
        /// </summary>
        bool RequiresGenericEntityQuery { get; set; }

        /// <summary>
        /// Indicates whether the results of the esql query should be the specified entity type or a generic DbDataRecord.  By default the return type is DbDataRecord but if an actual entity type is required then set this property to True.  An exception will occur in situations when it is only possible for the return type to be a DbDataRecord to handle custom select, aggregate, or expression fields.  This property is only used when calling CreateQuery on the QueryDefinition object.  It is not used when calling methods directly on Pinsonault.Data.Generic.
        /// </summary>
        public bool EntityAsReturnType { get; set; }

        /// <summary>
        /// Allows customization of queryString values prior to dynamically generating esql query.  Overrides will be present in queryString name/value collection when this method is called.
        /// </summary>
        /// <param name="queryString"></param>
        protected virtual void Preprocess(NameValueCollection queryString) 
        {
            RequiresGenericEntityQuery = !(string.IsNullOrEmpty(queryString["__select"]) && string.IsNullOrEmpty(queryString["__aggr"]) && string.IsNullOrEmpty(queryString["__expr"]));
            CanCreateQuery = true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="FieldName"></param>
        /// <param name="PropertyType">Data type of the property that a filter is applied.</param>
        /// <returns></returns>
        public virtual string GetQueryCondition(string FieldName, Type PropertyType)
        {
            string value = Values[FieldName];
            if ( PropertyType.Equals(typeof(String)) && (value.StartsWith("%") && value.EndsWith("%")) )
                return "like";
            else if ( string.Compare(FieldName, "rank", true) == 0 )
                return "<=";
            else
                return "=";
        }
        
        public virtual IEnumerable CreateQuery(ObjectContext context)
        {
            return CreateQuery(context, EntityAsReturnType);
        }

        public IEnumerable CreateQuery(ObjectContext context, bool EntityAsReturnType)
        {
            if ( CanCreateQuery )
            {
                if ( !EntityAsReturnType )
                    return CreateEntityQuery("CreateGenericEntityQuery", context);
                else if ( !RequiresGenericEntityQuery )
                    return CreateEntityQuery("CreateEntityQuery", context);
                else
                    throw new InvalidOperationException("EntityAsReturnType cannot be true when custom projections have been requested through __select, __aggr, or __expr properties.");
            }
            else
                throw new InvalidOperationException("CreateQuery cannot be called because Preprocess has not been called.  If a class overrides Preprocess make sure the base implementation is executed.");
        }
        
        IEnumerable CreateEntityQuery(string methodName, ObjectContext context)
        {
            if ( !string.IsNullOrEmpty(EntityTypeName) )
            {
                string typeName = string.Format("{0}.{1}, {2}", context.GetType().Namespace, EntityTypeName, context.GetType().Assembly.FullName);
                Type entityType = Type.GetType(typeName);
                if ( entityType != null )
                {
                    //GD 4/26/10
                    //MethodInfo method = typeof(Pinsonault.Web.Data.Generic).GetMethod("CreateEntityQuery", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(ObjectContext), typeof(NameValueCollection), typeof(bool) }, null);
                    MethodInfo method = typeof(Pinsonault.Data.Generic).GetMethod(methodName, BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(ObjectContext), typeof(QueryDefinition), typeof(bool) }, null);

                    method = method.MakeGenericMethod(entityType);

                    //filterSet = prepFilterSet(filterSet, def.Sort);

                    return method.Invoke(null, new object[] { context, this, this.IgnorePageSizeOnExport }) as IEnumerable;
                }
                else
                    throw new ArgumentException(string.Format("EntityTypeName is invalid for specified context. {0}", typeName));
            }
            else
                throw new InvalidOperationException(string.Format("{0} cannot be called if EntityTypeName has not been set.", methodName));
        }
    }

}
