using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Common;


namespace Pinsonault.Data.Reports
{
    public class ReportDefinition
    {
        public string ReportKey { get; set; }
        public string Tile { get; set; }
        public string SectionTitle { get; set; }

        string _entityTypeName = null;
        public string EntityTypeName
        {
            get { return _entityTypeName; }
            set
            {
                if ( QueryDefinition == null )
                    _entityTypeName = value;
                else
                    throw new InvalidOperationException("EntityTypeName cannot be set once filters have been applied and the QueryDefinition has been instantiated.");
            }
        }
        public string Sort { get; set; }
        public bool RequiresFilters { get; set; }
        public bool Visible { get; set; }

        public IList<ColumnMap> ColumnMapping { get; private set; }

        public ReportStyle Style { get; set; }

        public IList<ReportDefinition> ReportDefinitions { get; set; }

        public ReportDefinition()
        {
            Style = ReportStyle.Grid;
            RequiresFilters = true;
            Visible = true;
            ReportDefinitions = new List<ReportDefinition>();
            ColumnMapping = new List<ColumnMap>();
        }

        public QueryDefinition QueryDefinition { get; private set; }

        protected virtual QueryDefinition CreateQueryDefinition(NameValueCollection filters)
        {
            return new QueryDefinition(EntityTypeName, filters);
        }

        public QueryDefinition ApplyFilters(NameValueCollection filters)
        {
            QueryDefinition = CreateQueryDefinition(filters);

            return QueryDefinition;
        }

        /// <summary>
        /// Retreives the column mapping for the current report definition using the supplied data context.
        /// </summary>
        /// <typeparam name="T">Entity type that contains column information.</typeparam>
        /// <param name="context">Data context that is used to perform the column mapping lookup.</param>
        /// <returns></returns>
        public void LoadColumnMap<T>(ObjectContext context)
        {
            //Some report definitions are simply hosts for sub sections and do not have mappings
            if ( ReportDefinitions.Count == 0 ) 
            {
                NameValueCollection col = new NameValueCollection();
                col["ReportKey"] = ReportKey;
                col["TileKey"] = Tile;
                col["__sort"] = "SortIndex, PropertyName";

                QueryDefinition query = new QueryDefinition(col);

                var q = Generic.CreateGenericEntityQuery<T>(context, query);

                foreach ( DbDataRecord record in q )
                {
                    ColumnMapping.Add(CreateColumnMap(record));
                }

                if ( ColumnMapping.Count == 0 )
                    throw new ArgumentException(string.Format("The report/section {0}/{1} specified does not have any column mappings for Excel.", ReportKey, Tile));
            }
            else //host - so load child defs
            {
                foreach ( ReportDefinition reportDefinition in ReportDefinitions )
                {
                    reportDefinition.LoadColumnMap<T>(context);
                }
            }
        }

        protected virtual ColumnMap CreateColumnMap(DbDataRecord columnRecord)
        {
            ColumnMap map = new ColumnMap();

            int index;

            index = columnRecord.GetOrdinal("PropertyName");
            if ( !columnRecord.IsDBNull(index) )
                map.PropertyName = columnRecord.GetString(index);

            index = columnRecord.GetOrdinal("TranslatedName");
            if ( !columnRecord.IsDBNull(index) )
                map.TranslatedName = columnRecord.GetString(index);

            index = columnRecord.GetOrdinal("DataFormat");
            if ( !columnRecord.IsDBNull(index) )
                map.DataFormat = columnRecord.GetString(index);

            index = columnRecord.GetOrdinal("Width");
            if ( !columnRecord.IsDBNull(index) )
                map.Width = columnRecord.GetInt32(index);

            return map;
        }
    }
}