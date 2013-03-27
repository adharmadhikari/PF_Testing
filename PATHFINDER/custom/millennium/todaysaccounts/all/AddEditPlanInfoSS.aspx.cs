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
            setComboValues();
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
            //setComboValues();
            Page.ClientScript.RegisterStartupScript(typeof(Page), "UpdateState", "UpdStatesCovered();", true);
            
            
            titleText.Text = String.Format("{0} Plan - {1}", Request.QueryString["LinkClicked"].Replace("Plan", ""), Request.QueryString["PlanName"]);        
        }
        
    }
    protected void setComboValues()
    {
        string strPlanID = System.Web.HttpContext.Current.Request.QueryString["PlanID"];
        int Plan_ID = 0;


        if (strPlanID != null && strPlanID != "")
        {
            Plan_ID = System.Convert.ToInt32(strPlanID);
        }
        if (Plan_ID != 0)
        {
            using (PathfinderMillenniumEntities context = new PathfinderMillenniumEntities())
            {
                var ssstate = (from s in context.CustomPlanStateSet
                               where s.Plan_ID == Plan_ID
                               select s).ToList().Select(s => string.Format("{0}", s.State_ID.ToString()));
                //Comma separate individual record's data.
                hdnStatesCovered.Value = string.Join(",", ssstate.ToArray());
               
            }
        }
        else { hdnStatesCovered.Value = "";  }
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
                customplan.Website = ((System.Web.UI.WebControls.TextBox)(this.formViewPlanInfo.FindControl("TxtWebsite"))).Text;
                customplan.City = ((System.Web.UI.WebControls.TextBox)(this.formViewPlanInfo.FindControl("TxtCity"))).Text;
                customplan.Zip = ((System.Web.UI.WebControls.TextBox)(this.formViewPlanInfo.FindControl("TxtZip"))).Text;
                customplan.Zip_4 = ((System.Web.UI.WebControls.TextBox)(this.formViewPlanInfo.FindControl("TxtZip4"))).Text;
                
                if (!string.IsNullOrEmpty(((Telerik.Web.UI.RadComboBox)(this.formViewPlanInfo.FindControl("rdlState"))).SelectedValue))
                    customplan.State = ((Telerik.Web.UI.RadComboBox)(this.formViewPlanInfo.FindControl("rdlState"))).SelectedValue;
                else
                    customplan.State = null;
                customplan.Client_ID = Pinsonault.Web.Session.ClientID;
                customplan.Section_ID = 106;
                customplan.Modified_DT = DateTime.UtcNow;
                customplan.Modified_BY = Pinsonault.Web.Session.FullName;
                customplan.Status = true;
                context.SaveChanges();
            }
            else
            {
                customplan = new PlansClient();
                //customplan = context.PlansClientSet.FirstOrDefault(p => p.Plan_ID == Plan_ID);
                customplan.Created_DT = DateTime.UtcNow;
                customplan.Created_BY = Pinsonault.Web.Session.FullName;
                msg = "<br/><b>Plan Information has been added successfully.</b><br/><br/>";
                customplan.Plan_Name = ((System.Web.UI.WebControls.TextBox)(this.formViewPlanInfo.FindControl("txtOrganizationName"))).Text;
                customplan.Address1 = ((System.Web.UI.WebControls.TextBox)(this.formViewPlanInfo.FindControl("TxtAddress1"))).Text;
                customplan.Address2 = ((System.Web.UI.WebControls.TextBox)(this.formViewPlanInfo.FindControl("TxtAddress2"))).Text;
                customplan.Website = ((System.Web.UI.WebControls.TextBox)(this.formViewPlanInfo.FindControl("TxtWebsite"))).Text;
                customplan.City = ((System.Web.UI.WebControls.TextBox)(this.formViewPlanInfo.FindControl("TxtCity"))).Text;
                customplan.Zip = ((System.Web.UI.WebControls.TextBox)(this.formViewPlanInfo.FindControl("TxtZip"))).Text;
                customplan.Zip_4 = ((System.Web.UI.WebControls.TextBox)(this.formViewPlanInfo.FindControl("TxtZip4"))).Text;
               
                if (!string.IsNullOrEmpty(((Telerik.Web.UI.RadComboBox)(this.formViewPlanInfo.FindControl("rdlState"))).SelectedValue))
                    customplan.State = ((Telerik.Web.UI.RadComboBox)(this.formViewPlanInfo.FindControl("rdlState"))).SelectedValue;
                else
                    customplan.State = null;
                customplan.Modified_DT = DateTime.UtcNow;
                customplan.Modified_BY = Pinsonault.Web.Session.FullName;
                customplan.Client_ID = Pinsonault.Web.Session.ClientID;
                customplan.Section_ID = 106;
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

            var ssstate = from s in context.CustomPlanStateSet
                          where s.Plan_ID == Plan_ID
                          select s;
            foreach (var plan in ssstate) context.DeleteObject(plan);
            
            context.SaveChanges();

            if (!String.IsNullOrEmpty(hdnStatesCovered.Value.ToString()))
            {

                if (hdnStatesCovered.Value.ToString().IndexOf(",") > 0)
                {
                    //Split the data by comma 
                    string[] proDisids = hdnStatesCovered.Value.ToString().Split(new Char[] { ',' });
                    foreach (string ids in proDisids)
                    {
                        int proID = Convert.ToInt32(ids);
                        CustomPlanState ST = new CustomPlanState();
                        int PlanID = customplan.Plan_ID;
                        ST.Plans_Client = context.PlansClientSet.FirstOrDefault(p => p.Plan_ID == PlanID);
                        ST.State_ID = Convert.ToInt32(proID);
                        context.AddToCustomPlanStateSet(ST);
                    }

                }
                else
                {
                    string proDisid = hdnStatesCovered.Value.ToString();
                    CustomPlanState ST = new CustomPlanState();
                    int PlanID = customplan.Plan_ID;
                    ST.Plans_Client = context.PlansClientSet.FirstOrDefault(p => p.Plan_ID == PlanID);
                    ST.State_ID = Convert.ToInt32(proDisid);
                    context.AddToCustomPlanStateSet(ST);

                }
               
            }
            context.SaveChanges();
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
