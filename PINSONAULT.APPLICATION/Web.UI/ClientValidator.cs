using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace Pinsonault.Web.UI
{


    /// <summary>
    /// Summary description for ClientValidator
    /// </summary>
    public class ClientValidator : WebControl
    {
        public string Target { get; set; }
        public string Text { get; set; }
        public string DataField { get; set; }
        public string RegExp { get; set; }
        public bool Required { get; set; }
        public int MaxLength { get; set; }
        public string CompareTo { get; set; }
        public string CompareToValue { get; set; }

        public ValidationCompareOperator CompareOperator { get; set; }

        public ValidationDataType DataType { get; set; }

        protected override HtmlTextWriterTag TagKey
        {
            get { return HtmlTextWriterTag.Span; }
        }

        public override string CssClass
        {
            get { return "validator"; }
            set { throw new NotSupportedException("CssClass property on ClientValidator control always returns the css name 'validator'"); }
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, CssClass);

            Control target = FindControl(Target);
            if ( target != null )
            {
                writer.AddAttribute("target", target.ClientID);// string.Format("{0}_{1}", this.NamingContainer.ClientID, Target));
                writer.AddAttribute("formField", target.UniqueID); //string.Format("{0}${1}", this.NamingContainer.UniqueID, Target));
            }
            else
            {
                writer.AddAttribute("target", Target);
                writer.AddAttribute("formField", Target);
            }

            //datafield is value stored in "data" collection that is posted to server - if not explicitly set then use UniqueID which translates to the controls form name
            if(!string.IsNullOrEmpty(DataField))
                writer.AddAttribute("dataField", DataField);
             

            if ( Required )
                writer.AddAttribute("_required", "true");

            if ( DataType != ValidationDataType.String )
                writer.AddAttribute("_dataType", DataType.ToString());

            if ( !string.IsNullOrEmpty(CompareTo) )            
                writer.AddAttribute("_compareTo", string.Format("{0}_{1}", this.NamingContainer.ClientID, CompareTo));                                           
            else if (!string.IsNullOrEmpty(CompareToValue) )            
                writer.AddAttribute("_compareToValue", CompareToValue);

            writer.AddAttribute("_compareOp", CompareOperator.ToString());

            if ( !string.IsNullOrEmpty(RegExp) )
            {
                writer.AddAttribute("_regExp", RegExp);
            }

            if (MaxLength > 0)
            {
                writer.AddAttribute("_maxLength", MaxLength.ToString()); 
            }

            base.AddAttributesToRender(writer);
        }


        protected override void RenderContents(HtmlTextWriter writer)
        {
            writer.Write(string.IsNullOrEmpty(Text) ? string.Format("[{0}] is invalid.", Target) : Text);
        }
    }
}