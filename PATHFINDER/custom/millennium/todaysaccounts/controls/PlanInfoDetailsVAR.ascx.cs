using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class custom_millennium_todaysaccounts_controls_PlanInfoDetails : UserControl
{
    protected string FormatCustomerMasterID(String input)
    {
        if (!String.IsNullOrEmpty(input))
        {
            if (input.Trim() == "0")
                return "";
            else
                return input;
        }
        else
            return "";
    }
}
