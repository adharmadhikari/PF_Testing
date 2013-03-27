using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pinsonault.Data.Reports;

public partial class custom_controls_ccrProductsDiscussedDataTemplate : System.Web.UI.UserControl
{
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
    }
    //public override Telerik.Web.UI.RadGrid HostedGrid
    public Telerik.Web.UI.RadGrid HostedGrid
    {
        get { return gridCcrProductsDiscussed; }
    }

    protected void gridCcrProductsDiscussed_PreRender(object sender, EventArgs e)
    {
        foreach (Telerik.Web.UI.GridDataItem item in gridCcrProductsDiscussed.MasterTableView.Items)
        {
            System.Web.UI.WebControls.TableCell c = item["Products_Discussed_ID"];

            int num;
            bool isNum = int.TryParse(c.Text, out num);

            if (isNum)
            {
                c.BackColor = ReportColors.CustomerContactReports.GetColor(num - 1);
                c.Text = "";
            }
        }
    }

}
