using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Pinsonault.Data.Reports;

public partial class DistrictRegionBrandReportPDF : System.Web.UI.Page
{
    protected override void OnInit(EventArgs e)
    {
        Pinsonault.Web.Session.CheckSessionState();       
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        IList<NameValueCollection> images = Pinsonault.Data.Reports.Report.ExtractImagesFromRequest(Request.QueryString);
        if (images.Count > 0)
        {
            if (images[0] != null)
            {
                imgCommercialChart.ImageUrl = images[0]["path"];
            }
        }

        imgCommercialChart.Visible = !string.IsNullOrEmpty(imgCommercialChart.ImageUrl);
    
    }    
}
