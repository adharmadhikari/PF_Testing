using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;
using Pathfinder;
using System.Data;
using System.Reflection;
using Pinsonault.Data.Reports;
using Pinsonault.Web.UI;
using System.Collections;

public partial class Controls_PrintExport : System.Web.UI.UserControl
{
      
    protected override void OnLoad(EventArgs e)
    {
        String report = Request.QueryString["report"];
        String tile = Request.QueryString["tile"];
        ExporterBase exporter = ExporterBase.CreateInstance<HtmlExporter>(report, Request.QueryString);
        IList<ReportSubsection> ReportSubsections = exporter.ReportSubsections;
        ltTitle.Text = exporter.Title;

        if (exporter.ReportSubsections.Count > 0)
        {
            int iSection = 0;
            foreach (ReportSubsection subsection in ReportSubsections)
            {
                AddSectionTitle(subsection, exporter, iSection);

                if ( subsection.IsImage() )
                {
                    Image imgChart = new Image();
                    pnlPrintContent.Controls.Add(imgChart);
                    imgChart.ImageUrl = Page.ResolveUrl(string.Format("~/usercontent/chart.ashx?id={0}&type=jpeg", subsection.ChartID)); //subsection.ImageUrl;
                }

                switch(subsection.ReportDefinition.Style)
                {
                    case ReportStyle.Chart:
                        Image imgChart = new Image();
                        pnlImage.Controls.Add(imgChart);
                        imgChart.ImageUrl = subsection.ImageUrl;
                        break;

                    case ReportStyle.Grid:                        
                        CreateDataGridView(exporter, subsection, iSection, false);
                        break;
                    case ReportStyle.List:
                        CreateDataList(exporter, subsection, iSection);
                        break;
                    case ReportStyle.DualHeaderGrid:
                        CreateDataGridView(exporter, subsection, iSection,true);
                        break;
                }                
                iSection++;

                //if (iSection == exporter.ReportSubsections.Count && subsection.Name == "Benefit Design - Drug Level")
                //{
                //    AddFooter(subsection);
                //}
            }

         }       
    }

    /// <summary>
    /// For Adding section Title with selected criteria
    /// </summary>
    /// <param name="subsection"></param>
    /// <param name="exporter"></param>
    /// <param name="iSection"></param>
    public void AddSectionTitle(ReportSubsection subsection, ExporterBase exporter, int iSection)
    {       
       Literal ltSectionTitle = new Literal();
       ltSectionTitle.ID = "ltSectionTitle" + iSection;
       ltSectionTitle.Text = "<h4>" + ExporterBase.GetCriteriaDetails(subsection, subsection.ReportDefinition.SectionTitle, ReportType.Print, exporter.ReportDate) + "</h4>";
       pnlPrintContent.Controls.Add(ltSectionTitle);
    }

    public void AddFooter(ReportSubsection subsection)
    {
        //log.Debug("Updating Footer...");
        string criteria = subsection.CriteriaItems["section_id"].Text;

        string footer = string.Empty;
        if (criteria.IndexOf("DoD") != -1)
            footer = Pinsonault.Web.Support.GetDataUpdateDateByKey("DoD Formulary", Resources.Resource.Label_Section_Last_Updated);

        if (criteria.IndexOf("Commercial") != -1 || criteria.IndexOf("PBM") != -1 || criteria.IndexOf("FEP") != -1 || criteria.IndexOf("Managed Medicaid") != -1)
            footer = Pinsonault.Web.Support.GetDataUpdateDateByKey("Commercial Formulary", Resources.Resource.Label_Section_Last_Updated);

        if (criteria.IndexOf("Part D") != -1)
            footer = Pinsonault.Web.Support.GetDataUpdateDateByKey("Part-D Formulary", Resources.Resource.Label_Section_Last_Updated);

        if (criteria.IndexOf("State Medicaid") != -1)
            footer = Pinsonault.Web.Support.GetDataUpdateDateByKey("State Medicaid Formulary", Resources.Resource.Label_Section_Last_Updated);

        if (!string.IsNullOrEmpty(footer))
        {
            footer = footer + ". Data subject to change";
            Literal ltFooter = new Literal();
            ltFooter.Text = "<h4>" + footer + "</h4>";
            pnlPrintContent.Controls.Add(ltFooter);

        }
    }
    /// <summary>
    /// For creating grid view and adding columns
    /// </summary>
    /// <param name="exporter"></param>
    /// <param name="subsection"></param>
    /// <param name="strGridName"></param>
    public void CreateDataGridView(ExporterBase exporter, ReportSubsection subsection, int iSection, bool IsDualHeaderGrid)
    {
        //add gridview in panel control
       
        //GridView grvSubsection = new GridView();
        HtmlGrid grvSubsection = new HtmlGrid();

        string strGridName = "grvSubsection" + iSection.ToString();
        grvSubsection.ID = strGridName;
        //grvSubsection.AutoGenerateColumns = false;
        pnlPrintContent.Controls.Add(grvSubsection);
        grvSubsection.Visible = true;
        grvSubsection.CssClass = "htmlGrid";       

        //bind the data
        grvSubsection.DataSource = subsection.Data;
        grvSubsection.ColumnSource = subsection.ColumnMap;
        grvSubsection.IsDualHeaderGrid = IsDualHeaderGrid;
      
        //Bind the Grid.
        grvSubsection.DataBind();
        
    }
    /// <summary>
    /// This function adds a grid view after pivoting the columns in to rows 
    /// </summary>
    /// <param name="exporter"></param>
    /// <param name="subsection"></param>
    /// <param name="iSection"></param>
    public void CreateDataList(ExporterBase exporter, ReportSubsection subsection, int iSection)
    {
        //Type type;
        //string propValue;
        string strCol1 = "name"; //column name in the grid 
        string strCol2 = "value"; //column name in the grid having value of column1 

        //create a dataset from the subsection data and pivot the column in to rows
        DataSet ds = new DataSet();
        string strTableName = "subsection" + iSection.ToString();
        DataTable dt = ds.Tables.Add(strTableName);
        dt.Columns.Add(strCol1);
        dt.Columns.Add(strCol2);

        //foreach (Object elem in subsection.Data)
        foreach (System.Data.Common.DbDataRecord elem in subsection.Data)
        {
           //Type type = elem.GetType();
           List<string> properties = new List<string>();

           for (int s = 0; s < elem.FieldCount; s++)
               properties.Add(elem.GetName(s));

           foreach (ColumnMap map in subsection.ColumnMap)
           {
                 //PropertyInfo pi = type.GetProperty(map.PropertyName);

                 //if (pi != null)
                 //{  
               IEnumerable<string> results = properties.Where(p => p.Equals(map.PropertyName));
               if (results.Count() > 0)
               {
                   //object value = pi.GetValue(elem, null);
                   //propValue = value != null ? value.ToString() : "";
                   String fieldName = results.Single();
                   int fieldOrdinal = elem.GetOrdinal(fieldName);
                   String fieldValue = elem.GetValue(fieldOrdinal).ToString();

                   //if (!string.IsNullOrEmpty(propValue))
                   if (!string.IsNullOrEmpty(fieldValue))
                   {
                       //if ( !String.IsNullOrEmpty(map.DataFormat) )
                       //    propValue = String.Format(map.DataFormat, propValue);

                       if (!String.IsNullOrEmpty(map.DataFormat))
                       {
                           int intOutput = 0;
                           double dOutput = 0;
                           if (Int32.TryParse(fieldValue, out intOutput))
                           {
                               fieldValue = String.Format(map.DataFormat, Convert.ToInt32(fieldValue));
                           }
                           else if(Double.TryParse(fieldValue, out dOutput))
                           {
                              fieldValue = String.Format(map.DataFormat, Convert.ToDecimal(fieldValue));
                           } 
                       }
                       DataRow dr = dt.NewRow();
                       dr[0] = map.TranslatedName;
                       //dr[1] = propValue;
                       dr[1] = fieldValue;
                       dt.Rows.Add(dr);
                   }
               }
                 //}               
           }
        }
        //create a gridview and bind it with the dataset as created above
        GridView grvSubsectionList = new GridView();      
        grvSubsectionList.ID = "grvSubsectionList" + iSection.ToString();

        //the newly created dataset will be the datasource for gridview
        grvSubsectionList.DataSource = ds;
        grvSubsectionList.AutoGenerateColumns = true;
        grvSubsectionList.ShowHeader = false;
        grvSubsectionList.BorderStyle = BorderStyle.Solid;       
        grvSubsectionList.Visible = true;       

        //Bind the GridView.      
        grvSubsectionList.DataBind();

        //add the grid view in asp panel control
        pnlPrintContent.Controls.Add(grvSubsectionList);
        ds.Clear();       
    }
  
}
