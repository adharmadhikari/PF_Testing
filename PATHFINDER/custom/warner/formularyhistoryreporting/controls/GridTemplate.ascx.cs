using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class custom_warner_formularyhistoryreporting_controls_GridTemplate : System.Web.UI.UserControl
{
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
    }

    public RadGrid HostedGrid
    {
        get { return gridTemplate; }
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


    protected void gridTemplate_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //NameValueCollection queryValues = new NameValueCollection(Request.QueryString);

        // if (e.Row.RowType == DataControlRowType.DataRow)
        // {  
        //    var item = e.Row.DataItem;
        //    string[] drugIDarr = queryValues["Drug_ID"].Split(',');
        //    int c = drugIDarr.Count();
        //    for (int i = 0; i < c; i++)
        //    {
        //        HyperLink link = (HyperLink)e.Row.FindControl("Drug" + (i + 1) + "_TotalCovered1"); 
        //    }
        //     //var linkCol = (HyperLinkField)e.Row.Cells[yourHyperLinkIndex];  
        //     //linkCol.NavigateUrl = item["columnName"];   
        // } 
    }
}
