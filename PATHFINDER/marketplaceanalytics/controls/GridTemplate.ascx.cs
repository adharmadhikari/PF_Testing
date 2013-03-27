using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using PathfinderClientModel;
using Pinsonault.Web;
using System.IO;
using Telerik.Web.UI;
using Pinsonault.Application.MarketplaceAnalytics;

public partial class marketplaceanalytics_controls_GridTemplate : System.Web.UI.UserControl
{

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
    }
    //public override Telerik.Web.UI.RadGrid HostedGrid
    //public Telerik.Web.UI.RadGrid HostedGrid
    //{
    //    get { return gridTemplate; }
    //}
    //public DataGrid HostedGrid
    //{
    //    get { return gridTemplate; }
    //}

    public GridView HostedGrid
    {
        get { return gridTemplate; }
    }

    protected void drillDownContainer_DataBound(object sender, EventArgs e)
    {
        //Color code changes if FHR report
        //if (Session["FHR_Changes"] != null)
        //{
        //    GridView g = (GridView)sender;
        //    List<FHRChanges> changes = (List<FHRChanges>)Session["FHR_Changes"];

        //    foreach (GridViewRow gvr in g.Rows)
        //    {
        //        if (gvr.RowType == DataControlRowType.DataRow)
        //        {
        //            int product_id = Convert.ToInt32(gvr.Cells[getColumnID("Product_ID", g)].Text);
        //            int cellCount = gvr.Cells.Count;

        //            for (int x = 0; x < cellCount; x++)
        //            {
        //                string headerText = g.HeaderRow.Cells[x].Text;
        //                int? timeframe;

        //                using (PathfinderMarketplaceAnalyticsEntities context = new PathfinderMarketplaceAnalyticsEntities(Pinsonault.Web.Session.GetClientApplicationConnectionString("MarketplaceAnalytics")))
        //                {
        //                    if (headerText.IndexOf('Q') > -1)
        //                    {
        //                        timeframe = (from p in context.FHQuarterYearsSet
        //                                     where p.Name == headerText
        //                                     select p.ID).FirstOrDefault();
        //                    }
        //                    else
        //                    {
        //                        timeframe = (from p in context.FHMonthYearsSet
        //                                     where p.Name == headerText
        //                                     select p.ID).FirstOrDefault();
        //                    }                            
        //                }

        //                if (timeframe != null)
        //                {
        //                    int? changed = 0;
        //                    changed = changes.Where(t => t.ID == product_id).Where(t => t.Timeframe == timeframe).Select(t => t.Changed).FirstOrDefault();

        //                    if (changed > 0)
        //                    {
        //                        gvr.Cells[x].BackColor = System.Drawing.Color.Yellow;
        //                    }
        //                }                  
        //            }
        //        }
        //    }
        //}
        //Session["FHR_Changes"] = null;
    }

    public int getColumnID(string columnName, GridView grid)
    {
        int columnID = 0;
        //foreach (DataControlField column in grid.Columns)
        //{
        //    if (string.Compare(column.HeaderText, columnName, true) == 0)
        //        columnID = grid.Columns.IndexOf(column);
        //}
        for (int i = 0; i < grid.Columns.Count; i++)
        {
            BoundField bf = grid.Columns[i] as BoundField;
            if (bf != null && bf.DataField == columnName)
                columnID = i;
        }


        return columnID;
    }

    protected void drillDownContainer_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;

        //This replaces <td> with <th> and adds the scope attribute
        gv.UseAccessibleHeader = true;

        if (gv.HeaderRow != null)
            gv.HeaderRow.TableSection = TableRowSection.TableHeader;//This will add the <thead> and <tbody> elements

        int index = 0;

        foreach (GridViewRow gvRow in gv.Rows)
        {
            int num;
            bool isNum = int.TryParse(gvRow.Cells[0].Text, out num);

            if (isNum)
            {
                gvRow.Cells[0].BackColor = Pinsonault.Data.Reports.ReportColors.StandardReports.GetColor(index % 6);
                gvRow.Cells[0].Text = "&nbsp;";

                index++;
            }
        }
    }
}
