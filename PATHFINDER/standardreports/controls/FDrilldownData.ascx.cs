using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderModel;
public partial class standardreports_controls_FDrilldownData : System.Web.UI.UserControl
{
    //public bool ShowProductName { get; set; } 

    protected override void OnLoad(EventArgs e)
    {
        gridF.Columns.FindByUniqueName("Plan_Name").HeaderStyle.Width = new Unit("10%");
        gridF.Columns.FindByUniqueName("Formulary_Status_Name").HeaderStyle.Width = new Unit("6%");
        gridF.Columns.FindByUniqueName("Formulary_Name").HeaderStyle.Width = new Unit("8%");
        gridF.Columns.FindByUniqueName("Drug_Name").HeaderStyle.Width = new Unit("7%");
        gridF.Columns.FindByUniqueName("Geography_Name").HeaderStyle.Width = new Unit("8%");
        gridF.Columns.FindByUniqueName("PBM_Name").HeaderStyle.Width = new Unit("8%");
        gridF.Columns.FindByUniqueName("Comments").HeaderStyle.Width = new Unit("2%");
        gridF.Columns.FindByUniqueName("FDD_Pharmacy_Lives").HeaderStyle.Width = new Unit("7%");
        base.OnLoad(e);
    }
}
