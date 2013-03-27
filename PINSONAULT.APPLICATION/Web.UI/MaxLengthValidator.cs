using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pinsonault.Web.UI
{
    /// <summary>
    /// Summary description for MaxLengthValidator.
    /// </summary>
    public class MaxLengthValidator : CustomValidator
    {
        public MaxLengthValidator()
        {
        }

        public int MaxLength 
        {
            get { return (int)ViewState["MaxLength"]; }
            set { ViewState["MaxLength"] = value; }
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            ClientValidationFunction = "CheckMaxLength";
            Attributes.Add("MaxLength", MaxLength.ToString());
        }

        protected override bool ControlPropertiesValid()
        {
            //	return base.ControlPropertiesValid ();
            return true;
        }

        protected override bool EvaluateIsValid()
        {
            //Control control = FindControl(ControlToValidate);

            string value = GetControlValidationValue(ControlToValidate);

            return (value.Length <= MaxLength);
        }

    }
}