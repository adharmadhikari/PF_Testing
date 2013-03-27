using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class todaysaccounts_all_OpenAddEditMyKCs : PageBase
{
    protected override void OnInit(EventArgs e)
    {
        dsKeyContacts.ConnectionString = Pinsonault.Web.Session.ClientConnectionString;

        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.formViewKC.Visible = true;
            this.Msglbl.Visible = false;
            this.CloseMsglbl.Visible = false;

            //Updates Plan Name hidden variable based on Querystring.
            this.PlanNameHdn.Value = System.Web.HttpContext.Current.Request.QueryString["PlanName"];
           
            //Depending on actions(View/Add/Update)it changes form mode to ReadOnly/Insert/Edit.
            //Also, sets the form header with PlanName included.
            if (HttpContext.Current.Request["LinkClicked"] == "ViewKC")
            {
                if (Context.User.IsInRole("editcontacts"))
                {
                    this.formViewKC.ChangeMode(FormViewMode.Edit);
                    this.titleText.Text = this.PlanNameHdn.Value + " - " + this.titleText.Text + " Update";
                }
                else
                {
                    this.formViewKC.ChangeMode(FormViewMode.ReadOnly);
                    this.titleText.Text = this.PlanNameHdn.Value + " - " + this.titleText.Text;
                }
            }
            else if (HttpContext.Current.Request["LinkClicked"] == "AddKC")
            {
                this.formViewKC.ChangeMode(FormViewMode.Insert);
                this.titleText.Text = this.PlanNameHdn.Value + " - Add " + this.titleText.Text;
            }
            else
            {
                this.formViewKC.ChangeMode(FormViewMode.Edit);
                this.titleText.Text = this.PlanNameHdn.Value + " - " + this.titleText.Text + " Update";
            }
        }
        //else
        //{
        //    this.formViewKC.Visible = false;

        //    //Confirmation message is displayed after Insert/Update.
        //    if (HttpContext.Current.Request["LinkClicked"] == "AddKC")
        //    {
        //        this.formViewKC.Visible = false;
        //        this.Msglbl.Text = "<br/><b>New key contact has been added successfully.</b><br/><br/>";
        //        this.Msglbl.Visible = true;
        //        this.CloseMsglbl.Visible = true ;
        //    }
        //    else if (HttpContext.Current.Request["LinkClicked"] == "UpdKC")
        //    {
        //        this.formViewKC.Visible = false;
        //        this.Msglbl.Text = "<br/><b>Key contact has been updated successfully.</b><br/><br/>";
        //        this.Msglbl.Visible = true;
        //        this.CloseMsglbl.Visible = true;
        //    }
        //    else if (HttpContext.Current.Request["LinkClicked"] == "ViewKC")
        //    {
        //        if (Context.User.IsInRole("editcontacts"))
        //        {
        //            this.formViewKC.Visible = false;
        //            this.Msglbl.Text = "<br/><b>Key contact has been updated successfully.</b><br/><br/>";
        //            this.Msglbl.Visible = true;
        //            this.CloseMsglbl.Visible = true;
        //        }
        //    }

        //    //Calls Javascript function RefreshMyKCs() to refresh my key contacts parent grid.
        //    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "RefreshMyKCs", "RefreshMyKCs();", true);
        //}
    }

    protected override void OnPreRender(EventArgs e)
    {
        if ( Page.IsPostBack && Page.IsValid )
        {
            this.formViewKC.Visible = false;

            //Confirmation message is displayed after Insert/Update.
            if ( HttpContext.Current.Request["LinkClicked"] == "AddKC" )
            {
                this.formViewKC.Visible = false;
                this.Msglbl.Text = "<br/><b>New key contact has been added successfully.</b><br/><br/>";
                this.Msglbl.Visible = true;
                this.CloseMsglbl.Visible = true;
            }
            else if ( HttpContext.Current.Request["LinkClicked"] == "UpdKC" )
            {
                this.formViewKC.Visible = false;
                this.Msglbl.Text = "<br/><b>Key contact has been updated successfully.</b><br/><br/>";
                this.Msglbl.Visible = true;
                this.CloseMsglbl.Visible = true;
            }
            else if ( HttpContext.Current.Request["LinkClicked"] == "ViewKC" )
            {
                if ( Context.User.IsInRole("editcontacts") )
                {
                    this.formViewKC.Visible = false;
                    this.Msglbl.Text = "<br/><b>Key contact has been updated successfully.</b><br/><br/>";
                    this.Msglbl.Visible = true;
                    this.CloseMsglbl.Visible = true;
                }
            }

            //Calls Javascript function RefreshMyKCs() to refresh my key contacts parent grid.
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "RefreshMyKCs", "RefreshMyKCs();", true);
        }
        base.OnPreRender(e);
    }

    //Called when "Delete" button is clicked on my key contact update form.
    protected void Delbtn_Click(object sender, EventArgs e)
    {
        //Update contact status from active to inactive, It doesn't delete the contact physically.
        using (PathfinderClientModel.PathfinderClientEntities context = new PathfinderClientModel.PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
        {
            //Updates Key_Contacts.Status field to False            
            if ( Pinsonault.Application.TodaysAccounts.TodaysAccountsClientDataService.DeleteContact(System.Convert.ToInt32(Page.Request.QueryString["KCID"]), context) )
            {
                //Confirmation message is displayed after deletion.
                this.formViewKC.Visible = false;
                this.Msglbl.Text = "<br/><b>Key contact has been deleted successfully.</b><br/><br/>";
                this.Msglbl.Visible = true;
                this.CloseMsglbl.Visible = true;
            }
        }

    }

    //While inserting the data into Key_Contacts table, it calls AddData to set the Plan_ID, Status 
    //and KC_Title_Name fields to appropriate values.
    protected void AddData(object sender, EntityDataSourceChangingEventArgs e)
    {
        ((PathfinderClientModel.KeyContact)e.Entity).Plan_ID = System.Convert.ToInt32(System.Web.HttpContext.Current.Request.QueryString["Plan_ID"]);
        ((PathfinderClientModel.KeyContact)e.Entity).Status = true;
        ((PathfinderClientModel.KeyContact)e.Entity).KC_Title_Name = ((Telerik.Web.UI.RadComboBox)(this.formViewKC.FindControl("rdcmbDesg"))).Text;
    }

    //While updating the data into Key_Contacts table, it calls UpdateData to set KC_Title_Name field 
    //to appropriate value.
    protected void UpdateData(object sender, EntityDataSourceChangingEventArgs e)
    {
        ((PathfinderClientModel.KeyContact)e.Entity).KC_Title_Name = ((Telerik.Web.UI.RadComboBox)(this.formViewKC.FindControl("rdcmbDesg"))).Text;
    }

    protected void UpdDelViewbtn_Click(object sender, EventArgs e)
    {
        string Urlstr = "./OpenAddEditMyKCs.aspx?LinkClicked=UpdKC" + "&KCID=" + System.Web.HttpContext.Current.Request.QueryString["KCID"];
        Urlstr = Urlstr + "&PlanID=" + System.Web.HttpContext.Current.Request.QueryString["PlanID"];
        Urlstr = Urlstr + "&PlanName=" + System.Web.HttpContext.Current.Request.QueryString["PlanName"];

        Response.Redirect(Urlstr);
    }

  }
