using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace Pinsonault.Web.UI
{
    /// <summary>
    /// CustomButtonNonServer should be used when button does not need to postback to the server and a client event will be attached instead.  The ID of the control will be assigned to the INPUT tag in the original, unmangled, format.  The outer span can still be referenced using unique id assigned by .Net framework if needed.
    /// </summary>
    public class CustomButtonNonServer : WebControl
    {
        protected override HtmlTextWriterTag TagKey
        {
            get { return HtmlTextWriterTag.Span; }
        }

        public string Text { get; set; }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, string.Format("{0} coreBtn", CssClass));
            base.AddAttributesToRender(writer);
        }

        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            base.RenderBeginTag(writer);

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "bg");
            writer.RenderBeginTag(HtmlTextWriterTag.Span);

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "bg2");
            writer.RenderBeginTag(HtmlTextWriterTag.Span);

            writer.AddAttribute(HtmlTextWriterAttribute.Type, "button");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "button");
            writer.AddAttribute(HtmlTextWriterAttribute.Value, Text);
            writer.AddAttribute(HtmlTextWriterAttribute.Id, ID);// we want short id to easily reference in client script - hopefully all will be unique
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
            base.RenderEndTag(writer);
        }
    }
}