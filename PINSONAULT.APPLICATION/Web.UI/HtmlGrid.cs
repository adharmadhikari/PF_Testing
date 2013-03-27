using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Web.UI;
using System.ComponentModel;
using Pinsonault.Data.Reports;
using System.Drawing;

namespace Pinsonault.Web.UI
{
    /// <summary>
    /// Renders a HTML Table based on a specified data source with columns automatically generated using a ColumnMap list.  This control was created for the purpose of supporting Print export feature but can be used for any web page.
    /// </summary>
    public class HtmlGrid : System.Web.UI.WebControls.DataBoundControl
    {
        IList<ColumnMap> _columns = new List<ColumnMap>();
        public IList<ColumnMap> Columns
        {
            get { return _columns; }
        }

        public IList<ColumnMap> ColumnSource
        {
            get { return Columns; }
            set
            {
                if ( value != null )
                    _columns = new List<ColumnMap>(value);
                else
                    throw new ArgumentNullException();
            }
        }

        /// <summary>
        /// property for defining if the grid is dual header or not
        /// </summary>
        public bool IsDualHeaderGrid { get; set; }

        IEnumerable _data = null;
        protected override void PerformDataBinding(System.Collections.IEnumerable data)
        {
            _data = data;
        }

        protected override HtmlTextWriterTag TagKey
        {
            get { return HtmlTextWriterTag.Table; }
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");

            base.AddAttributesToRender(writer);
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            PropertyDescriptorCollection properties = null;
            PropertyDescriptor property;
            object value;
            bool ADD_Lives_Total_Row = false;
            int[] Lives_Y_Col;
            Lives_Y_Col = new int[100];
            int col = 0;
            //add header
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            foreach ( ColumnMap column in Columns )
            {
                
                //if dual header and colspan > 1 
                if (column.MergedCellSpan > 0 && IsDualHeaderGrid && !(string.IsNullOrEmpty(column.FirstHeaderTranslatedName)))
                {
                    //populate column name only if it is not a repetitive column name
                    if (column.HeaderRepeaterCell != "R")
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Colspan, Convert.ToString(column.MergedCellSpan));
                        writer.RenderBeginTag(HtmlTextWriterTag.Th);
                        writer.Write(column.FirstHeaderTranslatedName);
                        writer.RenderEndTag();
                    }
                }
                else
                {

                    // sl 4/10/2012 hide TA: Lives/Formulary Tab - Covered Lives: Covered_Lives_Order column
                    if (column.PropertyName.ToLower() == "covered_lives_order")
                    {
                        //writer.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
                        //writer.RenderBeginTag(HtmlTextWriterTag.Th);
                        //writer.RenderEndTag();
                    }
                    else
                    {
                        if (column.TranslatedName.Contains("HasSum"))
                        {
                            Lives_Y_Col[col] = col;
                            ADD_Lives_Total_Row = true;
                        }
                        writer.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid");
                        writer.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px");
                        writer.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, "#cccccc");
                        writer.RenderBeginTag(HtmlTextWriterTag.Th);
                        if (column.TranslatedName.IndexOf("|HIGHLIGHT|") > -1) //User for FHR Highlighting
                            writer.Write(column.TranslatedName.Replace("|HIGHLIGHT|", ""));
                        else if (column.TranslatedName.IndexOf("HasSum") > -1) //If column is HasSum i.e Get Total of that Column in excel remove HasSum tag to display proper Heading
                           writer.Write(column.TranslatedName.Replace("HasSum", ""));
                        else if (string.Compare(column.TranslatedName, "key", true) != 0)
                            writer.Write(column.TranslatedName);
                        writer.RenderEndTag();
                    }
                    
                }
                col++;
            }
            writer.RenderEndTag();

            //add second header if IsDualHeaderGrid = true
            if (IsDualHeaderGrid)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                foreach (ColumnMap column in Columns)
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Th);
                    if (!string.IsNullOrEmpty(column.SecondHeaderTranslatedName))
                        writer.Write(column.SecondHeaderTranslatedName);
                    writer.RenderEndTag();
                }
                writer.RenderEndTag();
            }

            double[] Formulary_Lives_Count;
            Formulary_Lives_Count = new double[100];
            //add rows in the grid
            foreach ( object obj in _data )
            {
                if ( properties == null )
                    properties = TypeDescriptor.GetProperties(obj);
              
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);

                int x = 0;
                foreach ( ColumnMap column in Columns )
                {                    
                    property = properties[column.PropertyName];

                    if ( !property.PropertyType.IsClass )
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "alignRight");

                    value = property.GetValue(obj);

                    if (Lives_Y_Col[x] > 0)
                    {
                        if (value != System.DBNull.Value)
                        {
                            Formulary_Lives_Count[x] = Formulary_Lives_Count[x] + Convert.ToInt32(value);
                        }
                    }

                    if (string.Compare(column.TranslatedName, "key", true) == 0)//reports legend color matching
                    {
                        writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, ReportColors.CustomerContactReports.GetColorAsHexString(Convert.ToInt32(value) - 1));
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                        writer.Write("&nbsp;");
                        writer.RenderEndTag();                        
                    }
                    else if (column.TranslatedName.IndexOf("|HIGHLIGHT|") > -1)
                    {
                        //Get value of preceding corresponding timeframe/column
                        PropertyDescriptor previousProperty = properties[Columns[x - 1].PropertyName];
                        object previousValue = previousProperty.GetValue(obj);

                        //If previous value is not equal to current value, highlight cell
                        if (string.Compare(previousValue.ToString(), value.ToString(), true) != 0 )
                            writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "Yellow");                         

                        writer.RenderBeginTag(HtmlTextWriterTag.Td);                                                                         

                        if (!string.IsNullOrEmpty(column.DataFormat))
                            writer.Write(string.Format(column.DataFormat, value));
                        else
                            writer.Write(value);

                        writer.RenderEndTag();
                    }
                    else if (!string.IsNullOrEmpty(column.DBColToCompare) && column.HeaderRepeaterCell == "R") //color code if pivoted columns are present
                    {
                        string strDBCompare = column.DBColToCompare; 
                        //get the value of dbcompare column

                        String FieldValue = properties[strDBCompare].GetValue(obj).ToString();
                        if (FieldValue == "1")
                        {
                            writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "Yellow");  
                        }
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);

                        if (!string.IsNullOrEmpty(column.DataFormat))
                            writer.Write(string.Format(column.DataFormat, value));
                        else
                            writer.Write(value);

                        writer.RenderEndTag();
                    }
                    else 
                    {
                       // sl 4/10/2012 hide TA: Lives/Formulary Tab - Covered Lives: Covered_Lives_Order column
                        if (column.PropertyName.ToLower() == "covered_lives_order")
                        {
                            //writer.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
                            //writer.RenderBeginTag(HtmlTextWriterTag.Td);
                            //writer.RenderEndTag();
                        }
                        else
                        {
                            writer.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid");
                            writer.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px");
                            writer.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, "#cccccc");
                            writer.RenderBeginTag(HtmlTextWriterTag.Td);
                            if (!string.IsNullOrEmpty(column.DataFormat))
                                writer.Write(string.Format(column.DataFormat, value));
                            else
                                writer.Write(value);
                            writer.RenderEndTag();
                        }
                    }

                    x++;
                }
                writer.RenderEndTag();
            }
            if (ADD_Lives_Total_Row == true)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);

                    
                        for (int xx = 0; xx < col; xx++)
                        {
                            writer.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid");
                            writer.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px");
                            writer.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, "#cccccc");
                            writer.AddAttribute(HtmlTextWriterAttribute.Class, "alignRight");
                            writer.RenderBeginTag(HtmlTextWriterTag.Td);
                            if (xx == 0) { writer.Write("Total"); }
                            if (Formulary_Lives_Count[xx] > 0)
                                writer.Write(Formulary_Lives_Count[xx].ToString("#,##0"));
                            else
                                writer.Write("");
                        }

                    writer.RenderEndTag();

                writer.RenderEndTag();
            }
        }
    }
}
