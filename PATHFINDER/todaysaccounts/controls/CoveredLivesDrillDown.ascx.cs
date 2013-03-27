using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI; 

public partial class todaysaccounts_controls_CoveredLivesDrillDown : System.Web.UI.UserControl
{
    public todaysaccounts_controls_CoveredLivesDrillDown()
    {
        ContainerID = "section2";
        IsStateMedicaid = "false";
    }

    public string IsStateMedicaid { get; set; }
    public string ContainerID { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, rdcmbCLTheraClass.ClientID, null, ContainerID);
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, rdcmbCLDrugs.ClientID, null, ContainerID);

        radGridWrapper.ContainerID = ContainerID;


        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "_clddpagevars", string.Format("var gridCLDrillDownID = '{0}';var drugCtrlID='{1}';var drugCLTheraClassCtrlID='{2}'; ", gridcoveredlivesdrilldown.ClientID, rdcmbCLDrugs.ClientID, rdcmbCLTheraClass.ClientID), true);

        //hide med policy and tier for State Medicaid
        if (IsStateMedicaid == "true")
        {            
            gridcoveredlivesdrilldown.Columns[6].Visible = false; //med policy, tier is visible for state medicaid as well
        }

        //Show QL Restriction criteria Hyperlink if user is part of Restriction Criteria - QL role
        //if (Context.User.IsInRole("restr_ql"))
        //{
        //    gridcoveredlivesdrilldown.Columns[5].Visible = true;
        //    gridcoveredlivesdrilldown.Columns[4].Visible = false;
        //}//Otherwise don't show the hyperlink
        //else
        //{
        //    gridcoveredlivesdrilldown.Columns[5].Visible = false;
        //    gridcoveredlivesdrilldown.Columns[4].Visible = true;
        //}
    }
}
