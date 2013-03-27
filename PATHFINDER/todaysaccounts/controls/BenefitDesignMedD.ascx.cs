using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using PathfinderModel; 

public partial class controls_MedDBenefitDesign : System.Web.UI.UserControl
{
    public string ContainerID { get; set; }
    public string OnClientRowSelected { get; set; }
    public bool AllowSorting { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(this.Page, gridMedDBenefitDesg.ClientID, "{}", ContainerID);
        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "_meddbdpagevars", string.Format("var gridMedDBenefitDesignID = '{0}';", gridMedDBenefitDesg.ClientID), true);

        //For Channel = Commercial, based on selected plan's Segment_ID, hide or show Medicare Part D Grid.
        //SegmentID = 1(Show only Commercial Grid)
        //SegmentID = 2(Show only Medicare Part D grid)
        //SegmentID = 3(Show both grids, Commercial and Medicare Part D)

        // sl 3/22/2012 : Business Rule changed
        // based on Plan_Mast (3 additional fields: Has_Commercial_Business, Has_Medicare_partD_Business, Has_Managed_Medicaid_Business)
        // show/hide Grid

        // int? segmentID = 0;

        bool hasMedD = false;
        string id = Request.QueryString["Plan_ID"];
        int planID = 0;
        if (int.TryParse(id, out planID))
        {
            using (PathfinderModel.PathfinderEntities context = new PathfinderModel.PathfinderEntities())
            {
                //segmentID = context.PlanMasterSet.Where(p => p.Plan_ID == planID).Select(p=>p.Segment_ID).FirstOrDefault();                

                var p = (from q in context.PlanMasterSet
                         where q.Plan_ID == planID
                         select q.Has_Medicare_partD_Business).FirstOrDefault();
                if (p == true)
                    hasMedD = true;

            }
        }


        gridMedDBenefitDesg.AllowSorting = AllowSorting;
        gridMedDBenefitDesg.MasterTableView.AllowSorting = AllowSorting;

        string strProdID = Request.QueryString["Prod_ID"];

        if (hasMedD)
        {
            //if state is selected then Prod_id is present in querystring
            if (string.IsNullOrEmpty(strProdID))
            {
                gridMedDBenefitDesg.DataSourceID = "dsMedDBenefitDesign";
            }
            else
            {
                gridMedDBenefitDesg.DataSourceID = "dsBenefitDesignMedDState";
            }

            BDHeader2.Visible = true;
            gridMedDBenefitDesg.Visible = true;
        }
        else
        {
            BDHeader2.Visible = false;
            gridMedDBenefitDesg.Visible = false;
        }

        //if (segmentID == 1)
        //{
        //    BDHeader2.Visible = false;
        //    gridMedDBenefitDesg.Visible = false;
        //}
        //else
        //{
        //    //if state is selected then Prod_id is present in querystring
        //    if (string.IsNullOrEmpty(strProdID))
        //    {
        //    gridMedDBenefitDesg.DataSourceID = "dsMedDBenefitDesign";
        //    }
        //    else
        //    {
        //        gridMedDBenefitDesg.DataSourceID = "dsBenefitDesignMedDState";
        //    }
        //    BDHeader2.Visible = true;
        //    gridMedDBenefitDesg.Visible = true;
        //}

        if (!string.IsNullOrEmpty(OnClientRowSelected))
        {
            ////If formulary is enabled for the user then show drilldown grid else hide it.
            if (Context.User.IsInRole("frmly_17"))
            {
                gridMedDBenefitDesg.ClientSettings.Selecting.AllowRowSelect = true;
                gridMedDBenefitDesg.ClientSettings.ClientEvents.OnRowSelected = OnClientRowSelected;
            }
        }

        //formularyDate.Visible = gridMedDBenefitDesg.Visible;
    }

    ////Logic for setting the rowspan.
    //protected void gridMedDBenefitDesg_DataBound(object sender, EventArgs e)
    //{
    //    //Group if there is no sort or 1st sort expression is Prod_Name
    //    if ( gridMedDBenefitDesg.MasterTableView.SortExpressions.Count == 0 || string.Compare(gridMedDBenefitDesg.MasterTableView.SortExpressions[0].FieldName, "Prod_Name", true) == 0 )
    //    {
    //        int i = 0, rowSpanCnt = 0, rowSpanItem = 0;
    //        string prevProd = "";

    //        foreach ( GridDataItem item in gridMedDBenefitDesg.Items )
    //        {
    //            //For first item in the list, store the Product name in PrevProd variable.
    //            if ( i == 0 )
    //            {
    //                prevProd = item["Prod_Name"].Text;
    //                //Increment rowspan counter
    //                rowSpanCnt = rowSpanCnt + 1;
    //                //Item for which rowspan will be set.
    //                rowSpanItem = 0;
    //            }
    //            else
    //            {
    //                //If previous ProductName is same as current then make current as previous and go on till you 
    //                //find a difference.
    //                if ( prevProd == item["Prod_Name"].Text )
    //                {
    //                    //Hide current row's ProductName column.
    //                    item["Prod_Name"].Visible = false;
    //                    //Increment rowspan counter
    //                    rowSpanCnt = rowSpanCnt + 1;

    //                    //If you came to last item in the list then assign the rowspan.
    //                    if ( i == (gridMedDBenefitDesg.Items.Count - 1) )
    //                    {
    //                        gridMedDBenefitDesg.Items[rowSpanItem]["Prod_Name"].RowSpan = rowSpanCnt;
    //                    }
    //                }
    //                //If previos ProductName is not same as current. 
    //                else
    //                {
    //                    //Set rowspan for previous element.
    //                    gridMedDBenefitDesg.Items[rowSpanItem]["Prod_Name"].RowSpan = rowSpanCnt;
    //                    //Make current as previous
    //                    prevProd = item["Prod_Name"].Text;
    //                    //Initialize counter variable.
    //                    rowSpanCnt = 1;
    //                    //rowSpanItem will be current item.
    //                    rowSpanItem = i;
    //                }
    //            }
    //            i++;
    //        } //End of ForEach
    //    }
    //    ////If Segment = Medicare Part D then select the first row of the grid by default.
    //    //string SegID = ((System.Web.UI.WebControls.HiddenField)(this.SegIDMedDCovLives.FindControl("Seg_ID"))).Value;
    //    ////if (SegID == "2")
    //    ////{
    //    ////     gridMedDBenefitDesg.Items[0].Selected = true;
    //    ////}
    //}
}

