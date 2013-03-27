using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderModel;

public partial class controls_CoveredLivesScript : System.Web.UI.UserControl
{
    public string ContainerID { get; set; }

    //sl 7/20/2012 QL link fix
    public string ShowQLLink
    {
        get
        {
            bool yesno = true;
            bool yesno2 = false;
            if (Context.User.IsInRole("restr_ql"))
                return yesno.ToString().ToLower();
            else
                return yesno2.ToString().ToLower();
              
        }
    }
}
