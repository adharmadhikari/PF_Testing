using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web.UI;
using System.Reflection;
using System.ComponentModel;

namespace Pinsonault.Web.UI
{
    /// <summary>
    /// Customized CheckboxList control
    /// </summary>
    public class CheckboxValueList : DataBoundControl
    {
        public CheckboxValueList()
        {
            TrueValue = "true";
        }

        public string DataTextField { get; set; }
        public string DataValueField { get; set; }
        public string DataValueFormatString { get; set; }

        public string ItemCssClass { get; set; }

        public string SelectedValue { get; set; }

        /// <summary>
        /// Indicates if the value of the field is boolean and not an identifier.  If True DataValueField still needs to be set to set the control's id.  The default is False.
        /// </summary>
        public bool TrueFalse { get; set; }

        /// <summary>
        /// The value of a checkbox when selected if TrueFalse is set to True.  The default is "true".
        /// </summary>
        public string TrueValue { get; set; }

        /// <summary>
        /// If true the name attribute of each checkbox is assigned the value of the control's ID (unmangled).  If false the name attribute is assigned the value of the control's UniqueID property plus the value of the input control.
        /// </summary>
        public bool GroupValues { get; set; }

        /// <summary>
        /// Indicates how many checkboxes are output before a line break is inserted.  If not specified no line breaks are added.
        /// </summary>
        public int BreakCount { get; set; }

        /// <summary>
        /// Determines if a checkbox list will include an option for "All".
        /// </summary>
        public bool HasAllOption { get; set; }

        IEnumerable _data = null;

        protected override void PerformDataBinding(IEnumerable data)
        {
            _data = data;
        }

        protected override HtmlTextWriterTag TagKey
        {
            get { return HtmlTextWriterTag.Div; }
        }

        void renderAllOption(HtmlTextWriter writer)
        {
            string id = string.Format("{0}_0", this.ClientID);
            writer.AddAttribute(HtmlTextWriterAttribute.Id, id);
            writer.AddAttribute(HtmlTextWriterAttribute.Name, GroupValues ? ID : string.Format("{0}$0", this.UniqueID) );
            writer.AddAttribute(HtmlTextWriterAttribute.Value, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, string.Format("notfilter chkItem {0}",ItemCssClass));
            if ( string.IsNullOrEmpty(SelectedValue) )
                writer.AddAttribute(HtmlTextWriterAttribute.Checked, "true");
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "checkbox");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.For, id);
            writer.RenderBeginTag(HtmlTextWriterTag.Label);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "labelAll");
            //writer.Write(Resources.Resource.Label_All);
            writer.Write("All"); //currently no acces to a resource file after code moved to assembly
            writer.RenderEndTag();
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            PropertyDescriptorCollection properties;
            PropertyDescriptor property;
            object text;
            object value;
            string formattedValue;
            string id;
            int index = 0;

            if ( HasAllOption )
                renderAllOption(writer);

            foreach ( object o in _data )
            {
                properties = ((ICustomTypeDescriptor)o).GetProperties();
                
                property = properties[DataTextField];
                if ( property == null )
                    throw new HttpException(500, string.Format("Databinding failed for CheckboxValueList '{0}'.  Unable to find property '{1}' specified by DataTextField.  Property names are case sensitive.", ClientID, DataTextField));
                text = property.GetValue(o);

                property = properties[DataValueField];
                if ( property == null )
                    throw new HttpException(500, string.Format("Databinding failed for CheckboxValueList '{0}'.  Unable to find property '{1}' specified by DataValueField.  Property names are case sensitive.", ClientID, DataValueField));
                value = property.GetValue(o);

                if ( string.IsNullOrEmpty(DataValueFormatString) )
                    formattedValue = value.ToString();
                else
                    formattedValue = string.Format(DataValueFormatString, value);

                writer.RenderBeginTag(HtmlTextWriterTag.Span); //begin span to wrap INPUT & LABEL

                id = string.Format("{0}_{1}", this.ClientID, formattedValue);
                writer.AddAttribute(HtmlTextWriterAttribute.Id, id);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, string.Format("chkItem {0}", ItemCssClass));
                writer.AddAttribute(HtmlTextWriterAttribute.Name, GroupValues ? ID : string.Format("{0}${1}", this.UniqueID, formattedValue));
                writer.AddAttribute(HtmlTextWriterAttribute.Value, (!TrueFalse ? formattedValue : TrueValue));
                if ( string.Compare(SelectedValue, (!TrueFalse ? formattedValue : TrueValue), true) == 0 )
                    writer.AddAttribute(HtmlTextWriterAttribute.Checked, "true");
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "checkbox");
                writer.RenderBeginTag(HtmlTextWriterTag.Input);
                writer.RenderEndTag();

                writer.AddAttribute(HtmlTextWriterAttribute.For, id);
                writer.RenderBeginTag(HtmlTextWriterTag.Label);
                writer.Write(text.ToString());
                writer.RenderEndTag();

                writer.RenderEndTag(); //end span

                index++;

                

            }
        }
    }
}