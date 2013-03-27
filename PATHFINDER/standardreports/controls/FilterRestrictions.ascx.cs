using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class standardreports_controls_FilterRestrictions : System.Web.UI.UserControl
{
    public string DefaultValue
    {
        get { return restrictionsList.DefaultValue; }
        set 
        { 
            restrictionsList.DefaultValue = value;
            //if ( string.IsNullOrEmpty(restrictions.SelectedValue) )
            //    restrictions.SelectedValue = value;
        }
    }

    public bool HasAllOption
    {
        get { return restrictions.HasAllOption; }
        set { restrictions.HasAllOption = value; }
    }
}
