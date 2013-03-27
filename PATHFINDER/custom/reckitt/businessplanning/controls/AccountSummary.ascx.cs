using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class custom_reckitt_businessplanning_controls_AccountSummary : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected string ConvertLineBreaks(String input)
    {
        if (!String.IsNullOrEmpty(input))
        {
            //Replace carrige return/linefeed with <BR/>.
            input = input.Replace("\r\n", "<BR/>");
            //Replace registered trademark character(®) with &reg;
            return input.Replace("®", "&reg;");
        }
        else
            return "";
    }
}
