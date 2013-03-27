using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pinsonault.Data.Reports
{
    /// <summary>
    /// Column mapping entry.
    /// </summary>
    public class ColumnMap
    {
        /// <summary>
        /// Column width.
        /// Uses template's default width if not set.
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// Format string.
        /// </summary>
        public String DataFormat { get; set; }

        /// <summary>
        /// Name of the class property this mapping is for.
        /// </summary>
        public String PropertyName { get; set; }

        private String _translatedName;

        /// <summary>
        /// Translated name of the class property this mapping is for.
        /// If none is specified, this defaults to the original PropertyName.
        /// </summary>
        public String TranslatedName
        {
            get { return String.IsNullOrEmpty(_translatedName) ? PropertyName : GetTranslatedNameFromResource(_translatedName); }
            set { _translatedName = value; }
        }

        public static string GetTranslatedNameFromResource(string name)
        {
            string translatedName = Resources.Resource.ResourceManager.GetString(name);
            if ( !string.IsNullOrEmpty(translatedName) )
                return translatedName;

            return name;
        }        

        /// <summary>
        /// For Dual Header Grid, this value is for First Header Row Cell
        /// </summary>
        public String FirstHeaderTranslatedName { get; set; }

        /// <summary>
        /// For Dual Header Grid, this value is for CellSpan required for merging First Header Cells
        /// </summary>
        public int MergedCellSpan { get; set; }
        
        /// <summary>
        /// For Dual Header Grid, this value is either "H" if it is first Header cell in merged cell or "R" if it is repeated first header cell
        /// </summary>
        public string HeaderRepeaterCell { get; set; }

        /// <summary>
        /// For Dual Header Grid, this value is for second Header Row Cell
        /// </summary>
        public string SecondHeaderTranslatedName { get; set; }

        /// <summary>
        /// For Dual Header Grid, this value is for getting the required DB column name, whoose value will be required for special tasks i.e. color coding the cell etc.
        /// </summary>
        public string DBColToCompare { get; set; }

    }
}
