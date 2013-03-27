using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pinsonault.Web;
using PathfinderClientModel;
using Telerik.Web.UI;
using Pinsonault.Application.Dey;



public partial class custom_dey_sellsheets_sellsheets : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
      Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "_sspagevars", string.Format("var regionCtrlID = '{0}'; var DistrictCtrlID = '{1}';var RepCtrlID = '{2}'; ", rdcmbRegion.ClientID, rdcmbDistrict.ClientID, rdcmbRep.ClientID), true);
    }
  
    protected void btn_RepData_Click(object sender, EventArgs e)
    {
        Int32 SheetID = Convert.ToInt32(SelectedSheetID.Value);

        string Selected_Reps = SelectedRepID.Value;

    }
}
 