using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Pinsonault.Data
{
    /// <summary>
    /// Custom PropertyDescriptor that allows for data binding of GenericDataRecord objects.
    /// </summary>
    public class GenericDataPropertyDescriptor : PropertyDescriptor
    {
        public GenericDataPropertyDescriptor(string Name, Type Type, Attribute[] Attributes)
            : base(Name, Attributes)
        {
            _propertyType = Type;
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override Type ComponentType
        {
            get { return typeof(GenericDataRecord); }
        }

        public override object GetValue(object component)
        {
            return ((GenericDataRecord)component)[Name];
        }

        public override bool IsReadOnly
        {
            get { return true; }
        }

        Type _propertyType = null;
        public override Type PropertyType
        {
            get { return _propertyType; }
        }

        public override void ResetValue(object component)
        {
            throw new NotImplementedException();
        }

        public override void SetValue(object component, object value)
        {
            throw new NotImplementedException();
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
    }
}
