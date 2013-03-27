using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.ComponentModel;
using System.Data;

namespace Pinsonault.Data
{    
    /// <summary>
    /// Custom DbDataRecord that is loaded from a data reader.  This allows for ad-hoc queries to be executed against a data source while providing compatibility with web pages and proprietary export code.
    /// </summary>
    public class GenericDataRecord : DbDataRecord, ICustomTypeDescriptor
    {
        /// <summary>
        /// Creates a collection of DbDataRecords generated from a data reader that can be used to bind to web controls and is also compatible with export functionality.
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        public static IEnumerable<GenericDataRecord> CreateCollection(DbDataReader dataReader)
        {
            List<GenericDataRecord> data = new List<GenericDataRecord>();


            Dictionary<string, int> fieldLookup = new Dictionary<string, int>(new Pinsonault.Core.CaseInsensitiveComparer());
            List<PropertyDescriptor> properties = new List<PropertyDescriptor>();
            DataTable table = dataReader.GetSchemaTable();
            DataRow row;

            string name;
            for ( int i = 0; i < dataReader.FieldCount; i++ )
            {
                row = table.Rows[i];

                name = row[0].ToString();
                    
                fieldLookup.Add(name, i);
                
                properties.Add(new GenericDataPropertyDescriptor(name, (Type)row["DataType"], null));
            }

            while ( dataReader.Read() )
            {
                data.Add(GenericDataRecord.Create(dataReader, fieldLookup, new PropertyDescriptorCollection(properties.ToArray(), true)));
            }

            return data;
        }              

        static GenericDataRecord Create(DbDataReader dataReader, Dictionary<string, int> fieldLookup, PropertyDescriptorCollection properties)
        {
            object[] data = new object[dataReader.FieldCount];
            dataReader.GetValues(data);

            return new GenericDataRecord(fieldLookup, data, properties);
        }

        object[] _data = new object[0];
        Dictionary<string, int> _fieldLookup = null;
        PropertyDescriptorCollection _propertyDescriptors = null;

        GenericDataRecord(Dictionary<string, int> fields, object[] data, PropertyDescriptorCollection properties)
        {            
            _data = data;
            _fieldLookup = fields;
            _propertyDescriptors = properties;
        }

        public override int FieldCount
        {
            get { return _data.Length; }
        }

        public override bool GetBoolean(int i)
        {
            throw new NotImplementedException();
        }

        public override byte GetByte(int i)
        {
            throw new NotImplementedException();
        }

        public override long GetBytes(int i, long dataIndex, byte[] buffer, int bufferIndex, int length)
        {
            throw new NotImplementedException();
        }

        public override char GetChar(int i)
        {
            throw new NotImplementedException();
        }

        public override long GetChars(int i, long dataIndex, char[] buffer, int bufferIndex, int length)
        {
            throw new NotImplementedException();
        }

        public override string GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }

        public override DateTime GetDateTime(int i)
        {
            throw new NotImplementedException();
        }

        public override decimal GetDecimal(int i)
        {
            throw new NotImplementedException();
        }

        public override double GetDouble(int i)
        {
            throw new NotImplementedException();
        }

        public override Type GetFieldType(int i)
        {
            return _propertyDescriptors[i].GetType();
        }

        public override float GetFloat(int i)
        {
            throw new NotImplementedException();
        }

        public override Guid GetGuid(int i)
        {
            throw new NotImplementedException();
        }

        public override short GetInt16(int i)
        {
            throw new NotImplementedException();
        }

        public override int GetInt32(int i)
        {
            throw new NotImplementedException();
        }

        public override long GetInt64(int i)
        {
            throw new NotImplementedException();
        }

        public override string GetName(int i)
        {
            return _fieldLookup.FirstOrDefault(f => f.Value == i).Key;
        }

        public override int GetOrdinal(string name)
        {
            return _fieldLookup[name];
        }

        public override string GetString(int i)
        {
            throw new NotImplementedException();
        }

        public override object GetValue(int i)
        {
            return this[i];
        }

        public override int GetValues(object[] values)
        {
            _data.CopyTo(values, 0);
            return _data.Length;
        }

        public override bool IsDBNull(int i)
        {
            return DBNull.Value.Equals(this[i]);
        }

        public override object this[string name]
        {
            get { return _data[_fieldLookup[name]]; }
        }

        public override object this[int i]
        {
            get { return _data[i]; }
        }

        #region ICustomTypeDescriptor Members

        AttributeCollection ICustomTypeDescriptor.GetAttributes()
        {
            return new AttributeCollection();
        }

        string ICustomTypeDescriptor.GetClassName()
        {
            return this.GetType().Name;
        }

        string ICustomTypeDescriptor.GetComponentName()
        {
            return null;
        }

        TypeConverter ICustomTypeDescriptor.GetConverter()
        {
            return null;
        }

        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
        {
            return null;
        }

        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
        {
            return null;
        }

        object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
        {
            return null;
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
        {
            return null;
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            return null;
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        {
            return GetProperties();
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            return GetProperties();
        }

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        #endregion

        private PropertyDescriptorCollection GetProperties()
        {
            return _propertyDescriptors;
        }
    }
}
