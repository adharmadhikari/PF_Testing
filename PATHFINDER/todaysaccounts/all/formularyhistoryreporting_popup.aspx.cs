using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Data;
using PathfinderModel;
using PathfinderClientModel;
using System.Collections;
using Pinsonault.Application.MarketplaceAnalytics;
using Pinsonault.Data;
using System.Text;
using Telerik.Web.UI;
using System.Configuration;
using Pinsonault.Web;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Data.SqlClient;


public partial class todaysaccounts_all_formularyhistoryreporting_popup : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        NameValueCollection queryValues = new NameValueCollection(Request.QueryString);
        GridView grid1 = (GridView)fhrGrid.FindControl("gridTemplate");
        IEnumerable<GenericDataRecord> g = null;
        Pinsonault.Application.TodaysAccounts.FormularyHistoryQueryDefinition a = new Pinsonault.Application.TodaysAccounts.FormularyHistoryQueryDefinition(queryValues);

        using (PathfinderModel.PathfinderEntities context = new PathfinderEntities())
            g = a.CreateQuery(context).Cast<GenericDataRecord>();
        
        if (g.Count() > 0)
            ProcessGrid(grid1, g, queryValues);

        //Populate Sub-header text
        using (PathfinderModel.PathfinderEntities context = new PathfinderEntities())
        {
            int sectionID = Convert.ToInt32(queryValues["Section_ID"]);
            int planID = Convert.ToInt32(queryValues["Plan_ID"]);

            string sectionName = (from d in context.SectionSet
                                  where d.ID == sectionID
                                  select d.Name).FirstOrDefault();

            string formularyName = queryValues["__options"].Trim('{', '}', '"').Replace('"', ' ').Split(':')[1].Replace("|", " ");

            string planName = (from p in context.PlanMasterSet
                               where p.Plan_ID == planID
                               select p.Plan_Name).FirstOrDefault();

            this.formularyName.InnerText = string.Format("{0} - {1} - {2}", sectionName, planName, formularyName);
            this.marketBasketName.InnerText = queryValues["Thera_Name"].Replace("|", " ");

            
        }
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
    }   

    public void ProcessGrid(GridView grid, IEnumerable<GenericDataRecord> g, NameValueCollection queryVals)
    {
        int segmentID;

        if (queryVals["Section_ID"] == "9")//Hide Tier Column for State Medicaid
        {
            segmentID = 3;

            BoundField tierColumn = grid.Columns.OfType<BoundField>().FirstOrDefault(c => c.DataField == "Tier_Name");
            if (tierColumn != null)
                tierColumn.Visible = false;
        }
        else
            segmentID = Convert.ToInt32(queryVals["Segment_ID"]);

        BoundField boundCol;

        string previousTimeFrame = Pinsonault.Application.TodaysAccounts.TodaysAccountsDataService.GetFormularyHistoryTimeframeName("P_M", segmentID);
        string currentTimeFrame = Pinsonault.Application.TodaysAccounts.TodaysAccountsDataService.GetFormularyHistoryTimeframeName("C", segmentID); 

        //Add Formulary History specific columns
        //Only add Tier column if not State Medicaid
        if (queryVals["Section_ID"] != "9")
        {
            boundCol = new BoundField();
            boundCol.DataField = "Tier_Name0";
            boundCol.HeaderText = previousTimeFrame;
            grid.Columns.Add(boundCol);

            boundCol = new BoundField();
            boundCol.DataField = "Tier_Name1";
            boundCol.HeaderText = currentTimeFrame;
            grid.Columns.Add(boundCol);
        }

        boundCol = new BoundField();
        boundCol.DataField = "Formulary_Status_Abbr0";
        boundCol.HeaderText = previousTimeFrame;
        grid.Columns.Add(boundCol);

        boundCol = new BoundField();
        boundCol.DataField = "Formulary_Status_Abbr1";
        boundCol.HeaderText = currentTimeFrame;
        grid.Columns.Add(boundCol);

        boundCol = new BoundField();
        boundCol.DataField = "PA0";
        boundCol.HeaderText = previousTimeFrame;
        grid.Columns.Add(boundCol);

        boundCol = new BoundField();
        boundCol.DataField = "PA1";
        boundCol.HeaderText = currentTimeFrame;
        grid.Columns.Add(boundCol);

        boundCol = new BoundField();
        boundCol.DataField = "QL0";
        boundCol.HeaderText = previousTimeFrame;
        grid.Columns.Add(boundCol);

        boundCol = new BoundField();
        boundCol.DataField = "QL1";
        boundCol.HeaderText = currentTimeFrame;
        grid.Columns.Add(boundCol);

        boundCol = new BoundField();
        boundCol.DataField = "ST0";
        boundCol.HeaderText = previousTimeFrame;
        grid.Columns.Add(boundCol);

        boundCol = new BoundField();
        boundCol.DataField = "ST1";
        boundCol.HeaderText = currentTimeFrame;
        grid.Columns.Add(boundCol);

        boundCol = new BoundField();
        boundCol.DataField = "Co_Pay0";
        boundCol.HeaderText = previousTimeFrame;
        grid.Columns.Add(boundCol);

        boundCol = new BoundField();
        boundCol.DataField = "Co_Pay1";
        boundCol.HeaderText = currentTimeFrame;
        grid.Columns.Add(boundCol);


        grid.DataSource = g;
        grid.DataBind();
    }
}
