using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace Pinsonault.Web.UI
{
    /// <summary>
    /// Customized textbox control for use with grids that require searching by user input text.  It provides script events for textbox change events "oncut", "onpaste", and "onkeyup".
    /// </summary>
    public class SearchTextBox : TextBox
    {
        public string FieldName { get; set; }
        public string FilterType { get; set; }

        protected override void OnInit(EventArgs e)
        {
            FilterType = "Contains";
            base.OnInit(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            if ( !string.IsNullOrEmpty(FieldName) )
            {
                if ( string.IsNullOrEmpty(FilterType) )
                    FilterType = "Contains";


            }
            base.OnPreRender(e);
        }

        protected override void AddAttributesToRender(System.Web.UI.HtmlTextWriter writer)
        {
            string func = string.Format("setSearchTimeout(this, {0}fieldName:'{1}',filterType:'{2}',event:event{3})", "{", FieldName, FilterType, "}");
            writer.AddAttribute("oncut", func);
            writer.AddAttribute("onpaste", func);
            writer.AddAttribute("onkeyup", func);

            base.AddAttributesToRender(writer);
        }
    }
}