using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.Sql;
using System.Data.SqlClient;
using System.ComponentModel;
using Telerik.Web.UI;

using Pinsonault.Data;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

public partial class mycampaigns_opportunitiesReal : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        dsBrands.ConnectionString = Pinsonault.Web.Session.ClientDBConnectionString;
        dsSegments.ConnectionString = Pinsonault.Web.Session.ClientDBConnectionString;
        dsMarketBasket.ConnectionString = Pinsonault.Web.Session.ClientDBConnectionString;
    }

    int _totalBrand = 0;
    public mycampaigns_opportunitiesReal()
    {
        ContainerID = "moduleOptionsContainer";
        MaxDrugs = 1;
    }

    public string ContainerID { get; set; }
    public int MaxDrugs { get; set; }  
  
    protected string CompetitorsBrandIDs
    {
        get
        {
            return NNBrandsList.Value;            
        }

    }

    protected void mbradcombobox_SelectedIndexChanged(object sender, EventArgs e)
    {
       divResult.Visible = false;     // Hide gridview section
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        string strBrandId = tempbox1.Value;
        string[] strSplitArr = strBrandId.Split(',');

        if (!string.IsNullOrEmpty(strBrandId))
        {
            _totalBrand = strSplitArr.Count();
        }

        if (_totalBrand == 0) //if no checkbox is checked
        {
            //brandscnt.Value = "1"; // set to 1 
            //_totalBrand = 1; //   set to 1  
        }
        else
        {
            brandscnt.Value = _totalBrand.ToString();
            NNBrandsList.Value = strBrandId.ToString();
        }

        IEnumerable<GenericDataRecord> g = null;
        g = GetPlanProductBrandsData(_totalBrand, strSplitArr);

        if (g.Count() > 0)
        {
            buildDynamicColumns(_totalBrand);

            radPlanProductBrandData.DataSource = g;

            radPlanProductBrandData.DataBind();
            radPlanProductBrandData.Visible = true; // Show RadGrid section   
            divResult.Visible = true; // RadGrid section   
        }

    }
    public IEnumerable<GenericDataRecord> GetPlanProductBrandsData(int totalBrands, string[] strSplitArr)
    {
        SqlConnection conn = new SqlConnection(Pinsonault.Web.Session.ClientDBConnectionString);
        SqlCommand comm = new SqlCommand();

        StringBuilder sb = new StringBuilder();
        StringBuilder sbCompProdQuery = new StringBuilder();

        sb.AppendFormat(" Select distinct brand0.Plan_Name,brand0.covered_lives,brand0.Formulary_Lives,brand0.MB_Trx,brand0.PP_MB_ID ");
        sb.AppendFormat(" ,brand0.Plan_ID, brand0.PP_Brand_ID, brand0.Segment_Id,brand0.Section_ID, brand0.Segment_Name ");

        int c = 0; //initialize product count

        while (c < totalBrands+1)
        {
            sb.AppendFormat(" ,brand{0}.PP_Brand_Name as BrandName{0} ", c);
            sb.AppendFormat(" ,brand{0}.Brand_TRx as Brand_TRx{0} ", c);
            sb.AppendFormat(" ,brand{0}.Brand_MST as Brand_MST{0} ", c);
            sb.AppendFormat(" ,brand{0}.Tier_No As TierID{0} ", c);
            sb.AppendFormat(" ,brand{0}.Tier_Name as Tier_Name{0} ", c);
            sb.AppendFormat(" ,brand{0}.Co_Pay as Co_Pay{0} ", c);

            if (c > 0)
            {                
                sbCompProdQuery.AppendFormat(" left outer join ");
            }

            sbCompProdQuery.AppendFormat(" ( Select Plan_Name,covered_lives,Formulary_Lives,MB_Trx,PP_MB_ID ");
            sbCompProdQuery.AppendFormat(" ,Plan_ID, PP_Brand_ID, Segment_Id, Section_ID, Segment_Name ");
            sbCompProdQuery.AppendFormat(" ,PP_Brand_Name ");
            sbCompProdQuery.AppendFormat(" ,Brand_TRx ");
            sbCompProdQuery.AppendFormat(" ,Brand_MST ");
            sbCompProdQuery.AppendFormat(" ,Tier_No ");
            sbCompProdQuery.AppendFormat(" ,Tier_Name ");
            sbCompProdQuery.AppendFormat(" ,Co_Pay ");
            sbCompProdQuery.AppendFormat(" from  {0}.{1}.{2}" ,"PF_Pinso", "pprx", "V_GetPlanProductBrandData");

             //where conditions
            sbCompProdQuery.AppendFormat("  where PP_MB_ID = {0} ", mbradcombobox.SelectedValue); //selected Market Basket ID   
      
            if(rcbsegment.SelectedValue != "0")
                sbCompProdQuery.AppendFormat("  and Segment_ID = {0}  ", rcbsegment.SelectedValue); //selected segmentid           
                       
            sbCompProdQuery.AppendFormat("  and Territory_ID = '{0}' " ,Pinsonault.Web.Session.TerritoryID); //loggedin user's territoryid
           
            if (c == 0) // first brand is campaign brand
            {
                sbCompProdQuery.AppendFormat(" and Is_Campaign_Brand = 1 ");
               //sbCompProdQuery.AppendFormat("  and PP_Brand_ID = {0} ", "17"); //Campaign BrandID for selected market basket id
            }
            else
            {
                sbCompProdQuery.AppendFormat(" and Is_Campaign_Brand = 0 ");
                sbCompProdQuery.AppendFormat("  and PP_Brand_ID = {0} ", strSplitArr[c-1]); //pass the products in loop
            }

            sbCompProdQuery.AppendFormat(" )brand{0}", c);

            if (c > 0)
            {
                sbCompProdQuery.AppendFormat(" on brand0.PP_MB_ID = brand{0}.PP_MB_ID ",c); 
                sbCompProdQuery.AppendFormat(" and brand0.Segment_ID = brand{0}.Segment_ID " ,c);
                sbCompProdQuery.AppendFormat(" and brand0.Plan_ID = brand{0}.Plan_ID ", c);
            }

            c++;    
        }
        sb.AppendFormat(" from {0} ", sbCompProdQuery);        

        comm.CommandText = string.Format(" SELECT RANK() OVER(ORDER BY MB_Trx desc ) PlanRank, *  from ({0}) vfinal", sb.ToString());        
        comm.Connection = conn;

        conn.Open();
        DbDataReader rdr = comm.ExecuteReader();
        IEnumerable<GenericDataRecord> g = Pinsonault.Data.GenericDataRecord.CreateCollection(rdr);
        conn.Close();

        return g;
    }

    void buildDynamicColumns(int total_Brands)
    {
        GridTemplateColumn column;
        string BrandName;
        //hide existing columns since they may not be needed

        for (int i = 11; i < radPlanProductBrandData.Columns.Count; i++)
        {
            radPlanProductBrandData.Columns[i].Visible = false;
        }

        //make sure template columns are generated for each brand and set visible

        for (int i = 0 ; i < total_Brands +1 ; i++)
        {
            BrandName = string.Format("brandname{0}",i);                
            column = AddColumn(radPlanProductBrandData, BrandName, "~/powerplanrx/controls/opportunityBrandHeaderTemplate.ascx", "~/powerplanrx/controls/opportunityBrandTemplate.ascx");
            column.Visible = true;
        }

    }
    public static GridTemplateColumn AddColumn(RadGrid grid, string name, string headerTemplate, string dataTemplate)
    {
        GridTemplateColumn column = null;

        column = grid.Columns.FindByUniqueNameSafe(name) as GridTemplateColumn;
        if (column == null)
        {
            //must add column to collection before setting properties
            column = new GridTemplateColumn();
            grid.Columns.Add(column);

            column.UniqueName = name;
        }
        column.HeaderTemplate = grid.Page.LoadTemplate(headerTemplate);
        column.ItemTemplate = grid.Page.LoadTemplate(dataTemplate);
        //column.HeaderStyle.Width = 100;
        //column.ItemStyle.Width = 100;

        return column;
    }

    protected void onSort(object sender, EventArgs e)
    {
        string strBrandId = "";
        strBrandId = tempbox1.Value;
        char[] separator = new char[] { ',' };
        string[] strSplitArr = strBrandId.Split(separator);

        if (strBrandId.ToString() != "")
        {
            _totalBrand = strSplitArr.Length;
        }

        buildDynamicColumns(_totalBrand);
    } 

}

