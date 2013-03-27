using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using PathfinderClientModel;
using Pinsonault.Web;
using System.IO;
using Telerik.Web.UI;

public partial class restrictionsreport_controls_MedicalPharmacyCoverageDataTemplate : System.Web.UI.UserControl
{
    //public override Telerik.Web.UI.RadGrid HostedGrid
    public RadGrid HostedGrid
    {
        get { return gridRestrictionsReport; }
    }

    public string GridLabel
    {
        get { return geoName.Text; }
        set { geoName.Text = value; }
    }

    protected void gridRestrictionsReport_PreRender(object sender, EventArgs e)
    {
        //Merge headers
        foreach (GridHeaderItem dataItem in gridRestrictionsReport.MasterTableView.GetItems(GridItemType.Header))
        {
            for (int x = 0; x < dataItem.Cells.Count; x++)
            {
                if (x + 1 != dataItem.Cells.Count && dataItem.Cells[x].Text == dataItem.Cells[x + 1].Text)
                {
                    dataItem.Cells[x].ColumnSpan = 2;
                    dataItem.Cells[x + 1].Visible = false;
                }
            }
        }
    }
}
