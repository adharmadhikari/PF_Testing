using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections;
using Pinsonault.Application.Millennium;

public partial class custom_millennium_todaysaccounts_all_AddEditKDMAddressSS : PageBase
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
        titleText.Text = String.Format("{0} Address for Key Decision Maker", Request.QueryString["LinkClicked"].Replace("KDM", ""));
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

            if (DeletePlan(System.Convert.ToInt32(Page.Request.QueryString["KDM_ADD_ID"])))
            {

                this.Msglbl.Text = "<br/><b>Selected Address has been deleted successfully.</b><br/><br/>";
                this.Msglbl.Visible = true;
                formKDMView.Visible = false;
                Page.ClientScript.RegisterStartupScript(typeof(Page), "RefreshPlanInfo", "RefreshPlanInfo();", true);
            }

        }
    }


    public bool DeletePlan(int KDM_ADD_ID)
    {
        int status_False = 0;

        if (KDM_ADD_ID != 0)
        {
            using (PathfinderMillenniumEntities context = new PathfinderMillenniumEntities())
            {


                Custom_KDM_Address customKDMAddress;
                customKDMAddress = context.Custom_KDM_Address.FirstOrDefault(p => p.ID == KDM_ADD_ID);
                customKDMAddress.status = Convert.ToBoolean(status_False);
                context.SaveChanges();

            }

            return true;
        }

        return false;


    }

    protected void ProcessRequest(string BtnClicked)
    {
        //Page.ClientScript.RegisterStartupScript(typeof(Page), "UpdCustomTitle", "updCustomTitleChkSelection();", true);
        //Page.ClientScript.RegisterStartupScript(typeof(Page), "UpdCredentialsTitle", "updCredentialsChkSelection();", true);

        string msg = "";
        string strPlanID = System.Web.HttpContext.Current.Request.QueryString["PlanID"];
        int Plan_ID = 0;
        int status_True = 1;
        int is_primary = 1;
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
        string KDMADDID = System.Web.HttpContext.Current.Request.QueryString["KDM_ADD_ID"];
        int KDM_ADD_ID = 0;
        if (KDMADDID != null && KDMADDID != "")
        {
            KDM_ADD_ID = System.Convert.ToInt32(KDMADDID);
        }

        using (PathfinderMillenniumEntities context = new PathfinderMillenniumEntities())
        {

            Custom_KDM_Address customKDMAddress;
            if (KDM_ADD_ID != 0)
            {
                customKDMAddress = new Custom_KDM_Address();
                msg = "<br/><b>Address has been updated successfully.</b><br/><br/>";
                customKDMAddress = context.Custom_KDM_Address.FirstOrDefault(p => p.ID == KDM_ADD_ID);
                customKDMAddress.Custom_KDM = context.CustomKDMSet.FirstOrDefault(p => p.KDM_ID == KDM_ID);
                customKDMAddress.KDM_Address1 = ((System.Web.UI.WebControls.TextBox)(this.formKDMView.FindControl("Address1txt"))).Text;
                customKDMAddress.KDM_Address2 = ((System.Web.UI.WebControls.TextBox)(this.formKDMView.FindControl("Address2txt"))).Text;
                customKDMAddress.KDM_City = ((System.Web.UI.WebControls.TextBox)(this.formKDMView.FindControl("Citytxt"))).Text;

                if (!string.IsNullOrEmpty(((Telerik.Web.UI.RadComboBox)(this.formKDMView.FindControl("rdlState"))).SelectedValue))
                    customKDMAddress.KDM_State = ((Telerik.Web.UI.RadComboBox)(this.formKDMView.FindControl("rdlState"))).SelectedValue;
                else
                    customKDMAddress.KDM_State = null;

                customKDMAddress.KDM_Zip = ((System.Web.UI.WebControls.TextBox)(this.formKDMView.FindControl("Ziptxt"))).Text;
                customKDMAddress.KDM_Zip_4 = ((System.Web.UI.WebControls.TextBox)(this.formKDMView.FindControl("Zip4txt"))).Text;
                //customKDMAddress.Is_Primary_Addr = Convert.ToBoolean(is_primary);
                customKDMAddress.status = Convert.ToBoolean(status_True);

            }
            else
            {
                customKDMAddress = new Custom_KDM_Address();

                msg = "<br/><b>Address has been added successfully.</b><br/><br/>";
                customKDMAddress.Custom_KDM = context.CustomKDMSet.FirstOrDefault(p => p.KDM_ID == KDM_ID);
                customKDMAddress.KDM_Address1 = ((System.Web.UI.WebControls.TextBox)(this.formKDMView.FindControl("Address1txt"))).Text;
                customKDMAddress.KDM_Address2 = ((System.Web.UI.WebControls.TextBox)(this.formKDMView.FindControl("Address2txt"))).Text;
                customKDMAddress.KDM_City = ((System.Web.UI.WebControls.TextBox)(this.formKDMView.FindControl("Citytxt"))).Text;

                if (!string.IsNullOrEmpty(((Telerik.Web.UI.RadComboBox)(this.formKDMView.FindControl("rdlState"))).SelectedValue))
                    customKDMAddress.KDM_State = ((Telerik.Web.UI.RadComboBox)(this.formKDMView.FindControl("rdlState"))).SelectedValue;
                else
                    customKDMAddress.KDM_State = null;

                customKDMAddress.KDM_Zip = ((System.Web.UI.WebControls.TextBox)(this.formKDMView.FindControl("Ziptxt"))).Text;
                customKDMAddress.KDM_Zip_4 = ((System.Web.UI.WebControls.TextBox)(this.formKDMView.FindControl("Zip4txt"))).Text;
                //customKDMAddress.Is_Primary_Addr = Convert.ToBoolean(is_primary);
                customKDMAddress.status = Convert.ToBoolean(status_True);
                context.AddToCustom_KDM_Address(customKDMAddress);

            }
            context.SaveChanges();



            context.SaveChanges();
            Page.ClientScript.RegisterStartupScript(typeof(Page), "RefreshPlanInfo", "RefreshPlanInfo();", true);
            this.Msglbl.Text = msg;
            this.Msglbl.Visible = true;
            formKDMView.Visible = false;
        }
    }
}
