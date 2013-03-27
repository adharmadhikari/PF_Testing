using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections;
//using PathfinderClientModel;
using Pinsonault.Application.Millennium;

public partial class custom_millennium_todaysaccounts_all_AddEditKDMMJ : PageBase
{
    protected void Page_Init(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            setComboValues();
            this.formKDMView.Visible = true;

        }
        if (Request.QueryString["LinkClicked"] == "AddKDM")
        {
            this.formKDMView.ChangeMode(FormViewMode.Insert);
        }
        else if (Request.QueryString["LinkClicked"] == "EditKDM")
        {
            this.formKDMView.ChangeMode(FormViewMode.Edit);
        }
        else if (Request.QueryString["LinkClicked"] == "DelKDM")
        {
            this.formKDMView.ChangeMode(FormViewMode.ReadOnly);
        }


    }
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Msglbl.Visible = false;
        Page.ClientScript.RegisterStartupScript(typeof(Page), "UpdCustomTitle", "updCustomTitleChkSelection();", true);
        Page.ClientScript.RegisterStartupScript(typeof(Page), "UpdCredentialsTitle", "updCredentialsChkSelection();", true);
        Page.ClientScript.RegisterStartupScript(typeof(Page), "UpdCAC", "updCACChkSelection();", true);
        titleText.Text = String.Format("{0} Key Decision Maker", Request.QueryString["LinkClicked"].Replace("KDM", ""));
    }

    protected void setComboValues()
    {
        string strKDMID = System.Web.HttpContext.Current.Request.QueryString["KDM_ID"];
        int KDM_ID = 0;


        if (strKDMID != null && strKDMID != "")
        {
            KDM_ID = System.Convert.ToInt32(strKDMID);
        }
        if (KDM_ID != 0)
        {
            using (PathfinderMillenniumEntities context = new PathfinderMillenniumEntities())
            {

                var Title = (from j in context.CustomKDMTitleSet
                                where j.KDM_ID == KDM_ID
                                select j).ToList().Select(j => string.Format("{0}", j.Title_ID.ToString()));
                //Comma separate individual record's data.
                hdnMeetOutcome.Value = string.Join(",", Title.ToArray());



                var Credentials = (from j in context.CustomKDMCredentialSet
                                      where j.KDM_ID == KDM_ID
                                      select j).ToList().Select(j => string.Format("{0}", j.Credentials_ID.ToString()));
                //Comma separate individual record's data.
                hdnCredentialsOutcome.Value = string.Join(",", Credentials.ToArray());


                var CAC = (from j in context.CustomKDMCACSet
                                   where j.KDM_ID == KDM_ID
                                   select j).ToList().Select(j => string.Format("{0}", j.CAC_ID.ToString()));
                //Comma separate individual record's data.
                hdnCACOutcome.Value = string.Join(",", CAC.ToArray());
            }
        }
        else { hdnMeetOutcome.Value = ""; hdnCredentialsOutcome.Value = ""; hdnCACOutcome.Value = ""; }
    }

    protected void Addbtn_Click(object sender, EventArgs e)
    {
        ProcessRequest("Add");
    }

    protected void Editbtn_Click(object sender, EventArgs e)
    {
        ProcessRequest("Edit");
    }
    //Called when "Delete" button is clicked on my key contact update form.
    protected void Delbtn_Click(object sender, EventArgs e)
    {
        //ProcessRequest("Edit");

        using (PathfinderMillenniumEntities context = new PathfinderMillenniumEntities())
        {

            if (DeletePlan(System.Convert.ToInt32(Page.Request.QueryString["KDM_ID"])))
            {

                this.Msglbl.Text = "<br/><b>Selected Key Decision Maker has been deleted successfully.</b><br/><br/>";
                this.Msglbl.Visible = true;
                formKDMView.Visible = false;
                Page.ClientScript.RegisterStartupScript(typeof(Page), "RefreshPlanInfo", "RefreshPlanInfo();", true);
            }

        }
    }


    public bool DeletePlan(int KDM_ID)
    {


        if (KDM_ID != null)
        {
            using (PathfinderMillenniumEntities context = new PathfinderMillenniumEntities())
            {

                Custom_KDM customKDM;
                customKDM = context.CustomKDMSet.FirstOrDefault(p => p.KDM_ID == KDM_ID);
                customKDM.Status = 0;
                customKDM.Modified_DT = DateTime.UtcNow;
                customKDM.Modified_BY = Pinsonault.Web.Session.FullName;
                context.SaveChanges();

            }

            return true;
        }

        return false;


    }

    protected void ProcessRequest(string BtnClicked)
    {
        Page.ClientScript.RegisterStartupScript(typeof(Page), "UpdCustomTitle", "updCustomTitleChkSelection();", true);
        Page.ClientScript.RegisterStartupScript(typeof(Page), "UpdCredentialsTitle", "updCredentialsChkSelection();", true);
        Page.ClientScript.RegisterStartupScript(typeof(Page), "UpdCAC", "updCACChkSelection();", true);

        string msg = "";
        string strPlanID = System.Web.HttpContext.Current.Request.QueryString["PlanID"];
        int Plan_ID = 0;
        if (strPlanID != null && strPlanID != "")
        {
            Plan_ID = System.Convert.ToInt32(strPlanID);
        }
        string KDMID = System.Web.HttpContext.Current.Request.QueryString["KDM_ID"];
        int KDM_ID = 0;
        if (KDMID != null && KDMID != "")
        {
            KDM_ID = System.Convert.ToInt32(KDMID);
        }

        using (PathfinderMillenniumEntities context = new PathfinderMillenniumEntities())
        {
            Custom_KDM customKDM;

            if (KDM_ID != 0)
            {
                msg = "<br/><b>Key Decision Maker Information has been updated successfully.</b><br/><br/>";
                customKDM = context.CustomKDMSet.FirstOrDefault(p => p.KDM_ID == KDM_ID);
                customKDM.Plans_Client = context.PlansClientSet.FirstOrDefault(p => p.Plan_ID == Plan_ID);
                customKDM.CAC_CMD = ((Telerik.Web.UI.RadComboBox)(this.formKDMView.FindControl("rdlCAC_CMD"))).SelectedValue;
                customKDM.KDM_F_Name = ((System.Web.UI.WebControls.TextBox)(this.formKDMView.FindControl("FNametxt"))).Text;
                customKDM.KDM_L_Name = ((System.Web.UI.WebControls.TextBox)(this.formKDMView.FindControl("LNametxt"))).Text;
                customKDM.KDM_Email = ((System.Web.UI.WebControls.TextBox)(this.formKDMView.FindControl("Emailtxt"))).Text;
                customKDM.KDM_Phone = ((System.Web.UI.WebControls.TextBox)(this.formKDMView.FindControl("Phtxt"))).Text;
                customKDM.KDM_Fax = ((System.Web.UI.WebControls.TextBox)(this.formKDMView.FindControl("Faxtxt"))).Text;
                customKDM.Aff_MJ_Org_Name = ((System.Web.UI.WebControls.TextBox)(this.formKDMView.FindControl("Aff_MJ_Org_Name"))).Text;
                customKDM.Status = 1;

            }
            else
            {
                customKDM = new Custom_KDM();

                msg = "<br/><b>Key Decision Maker Information has been added successfully.</b><br/><br/>";
                // customKDM = context.CustomKDMSet.FirstOrDefault(p => p.KDM_ID == KDM_ID);
                customKDM.Plans_Client = context.PlansClientSet.FirstOrDefault(p => p.Plan_ID == Plan_ID);
                customKDM.CAC_CMD = ((Telerik.Web.UI.RadComboBox)(this.formKDMView.FindControl("rdlCAC_CMD"))).SelectedValue;
                customKDM.KDM_F_Name = ((System.Web.UI.WebControls.TextBox)(this.formKDMView.FindControl("FNametxt"))).Text;
                customKDM.KDM_L_Name = ((System.Web.UI.WebControls.TextBox)(this.formKDMView.FindControl("LNametxt"))).Text;
                customKDM.KDM_Email = ((System.Web.UI.WebControls.TextBox)(this.formKDMView.FindControl("Emailtxt"))).Text;
                customKDM.KDM_Phone = ((System.Web.UI.WebControls.TextBox)(this.formKDMView.FindControl("Phtxt"))).Text;
                customKDM.KDM_Fax = ((System.Web.UI.WebControls.TextBox)(this.formKDMView.FindControl("Faxtxt"))).Text;
                customKDM.Aff_MJ_Org_Name = ((System.Web.UI.WebControls.TextBox)(this.formKDMView.FindControl("Aff_MJ_Org_Name"))).Text;
                customKDM.Status = 1;
                context.AddToCustomKDMSet(customKDM);

            }
            context.SaveChanges();

            var title = from t in context.CustomKDMTitleSet
                           where t.KDM_ID == KDM_ID
                           select t;
            foreach (var plan in title) context.DeleteObject(plan);

            var cd = from c in context.CustomKDMCredentialSet
                        where c.KDM_ID == KDM_ID
                        select c;
            foreach (var plan in cd) context.DeleteObject(plan);

            var cac = from ca in context.CustomKDMCACSet
                     where ca.KDM_ID == KDM_ID
                     select ca;
            foreach (var plan in cac) context.DeleteObject(plan);

            context.SaveChanges();


            if (!String.IsNullOrEmpty(hdnMeetOutcome.Value.ToString()))
            {

                if (hdnMeetOutcome.Value.ToString().IndexOf(",") > 0)
                {
                    //Split the data by comma 
                    string[] JCids = hdnMeetOutcome.Value.ToString().Split(new Char[] { ',' });
                    foreach (string ids in JCids)
                    {
                        int JCIDs = Convert.ToInt32(ids);
                        Custom_KDM_Title JC = new Custom_KDM_Title();
                        int _KDMID = customKDM.KDM_ID;
                        JC.Custom_KDM = context.CustomKDMSet.FirstOrDefault(p => p.KDM_ID == _KDMID);
                        int Title = Convert.ToInt32(JCIDs);
                        JC.Lkp_Custom_Title = context.CustomTitleSet.FirstOrDefault(s => s.Title_ID == Title);
                        context.AddToCustomKDMTitleSet(JC);
                    }

                }
                else
                {
                    string JCid = hdnMeetOutcome.Value.ToString();
                    Custom_KDM_Title JC = new Custom_KDM_Title();
                    int _KDMID = customKDM.KDM_ID;
                    JC.Custom_KDM = context.CustomKDMSet.FirstOrDefault(p => p.KDM_ID == _KDMID);
                    int Title = Convert.ToInt32(JCid);
                    JC.Lkp_Custom_Title = context.CustomTitleSet.FirstOrDefault(s => s.Title_ID == Title);
                    context.AddToCustomKDMTitleSet(JC);
                }


            }


            if (!String.IsNullOrEmpty(hdnCredentialsOutcome.Value.ToString()))
            {

                if (hdnCredentialsOutcome.Value.ToString().IndexOf(",") > 0)
                {
                    //Split the data by comma 
                    string[] CDids = hdnCredentialsOutcome.Value.ToString().Split(new Char[] { ',' });
                    foreach (string ids in CDids)
                    {
                        int CDIDs = Convert.ToInt32(ids);
                        Custom_KDM_Credential JC = new Custom_KDM_Credential();
                        int _KDMID = customKDM.KDM_ID;
                        JC.Custom_KDM = context.CustomKDMSet.FirstOrDefault(p => p.KDM_ID == _KDMID);
                        int Credentials = Convert.ToInt32(CDIDs);
                        JC.Lkp_Custom_Credentials = context.CustomCredentialsSet.FirstOrDefault(s => s.Credentials_ID == Credentials);
                        context.AddToCustomKDMCredentialSet(JC);
                    }

                }
                else
                {
                    string CDid = hdnCredentialsOutcome.Value.ToString();
                    Custom_KDM_Credential JC = new Custom_KDM_Credential();
                    int _KDMID = customKDM.KDM_ID;
                    JC.Custom_KDM = context.CustomKDMSet.FirstOrDefault(p => p.KDM_ID == _KDMID);
                    int Credentials = Convert.ToInt32(CDid);
                    JC.Lkp_Custom_Credentials = context.CustomCredentialsSet.FirstOrDefault(s => s.Credentials_ID == Credentials);
                    context.AddToCustomKDMCredentialSet(JC);
                }


            }


            if (!String.IsNullOrEmpty(hdnCACOutcome.Value.ToString()))
            {

                if (hdnCACOutcome.Value.ToString().IndexOf(",") > 0)
                {
                    //Split the data by comma 
                    string[] CACids = hdnCACOutcome.Value.ToString().Split(new Char[] { ',' });
                    foreach (string ids in CACids)
                    {
                        int CACIDs = Convert.ToInt32(ids);
                        Custom_KDM_CAC CA = new Custom_KDM_CAC();
                        int _KDMID = customKDM.KDM_ID;
                        CA.Custom_KDM = context.CustomKDMSet.FirstOrDefault(p => p.KDM_ID == _KDMID);
                        int CAC = Convert.ToInt32(CACIDs);
                        CA.Lkp_Custom_CAC = context.CustomCACSet.FirstOrDefault(s => s.CAC_ID == CAC);
                        context.AddToCustomKDMCACSet(CA);
                    }

                }
                else
                {
                    string CACid = hdnCACOutcome.Value.ToString();
                    Custom_KDM_CAC CA = new Custom_KDM_CAC();
                    int _KDMID = customKDM.KDM_ID;
                    CA.Custom_KDM = context.CustomKDMSet.FirstOrDefault(p => p.KDM_ID == _KDMID);
                    int CAC = Convert.ToInt32(CACid);
                    CA.Lkp_Custom_CAC = context.CustomCACSet.FirstOrDefault(s => s.CAC_ID == CAC);
                    context.AddToCustomKDMCACSet(CA);
                }


            }

            context.SaveChanges();
            Page.ClientScript.RegisterStartupScript(typeof(Page), "RefreshPlanInfo", "RefreshPlanInfo();", true);
            this.Msglbl.Text = msg;
            this.Msglbl.Visible = true;
            formKDMView.Visible = false;
        }
    }
}
