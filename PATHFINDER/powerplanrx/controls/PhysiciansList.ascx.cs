using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Pinsonault.Application.PowerPlanRx;

public partial class controls_PhysiciansList : System.Web.UI.UserControl
{
    protected override void OnInit(EventArgs e)
    {
        Pinsonault.Web.Session.CheckSessionState();          
    }
    private void Page_Load(object sender, System.EventArgs e)
    {
        DataSet dsPhys = Campaign.GetPhysList(Request.QueryString["dist"].ToString(), Convert.ToInt32(Request.QueryString["id"]));
        frmPhysician.DataSource = dsPhys;
        frmPhysician.DataBind();
       
        string col1 = "Territory_ID";
        string col2 = "Territory_Name";
        
        InsertSubHeading(ref dsPhys, col1, col2);
        if (dsPhys.Tables[0].Rows.Count > 0)
        {
            grdPhysicians.DataSource = dsPhys;
            grdPhysicians.DataBind();
        }
        else
        {
            grdPhysicians.Visible = false;
            Parent.FindControl("divExport").Visible = false;            
        }
    }
    /// <summary>
    /// For inserting a new row before another (different)Territory ID.
    /// </summary>
    /// <param name="ds"></param>
    /// <param name="columnName1"></param>
    /// <param name="columnName2"></param>
    public void InsertSubHeading(ref DataSet ds, string columnName1, string columnName2)
    {
        int i = 0;
        string prevsub = "";
        while (i <= ds.Tables[0].Rows.Count - 1)
        {
            DataRow dr = ds.Tables[0].Rows[i];
            string subheading = dr[columnName1].ToString();
            if (subheading != prevsub)
            {
                dr[0] = "";
                prevsub = subheading;
                DataRow newrow = ds.Tables[0].NewRow();
                newrow[0] = "Territory: " + subheading + " " + dr[columnName2].ToString();
                ds.Tables[0].Rows.InsertAt(newrow, i);
               
                DataRow newrowSubHeading = ds.Tables[0].NewRow();
                
                newrowSubHeading[0] = string.Format("{0}", "PINSO_ID");
                //newrowSubHeading[1
                ds.Tables[0].Rows.InsertAt(newrowSubHeading, i+1);
                i= i+2;
            }
            else
            {
                dr[0] = "";
            }
            i++;
        }
    }
    protected void grdPhysicians_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            if (e.Item.Cells[0].Text.Contains("Territory"))
            {
                //provide the font or color   ;
                e.Item.Cells[0].CssClass = "terrHeader" ;
                
                e.Item.Cells[0].ColumnSpan = 4;
                //remove extra cells
                e.Item.Cells.Remove(e.Item.Cells[4]);
                e.Item.Cells.Remove(e.Item.Cells[3]);
                e.Item.Cells.Remove(e.Item.Cells[2]);
                e.Item.Cells.Remove(e.Item.Cells[1]);
            }
            else if (e.Item.Cells[0].Text.Contains("PINSO_ID"))
            {
                e.Item.Cells[1].Text = "Physician Name";
                e.Item.Cells[2].Text = "Market Volume";
                e.Item.Cells[3].Text = "Brand Trx";

                e.Item.Cells[0].CssClass = "physListHeader";
                e.Item.Cells[1].CssClass = "physListHeader";
                e.Item.Cells[2].CssClass = "physListHeader";
                e.Item.Cells[3].CssClass = "physListHeader";
                e.Item.Cells.Remove(e.Item.Cells[4]);
            }

            else
            {
                e.Item.Cells.Remove(e.Item.Cells[0]);
            }
        }
        else if (e.Item.ItemType == ListItemType.Header)
        {
            
            e.Item.Visible = false;
        }
    }
}
