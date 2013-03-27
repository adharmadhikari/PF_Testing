using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pinsonault.Web.Data ;
using System.Data;
using Utilities = Pinsonault.Web.Utilities;
using Pinsonault.Data;

public partial class todaysaccounts_all_OpenNotes : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int iPlanID = Convert.ToInt32(System.Web.HttpContext.Current.Request.QueryString["Plan_ID"]);
        int iDrugID = Convert.ToInt32(System.Web.HttpContext.Current.Request.QueryString["Drug_ID"]);
        int iFormularyID = Convert.ToInt32(System.Web.HttpContext.Current.Request.QueryString["FormularyID"]);
        int iSegmentID = Convert.ToInt32(System.Web.HttpContext.Current.Request.QueryString["SegmentID"]);
        string strType = System.Web.HttpContext.Current.Request.QueryString["Type"];

        //get the details from database
        string strQLNotes = "";
        string strSTNotes = "";
        string strComments = "";
        string strDrugName = "";
        
        DrugDetails drd = new DrugDetails();
        try
        {
            DataTable dtDrugDetails = drd.GetDrugDetails(iPlanID, iDrugID, iFormularyID,iSegmentID);
            if (dtDrugDetails != null)
            {
                if (dtDrugDetails.Rows.Count > 0)
                {
                    DataRow drDrugDetails = dtDrugDetails.Rows[0];
                    strQLNotes = drDrugDetails["DQL_Notes"].ToString();
                    strSTNotes = drDrugDetails["DST_Notes"].ToString();
                    strComments = drDrugDetails["Comments"].ToString();
                    strDrugName = drDrugDetails["Drug_Name"].ToString();                    
                }
            }
            // check the type and parse the notes accordingly
            this.titleText.Text = "Drug Name: " + strDrugName;
            switch (strType)
            {
                case "QL":
                    this.headerText.Text = strType + " Notes";
                    this.notesText.Text = strQLNotes;
                    break;
                case "ST":
                    this.headerText.Text = strType + " Notes";
                    this.notesText.Text = strSTNotes;
                    break;
                case "comments":
                    this.headerText.Text = "Comments";
                    this.notesText.Text = strComments;
                    break;                
                default:
                    this.notesText.Text = "";
                    break;

            }
        }
        catch (Exception ex)
        {
            Utilities.EventLogLogger eventLogger = new Utilities.EventLogLogger();
            eventLogger.LogError(ex.Message.ToString(), ex.StackTrace.ToString());
        }       
    }
}
