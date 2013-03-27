using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class controls_opportunityBrandTemplate : DynamicTemplateBase
{
    public override string DataFieldFormat1
    {
        get { return "Tier_Name{0}"; }
    }
    public override string DataFieldFormat2
    {
        get { return "Co_Pay{0}"; }
    }
    public override string DataFieldFormat3
    {
        get { return "Brand_TRx{0}"; }
    }
    public override string DataFieldFormat4
    {
        get { return "Brand_MST{0}"; }
    }

    public override string DataFieldFormat5
    {
        get { return "BrandName{0}"; }
    }
}