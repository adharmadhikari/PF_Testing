using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderModel;
using Pinsonault.Web;
using Pinsonault.Application.Dey;


public partial class custom_controls_NewSellSheetOrder : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "_sspagevars", string.Format("var regionCtrlID = '{0}'; var DistrictCtrlID = '{1}';var RepCtrlID = '{2}'; var GridCtrlID = '{3}'; ", rdcmbRegion.ClientID, rdcmbDistrict.ClientID, rdcmbRep.ClientID, gridRep.ClientID), true);
       
    }

   

    

}
