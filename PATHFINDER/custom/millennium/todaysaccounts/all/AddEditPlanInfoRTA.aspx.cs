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
using System.Data.SqlClient;
using System.Configuration;
using System.Data;


public partial class custom_millennium_todaysaccounts_all_AddEditPlanInfo : PageBase
{
    protected void Page_Init(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
           
            this.formViewPlanInfo.Visible = true;
          
        }
        if ( Request.QueryString["LinkClicked"] == "AddPlan" )
        {
            this.formViewPlanInfo.ChangeMode(FormViewMode.Insert);           
        }
                          
                
    }
    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!Page.IsPostBack)
        {
            
            this.Msglbl.Visible = false;
           
            titleText.Text = String.Format("{0} Plan - {1}", Request.QueryString["LinkClicked"].Replace("Plan", ""), Request.QueryString["PlanName"]);        
        }
        
    }
   
    protected void ProcessRequest(string BtnClicked)
    {
        Page.ClientScript.RegisterStartupScript(typeof(Page), "UpdateState", "UpdStatesCovered();", true);
        
        string msg = "";
        string strPlanID = System.Web.HttpContext.Current.Request.QueryString["PlanID"];
        int Plan_ID = 0;
        if (strPlanID != null && strPlanID != "")
        {
            Plan_ID = System.Convert.ToInt32(strPlanID);
        }
        using (PathfinderMillenniumEntities context = new PathfinderMillenniumEntities())
        {
            PlansClient customplan;
            if (Plan_ID != 0)
            {
                customplan = context.PlansClientSet.FirstOrDefault(p => p.Plan_ID == Plan_ID);
                msg = "<br/><b>Plan Information has been updated successfully.</b><br/><br/>";
                customplan.Plan_Name = ((System.Web.UI.WebControls.TextBox)(this.formViewPlanInfo.FindControl("txtOrganizationName"))).Text;
                customplan.Address1 = ((System.Web.UI.WebControls.TextBox)(this.formViewPlanInfo.FindControl("TxtAddress1"))).Text;
                customplan.Address2 = ((System.Web.UI.WebControls.TextBox)(this.formViewPlanInfo.FindControl("TxtAddress2"))).Text;
                //customplan.Website = ((System.Web.UI.WebControls.TextBox)(this.formViewPlanInfo.FindControl("TxtWebsite"))).Text;
                customplan.City = ((System.Web.UI.WebControls.TextBox)(this.formViewPlanInfo.FindControl("TxtCity"))).Text;
                customplan.Zip = ((System.Web.UI.WebControls.TextBox)(this.formViewPlanInfo.FindControl("TxtZip"))).Text;
                customplan.Zip_4 = ((System.Web.UI.WebControls.TextBox)(this.formViewPlanInfo.FindControl("TxtZip4"))).Text;
                customplan.Phone = ((System.Web.UI.WebControls.TextBox)(this.formViewPlanInfo.FindControl("TxtPhone"))).Text;
                customplan.fax = ((System.Web.UI.WebControls.TextBox)(this.formViewPlanInfo.FindControl("TxtFax"))).Text;
                
                if (!string.IsNullOrEmpty(((Telerik.Web.UI.RadComboBox)(this.formViewPlanInfo.FindControl("rdlRevlimid"))).SelectedValue))
                    customplan.Revlimid = Convert.ToBoolean(Convert.ToInt32(((Telerik.Web.UI.RadComboBox)(this.formViewPlanInfo.FindControl("rdlRevlimid"))).SelectedValue));
                else
                    customplan.Revlimid = null;

                if (!string.IsNullOrEmpty(((Telerik.Web.UI.RadComboBox)(this.formViewPlanInfo.FindControl("rdlInOfficeDispensing"))).SelectedValue))
                    customplan.In_Office_Dispensing = Convert.ToBoolean(Convert.ToInt32(((Telerik.Web.UI.RadComboBox)(this.formViewPlanInfo.FindControl("rdlInOfficeDispensing"))).SelectedValue));
                else
                    customplan.In_Office_Dispensing= null;
                if (!string.IsNullOrEmpty(((Telerik.Web.UI.RadComboBox)(this.formViewPlanInfo.FindControl("rdlState"))).SelectedValue))
                    customplan.State = ((Telerik.Web.UI.RadComboBox)(this.formViewPlanInfo.FindControl("rdlState"))).SelectedValue;
                else
                    customplan.State = null;
                if (!string.IsNullOrEmpty(((Telerik.Web.UI.RadComboBox)(this.formViewPlanInfo.FindControl("rdlclassoftrade"))).SelectedValue))
                    customplan.Class_of_Trade_ID = Convert.ToInt32(((Telerik.Web.UI.RadComboBox)(this.formViewPlanInfo.FindControl("rdlclassoftrade"))).SelectedValue);
                else
                    customplan.Class_of_Trade_ID = null;
                if (!string.IsNullOrEmpty(((Telerik.Web.UI.RadComboBox)(this.formViewPlanInfo.FindControl("rdlPrimaryNetwork"))).SelectedValue))
                    customplan.Primary_Network_id = Convert.ToInt32(((Telerik.Web.UI.RadComboBox)(this.formViewPlanInfo.FindControl("rdlPrimaryNetwork"))).SelectedValue);
                else
                    customplan.Primary_Network_id = null;
                customplan.Client_ID = Pinsonault.Web.Session.ClientID;
                customplan.Section_ID = 108;
                customplan.Modified_DT = DateTime.UtcNow;
                customplan.Modified_BY = Pinsonault.Web.Session.FullName;
                customplan.Status = true;
                context.SaveChanges();

            }
            else
            {
                customplan = new PlansClient();
               // customplan = context.PlansClientSet.FirstOrDefault(p => p.Plan_ID == Plan_ID);
                customplan.Created_DT = DateTime.UtcNow;
                customplan.Created_BY = Pinsonault.Web.Session.FullName;
                msg = "<br/><b>Plan Information has been added successfully.</b><br/><br/>";
                customplan.Plan_Name = ((System.Web.UI.WebControls.TextBox)(this.formViewPlanInfo.FindControl("txtOrganizationName"))).Text;
                customplan.Address1 = ((System.Web.UI.WebControls.TextBox)(this.formViewPlanInfo.FindControl("TxtAddress1"))).Text;
                customplan.Address2 = ((System.Web.UI.WebControls.TextBox)(this.formViewPlanInfo.FindControl("TxtAddress2"))).Text;
               // customplan.Website = ((System.Web.UI.WebControls.TextBox)(this.formViewPlanInfo.FindControl("TxtWebsite"))).Text;
                customplan.City = ((System.Web.UI.WebControls.TextBox)(this.formViewPlanInfo.FindControl("TxtCity"))).Text;
                customplan.Zip = ((System.Web.UI.WebControls.TextBox)(this.formViewPlanInfo.FindControl("TxtZip"))).Text;
                customplan.Zip_4 = ((System.Web.UI.WebControls.TextBox)(this.formViewPlanInfo.FindControl("TxtZip4"))).Text;
                customplan.Phone = ((System.Web.UI.WebControls.TextBox)(this.formViewPlanInfo.FindControl("TxtPhone"))).Text;
                customplan.fax = ((System.Web.UI.WebControls.TextBox)(this.formViewPlanInfo.FindControl("TxtFax"))).Text;

                if (!string.IsNullOrEmpty(((Telerik.Web.UI.RadComboBox)(this.formViewPlanInfo.FindControl("rdlRevlimid"))).SelectedValue))
                    customplan.Revlimid = Convert.ToBoolean(Convert.ToInt32(((Telerik.Web.UI.RadComboBox)(this.formViewPlanInfo.FindControl("rdlRevlimid"))).SelectedValue));
                else
                    customplan.Revlimid = null;
                if (!string.IsNullOrEmpty(((Telerik.Web.UI.RadComboBox)(this.formViewPlanInfo.FindControl("rdlInOfficeDispensing"))).SelectedValue))
                    customplan.In_Office_Dispensing = Convert.ToBoolean(Convert.ToInt32(((Telerik.Web.UI.RadComboBox)(this.formViewPlanInfo.FindControl("rdlInOfficeDispensing"))).SelectedValue));
                else
                    customplan.In_Office_Dispensing = null;
                if (!string.IsNullOrEmpty(((Telerik.Web.UI.RadComboBox)(this.formViewPlanInfo.FindControl("rdlState"))).SelectedValue))
                    customplan.State = ((Telerik.Web.UI.RadComboBox)(this.formViewPlanInfo.FindControl("rdlState"))).SelectedValue;
                else
                    customplan.State = null;
                if (!string.IsNullOrEmpty(((Telerik.Web.UI.RadComboBox)(this.formViewPlanInfo.FindControl("rdlState"))).SelectedValue))
                    customplan.State = ((Telerik.Web.UI.RadComboBox)(this.formViewPlanInfo.FindControl("rdlState"))).SelectedValue;
                else
                    customplan.State = null;
                if (!string.IsNullOrEmpty(((Telerik.Web.UI.RadComboBox)(this.formViewPlanInfo.FindControl("rdlclassoftrade"))).SelectedValue))
                    customplan.Class_of_Trade_ID = Convert.ToInt32(((Telerik.Web.UI.RadComboBox)(this.formViewPlanInfo.FindControl("rdlclassoftrade"))).SelectedValue);
                else
                    customplan.Class_of_Trade_ID = null;
                if (!string.IsNullOrEmpty(((Telerik.Web.UI.RadComboBox)(this.formViewPlanInfo.FindControl("rdlPrimaryNetwork"))).SelectedValue))
                    customplan.Primary_Network_id = Convert.ToInt32(((Telerik.Web.UI.RadComboBox)(this.formViewPlanInfo.FindControl("rdlPrimaryNetwork"))).SelectedValue);
                else
                    customplan.Primary_Network_id = null;
                customplan.Modified_DT = DateTime.UtcNow;
                customplan.Modified_BY = Pinsonault.Web.Session.FullName;
                customplan.Client_ID = Pinsonault.Web.Session.ClientID;
                customplan.Section_ID = 108;
                customplan.Status = true;
                context.AddToPlansClientSet(customplan);
                context.SaveChanges();

                // sl 2/26/2013 to save user-Territory/Plan mapping 
                int newPlanID = 0;
                newPlanID = customplan.Plan_ID;
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MillenniumDB"].ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("usp_AddUserTerritoryPlanMapping", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ClientID", Pinsonault.Web.Session.ClientID);
                    cmd.Parameters.AddWithValue("@UserID", Pinsonault.Web.Session.UserID);
                    cmd.Parameters.AddWithValue("@PlanID", newPlanID);

                    cmd.ExecuteScalar();

                }


            }
            //context.SaveChanges();
      
        }
        Page.ClientScript.RegisterStartupScript(typeof(Page), "RefreshPlanInfo", "RefreshPlanInfo();", true);
        this.Msglbl.Text = msg;
        this.Msglbl.Visible = true;
        formViewPlanInfo.Visible = false;
    }
    protected void Addbtn_Click(object sender, EventArgs e)
    {
        ProcessRequest("Add");
    }
    protected void Editbtn_Click(object sender, EventArgs e)
    {
         ProcessRequest("Edit");
    }
   
}
