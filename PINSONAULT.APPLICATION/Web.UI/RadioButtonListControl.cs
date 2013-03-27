using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;

/// <summary>
/// Summary description for RadioButtonListControl
/// </summary>
/// 
namespace Pinsonault.Web.UI
{
    public class RadioButtonListControl : System.Web.UI.WebControls.RadioButtonList
    {
        public RadioButtonListControl()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        [Bindable(true)]
        public bool? SelectedVal
        {
            get
            {
                if (String.IsNullOrEmpty(SelectedValue))
                    return null;
                else
                    return Convert.ToBoolean(SelectedValue);
            }
            set
            {
                if (value != null)
                    SelectedValue = value.ToString();
            }
        }
    }
}
