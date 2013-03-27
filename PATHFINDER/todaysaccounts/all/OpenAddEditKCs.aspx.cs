using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Telerik.Web.UI;

public partial class todaysaccounts_all_OpenAddEditKCs : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.formViewKC.Visible = true;
            this.Msglbl.Visible = false;
            this.CloseMsglbl.Visible = false;

            //Updates Plan Name hidden variable based on Querystring.
            this.PlanNameHdn.Value = System.Web.HttpContext.Current.Request.QueryString["PlanName"];

            //Depending on actions(View/Add/Update)it changes form mode to ReadOnly/Edit/Insert.
            //Also, sets the form header with PlanName included.
            if ( HttpContext.Current.Request["LinkClicked"] == "ViewKC" || !Context.User.IsInRole("editcontacts") )
            {
                //if (Context.User.IsInRole("editcontacts"))
                //{
                //    this.formViewKC.ChangeMode(FormViewMode.Edit);
                //    this.titleText.Text = this.PlanNameHdn.Value + " - " + this.titleText.Text + " Update";
                //}
                //else
                //{
                    this.formViewKC.ChangeMode(FormViewMode.ReadOnly);
                    this.titleText.Text = this.PlanNameHdn.Value + " - " + this.titleText.Text;
                //}

                //If user is not in 'editcontacts' role, hide the data and display message
                if (!Context.User.IsInRole("editcontacts"))
                {
                    this.formViewKC.Visible = false;
                    this.titleText.Text = "Permission not available";
                    this.Msglbl.Text = "Permission to send a new Key Contact is not available.";
                    this.Msglbl.Visible = true;
                    this.permission.Visible = false;
                }
            }
            else if (HttpContext.Current.Request["LinkClicked"] == "AddKC")
            {
                this.formViewKC.ChangeMode(FormViewMode.Insert);
                this.titleText.Text = this.PlanNameHdn.Value + " - Add " + this.titleText.Text ;
                printLink.Visible = false;
            }
            else 
            {
                this.formViewKC.ChangeMode(FormViewMode.Edit);
                this.titleText.Text = this.PlanNameHdn.Value + " - " + this.titleText.Text + " Update";
            }
        }
        else
        {
            if (string.IsNullOrEmpty(this.PlanNameHdn.Value))
            {
                //Updates Plan Name hidden variable based on Querystring.
                this.PlanNameHdn.Value = System.Web.HttpContext.Current.Request.QueryString["PlanName"];
            }
        }
    }

    //Gets form data, builds email message and sends email to customer support group.
    //param BtnClicked: New/Update/Delete
    protected void ProcessRequest(string BtnClicked) 
    {
        string strBody = GetData(BtnClicked);
        string strFrom = Pinsonault.Web.Support.UserEmail;
        string strTo = Pinsonault.Web.Support.CustomerSupportEmail;
        string strSub =  string.Format(Resources.Resource.Subject_Submit_Key_Contact_Update, this.PlanNameHdn.Value , BtnClicked);

        Pinsonault.Web.Support.SendEmail(strFrom, strTo, strSub, strBody, false);

        //Confirmation message is displayed after sending an email.
        this.formViewKC.Visible = false;
        this.Msglbl.Text = "<br/><b>Email has been sent to the customer support group.</b><br/><br/>";
        this.Msglbl.Visible = true;
        this.CloseMsglbl.Visible = true;

        //Calls Javascript function RefreshMyKCs() to refresh my key contacts parent grid.
        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "ConfirmMsg", "ConfirmMsg();", true);
    }

    protected void Addbtn_Click(object sender, EventArgs e)
    {
        ProcessRequest("New");
    }

    protected void SendUpdbtn_Click(object sender, EventArgs e)
    {
        ProcessRequest("Update");
    }

    protected void SendDelbtn_Click(object sender, EventArgs e)
    {
        ProcessRequest("Delete");
    }

    protected void SendUpdViewbtn_Click(object sender, EventArgs e)
    {
        string Urlstr = string.Format("./OpenAddEditKCs.aspx?LinkClicked=UpdKC&KCID={0}&PlanID={1}&PlanName={2}"
                ,Request.QueryString["KCID"]
                ,Request.QueryString["PlanID"]
                , HttpUtility.UrlEncode(Request.QueryString["PlanName"]));
        
        Response.Redirect(Urlstr);
    }
    
    //Gets form data and builds email mesage.
    protected string GetData(string BtnClicked)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("{0} - {1} Key Contact: \n", this.PlanNameHdn.Value, BtnClicked)
            .AppendLine()
            .AppendFormat("Prefix: {0}\n", ((TextBox)(this.formViewKC.FindControl("Prefixtxt"))).Text)
            .AppendFormat("First Name: {0}\n", ((TextBox)(this.formViewKC.FindControl("FNametxt"))).Text)
            .AppendFormat("Last Name: {0}\n", ((TextBox)(this.formViewKC.FindControl("LNametxt"))).Text)
            .AppendFormat("Suffix: {0}\n", ((TextBox)(this.formViewKC.FindControl("Suffixtxt"))).Text)
            .AppendFormat("Designation: {0}\n", ((RadComboBox)(this.formViewKC.FindControl("rdcmbDesg"))).Text)
            .AppendFormat("Title: {0}\n", ((TextBox)(this.formViewKC.FindControl("Titletxt"))).Text)
            .AppendFormat("Address1: {0}\n", ((TextBox)(this.formViewKC.FindControl("Addr1txt"))).Text)
            .AppendFormat("Address2: {0}\n", ((TextBox)(this.formViewKC.FindControl("Addr2txt"))).Text)
            .AppendFormat("City: {0}\n", ((TextBox)(this.formViewKC.FindControl("Citytxt"))).Text)
            .AppendFormat("State: {0}\n", ((RadComboBox)(this.formViewKC.FindControl("rdlStates"))).Text)
            .AppendFormat("Zip: {0}\n", ((TextBox)(this.formViewKC.FindControl("Ziptxt"))).Text)
            .AppendFormat("Email: {0}\n", ((TextBox)(this.formViewKC.FindControl("Email1txt"))).Text)
            .AppendFormat("Phone1: {0}\n", ((TextBox)(this.formViewKC.FindControl("Ph1txt"))).Text)
            .AppendFormat("Mobile Phone: {0}\n", ((TextBox)(this.formViewKC.FindControl("Mobiletxt"))).Text)
            .AppendFormat("Fax: {0}\n", ((TextBox)(this.formViewKC.FindControl("Faxtxt"))).Text)
            .AppendLine()
            .AppendFormat("Assistant Details:\n")
            .AppendFormat("Name: {0}\n", ((TextBox)(this.formViewKC.FindControl("AsstNmtxt"))).Text)
            .AppendFormat("Phone: {0}\n", ((TextBox)(this.formViewKC.FindControl("AsstPh1txt"))).Text)
            .AppendFormat("Email: {0}\n", ((TextBox)(this.formViewKC.FindControl("AsstEmailtxt"))).Text)
            .AppendFormat("Comments: {0}\n", ((TextBox)(this.formViewKC.FindControl("Cmtstxt"))).Text)
            .AppendLine()
            .AppendFormat("Submitted By: {0} of {1}\n", Pinsonault.Web.Session.FullName, Pinsonault.Web.Session.ClientName)
            .AppendLine()
            .AppendFormat("User Email: {0}", HttpContext.Current.User.Identity.Name);

        return sb.ToString();
    }
  
}
