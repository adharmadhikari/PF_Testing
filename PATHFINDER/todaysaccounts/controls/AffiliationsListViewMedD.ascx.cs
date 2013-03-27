using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class todaysaccounts_controls_AffiliationsListViewMedD : System.Web.UI.UserControl
{
    public string AffiliationType { get; set; }
    public bool ShowPBMServices { get; set; }


    protected override void OnPreRender(EventArgs e)
    {
        GridColumn column = gridAffiliations.Columns.FindByUniqueNameSafe("PBM_Service");
        if ( column != null )
        {
            column.Visible = ShowPBMServices;
        }

        base.OnPreRender(e);
    }
}
