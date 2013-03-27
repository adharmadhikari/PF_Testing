using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class controls_opportunityBrandHeaderTemplate : DynamicTemplateBase
{
    public override string DataFieldFormat1
    {
        get { return "BrandName{0}"; }
    }
    public override string DataFieldFormat2
    {
        get { return ""; }
    }
    public override string DataFieldFormat3
    {
        get { return ""; }
    }
    public override string DataFieldFormat4
    {
        get { return ""; }
    }

}
