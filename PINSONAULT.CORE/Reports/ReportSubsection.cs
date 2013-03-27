using System.Collections.Generic;
using System.Collections;
namespace Pinsonault.Data.Reports
{

    public class ReportSubsection
    {
        public ReportSubsection()
        {
            ColumnMap = new List<ColumnMap>();
            //Criteria = new NameValueCollection();
        }

        public Dictionary<string, CriteriaItem> CriteriaItems { get; set; }

        string _name = string.Empty;
        public string Name
        {
            get
            {
                if ( !string.IsNullOrEmpty(_name) )
                {
                    string key = _name.ToLower();
                    if ( key.StartsWith("#") && CriteriaItems.ContainsKey(key.Substring(1)) )
                    {
                        return CriteriaItems[key.Substring(1)].Text;
                    }
                    return _name;
                }
                return string.Empty;
            }
            set { _name = value; }
        }

        /// <summary>
        /// Column mapping (Required; Ordered List)
        /// </summary>
        /// <remarks>
        /// Shows up as the columns in the template's table block.
        /// </remarks>
        public IList<ColumnMap> ColumnMap { get; set; }

        /// <summary>
        /// List of report filtering criteria used (Optional)
        /// </summary>
        /// <remarks>
        /// Shows up in template's header block.
        /// </remarks>
        //public NameValueCollection Criteria { get; set; }


        /// <summary>
        /// Data to bind to.
        /// </summary>
        public IEnumerable Data { get; set; }

        public ReportDefinition ReportDefinition { get; set; }

        public string ImageUrl { get; set; }
        public string ImagePath { get; set; }
        public string ChartID { get; set; }

        public float Height { get; set; }
        public float Width { get; set; }

        public bool IsImage()
        {
            return !string.IsNullOrEmpty(ChartID);
        }
    }
}