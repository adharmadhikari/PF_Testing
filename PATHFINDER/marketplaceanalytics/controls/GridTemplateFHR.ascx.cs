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
using System.Collections;

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

    protected void drillDownContainer_PreRender(object sender, EventArgs e)
    {
        lblNote.Text = Resources.Resource.Label_FHR_Note;

        GridView gv = (GridView)sender;

        //This replaces <td> with <th> and adds the scope attribute
        //gv.UseAccessibleHeader = true;

        //if (gv.HeaderRow != null)
        //    gv.HeaderRow.TableSection = TableRowSection.TableHeader;//This will add the <thead> and <tbody> elements

        
        //Change background color of cell if there is a change in Formulary Data
        foreach (GridViewRow gvRow in gv.Rows)
        {
            if (gvRow.RowType == DataControlRowType.DataRow)
            {
                //Change Forecolor for selected drug from grid
                if (String.Compare(gv.DataKeys[gvRow.RowIndex].Value.ToString(), Request.QueryString["Selected_Product_ID"], true) == 0)
                {
                    //gvRow.BackColor = System.Drawing.Color.FromArgb(172, 199, 255);
                    gvRow.CssClass = "fhrSelectedDrug";
                }

                //Tier
                //Only process Tier column if not State Medicaid
                if (Request.QueryString["Section_ID"] != "9")
                {
                    if (gvRow.Cells[getColumnID("TierChanged", gv)].Text == "1")
                        gvRow.Cells[getColumnID("Tier_Name1", gv)].BackColor = System.Drawing.Color.Yellow;
                }
                //PA
                if (gvRow.Cells[getColumnID("PAChanged", gv)].Text == "1")
                    gvRow.Cells[getColumnID("PA1", gv)].BackColor = System.Drawing.Color.Yellow;

                //QL
                if (gvRow.Cells[getColumnID("QLChanged", gv)].Text == "1")
                    gvRow.Cells[getColumnID("QL1", gv)].BackColor = System.Drawing.Color.Yellow;

                //ST
                if (gvRow.Cells[getColumnID("STChanged", gv)].Text == "1")
                    gvRow.Cells[getColumnID("ST1", gv)].BackColor = System.Drawing.Color.Yellow;

                //Copay
                if (gvRow.Cells[getColumnID("CopayChanged", gv)].Text == "1")
                    gvRow.Cells[getColumnID("Co_Pay1", gv)].BackColor = System.Drawing.Color.Yellow;

                //Formulary Status
                if (gvRow.Cells[getColumnID("FSChanged", gv)].Text == "1")
                    gvRow.Cells[getColumnID("Formulary_Status_Abbr1", gv)].BackColor = System.Drawing.Color.Yellow;
            }

        }
    }

    protected void drillDownContainer_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //For each desired row, create a new FormatCells List
        //FormatCells.Add(<column number>,<header name,colspan,rowspan>)
        SortedList FormatCells = new SortedList();
        FormatCells.Add("1", ",1,1");
        FormatCells.Add("2", string.Format("{0},1,1",Request.QueryString["Trx_Mst"]));
        if (Request.QueryString["Section_ID"] != "9") //Only process tier column if not state medicaid
            FormatCells.Add("3", "Tier,2,1");
        FormatCells.Add("4", "Status,2,1");
        FormatCells.Add("5", "PA,2,1");
        FormatCells.Add("6", "QL,2,1");
        FormatCells.Add("7", "ST,2,1");
        FormatCells.Add("8", "Copay,2,1");

        GetMultiRowHeader(e, FormatCells );

        //Add column border
        if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
        {

            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                if (i == 7 || i == 8 || i == 10 || i == 12 || i == 14 || i == 16 || i == 18 || i == 20)
                    e.Row.Cells[i].CssClass = "fhrBorderLeft";
            }
        }
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

    public void GetMultiRowHeader(GridViewRowEventArgs e, SortedList GetCels)
    {

        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridViewRow row;
            IDictionaryEnumerator enumCels = GetCels.GetEnumerator();

            row = new GridViewRow(-1, -1, DataControlRowType.Header, DataControlRowState.Normal);
            while (enumCels.MoveNext())
            {

                string[] count = enumCels.Value.ToString().Split(Convert.ToChar(","));
                TableHeaderCell Cell;
                Cell = new TableHeaderCell();
                Cell.RowSpan = Convert.ToInt16(count[2].ToString());
                Cell.ColumnSpan = Convert.ToInt16(count[1].ToString());
                Cell.Controls.Add(new LiteralControl(count[0].ToString()));
                Cell.HorizontalAlign = HorizontalAlign.Center;
                //Cell.ForeColor = System.Drawing.Color.White;
                Cell.CssClass = "doubleHeader";
                if (string.IsNullOrEmpty(count[0].ToString()))
                    Cell.CssClass = "doubleHeader";
                else
                    Cell.CssClass = "doubleHeader fhrBorderLeft";
                row.Cells.Add(Cell);
            }


            e.Row.Parent.Controls.AddAt(0, row);

        }
    } 
}
