using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Pinsonault.Web;
using System.Collections.Specialized;
using System.Data.Objects;
using System.ComponentModel;
using System.Data.Common;
using PathfinderModel;

public partial class standardreports_controls_FormularyStatusData : System.Web.UI.UserControl, IReportGrid
{
    #region IReportGrid Members

    public void ProcessGrid(IList<System.Data.Common.DbDataRecord> Data, string Title, string region)
    {
        RadGrid grid  = null;
        if ( string.Compare(region, "US", true) == 0 )
        {
            grid = gridNational.HostedGrid;
            gridNational.GridTitle = Title;
            gridNational.Visible = true;            
        }
        else
        {
            grid = gridRegional.HostedGrid;
            gridRegional.GridTitle = Title;
            gridRegional.Visible = true;
        }

        Support.RegisterComponentWithClientManager(this.Page, grid.ClientID);
        addColumns(grid, Data);
        grid.DataSource = Data;
        grid.DataBind();
    }

    #endregion
    void addColumns(RadGrid grid, IList<DbDataRecord> Data)
    {


        Telerik.Web.UI.GridHyperLinkColumn col;

        IList<CoverageStatus> coverageStatus = null;
        if (coverageStatus == null)
        {
            using (PathfinderModel.PathfinderEntities context = new PathfinderModel.PathfinderEntities())
            {
                coverageStatus = context.CoverageStatusSet.OrderBy(cs => cs.ID).ToList();
            }
        }

        foreach (CoverageStatus cs in coverageStatus)
        {
            bool IsColumnVisible = false;
            foreach (DbDataRecord record in Data)
            {
                if (cs.ID != 4)
                {
                    decimal statusPercent = Convert.ToDecimal(record.GetValue(record.GetOrdinal(string.Format("F{0}_Lives", cs.ID))));
                    if (statusPercent > 0) IsColumnVisible = true;
                }
                else
                {
                    decimal statusPercent = Convert.ToDecimal(record.GetValue(record.GetOrdinal(string.Format("Other_Lives"))));
                    if (statusPercent > 0) IsColumnVisible = true;
                }

            }
            if (IsColumnVisible) //if all the records for a column are having value > 0 then add the column else don't add it
            {

                col = new Telerik.Web.UI.GridHyperLinkColumn();
                grid.Columns.Add(col);
                if (cs.ID != 4)
                {
                    col.DataTextField = string.Format("F{0}", cs.ID);
                }
                else
                {
                    col.DataTextField = string.Format("F5");
                }
                col.DataTextFormatString = "{0:n2}%";
                string text="";
                if (cs.ID == 1)
                text = Resources.Resource.Label_Formulary_Status_1;
                if (cs.ID == 2)
                    text = Resources.Resource.Label_Formulary_Status_2;
                if (cs.ID == 3)
                    text = Resources.Resource.Label_Formulary_Status_3;
                if (cs.ID == 4)
                    text = Resources.Resource.Label_Formulary_Status_5;
                if (!string.IsNullOrEmpty(Request.QueryString["Section_ID"]))
                {
                    col.DataNavigateUrlFields = new string[] { "Geography_ID", "Drug_ID", "Drug_Name", "Section_ID" };
                    col.DataNavigateUrlFormatString = "javascript:gridFSDrilldown_setfilter({1}, \"{2}\"," + cs.ID + ",f" + cs.ID + "Text, \"{0}\",{3})";
                    if (cs.ID == 4)
                        col.DataNavigateUrlFormatString = "javascript:gridFSDrilldown_setfilter({1}, \"{2}\"," + cs.ID + ",f5Text, \"{0}\",{3})";
                }
                else
                {
                    col.DataNavigateUrlFields = new string[] { "Geography_ID", "Drug_ID", "Drug_Name" };
                    col.DataNavigateUrlFormatString = "javascript:gridFSDrilldown_setfilter({1}, \"{2}\"," + cs.ID + ",f"+cs.ID+"Text, \"{0}\",0)";
                    if (cs.ID == 4)
                        col.DataNavigateUrlFormatString = "javascript:gridFSDrilldown_setfilter({1}, \"{2}\"," + cs.ID + ",f5Text, \"{0}\",0)";
              }
                
                col.HeaderText = text;
                col.UniqueName = col.DataTextField;
                col.SortExpression = col.DataTextField;
                col.ItemStyle.CssClass = "alignRight";
                col.ItemStyle.Width = Unit.Percentage(9);
            }
        }
    }
}
