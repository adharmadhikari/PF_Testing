using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;

namespace Pinsonault.Data
{
    public static class SQLUtil
    {
        static int _maxFragmentLength = 30;

        public static int MaxFragmentLength
        {
            get { return _maxFragmentLength; }
            set
            {
                System.Diagnostics.Debug.WriteLine(string.Format("SQLUtil Warning: MaxFragmentLength has been modified from {0} to {1}.", _maxFragmentLength, value));

                _maxFragmentLength = value;
            }
        }

        /// <summary>
        /// Validates a string to be used in a SQL statement verifying it does not contain any special characters and does not exceed a defined max length.  Only letters, numbers, and underscores are permitted.
        /// </summary>
        /// <param name="item">Text to validate</param>
        /// <returns>Validated text unchanged</returns>
        public static string CheckSQLItem(string item)
        {
            return CheckSQLItem(item, @"\W");
        }

        /// <summary>
        /// Validates a string to be used as a table name in a SQL statement verifying it does not contain any special characters and does not exceed a defined max length.  Only letters, numbers, underscores, or a leading # is permitted.
        /// </summary>
        /// <param name="item">Text to validate</param>
        /// <returns>Validated text unchanged</returns>
        public static string CheckSQLTableName(string item)
        {
            return CheckSQLItem(item, @"[#]\W|[^#]\W");
        }

        static string CheckSQLItem(string item, string regExp)
        {
            if ( item.Length > MaxFragmentLength )
                throw new ArgumentException(string.Format("Invalid SQL fragment: Fragments are limited to {0} characters.", MaxFragmentLength));

            //only allow letters, numbers, underscore, or leading "#" for temp tables
            Regex regexp = new Regex(regExp);
            if ( regexp.IsMatch(item) )
                throw new ArgumentException(string.Format("Invalid SQL {0}", item));

            return item;
        }
    }

    public class SQLPivotQuery
    {
        public const string PrimaryTable = "pivotTable0";
    }

    public enum SQLTop
    {
        Undefined,
        Percent,
        Count
    }

    public enum SQLSortDirection
    {
        Ascending,
        Descending
    }

    public class SQLOperator
    {
        public string Text { get; private set; }
        public bool RequiresValue { get; private set; }
        public bool RequiresList { get; private set; }

        public static SQLOperator EqualTo { get { return new SQLOperator { Text = "=", RequiresValue = true }; } }
        public static SQLOperator LessThan { get { return new SQLOperator { Text = "<", RequiresValue = true }; } }
        public static SQLOperator GreaterThan { get { return new SQLOperator { Text = ">", RequiresValue = true }; } }
        public static SQLOperator NotEqual { get { return new SQLOperator { Text = "<>", RequiresValue = true }; } }
        public static SQLOperator LessThanEqual { get { return new SQLOperator { Text = "<=", RequiresValue = true }; } }
        public static SQLOperator GreaterThanEqual { get { return new SQLOperator { Text = ">=", RequiresValue = true }; } }
        public static SQLOperator Like { get { return new SQLOperator { Text = "Like", RequiresValue = true }; } }
        public static SQLOperator NotLike { get { return new SQLOperator { Text = "Not Like", RequiresValue = true }; } }
        public static SQLOperator IsNull { get { return new SQLOperator { Text = "Is Null" }; } }
        public static SQLOperator IsNotNull { get { return new SQLOperator { Text = "Is Not Null" }; } }
        public static SQLOperator In { get { return new SQLOperator { Text = "In", RequiresValue = true, RequiresList = true }; } }
        public static SQLOperator NotIn { get { return new SQLOperator { Text = "Not In", RequiresValue = true, RequiresList = true }; } }
    }

    public enum SQLFunction
    {
        AVG,
        MAX,
        MIN,
        SUM,
        COUNT
    }

    public class SQLRelation
    {
        /// <summary>
        /// Creates a standard Parent/Child relationship between two database columns.
        /// </summary>
        /// <param name="ParentColumnName">Name of the database column on the PrimaryTable.</param>
        /// <param name="ChildColumnName">Name of the database column on the child table.</param>
        public SQLRelation(string ParentColumnName, string ChildColumnName)
        {
            if ( string.IsNullOrEmpty(ParentColumnName) )
                throw new ArgumentNullException("ParentColumnName", "SQLRelation object cannot be created without a ParentColumnName specified.");
            if ( string.IsNullOrEmpty(ChildColumnName) )
                throw new ArgumentNullException("ChildColumnName", "SQLRelation object cannot be created without a ChildColumnName specified.");

            this.ParentColumnName = SQLUtil.CheckSQLItem(ParentColumnName);
            this.ChildColumnName = SQLUtil.CheckSQLItem(ChildColumnName);
        }

        /// <summary>
        /// Creates a standard Parent/Child relationship between two database columns.
        /// </summary>
        /// <param name="ParentTableName">Parent table name.  Value can be a fully qualified table name such as 'mydb.dbo.mytable'.</param>
        /// <param name="ParentColumnName">Name of the database column on the parent table.</param>
        /// <param name="ChildColumnName">Name of the database column on the child table.</param>
        public SQLRelation(string ParentTableName, string ParentColumnName, string ChildColumnName)
        {
            if ( string.IsNullOrEmpty(ParentTableName) )
                throw new ArgumentNullException("ParentTableName", "SQLRelation object cannot be created without a ParentTableName specified.");
            if ( string.IsNullOrEmpty(ParentColumnName) )
                throw new ArgumentNullException("ParentColumnName", "SQLRelation object cannot be created without a ParentColumnName specified.");
            if ( string.IsNullOrEmpty(ChildColumnName) )
                throw new ArgumentNullException("ChildColumnName", "SQLRelation object cannot be created without a ChildColumnName specified.");

            SQLTableName tableName = new SQLTableName(ParentTableName);

            if ( tableName.Database != null )
                this.ParentTableDatabase = SQLUtil.CheckSQLItem(tableName.Database);
            if ( tableName.Schema != null )
                this.ParentTableSchema = SQLUtil.CheckSQLItem(tableName.Schema);
            this.ParentTableName = SQLUtil.CheckSQLTableName(tableName.Name);
            this.ParentColumnName = SQLUtil.CheckSQLItem(ParentColumnName);
            this.ChildColumnName = SQLUtil.CheckSQLItem(ChildColumnName);
        }

        /// <summary>
        /// Creates a standard Parent/Child relationship between two database columns from two different databases.
        /// </summary>
        /// <param name="ParentTableDatabase">Database instance where parent table is located.</param>
        /// <param name="ParentTableSchema">Database schema that the parent table belongs.</param>
        /// <param name="ParentTableName">Parent table name.</param>
        /// <param name="ParentColumnName">Name of the database column on the parent table.</param>
        /// <param name="ChildColumnName">Name of the database column on the child table.</param>
        public SQLRelation(string ParentTableDatabase, string ParentTableSchema, string ParentTableName, string ParentColumnName, string ChildColumnName)
        {
            if ( string.IsNullOrEmpty(ParentTableDatabase) )
                throw new ArgumentNullException("ParentTableDatabase", "SQLRelation object cannot be created without a ParentTableDatabase specified.");
            if ( string.IsNullOrEmpty(ParentTableSchema) )
                throw new ArgumentNullException("ParentTableSchema", "SQLRelation object cannot be created without a ParentTableSchema specified.");
            if ( string.IsNullOrEmpty(ParentTableName) )
                throw new ArgumentNullException("ParentTableName", "SQLRelation object cannot be created without a ParentTableName specified.");
            if ( string.IsNullOrEmpty(ParentColumnName) )
                throw new ArgumentNullException("ParentColumnName", "SQLRelation object cannot be created without a ParentColumnName specified.");
            if ( string.IsNullOrEmpty(ChildColumnName) )
                throw new ArgumentNullException("ChildColumnName", "SQLRelation object cannot be created without a ChildColumnName specified.");

            this.ParentTableDatabase = SQLUtil.CheckSQLItem(ParentTableDatabase);
            this.ParentTableSchema = SQLUtil.CheckSQLItem(ParentTableSchema);
            this.ParentTableName = SQLUtil.CheckSQLTableName(ParentTableName);
            this.ParentColumnName = SQLUtil.CheckSQLItem(ParentColumnName);
            this.ChildColumnName = SQLUtil.CheckSQLItem(ChildColumnName);
        }

        /// <summary>
        /// Creates filter condition within the join operation.  This type of SQLRelation is usually added in addition to standard Parent/Child relations.  It is helpful in adding a "Where" condition to a Left Outer Join when it is not possible to add such a filter to the actual Where clause.
        /// </summary>
        /// <param name="ChildColumnName">Name of the child column</param>
        /// <param name="ParameterName">Name to assign to the parameter</param>
        /// <param name="Operator">Query operator to apply in join condition</param>
        public SQLRelation(string ChildColumnName, string ParameterName, SQLOperator Operator)
        {
            if ( string.IsNullOrEmpty(ChildColumnName) )
                throw new ArgumentNullException("ChildColumnName", "SQLRelation object cannot be created without a ChildColumnName specified.");
            if ( string.IsNullOrEmpty(ParameterName) )
                throw new ArgumentNullException("ParameterName", "SQLRelation object cannot be created without a ParameterName specified.");

            this.ParameterName = SQLUtil.CheckSQLItem(ParameterName);
            this.ChildColumnName = SQLUtil.CheckSQLItem(ChildColumnName);
            this.Operator = Operator;
        }

        public string FullyQualifiedParentTableName
        {
            get
            {
                List<string> list = new List<string>();
                if ( !string.IsNullOrEmpty(ParentTableDatabase) )
                    list.Add(ParentTableDatabase);
                if ( !string.IsNullOrEmpty(ParentTableSchema) )
                    list.Add(ParentTableSchema);
                list.Add(ParentTableName);
                return string.Join(".", list.ToArray());
            }
        }

        public string ParentTableDatabase { get; private set; }
        public string ParentTableSchema { get; private set; }
        public string ParentTableName { get; private set; }
        public string ParentColumnName { get; private set; }
        public string ChildColumnName { get; private set; }
        public string ParameterName { get; private set; }
        public bool ChildIsParameter
        {
            get { return !string.IsNullOrEmpty(ParameterName); }
        }

        SQLOperator _operator = SQLOperator.EqualTo;
        public SQLOperator Operator
        {
            get { return _operator; }
            set
            {
                if ( value.RequiresValue && !value.RequiresList )
                    _operator = value;
                else
                    throw new ArgumentException("SQLRelation Operator property can only be set to an operator that requires a value and is not a list.");
            }
        }
    }

    internal class SQLCondition
    {

        internal SQLCondition(SQLTable Parent, string ColumnName, string ParameterName, SQLOperator Operator, int ArgumentCount)
        {
            if ( string.IsNullOrEmpty(ColumnName) ) throw new ArgumentNullException("ColumnName", "ColumnName cannot be null or empty for SQLCondition.");
            if ( Operator == null ) throw new ArgumentNullException("Operator", "Operator cannot be null for SQLCondition.");
            if ( ArgumentCount < 1 ) throw new ArgumentOutOfRangeException("ArgumentCount must be at least 1.");

            this.Parent = Parent;
            this.ColumnName = SQLUtil.CheckSQLItem(ColumnName);
            this.ParameterName = (!string.IsNullOrEmpty(ParameterName) ? SQLUtil.CheckSQLItem(ParameterName) : this.ColumnName);
            this.Operator = Operator;
            this.ArgumentCount = ArgumentCount;
        }

        SQLTable Parent { get; set; }
        SQLOperator Operator { get; set; }
        int ArgumentCount { get; set; }

        public string ParameterName { get; private set; }
        public string ColumnName { get; private set; }

        public string ConditionAsString
        {
            get
            {
                //string parentName = Parent.IsPrimary ? "pivotTable0" : Parent.Name;
                string parentName = Parent.IsPrimary ? string.Empty : string.Format("{0}.", Parent.Name);

                if ( Operator.RequiresList )
                {
                    StringBuilder list = new StringBuilder();

                    list.AppendFormat("{0}{1} {2}(", parentName, ColumnName, Operator.Text);

                    for ( int i = 0; i < ArgumentCount; i++ )
                    {
                        if ( i > 0 ) list.Append(",");
                        list.AppendFormat("@{0}{1}", ParameterName, i);
                    }
                    list.Append(")");

                    return list.ToString();
                }
                else if ( Operator.RequiresValue )
                {
                    return string.Format("{0}{1} {2} @{3}", parentName, ColumnName, Operator.Text, ParameterName);
                }
                else //should be Is Null or Is Not Null operator
                {
                    return string.Format("{0}{1} {2}", parentName, ColumnName, Operator.Text);
                }
            }
        }
    }

    /// <summary>
    /// Helper class for parsing table names.
    /// </summary>
    internal class SQLTableName
    {
        public string Name { get; set; }
        public string Schema { get; set; }
        public string Database { get; set; }

        public SQLTableName(string Value)
        {
            string[] values = Value.Split('.');
            if ( values.Length > 2 )
            {
                Database = values[0];
                Schema = values[1];
                Name = values[2];
            }
            else if ( values.Length > 1 )
            {
                Schema = values[0];
                Name = values[1];
            }
            else
            {
                Name = Value;
            }
        }

        public bool HasSchema { get { return !string.IsNullOrEmpty(Schema); } }
        public bool HasDatabase { get { return !string.IsNullOrEmpty(Database); } }
    }

    /// <summary>
    /// Defines a database table including the columns that are selected, filter conditions to apply in the Where clause, and key columns (if it is the PrimaryTable of a SQLPivotQuery instance).
    /// </summary>
    public class SQLTable
    {
        /// <summary>
        /// Constructs a basic SQLTable object with no keys or selected fields.  This should be used for child tables.
        /// </summary>
        /// <param name="Name"></param>
        public SQLTable(string Name) : this(Name, null, null, new string[0]) { }

        /// <summary>
        /// Constructs a basic SQLTable object with no keys or selected fields.  This should be used for child tables.
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Database"></param>
        /// <param name="Schema"></param>        
        public SQLTable(string Name, string Database, string Schema) : this(Name, Database, Schema, new string[0]) { }

        /// <summary>
        /// Constructs a basic SQLTable object with specified selected fields.  This should be used for child tables.
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Database"></param>
        /// <param name="Schema"></param>
        /// <param name="SelectFields"></param>
        public SQLTable(string Name, string Database, string Schema, params string[] SelectFields) : this(Name, Database, Schema, new string[0], SelectFields) { }

        /// <summary>
        /// Constructs a SQLTable object and assigns the specified key fields.  This should be used for the primary table.
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Database"></param>
        /// <param name="Schema"></param>        
        /// <param name="Keys"></param>
        internal SQLTable(string Name, string Database, string Schema, IList<string> Keys) : this(Name, Database, Schema, Keys, null) { }


        SQLTable(string Name, string Database, string Schema, IList<string> Keys, params string[] SelectFields)
        {
            if ( Keys == null ) throw new ArgumentNullException("Keys cannot be null.");

            SQLTableName tableName = new SQLTableName(Name);

            this.Name = tableName.Name;

            if ( tableName.HasDatabase )
                this.Database = tableName.Database;
            else
                this.Database = Database;

            if ( tableName.HasSchema )
                this.Schema = tableName.Schema;
            else
                this.Schema = Schema;

            this.Keys = Keys;

            this.SelectFields = new List<string>();
            WhereConditions = new List<SQLCondition>();

            if ( SelectFields != null )
            {
                foreach ( string field in SelectFields )
                {
                    this.AddSelectField(field);
                }
            }
        }

        string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if ( string.IsNullOrEmpty(value) )
                    throw new ArgumentNullException("SQLTable Name property cannot be null or empty.");

                _name = SQLUtil.CheckSQLTableName(value);
            }
        }

        //GD
        string _database;
        public string Database
        {
            get { return _database; }
            set { _database = value != null ? SQLUtil.CheckSQLItem(value) : null; }
        }

        string _schema;
        public string Schema
        {
            get
            {
                if ( string.IsNullOrEmpty(_schema) )
                    return "dbo";

                return _schema;
            }
            set { _schema = value != null ? SQLUtil.CheckSQLItem(value) : null; }
        }

        List<string> SelectFields { get; set; }
        List<SQLCondition> WhereConditions { get; set; }

        public string FullyQualifiedName
        {
            get
            {
                List<string> list = new List<string>();

                if ( !string.IsNullOrEmpty(Database) )
                    list.Add(Database);
                if ( !string.IsNullOrEmpty(Schema) )
                    list.Add(Schema);
                list.Add(Name);

                return string.Join(".", list.ToArray());
            }
        }

        internal bool IsPrimary { get; set; }

        List<string> _keys;

        public IList<string> Keys
        {
            get { return _keys.ToArray(); }
            set
            {
                List<string> newList = new List<string>();
                foreach ( string key in value )
                {
                    newList.Add(SQLUtil.CheckSQLItem(key));
                }

                //newList is ok - no exceptions from CheckSQLItem so use as new list
                _keys = newList;
            }
        }

        public bool HasKeys { get { return _keys != null && _keys.Count > 0; } }

        public string KeysAsString
        {
            get
            {
                if ( _keys.Count > 0 )
                {
                    return string.Format("{0}.{1}", Name, string.Join(string.Format(", {0}.", Name), _keys.ToArray()));
                }

                return string.Empty;
            }
        }

        public string SelectFieldsAsString
        {
            get
            {
                if ( SelectFields.Count > 0 )
                {
                    if ( (!string.IsNullOrEmpty(Database)) && (!string.IsNullOrEmpty(Schema)) )
                        return string.Format("{0}.{1}.{2}.{3}", Database, Schema, Name, string.Join(string.Format(", {0}.{1}.{2}.", Database, Schema, Name), SelectFields.ToArray()));
                    else
                        return string.Format("{0}.{1}", Name, string.Join(string.Format(", {0}.", Name), SelectFields.ToArray()));
                }
                else
                    return string.Format("{0}.*", Name);
            }
        }

        public bool HasSelectFields { get { return SelectFields.Count > 0; } }

        public bool HasWhereConditions { get { return WhereConditions.Count > 0; } }

        public string WhereConditionsAsString
        {
            get
            {
                if ( WhereConditions.Count > 0 )
                {
                    StringBuilder where = new StringBuilder();
                    for ( int i = 0; i < WhereConditions.Count; i++ )
                    {
                        where.AppendFormat("{0}{1}", (i > 0 ? " and " : string.Empty), WhereConditions[i].ConditionAsString);
                    }

                    return where.ToString();
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Add a field to be included in the Select list.
        /// </summary>
        /// <param name="Name">Name of the database column to select.  Table name or alias prefix is not required.</param>
        /// <returns></returns>
        public SQLTable AddSelectField(string Name)
        {
            if ( string.IsNullOrEmpty(Name) )
                throw new ArgumentNullException("Name", "AddSelectField expects a value for Name parameter.");

            SelectFields.Add(SQLUtil.CheckSQLItem(Name));

            return this;
        }

        /// <summary>
        /// Adds an optional Where condition to the PrimaryTable.  *Where conditions are appended to the end of the generated query in the form of SQL parameters.
        /// </summary>
        /// <param name="ColumnName">Name of the database column that is being filtered.</param>
        /// <param name="Operator">SQL operator to apply to the filter.</param>
        /// <returns>Updated SQLTable instance</returns>
        public SQLTable AddWhereCondition(string ColumnName, SQLOperator Operator)
        {
            return AddWhereCondition(ColumnName, Operator, 1);
        }

        /// <summary>
        /// Adds an optional Where condition to the PrimaryTable.  *Where conditions are appended to the end of the generated query in the form of SQL parameters.
        /// </summary>
        /// <param name="ColumnName">Name of the database column that is being filtered.</param>
        /// <param name="Operator">SQL operator to apply to the filter.</param>
        /// <param name="ArgumentCount">Specifies the number of arguments required to construct the filter.  *Only applies to "In" and "Not in" operators</param>
        /// <returns>Updated SQLTable instance</returns>
        public SQLTable AddWhereCondition(string ColumnName, SQLOperator Operator, int ArgumentCount)
        {
            return AddWhereCondition(ColumnName, null, Operator, ArgumentCount);
        }

        /// <summary>
        /// Adds an optional Where condition to the PrimaryTable.  *Where conditions are appended to the end of the generated query in the form of SQL parameters.
        /// </summary>
        /// <param name="ColumnName">Name of the database column that is being filtered.</param>
        /// <param name="ParameterName">Optional parameter name.  If no value is specified the ColumnName is used in created the final parameter name. *Not setting this value could result in conflicting parameter names.</param>
        /// <param name="Operator">SQL operator to apply to the filter.</param>
        /// <returns>Updated SQLTable instance</returns>
        public SQLTable AddWhereCondition(string ColumnName, string ParameterName, SQLOperator Operator)
        {
            WhereConditions.Add(new SQLCondition(this, ColumnName, ParameterName, Operator, 1));

            return this;
        }

        /// <summary>
        /// Adds an optional Where condition to the PrimaryTable.  *Where conditions are appended to the end of the generated query in the form of SQL parameters.
        /// </summary>
        /// <param name="ColumnName">Name of the database column that is being filtered.</param>
        /// <param name="ParameterName">Optional parameter name.  If no value is specified the ColumnName is used in created the final parameter name. *Not setting this value could result in conflicting parameter names.</param>
        /// <param name="Operator">SQL operator to apply to the filter.</param>
        /// <param name="ArgumentCount">Specifies the number of arguments required to construct the filter.  *Only applies to "In" and "Not in" operators</param>
        /// <returns>Updated SQLTable instance</returns>
        public SQLTable AddWhereCondition(string ColumnName, string ParameterName, SQLOperator Operator, int ArgumentCount)
        {
            WhereConditions.Add(new SQLCondition(this, ColumnName, ParameterName, Operator, ArgumentCount));

            return this;
        }
    }

    /// <summary>
    /// Defines a relation between the PrimaryTable and a child table.
    /// </summary>
    public abstract class SQLJoin
    {
        public SQLJoin(SQLTable table)
        {
            if ( table == null )
                throw new ArgumentNullException("table", "SQLJoin object cannot be created if table parameter is null.");

            Table = table;
            Relations = new List<SQLRelation>();
        }

        public abstract string JoinText { get; }

        public SQLTable Table { get; private set; }
        List<SQLRelation> Relations { get; set; }

        /// <summary>
        /// Adds an SQLRelation which defines a foreign key relationship between two columns.
        /// </summary>
        /// <param name="Relation"></param>
        /// <returns>Updated SQLJoin instance</returns>
        public SQLJoin AddRelation(SQLRelation Relation)
        {
            if ( Relation == null )
                throw new ArgumentNullException("Relation", "AddRelation expects a value for Relation parameter.");

            Relations.Add(Relation);

            return this;
        }

        public string JoinAsString
        {
            get
            {
                //inner join tablename on tablename.childfield = parenttable.parentfield and ...
                StringBuilder join = new StringBuilder();

                if ( Relations.Count > 0 )
                {
                    string and = "";

                    join.AppendLine();

                    if ( string.IsNullOrEmpty(Table.Database) )
                        join.AppendFormat(" {0} Join ", JoinText).Append(string.Format("{0}.{1}", Table.Schema, Table.Name)).Append(" on ");
                    else
                        join.AppendFormat(" {0} Join ", JoinText).Append(string.Format("{0}.{1}.{2}", Table.Database, Table.Schema, Table.Name)).Append(" on ");

                    foreach ( SQLRelation relation in Relations )
                    {
                        //ASSUMING PARENT IS "pivotTable0" - NEED TO DETERMINE IF PARENT TABLE IS REQUIRED - CURRENTLY JOINING AT OUTER MOST LEVEL SO MAY NOT BE NEEDED
                        if ( !relation.ChildIsParameter )
                            if ( (string.IsNullOrEmpty(relation.ParentTableName)) && (string.IsNullOrEmpty(Table.Name)) )
                                join.AppendFormat("{0}{1} {2} {3}.{4}", and, relation.ChildColumnName, relation.Operator.Text, SQLPivotQuery.PrimaryTable, relation.ParentColumnName);
                            else if ( string.IsNullOrEmpty(relation.ParentTableName) )
                                join.AppendFormat("{0}{1}.{2}.{3}.{4} {5} {6}.{7}", and, Table.Database, Table.Schema, Table.Name, relation.ChildColumnName, relation.Operator.Text, SQLPivotQuery.PrimaryTable, relation.ParentColumnName);
                            else
                                join.AppendFormat("{0}{1}.{2}.{3}.{4} {5} {6}.{7}", and, Table.Database, Table.Schema, Table.Name, relation.ChildColumnName, relation.Operator.Text, relation.FullyQualifiedParentTableName, relation.ParentColumnName);
                        else
                            join.AppendFormat("{0}{1}.{2} {3} @{4}", and, Table.Name, relation.ChildColumnName, relation.Operator.Text, relation.ParameterName);
                        and = " and ";
                    }
                    join.AppendLine();
                }

                return join.ToString();
            }
        }
    }

    public sealed class SQLInnerJoin : SQLJoin
    {
        public SQLInnerJoin(SQLTable Table) : base(Table) { }

        public override string JoinText { get { return "Inner"; } }
    }

    public sealed class SQLLeftOuterJoin : SQLJoin
    {
        public SQLLeftOuterJoin(SQLTable Table) : base(Table) { }

        public override string JoinText { get { return "Left Outer"; } }
    }

    public sealed class SQLRightOuterJoin : SQLJoin
    {
        public SQLRightOuterJoin(SQLTable Table) : base(Table) { }

        public override string JoinText { get { return "Right Outer"; } }
    }

    public class SQLPivot
    {
        public SQLPivot(SQLFunction Function, string ColumnName)
        {
            if ( string.IsNullOrEmpty(ColumnName) )
                throw new ArgumentNullException("ColumnName", "ColumnName not specified when constructing SQLPivot.");

            this.Function = Function;
            this.ColumnName = SQLUtil.CheckSQLItem(ColumnName);
        }

        public SQLFunction Function { get; set; }
        public string ColumnName { get; private set; }

    }

    /// <summary>
    /// Generates dynamic SQL for PIVOT queries that are executed against SQL Server.
    /// </summary>
    /// <typeparam name="T">Data type of pivot values.</typeparam>
    public class SQLPivotQuery<T>
    {
        List<SQLPivot> Pivots { get; set; }


        public SQLTop Top { get; set; }

        public int TopValue { get; set; }


        /// <summary>
        /// Creates an instance of SQLPivotQuery initializing the PrimaryTable and Pivot condition.
        /// </summary>
        /// <param name="PrimaryTableName">Name of the database table that is pivoted.</param>
        /// <param name="Keys">List of columns that define a unique record (does not have to be primary keys).</param>
        /// <param name="PivotWhere">Column that is used in pivot condition to restrict results and whose values will become the columns for pivoted fields.</param>
        /// <param name="PivotValues">Values that have been selected for restricting results and will become columns for pivoted fields.</param>
        /// <returns>Initialized SQLPivotQuery</returns>
        public static SQLPivotQuery<T> Create(string PrimaryTableName, string Database, string Schema, IList<string> Keys, string PivotWhere, IList<T> PivotValues)
        {
            return new SQLPivotQuery<T>(PrimaryTableName, Database, Schema, Keys, PivotWhere, PivotValues);
        }

        /// <summary>
        /// Creates an instance of SQLPivotQuery initializing the PrimaryTable and Pivot condition.
        /// </summary>
        /// <param name="PrimaryTableName">Name of the database table that is pivoted.</param>
        /// <param name="Keys">List of columns that define a unique record (does not have to be primary keys).</param>
        /// <param name="PivotWhere">Column that is used in pivot condition to restrict results and whose values will become the columns for aggregated fields.</param>
        /// <param name="PivotValues">Values that have been selected for restricting results and will become columns for aggregated fields.</param>
        /// <returns>Initialized SQLPivotQuery</returns>
        public SQLPivotQuery(string PrimaryTableName, string Database, string Schema, IList<string> Keys, string PivotWhere, IList<T> PivotValues)
            : this(new SQLTable(PrimaryTableName, Database, Schema, Keys), PivotWhere, PivotValues)
        {
            if ( Keys == null || Keys.Count == 0 ) throw new ArgumentException("Keys must be specified for the primary table.");
        }

        /// <summary>
        /// Creates an instance of SQLPivotQuery initializing the PrimaryTable and Pivot condition.
        /// </summary>
        /// <param name="Table">SQLTable that will serve as the primary table..</param>
        /// <param name="Keys">List of columns that define a unique record (does not have to be primary keys).</param>
        /// <param name="PivotWhere">Column that is used in pivot condition to restrict results and whose values will become the columns for pivoted fields.</param>
        /// <param name="PivotValues">Values that have been selected for restricting results and will become columns for pivoted fields.</param>
        /// <returns>Initialized SQLPivotQuery</returns>
        public SQLPivotQuery(SQLTable Table, string PivotWhere, IList<T> PivotValues)
        {
            if ( Table == null ) throw new ArgumentNullException("A primary table must be defined to create an SQLPivotQuery instance.");
            if ( !Table.HasKeys ) throw new ArgumentException("Primary table must have keys defined.");

            PrimaryTable = Table;
            PrimaryTable.IsPrimary = true;
            ChildTables = new List<SQLJoin>();
            Pivots = new List<SQLPivot>();
            OrderByFields = new List<string>();

            this.PivotValues = PivotValues;
            this.PivotWhere = SQLUtil.CheckSQLItem(PivotWhere);
        }

        /// <summary>
        /// Base database table used in constructing pivot query.  This table contains the columns that are pivoted.
        /// </summary>
        public SQLTable PrimaryTable { get; private set; }

        /// <summary>
        /// Additional database tables that are joined to the PrimaryTable to retreive related data not found in PrimaryTable.
        /// </summary>
        public List<SQLJoin> ChildTables { get; private set; }



        bool _calculateAverage = false;
        public bool CalculateAverage
        {
            get { return _calculateAverage; }
            set
            {
                if ( typeof(T).Equals(typeof(string)) )
                    throw new InvalidOperationException("CalculateAverage cannot be set if Pivot type is String");

                _calculateAverage = value;
            }
        }

        bool _calculateSum = false;
        public bool CalculateSum
        {
            get { return _calculateSum; }
            set
            {
                if ( typeof(T).Equals(typeof(string)) )
                    throw new InvalidOperationException("CalculateSum cannot be set if Pivot type is String");

                _calculateSum = value;
            }
        }

        bool _calculatePercent = false;
        public bool CalculatePercent
        {
            get { return _calculatePercent; }
            set
            {
                if ( typeof(T).Equals(typeof(string)) )
                    throw new InvalidOperationException("CalculatePercent cannot be set if Pivot type is String");
                _calculatePercent = value;
            }
        }

        //for comparing two columns
        bool _comparepivotcolumns = false;
        public bool ComparePivotColumns
        {
            get { return _comparepivotcolumns; }
            set
            {           
                _comparepivotcolumns = value;
            }
        }

        //for converting null values to empty string
        bool _convertNullValueToEmptyString = false;
        public bool ConvertNullValueToEmptyString
        {
            get { return _convertNullValueToEmptyString; }
            set
            {
                _convertNullValueToEmptyString = value;
            }
        }

        /// <summary>
        /// Values that are pivoted
        /// </summary>
        public IList<T> PivotValues { get; set; }

        /// <summary>
        /// Name of the column that is pivoted
        /// </summary>
        public string PivotWhere { get; private set; }

        IList<string> OrderByFields { get; set; }

        /// <summary>
        /// Returns a child table from the specified name.  If none exists null is returned.
        /// </summary>
        /// <param name="Name">Name of the child table.</param>
        /// <returns></returns>
        public SQLTable GetChildTableByName(string Name)
        {
            SQLJoin join = ChildTables.Where(c => string.Compare(c.Table.Name, Name, true) == 0).FirstOrDefault();
            if ( join != null )
                return join.Table;

            return null;
        }

        string pivotValuesToString(bool boxed, bool named, string PivotColumnName, bool inClause)
        {

            StringBuilder sb = new StringBuilder();
            bool isString = typeof(T).Equals(typeof(string));
            string start = boxed ? "[" : (isString ? "'" : string.Empty);
            string end = boxed ? "]" : (isString ? "'" : string.Empty);
            string name = string.Empty;

            //string alias = string.IsNullOrEmpty(tableName) ? string.Empty : string.Format("{0}.", tableName);

            T value;
            for ( int i = 0; i < PivotValues.Count; i++ )
            {
                if ( i > 0 )
                    sb.Append(", ");

                if ( named )
                    name = string.Format(" as {0}{1}", PivotColumnName, i);

                value = PivotValues[i];
                if ( isString )
                    SQLUtil.CheckSQLItem(value.ToString());

                if (boxed && !isString && !inClause && !ConvertNullValueToEmptyString)
                    sb.AppendFormat("ISNULL({0}{1}{2}, 0){3}", start, value, end, name);
                else if (boxed && !isString && !inClause && ConvertNullValueToEmptyString)
                    sb.AppendFormat("ISNULL({0}{1}{2}, ''){3}", start, value, end, name);
                else
                    sb.AppendFormat("{0}{1}{2}{3}", start, value, end, name);
            }

            if ( boxed && named && !isString )
            {
                string total = string.Join("+", PivotValues.Select(v => string.Format("ISNULL([{0}],0)", v)).ToArray());
                
                if ( CalculateSum )
                    sb.AppendFormat(",({0}) as {1}_Sum", total, PivotColumnName);
                if ( CalculateAverage )
                    sb.AppendFormat(",({0})/{1} as {2}_Average", total, PivotValues.Count, PivotColumnName);
                if ( CalculatePercent )
                {
                    List<string> list = PivotValues.Select(v => string.Format("[{0}]/Convert(float,({1}))", v, total)).ToList();
                    for ( int i = 0; i < list.Count; i++ )
                    {
                        sb.AppendFormat(",{0} as {1}{2}_Percent", list[i], PivotColumnName, i);
                    }
                }
                //to compare pivot columns
                if (ComparePivotColumns)
                {
                    // if required do a for loop to compare multiple values                   
                    sb.AppendFormat(",case when ISNULL([{0}],'') != ISNULL([{1}],'') then 1 else 0 End As Is_{2}_Changed", PivotValues[0], PivotValues[1], PivotColumnName);                    
                }
            }

            return sb.ToString();
        }

        public string PivotValuesAsColumnStringInClause
        {
            get { return pivotValuesToString(true, false, null, true); }
        }

        public string PivotValuesAsColumnString
        {
            get { return pivotValuesToString(true, false, null, false); }
        }

        public string PivotValuesAsNamedColumnString(string PivotColumnName)
        {
            return pivotValuesToString(true, true, PivotColumnName, false);
        }

        public string PivotValuesAsString
        {
            get { return pivotValuesToString(false, false, null, false); }
        }


        /// <summary>
        /// Adds an SQLJoin to the query in order to return data related to the PrimaryTable
        /// </summary>
        /// <param name="TableName">Name of the child table.</param>
        /// <param name="ParentColumnName">Name of the column in the PrimaryTable that forms the relation with the child table.</param>
        /// <param name="ChildColumnName">Name of the column in the child table that forms the relation with the parent table.</param>
        /// <param name="SelectFields">List of fields that should be selected from the child table.</param>
        /// <returns>Updated SQLPivotQuery instance</returns>
        public SQLPivotQuery<T> InnerJoin(string TableName, string ParentTableName, string ParentColumnName, string ChildColumnName, params string[] SelectFields)
        {
            SQLTable table = buildTable(TableName, SelectFields);

            Join(new SQLInnerJoin(table).AddRelation(new SQLRelation(ParentTableName, ParentColumnName, ChildColumnName)));

            return this;
        }

        /// <summary>
        /// Adds an SQLJoin to the query in order to return data related to the PrimaryTable
        /// </summary>
        /// <param name="TableName">Name of the child table.  Value can be fully qualified with database name and schema such as 'mydb.dbo.mytable'.</param>
        /// <param name="ParentColumnName">Name of the column in the PrimaryTable that forms the relation with the child table.</param>
        /// <param name="ChildColumnName">Name of the column in the child table that forms the relation with the parent table.</param>
        /// <param name="SelectFields">List of fields that should be selected from the child table.</param>
        /// <returns>Updated SQLPivotQuery instance</returns>
        public SQLPivotQuery<T> LeftOuterJoin(string TableName, string ParentTableName, string ParentColumnName, string ChildColumnName, params string[] SelectFields)
        {
            SQLTable table = buildTable(TableName, SelectFields);

            Join(new SQLLeftOuterJoin(table).AddRelation(new SQLRelation(ParentTableName, ParentColumnName, ChildColumnName)));

            return this;
        }

        SQLTable buildTable(string TableName, IList<string> SelectFields)
        {
            SQLTable table = new SQLTable(TableName);
            foreach ( string field in SelectFields )
            {
                table.AddSelectField(field);
            }

            return table;
        }

        /// <summary>
        /// Adds an SQLJoin to the query in order to return data related to the PrimaryTable (use this version of AddChildTable if JOIN has more than one foreign key relation between Parent and Child)
        /// </summary>
        /// <param name="Table"></param>
        /// <returns>Updated SQLPivotQuery instance</returns>
        public SQLPivotQuery<T> Join(SQLJoin Table)
        {
            this.ChildTables.Add(Table);

            return this;
        }

        /// <summary>
        /// Adds a SQLPivot to the query which represents an aggregated value.
        /// </summary>
        /// <param name="Function">Aggregate function to apply to the database column.</param>
        /// <param name="ColumnName">Name of the column that is aggregated.</param>
        /// <returns>Updated SQLPivotQuery instance</returns>
        public SQLPivotQuery<T> Pivot(SQLFunction Function, string ColumnName)
        {
            Pivots.Add(new SQLPivot(Function, ColumnName));
            return this;
        }

        /// <summary>
        /// Adds a SQLPivot to the query which represents an aggregated value.
        /// </summary>
        /// <param name="Pivot">SQLPivot instance</param>
        /// <returns>Updated SQLPivotQuery instance</returns>
        public SQLPivotQuery<T> Pivot(SQLPivot Pivot)
        {
            if ( Pivot == null ) throw new ArgumentNullException("Pivot cannot be null.");

            this.Pivots.Add(Pivot);

            return this;
        }

        /// <summary>
        /// Adds an Order By clause to the end of the query.
        /// </summary>
        /// <param name="FieldName">Field in select statement that is sorted.</param>
        /// <param name="Direction">Specifies whether the sort direction is Ascending or Descending.</param>
        /// <returns></returns>
        public SQLPivotQuery<T> OrderBy(string FieldName, SQLSortDirection Direction)
        {
            if ( string.IsNullOrEmpty(FieldName) )
                throw new ArgumentNullException("FieldName", "OrderBy expects parameter FieldName.");

            OrderByFields.Add(string.Format("{0}{1}", SQLUtil.CheckSQLItem(FieldName), (Direction != SQLSortDirection.Descending ? " ASC" : " DESC")));

            return this;
        }

        /// <summary>
        /// Adds an optional Where condition to the PrimaryTable.  *Where conditions are appended to the end of the generated query in the form of SQL parameters.
        /// </summary>
        /// <param name="ColumnName">Name of the database column that is being filtered.</param>
        /// <param name="ParameterName">Optional parameter name.  If no value is specified the ColumnName is used in created the final parameter name. *Not setting this value could result in conflicting parameter names.</param>
        /// <param name="Operator">SQL operator to apply to the filter.</param>
        /// <returns>Updated SQLPivotQuery instance</returns>
        public SQLPivotQuery<T> Where(string ColumnName, string ParameterName, SQLOperator Operator)
        {
            PrimaryTable.AddWhereCondition(ColumnName, ParameterName, Operator, 1);

            return this;
        }

        /// <summary>
        /// Adds an optional Where condition to the PrimaryTable.  *Where conditions are appended to the end of the generated query in the form of SQL parameters.
        /// </summary>
        /// <param name="ColumnName">Name of the database column that is being filtered.</param>
        /// <param name="ParameterName">Optional parameter name.  If no value is specified the ColumnName is used in created the final parameter name. *Not setting this value could result in conflicting parameter names.</param>
        /// <param name="Operator">SQL operator to apply to the filter.</param>
        /// <param name="ArgumentCount">Specifies the number of arguments required to construct the filter.  *Only applies to "In" and "Not in" operators</param>
        /// <returns>Updated SQLPivotQuery instance</returns>
        public SQLPivotQuery<T> Where(string ColumnName, string ParameterName, SQLOperator Operator, int ArgumentCount)
        {
            PrimaryTable.AddWhereCondition(ColumnName, ParameterName, Operator, ArgumentCount);

            return this;
        }

        /// <summary>
        /// Adds an optional Where condition to the specified child table.  *Where conditions are appended to the end of the generated query in the form of SQL parameters.
        /// </summary>
        /// <param name="TableName">Name of the child table that is being filtered.  The name of the child table must already be added to the current instance of SQLPivotQuery.</param>
        /// <param name="ColumnName">Name of the database column that is being filtered.</param>
        /// <param name="ParameterName">Optional parameter name.  If no value is specified the ColumnName is used in created the final parameter name. *Not setting this value could result in conflicting parameter names.</param>
        /// <param name="Operator">SQL operator to apply to the filter.</param>
        /// <returns>Updated SQLPivotQuery instance</returns>
        public SQLPivotQuery<T> Where(string TableName, string ColumnName, string ParameterName, SQLOperator Operator)
        {
            SQLTable table = GetChildTableByName(TableName);
            if ( table != null )
                table.AddWhereCondition(ColumnName, ParameterName, Operator, 1);
            else
                throw new ArgumentException("TableName was not set to a name of an existing child table.");

            return this;
        }

        /// <summary>
        /// Adds an optional Where condition to the specified child table.  *Where conditions are appended to the end of the generated query in the form of SQL parameters.
        /// </summary>
        /// /// <param name="TableName">Name of the child table that is being filtered.  The name of the child table must already be added to the current instance of SQLPivotQuery.</param>
        /// <param name="ColumnName">Name of the database column that is being filtered.</param>
        /// <param name="ParameterName">Optional parameter name.  If no value is specified the ColumnName is used in created the final parameter name. *Not setting this value could result in conflicting parameter names.</param>
        /// <param name="Operator">SQL operator to apply to the filter.</param>
        /// <param name="ArgumentCount">Specifies the number of arguments required to construct the filter.  *Only applies to "In" and "Not in" operators</param>
        /// <returns>Updated SQLPivotQuery instance</returns>
        public SQLPivotQuery<T> Where(string TableName, string ColumnName, string ParameterName, SQLOperator Operator, int ArgumentCount)
        {
            SQLTable table = GetChildTableByName(TableName);
            if ( table != null )
                table.AddWhereCondition(ColumnName, ParameterName, Operator, ArgumentCount);
            else
                throw new ArgumentException("TableName was not set to a name of an existing child table.");

            return this;
        }

        /// <summary>
        /// Adds a TOP n clause to the query so it returns a specified number of records.
        /// </summary>
        /// <param name="Value">Number of records to return.</param>
        /// <returns>Updated SQLPivotQuery instance</returns>
        public SQLPivotQuery<T> TakeTop(int Value)
        {
            if ( Value < 1 ) throw new ArgumentOutOfRangeException("Value", "TakeTop expects a value greater than 0.");

            this.Top = SQLTop.Count;
            this.TopValue = Value;

            return this;
        }

        /// <summary>
        /// Adds a TOP n PERCENT clause to the query so it returns a specified number of records.
        /// </summary>
        /// <param name="Value">Percentage of records to return.</param>
        /// <returns>Updated SQLPivotQuery instance</returns>
        public SQLPivotQuery<T> TakeTopPercent(int Value)
        {
            if ( Value < 1 || Value > 100 ) throw new ArgumentOutOfRangeException("Value", "TakeTopPercent expects a value between 1 and 100 is expected.");

            this.Top = SQLTop.Percent;
            this.TopValue = Value;

            return this;
        }

        /// <summary>
        /// Clears the TOP clause so it is not added to the query.  
        /// </summary>
        /// <returns>Updated SQLPivotQuery instance</returns>
        public SQLPivotQuery<T> TakeAll()
        {
            this.Top = SQLTop.Undefined;
            this.TopValue = 0;

            return this;
        }

        public SQLPivotQuery<T> IncludeSum()
        {
            CalculateSum = true;
            return this;
        }

        public SQLPivotQuery<T> IncludeAverage()
        {
            CalculateAverage = true;
            return this;
        }

        public SQLPivotQuery<T> IncludePercent()
        {
            CalculatePercent = true;
            return this;
        }
        //to include comparison of pivot columns
        public SQLPivotQuery<T> IncludeComparison()
        {
            ComparePivotColumns = true;
            return this;
        }
        //for converting null values of Pivot columns to Empty string
        public SQLPivotQuery<T> ConvertNullToEmptyString()
        {
            ConvertNullValueToEmptyString = true;
            return this;
        }

        //Returns the dynamically generated query.
        public override string ToString()
        {
            return generateSQL();
        }

        string generateSQL()
        {

            if ( PrimaryTable == null ) throw new ArgumentException("PrimaryTable has not been set.");
            if ( string.IsNullOrEmpty(PrimaryTable.KeysAsString) ) throw new ArgumentException("PrimaryTable's Keys property has not been set.");
            if ( string.IsNullOrEmpty(PrimaryTable.Name) ) throw new ArgumentException("PrimaryTable's Name has not been set.");
            if ( Pivots.Count == 0 ) throw new ArgumentException("Failed to generate SQL.  No SQLPivot objects have been added.");
            if ( PivotValues == null || PivotValues.Count == 0 ) throw new ArgumentException("Failed to generate SQL. No values have been specified to pivot on.");
            if ( string.IsNullOrEmpty(PivotWhere) ) throw new ArgumentException("PivotWhere has not be set.");

            string keys = string.Join(", ", PrimaryTable.Keys.ToArray());
            string pivotValues = PivotValuesAsString;
            string pivotColumnValues = PivotValuesAsColumnString;


            StringBuilder query = new StringBuilder("select ");
            StringBuilder additionalSelect = new StringBuilder();

            //select
            if ( Top != SQLTop.Undefined )
            {
                query.AppendFormat("top {0}{1} ", TopValue, (Top == SQLTop.Percent ? " percent" : string.Empty));
            }

            query.Append(string.Format("{0}.{1}", SQLPivotQuery.PrimaryTable, string.Join(string.Format(", {0}.", SQLPivotQuery.PrimaryTable), PrimaryTable.Keys.ToArray())));

            query.Append(", ").Append(PivotValuesAsNamedColumnString(Pivots[0].ColumnName));
            for ( int i = 1; i < Pivots.Count; i++ )
            {
                for ( int j = 0; j < PivotValues.Count; j++ )
                    query.Append(",").Append(string.Format("{0}{1}", Pivots[i].ColumnName, j));
            }

            if ( ChildTables.Count > 0 )
            {
                foreach ( SQLJoin join in ChildTables )
                    query.Append(", ").Append(join.Table.SelectFieldsAsString);
            }

            query.Append(" from ");

            //pivots      
            SQLPivot pivot;
            for ( int i = 0; i < Pivots.Count; i++ )
            {
                pivot = Pivots[i];

                query.AppendLine();

                if ( i > 0 )
                {
                    query.AppendLine(" inner join ");
                    query.AppendFormat("(select {0}, {1} from ", keys, PivotValuesAsNamedColumnString(pivot.ColumnName));
                }

                query.Append("(select ");
                query.Append(PrimaryTable.KeysAsString);
                query.Append(",").Append(PivotWhere);
                
                //if pivot column is bool then first convert it in Tinyint
                //if (pivot.ColumnName == "PA" || pivot.ColumnName == "QL" || pivot.ColumnName == "ST")
                //    query.AppendFormat(",CAST({0} AS TINYINT) AS {0} ", pivot.ColumnName);
                //else
                    query.Append(",").Append(pivot.ColumnName);

                query.AppendFormat(" from {0} where {1} in ({2})", PrimaryTable.FullyQualifiedName, PivotWhere, pivotValues);


                if ( PrimaryTable.HasWhereConditions )
                    query.AppendFormat(" and {0}", PrimaryTable.WhereConditionsAsString);

                query.Append(") as sourceTable ");

                query.AppendLine();
                query.AppendLine("pivot(");
                query.AppendFormat("{0}({1})\nfor {2} in ({3})", pivot.Function, pivot.ColumnName, PivotWhere, PivotValuesAsColumnStringInClause);
                query.AppendFormat("\n) as pivotTable{0}", i);

                if ( i > 0 )
                {
                    query.AppendFormat(") as pivotTable{0} on ", i);
                    string and = string.Empty;
                    foreach ( string key in PrimaryTable.Keys.ToArray() )
                    {
                        query.AppendFormat("{0}{1}.{2} = pivotTable{3}.{2}", and, SQLPivotQuery.PrimaryTable, key, i);
                        and = " and ";
                    }
                }
            }

            //joins
            foreach ( SQLJoin table in ChildTables )
            {
                query.Append(table.JoinAsString);
            }


            //where
            StringBuilder where = new StringBuilder();
            //if ( PrimaryTable.HasWhereConditions )
            //{
            //    where.Append(PrimaryTable.WhereConditionsAsString);
            //}
            foreach ( SQLJoin join in ChildTables )
            {
                if ( join.Table.HasWhereConditions )
                {
                    if ( where.Length > 0 )
                        where.Append(" and ");
                    where.Append(join.Table.WhereConditionsAsString);
                }
            }
            //

            if ( where.Length > 0 )
            {
                query.Append(" Where ").Append(where.ToString());
            }

            //order by
            if ( OrderByFields.Count > 0 )
            {
                int count = OrderByFields.Count;

                query.AppendLine();
                for ( int i = 0; i < count; i++ )
                {
                    if ( i == 0 )
                        query.Append(" Order By ");
                    else
                        query.Append(", ");

                    query.Append(OrderByFields[i]);
                }
            }

            return query.ToString();
        }


    }
}
