using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;

public partial class standardreports_controls_LivesDistribution : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        //String s = Request.QueryString["Section_ID"];
        
        //switch (s)
        //{
        //    // Commercial
        //    case "1":
        //        break;
        //    // PBM
        //    case "4":                
        //        gridLDReport.Columns.FindByUniqueName("Commercial_Pharmacy_Lives").Visible = false;
        //        gridLDReport.Columns.FindByUniqueName("HMO_Lives").Visible = false;
        //        gridLDReport.Columns.FindByUniqueName("PPO_Lives").Visible = false;
        //        gridLDReport.Columns.FindByUniqueName("POS_Lives").Visible = false;
        //        gridLDReport.Columns.FindByUniqueName("CDH_Lives").Visible = false;
        //        gridLDReport.Columns.FindByUniqueName("Geography_Name").Visible = false;
        //        gridLDReport.Columns.FindByUniqueName("Health_Plan_Lives").Visible = true;
        //        gridLDReport.Columns.FindByUniqueName("Employer_Lives").Visible = true;
        //        gridLDReport.Columns.FindByUniqueName("Fully_Insured_Lives").Visible = true;
        //        gridLDReport.Columns.FindByUniqueName("Labor_Organization_Lives").Visible = true;
        //        break;
        //    //Managed Medicaid
        //    case "6":
        //        break;
        //    // State Medicaid
        //    case "9":
        //        gridLDReport.Columns.FindByUniqueName("Total_Covered").Visible = false;
        //        gridLDReport.Columns.FindByUniqueName("Medicaid_Enrollment").Visible = true;
        //        gridLDReport.Columns.FindByUniqueName("Medical_Lives").Visible = false;
        //        gridLDReport.Columns.FindByUniqueName("Total_Pharmacy").Visible = false;
        //        gridLDReport.Columns.FindByUniqueName("Commercial_Pharmacy_Lives").Visible = false;
        //        gridLDReport.Columns.FindByUniqueName("HMO_Lives").Visible = false;
        //        gridLDReport.Columns.FindByUniqueName("PPO_Lives").Visible = false;
        //        gridLDReport.Columns.FindByUniqueName("POS_Lives").Visible = false;
        //        gridLDReport.Columns.FindByUniqueName("Managed_Medicaid_Lives").Visible = false;
        //        gridLDReport.Columns.FindByUniqueName("Medicaid_Mcare_Enrollment").Visible = true;
        //        gridLDReport.Columns.FindByUniqueName("CDH_Lives").Visible = false;
        //        gridLDReport.Columns.FindByUniqueName("Medicare_PartD_Lives").Visible = false;
        //        gridLDReport.Columns.FindByUniqueName("MAPD_Lives").Visible = false;
        //        gridLDReport.Columns.FindByUniqueName("PDP_Lives").Visible = false;
        //        gridLDReport.Columns.FindByUniqueName("Health_Plan_Lives").Visible = false;
        //        gridLDReport.Columns.FindByUniqueName("Self_Insured_Lives").Visible = false;
        //        gridLDReport.Columns.FindByUniqueName("Other_Lives").Visible = false;
        //        gridLDReport.Columns.FindByUniqueName("Percent_ManagedCare").Visible = true;
        //        gridLDReport.Columns.FindByUniqueName("FFS_Lives").Visible = true;
        //        gridLDReport.Columns.FindByUniqueName("Geography_Name").Visible = false;
        //        break;
        //    // Medicare Part D
        //    case "17":
        //        gridLDReport.Columns.FindByUniqueName("Total_Covered").Visible = false;
        //        gridLDReport.Columns.FindByUniqueName("Medicaid_Enrollment").Visible = false;
        //        gridLDReport.Columns.FindByUniqueName("Medical_Lives").Visible = false;
        //        gridLDReport.Columns.FindByUniqueName("Total_Pharmacy").Visible = false;
        //        gridLDReport.Columns.FindByUniqueName("Commercial_Pharmacy_Lives").Visible = false;
        //        gridLDReport.Columns.FindByUniqueName("HMO_Lives").Visible = false;
        //        gridLDReport.Columns.FindByUniqueName("PPO_Lives").Visible = false;
        //        gridLDReport.Columns.FindByUniqueName("POS_Lives").Visible = false;
        //        gridLDReport.Columns.FindByUniqueName("Managed_Medicaid_Lives").Visible = false;
        //        gridLDReport.Columns.FindByUniqueName("Medicaid_Mcare_Enrollment").Visible = false;
        //        gridLDReport.Columns.FindByUniqueName("CDH_Lives").Visible = false;
        //        gridLDReport.Columns.FindByUniqueName("Self_Insured_Lives").Visible = false;
        //        gridLDReport.Columns.FindByUniqueName("Other_Lives").Visible = false;
        //        break;
        //}
        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "resetGridHeaders", "resetGridHeaders();", true);
    }
}
