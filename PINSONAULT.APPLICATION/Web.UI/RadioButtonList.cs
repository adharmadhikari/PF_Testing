using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;

/// <summary>
/// Customized RadioButtonList control 
/// </summary>
/// 
namespace Pinsonault.Web.UI
{
    public class RadiobuttonValueList : System.Web.UI.WebControls.RadioButtonList  
    {
        public RadiobuttonValueList()
	    {
		    //
		    // TODO: Add constructor logic here
		    //
	    }

        [Bindable(true)]
        public int? SelectedVal
        {
            get
            {
                if (String.IsNullOrEmpty(SelectedValue))
                    return null;
                else
                    return Convert.ToInt32(SelectedValue);
            }
            set
            {
                if (value != null)
                    SelectedValue = value.ToString();
            }
        }
    }
}

