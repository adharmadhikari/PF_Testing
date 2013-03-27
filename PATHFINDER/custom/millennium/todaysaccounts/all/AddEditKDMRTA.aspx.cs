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

public partial class custom_millennium_todaysaccounts_all_AddEditKDMRTA : PageBase
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
          Page.ClientScript.RegisterStartupScript(typeof(Page), "UpdCustomSpecialty", "updCustomSpecialtyChkSelection();", true);
          Page.ClientScript.RegisterStartupScript(typeof(Page), "UpdCustomJobFunction", "updCustomJobFunctionChkSelection();", true);             
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
                
                var TitleRTA = (from j in context.CustomKDMTitleSet
                                where j.KDM_ID == KDM_ID
                            select j).ToList().Select(j => string.Format("{0}", j.Title_ID.ToString()));
                //Comma separate individual record's data.
                hdnMeetOutcome.Value = string.Join(",", TitleRTA.ToArray());



                var CredentialsRTA = (from j in context.CustomKDMCredentialSet
                                where j.KDM_ID == KDM_ID
                                select j).ToList().Select(j => string.Format("{0}", j.Credentials_ID.ToString()));
                //Comma separate individual record's data.
                hdnCredentialsOutcome.Value = string.Join(",", CredentialsRTA.ToArray());


                var Specialty = (from j in context.CustomKDMSpecialtySet
                                 where j.KDM_ID == KDM_ID
                                 select j).ToList().Select(j => string.Format("{0}", j.Specialty_ID.ToString()));
                //Comma separate individual record's data.
                hdnSpecialty.Value = string.Join(",", Specialty.ToArray());



                var JobFunction = (from j in context.CustomKDMJobFunctionSet
                                   where j.KDM_ID == KDM_ID
                                   select j).ToList().Select(j => string.Format("{0}", j.Job_Function_ID.ToString()));
                //Comma separate individual record's data.
                hdnJobFunction.Value = string.Join(",", JobFunction.ToArray());
            }
        }
        else { hdnMeetOutcome.Value = ""; hdnCredentialsOutcome.Value = ""; }
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
        Page.ClientScript.RegisterStartupScript(typeof(Page), "UpdCustomSpecialty", "updCustomSpecialtyChkSelection();", true);
        Page.ClientScript.RegisterStartupScript(typeof(Page), "UpdCustomJobFunction", "updCustomJobFunctionChkSelection();", true); 

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
             
            if (KDM_ID!=0)
            {
                msg = "<br/><b>Key Decision Maker Information has been updated successfully.</b><br/><br/>";
                customKDM = context.CustomKDMSet.FirstOrDefault(p => p.KDM_ID == KDM_ID);
                customKDM.Plans_Client = context.PlansClientSet.FirstOrDefault(p => p.Plan_ID == Plan_ID);
                customKDM.KDM_F_Name = ((System.Web.UI.WebControls.TextBox)(this.formKDMView.FindControl("FNametxt"))).Text;
                customKDM.KDM_L_Name = ((System.Web.UI.WebControls.TextBox)(this.formKDMView.FindControl("LNametxt"))).Text;
                customKDM.KDM_Email = ((System.Web.UI.WebControls.TextBox)(this.formKDMView.FindControl("Emailtxt"))).Text;
                customKDM.KDM_Phone = ((System.Web.UI.WebControls.TextBox)(this.formKDMView.FindControl("Phtxt"))).Text;
                customKDM.KDM_Fax = ((System.Web.UI.WebControls.TextBox)(this.formKDMView.FindControl("Faxtxt"))).Text;
                if (!string.IsNullOrEmpty(((Telerik.Web.UI.RadComboBox)(this.formKDMView.FindControl("rdlRTA_Affiliation"))).SelectedValue))
                    customKDM.RTA_Affiliation = ((Telerik.Web.UI.RadComboBox)(this.formKDMView.FindControl("rdlRTA_Affiliation"))).SelectedValue;
                else
                    customKDM.RTA_Affiliation = null;
                customKDM.Status = 1;
                
            }
            else{
                customKDM = new Custom_KDM();
                
                msg = "<br/><b>Key Decision Maker Information has been added successfully.</b><br/><br/>";
               // customKDM = context.CustomKDMSet.FirstOrDefault(p => p.KDM_ID == KDM_ID);
                customKDM.Plans_Client = context.PlansClientSet.FirstOrDefault(p => p.Plan_ID == Plan_ID);  
                customKDM.KDM_F_Name = ((System.Web.UI.WebControls.TextBox)(this.formKDMView.FindControl("FNametxt"))).Text;
                customKDM.KDM_L_Name = ((System.Web.UI.WebControls.TextBox)(this.formKDMView.FindControl("LNametxt"))).Text;
                customKDM.KDM_Email = ((System.Web.UI.WebControls.TextBox)(this.formKDMView.FindControl("Emailtxt"))).Text;
                customKDM.KDM_Phone = ((System.Web.UI.WebControls.TextBox)(this.formKDMView.FindControl("Phtxt"))).Text;
                customKDM.KDM_Fax = ((System.Web.UI.WebControls.TextBox)(this.formKDMView.FindControl("Faxtxt"))).Text;
                if (!string.IsNullOrEmpty(((Telerik.Web.UI.RadComboBox)(this.formKDMView.FindControl("rdlRTA_Affiliation"))).SelectedValue))
                    customKDM.RTA_Affiliation = ((Telerik.Web.UI.RadComboBox)(this.formKDMView.FindControl("rdlRTA_Affiliation"))).SelectedValue;
                else
                    customKDM.RTA_Affiliation = null;
                customKDM.Status = 1;
                context.AddToCustomKDMSet(customKDM);

            }
            context.SaveChanges();

            var titleRTA = from t in context.CustomKDMTitleSet
                          where t.KDM_ID == KDM_ID
                          select t;
            foreach (var plan in titleRTA) context.DeleteObject(plan);

            var cdRTA = from c in context.CustomKDMCredentialSet
                       where c.KDM_ID == KDM_ID
                       select c;
            foreach (var plan in cdRTA) context.DeleteObject(plan);

            var sp = from s in context.CustomKDMSpecialtySet
                     where s.KDM_ID == KDM_ID
                     select s;
            foreach (var plan in sp) context.DeleteObject(plan);

            var jb = from j in context.CustomKDMJobFunctionSet
                     where j.KDM_ID == KDM_ID
                     select j;
            foreach (var plan in jb) context.DeleteObject(plan);

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


                if (!String.IsNullOrEmpty(hdnSpecialty.Value.ToString()))
                {

                    if (hdnSpecialty.Value.ToString().IndexOf(",") > 0)
                    {
                        //Split the data by comma 
                        string[] SPids = hdnSpecialty.Value.ToString().Split(new Char[] { ',' });
                        foreach (string ids in SPids)
                        {
                            int SPIDs = Convert.ToInt32(ids);
                            Custom_KDM_Specialty SPE = new Custom_KDM_Specialty();
                            int _KDMID = customKDM.KDM_ID;
                            SPE.Custom_KDM = context.CustomKDMSet.FirstOrDefault(p => p.KDM_ID == _KDMID);
                            int Specialty = Convert.ToInt32(SPIDs);
                            SPE.Lkp_Custom_Specialty = context.CustomSpecialtySet.FirstOrDefault(s => s.Specialty_ID == Specialty);
                            context.AddToCustomKDMSpecialtySet(SPE);
                        }

                    }
                    else
                    {
                        string SPid = hdnSpecialty.Value.ToString();
                        Custom_KDM_Specialty SPE = new Custom_KDM_Specialty();
                        int _KDMID = customKDM.KDM_ID;
                        SPE.Custom_KDM = context.CustomKDMSet.FirstOrDefault(p => p.KDM_ID == _KDMID);
                        int Specialty = Convert.ToInt32(SPid);
                        SPE.Lkp_Custom_Specialty = context.CustomSpecialtySet.FirstOrDefault(s => s.Specialty_ID == Specialty);
                        context.AddToCustomKDMSpecialtySet(SPE);
                    }


                }



                if (!String.IsNullOrEmpty(hdnSpecialty.Value.ToString()))
                {

                    if (hdnJobFunction.Value.ToString().IndexOf(",") > 0)
                    {
                        //Split the data by comma 
                        string[] JBids = hdnJobFunction.Value.ToString().Split(new Char[] { ',' });
                        foreach (string ids in JBids)
                        {
                            int JBIDs = Convert.ToInt32(ids);
                            Custom_KDM_Job_Function JBF = new Custom_KDM_Job_Function();
                            int _KDMID = customKDM.KDM_ID;
                            JBF.Custom_KDM = context.CustomKDMSet.FirstOrDefault(p => p.KDM_ID == _KDMID);
                            int JobFunction = Convert.ToInt32(JBIDs);
                            JBF.Lkp_Custom_Job_Function = context.CustomJobFunctionSet.FirstOrDefault(s => s.Job_Function_ID == JobFunction);
                            context.AddToCustomKDMJobFunctionSet(JBF);
                        }

                    }
                    else
                    {
                        string JBid = hdnJobFunction.Value.ToString();
                        Custom_KDM_Job_Function JBF = new Custom_KDM_Job_Function();
                        int _KDMID = customKDM.KDM_ID;
                        JBF.Custom_KDM = context.CustomKDMSet.FirstOrDefault(p => p.KDM_ID == _KDMID);
                        int JobFunction = Convert.ToInt32(JBid);
                        JBF.Lkp_Custom_Job_Function = context.CustomJobFunctionSet.FirstOrDefault(s => s.Job_Function_ID == JobFunction);
                        context.AddToCustomKDMJobFunctionSet(JBF);
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
