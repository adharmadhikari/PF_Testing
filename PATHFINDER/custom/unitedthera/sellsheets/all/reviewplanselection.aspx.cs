using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderClientModel;
using Pinsonault.Web;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class custom_unitedthera_sellsheets_reviewplanselection : InputFormBase
{
    protected string PrevSegmentName{ get; set; }
    
    protected override void OnInit(EventArgs e)
    {
        dsSellSheetMast.ConnectionString = Pinsonault.Web.Session.ClientConnectionString;
        dsSellSheetReviewPlansList.ConnectionString = Pinsonault.Web.Session.ClientDBConnectionString;
         base.OnInit(e);
    }

    protected override bool IsRequestValid()
    {
        return true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, ReviewPlansListView.ClientID);

        Int32 SheetID = 0;
        //Get selected Sell_Sheet_ID
        SheetID = Convert.ToInt32(HttpContext.Current.Request.QueryString["Sell_Sheet_ID"]);

        if (!Page.IsPostBack)
        {

            //Get currently selected plans(PlanID|FormularyID|ProductID) for hightlighted SellSheet and store it comma separated in a hidden variable.
            using (PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
            {
                //Get the list of highlighted records(containing PlanID, FormularyID and ProductID separated by pipe(|))
                var ssPlansQuery = (from d in context.SellSheetPlansSet
                                    where d.Sell_Sheet_ID == SheetID && d.Is_Highlighted == true
                                    select d).ToList().Select(d => string.Format("{0}|{1}|{2}", d.Plan_ID.ToString(), d.Formulary_ID.ToString(), d.Plan_Product_ID.ToString()));

                //Comma separate individual record's data.
                hdnPlansHighlighted.Value = string.Join(",", ssPlansQuery.ToArray());
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

                //Update Is_Hightlighted field to false for all the records from Sell_Sheet_Plans table for selected Sell_Sheet_ID.
                foreach (var plan in ssPlansQuery)
                {
                    plan.Is_Highlighted = false; 
                }
                context.SaveChanges();

                if(!String.IsNullOrEmpty(hdnPlansHighlighted.Value.ToString()))
                {
                    //Update Sell_Sheet_Plans table with highlighted record containing PlanID, FormularyID and ProductID.
                    if (hdnPlansHighlighted.Value.ToString().IndexOf(",") > 0)
                    {
                        //Split the data by comma first and then by pipe(|)
                        string[] selPlanids = hdnPlansHighlighted.Value.ToString().Split(new Char[] { ',' });
                        foreach (string ids in selPlanids)
                        {
                            string[] splitIDs = ids.Split(new Char[] { '|' });
                            updateSellSheetPlans(SheetID, splitIDs);
                        }
                    }
                    else
                    {
                        string[] splitIDs = hdnPlansHighlighted.Value.ToString().Split(new Char[] { '|' });
                        updateSellSheetPlans(SheetID, splitIDs);
                    }

                    context.SaveChanges();
                }

                UpdateCurrentStep(SheetID);

                PostBackResult.Success = true;
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
            Int32 PlanID, FormID, ProdID = 0;

            PlanID = Convert.ToInt32(splitIDs[0]);
            FormID = Convert.ToInt32(splitIDs[1]);
            ProdID = Convert.ToInt32(splitIDs[2]);

            SellSheetPlans ssPlans = null; 
            ssPlans = (from d in context.SellSheetPlansSet 
                      where (d.Sell_Sheet_ID == SheetID && 
                             d.Plan_ID ==  PlanID && 
                             d.Formulary_ID == FormID &&
                             d.Plan_Product_ID == ProdID)
                      select d).FirstOrDefault();

            ssPlans.Is_Highlighted = true; 
            ssPlans.Modified_DT = DateTime.Now;
            ssPlans.Modified_BY = Pinsonault.Web.Session.FullName;
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
   

    protected void ReviewPlanListView_OnItemDataBound(object sender, ListViewItemEventArgs e)
    {
        ListViewDataItem lstdataitem = e.Item as ListViewDataItem;
        String CurrSegmentNM= null;

        Int32 iColSpan;
        if (((HiddenField)(this.formSSPlans.FindControl("hdnCopay"))).Value == "True")
            iColSpan = 2;
        else
            iColSpan = 1;

        //Get Drugs names for Client and Competitor Drugs
        Int32 SheetID = Convert.ToInt32(HttpContext.Current.Request.QueryString["Sell_Sheet_ID"]);
        List<String> lstDrugs = Pinsonault.Web.Data.Client.GetSellSheetDrugs(SheetID, Pinsonault.Web.Session.UserID);

        Boolean highlight = false;

        if (lstDrugs[lstDrugs.Count - 1] == "TRUE")
            highlight = true;

        HtmlTableCell Header2 = e.Item.Parent.FindControl("Header2") as HtmlTableCell;
        HtmlTableCell Footer2 = e.Item.Parent.FindControl("Footer2") as HtmlTableCell;
        HtmlTableCell TierHeader1 = e.Item.FindControl("Tierheader1") as HtmlTableCell;
        HtmlTableCell Tier1Data = e.Item.FindControl("Tier1Data") as HtmlTableCell;
        HtmlTableCell CovStatusHeader1 = e.Item.FindControl("CovStatusHeader1") as HtmlTableCell;
        HtmlTableCell Cov1Data = e.Item.FindControl("Cov1Data") as HtmlTableCell;
        HtmlTableCell CopayHeader1 = e.Item.FindControl("CopayHeader1") as HtmlTableCell;
        HtmlTableCell Copay1Data = e.Item.FindControl("Copay1Data") as HtmlTableCell;

        Header2.ColSpan = iColSpan;
        Footer2.ColSpan = iColSpan;
        Header2.InnerText = lstDrugs[0];

        if (lstDrugs[0].ToString() == "Your Product")
        {
            Header2.Visible = false;
            Footer2.Visible = false;
            TierHeader1.Visible = false;
            CovStatusHeader1.Visible = false;
            CopayHeader1.Visible = false;
            Tier1Data.Visible = false;
            Cov1Data.Visible = false;
            Copay1Data.Visible = false; 
        }

        HtmlTableCell Header3 = e.Item.Parent.FindControl("Header3") as HtmlTableCell;
        HtmlTableCell Footer3 = e.Item.Parent.FindControl("Footer3") as HtmlTableCell;
        HtmlTableCell TierHeader2 = e.Item.FindControl("Tierheader2") as HtmlTableCell;
        HtmlTableCell CovStatusHeader2 = e.Item.FindControl("CovStatusHeader2") as HtmlTableCell;
        HtmlTableCell CopayHeader2 = e.Item.FindControl("CopayHeader2") as HtmlTableCell;
        HtmlTableCell Tier2Data = e.Item.FindControl("Tier2Data") as HtmlTableCell;
        HtmlTableCell Cov2Data = e.Item.FindControl("Cov2Data") as HtmlTableCell;
        HtmlTableCell Copay2Data = e.Item.FindControl("Copay2Data") as HtmlTableCell;

        Header3.ColSpan = iColSpan;
        Footer3.ColSpan = iColSpan;
        Header3.InnerText = lstDrugs[1];

        if (lstDrugs[1].ToString() == "Competitor1")
        {
            Header3.Visible = false;
            Footer3.Visible = false;  
            TierHeader2.Visible = false;
            CovStatusHeader2.Visible = false;
            CopayHeader2.Visible = false;
            Tier2Data.Visible = false;
            Cov2Data.Visible = false;
            Copay2Data.Visible = false; 
        }

        HtmlTableCell Header4 = e.Item.Parent.FindControl("Header4") as HtmlTableCell;
        HtmlTableCell Footer4 = e.Item.Parent.FindControl("Footer4") as HtmlTableCell;
        HtmlTableCell TierHeader3 = e.Item.FindControl("Tierheader3") as HtmlTableCell;
        HtmlTableCell CovStatusHeader3 = e.Item.FindControl("CovStatusHeader3") as HtmlTableCell;
        HtmlTableCell CopayHeader3 = e.Item.FindControl("CopayHeader3") as HtmlTableCell;
        HtmlTableCell Tier3Data = e.Item.FindControl("Tier3Data") as HtmlTableCell;
        HtmlTableCell Cov3Data = e.Item.FindControl("Cov3Data") as HtmlTableCell;
        HtmlTableCell Copay3Data = e.Item.FindControl("Copay3Data") as HtmlTableCell;

        Header4.ColSpan = iColSpan;
        Footer4.ColSpan = iColSpan;
        Header4.InnerText = lstDrugs[2];

        if (lstDrugs[2].ToString() == "Competitor2")
        {
            Header4.Visible = false;
            Footer4.Visible = false;
            TierHeader3.Visible = false;
            CovStatusHeader3.Visible = false;
            CopayHeader3.Visible = false;
            Tier3Data.Visible = false;
            Cov3Data.Visible = false;
            Copay3Data.Visible = false; 
        }

        if (lstdataitem != null)
        {
            CurrSegmentNM = ((DataRowView)lstdataitem.DataItem)["Segment_Name"].ToString();
            Control row = e.Item.FindControl("Tr1");

            if(String.IsNullOrEmpty(PrevSegmentName))
            {
                 PrevSegmentName = CurrSegmentNM ; 
            }
            else if (PrevSegmentName == CurrSegmentNM)
            {
                row.Visible = false;
            }
            else if (PrevSegmentName != CurrSegmentNM)
            {
                PrevSegmentName = CurrSegmentNM;
                row.Visible = true; 
            }
        }

        //Code to highlight if selected
        //Display options are
        //  Tier | Copay
        //  Coverage | Copay
        //  Tier
        //  Coverage

        if (highlight)
        {
            Header2.Attributes.Add("class", "primary");
            Footer2.Attributes.Remove("class");
            Footer2.Attributes.Add("class", "yourProductBottom");

            if (CopayHeader1.Visible == true || Copay1Data.Visible == true)
            {
                TierHeader1.Attributes.Add("class", "yourProductLeft");
                CovStatusHeader1.Attributes.Add("class", "yourProductLeft");
                CopayHeader1.Attributes.Add("class", "yourProductRight");
                Tier1Data.Attributes.Add("class", "yourProductLeft");
                Cov1Data.Attributes.Add("class", "yourProductLeft");
                Copay1Data.Attributes.Remove("class");
                Copay1Data.Attributes.Add("class", "alignRight yourProductRight");                
            }
            else
            {
                TierHeader1.Attributes.Add("class", "yourProductSingle");
                CovStatusHeader1.Attributes.Add("class", "yourProductSingle");
                Tier1Data.Attributes.Add("class", "yourProductSingle");
                Cov1Data.Attributes.Add("class", "yourProductSingle");
            }
        }
    }
}
