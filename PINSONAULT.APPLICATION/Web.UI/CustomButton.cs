using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace Pinsonault.Web.UI
{
    public class CustomButton : Button
    {
        public CustomButton()
        {
        }

        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            if(string.IsNullOrEmpty(CssClass))
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "coreBtn");
            else
                writer.AddAttribute(HtmlTextWriterAttribute.Class, string.Format("coreBtn {0}", CssClass));
            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "bg");
            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "bg2");
            writer.RenderBeginTag(HtmlTextWriterTag.Span);

            base.RenderBeginTag(writer);
        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {


            base.RenderEndTag(writer);

            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
        }
    }
}