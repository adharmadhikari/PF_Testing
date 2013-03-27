using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class controls_MyKeyContactsList : System.Web.UI.UserControl
{
    public int DrillDownLevel
    {
        get { return gridWrapper.DrillDownLevel; }
        set { gridWrapper.DrillDownLevel = value; }
    }


}
