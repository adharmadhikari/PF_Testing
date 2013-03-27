using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using PathfinderClientModel; 

public partial class custom_pinso_sellsheets_AddAccount : PageBase
{
    protected override void OnPreInit(EventArgs e)
    {
        this.Response.Cache.SetNoStore();

        base.OnPreInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        dsPlans.ConnectionString = Pinsonault.Web.Session.ClientDBConnectionString;
        if (Page.IsPostBack)
        {
            //Store the selected values in hidden variable.
            hdnSelectedSegment.Value = rdcmbMktSegment.SelectedValue;
        }
        else
        {
            frmDiv.Visible = true;
            Msglbl.Visible = true;
            Msglbl.Style.Add("color", "none"); 
        }

        Dictionary<string, PathfinderModel.ClientApplicationAccess> access = Pinsonault.Web.Session.ClientApplicationAccess;
        //Select all available Section IDs for Todays Accounts
        rdcmbMktSegment.DataSource = access.Where(i => i.Value.ApplicationID == 1 && i.Value.Section != null && ((i.Value.SectionID == 1) || (i.Value.SectionID == 4) || (i.Value.SectionID == 6) || (i.Value.SectionID == 12) || (i.Value.SectionID == 20) || (i.Value.SectionID == 17))).Select(i => new { ID = i.Value.SectionID, Name = i.Value.Section.Name });
        rdcmbMktSegment.DataBind();

       //Set the market segment as per hidden variable.
        if (!String.IsNullOrEmpty(hdnSelectedSegment.Value))
        {
            rdcmbMktSegment.SelectedValue = hdnSelectedSegment.Value;
        }
  }

     protected void btnSubmit_Click(object sender, EventArgs e)
     {
         Int32 SheetID = Convert.ToInt32(HttpContext.Current.Request.QueryString["Sell_Sheet_ID"]);
         Int32 PlanID = 0;

         if (!String.IsNullOrEmpty(rdcmbPlans.SelectedValue))
         {
             PlanID = Convert.ToInt32(rdcmbPlans.SelectedValue);
         }

         if (PlanID != 0)
         {
             AddAdditionalPlans(SheetID, PlanID);

             frmDiv.Visible = false;
             //Msglbl.Text = "Selected Plan added successfully.";
             //Msglbl.Visible = true;
             //Msglbl.Style.Add("color", "none"); 
            
             //Calls Javascript function RefreshPlanSelection() to refresh plan selection list parent grid.
             Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "RefreshPlanSelection", "RefreshPlanSelection();", true);
         }
     }

    //Adds selected PlanID to Sell_Sheet_Additional_Plans table.
     protected void AddAdditionalPlans(int SheetID, int PlanID)
     {
         using ( PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString) )
         {
             SellSheetAdditionalPlans ssAddPlans = new SellSheetAdditionalPlans();
             ssAddPlans.Sell_Sheet_ID = SheetID;
             ssAddPlans.Plan_ID = PlanID;
             ssAddPlans.Created_DT = DateTime.Now;
             ssAddPlans.Created_BY = Pinsonault.Web.Session.FullName;
             context.AddToSellSheetAdditionalPlansSet(ssAddPlans);
             context.SaveChanges();
         }
     }

     protected void rdcmbMktSegment_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
     {
         rdcmbMktSegment.Text = e.Text;
         rdcmbMktSegment.SelectedValue = e.Value;
     }

     protected void rdcmbPlans_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
     {
         rdcmbPlans.Text = e.Text;
         rdcmbPlans.SelectedValue = e.Value;
     }

     protected void rdcmbPlans_OnDataBound(object sender, EventArgs  e)
     {
         if (rdcmbPlans != null)
         {
             if (rdcmbPlans.Items.Count == 0)
             {
                 planlbl.Visible = false;
                 rdcmbPlans.Visible = false;
                 noplanlbl.Visible = true;
             }
             else
             {
                 planlbl.Visible = true;
                 rdcmbPlans.Visible = true;
                 noplanlbl.Visible = false;
             }
         }       
     }
}

