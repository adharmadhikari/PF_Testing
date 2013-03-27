using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pinsonault.Web.UI
{
    /// <summary>
    /// Summary description for TipLink
    /// </summary>
    public class TipLink : WebControl
    {
        public TipLink() { }

        public string Href { get; set; }
        public string ImageUrl { get; set; }

        protected override HtmlTextWriterTag TagKey
        {
            get { return HtmlTextWriterTag.A; }
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "jTip");
            writer.AddAttribute(HtmlTextWriterAttribute.Href, Href);
            writer.AddAttribute(HtmlTextWriterAttribute.Name, "Important Information");
            base.AddAttributesToRender(writer);
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Id, string.Format("{0}_img", this.ClientID));
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "moreInfo");
            writer.AddAttribute(HtmlTextWriterAttribute.Src, "content/images/spacer.gif");
            writer.AddAttribute(HtmlTextWriterAttribute.Name, "Important Information");
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
        }
    }
}