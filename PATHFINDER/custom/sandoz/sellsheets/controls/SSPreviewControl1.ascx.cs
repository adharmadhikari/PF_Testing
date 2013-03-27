using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderClientModel;
using PathfinderModel; 
using Pinsonault.Web;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class custom_controls_SSPreviewControl1 : System.Web.UI.UserControl
{
    protected string PrevSegmentName { get; set; }
    public Int32 HeaderColspan { get; set; }
    public bool? IsHighlighted;

    //These flags are used to set footer-restrictionslabel based on restrictions displayed in the grid.
    protected bool IsPA = false, IsQL = false, IsST = false, IsSpecialty = false, IsMedical = false;
   
    protected override void OnInit(EventArgs e)
    {
        dsSellSheetReviewPlansList.ConnectionString = Pinsonault.Web.Session.ClientDBConnectionString;
         
        base.OnInit(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        Int32 SheetID = Convert.ToInt32(Request.QueryString["Sell_Sheet_ID"]);
        Boolean SegmentCP = false, SegmentMD = false;
        DateTime UpdDate;
        String strTemplateName = "";

        using ( PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString) )
        {
            SellSheetMast ssMast = null;

            ssMast = (from d in context.SellSheetMastSet
                      where d.Sell_Sheet_ID == SheetID
                      select d).First();

            IsHighlighted = ssMast.Is_Highlighted;

            if (!String.IsNullOrEmpty(ssMast.Template_Name))
                strTemplateName = ssMast.Template_Name.ToString();

            if (!String.IsNullOrEmpty(ssMast.Message_Name))
            {
                if(ssMast.Message_ID != null && ssMast.Message_ID != 3) 
                    msglbl.Text = ssMast.Message_Name.ToString();

                //Based on 'Include Geography Name' checkbox selection in step 6, show/hide the Geography Name in preview.
                if (ssMast.Include_Territory_Name == true)
                {
                    if (!String.IsNullOrEmpty(ssMast.Geography_Name.ToString()))
                        geolbl.Text = "Omnitrope Formulary Coverage for " + ssMast.Geography_Name.ToString();
                }
                SegmentCP = Convert.ToBoolean(ssMast.Segment_CP);
                SegmentMD = Convert.ToBoolean(ssMast.Segment_MD);  
            }
        }

        //Get formulary data last updated date based on selected segment(Commercial/Med-D)
        using (PathfinderEntities context = new PathfinderEntities())
        {
            DataUpdateDates formDates = null;
            if((SegmentCP == true) && (SegmentMD == false))
            {
                formDates = (from d in context.DataUpdateDatesSet
                      where d.Data_Item == "Commercial Formulary"
                      select d).First();
            }
            else if ((SegmentCP == false) && (SegmentMD == true))
            {
                formDates = (from d in context.DataUpdateDatesSet
                             where d.Data_Item == "Part-D formulary"
                             select d).First();
            }
            else if ((SegmentCP == false) && (SegmentMD == false))
            {
                formDates = (from d in context.DataUpdateDatesSet
                             orderby d.Update_Date descending
                             select d).First();
            }
            else
            {
                formDates = (from d in context.DataUpdateDatesSet
                             orderby d.Update_Date descending
                             select d).First();
            }
            UpdDate = formDates.Update_Date; 
        }

        footerdatelbl.Text = string.Format("{0:y}", UpdDate) + ".";
    }

    protected void ReviewPlanListView_OnDataBound(object sender, EventArgs e)
    {
        //Depending on PA/QL/ST/S(Speciality)/M(Medical) flags show the footer-restrictionslabel.
        if (IsPA == true)
            restrictionslbl.Text = "PA = Prior Authorization; ";

        if (IsQL == true)
            restrictionslbl.Text = restrictionslbl.Text + "QL = Quantity Limits; ";

        if (IsST == true)
            restrictionslbl.Text = restrictionslbl.Text + "ST = Step Therapy; ";

        if (IsSpecialty == true)
            restrictionslbl.Text = restrictionslbl.Text + "S = Specialty; ";

        if (IsMedical == true)
            restrictionslbl.Text = restrictionslbl.Text + "M = Medical; ";
    }

    protected void ReviewPlanListView_OnItemDataBound(object sender, ListViewItemEventArgs e)
    {
        ListViewDataItem lstdataitem = e.Item as ListViewDataItem;
        String CurrSegmentNM = null;

        Int32 SheetID = Convert.ToInt32(HttpContext.Current.Request.QueryString["Sell_Sheet_ID"]);
        List<String> lstDrugs = Pinsonault.Web.Data.Client.GetSellSheetDrugs(SheetID, Pinsonault.Web.Session.UserID);

        HtmlTableCell Header2 = e.Item.Parent.FindControl("Header2") as HtmlTableCell;
        HtmlTableCell Footer2 = e.Item.Parent.FindControl("Footer2") as HtmlTableCell;
        HtmlTableCell TierHeader1 = e.Item.FindControl("Tierheader1") as HtmlTableCell;
        HtmlTableCell Tier1Data = e.Item.FindControl("Tier1Data") as HtmlTableCell;
        HtmlTableCell CovStatusHeader1 = e.Item.FindControl("CovStatusHeader1") as HtmlTableCell;
        HtmlTableCell Cov1Data = e.Item.FindControl("Cov1Data") as HtmlTableCell;
        HtmlTableCell CopayHeader1 = e.Item.FindControl("CopayHeader1") as HtmlTableCell;
        HtmlTableCell Copay1Data = e.Item.FindControl("Copay1Data") as HtmlTableCell;

        Header2.ColSpan = HeaderColspan;
        Footer2.ColSpan = HeaderColspan;
        Header2.InnerText = lstDrugs[0];

        if(IsPA == false)
        {
            //If tier contains "PA" restriction then set IsPA flag to true
            if (((System.Web.UI.WebControls.Label)(e.Item.FindControl("Tier1lbl"))).Text.ToString().Contains("(PA)") == true)
                IsPA = true;
        }

        if (IsQL == false)
        {
            //If tier contains "QL" restriction then set IsQL flag to true
            if (((System.Web.UI.WebControls.Label)(e.Item.FindControl("Tier1lbl"))).Text.ToString().Contains("(QL)") == true)
                IsQL = true;
        }

        if (IsST == false)
        {
            //If tier contains "ST" restriction then set IsST flag to true
            if (((System.Web.UI.WebControls.Label)(e.Item.FindControl("Tier1lbl"))).Text.ToString().Contains("(ST)")== true)
                IsST = true;
        }

        if (IsSpecialty == false)
        {
            //If tier contains "S" (for Specialty) then set IsSpecialty flag to true
            if (((System.Web.UI.WebControls.Label)(e.Item.FindControl("Tier1lbl"))).Text.ToString().Trim().ToUpper() == "S")
                IsSpecialty = true;
            else if (((System.Web.UI.WebControls.Label)(e.Item.FindControl("Tier1lbl"))).Text.ToString().Trim().ToUpper() == "S(ST)")
                IsSpecialty = true;
        }

        if (IsMedical == false)
        {
            //If tier contains "M" (for Medical) then set IsMedical flag to true
            if (((System.Web.UI.WebControls.Label)(e.Item.FindControl("Tier1lbl"))).Text.ToString().Contains("M") == true)
                IsMedical = true;
        }

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

        Header3.ColSpan = HeaderColspan;
        Footer3.ColSpan = HeaderColspan;
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

        Header4.ColSpan = HeaderColspan;
        Footer4.ColSpan = HeaderColspan;
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

            if (String.IsNullOrEmpty(PrevSegmentName))
            {
                PrevSegmentName = CurrSegmentNM;
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

        if (IsHighlighted == true)
        {
            // If this value is TRUE then client product is selected in Step1 else not.
            if (lstDrugs[lstDrugs.Count-1].ToString() == "TRUE")
            {
                Header2.Attributes.Add("class", "primary");
                Footer2.Attributes.Remove("class");
                Footer2.Attributes.Add("class", "primary");
                Footer2.InnerHtml = "&nbsp;";

                if (CopayHeader1.Visible == true || Copay1Data.Visible == true)
                {
                    TierHeader1.Attributes.Add("class", "yourProductLeft");
                    CovStatusHeader1.Attributes.Add("class", "yourProductLeft");
                    CopayHeader1.Attributes.Add("class", "yourProductRight");
                    Tier1Data.Attributes.Add("class", "content yourProductLeft alignCenter");
                    Cov1Data.Attributes.Add("class", "content yourProductLeft alignCenter");
                    Copay1Data.Attributes.Remove("class");
                    Copay1Data.Attributes.Add("class", "content yourProductRight alignCenter");
                }
                else
                {
                    TierHeader1.Attributes.Add("class", "yourProductSingle");
                    CovStatusHeader1.Attributes.Add("class", "yourProductSingle");
                    Tier1Data.Attributes.Add("class", "content yourProductSingle alignCenter");
                    Cov1Data.Attributes.Add("class", "content yourProductSingle alignCenter");
                }
            }
        }
    }
}
