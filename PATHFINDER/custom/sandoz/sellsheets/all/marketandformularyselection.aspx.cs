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

public partial class custom_pinso_sellsheets_marketandformularyselection : InputFormBase
{
    protected string PageModule { get; set; }

    protected override void OnInit(EventArgs e)
    {
        PageModule = Path.GetFileNameWithoutExtension(Request.Url.AbsolutePath);
        dsSellSheetType.ConnectionString = Pinsonault.Web.Session.ClientConnectionString;
        base.OnInit(e);
    }

    protected override void OnError(EventArgs e)
    {
        
        base.OnError(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //Select Tier Status by default
            if (rblStatusType != null)
                rblStatusType.SelectedIndex = 0;

            //Check client options to enable controls
            Dictionary<string, PathfinderModel.ClientApplicationAccess> appAccess = Pinsonault.Web.Session.ClientApplicationAccess;
            foreach (KeyValuePair<string, PathfinderModel.ClientApplicationAccess> appAccessKeyVal in appAccess)
            {
                if (appAccessKeyVal.Value.ApplicationID == 1)
                {
                    //1 - Commericial
                    if (appAccessKeyVal.Value.SectionID == 1) 
                        chkSegmentCP.Visible = true;

                    //17 - Medicare Part D
                    if (appAccessKeyVal.Value.SectionID == 17) 
                        chkSegmentMD.Visible = true;
                }
            }

            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "_sspagevars", string.Format("var txtSegmentID = '{0}'; var txtRestrictionsID = '{1}';", txtSegment.ClientID, txtRestrictions.ClientID), true);

            Int32 ssid = Convert.ToInt32(Request.QueryString["Sell_Sheet_ID"]);

            using ( PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString) )
            {
                SellSheet ss = null; ;
                ss = (from d in context.SellSheetSet
                      where d.Sell_Sheet_ID == ssid
                      select d).FirstOrDefault();

                chkSegmentCP.Checked = (ss.Segment_CP == null ? false : (Boolean)ss.Segment_CP);
                chkSegmentMD.Checked = (ss.Segment_MD == null ? false : (Boolean)ss.Segment_MD);

                if (ss.Type_ID != null)
                    rblStatusType.SelectedValue = Convert.ToString(ss.Type_ID);
                //chkPA.Checked = (ss.Use_PA == null ? false : (Boolean)ss.Use_PA);
                //chkQL.Checked = (ss.Use_QL == null ? false : (Boolean)ss.Use_QL);
                chkST.Checked = (ss.Use_ST == null ? false : (Boolean)ss.Use_ST);
                if (ss.Use_Copay != null)
                    rblCopay.SelectedValue = Convert.ToString(ss.Use_Copay);

                //Set textbox text for validation purposes
                if (chkSegmentCP.Checked)
                    txtSegment.Value = txtSegment.Value + chkSegmentCP.ClientID;
                if (chkSegmentMD.Checked)
                    txtSegment.Value = txtSegment.Value + chkSegmentMD.ClientID;
                
                //if (chkPA.Checked)
                //    txtRestrictions.Value = txtRestrictions.Value + chkPA.ClientID;
                //if (chkQL.Checked)
                //    txtRestrictions.Value = txtRestrictions.Value + chkQL.ClientID;
                if (chkST.Checked)
                    txtRestrictions.Value = txtRestrictions.Value + chkST.ClientID;
            }
        }
    }

    protected override bool IsRequestValid()
    {
        return true;
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        try
        {
            Int32 ssid = Convert.ToInt32(Request.QueryString["Sell_Sheet_ID"]);

            using ( PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString) )
            {
                SellSheet ssheet = null; ;
                ssheet = (from d in context.SellSheetSet
                      where d.Sell_Sheet_ID == ssid
                      select d).FirstOrDefault();

                //Clear selected plans from Step 4 if Segment Selection is changed
                bool changed = false;
                bool? cp, md;
                cp = ssheet.Segment_CP ?? false;
                md = ssheet.Segment_MD ?? false;

                if (cp != chkSegmentCP.Checked || md != chkSegmentMD.Checked)
                    changed = true;

                if (changed) //Delete all previously selected plans since there was a change in segment selection
                {
                    var ssPlansQuery =
                    from d in context.SellSheetPlansSet
                    where d.Sell_Sheet_ID == ssid
                    select d;

                    //Delete all the records from Sell_Sheet_Plans table for selected Sell_Sheet_ID.
                    foreach (var plan in ssPlansQuery) context.DeleteObject(plan);
                }

                ssheet.Segment_CP = chkSegmentCP.Checked;
                ssheet.Segment_MD = chkSegmentMD.Checked;
                ssheet.Type_ID = Convert.ToInt32(rblStatusType.SelectedValue);
                //ssheet.Use_PA = chkPA.Checked;
                //ssheet.Use_QL = chkQL.Checked;
                ssheet.Use_ST = chkST.Checked;
                ssheet.Use_Copay = Convert.ToBoolean(rblCopay.SelectedValue);
                ssheet.Modified_BY = Pinsonault.Web.Session.FullName;
                ssheet.Modified_DT = DateTime.UtcNow;
                ssheet.Territory_ID = Pinsonault.Web.Session.TerritoryID;

                //Logic below is to check if current step needs to be updated
                if (Master.RequestedStep == Master.CurrentStep)
                {
                    string nextStep = (from ss in context.SellSheetStepSet
                                       where ss.Step_Order == (Master.RequestedStep + 1)
                                       select ss.Step_Key).FirstOrDefault();
                    ssheet.Current_Step = nextStep;
                }
                
                context.SaveChanges();
            }

            PostBackResult.Success = true;
        }
        catch
        {
            PostBackResult.Success = false;
        }
    }
}
