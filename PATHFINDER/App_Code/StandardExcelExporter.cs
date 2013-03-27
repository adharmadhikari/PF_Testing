using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Web.UI;
using Microsoft.Office.Interop.Excel;
using PathfinderModel;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.Configuration;
using log4net;
using Pinsonault.Data.Reports;
using Pinsonault.Application.PowerPlanRx;
using System.Data;
using System.Collections.Specialized;

namespace Pathfinder
{
    /// <summary>
    /// Standard reports Excel exporter.
    /// Transforms input to a shared document template using specialized formatting logic.
    /// This class is thread safe.
    /// </summary>
    public class StandardExcelExporter : ExcelExporter
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(StandardExcelExporter));

        /// <summary>
        /// Excel template file to use. This can be in XLS, XLSX, or XLT format.
        /// IMPORTANT: Note that when this file changes, CreateExcelFile() function needs to match what the file contains.
        /// </summary>
        private readonly String _templateFile;

        /// <summary>
        /// Row offset where column header names appear
        /// </summary>
        private readonly int _columnHeaderRowOffset = 2;

        public StandardExcelExporter()
        {
            HttpContext ctx = HttpContext.Current;
            if (ctx == null)
            {
                log.Error("Unable to obtain current HttpContext.");
                throw new ArgumentException("Unable to obtain current HttpContext");
            }
            
            _templateFile = ctx.Request.MapPath(
                Path.Combine(
                    ctx.Request.ApplicationPath,
                    "ExcelTemplates/StandardExcelExporter_Template.xlt")
                .Replace(@"\", "/"));
            log.Debug(String.Format("Using Excel template file \"{0}\".", _templateFile));
        }

        private void UpdateHeaders(Excel.Worksheet worksheet, ReportSubsection subsection)
        {
            log.Debug("Updating headers...");

            string strExcelCriteria = GetCriteriaDetails(subsection, Title, ReportType.Excel, ReportDate);
            log.Debug(String.Concat("Report criteria:\n", strExcelCriteria));
            ((TextBox)worksheet.TextBoxes(1)).Text = strExcelCriteria;
            ((TextBox)worksheet.TextBoxes(1)).Width = strExcelCriteria.Length;
        }

        private static string UpdateFooter(Excel.Worksheet worksheet, ReportSubsection subsection)
        {
            //log.Debug("Updating Footer...");
            string criteria = subsection.CriteriaItems["section_id"].Text;

            string footer = string.Empty;
            if (criteria.IndexOf("DoD") != -1)
             footer = Pinsonault.Web.Support.GetDataUpdateDateByKey("DoD Formulary", Resources.Resource.Label_Section_Last_Updated) ;

            if (criteria.IndexOf("Commercial") != -1 || criteria.IndexOf("PBM") != -1 || criteria.IndexOf("FEP") != -1 || criteria.IndexOf("Managed Medicaid") != -1)
                footer = Pinsonault.Web.Support.GetDataUpdateDateByKey("Commercial Formulary", Resources.Resource.Label_Section_Last_Updated);

             if (criteria.IndexOf("Part D") != -1)
                footer = Pinsonault.Web.Support.GetDataUpdateDateByKey("Part-D Formulary", Resources.Resource.Label_Section_Last_Updated) ;

             if (criteria.IndexOf("State Medicaid") != -1)
                footer = Pinsonault.Web.Support.GetDataUpdateDateByKey("State Medicaid Formulary", Resources.Resource.Label_Section_Last_Updated);

             if (!string.IsNullOrEmpty(footer))
             {
                 footer = footer + ". Data subject to change";
                 //((TextBox)worksheet.TextBoxes(1)).Text = ((TextBox)worksheet.TextBoxes(1)).Text + "\n" + footer;
             }
             return footer;
            //log.Debug(String.Concat("Report criteria:\n", strExcelCriteria));
        }

        private static String GetColName(int col)
        {
            String alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if (col > alpha.Length) {
                // GetColName(col) + alpha[floor(col / alpha.Length)]
                // 

                return String.Concat(
                        alpha.Substring((col / alpha.Length)-1, 1) +
                        GetColName(col - alpha.Length) );
            }

            return alpha.Substring(col % alpha.Length, 1);
        }

        private static string GetColNameExact(int intCol)
        {
            int dividend = intCol;
            if (dividend == 0)
                dividend = 1;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }
            return columnName;
        }

        /// <summary>
        /// Formats an alphanumeric cell name based on the given integer x/y coordinates.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private static String FormatCellName(int row, int col)
        {
            return String.Concat(GetColName(col), row);
        }
        /// <summary>
        /// Formats an alphanumeric cell name based on the given integer x/y coordinates.
        /// Use this function, if lot of cells are involved
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private static String FormatCellName2(int row, int col)
        {
            return String.Concat(GetColNameExact(col), row);
        }

        void InsertImage(Excel.Worksheet worksheet, ReportSubsection subsection)
        {
            log.Debug("Inserting image...");

            //string cellName = FormatCellName(1, 1);

            //Range range = worksheet.get_Range(cellName, cellName);
            //range.set_Item(1, 1, subsection.ImagePath);
            //Marshal.ReleaseComObject(range);
            
            worksheet.Shapes.AddPicture(
                subsection.ImagePath, 
                Microsoft.Office.Core.MsoTriState.msoFalse, 
                Microsoft.Office.Core.MsoTriState.msoTrue, 
                0F, 
                101F, 
                subsection.Width * 0.78F,   // Why is hardcoded scaling being done? -CL
                subsection.Height * 0.78F);
        }

        void UpdateList(Excel.Worksheet worksheet, ReportSubsection subsection, int rowOffset)
        {
            log.Debug("List compilation started.");
            DateTime startTime = DateTime.Now;
            String AppID = HttpContext.Current.Request.QueryString["application"].ToString();

                        // Populate rows
            int x = 0, rowBeginOffset = _columnHeaderRowOffset + rowOffset;
            //int totalCount = 0;
            //foreach (Object elem in subsection.Data)
            //    ++totalCount;

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
                        //    throw new InvalidOperationException(
                        //        String.Format("Property '{0}' (defined in ColumnMap) doesn't exist on mapped entity. Please ensure it is mapped property in the Excel_Export_Columns table.",
                        //            map.PropertyName));

                        //Object value = pi.GetValue(elem, null);
                        //string propValue = value != null ? value.ToString() : "";

                        String fieldName = results.Single();
                        int fieldOrdinal = elem.GetOrdinal(fieldName);
                        Object value = elem.GetValue(fieldOrdinal);
                        String fieldValue = value.ToString();

                        if ( !String.IsNullOrEmpty(map.DataFormat) )
                            fieldValue = String.Format(map.DataFormat, value);

                        //if (! string.IsNullOrEmpty(propValue))
                       
                        //For 'Custom Segments' App(appid = 20) include all the fields in excelworksheet irrespective of their value.
                        //For other applications, include the fields only if the corresponding value is not null or empty.
                        if ((!string.IsNullOrEmpty(fieldValue) || (AppID == "20")))
                        {
                            x++;
                            //if ( !String.IsNullOrEmpty(map.DataFormat) )
                            //    propValue = String.Format(map.DataFormat, propValue);

                            if (!String.IsNullOrEmpty(map.DataFormat))
                                fieldValue = String.Format(map.DataFormat, value);

                            string cellName = FormatCellName(rowBeginOffset + (x - 1), 0);
                            Excel.Range cell = worksheet.get_Range(cellName, cellName);
                            if (Convert.ToInt32(cell.ColumnWidth) < map.TranslatedName.Length)
                                cell.ColumnWidth = map.TranslatedName.Length;
                            cell.set_Item(1, 1, map.TranslatedName);
                            //cell.Borders.Color = ColorTranslator.ToWin32(Color.Black);
                            Marshal.ReleaseComObject(cell);

                            cellName = FormatCellName(rowBeginOffset + (x - 1), 1);
                            cell = worksheet.get_Range(cellName, cellName);

                            //formatting for zip code
                            if (map.PropertyName.ToLower() == "zip")
                            {                                
                                cell.NumberFormat = "00000";
                            }


                            // sl 4/10/2012 hide TA: Lives/Formulary Tab - Covered Lives: Covered_Lives_Order column
                            if (map.PropertyName.ToLower() == "covered_lives_order")
                            {
                               
                                cell.EntireColumn.Hidden = true;
                            }
                            /////////////






                            //if ( pi.PropertyType.Equals(typeof(string)) )
                            //    cell.NumberFormat = "@";
                            cell.WrapText = true;
                            //cell.set_Item(1, 1, propValue);
                            cell.set_Item(1, 1, fieldValue);
                            //cell.Borders.Color = ColorTranslator.ToWin32(Color.Black);
                            if ( map.Width != null )
                                cell.ColumnWidth = map.Width;
                            Marshal.ReleaseComObject(cell);
                        }
                    }

                    
                    //}
#if DEBUG
                    else
                    {
                        Debug.WriteLine(String.Format("Property '{0}' (defined in ColumnMap) doesn't exist on mapped entity. Please ensure it is mapped property in the Excel_Export_Columns table.",
                                                    map.PropertyName));
                    }                    
#endif
                    
                }
                x++; //add blank between records
            }

            TimeSpan interval = startTime - DateTime.Now;
            log.Debug(String.Format("List compilation completed in {0}.", interval));
        }

        private void UpdateTable(Excel.Worksheet worksheet, ReportSubsection subsection, int rowOffset)
        {
            log.Debug("Table compilation started.");
            DateTime startTime = DateTime.Now;

         
            // Populate and style column headers
            int y = 0;

            int col = 0;

            bool ADD_Lives_Total_Row = false;
            int[] Lives_Y_Col;
            Lives_Y_Col = new int[100];
            Excel.Range range = worksheet.get_Range(
                FormatCellName(_columnHeaderRowOffset + rowOffset, 0),
                FormatCellName(_columnHeaderRowOffset + rowOffset, subsection.ColumnMap.Count - 1));
            range.WrapText = true;
            Object[,] cells = new Object[1, subsection.ColumnMap.Count];
            foreach (ColumnMap map in subsection.ColumnMap)
            {
                if (map.TranslatedName.Contains("HasSum"))
                {
                    Lives_Y_Col[col] = y;
                    ADD_Lives_Total_Row = true;                    
                }

                if (map.TranslatedName.IndexOf("|HIGHLIGHT|") > -1) //If column is FHR Highlight, remove |HIGHLIGHT| tag
                    cells[0, y] = map.TranslatedName.Replace("|HIGHLIGHT|", "");
                else if (map.TranslatedName.IndexOf("HasSum") > -1) //If column is HasSum i.e Get Total of that Column in excel remove |HasSum tag
                    cells[0, y] = map.TranslatedName.Replace("HasSum", "");

                else if (map.TranslatedName == "Key") //If column is color Key, no title is needed
                    cells[0, y] = "";
                else
                    cells[0, y] = map.TranslatedName;

                if (map.Width != null)
                {
                    // Width adjustment needs to be made to column.
                    string cellName = FormatCellName(_columnHeaderRowOffset + rowOffset, y);
                    Excel.Range cell = worksheet.get_Range(cellName, cellName);
                    cell.ColumnWidth = map.Width.Value;
                    Marshal.ReleaseComObject(cell);
                }
                col++;
                y++;
            }
            range.set_Value(Missing.Value, cells);
            range.Interior.Color = ColorTranslator.ToWin32(Color.FromArgb(188, 188, 188));
            range.Borders.Color = ColorTranslator.ToWin32(Color.FromArgb(51, 51, 153));

            //Added by AM for enabling Auto filter.
            range.AutoFilter(1, Type.Missing, XlAutoFilterOperator.xlAnd, Type.Missing, true);

            Marshal.ReleaseComObject(range);

            // Populate and style rows
            int x = 0, rowBeginOffset = _columnHeaderRowOffset + rowOffset + 1;
            int totalCount = 0;
            foreach (Object elem in subsection.Data)
                ++totalCount;
            if (ADD_Lives_Total_Row == true)
                totalCount =  totalCount +1;
            range = worksheet.get_Range(
                FormatCellName(rowBeginOffset, 0),
                FormatCellName(rowBeginOffset + totalCount - 1, subsection.ColumnMap.Count - 1));
            cells = new Object[totalCount, subsection.ColumnMap.Count];
            //foreach (Object elem in subsection.Data)
            double[] Formulary_Lives_Count;
            Formulary_Lives_Count = new double[100];
            foreach (System.Data.Common.DbDataRecord elem in subsection.Data)
            {
                ++x;
                y = 0;
                
                //GD 4/26/10
                //PropertyInfo[] properties = elem.GetType().GetProperties();
                List<string> properties = new List<string>();

                for (int s = 0; s < elem.FieldCount; s++)
                    properties.Add(elem.GetName(s));

                foreach (ColumnMap map in subsection.ColumnMap)
                {
                    //IEnumerable<PropertyInfo> results = properties.Where(p => p.Name.Equals(map.PropertyName));

                    IEnumerable<string> results = properties.Where(p => p.Equals(map.PropertyName));
                    if (results.Count() == 0)
                        throw new InvalidOperationException(
                            String.Format("Property '{0}' (defined in ColumnMap) doesn't exist on mapped entity. Please ensure it is mapped property in the Excel_Export_Columns table.",
                                map.PropertyName));

                    //PropertyInfo pi = results.Single();
                    //Object value = pi.GetValue(elem, null);
                    //String propValue = value != null ? value.ToString() : "";                    
                    String fieldName = results.Single();
                    int fieldOrdinal = elem.GetOrdinal(fieldName);
                    Object value = elem.GetValue(fieldOrdinal);
                    String fieldValue = value.ToString();

                    if (Lives_Y_Col[y] > 0)
                    {
                        if( value != System.DBNull.Value)
                        {
                            Formulary_Lives_Count[y] = Formulary_Lives_Count[y] + Convert.ToInt32(value);
                        }
                    }
                        
                    

                    if (!String.IsNullOrEmpty(map.DataFormat))
                        fieldValue = String.Format(map.DataFormat, value);

                    //Check if value is date for proper formatting
                    if (value is DateTime)
                    {
                        Range rangeCell = worksheet.get_Range(
                                            FormatCellName((rowBeginOffset + (x - 1)), y - 1),
                                            FormatCellName((rowBeginOffset + (x - 1)), y - 1));

                        rangeCell.NumberFormat = "mm/dd/yyyy";
                    }
                    //for providing the zip code formatting
                    if (map.PropertyName.ToLower() == "zip")
                    {
                        Range rangeCell = worksheet.get_Range(
                                            FormatCellName((rowBeginOffset + (x - 1)), y - 1),
                                            FormatCellName((rowBeginOffset + (x - 1)), y - 1));

                        rangeCell.NumberFormat = "00000";
                    }



                    // sl 4/10/2012 hide TA: Lives/Formulary Tab - Covered Lives: Covered_Lives_Order column
                    if (map.PropertyName.ToLower() == "covered_lives_order")
                    {
                        Range rangeCell = worksheet.get_Range(
                                           FormatCellName((rowBeginOffset + (x - 1)), y),
                                           FormatCellName((rowBeginOffset + (x - 1)), y));
                        rangeCell.EntireColumn.Hidden = true;
                    }
                    /////////////


                    if (map.PropertyName == "Formulary_Name")
                    {
                        Range rangeCell = worksheet.get_Range(
                                            FormatCellName((rowBeginOffset + (x - 1)), y),
                                            FormatCellName((rowBeginOffset + (x - 1)), y));
                        rangeCell.NumberFormat = "00000000";
                    }

                    if (map.TranslatedName == "Key") //Legend Color
                    {
                        //If column is color Key, set cell value to empty string and cell color to Key value
                        int num;
                        bool isNum = int.TryParse(fieldValue, out num);

                        if (isNum)
                        {
                            cells[x - 1, y++] = "";
                            Range rangeCell = worksheet.get_Range(
                            FormatCellName((rowBeginOffset + (x - 1)), y - 1),
                                FormatCellName((rowBeginOffset + (x - 1)), y - 1));
                            rangeCell.Interior.Color = ColorTranslator.ToWin32(ReportColors.CustomerContactReports.GetColor(num - 1));
                        }
                    }
                    else if (map.TranslatedName.IndexOf("|HIGHLIGHT|") > -1) //Formulary history comparison color coding
                    {
                        //Remove |HIGHLIGHT| Reference from TranslatedName
                        map.TranslatedName.Replace("|HIGHLIGHT|", "");

                        //Get 'previous' timeframe property name, |HIGHLIGHT| fields are defined as the 
                        //current timeframe in Excel_Report_Columns
                        string previousTimeframe = map.PropertyName.Replace('1', '0');

                        IEnumerable<string> correspondingTimeframe = properties.Where(p => p.Equals(previousTimeframe));
                        String previousFieldName = correspondingTimeframe.Single();
                        int previousFieldOrdinal = elem.GetOrdinal(previousFieldName);
                        Object previousValue = elem.GetValue(previousFieldOrdinal);
                        String previousFieldValue = previousValue.ToString();

                        if (previousFieldValue != fieldValue)
                        {
                            Range rangeCell = worksheet.get_Range(
                            FormatCellName((rowBeginOffset + (x - 1)), y),
                            FormatCellName((rowBeginOffset + (x - 1)), y));
                            rangeCell.Interior.Color = ColorTranslator.ToWin32(Color.FromArgb(255, 255, 0));
                        }

                        cells[x - 1, y++] = fieldValue;
                    }
                    else
                        cells[x - 1, y++] = fieldValue;




                }
            }

            if (ADD_Lives_Total_Row == true)
            {
                cells[x, 0] = "Total";
                for (int xx = 0; xx < col; xx++)
                {
                    if (Formulary_Lives_Count[xx] > 0)
                        cells[x, xx] = Formulary_Lives_Count[xx].ToString("#,##0");
                }
            }

            range.set_Value(Missing.Value, cells);
            range.WrapText = true;
            range.Borders.Color = ColorTranslator.ToWin32(Color.Black);
            Marshal.ReleaseComObject(range);

            //if (subsection.Name == "Benefit Design - Drug Level")
            //{
            //    string footer = UpdateFooter(worksheet, subsection);

            //    if (!string.IsNullOrEmpty(footer))
            //    {
            //        //cells = new Object[1, 1];
            //        string cellName1 = FormatCellName(x + 5, 0);
            //        string cellName2 = FormatCellName(x + 5, 5);
            //        Excel.Range cell = worksheet.get_Range(cellName1, cellName2);
            //        cell.Value2 = footer;
            //        cell.Font.Name = "Calibri";
            //        cell.Font.Size = 10;
            //        cell.WrapText = true;
            //        cell.MergeCells = true;
            //        Marshal.ReleaseComObject(cell);
            //    }
            //}
        

            TimeSpan interval = startTime - DateTime.Now;
            log.Debug(String.Format("Table compilation completed in {0}.", interval));
        }

        /// <summary>
        /// for adding dual header table, used for FHR. For using this function, add following properties in column map to populate dual header table:
        /// FirstHeaderTranslatedName,MergedCellSpan,HeaderRepeaterCell,SecondHeaderTranslatedName,DBColToCompare
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="subsection"></param>
        /// <param name="rowOffset"></param>
        private void UpdateDualHeaderTable(Excel.Worksheet worksheet, ReportSubsection subsection, int rowOffset)
        {
            log.Debug("Table compilation started.");
            DateTime startTime = DateTime.Now;
                        
              
            // Populate and style column headers
            int y = 0;
            Excel.Range range = worksheet.get_Range(
                FormatCellName2(_columnHeaderRowOffset + rowOffset, 1),
                FormatCellName2(_columnHeaderRowOffset + rowOffset, subsection.ColumnMap.Count));
            range.WrapText = true;
            Object[,] cells = new Object[1, subsection.ColumnMap.Count];
            int intx = 0;

            //*********first header row - starts*********
            cells = new Object[1, subsection.ColumnMap.Count];
            foreach (ColumnMap map in subsection.ColumnMap)
            {
                string strHeader = map.TranslatedName;
                string strRepeat = "";
                if (y < subsection.ColumnMap.Count)
                {
                    //if (map.TranslatedName.IndexOf("|HIGHLIGHT|") > -1) //If column is FHR Highlight, remove |HIGHLIGHT| tag
                    //    cells[intx, y] = map.TranslatedName.Replace("|HIGHLIGHT|", "");
                    //else 
                    if (map.TranslatedName == "Key") //If column is color Key, no title is needed
                        cells[intx, y] = "";
                    else if (map.PropertyName.IndexOfAny(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }) == -1) //if Header is static, remove first line
                    {
                        cells[intx, y] = "";                            
                    }
                    else
                    {
                        //string[] strTranlatedName = map.TranslatedName.Split('|');
                        string strFirstHeaderTranslatedName = map.FirstHeaderTranslatedName; //strTranlatedName[0];
                        int iCellspan = map.MergedCellSpan - 1;//Convert.ToInt32(strTranlatedName[1])-1;
                        strRepeat = map.HeaderRepeaterCell;//strTranlatedName[3];
                        if (strRepeat != "R")
                        {
                            cells[intx, y] = strFirstHeaderTranslatedName;//map.TranslatedName;
                            //int iCellspan = 2;
                            // Width adjustment needs to be made to column.                    
                            string cellName = FormatCellName2(_columnHeaderRowOffset + rowOffset, y+1);
                            string cellName_Extended = FormatCellName2(_columnHeaderRowOffset, y + iCellspan+1);
                            Excel.Range cell = worksheet.get_Range(cellName, cellName_Extended);
                            if (iCellspan > 0)                                
                                cell.MergeCells = true;
                            cell.HorizontalAlignment = 7;//text is aligned in center of the cell
                            Marshal.ReleaseComObject(cell);
                            y = y + iCellspan;
                        }
                    }

                    if (map.Width != null)
                    {
                        // Width adjustment needs to be made to column.
                        string cellName = FormatCellName2(_columnHeaderRowOffset + rowOffset, y+1);
                        Excel.Range cell = worksheet.get_Range(cellName, cellName);
                        cell.ColumnWidth = map.Width.Value;
                        Marshal.ReleaseComObject(cell);
                    }
                    if (strRepeat != "R")
                        y++;
                }
            }
            range.set_Value(Missing.Value, cells);
            range.Interior.Color = ColorTranslator.ToWin32(Color.FromArgb(188, 188, 188));
            range.Borders.Color = ColorTranslator.ToWin32(Color.FromArgb(51, 51, 153));

            //Added by AM for enabling Auto filter.
            //range.AutoFilter(1, Type.Missing, XlAutoFilterOperator.xlAnd, Type.Missing, true);

            Marshal.ReleaseComObject(range);
            intx = 0;
            y = 0;
            //cells = new Object[1, subsection.ColumnMap.Count];
            rowOffset = 1;
            cells = new Object[1, subsection.ColumnMap.Count];
            range = worksheet.get_Range(
            FormatCellName2(_columnHeaderRowOffset + rowOffset, 1),
            FormatCellName2(_columnHeaderRowOffset + rowOffset, subsection.ColumnMap.Count));
            //*********first header row - ends*********

            //*********second header row starts *********
            foreach (ColumnMap map in subsection.ColumnMap)
            {
                if (map.TranslatedName.IndexOf("|HIGHLIGHT|") > -1) //If column is FHR Highlight, remove |HIGHLIGHT| tag
                    cells[intx, y] = map.SecondHeaderTranslatedName;//map.TranslatedName.Replace("|HIGHLIGHT|", "");
                else if (map.TranslatedName == "Key") //If column is color Key, no title is needed
                    cells[intx, y] = "";
                else if (map.PropertyName.IndexOfAny(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }) > -1)
                    cells[intx, y] = map.SecondHeaderTranslatedName;//map.TranslatedName.Split('|')[2];
                else
                    cells[intx, y] = map.TranslatedName;

                if (map.Width != null)
                {
                    // Width adjustment needs to be made to column.
                    string cellName = FormatCellName2(_columnHeaderRowOffset + rowOffset, y+1);
                    Excel.Range cell = worksheet.get_Range(cellName, cellName);
                    cell.ColumnWidth = map.Width.Value;
                    Marshal.ReleaseComObject(cell);
                }
                y++;
            }
            intx = 1;
            range.set_Value(Missing.Value, cells);
            range.Interior.Color = ColorTranslator.ToWin32(Color.FromArgb(188, 188, 188));
            range.Borders.Color = ColorTranslator.ToWin32(Color.FromArgb(51, 51, 153));

            //Added by AM for enabling Auto filter.
            range.AutoFilter(1, Type.Missing, XlAutoFilterOperator.xlAnd, Type.Missing, true);

            Marshal.ReleaseComObject(range);
            //********* second header row ends *********

            // Populate and style rows
            int x = 0, rowBeginOffset = _columnHeaderRowOffset + rowOffset + 1;
            int totalCount = 0;
            foreach (Object elem in subsection.Data)
                ++totalCount;
            range = worksheet.get_Range(
                FormatCellName2(rowBeginOffset, 1),
                FormatCellName2(rowBeginOffset + totalCount - 1, subsection.ColumnMap.Count));
            cells = new Object[totalCount + 1, subsection.ColumnMap.Count];
            //foreach (Object elem in subsection.Data)
            foreach (System.Data.Common.DbDataRecord elem in subsection.Data)
            {
                ++x;
                y = 0;
                //GD 4/26/10
                //PropertyInfo[] properties = elem.GetType().GetProperties();
                List<string> properties = new List<string>();

                for (int s = 0; s < elem.FieldCount; s++)
                    properties.Add(elem.GetName(s));

                foreach (ColumnMap map in subsection.ColumnMap)
                {
                    //IEnumerable<PropertyInfo> results = properties.Where(p => p.Name.Equals(map.PropertyName));

                    IEnumerable<string> results = properties.Where(p => p.Equals(map.PropertyName));
                    if (results.Count() == 0)
                        throw new InvalidOperationException(
                            String.Format("Property '{0}' (defined in ColumnMap) doesn't exist on mapped entity. Please ensure it is mapped property in the Excel_Export_Columns table.",
                                map.PropertyName));

                    //PropertyInfo pi = results.Single();
                    //Object value = pi.GetValue(elem, null);
                    //String propValue = value != null ? value.ToString() : "";                    
                    String fieldName = results.Single();
                    int fieldOrdinal = elem.GetOrdinal(fieldName);
                    Object value = elem.GetValue(fieldOrdinal);
                    String fieldValue = value.ToString();

                    if (!String.IsNullOrEmpty(map.DataFormat))
                        fieldValue = String.Format(map.DataFormat, value);

                    //Check if value is date for proper formatting
                    if (value is DateTime)
                    {
                        Range rangeCell = worksheet.get_Range(
                                            FormatCellName2((rowBeginOffset + (x - 1)), y + 1),
                                            FormatCellName2((rowBeginOffset + (x - 1)), y + 1));

                        rangeCell.NumberFormat = "mm/dd/yyyy";
                    }
                    //for providing the zip code formatting
                    if (map.PropertyName.ToLower() == "zip")
                    {
                        Range rangeCell = worksheet.get_Range(
                                            FormatCellName2((rowBeginOffset + (x - 1)), y + 1),
                                            FormatCellName2((rowBeginOffset + (x - 1)), y + 1));

                        rangeCell.NumberFormat = "00000";
                    }

                    if (map.PropertyName == "Formulary_Name")
                    {
                        Range rangeCell = worksheet.get_Range(
                                            FormatCellName2((rowBeginOffset + (x - 1)), y + 1),
                                            FormatCellName2((rowBeginOffset + (x - 1)), y + 1));
                        rangeCell.NumberFormat = "00000000";
                    }

                    if (map.TranslatedName == "Key") //Legend Color
                    {
                        //If column is color Key, set cell value to empty string and cell color to Key value
                        int num;
                        bool isNum = int.TryParse(fieldValue, out num);

                        if (isNum)
                        {
                            cells[x - 1, y++] = "";
                            Range rangeCell = worksheet.get_Range(
                            FormatCellName2((rowBeginOffset + (x - 1)), y + 1),
                                FormatCellName2((rowBeginOffset + (x - 1)), y + 1));
                            rangeCell.Interior.Color = ColorTranslator.ToWin32(ReportColors.CustomerContactReports.GetColor(num - 1));
                        }
                    }
                    else if (map.TranslatedName.IndexOf("|HIGHLIGHT|") > -1) //Formulary history comparison color coding
                    {
                        //Remove |HIGHLIGHT| Reference from TranslatedName
                        map.TranslatedName.Replace("|HIGHLIGHT|", "");

                        //Get 'previous' timeframe property name, |HIGHLIGHT| fields are defined as the 
                        //current timeframe in Excel_Report_Columns
                        string previousTimeframe = map.PropertyName.Replace('1', '0');

                        IEnumerable<string> correspondingTimeframe = properties.Where(p => p.Equals(previousTimeframe));
                        String previousFieldName = correspondingTimeframe.Single();
                        int previousFieldOrdinal = elem.GetOrdinal(previousFieldName);
                        Object previousValue = elem.GetValue(previousFieldOrdinal);
                        String previousFieldValue = previousValue.ToString();

                        if (previousFieldValue != fieldValue)
                        {
                            Range rangeCell = worksheet.get_Range(
                            FormatCellName2((rowBeginOffset + (x - 1)), y + 1),
                            FormatCellName2((rowBeginOffset + (x - 1)), y + 1));
                            rangeCell.Interior.Color = ColorTranslator.ToWin32(Color.FromArgb(255, 255, 0));
                        }

                        cells[x - 1, y++] = fieldValue;
                    }
                    else if ((!string.IsNullOrEmpty(map.DBColToCompare) && map.HeaderRepeaterCell == "R") || (subsection.ReportDefinition.ReportKey == "coFormularyHxRolling" && !string.IsNullOrEmpty(map.DBColToCompare))) //color code if pivoted columns are present
                    {
                        string strDBCompare = map.DBColToCompare; //map.TranslatedName.Split('|')[4]; //variable used to compare the db changed value column and color coding
                        IEnumerable<string> correspondingDBCompare = properties.Where(p => p.Equals(strDBCompare));
                        String FieldName = correspondingDBCompare.Single();
                        int FieldOrdinal = elem.GetOrdinal(FieldName);
                        Object objValue = elem.GetValue(FieldOrdinal);
                        String FieldValue = objValue.ToString();
                        if (FieldValue == "1")
                        {
                            Range rangeCell = worksheet.get_Range(
                            FormatCellName2((rowBeginOffset + (x - 1)), y + 1),
                            FormatCellName2((rowBeginOffset + (x - 1)), y + 1));
                            rangeCell.Interior.Color = ColorTranslator.ToWin32(Color.FromArgb(255, 255, 0));
                        }
                        cells[x - 1, y++] = fieldValue;
                    }
                    else
                        cells[x - 1, y++] = fieldValue;
                }
            }

            range.set_Value(Missing.Value, cells);
            range.WrapText = true;
            range.Borders.Color = ColorTranslator.ToWin32(Color.Black);
            Marshal.ReleaseComObject(range);

            //if (subsection.Name == "Benefit Design - Drug Level")
            //{
            //    string footer = UpdateFooter(worksheet, subsection);

            //    if (!string.IsNullOrEmpty(footer))
            //    {
            //        //cells = new Object[1, 1];
            //        string cellName1 = FormatCellName(x + 5, 0);
            //        string cellName2 = FormatCellName(x + 5, 5);
            //        Excel.Range cell = worksheet.get_Range(cellName1, cellName2);
            //        cell.Value2 = footer;
            //        cell.Font.Name = "Calibri";
            //        cell.Font.Size = 10;
            //        cell.WrapText = true;
            //        cell.MergeCells = true;
            //        Marshal.ReleaseComObject(cell);
            //    }
            //}

            TimeSpan interval = startTime - DateTime.Now;
            log.Debug(String.Format("Table compilation completed in {0}.", interval));
            
          
        }

        private void UpdateProtection(Excel.Worksheet worksheet)
        {
            if (ProtectOutputFile)
            {
                // Generate a random password.
                String password = Guid.NewGuid().ToString();
                log.Debug(String.Format("Protecting report with password \"{0}\".", password));

                // Restrict operations, and require a password to perform them.
                worksheet.Protect(password, Missing.Value, Missing.Value, Missing.Value,
                    false,      // UI only?
                    true,       // Allow formatting cells?
                    true,       // Allow formatting columns?
                    true,       // Allow formatting rows?
                    false,      // Allow inserting columns?
                    false,      // Allow inserting rows?
                    false,      // Allow inserting hyperlinks?
                    false,      // Allow deleting columns?
                    false,      // Allow deleting rows?
                    true,       // Allow sorting?
                    true,       // Allow filtering?
                    false);     // Allow using pivot tables?
            }
        }
        
        public override void ExportToWeb(HtmlTextWriter writer)
        {
            throw new NotImplementedException();
        }

        string cleanSectionName(string Name)
        {
            //not allowed by excel for worksheet name \  /  ?  *  [  or  ]
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"[\\/\?\*\[\]]");

            //make sure only 31 chars
            return new string(regex.Replace(Name, "_").ToCharArray().Take(31).ToArray());

        }

        public override string ExportToFile()
        {
            try
            {
                // Acquire app from pool.
                Excel.Application app = ExcelApp;

                // Ensure thread safety.
                lock (app)
                {
                    string tempPath = OutputFolder;
                    if(string.IsNullOrEmpty(tempPath))
                        tempPath = Pinsonault.Web.Support.TempFolder; // ConfigurationManager.AppSettings["TempFolder"];
                    if (string.IsNullOrEmpty(tempPath) || !Directory.Exists(tempPath))
                        throw new ArgumentException("TempFolder does not exist or has not been specified in AppSettings section of web.config.  Please specify a temporary folder for generating excel files that is not a system directory.");

                    // Generate a random report file name (.xls)
                    string result = Path.Combine(tempPath, String.Concat("XR-", Guid.NewGuid().ToString().ToUpper(), ".xls"));

                    // Open the template workbook and get its default worksheet
                    Excel.Workbook workbook = ExcelApp.Workbooks.Open(_templateFile,
                        Missing.Value, true, Missing.Value, Missing.Value, Missing.Value,
                        Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                        Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                    Excel.Worksheet worksheet = (Excel.Worksheet)workbook.ActiveSheet;
                    if (ReportSubsections.Count > 1)
                    {
                        for (int i = 0; i < ReportSubsections.Count - 1; i++)
                            workbook.Sheets.Add(Missing.Value, worksheet, 1, _templateFile);
                    }

                    int index = 1;
                    int offset;
                    foreach (ReportSubsection subsection in ReportSubsections)
                    {
                        offset = 0;
                        worksheet = (Excel.Worksheet)workbook.Sheets[index];

                        worksheet.Name = cleanSectionName(subsection.Name);

                        UpdateHeaders(worksheet, subsection);

                        //SPH 3/19/2010 - images can be on same tab as data now
                        if ( subsection.IsImage() )
                        {
                            InsertImage(worksheet, subsection);
                            offset = 25;
                        }

                        switch (subsection.ReportDefinition.Style)
                        {
                            case ReportStyle.Chart:
                                InsertImage(worksheet, subsection);
                                break;
                            case ReportStyle.Grid:
                                UpdateTable(worksheet, subsection, offset);
                                break;
                            case ReportStyle.List:
                                UpdateList(worksheet, subsection, offset);
                                break;
                            case ReportStyle.DualHeaderGrid:
                                UpdateDualHeaderTable(worksheet, subsection, offset);
                                break;
                        }

                        // Document write protection
                        UpdateProtection(worksheet);
                        
                        Marshal.ReleaseComObject(worksheet);

                       
                        index++;
                    }
                    ((Excel.Worksheet)workbook.Sheets[1]).Activate();

                    foreach (Window window in workbook.Windows)
                    {
                        try
                        {
                            //make sure window is visible in case excel opens it tiled (not maximized)
                            window.WindowState = XlWindowState.xlNormal;
                            window.Width = 700;
                            window.Height = 400;
                            window.Left = 0;
                            window.Top = 0;
                        }
                        catch (Exception) { }
                    }

                    // Save new document to disk
                    workbook.Author = Resources.Resource.Assembly_CompanyName;
                    workbook.SaveCopyAs(result);
                    workbook.Close(false, Missing.Value, Missing.Value);
                    
                    Marshal.ReleaseComObject(workbook);

                    return result;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                ReleaseExcelApp();
            }
        }

        #region powerplanrx export

        private void UpdateTableFormatted(Excel.Worksheet worksheet, System.Data.DataTable dtReportTable, IList<int> MonthListDesc, int iReportType, System.Data.DataTable dtReportSummary)
        {
            try
            {
                log.Debug("Table compilation started.");
                DateTime startTime = DateTime.Now;
                int icolumnHeaderRowOffset = _columnHeaderRowOffset;

                int iHeaderRow1 = 0; //Optional first row for header (used in report summary and details)
                int iHeaderRow2 = 0; //Optional second row for header (used in report summary and details)
                int iHeaderRowTableColumns = 1; //Header row having translated column name; applicable for all reports 

                switch (iReportType)
                {
                    case 1: //Report Summary
                        iHeaderRow1 = 1;
                        iHeaderRow2 = 0;
                        iHeaderRowTableColumns = 2;
                        break;
                    case 2: //Report Details
                        iHeaderRow1 = 1;
                        iHeaderRow2 = 2;
                        iHeaderRowTableColumns = 3;
                        break;

                }
                //get the translated name of required columns from Lkp_Excel_ImpactReportTemplate table
                System.Data.DataTable dtRequiredReportTable_Columns = Campaign.GetReportTemplate(iReportType, iHeaderRowTableColumns);

                Excel.Range range = worksheet.get_Range(
                      FormatCellName2(icolumnHeaderRowOffset, 0),
                      FormatCellName2(icolumnHeaderRowOffset, dtRequiredReportTable_Columns.Rows.Count));
                range.WrapText = true;

                Object[,] cells = new Object[1, dtRequiredReportTable_Columns.Rows.Count];

                int y = 0;
                //enter first header row
                #region first row having text populated from DB and if month populated from list - MonthListDesc
                if (iHeaderRow1 > 0)
                {

                    System.Data.DataTable dtReportTemplateRow1 = Campaign.GetReportTemplate(iReportType, iHeaderRow1);
                    int iMonthCount = 0;

                    for (int iRow = 0; iRow < dtReportTemplateRow1.Rows.Count; iRow++)
                    {
                        string strColumnName = dtReportTemplateRow1.Rows[iRow]["Column_DBName"].ToString();
                        int iCellspan = Convert.ToInt32(dtReportTemplateRow1.Rows[iRow]["CellSpan"]) - 1;
                        //make a list for Column_DB_Name and pass it to report table
                        string strTranslatedName = dtReportTemplateRow1.Rows[iRow]["Column_TranslatedName"].ToString();


                        if (dtReportTable.Columns[strColumnName] != null)
                        {
                            cells[0, y] = strTranslatedName;
                            // Width adjustment needs to be made to column.                    
                            string cellName = FormatCellName2(icolumnHeaderRowOffset, y + 1);

                            string cellName_Extended = FormatCellName2(icolumnHeaderRowOffset, y + iCellspan + 1);
                            Excel.Range cell = worksheet.get_Range(cellName, cellName_Extended);
                            if (iCellspan > 0)
                            {
                                cell.HorizontalAlignment = 7;//text is aligned in center of the cell
                                cell.MergeCells = true;
                            }
                            Marshal.ReleaseComObject(cell);
                            y = y + iCellspan;
                        }

                        else if (strColumnName.Contains("month") && y < dtRequiredReportTable_Columns.Rows.Count && iMonthCount < MonthListDesc.Count)
                        {
                            strTranslatedName = MonthListDesc[iMonthCount].ToString().Substring(4, 2) + "/" + MonthListDesc[iMonthCount].ToString().Substring(0, 4);
                            cells[0, y] = strTranslatedName;
                            iMonthCount++;

                            string cellName = FormatCellName2(icolumnHeaderRowOffset, y + 1);

                            string cellName_Extended = FormatCellName2(icolumnHeaderRowOffset, y + iCellspan + 1);
                            Excel.Range cell = worksheet.get_Range(cellName, cellName_Extended);
                            cell.NumberFormat = "mmm-yy";

                            if (iCellspan > 0)
                            {
                                cell.HorizontalAlignment = 7;//text is aligned in center of the cell
                                cell.MergeCells = true;
                            }

                            Marshal.ReleaseComObject(cell);
                            y = y + iCellspan;
                        }
                        y++;
                    }

                    range.set_Value(Missing.Value, cells);
                    range.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                    range.Borders.Color = ColorTranslator.ToWin32(Color.FromArgb(51, 51, 153));
                    range.Font.Bold = true;
                    Marshal.ReleaseComObject(range);

                    dtReportTemplateRow1.Dispose();

                    icolumnHeaderRowOffset++;
                }
                #endregion

                #region Row 2 Header Row --for report details
                //optional row for Details
                //int icolumnHeaderRowOffset = 3;          
                if (iHeaderRow2 > 0)
                {
                    y = 0;
                    range = worksheet.get_Range(
                    FormatCellName(icolumnHeaderRowOffset, 0),
                    FormatCellName(icolumnHeaderRowOffset, dtRequiredReportTable_Columns.Rows.Count));
                    range.WrapText = true;

                    cells = new Object[icolumnHeaderRowOffset, dtRequiredReportTable_Columns.Rows.Count];

                    System.Data.DataTable dtReportDetailsHeaderRow = Campaign.GetReportTemplate(iReportType, iHeaderRow2);

                    for (int iRow = 0; iRow < dtReportDetailsHeaderRow.Rows.Count; iRow++)
                    {
                        string strColumnName = dtReportDetailsHeaderRow.Rows[iRow]["Column_DBName"].ToString();
                        int iCellspan = Convert.ToInt32(dtReportDetailsHeaderRow.Rows[iRow]["CellSpan"]) - 1;
                        //make a list for Column_DB_Name and pass it to report table
                        string strTranslatedName = dtReportDetailsHeaderRow.Rows[iRow]["Column_TranslatedName"].ToString();

                        if (dtReportTable.Columns[strColumnName] != null)
                        {
                            cells[0, y] = strTranslatedName;

                            string cellName = FormatCellName(icolumnHeaderRowOffset, y + 1);
                            string cellName_Extended = FormatCellName(icolumnHeaderRowOffset, y + iCellspan + 1);

                            Excel.Range cell = worksheet.get_Range(cellName, cellName_Extended);
                            if (iCellspan > 0)
                            {
                                cell.HorizontalAlignment = 7; //text is aligned in center of the cell
                                cell.MergeCells = true;
                            }
                            Marshal.ReleaseComObject(cell);
                            y = y + iCellspan;
                        }
                        y++;
                    }
                    range.set_Value(Missing.Value, cells);
                    range.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                    range.Borders.Color = ColorTranslator.ToWin32(Color.FromArgb(51, 51, 153));
                    range.Font.Bold = true;
                    Marshal.ReleaseComObject(range);
                    dtReportDetailsHeaderRow.Dispose();
                    icolumnHeaderRowOffset++;
                }
                #endregion

                #region row for db column's translated name
                //insert required db column's translated name in the row

                range = worksheet.get_Range(
                FormatCellName2(icolumnHeaderRowOffset, 0),
                FormatCellName2(icolumnHeaderRowOffset, dtRequiredReportTable_Columns.Rows.Count));
                range.WrapText = true;

                cells = new Object[icolumnHeaderRowOffset, dtRequiredReportTable_Columns.Rows.Count];

                y = 0;
                for (int iRow = 0; iRow < dtRequiredReportTable_Columns.Rows.Count; iRow++)
                {
                    string strColumnName = dtRequiredReportTable_Columns.Rows[iRow]["Column_DBName"].ToString();

                    //make a list for Column_DB_Name and pass it to report table
                    string strTranslatedName = dtRequiredReportTable_Columns.Rows[iRow]["Column_TranslatedName"].ToString();

                    cells[0, y] = strTranslatedName;
                    if (!string.IsNullOrEmpty(dtRequiredReportTable_Columns.Rows[iRow]["Width"].ToString()))
                    {
                        string cellName = FormatCellName2(_columnHeaderRowOffset + 1, y + 1);
                        Excel.Range cell = worksheet.get_Range(cellName, cellName);
                        cell.ColumnWidth = dtRequiredReportTable_Columns.Rows[iRow]["Width"].ToString();
                        Marshal.ReleaseComObject(cell);
                    }
                    y++;
                }
                range.set_Value(Missing.Value, cells);
                range.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                range.Borders.Color = ColorTranslator.ToWin32(Color.FromArgb(51, 51, 153));
                range.Font.Bold = true;
                Marshal.ReleaseComObject(range);
                icolumnHeaderRowOffset++;
                #endregion
                #region populate rows from database
                // Populate and style rows
                int x = 0, rowBeginOffset = icolumnHeaderRowOffset;
                int totalCount = 0;
                range = worksheet.get_Range(
                FormatCellName2(rowBeginOffset, 0),
                FormatCellName2(rowBeginOffset, dtRequiredReportTable_Columns.Rows.Count));
                range.WrapText = true;
                if (iReportType == 2)
                {
                    int iRowCount = 0;//dtReportTable.Rows.Count + dtReportSummary.Rows.Count;
                    for (int i = 0; i < dtReportSummary.Rows.Count; i++)
                    {
                        iRowCount = iRowCount + 1;
                        string strCampaignID = dtReportSummary.Rows[i]["Campaign_ID"].ToString();
                        string strFilterString = string.Format(" Campaign_ID = {0} ", strCampaignID);
                        System.Data.DataTable dtFilterTable = FilterTable(dtReportTable, strFilterString);
                        iRowCount = iRowCount + dtFilterTable.Rows.Count;
                    }

                    for (int iRow = 0; iRow < iRowCount; iRow++)
                    {
                        ++totalCount;

                        range = worksheet.get_Range(
                            FormatCellName2(rowBeginOffset, 0),
                            FormatCellName2(rowBeginOffset + totalCount, dtRequiredReportTable_Columns.Rows.Count));
                        cells = new Object[rowBeginOffset + totalCount, dtRequiredReportTable_Columns.Rows.Count];
                    }
                }
                else
                {
                    for (int iRow = 0; iRow < dtReportTable.Rows.Count; iRow++)
                    {
                        ++totalCount;

                        range = worksheet.get_Range(
                            FormatCellName2(rowBeginOffset, 0),
                            FormatCellName2(rowBeginOffset + totalCount, dtRequiredReportTable_Columns.Rows.Count));
                        cells = new Object[rowBeginOffset + totalCount, dtRequiredReportTable_Columns.Rows.Count];
                    }
                }
                totalCount = 0;
                //Report for Impact Report Details , Report TypeID = 2
                if (iReportType == 2 && !(dtReportSummary == null))
                {
                    //insert report summary
                    for (int iRowSummary = 0; iRowSummary < dtReportSummary.Rows.Count; iRowSummary++)
                    {
                        ++x;
                        y = 0;
                        string strCampaignID = dtReportSummary.Rows[iRowSummary]["Campaign_ID"].ToString();

                        //check all the required columns in report table and insert the row
                        for (int iCol = 0; iCol < dtRequiredReportTable_Columns.Rows.Count; iCol++)
                        {
                            string strReportColName = dtRequiredReportTable_Columns.Rows[iCol]["Column_DBName"].ToString();
                            string strFormat = dtRequiredReportTable_Columns.Rows[iCol]["DataFormat"].ToString();

                            //make the report summary record's cell font - bold
                            Excel.Range rangeSummary = worksheet.get_Range(
                                   FormatCellName2(rowBeginOffset + totalCount, 0),
                                   FormatCellName2(rowBeginOffset + totalCount, dtRequiredReportTable_Columns.Rows.Count));
                            rangeSummary.Font.Bold = true;
                            Marshal.ReleaseComObject(rangeSummary);

                            //insert the summary table records
                            if (dtReportSummary.Columns[strReportColName] != null)
                            {
                                try
                                {
                                    if (!string.IsNullOrEmpty(strFormat))
                                        cells[x - 1, iCol] = String.Format(strFormat, dtReportSummary.Rows[iRowSummary][strReportColName]);
                                    else
                                        cells[x - 1, iCol] = dtReportSummary.Rows[iRowSummary][strReportColName];
                                }
                                catch (FormatException)
                                {
                                    cells[x - 1, iCol] = dtReportSummary.Rows[iRowSummary][strReportColName];
                                }
                                catch
                                { }
                            }
                            //if Required DB Column Name = Actual_District_Name, insert the Plan Name for showing the Plan level details 
                            //, all districts name will be added in the next row through districtdetails table (dtReportTable)
                            else if (strReportColName == "Actual_District_Name")
                                cells[x - 1, iCol] = dtReportSummary.Rows[iRowSummary]["Plan_Name"];
                            else
                                cells[x - 1, iCol] = "";

                        }

                        string strFilterString = string.Format(" Campaign_ID = {0} ", strCampaignID);
                        System.Data.DataTable dtFilterTable = FilterTable(dtReportTable, strFilterString);
                        //now insert the report district details
                        for (int iRow = 0; iRow < dtFilterTable.Rows.Count; iRow++)
                        {
                            ++x;
                            y = 0;

                            bool bColorGoalPercent = true;
                            for (int iColDetails = 0; iColDetails < dtRequiredReportTable_Columns.Rows.Count; iColDetails++)
                            {
                                string strReportDetailsColName = dtRequiredReportTable_Columns.Rows[iColDetails]["Column_DBName"].ToString();
                                string strFormat = dtRequiredReportTable_Columns.Rows[iColDetails]["DataFormat"].ToString();

                                string cellName = FormatCellName2(rowBeginOffset + totalCount + 1, iColDetails + 1);
                                Excel.Range cell = worksheet.get_Range(cellName, cellName);

                                if (dtFilterTable.Columns[strReportDetailsColName] != null && strReportDetailsColName != "Campaign_ID")
                                {
                                    try
                                    {
                                        if (!string.IsNullOrEmpty(strFormat))
                                            cells[x - 1, iColDetails] = String.Format(strFormat, dtFilterTable.Rows[iRow][strReportDetailsColName]);
                                        else
                                            cells[x - 1, iColDetails] = dtFilterTable.Rows[iRow][strReportDetailsColName];
                                    }
                                    catch (FormatException)
                                    {
                                        cells[x - 1, iColDetails] = dtFilterTable.Rows[iRow][strReportDetailsColName];
                                    }
                                    catch
                                    { }

                                    //color the cells for Goal Percent for most recent record i.e. if the cell is not colored before
                                    if (bColorGoalPercent && (strReportDetailsColName == "Result_GoalPercent" || strReportDetailsColName.Contains("Curr_Trx_GoalPercent")))
                                    {
                                        if (!string.IsNullOrEmpty(dtFilterTable.Rows[iRow][strReportDetailsColName].ToString()))
                                        {
                                            bColorGoalPercent = false;
                                            if (Convert.ToInt32(dtFilterTable.Rows[iRow][strReportDetailsColName]) >= 100)
                                                cell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green);
                                            else if (Convert.ToInt32(dtFilterTable.Rows[iRow][strReportDetailsColName]) >= 90)
                                                cell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                            else
                                                cell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                        }
                                    }
                                    Marshal.ReleaseComObject(cell);
                                }
                                //keep the cell blank to prevent the same campaign id in each row
                                else if (dtFilterTable.Columns[strReportDetailsColName] != null && strReportDetailsColName == "Campaign_ID")
                                    cells[x - 1, iColDetails] = "";
                            }
                            totalCount++;
                        }
                        ++totalCount;
                    }

                    //end of report district details

                }

                //for other reports except report district details report
                else
                {
                    for (int iRow = 0; iRow < dtReportTable.Rows.Count; iRow++)
                    {
                        ++x;
                        y = 0;
                        //bool bColorGoalPercent = true;
                        for (int iCol = 0; iCol < dtRequiredReportTable_Columns.Rows.Count; iCol++)
                        {
                            string strReportColName = dtRequiredReportTable_Columns.Rows[iCol]["Column_DBName"].ToString();
                            string strFormat = dtRequiredReportTable_Columns.Rows[iCol]["DataFormat"].ToString();

                            if (dtReportTable.Columns[strReportColName] != null)
                            {
                                try
                                {
                                    if (!string.IsNullOrEmpty(strFormat))
                                        cells[x - 1, iCol] = String.Format(strFormat, dtReportTable.Rows[iRow][strReportColName]);
                                    else
                                        cells[x - 1, iCol] = dtReportTable.Rows[iRow][strReportColName];
                                }
                                catch (FormatException)
                                {
                                    cells[x - 1, iCol] = dtReportTable.Rows[iRow][strReportColName];
                                }
                                catch
                                { }
                            }
                        }
                        ++totalCount;
                    }
                }//end of else
                range.set_Value(Missing.Value, cells);
                range.WrapText = true;
                range.Borders.Color = ColorTranslator.ToWin32(Color.Black);
                Marshal.ReleaseComObject(range);
                dtRequiredReportTable_Columns.Dispose();
                dtReportTable.Dispose();
                #endregion

                TimeSpan interval = startTime - DateTime.Now;
                log.Debug(String.Format("Table compilation completed in {0}.", interval));
            }
            catch (Exception ex)
            {
                //throw ex;
            }
        }

        public System.Data.DataTable FilterTable(System.Data.DataTable dt, string filterString)
        {
            DataRow[] filteredRows = dt.Select(filterString);
            System.Data.DataTable filteredDt = dt.Clone();

            DataRow dr;
            foreach (DataRow oldDr in filteredRows)
            {
                dr = filteredDt.NewRow();
                for (int i = 0; i < dt.Columns.Count; i++)
                    if (dt.Columns[i].ColumnName != "Plan_Name" && dt.Columns[i].ColumnName != "AEName" && dt.Columns[i].ColumnName != "Brand_Name"
                        && dt.Columns[i].ColumnName != "Start_Date" && dt.Columns[i].ColumnName != "End_Date" && dt.Columns[i].ColumnName != "Campaign_ID")
                        dr[dt.Columns[i].ColumnName] = oldDr[dt.Columns[i].ColumnName];
                filteredDt.Rows.Add(dr);

            }

            return filteredDt;
        }
        private void UpdateHeaders(Excel.Worksheet worksheet, string ReportName, string strHeader)
        {
            log.Debug("Updating headers...");

            worksheet.Name = ReportName;
            string strExcelCriteria = strHeader;
            log.Debug(String.Concat("Report criteria:\n", strExcelCriteria));
            ((TextBox)worksheet.TextBoxes(1)).Text = strExcelCriteria;
        }

        private void InsertImage(Excel.Worksheet worksheet, string strImagePath, float Width, float Height)
        {
            log.Debug("Inserting image...");
            worksheet.Shapes.AddPicture(
                strImagePath,
                Microsoft.Office.Core.MsoTriState.msoFalse,
                Microsoft.Office.Core.MsoTriState.msoTrue,
                0F,
                160F,
                Width,
                Height);
        }
        /// <summary>
        /// for formatted output
        /// </summary>
        /// <param name="strHeader"></param>
        /// <param name="strReportName"></param>
        /// <param name="dsReportSet"></param>
        /// <param name="QueryString"></param>
        /// <returns></returns>
        public override string ExportToFileWithFormat(string strHeader, string strReportName, DataSet dsReportSet, NameValueCollection QueryString, IList<int> MonthListDesc, int iReportType, System.Data.DataTable dtReportSummary)
        {
            try
            {
                // Acquire app from pool.
                Excel.Application app = ExcelApp;

                // Ensure thread safety.
                lock (app)
                {
                    string tempPath = Pinsonault.Web.Support.TempFolder;
                    if (string.IsNullOrEmpty(tempPath) || !Directory.Exists(tempPath))
                        throw new ArgumentException("TempFolder does not exist or has not been specified in AppSettings section of web.config.  Please specify a temporary folder for generating excel files that is not a system directory.");

                    // Generate a random report file name (.xls)
                    string result = Path.Combine(tempPath, String.Concat("XR-", Guid.NewGuid().ToString().ToUpper(), ".xls"));

                    // Open the template workbook and get its default worksheet
                    Excel.Workbook workbook = ExcelApp.Workbooks.Open(_templateFile,
                        Missing.Value, true, Missing.Value, Missing.Value, Missing.Value,
                        Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                        Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                    Excel.Worksheet worksheet = (Excel.Worksheet)workbook.ActiveSheet;

                    int index = 1;
                    IList<NameValueCollection> images = Report.ExtractImagesFromRequest(QueryString);

                    foreach (NameValueCollection image in images)
                    {
                        worksheet = (Excel.Worksheet)workbook.Sheets[index];
                        //update worksheet name
                        worksheet.Name = string.Format("chart{0}", index);

                        InsertImage(worksheet, image["path"], float.Parse(image["Width"]), float.Parse(image["Height"]));
                        workbook.Sheets.Add(Missing.Value, worksheet, 1, _templateFile);
                        index++;
                    }

                    for (int i = 0; i < dsReportSet.Tables.Count; i++)
                    {
                        worksheet = (Excel.Worksheet)workbook.Sheets[index];
                        //update worksheet name and report criteria on top of excel sheet
                        UpdateHeaders(worksheet, strReportName, strHeader);
                        //insert the dataset records in excel
                        UpdateTableFormatted(worksheet, dsReportSet.Tables[i], MonthListDesc, iReportType, dtReportSummary);
                        workbook.Sheets.Add(Missing.Value, worksheet, 1, _templateFile);
                        index++;
                    }
                    // Document write protection
                    UpdateProtection(worksheet);

                    Marshal.ReleaseComObject(worksheet);

                    ((Excel.Worksheet)workbook.Sheets[1]).Activate();


                    // Save new document to disk
                    workbook.Author = Resources.Resource.Assembly_CompanyName;
                    workbook.SaveCopyAs(result);
                    workbook.Close(false, Missing.Value, Missing.Value);

                    Marshal.ReleaseComObject(workbook);

                    return result;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                ReleaseExcelApp();
            }
        }

        #endregion
    }

}
