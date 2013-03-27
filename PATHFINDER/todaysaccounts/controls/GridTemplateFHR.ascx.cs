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

public partial class todaysaccounts_controls_GridTemplate : System.Web.UI.UserControl
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
        GridView gv = (GridView)sender;

        //This replaces <td> with <th> and adds the scope attribute
        //gv.UseAccessibleHeader = true;

        //if (gv.HeaderRow != null)
        //    gv.HeaderRow.TableSection = TableRowSection.TableHeader;//This will add the <thead> and <tbody> elements

        lblNote.Text = Resources.Resource.Label_FHR_Note;

        
        foreach (GridViewRow gvRow in gv.Rows)
        {
            //Change background color of cell if there is a change in Formulary Data
            if (gvRow.RowType == DataControlRowType.DataRow)
            {
                
                //Tier
                //Only process Tier column if not State Medicaid
                if (Request.QueryString["Section_ID"] != "9")
                {
                    if (gvRow.Cells[getColumnID("TierChanged", gv)].Text == "1")
                    {
                        string ss = gvRow.Cells[getColumnID("TierChanged", gv)].Text;
                        gvRow.Cells[getColumnID("Tier_Name1", gv)].BackColor = System.Drawing.Color.Yellow;
                    }
                }
                //PA
                if (gvRow.Cells[getColumnID("PAChanged", gv)].Text == "1")
                {
                    string ss = gvRow.Cells[getColumnID("PAChanged", gv)].Text;
                    gvRow.Cells[getColumnID("PA1", gv)].BackColor = System.Drawing.Color.Yellow;
                }

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
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //For each desired row, create a new FormatCells List
            //FormatCells.Add(<column number>,<header name,colspan,rowspan>)
            SortedList FormatCells = new SortedList();
            FormatCells.Add("1", ",1,1");
            if (Request.QueryString["Section_ID"] != "9") //Only process tier column if not state medicaid
                FormatCells.Add("2", "Tier,2,1");
            FormatCells.Add("3", "Status,2,1");
            FormatCells.Add("4", "PA,2,1");
            FormatCells.Add("5", "QL,2,1");
            FormatCells.Add("6", "ST,2,1");
            FormatCells.Add("7", "Copay,2,1");

            GetMultiRowHeader(e, FormatCells);
        }

        //Add column border
        if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
        {

            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                if (i == 7 || i == 9 || i == 11 || i == 13 || i == 15 || i == 17)
                    e.Row.Cells[i].CssClass = "fhrBorderLeft";
            }
        }
    }

    public int getColumnID(string columnName, GridView grid)
    {
        int columnID = 0;

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
