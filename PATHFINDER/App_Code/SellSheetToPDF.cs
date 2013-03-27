using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASPPDFLib;
using System.IO;
using System.Reflection;

/// <summary>
/// Summary description for SellSheetToPDF
/// </summary>
public class SellSheetToPDF
{
    public void ExportToPDF(string url, string cookieName, string cookieValue, string fileName, string templatePath, bool inline)
    {
        IPdfManager pdf = new PdfManager();
        IPdfDocument doc = pdf.CreateDocument(Missing.Value);
        
        if (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["Preview"]))
            doc.SetViewerPrefs("HideToolbar=true,HideMenubar=true,HideWindowUI=true,FitWindow=true");

        if (templatePath != null && templatePath.IndexOf("_portrait") > 0)
            doc.ImportFromUrl(url, "landscape=false, LeftMargin=0,RightMargin=0,TopMargin=-7,BottomMargin=0", "Cookie:" + cookieName, cookieValue);
        else
            doc.ImportFromUrl(url, "landscape=true, LeftMargin=-7,RightMargin=0,TopMargin=0,BottomMargin=0", "Cookie:" + cookieName, cookieValue);

        //output background (Template)
        if (string.Compare(templatePath, "_landscape", true) != 0 && File.Exists(templatePath))
        {
            IPdfImage image = doc.OpenImage(templatePath, Missing.Value);
            IPdfParam param = pdf.CreateParam(Missing.Value);
            IPdfPage page = doc.Pages[1];

            param["x"].Value = 1;
            param["y"].Value = 1;
            //Assuming templates of 300dpi
            param["ScaleX"].Value = 1F;//4.1667F;
            param["ScaleY"].Value = 1F;//4.1667F;

            page.Background.DrawImage(image, param);
        }

        if (inline)
            doc.SaveHttp(string.Format("inline;filename={0}.pdf", fileName), Missing.Value);
        else
            doc.Save(string.Format("{0}.pdf", fileName), true);
        
        doc = null;
        pdf = null;
    }

    //Attaches a footer information page after the sellsheets page. 
    public void ExportToPDFwithFooter(string url, string cookieName, string cookieValue, string fileName, string templatePath, bool inline, string footertemplatePath)
    {
        PdfManager pdf = new PdfManager();
        IPdfDocument doc = pdf.CreateDocument(Missing.Value);

        if (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["Preview"]))
            doc.SetViewerPrefs("HideToolbar=true,HideMenubar=true,HideWindowUI=true,FitWindow=true");

        if (templatePath != null && templatePath.IndexOf("_portrait") > 0)
            doc.ImportFromUrl(url, "landscape=false, LeftMargin=0,RightMargin=0,TopMargin=-7,BottomMargin=0", "Cookie:" + cookieName, cookieValue);
        else
            doc.ImportFromUrl(url, "landscape=true, LeftMargin=-7,RightMargin=0,TopMargin=0,BottomMargin=0", "Cookie:" + cookieName, cookieValue);

        //output background (Template)
        if (string.Compare(templatePath, "_landscape", true) != 0 && File.Exists(templatePath))
        {
            IPdfImage image = doc.OpenImage(templatePath, Missing.Value);
            IPdfParam param = pdf.CreateParam(Missing.Value);
            IPdfPage page = doc.Pages[1];

            param["x"].Value = 1;
            param["y"].Value = 1;
            //Assuming templates of 300dpi
            param["ScaleX"].Value = 1F;//4.1667F;
            param["ScaleY"].Value = 1F;//4.1667F;
            
            page.Background.DrawImage(image, param);

         
            //applying footer image.
             //output background (footerTemplate)
            if (File.Exists(footertemplatePath))
            {
                image = doc.OpenImage(footertemplatePath, Missing.Value);
                param = pdf.CreateParam(Missing.Value);
                doc.Pages.Add(Missing.Value, Missing.Value, Missing.Value); ;
                page = doc.Pages[2];

                if (string.Compare(footertemplatePath, "_landscape", true) != 0)
                    page.Rotate = 90;

                param["x"].Value = 1;
                param["y"].Value = 1;
                //Assuming templates of 300dpi
                param["ScaleX"].Value = 1F;//4.1667F;
                param["ScaleY"].Value = 1F;//4.1667F;

                page.Background.DrawImage(image, param);


                if (doc.Pages.Count == 3)
                    doc.Pages.Remove(3);
            }
        }

        if (inline)
            doc.SaveHttp(string.Format("inline;filename={0}.pdf", fileName), Missing.Value);
        else
            doc.Save(string.Format("{0}.pdf", fileName), true);

        doc = null;
        pdf = null;
    }

    public void Dey_ExportToPDFwithFooter(string url, string cookieName, string cookieValue, string fileName, string templatePath, bool inline, string footertemplatePath)
    {
        PdfManager pdf = new PdfManager();
        IPdfDocument doc = pdf.CreateDocument(Missing.Value);

        if (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["Preview"]))
            doc.SetViewerPrefs("HideToolbar=true,HideMenubar=true,HideWindowUI=true,FitWindow=true");

        if (templatePath != null && templatePath.IndexOf("_portrait") > 0)
            doc.ImportFromUrl(url, "landscape=false, LeftMargin=0,RightMargin=0,TopMargin=-7,BottomMargin=0", "Cookie:" + cookieName, cookieValue);

        //output background (Template)
        if (string.Compare(templatePath, "_landscape", true) != 0 && File.Exists(templatePath))
        {
            IPdfImage image = doc.OpenImage(templatePath, Missing.Value);

            float fWidth = image.Width * 72.0f / image.ResolutionX;
            float fHeight = image.Height * 72.0f / image.ResolutionY;

            string params_landscape = "landscape=true, LeftMargin=7,RightMargin=0,TopMargin=0,BottomMargin=0";
            params_landscape = params_landscape + ",PageWidth=" + fWidth + ",PageHeight=" + fHeight;

            doc.ImportFromUrl(url, params_landscape, "Cookie:" + cookieName, cookieValue);

            IPdfParam param = pdf.CreateParam(Missing.Value);
            IPdfPage page = doc.Pages[1];

            param["x"].Value = 1;
            param["y"].Value = 1;

            //Assuming templates of 300dpi
            param["ScaleX"].Value = 1F;//4.1667F;
            param["ScaleY"].Value = 1F;//4.1667F;

            page.Background.DrawImage(image, param);

            //applying footer image.
            //output background (footerTemplate)
            if (File.Exists(footertemplatePath))
            {
                image = doc.OpenImage(footertemplatePath, Missing.Value);
                param = pdf.CreateParam(Missing.Value);

                float fWidth_footer = image.Width * 72.0f / image.ResolutionX;
                float fHeight_footer = image.Height * 72.0f / image.ResolutionY;

                doc.Pages.Add(fWidth_footer, fHeight_footer, Missing.Value);

                //doc.Pages.Add(fWidth_footer, Missing.Value, Missing.Value); ;
                page = doc.Pages[2];

                if (string.Compare(footertemplatePath, "_landscape", true) != 0)
                    page.Rotate = 90;

                param["x"].Value = 1;
                param["y"].Value = 1;
                //Assuming templates of 300dpi
                param["ScaleX"].Value = 1F;//4.1667F;
                param["ScaleY"].Value = 1F;//4.1667F;

                page.Background.DrawImage(image, param);


                if (doc.Pages.Count == 3)
                    doc.Pages.Remove(3);
            }
        }

        if (inline)
            doc.SaveHttp(string.Format("inline;filename={0}.pdf", fileName), Missing.Value);
        else
            doc.Save(string.Format("{0}.pdf", fileName), true);

        doc = null;
        pdf = null;
    }
}
