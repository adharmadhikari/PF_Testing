using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderClientModel;
using Pinsonault.Web;
using Telerik.Web.UI; 

public partial class custom_pinso_sellsheets_planselection : InputFormBase
{
     protected override void OnInit(EventArgs e)
    {
        dsSellSheetMast.ConnectionString = Pinsonault.Web.Session.ClientConnectionString;
        dsSellSheetPlansList.ConnectionString = Pinsonault.Web.Session.ClientDBConnectionString;
        base.OnInit(e);
    }

    protected override bool IsRequestValid()
    {
        return true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, gridPlanSelectionList.ClientID);
        Int32 SheetID = 0;

        //Get selected Sell_Sheet_ID
        SheetID = Convert.ToInt32(HttpContext.Current.Request.QueryString["Sell_Sheet_ID"]);

        JSONHeaderBuilder headerBuilder = new JSONHeaderBuilder("_HeaderDetails");
        headerBuilder.AddColumn("", 6);
        List<String> lstDrugs = Pinsonault.Web.Data.Client.GetSellSheetDrugs(SheetID, Pinsonault.Web.Session.UserID);

        Boolean highlight = false;

        if (lstDrugs[lstDrugs.Count - 1] == "TRUE")
            highlight = true;

        Int32 iColSpan;
        
        if(((HiddenField)(this.formSSPlans.FindControl("hdnCopay"))).Value == "True")
            iColSpan = 2;
        else
            iColSpan =1;

        for (Int32 i = 0; i < (lstDrugs.Count - 1);i++)
        {
            if ((lstDrugs[i].ToString() != "Your Product") && (lstDrugs[i].ToString() != "Competitor1") && (lstDrugs[i].ToString() != "Competitor2"))
                if (highlight)
                    headerBuilder.AddColumn(lstDrugs[i], iColSpan, i==0?"primary":"");
                else
                    headerBuilder.AddColumn(lstDrugs[i], iColSpan, i == 0 ? "" : "");
        }

        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "HeaderDetails", headerBuilder.ToString(), true);  

        if (!Page.IsPostBack)
        {           
            //Get currently selected plans(PlanID|FormularyID|ProductID) for selected SellSheet and store it comma separated in a hidden variable.
            using (PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
            {
                //Get the list of selected records(containing PlanID, FormularyID and ProductID separated by pipe(|))
                var ssPlansQuery = (from d in context.SellSheetPlansSet
                               where d.Sell_Sheet_ID == SheetID
                               select d).ToList().Select(d => string.Format("{0}|{1}|{2}", d.Plan_ID.ToString(), d.Formulary_ID.ToString(), d.Plan_Product_ID.ToString()));

                //Comma separate individual record's data.
                hdnPlansSelected.Value = string.Join(",", ssPlansQuery.ToArray());
            }
        }

        //For selected sellsheet, if Type_ID = 1(Tier Status) then hide coveragestatus columns and 
        //show tier status columns else vice versa.
        if (((System.Web.UI.WebControls.HiddenField)(this.formSSPlans.FindControl("hdnTypeID"))).Value == "1")
        {
            gridPlanSelectionList.Columns.FindByUniqueName("FirstCoverageStatus").Display = false;
            gridPlanSelectionList.Columns.FindByUniqueName("SecondCoverageStatus").Display = false;
            gridPlanSelectionList.Columns.FindByUniqueName("ThirdCoverageStatus").Display = false;

            gridPlanSelectionList.Columns.FindByUniqueName("FirstFormularyStatus").Display = false;
            gridPlanSelectionList.Columns.FindByUniqueName("SecondFormularyStatus").Display = false;
            gridPlanSelectionList.Columns.FindByUniqueName("ThirdFormularyStatus").Display = false;

            gridPlanSelectionList.Columns.FindByUniqueName("FirstTierName").Display = true ;
            gridPlanSelectionList.Columns.FindByUniqueName("SecondTierName").Display = true;
            gridPlanSelectionList.Columns.FindByUniqueName("ThirdTierName").Display = true;                
        }
        else if (((System.Web.UI.WebControls.HiddenField)(this.formSSPlans.FindControl("hdnTypeID"))).Value == "2") //Type_ID = 2(Coverage Status)
        {
            gridPlanSelectionList.Columns.FindByUniqueName("FirstTierName").Display = false;
            gridPlanSelectionList.Columns.FindByUniqueName("SecondTierName").Display = false;
            gridPlanSelectionList.Columns.FindByUniqueName("ThirdTierName").Display = false;
            
            gridPlanSelectionList.Columns.FindByUniqueName("FirstCoverageStatus").Display = true;
            gridPlanSelectionList.Columns.FindByUniqueName("SecondCoverageStatus").Display = true;
            gridPlanSelectionList.Columns.FindByUniqueName("ThirdCoverageStatus").Display = true;

            gridPlanSelectionList.Columns.FindByUniqueName("FirstFormularyStatus").Display = false;
            gridPlanSelectionList.Columns.FindByUniqueName("SecondFormularyStatus").Display = false;
            gridPlanSelectionList.Columns.FindByUniqueName("ThirdFormularyStatus").Display = false;
        }
        else if (((System.Web.UI.WebControls.HiddenField)(this.formSSPlans.FindControl("hdnTypeID"))).Value == "3") //Type_ID = 3(Formulary Status)
        {
            gridPlanSelectionList.Columns.FindByUniqueName("FirstTierName").Display = false;
            gridPlanSelectionList.Columns.FindByUniqueName("SecondTierName").Display = false;
            gridPlanSelectionList.Columns.FindByUniqueName("ThirdTierName").Display = false;

            gridPlanSelectionList.Columns.FindByUniqueName("FirstCoverageStatus").Display = false;
            gridPlanSelectionList.Columns.FindByUniqueName("SecondCoverageStatus").Display = false;
            gridPlanSelectionList.Columns.FindByUniqueName("ThirdCoverageStatus").Display = false;

            gridPlanSelectionList.Columns.FindByUniqueName("FirstFormularyStatus").Display = true;
            gridPlanSelectionList.Columns.FindByUniqueName("SecondFormularyStatus").Display = true;
            gridPlanSelectionList.Columns.FindByUniqueName("ThirdFormularyStatus").Display = true;
        }
        else
        {
            gridPlanSelectionList.Columns.FindByUniqueName("FirstCoverageStatus").Display = false;
            gridPlanSelectionList.Columns.FindByUniqueName("SecondCoverageStatus").Display = false;
            gridPlanSelectionList.Columns.FindByUniqueName("ThirdCoverageStatus").Display = false;
            gridPlanSelectionList.Columns.FindByUniqueName("FirstTierName").Display = true;
            gridPlanSelectionList.Columns.FindByUniqueName("SecondTierName").Display = true;
            gridPlanSelectionList.Columns.FindByUniqueName("ThirdTierName").Display = true;

            gridPlanSelectionList.Columns.FindByUniqueName("FirstFormularyStatus").Display = false;
            gridPlanSelectionList.Columns.FindByUniqueName("SecondFormularyStatus").Display = false;
            gridPlanSelectionList.Columns.FindByUniqueName("ThirdFormularyStatus").Display = false;
        }

        //If copay option in step 3 is Yes/true then show copay columns else hide them.
        if (((System.Web.UI.WebControls.HiddenField)(this.formSSPlans.FindControl("hdnCopay"))).Value == "True")
        {
            gridPlanSelectionList.Columns.FindByUniqueName("FirstCopay").Display = true;
            gridPlanSelectionList.Columns.FindByUniqueName("SecondCopay").Display = true;
            gridPlanSelectionList.Columns.FindByUniqueName("ThirdCopay").Display = true;             
        }
        else
        {
            gridPlanSelectionList.Columns.FindByUniqueName("FirstCopay").Display = false;
            gridPlanSelectionList.Columns.FindByUniqueName("SecondCopay").Display = false;
            gridPlanSelectionList.Columns.FindByUniqueName("ThirdCopay").Display = false;
        }

        if (lstDrugs[0].ToString() == "Your Product")
        {
            gridPlanSelectionList.Columns.FindByUniqueName("FirstTierName").Display = false;
            gridPlanSelectionList.Columns.FindByUniqueName("FirstCoverageStatus").Display = false;
            gridPlanSelectionList.Columns.FindByUniqueName("FirstCopay").Display = false;
            gridPlanSelectionList.Columns.FindByUniqueName("FirstFormularyStatus").Display = false;
        }

        if (lstDrugs[1].ToString() == "Competitor1")
        {
            gridPlanSelectionList.Columns.FindByUniqueName("SecondTierName").Display = false;
            gridPlanSelectionList.Columns.FindByUniqueName("SecondCoverageStatus").Display = false;
            gridPlanSelectionList.Columns.FindByUniqueName("SecondCopay").Display = false;
            gridPlanSelectionList.Columns.FindByUniqueName("SecondFormularyStatus").Display = false;
        }

        if (lstDrugs[2].ToString() == "Competitor2")
        {
            gridPlanSelectionList.Columns.FindByUniqueName("ThirdTierName").Display = false;
            gridPlanSelectionList.Columns.FindByUniqueName("ThirdCoverageStatus").Display = false;
            gridPlanSelectionList.Columns.FindByUniqueName("ThirdCopay").Display = false;
            gridPlanSelectionList.Columns.FindByUniqueName("ThirdFormularyStatus").Display = false;
        }

        //Code to highlight if selected
        //Display options are
        //  Tier | Copay
        //  Coverage | Copay
        //  Tier
        //  Coverage
        if (highlight)
        {
            gridPlanSelectionList.MasterTableView.ShowFooter = true;

            if (((System.Web.UI.WebControls.HiddenField)(this.formSSPlans.FindControl("hdnCopay"))).Value == "True")
            {
                gridPlanSelectionList.Columns.FindByUniqueName("FirstCoverageStatus").ItemStyle.CssClass = "yourProductLeft";
                gridPlanSelectionList.Columns.FindByUniqueName("FirstTierName").ItemStyle.CssClass = "yourProductLeft";
                gridPlanSelectionList.Columns.FindByUniqueName("FirstCopay").ItemStyle.CssClass = "alignRight yourProductRight";
                gridPlanSelectionList.Columns.FindByUniqueName("FirstCoverageStatus").HeaderStyle.CssClass = "planSelectHeader postback yourProductLeft";
                gridPlanSelectionList.Columns.FindByUniqueName("FirstTierName").HeaderStyle.CssClass = "planSelectHeader postback yourProductLeft";
                gridPlanSelectionList.Columns.FindByUniqueName("FirstCopay").HeaderStyle.CssClass = "planSelectHeader postback yourProductRight";
                gridPlanSelectionList.Columns.FindByUniqueName("FirstCoverageStatus").FooterStyle.CssClass = "yourProductBottom";
                gridPlanSelectionList.Columns.FindByUniqueName("FirstTierName").FooterStyle.CssClass = "yourProductBottom";
                gridPlanSelectionList.Columns.FindByUniqueName("FirstCopay").FooterStyle.CssClass = "yourProductBottom";
            }
            else
            {
                gridPlanSelectionList.Columns.FindByUniqueName("FirstCoverageStatus").ItemStyle.CssClass = "yourProductSingle";
                gridPlanSelectionList.Columns.FindByUniqueName("FirstTierName").ItemStyle.CssClass = "yourProductSingle";
                gridPlanSelectionList.Columns.FindByUniqueName("FirstCoverageStatus").HeaderStyle.CssClass = "planSelectHeader postback yourProductSingle";
                gridPlanSelectionList.Columns.FindByUniqueName("FirstTierName").HeaderStyle.CssClass = "planSelectHeader postback yourProductSingle";
                gridPlanSelectionList.Columns.FindByUniqueName("FirstCoverageStatus").FooterStyle.CssClass = "yourProductBottom";
                gridPlanSelectionList.Columns.FindByUniqueName("FirstTierName").FooterStyle.CssClass = "yourProductBottom";
            }
        }
    }

    protected void btnNextStep_Click(object sender, EventArgs e)
    {
        try
        {
            using (PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
            {
                Int32 SheetID = Convert.ToInt32(HttpContext.Current.Request.QueryString["Sell_Sheet_ID"]);

                var ssPlansQuery =
                    from d in context.SellSheetPlansSet 
                    where d.Sell_Sheet_ID == SheetID
                    select d;

                //Delete all the records from Sell_Sheet_Plans table for selected Sell_Sheet_ID.
                foreach (var plan in ssPlansQuery) context.DeleteObject(plan);
                context.SaveChanges();

                if (!String.IsNullOrEmpty(hdnPlansSelected.Value.ToString()))
                {
                    //Update Sell_Sheet_Plans table with selected record containing PlanID, FormularyID and ProductID.
                    if (hdnPlansSelected.Value.ToString().IndexOf(",") > 0)
                    {
                        //Split the data by comma first and then by pipe(|)
                        string[] selPlanids = hdnPlansSelected.Value.ToString().Split(new Char[] { ',' });
                        foreach (string ids in selPlanids)
                        {
                            string[] splitIDs = ids.Split(new Char[] { '|' });
                            updateSellSheetPlans(SheetID, splitIDs);
                        }
                    }
                    else
                    {
                        string[] splitIDs = hdnPlansSelected.Value.ToString().Split(new Char[] { '|' });
                        updateSellSheetPlans(SheetID, splitIDs);
                    }

                    context.SaveChanges();

                    UpdateCurrentStep(SheetID);
                    PostBackResult.Success = true;
                }
            }
        }
        catch
        {
            PostBackResult.Success = false;
        }
    }

    //Updates Sell_Sheet_Plans table with given PlanID, FormularyID, ProductID.
    protected void updateSellSheetPlans(int SheetID, string[] splitIDs)
    {
        using (PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
        {
            SellSheetPlans ssPlans = new SellSheetPlans();
            ssPlans.Sell_Sheet_ID = SheetID;
            ssPlans.Plan_ID = Convert.ToInt32(splitIDs[0]);
            ssPlans.Formulary_ID = Convert.ToInt32(splitIDs[1]);
            ssPlans.Plan_Product_ID = Convert.ToInt32(splitIDs[2]);
            ssPlans.Created_DT = DateTime.Now;
            ssPlans.Created_BY = Pinsonault.Web.Session.FullName;  
            context.AddToSellSheetPlansSet(ssPlans);
            context.SaveChanges();
        }
    }

    protected void UpdateCurrentStep(int SheetID)
    {
        using (PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
        {
            SellSheet ssheet = null; 
            ssheet = (from d in context.SellSheetSet
                      where d.Sell_Sheet_ID == SheetID
                      select d).FirstOrDefault();

            //Logic below is to check if current step needs to be updated
            if (Master.RequestedStep == Master.CurrentStep)
            {
                string nextStep = (from ss in context.SellSheetStepSet
                                   where ss.Step_Order == (Master.RequestedStep + 1)
                                   select ss.Step_Key).FirstOrDefault();
                ssheet.Current_Step = nextStep;
            }

            context.SaveChanges();
        }  
    }
}

