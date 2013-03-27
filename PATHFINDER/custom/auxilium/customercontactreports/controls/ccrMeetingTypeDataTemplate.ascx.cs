using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Pinsonault.Data.Reports;

public partial class custom_controls_MeetingTypeDataTemplate : System.Web.UI.UserControl
{
    public RadGrid HostedGrid
    {
        get { return gridCcrMeetingType; }
    }
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
    }

    protected void gridCcrMeetingType_PreRender(object sender, EventArgs e)
    {
        foreach (Telerik.Web.UI.GridDataItem item in gridCcrMeetingType.MasterTableView.Items)
        {
            System.Web.UI.WebControls.TableCell c = item["Meeting_Type_ID"];

            int num;
            bool isNum = int.TryParse(c.Text, out num);

            if (isNum)
            {
                c.BackColor = ReportColors.CustomerContactReports.GetColor(num - 1);
                c.Text = "";
            }
        }
    }
    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, gridCcrMeetingType.ClientID);
    //}
}

