using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class standardreports_controls_FormularyUpdateDate : System.Web.UI.UserControl
{
    protected override void OnLoad(EventArgs e)
    {
        string sectionID = Request.QueryString["Section_ID"];
        int id = 0;
        if ( !string.IsNullOrEmpty(sectionID) && int.TryParse(sectionID, out id) )
        {
            switch ( id )
            {
                case 1:
                    formularyUpdateDate.Text = Pinsonault.Web.Support.GetDataUpdateDateByKey("Commercial Formulary", Resources.Resource.Label_Section_Last_Updated);
                    break;
                case 17:
                    formularyUpdateDate.Text = Pinsonault.Web.Support.GetDataUpdateDateByKey("Part-D Formulary", Resources.Resource.Label_Section_Last_Updated);
                    break;
                //case 4:
                //    break;
                case 6://Managed Medicaid
                    formularyUpdateDate.Text = Pinsonault.Web.Support.GetDataUpdateDateByKey("Commercial Formulary", Resources.Resource.Label_Section_Last_Updated);
                    break;
                default:
                    formularyUpdateDate.Text = "";
                    break;
            }
        }

        base.OnLoad(e);
    }
}
