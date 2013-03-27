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
using Pinsonault.Application.Alcon;
using System.Web.UI.HtmlControls;

public partial class custom_controls_SSPreviewControl1 : System.Web.UI.UserControl
{
    protected string PrevSegmentName { get; set; }
    public Int32 HeaderColspan { get; set; }
    public bool? IsHighlighted;
    public int? Thera_id;
    public int? Templateid;
    public string state;
    public int? Type_id;
    public int PA = 0;
    public int QL = 0;
    public int ST = 0;
    public int NC = 0;
    public string Drug_Name;
   
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
        Int32 type_id;

        using (PathfinderAlconEntities clientContext = new PathfinderAlconEntities())
        {
            Sell_Sheet_Mast ssMast = null;

            ssMast = (from d in clientContext.Sell_Sheet_Mast
                      where d.Sell_Sheet_ID == SheetID
                      select d).First();

            IsHighlighted = ssMast.Is_Highlighted;
            strTemplateName = ssMast.Template_ID.ToString();

            Templateid = ssMast.Template_ID.Value;
            Thera_id = ssMast.Thera_ID.Value;
            state = ssMast.Geography_Name;

            SegmentCP = Convert.ToBoolean(ssMast.Segment_CP);
            SegmentMD = Convert.ToBoolean(ssMast.Segment_MD);
            Type_id = ssMast.Type_ID.Value;
            UpdDate = Convert.ToDateTime(ssMast.Modified_DT);
           
        }

        using (PathfinderAlconEntities alconContext = new PathfinderAlconEntities())
        {
            var ssSellSheetDrugs = (from d in alconContext.SellSheetReportSet
                           where d.Sell_Sheet_ID == SheetID
                           select d.Drug_Name).First();
            Drug_Name = ssSellSheetDrugs;           
        }
        SetHeader();
       
        footerdatelbl.Text = "PathfinderRx and is current as of " + string.Format("{0:y}", UpdDate) + ".";
    }

    protected void ReviewPlanListView_OnItemDataBound(object sender, ListViewItemEventArgs e)
    {
        ListViewDataItem lstdataitem = e.Item as ListViewDataItem;
        String CurrSegmentNM = null;

        Int32 SheetID = Convert.ToInt32(HttpContext.Current.Request.QueryString["Sell_Sheet_ID"]);
        List<String> lstDrugs = Pinsonault.Web.Data.Client.GetSellSheetDrugs(SheetID, Pinsonault.Web.Session.UserID);
        
        for (int i = 0; i < lstDrugs.Count; i++)
        {
            switch (lstDrugs[i])
            {
                case "Pataday":
                    //lstDrugs[i] = "Pataday&#153;";
                    lstDrugs[i] = "PATADAY&trade;";
                    break;
                case "Patanol":
                    lstDrugs[i] = "PATANOL<sup>&reg;</sup>";
                    break;
                case "Moxeza":
                    lstDrugs[i] = "MOXEZA<sup>&reg;</sup>";
                    break;
                case "Vigamox":
                    lstDrugs[i] = "VIGAMOX<sup>&reg;</sup>";
                    break;
                case "Tobradex ST":
                    lstDrugs[i] = "TOBRADEX<sup>&reg;</sup> ST";
                    break;
                case "Azopt":
                    lstDrugs[i] = "AZOPT<sup>&reg;</sup>";
                    break;
                case "Travatan Z":
                    lstDrugs[i] = "TRAVATAN Z<sup>&reg;</sup>";
                    break;
                case "Nevanac":
                    lstDrugs[i] = "NEVANAC<sup>&reg;</sup>";
                    break;
                case "Ciprodex Otic":
                    lstDrugs[i] = "CIPRODEX OTIC<sup>&reg;</sup>";
                    break;
                case "Cipro HC Otic":
                    lstDrugs[i] = "Cipro HC Otic<sup>&reg;</sup>";
                    break;
                case "Durezol":
                    lstDrugs[i] = "DUREZOL<sup>&reg;</sup>";
                    break;
                case "Bepreve":
                    lstDrugs[i] = "Bepreve<sup>&Dagger;</sup>";
                    break;
                case "Elestat":
                    lstDrugs[i] = "Elestat<sup>&Dagger;</sup>";
                    break;
                case "Lastacaft":
                    lstDrugs[i] = "Lastacaft<sup>&Dagger;</sup>";
                    break;
                case "Optivar":
                    lstDrugs[i] = "Optivar<sup>&Dagger;</sup>";
                    break;
                case "Azasite":
                    lstDrugs[i] = "Azasite<sup>&Dagger;</sup>";
                    break;
                case "Besivance":
                    lstDrugs[i] = "Besivance<sup>&Dagger;</sup>";
                    break;
                case "Zymaxid":
                    lstDrugs[i] = "Zymaxid<sup>&Dagger;</sup>";
                    break;
                case "Zylet":
                    lstDrugs[i] = "Zylet<sup>&Dagger;</sup>";
                    break;
                case "Combigan":
                    lstDrugs[i] = "Combigan<sup>&Dagger;</sup>";
                    break;
                case "Dorzolamide HCL":
                    lstDrugs[i] = "Dorzolamide HCL<sup>&Dagger;</sup>";
                    break;
                case "Latanoprost":
                    lstDrugs[i] = "Latanoprost<sup>&Dagger;</sup>";
                    break;
                case "Lumigan 0.01%":
                    lstDrugs[i] = "Lumigan 0.01%<sup>&Dagger;</sup>";
                    break;
                case "Lumigan 0.03%":
                    lstDrugs[i] = "Lumigan 0.03%<sup>&Dagger;</sup>";
                    break;
                case "Trusopt":
                    lstDrugs[i] = "Trusopt<sup>&Dagger;</sup>";
                    break;
                case "Xalatan":
                    lstDrugs[i] = "Xalatan<sup>&Dagger;</sup>";
                    break;
                case "Acular LS":
                    lstDrugs[i] = "Acular LS<sup>&Dagger;</sup>";
                    break;
                case "Bromday":
                    lstDrugs[i] = "Bromday<sup>&Dagger;</sup>";
                    break;
                case "Lotemax Suspension":
                    lstDrugs[i] = "Lotemax Suspension<sup>&Dagger;</sup>";
                    break;
                case "Acuvail":
                    lstDrugs[i] = "Acuvail<sup>&Dagger;</sup>";
                    break;
                case "Patanase":
                    lstDrugs[i] = "PATANASE<sup>&reg;</sup>";
                    break;
                case "Zioptan":
                    lstDrugs[i] = "Zioptan<sup>&Dagger;</sup>";
                    break;
                case "Cosopt PF":
                    lstDrugs[i] = "Cosopt PF<sup>&Dagger;</sup>";
                    break;
                case "Nasonex":
                    lstDrugs[i] = "Nasonex<sup>&Dagger;</sup>";
                    break;
                case "Omnaris":
                    lstDrugs[i] = "Omnaris<sup>&Dagger;</sup>";
                    break;
                case "Veramyst":
                    lstDrugs[i] = "Veramyst<sup>&Dagger;</sup>";
                    break;
                case "Astepro":
                    lstDrugs[i] = "Astepro<sup>&Dagger;</sup>";
                    break;
                case "Alphagan P 0.1%":
                    lstDrugs[i] = "Alphagan P 0.1%<sup>&Dagger;</sup>";
                    break;
            }
        }


        HtmlTableCell Header2 = e.Item.Parent.FindControl("Header2") as HtmlTableCell;
        HtmlTableCell Footer2 = e.Item.Parent.FindControl("Footer2") as HtmlTableCell;
        HtmlTableCell TierHeader1 = e.Item.FindControl("Tierheader1") as HtmlTableCell;
        HtmlTableCell Tier1Data = e.Item.FindControl("Tier1Data") as HtmlTableCell;
        HtmlTableCell CovStatusHeader1 = e.Item.FindControl("CovStatusHeader1") as HtmlTableCell;
        HtmlTableCell FormularyStatusHeader1 = e.Item.FindControl("FormularyStatusHeader1") as HtmlTableCell;
        
        HtmlTableCell Cov1Data = e.Item.FindControl("Cov1Data") as HtmlTableCell;
        HtmlTableCell CopayHeader1 = e.Item.FindControl("CopayHeader1") as HtmlTableCell;
        HtmlTableCell Copay1Data = e.Item.FindControl("Copay1Data") as HtmlTableCell;
        HtmlTableCell Formulary1Data = e.Item.FindControl("Formulary1Data") as HtmlTableCell;

        Header2.ColSpan = HeaderColspan;
        Footer2.ColSpan = HeaderColspan;
        Header2.InnerHtml = lstDrugs[0];
        if (lstDrugs[0].ToString() == "Your Product")
        {
            Header2.Visible = false;
            Footer2.Visible = false;
            TierHeader1.Visible = false;
            CovStatusHeader1.Visible = false;
            FormularyStatusHeader1.Visible = false;
            CopayHeader1.Visible = false;
            Tier1Data.Visible = false;
            Cov1Data.Visible = false;
            Copay1Data.Visible = false;
            Formulary1Data.Visible = false;
        }

        HtmlTableCell Header3 = e.Item.Parent.FindControl("Header3") as HtmlTableCell;
        HtmlTableCell Footer3 = e.Item.Parent.FindControl("Footer3") as HtmlTableCell;
        HtmlTableCell TierHeader2 = e.Item.FindControl("Tierheader2") as HtmlTableCell;
        HtmlTableCell CovStatusHeader2 = e.Item.FindControl("CovStatusHeader2") as HtmlTableCell;
        HtmlTableCell FormularyStatusHeader2 = e.Item.FindControl("FormularyStatusHeader2") as HtmlTableCell;
        HtmlTableCell CopayHeader2 = e.Item.FindControl("CopayHeader2") as HtmlTableCell;
        HtmlTableCell Tier2Data = e.Item.FindControl("Tier2Data") as HtmlTableCell;
        HtmlTableCell Cov2Data = e.Item.FindControl("Cov2Data") as HtmlTableCell;
        HtmlTableCell Copay2Data = e.Item.FindControl("Copay2Data") as HtmlTableCell;
        HtmlTableCell Formulary2Data = e.Item.FindControl("Formulary2Data") as HtmlTableCell;

        Header3.ColSpan = HeaderColspan;
        Footer3.ColSpan = HeaderColspan;
        Header3.InnerHtml = lstDrugs[1];

        if (lstDrugs[1].ToString() == "Competitor1")
        {
            Header3.Visible = false;
            Footer3.Visible = false;    
            TierHeader2.Visible = false;
            CovStatusHeader2.Visible = false;
            FormularyStatusHeader2.Visible = false;
            CopayHeader2.Visible = false;
            Tier2Data.Visible = false;
            Cov2Data.Visible = false;
            Copay2Data.Visible = false;
            Formulary2Data.Visible = false;
        }

        HtmlTableCell Header4 = e.Item.Parent.FindControl("Header4") as HtmlTableCell;
        HtmlTableCell Footer4 = e.Item.Parent.FindControl("Footer4") as HtmlTableCell;
        HtmlTableCell TierHeader3 = e.Item.FindControl("Tierheader3") as HtmlTableCell;
        HtmlTableCell CovStatusHeader3 = e.Item.FindControl("CovStatusHeader3") as HtmlTableCell;
        HtmlTableCell FormularyStatusHeader3 = e.Item.FindControl("FormularyStatusHeader3") as HtmlTableCell;
        HtmlTableCell CopayHeader3 = e.Item.FindControl("CopayHeader3") as HtmlTableCell;
        HtmlTableCell Tier3Data = e.Item.FindControl("Tier3Data") as HtmlTableCell;
        HtmlTableCell Cov3Data = e.Item.FindControl("Cov3Data") as HtmlTableCell;
        HtmlTableCell Copay3Data = e.Item.FindControl("Copay3Data") as HtmlTableCell;
        HtmlTableCell Formulary3Data = e.Item.FindControl("Formulary3Data") as HtmlTableCell;

        Header4.ColSpan = HeaderColspan;
        Footer4.ColSpan = HeaderColspan;
        Header4.InnerHtml = lstDrugs[2];

        if (lstDrugs[2].ToString() == "Competitor2")
        {
            Header4.Visible = false;
            Footer4.Visible = false;
            TierHeader3.Visible = false;
            CovStatusHeader3.Visible = false;
            FormularyStatusHeader3.Visible = false;
            CopayHeader3.Visible = false;
            Tier3Data.Visible = false;
            Cov3Data.Visible = false;
            Copay3Data.Visible = false;
            Formulary3Data.Visible = false;
        }
        HtmlTableCell Header5 = e.Item.Parent.FindControl("Header5") as HtmlTableCell;
        HtmlTableCell Footer5 = e.Item.Parent.FindControl("Footer5") as HtmlTableCell;
        HtmlTableCell TierHeader4 = e.Item.FindControl("Tierheader4") as HtmlTableCell;
        HtmlTableCell CovStatusHeader4 = e.Item.FindControl("CovStatusHeader4") as HtmlTableCell;
        HtmlTableCell FormularyStatusHeader4 = e.Item.FindControl("FormularyStatusHeader4") as HtmlTableCell;
        HtmlTableCell CopayHeader4 = e.Item.FindControl("CopayHeader4") as HtmlTableCell;
        HtmlTableCell Tier4Data = e.Item.FindControl("Tier4Data") as HtmlTableCell;
        HtmlTableCell Cov4Data = e.Item.FindControl("Cov4Data") as HtmlTableCell;
        HtmlTableCell Copay4Data = e.Item.FindControl("Copay4Data") as HtmlTableCell;
        HtmlTableCell Formulary4Data = e.Item.FindControl("Formulary4Data") as HtmlTableCell;

        Header5.ColSpan = HeaderColspan;
        Footer5.ColSpan = HeaderColspan;
        Header5.InnerHtml = lstDrugs[3];

        if (lstDrugs[3].ToString() == "FALSE")
        {
            Header5.Visible = false;
            Footer5.Visible = false;
            TierHeader4.Visible = false;
            CovStatusHeader4.Visible = false;
            FormularyStatusHeader4.Visible = false;
            CopayHeader4.Visible = false;
            Tier4Data.Visible = false;
            Cov4Data.Visible = false;
            Copay4Data.Visible = false;
            Formulary4Data.Visible = false;
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
            if (lstDrugs[lstDrugs.Count - 1].ToString() == "TRUE")
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
                    FormularyStatusHeader1.Attributes.Add("class", "yourProductSingle");
                    Formulary1Data.Attributes.Add("class", "content yourProductSingle alignCenter");
                }
            }
        }
        HtmlTableRow plans = e.Item.Parent.FindControl("plansRow") as HtmlTableRow;
        HtmlTableRow tr = e.Item.FindControl("Tr1") as HtmlTableRow;
        
        switch (Thera_id)
        {
            case 149:
                plans.Attributes.Add("class", "antiallergy");
                tr.Attributes.Add("class", "antiallergy");
                break;
            case 150:
                plans.Attributes.Add("class", "glaucoma");
                tr.Attributes.Add("class", "glaucoma");
                break;
            case 1501:
                plans.Attributes.Add("class", "glaucoma");
                tr.Attributes.Add("class", "glaucoma");
                break;
            case 151:
                plans.Attributes.Add("class", "infective");
                tr.Attributes.Add("class", "infective");
                break;
            case 153:
                plans.Attributes.Add("class", "otic");
                tr.Attributes.Add("class", "otic");
                break;
            case 157:
                plans.Attributes.Add("class", "nsaid");
                tr.Attributes.Add("class", "nsaid");
                break;
            case 158:
                plans.Attributes.Add("class", "combination");
                tr.Attributes.Add("class", "combination");
                break;
            case 159:
                plans.Attributes.Add("class", "steroid");
                tr.Attributes.Add("class", "steroid");
                break;
            case 187:
                plans.Attributes.Add("class", "patanase");
                tr.Attributes.Add("class", "patanase");
                break;
            default:
                break;
        }
        string prefix = string.Empty;
        
        if (Type_id == 1)
            prefix = "Tier";
        else 
            prefix = "Cov";

            for(int i=1; i<=4; i++)
            {
                string cellname = prefix+i.ToString()+"Data";
                HtmlTableCell datacell = tr.FindControl(cellname) as HtmlTableCell;
                if (datacell.Visible == true)
                {
                    string lblname = prefix+i.ToString()+"lbl";
                    Label lbl = tr.FindControl(lblname) as Label;
                    if (lbl.Text.Contains("ST"))
                        ST++;
                    if (lbl.Text.Contains("QL"))
                        QL++;
                    if (lbl.Text.Contains("PA"))
                        PA++;
                    if (lbl.Text.Contains("NC"))
                        NC++;
                }
            }
           
        
    }

    protected void SetHeader()
    {
        string headertext = state;
        switch (Thera_id)
        {
            case 149:
                if (Drug_Name.Contains("Pataday") && Drug_Name.Contains("Patanol"))
                    //Drug_Name = string.Format("{0} {1}", "Pataday&#153; Solution", " and Patanol<sup>&reg;</sup>");
                    Drug_Name = string.Format("{0} {1}", "PATADAY&trade; Solution", " and PATANOL<sup>&reg;</sup>");
                else
                   //Drug_Name = string.Format("{0}", "Pataday&#153;");
                    Drug_Name = string.Format("{0}", "PATADAY&trade;");
                
                headertext = string.Format("{0} {1} {2} {3}", headertext, "Formulary Table for", Drug_Name, "Solution");
                msglbl.ForeColor = System.Drawing.ColorTranslator.FromHtml("#0055a5");
                break;
            case 150:
                headertext = headertext + " Formulary Table for AZOPT<sup>&reg;</sup> Suspension";
                msglbl.ForeColor = System.Drawing.ColorTranslator.FromHtml("#0055a5");
                break;           
            case 1501:
                headertext = headertext + " Formulary Table for TRAVATAN Z<sup>&reg;</sup> Solution";
                msglbl.ForeColor = System.Drawing.ColorTranslator.FromHtml("#0055a5");
                break;
            case 151:
                if (Drug_Name.Contains("Moxeza"))
                    Drug_Name = string.Format("{0}", "MOXEZA<sup>&reg;</sup>");
                else if (Drug_Name.Contains("Vigamox"))
                    Drug_Name = string.Format("{0}", "VIGAMOX<sup>&reg;</sup>");
                else
                    Drug_Name = string.Format("{0}", "MOXEZA<sup>&reg;</sup>");
                headertext = string.Format("{0} {1} {2} {3}", headertext, "Formulary Table for", Drug_Name, "Solution");
                msglbl.ForeColor = System.Drawing.ColorTranslator.FromHtml("#C60651");
                break;
            case 153:
                headertext = headertext + " Formulary Table for CIPRODEX<sup>&reg;</sup> Suspension";
                msglbl.ForeColor = System.Drawing.ColorTranslator.FromHtml("#652d8a");
                break;
            case 157:
                headertext = headertext + " Formulary Table for NEVANAC<sup>&reg;</sup> Suspension";
                msglbl.ForeColor = System.Drawing.ColorTranslator.FromHtml("#231f20");
                break;
            case 158:
                headertext = headertext + " Formulary Table for TOBRADEX<sup>&reg;</sup> ST Suspension";
                msglbl.ForeColor = System.Drawing.ColorTranslator.FromHtml("#005199");
                break;
            case 159:
                headertext = headertext + " Formulary Table for DUREZOL<sup>&reg;</sup> Emulsion";
                msglbl.ForeColor = System.Drawing.ColorTranslator.FromHtml("#26328c");
                break;
            case 187:
                headertext = headertext + " Formulary Table for PATANASE<sup>&reg;</sup> Nasal Spray";
                msglbl.ForeColor = System.Drawing.ColorTranslator.FromHtml("#0055a5");
                break;
            default:
                break;
        }
        msglbl.Text = headertext;
    }
    /// <summary>
    /// function to convert special chars to HTML
    /// </summary>
    /// <param name="input"></param>
    /// <returns>input string with special characters replaced by HTML</returns>
    protected string ConvertSpecialCharsToHTML(String input)
    {
        if (!String.IsNullOrEmpty(input))
        {
            //Replace registered trademark character(®) with &reg; and ™ with &trade;
            return input.Replace("®", "&reg;").Replace("™", "&trade;"); 
        }
        else
            return "";
    }

    protected void ReviewPlansListView_DataBound(object sender, EventArgs e)
    {
        string restriction = string.Empty;
        if (PA > 0)
            restriction = restriction + " PA = Prior Authorization  ";
        if (QL > 0)
            restriction = restriction + " QL = Quantity Limits  ";
        if (ST > 0)
            restriction = restriction + " ST =  Step Therapy  ";
        if (NC > 0)
            restriction = restriction + " NC =  Not Covered  ";
        if (!string.IsNullOrEmpty(restriction))
            restriction_lbl.Text = restriction;
    }
}
