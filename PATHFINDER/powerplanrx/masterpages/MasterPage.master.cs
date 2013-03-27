using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
//using Impact;
using Pinsonault.Application.PowerPlanRx;

public partial class MasterPage : System.Web.UI.MasterPage 
{
    /// <summary>
    /// Determines if the Submit button is visible so user can save changes
    /// </summary>
    public bool IsPageEditable {get; set; }
    /// <summary>
    /// Determines if the campaign can be edited by the current user (must be AE and owner of campaign)
    /// </summary>
    public bool CanEdit { get; set; }
    //public bool IsCampaignOwner { get { return subtab.IsCampaignOwner; } }
    public int PhaseID { get; set; }
    
    protected override void OnInit(EventArgs e)
    {
        HttpContext.Current.Response.AddHeader ("p3p", "CP=\"IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT\"");

        //check session
     
        Pinsonault.Web.Session.CheckSessionState();          
             
        string pageName = Path.GetFileName(this.Request.PhysicalPath);

        //UserTitle title = UserTitle.CurrentTitle;
        //if ( title == null && string.Compare(pageName, "titleselect.aspx", true) != 0 )
        //    Response.Redirect("~/titleSelect.aspx?ReturnUrl=" + Server.UrlEncode(Request.RawUrl));
                 
        if (!string.IsNullOrEmpty(Request.QueryString["id"]) && pageName.StartsWith("createcampaign", StringComparison.InvariantCultureIgnoreCase))
        {
            subtab.GetCampaignStatus(Request.QueryString["id"].ToString(),pageName);

            PhaseID = subtab.PreviousPhaseID;  // sl
           
            //IsPageEditable property is used to determine if the page needs to be displayed in editable mode or not(on first load)
            //i.e if the phaseID < 2 in Account Profile page, page will be editable on first load with Submit and Reset button visible  
            //else phaseID = 2 in Account Profile page, page will be readonly on first load with edit button visible       
            //pseudo code for the page : if (((MasterPage)this.Master).IsPageEditable){Page is Editable}
            IsPageEditable = subtab.ShowSubmit;
            CanEdit = subtab.ShowEdit || subtab.ShowSubmit;
        }

        base.OnInit(e);
    }

    protected void OnSubmit(object sender, EventArgs e)
    {
        if ( this.Page is IEditPage )
        {            
            ViewState["edit"] = "";
            if (((IEditPage)Page).Save())
            {
                subtab.LoadStatusBox();
                IsPageEditable = false;
            }
            else
            {
                IsPageEditable = true;
            }
        }
    }

    protected void OnReset(object sender, EventArgs e)
    {
        if ( this.Page is IEditPage )
        {
            ((IEditPage)Page).Reset();
            ViewState["edit"] = "N";
        }
    }
    protected void OnEdit(object sender, EventArgs e)
    {
        if (this.Page is IEditPage)
        {
            ViewState.Add("edit", "Y");
            IsPageEditable = true;
            form.FindControl("divEditBtns").Visible = false;
            form.FindControl("divSubmitBtns").Visible = true;            
            ((IEditPage)Page).Edit();
        }
    }
     
    protected override void OnPreRender(EventArgs e)
    {
        main.Visible = !(subtab.InvalidStep || subtab.InvalidPhase);
        error.Visible = !main.Visible;
        
        if (this.Page is IEditPage)
        {
            form.FindControl("divButtons").Visible = subtab.ShowSubmit || subtab.ShowEdit;
          
            if (ViewState["edit"] != null )
            {
                if (ViewState["edit"].ToString() == "Y")
                {
                    IsPageEditable = true;                    
                }
            }
            form.FindControl("divEditBtns").Visible = !IsPageEditable && (subtab.RequestedPhaseID != (int)Pinsonault.Application.PowerPlanRx.PhaseID.Team_Setup); //never show edit button on teams
            form.FindControl("divSubmitBtns").Visible = IsPageEditable;
        }
        else
        {
            form.FindControl("divButtons").Visible = false;
        }       

        base.OnPreRender(e);
    }
}
