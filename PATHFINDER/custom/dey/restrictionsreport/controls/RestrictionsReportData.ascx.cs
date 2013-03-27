using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Data.Objects;
using System.ComponentModel;
using System.Data.Common;
using Telerik.Web.UI;
using PathfinderModel;

public partial class restrictionsreport_controls_MedicalPharmacyCoverageData : System.Web.UI.UserControl, IReportGrid
{
    #region IReportGrid Members

    public void ProcessGrid(IList<DbDataRecord> Data, string Title, string Region)
    {
        RadGrid grid;

        if ( string.Compare(Region, "US", true) == 0 )
        {
            grid = dataUS.HostedGrid;
            dataUS.GridLabel = Title;
            dataUS.Visible = true;            
        }
        else
        {
            grid = dataStateTerr.HostedGrid;
            dataStateTerr.GridLabel = Title;
            dataStateTerr.Visible = true;
        }

        Pinsonault.Web.Support.RegisterComponentWithClientManager(this.Page, grid.ClientID);

        addColumns(grid);

        grid.DataSource = Data;
        grid.DataBind();
    }

    #endregion

    void addColumns(RadGrid grid)
    {
        Telerik.Web.UI.GridHyperLinkColumn col;
        using (PathfinderEntities context = new PathfinderEntities())
        {
            //Show or hide the Section column based on multiple segment selection
            NameValueCollection nvc = new NameValueCollection(Request.QueryString);
            bool multipleSections = false;

            if (nvc["Section_ID"].Contains(','))
                multipleSections = true;

            grid.Columns[1].Visible = multipleSections;

            //Get all restriction criteria
            var q = (from p in context.LkpRestrictionCriteriaCombinationsSet
                     where p.Client_ID == Pinsonault.Web.Session.ClientID
                     select new { p.Criteria_Id, p.Criteria_Name }).Distinct().AsEnumerable().OrderBy(p => p.Criteria_Id);

            foreach (var r in q)
            {
                col = new Telerik.Web.UI.GridHyperLinkColumn();
                grid.Columns.Add(col);

                //col.DataField = string.Format("T{0}", tier.ID);
                col.DataTextField = string.Format("P{0}", r.Criteria_Id);
                col.DataTextFormatString = "{0:n2}%";
                col.DataNavigateUrlFields = new string[] { "Geography_ID", "Drug_ID", "Drug_Name", "Section_ID", "Section_Name" };
                col.DataNavigateUrlFormatString = "javascript:restrictionsReportDrilldown(\"{0}\",{1}," + r.Criteria_Id + ",\"{2}\",\"" + r.Criteria_Name + "\",{3},\"{4}\",\"" + multipleSections + "\")";
                //col.DataFormatString = "{0:n2}";
                col.HeaderText = r.Criteria_Name;
                col.UniqueName = col.DataTextField;
                col.SortExpression = col.DataTextField;
                col.ItemStyle.CssClass = "alignRight";

                col.HeaderStyle.Width = Unit.Pixel(70);
                col.ItemStyle.ForeColor = System.Drawing.Color.Orange;
            }
        }
    }
}
