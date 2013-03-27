using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using PathfinderClientModel;
using Pinsonault.Application.Alcon;
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

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //chkPA.Attributes.Add("onclick", "return false;");
            //chkQL.Attributes.Add("onclick", "return false;");
            //chkST.Attributes.Add("onclick", "return false;");
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "_sspagevars", string.Format("var txtSegmentID = '{0}'; var txtRestrictionsID = '{1}';", txtSegment.ClientID, txtRestrictions.ClientID), true);

            Int32 ssid = Convert.ToInt32(Request.QueryString["Sell_Sheet_ID"]);

            using (PathfinderAlconEntities alconcontext = new PathfinderAlconEntities())
            {
                //SellSheet ss = null;
                Sell_Sheet_Mast ss = null;
                ss = (from d in alconcontext.Sell_Sheet_Mast
                      where d.Sell_Sheet_ID == ssid
                      select d).FirstOrDefault();

                bool cp = (ss.Segment_CP == null ? false : (Boolean)ss.Segment_CP);
                bool md = (ss.Segment_MD == null ? false : (Boolean)ss.Segment_MD);
                bool sm = (ss.Segment_SM == null ? false : (Boolean)ss.Segment_SM);
                bool mm = (ss.Segment_MM == null ? false : (Boolean)ss.Segment_MM);
                bool pbm = (ss.Segment_PBM == null ? false : (Boolean)ss.Segment_PBM);

                //if (cp == false && md == false && sm == false)
                //{
                //    chkSegmentCP.Checked = true;
                //    chkSegmentMD.Checked = true;
                //    chkSegmentSM.Checked = true;
                //}
                //else
                //{
                    chkSegmentCP.Checked = cp;
                    chkSegmentMD.Checked = md;
                    chkSegmentSM.Checked = sm;
                    chkSegmentMM.Checked = mm;
                    chkSegmentPBM.Checked = pbm;
                //}
            
                
                if (ss.Type_ID != null)
                    rblStatusType.SelectedValue = Convert.ToString(ss.Type_ID);
             
                if (ss.Use_Copay != null)
                    rblCopay.SelectedValue = Convert.ToString(ss.Use_Copay);

                //Set textbox text for validation purposes
                if (chkSegmentCP.Checked)
                    txtSegment.Value = txtSegment.Value + chkSegmentCP.ClientID;
                if (chkSegmentMD.Checked)
                    txtSegment.Value = txtSegment.Value + chkSegmentMD.ClientID;
                if (chkSegmentSM.Checked)
                    txtSegment.Value = txtSegment.Value + chkSegmentSM.ClientID;
                if (chkSegmentMM.Checked)
                    txtSegment.Value = txtSegment.Value + chkSegmentMM.ClientID;
                if (chkSegmentPBM.Checked)
                    txtSegment.Value = txtSegment.Value + chkSegmentPBM.ClientID;
                //if (chkPA.Checked)
                //    txtRestrictions.Value = txtRestrictions.Value + chkPA.ClientID;
                //if (chkQL.Checked)
                //    txtRestrictions.Value = txtRestrictions.Value + chkQL.ClientID;
                //if (chkST.Checked)
                //    txtRestrictions.Value = txtRestrictions.Value + chkST.ClientID;
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

            using (PathfinderAlconEntities alconcontext = new PathfinderAlconEntities())
            {
                Sell_Sheet_Mast ssheet = null; ;
                ssheet = (from d in alconcontext.Sell_Sheet_Mast
                          where d.Sell_Sheet_ID == ssid
                          select d).FirstOrDefault();

                //Clear selected plans from Step 4 if Segment Selection is changed
                bool changed = false;
                bool? cp, md, sm, mm, pbm;
                cp = ssheet.Segment_CP ?? false;
                md = ssheet.Segment_MD ?? false;
                sm = ssheet.Segment_SM ?? false;
                mm = ssheet.Segment_MM ?? false;
                pbm = ssheet.Segment_PBM ?? false;

                if (cp != chkSegmentCP.Checked || md != chkSegmentMD.Checked || sm != chkSegmentSM.Checked ||
                    mm != chkSegmentMM.Checked || pbm != chkSegmentPBM.Checked)
                    changed = true;

                if (changed) //Delete all previously selected plans since there was a change in segment selection
                {
                    var ssPlansQuery =
                    from d in alconcontext.Sell_Sheet_Plans
                    where d.Sell_Sheet_ID == ssid
                    select d;

                    //Delete all the records from Sell_Sheet_Plans table for selected Sell_Sheet_ID.
                    foreach (var plan in ssPlansQuery) alconcontext.DeleteObject(plan);
                }

                ssheet.Segment_CP = chkSegmentCP.Checked;
                ssheet.Segment_MD = chkSegmentMD.Checked;
                ssheet.Segment_SM = chkSegmentSM.Checked;
                ssheet.Segment_MM = chkSegmentMM.Checked;
                ssheet.Segment_PBM = chkSegmentPBM.Checked;

                ssheet.Type_ID = Convert.ToInt32(rblStatusType.SelectedValue);
                ssheet.Use_PA = true;
                ssheet.Use_QL = true;
                ssheet.Use_ST = true;
                ssheet.Use_Copay = Convert.ToBoolean(rblCopay.SelectedValue);
                ssheet.Modified_BY = Pinsonault.Web.Session.FullName;
                ssheet.Modified_DT = DateTime.UtcNow;
                ssheet.Territory_ID = Pinsonault.Web.Session.TerritoryID;

                //Logic below is to check if current step needs to be updated
                if (Master.RequestedStep == Master.CurrentStep)
                {
                    using (PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
                    {
                        string nextStep = (from ss in context.SellSheetStepSet
                                           where ss.Step_Order == (Master.RequestedStep + 1)
                                           select ss.Step_Key).FirstOrDefault();
                        ssheet.Current_Step = nextStep;
                    }
                }

                alconcontext.SaveChanges();
            }

            PostBackResult.Success = true;
        }
        catch
        {
            PostBackResult.Success = false;
        }
    }
 
}
