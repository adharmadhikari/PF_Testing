using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace Pathfinder
{

    public class StylesheetOutputEntry
    {
        public string Url { get; set; }
    }

    /// <summary>
    /// Summary description for StylesheetOutput
    /// </summary>
    public class StylesheetOutput : WebControl
    {
        public StylesheetOutput()
        {
            IncludeMain = true;
            IncludeTheme = true;
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {

            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/css");
        }

        public bool IncludeTheme { get; set; }
        public bool IncludeMain { get; set; } 

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public List<StylesheetOutputEntry> StylesheetOutputEntries
        {
            get;
            set;
        }

        protected override HtmlTextWriterTag TagKey
        {
            get { return HtmlTextWriterTag.Style; }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            if ( IncludeMain ) writer.Write(getCleanedText("~/content/styles/main.css"));
            if ( IncludeTheme ) writer.Write(getCleanedText(string.Format("~/app_themes/{0}/pathfinder.css", Pinsonault.Web.Support.Theme)));

            if ( StylesheetOutputEntries != null )
            {
                foreach ( StylesheetOutputEntry styleSheet in StylesheetOutputEntries )
                {
                    writer.Write(getCleanedText(styleSheet.Url));
                }
            }
        }

        string getCleanedText(string url)
        {
            string[] allLines = File.ReadAllLines(HttpContext.Current.Server.MapPath(url)).Where(s=>!s.Contains("*") && !s.Contains("!important")).ToArray();
            return string.Join(" ", allLines);
        }
    }
}