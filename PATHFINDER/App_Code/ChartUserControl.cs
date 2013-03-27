using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Dundas.Charting.WebControl;
using System.Drawing;
using System.IO;


namespace Pinsonault.Web
{
    public class ChartHandler : Pinsonault.Web.GenericHttpHandler
    {
        public override bool IsReusable
        {
            get { return false; }
        }

        protected override void InternalProcessRequest(HttpContext context)
        {
            string id = context.Request["id"];
            if ( string.IsNullOrEmpty(id) )
                id = System.IO.Path.GetFileNameWithoutExtension(context.Request.Url.AbsolutePath);

            string type = context.Request["type"];
            if ( string.IsNullOrEmpty(type) )
                type = System.IO.Path.GetExtension(context.Request.Url.AbsolutePath);

            if ( !string.IsNullOrEmpty(id) )
            {
                string path = System.IO.Path.Combine(Pinsonault.Web.Support.GetClientTempFolder("charts"), id);
                path = System.IO.Path.ChangeExtension(path, type);
                if ( System.IO.File.Exists(path) )
                {
                    context.Response.Cache.SetExpires(DateTime.UtcNow.AddHours(12D));
                    context.Response.ContentType = (type.EndsWith("jpeg", StringComparison.InvariantCultureIgnoreCase) ? System.Net.Mime.MediaTypeNames.Image.Jpeg : System.Net.Mime.MediaTypeNames.Application.Octet);
                    context.Response.TransmitFile(path);
                }
                else
                    throw new HttpException(404, string.Format("Requested chart was not found with id of {0}.", id));
            }
            else
                throw new HttpException(500, "Failed to return Chart; id is required.");
        }
    }
}

public class ChartUserControlManager
{
    List<ChartUserControl> _charts = new List<ChartUserControl>();

    public static ChartUserControlManager Current
    {
        get
        {
            ChartUserControlManager chartManager = HttpContext.Current.Items["chartUserControlManager"] as ChartUserControlManager;
            if ( chartManager == null )
            {
                chartManager = new ChartUserControlManager();
                HttpContext.Current.Items["chartUserControlManager"] = chartManager;
            }

            return chartManager;
        }
    }

    public List<ChartUserControl> Charts { get { return _charts; } }

    public int GetVisibleChartCount()
    {
        int count = 0;
        foreach ( ChartUserControl chart in Charts )
        {
            if ( chart.Visible && chart.HostedChart.Visible )
            {
                count++;
            }
        }
        return count;
    }
    
    public System.Web.UI.WebControls.Unit GetPreferredHeight()
    {
        return new System.Web.UI.WebControls.Unit("240px"); 
    }

    public System.Web.UI.WebControls.Unit GetPreferredWidth()
    {
        int count = GetVisibleChartCount();
        return new System.Web.UI.WebControls.Unit((count <= 1 ? "480px" : count == 2 ? "380px" : "280px"));      
    }
}

/// <summary>
/// Summary description for ChartUserControl
/// </summary>
public abstract class ChartUserControl : UserControl
{
    public ChartUserControl()
    {
        ContainerCssClass = "chartContainer";
        ThumbnailCssClass = "chartThumb";
        //ShowToolTip = true;
        //ToolTipUrl = "content/tooltips/chartinfo.htm";

        ThumbnailLabel = Resources.Resource.Label_Chart_Thumbnail_Instruct;

        ChartUserControlManager.Current.Charts.Add(this);
    }

    public abstract Chart HostedChart { get; }
    
    public bool Thumbnail { get; set; }
    
    public string ThumbnailCssClass { get; set; }
    
    public string ThumbnailLabel { get; set; }

    public bool ShowToolTip { get; set; }

    public bool ShowLegendOnlyInExport { get; set; }

    public int ExportWidth { get; set; }

    public int ExportHeight { get; set; }

    public string ToolTipUrl { get; set; }

    public string ContainerCssClass { get; set; }

    public virtual System.Web.UI.WebControls.Unit GetRenderedHeight()
    {
        return ChartUserControlManager.Current.GetPreferredHeight();
    }

    public virtual System.Web.UI.WebControls.Unit GetRenderedWidth()
    {
        return ChartUserControlManager.Current.GetPreferredWidth(); 
    }

    protected virtual void RenderBeginThumb(HtmlTextWriter writer)
    {
        //writer.AddAttribute(HtmlTextWriterAttribute.Id, HostedChart.ID);

        if ( !string.IsNullOrEmpty(ThumbnailCssClass) )
            writer.AddAttribute(HtmlTextWriterAttribute.Class, ThumbnailCssClass);

        writer.RenderBeginTag(HtmlTextWriterTag.Div);

        //if ( ShowToolTip )
        //{
        //    writer.AddAttribute(HtmlTextWriterAttribute.Class, "jTip");
        //    writer.AddAttribute(HtmlTextWriterAttribute.Href, ToolTipUrl);
        //    writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + "_tip");
        //    writer.AddAttribute(HtmlTextWriterAttribute.Name, "Important Information");
        //    writer.RenderBeginTag(HtmlTextWriterTag.A);
        //}
        //if ( !string.IsNullOrEmpty(ThumbnailLabel) )
        //{
        //    writer.RenderBeginTag(HtmlTextWriterTag.Span);
        //    writer.Write(ThumbnailLabel);
        //    writer.RenderEndTag();
        //}        
    }

    protected virtual void RenderEndThumb(HtmlTextWriter writer)
    {
        //if ( ShowToolTip )
        //    writer.RenderEndTag();//close out A tag

        writer.RenderEndTag();
    }

    void renderParam(HtmlTextWriter writer, string name, string value)
    {
        writer.AddAttribute("NAME", name);
        writer.AddAttribute("VALUE", value);
        writer.RenderBeginTag(HtmlTextWriterTag.Param);
        writer.RenderEndTag();
    }

    protected override void Render(HtmlTextWriter writer)
    {
        writer.AddAttribute(HtmlTextWriterAttribute.Class, ContainerCssClass);
        writer.RenderBeginTag(HtmlTextWriterTag.Div);

        if ( HostedChart.Visible )
        {
            bool supportFlash = Pinsonault.Web.Session.SupportFlash;

            string chartID = Guid.NewGuid().ToString();

            HostedChart.ImageType = Dundas.Charting.WebControl.ChartImageType.Jpeg;
            HostedChart.AnimationTheme = AnimationTheme.None;
            HostedChart.BackColor = Color.White;
            HostedChart.RenderType = RenderType.ImageTag;

            if ( !supportFlash )
            {
                Font currentFont = HostedChart.Legends[0].Font;
                HostedChart.Legends[0].Font = new Font(currentFont.FontFamily, 7F);
                HostedChart.Titles[0].Font = new Font(currentFont.FontFamily, 10F);
                currentFont = HostedChart.Series[0].Font;
                HostedChart.Series[0].Font = new Font(currentFont.FontFamily, 8F);
                currentFont = HostedChart.ChartAreas[0].AxisX.LabelStyle.Font;
                HostedChart.ChartAreas[0].AxisX.LabelStyle.Font = new Font(currentFont.FontFamily, 8F);
                HostedChart.Width = GetRenderedWidth();
                HostedChart.Height =GetRenderedHeight();
            }

           
            //check if legend should be included in cache version for export
            if (ShowLegendOnlyInExport)
                HostedChart.Legends[0].Enabled = true; //enable legend before cache version is created

            //obtain dimensions of chart 

            System.Web.UI.WebControls.Unit width = HostedChart.Width;
            System.Web.UI.WebControls.Unit height = HostedChart.Height;

            if (supportFlash)
            {
                //set cache version dimensions to 800 x 400
                HostedChart.Width = ExportWidth != 0 ? ExportWidth : 800;
                HostedChart.Height = ExportHeight != 0 ? ExportHeight : 400;
            }

            //cache version for export - we are no longer using Dundas built in disk caching because we can't control for each client
            HostedChart.Save(Path.ChangeExtension(Path.Combine(Pinsonault.Web.Support.GetClientTempFolder("charts"), chartID), "jpeg"), ChartImageFormat.Jpeg);            

            //revert legend status for page display if legend should be included for export
            if (ShowLegendOnlyInExport)
                HostedChart.Legends[0].Enabled = false; //disable legend after cache version is created

            //revert dimensions
            HostedChart.Width = width;
            HostedChart.Height = height;

            //HostedChart.ImageUrl = string.Format("~/usercontent/chart.ashx?id={0}&type=swf", chartID);
            if ( supportFlash )//Pinsonault.Web.Session.SupportFlash )
            {
                //base.Render(writer);
                string url = Page.ResolveUrl(string.Format("~/usercontent/chart.ashx?id={0}&type=swf", chartID));
                //what was rendered previously by dundas
                writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
                writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "100%");
                if ( string.Compare(Request.Browser.Browser, "IE", true) == 0 )
                {
                    writer.AddAttribute("codeBase", "https://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,0,0");//<--hard coded may be a problem
                    writer.AddAttribute("classid", "clsid:D27CDB6E-AE6D-11cf-96B8-444553540000");//<--hard coded may be a problem
                    writer.AddAttribute("width", HostedChart.Width.Value.ToString());
                    writer.AddAttribute("height", HostedChart.Height.Value.ToString());
                    writer.RenderBeginTag(HtmlTextWriterTag.Object);
                    renderParam(writer, "FlashVars", "");
                    renderParam(writer, "Movie", url);
                    renderParam(writer, "Src", url);
                    renderParam(writer, "WMode", "Transparent");
                    renderParam(writer, "Play", "0");
                    renderParam(writer, "Loop", "-1");
                    renderParam(writer, "Quality", "High");
                    renderParam(writer, "SAlign", "");
                    renderParam(writer, "Menu", "-1");
                    renderParam(writer, "Base", "");
                    renderParam(writer, "AllowScriptAccess", "");
                    renderParam(writer, "Scale", "ShowAll");
                    renderParam(writer, "DeviceFont", "0");
                    renderParam(writer, "EmbedMovie", "0");
                    renderParam(writer, "BGColor", "");
                    renderParam(writer, "SWRemote", "");
                    renderParam(writer, "MovieData", "");
                    renderParam(writer, "SeamlessTabbing", "1");
                    renderParam(writer, "Profile", "0");
                    renderParam(writer, "ProfileAddress", "");
                    renderParam(writer, "ProfilePort", "0");
                    renderParam(writer, "AllowNetworking", "all");
                    renderParam(writer, "AllowFullScreen", "false");
                    writer.RenderEndTag();
                }
                else
                {
                    //
                    writer.AddAttribute(HtmlTextWriterAttribute.Src, url);
                    writer.AddAttribute("width", HostedChart.Width.Value.ToString());
                    writer.AddAttribute("height", HostedChart.Height.Value.ToString());
                    writer.AddAttribute("WMode", "Transparent");
                    writer.AddAttribute("pluginspage", "https://www.macromedia.com/go/getflashplayer");
                    writer.AddAttribute("type", "application/x-shockwave-flash");
                    writer.RenderBeginTag(HtmlTextWriterTag.Embed);
                    writer.RenderEndTag();
                }

                HostedChart.ImageType = ChartImageType.Flash;
                //cache version for export - we are no longer using Dundas built in disk caching because we can't control for each client
                HostedChart.Save(Path.ChangeExtension(Path.Combine(Pinsonault.Web.Support.GetClientTempFolder("charts"), chartID), "swf"), ChartImageFormat.Flash);
            }
            else //render as image - no flash - don't worry about saving - done regardless for thumbnail and export - see below
            {
                string map = string.Format("{0}_imageMap", chartID);
                writer.Write(HostedChart.GetHtmlImageMap(map));
                //writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
                //writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "100%");
                //writer.AddStyleAttribute(HtmlTextWriterStyle.Width, HostedChart.Width.ToString());
                //writer.AddStyleAttribute(HtmlTextWriterStyle.Height, HostedChart.Height.ToString());
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "chart");
                writer.AddAttribute(HtmlTextWriterAttribute.Usemap, "#" + map);
                writer.AddAttribute(HtmlTextWriterAttribute.Src, Page.ResolveUrl(string.Format("~/usercontent/chart.ashx?id={0}&type=jpeg", chartID)));
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                writer.RenderEndTag();                                
            }


            //render for thumbnail
            if ( supportFlash && Thumbnail )
            {
                HostedChart.ToolTip = ThumbnailLabel;

                RenderBeginThumb(writer);
                //HostedChart.RenderControl(writer);
                //writer.AddAttribute("_width", HostedChart.Width.Value.ToString());
                //writer.AddAttribute("_height", HostedChart.Height.Value.ToString());
                writer.AddAttribute("_width", ExportWidth != 0 ? ExportWidth.ToString() : "800");
                writer.AddAttribute("_height", ExportHeight != 0 ? ExportHeight.ToString() : "400");
                writer.AddAttribute("_chartid", chartID);
                writer.AddAttribute("_title", HostedChart.Attributes["_title"]);
                //writer.AddAttribute(HtmlTextWriterAttribute.Id, HostedChart.ClientID);
                writer.AddAttribute(HtmlTextWriterAttribute.Src, Page.ResolveUrl(string.Format("~/usercontent/chart.ashx?id={0}&type=jpeg", chartID)));
                writer.AddAttribute("title", ThumbnailLabel);
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                writer.RenderEndTag();
                RenderEndThumb(writer);
            }
        }
        else
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.Write(Resources.Resource.Label_Chart_No_Data);
            writer.RenderEndTag();
        }

        writer.RenderEndTag();
    }
}
