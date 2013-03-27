using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderClientModel;
using Pathfinder;
using Telerik.Web.UI;
using Pinsonault.Web;
using System.Collections.Specialized;
using Pinsonault.Application.Merz;

public partial class custom_merz_businessplanning_controls_AddEditBusinessPlanning : System.Web.UI.UserControl
{
    //Used for returning form's submit status(true if successful else false).
    public bool SubmitStatus { get; set; }

    public bool Export { get; set; }

    public Int32 BusinessPlanID { get; set; }

    protected override void OnPreRender(EventArgs e)
    {
        if (formVWBP.CurrentMode == FormViewMode.ReadOnly)
            this.frmvmMode.Value = "readonly";
        else if (formVWBP.CurrentMode == FormViewMode.Edit)
            this.frmvmMode.Value = "edit";

        Int32 Thera_ID = Convert.ToInt32(HttpContext.Current.Request.QueryString["Thera_ID"]);

        if (Export)
        {
            //((System.Web.UI.HtmlControls.HtmlControl)this.formVWBP.FindControl("PBMDiv")).Attributes["class"] = "leftTile  dashboardTable leftBgPDFTile";
            //((System.Web.UI.HtmlControls.HtmlControl)this.formVWBP.FindControl("SPPDiv")).Attributes["class"] = "rightTile dashboardTable sppPDFTile";
            ((System.Web.UI.HtmlControls.HtmlControl)this.formVWBP.FindControl("PBMDiv")).Attributes["class"] = "leftTile  leftBgPDFTile";
            ((System.Web.UI.HtmlControls.HtmlControl)this.formVWBP.FindControl("SPPDiv")).Attributes["class"] = "rightTile sppPDFTile";
            ((System.Web.UI.HtmlControls.HtmlControl)this.formVWBP.FindControl("BPInfoHeader")).Attributes["class"] = "tileContainerHeader PDFDivHeader";
            ((System.Web.UI.HtmlControls.HtmlControl)this.formVWBP.FindControl("BPInfoDetailsDiv")).Attributes["class"] = "bpPDFTile";
        }
        else
        {
            ((System.Web.UI.HtmlControls.HtmlControl)this.formVWBP.FindControl("PBMDiv")).Attributes["class"] = "leftTile  pbmTile dashboardTable";
            ((System.Web.UI.HtmlControls.HtmlControl)this.formVWBP.FindControl("SPPDiv")).Attributes["class"] = "rightTile sppTile dashboardTable";
            ((System.Web.UI.HtmlControls.HtmlControl)this.formVWBP.FindControl("BPInfoHeader")).Attributes["class"] = "tileContainerHeader";
            ((System.Web.UI.HtmlControls.HtmlControl)this.formVWBP.FindControl("BPInfoDetailsDiv")).Attributes["class"] = "bpTile";
        }

        base.OnPreRender(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //((System.Web.UI.WebControls.HiddenField)(this.Parent.FindControl("BP_ID"))).Value

        //Show and hide save link based on the current formview mode.
        //In readonly mode hide save link and in editable mode show save link.
        if (formVWBP.CurrentMode == FormViewMode.ReadOnly)
        {
            this.frmvmMode.Value = "readonly";
        }
        else if (formVWBP.CurrentMode == FormViewMode.Edit)
        {
            this.frmvmMode.Value = "edit";
        }

        Int32 Plan_ID = Convert.ToInt32(HttpContext.Current.Request.QueryString["Plan_ID"]);

        using (PathfinderMerzEntities context = new PathfinderMerzEntities())
        {
            //Check if PBM and SPP records are present for selected Plan
            int pbmcnt =
                   (from d in context.PlanAffiliationsForPBMSet 
                    where d.Child_ID == Plan_ID 
                    select d).Count();

            int sppcnt = (from d in context.PlanAffiliationsForSPPSet
                          where d.Child_ID == Plan_ID && d.Business_Plan_ID == BusinessPlanID
                          select d).Count();
            
            //If PBM and SPP record count is zero then hide Affiliations section.
            if ((pbmcnt == 0) && (sppcnt == 0))
            {
                ((System.Web.UI.HtmlControls.HtmlControl)this.formVWBP.FindControl("AffiliationsDiv")).Visible = false; 
            }
            else
                ((System.Web.UI.HtmlControls.HtmlControl)this.formVWBP.FindControl("AffiliationsDiv")).Visible = true; 
        }
    }

    //Used for updates.
    protected void EditData(object sender, EntityDataSourceChangingEventArgs e)
    {
        PathfinderMerzEntities context = ((PathfinderMerzEntities)e.Context);

        BusinessPlans bp = ((BusinessPlans)e.Entity);

        Int32 BusinessPlanID = Convert.ToInt32(bp.Business_Plan_ID);
        Int32 Thera_ID = Convert.ToInt32(HttpContext.Current.Request.QueryString["Thera_ID"]);
        Int32 Section_ID = Convert.ToInt32(HttpContext.Current.Request.QueryString["Section_ID"]);

        //Update coverage data only for Neurology(Thera_ID = 1)
        if (Thera_ID == 1) //Neurology
        {
            //Load BusinessPlanCoverage
            bp.Business_Plan_Coverage.Load();

            //Update data for each coverage row
            foreach (GridViewRow rwcov in ((GridView)this.formVWBP.FindControl("grdvwNeuCoverage")).Rows)
            {
                Int32 DrugID;
                DrugID = Convert.ToInt32(((Label)(rwcov.Cells[0].FindControl("Drug_ID"))).Text);

                //See if BusinessPlanCoverage record exists for current BusinessPlanID and Drug_ID.
                BusinessPlanCoverage cov = bp.Business_Plan_Coverage.FirstOrDefault(c => c.Business_Plan_ID == BusinessPlanID && c.Drug_ID == DrugID);

                //If not then insert a new record
                if (cov == null)
                {
                    cov = new BusinessPlanCoverage();
                    bp.Business_Plan_Coverage.Add(cov);
                    cov.Business_Plan_ID = BusinessPlanID;
                    cov.BusinessPlanDrug = context.BusinessPlanDrugSet.FirstOrDefault(c => c.Drug_ID == DrugID);
                }

                if (rwcov.RowType == DataControlRowType.DataRow)
                {
                    //Update dbo.Business_Plan_Coverage table
                    Int32 BenefitID = Convert.ToInt32(((Telerik.Web.UI.RadComboBox)(rwcov.FindControl("rdcmbBenefits"))).SelectedValue);

                    if (BenefitID == 0)
                        cov.BusinessPlanBenefit = null;
                    else
                        cov.BusinessPlanBenefit = context.BusinessPlanBenefitSet.FirstOrDefault(c => c.ID == BenefitID);

                    Int32 MedPolicyID = Convert.ToInt32(((Telerik.Web.UI.RadComboBox)(rwcov.Cells[3].FindControl("rdcmbMedPolicy"))).SelectedValue);
                    if (MedPolicyID == 0)
                        cov.BusinessPlanMedicalPolicy = null;
                    else
                        cov.BusinessPlanMedicalPolicy = context.BusinessPlanMedicalPolicySet.FirstOrDefault(c => c.ID == MedPolicyID);

                    Int32 Formulary_Status_ID = Convert.ToInt32(((Telerik.Web.UI.RadComboBox)(rwcov.Cells[4].FindControl("rdcmbFormularyStatus"))).SelectedValue);
                    if (Formulary_Status_ID == 0)
                        cov.Formulary_Status_ID = null;
                    else
                        cov.Formulary_Status_ID = Formulary_Status_ID;

                    String[] restrictions = ((HiddenField)(rwcov.Cells[5].FindControl("SelectedRestrictions"))).Value.ToString().Split(new Char[] { ',' });
                    Boolean bPA = false, bQL = false, bST = false;

                    foreach (string s in restrictions)
                    {
                        if (s == "PA")
                            bPA = true;
                        else if (s == "QL")
                            bQL = true;
                        else if (s == "ST")
                            bST = true;
                    }
                    cov.PA = bPA;
                    cov.QL = bQL;
                    cov.ST = bST;

                    if (((TextBox)(rwcov.Cells[6].FindControl("Num_Allocationstxt"))).Text != "")
                        cov.Num_Allocations = Convert.ToInt32(((TextBox)(rwcov.Cells[6].FindControl("Num_Allocationstxt"))).Text);

                    cov.Copay_Coinsurance = ((TextBox)(rwcov.Cells[7].FindControl("Copay_Coinsurancetxt"))).Text.ToString();

                    if (((TextBox)(rwcov.Cells[8].FindControl("Market_Sharetxt"))).Text != "")
                        cov.Market_Share = Convert.ToDecimal(((TextBox)(rwcov.Cells[8].FindControl("Market_Sharetxt"))).Text);

                    if (((TextBox)(rwcov.Cells[9].FindControl("Gross_Salestxt"))).Text != "")
                        cov.Gross_Sales = Convert.ToDecimal(((TextBox)(rwcov.Cells[9].FindControl("Gross_Salestxt"))).Text);

                    cov.Contact_with_Manufacturer = ((TextBox)(rwcov.Cells[10].FindControl("Contact_with_Manufacturertxt"))).Text.ToString();
                    cov.Created_BY = Pinsonault.Web.Session.FullName;
                    cov.Created_DT = DateTime.UtcNow;
                    cov.Modified_DT = cov.Created_DT;
                    cov.Modified_BY = cov.Created_BY;
                }
            }
        }

        //Update dbo.Business_Plans table
        bp.Modified_DT = DateTime.UtcNow;
        bp.Modified_BY = Pinsonault.Web.Session.FullName;
        if (Thera_ID == 1) //Neurology
        {
            bp.Issues = ((TextBox)this.formVWBP.FindControl("Issues1")).Text.ToString();
            bp.Strategies = ((TextBox)this.formVWBP.FindControl("Strategies1")).Text.ToString();
            bp.Tactics = ((TextBox)this.formVWBP.FindControl("Tactics1")).Text.ToString();
        }
        else //Dermatology
        {
            bp.Issues = ((TextBox)this.formVWBP.FindControl("Issues2")).Text.ToString();
            bp.Strategies = ((TextBox)this.formVWBP.FindControl("Strategies2")).Text.ToString();
            bp.Tactics = ((TextBox)this.formVWBP.FindControl("Tactics2")).Text.ToString();
        }

        //Update Business_Plan_Preferred_SPP table
        UpdateSPP(bp, BusinessPlanID, context);

        //After saving the data switch back to readonly mode.
        this.formVWBP.ChangeMode(FormViewMode.ReadOnly);
        this.frmvmMode.Value = "readonly";
    }

    protected void UpdateSPP(BusinessPlans bp, Int32 BusinessPlanID, PathfinderMerzEntities context)
    {
        bp.BusinessPlanPreferredSPP.Load();

        //Update data for each spp affliations row
        foreach (GridViewRow rwspp in ((GridView)this.formVWBP.FindControl("grdvwSPPAffliations")).Rows)
        {
            //Get SPPID
            Int32 SPPID = Convert.ToInt32(((Label)(rwspp.FindControl("SPP_ID"))).Text);

            //See if BusinessPlanPreferredSPP record exists for current BusinessPlanID and SPPID.
            BusinessPlanPreferredSPP spp = bp.BusinessPlanPreferredSPP.FirstOrDefault(s => s.Business_Plan_ID == BusinessPlanID && s.SPP_ID == SPPID);

            //If the rtecord doesn't exists then insert a new record for checked spp's.
            if (spp == null)
            {
                //If checkbox is checked then add a record in Business_Plan_Preferred_SPP table
                if (((CheckBox)(rwspp.FindControl("chkSPP"))).Checked == true)
                {
                    spp = new BusinessPlanPreferredSPP();
                    bp.BusinessPlanPreferredSPP.Add(spp);
                    spp.Business_Plan_ID = BusinessPlanID;
                    spp.SPP_ID = SPPID;
                }
            }
            else
            {
                //If spp record already exists and the checkbox is NOT checked then delete the record.
                if (((CheckBox)(rwspp.FindControl("chkSPP"))).Checked != true)
                {
                    context.DeleteObject(spp);
                }
            }

        }

    }

    //Confirmation after update.
    protected void ConfirmUpdate(object sender, EntityDataSourceChangedEventArgs e)
    {
        //return status(true/false) after update.
        if (e.Exception == null)
        {
            SubmitStatus = true;
        }
        else
        {
            SubmitStatus = false;
        }
    }

    //When clicked on 'Edit Coverage' button in readonly view mode it changes form's mode to Edit.
    protected void ViewEditbtn_Click(object sender, EventArgs e)
    {
        this.formVWBP.ChangeMode(FormViewMode.Edit);
        this.frmvmMode.Value = "edit";
    }

    protected string ConvertLineBreaks(String input)
    {
        if (!String.IsNullOrEmpty(input))
        {
            //Replace carrige return/linefeed with <BR/>.
            input = input.Replace("\r\n", "<BR/>");
            //Replace registered trademark character(®) with &reg;
            return input.Replace("®", "&reg;");
        }
        else
            return "";
    }
}
