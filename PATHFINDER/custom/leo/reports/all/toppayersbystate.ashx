<%@ WebHandler Language="C#" Class="topcommercialpayersbystate" %>

using System;
using System.IO;
using System.Web;
using System.Net;
using PathfinderModel;
using ICSharpCode.SharpZipLib.Zip;
using Pinsonault.Web;
using System.Linq;

public class topcommercialpayersbystate : GenericHttpHandler
{

    public override bool IsReusable
    {
        get { return true; }                
    }
    public string thera { get; set; }
    protected override void InternalProcessRequest(HttpContext context)
    {            
        string exportID = Guid.NewGuid().ToString();
        string tempFolder = Support.GetClientTempFolder(exportID);
        
        try
        {
            int topN = 10;
            string theraID = "53";//Set to Dermatological (Dermatoses) by default

            if ( !string.IsNullOrEmpty(context.Request.QueryString["TopN"]) )
            {
                if ( !int.TryParse(context.Request.QueryString["TopN"], out topN) )
                    topN = 10;
            }

            if (!string.IsNullOrEmpty(context.Request.QueryString["theraID"]))
            {
                thera = context.Request.QueryString["theraID"];
                theraID = thera;
                //if (!int.TryParse(thera , out theraID))
                    //theraID = "53";
            }

            //if (topN == 0) then Top Accounts Report
            if ( topN < 0 || topN > 99 ) throw new HttpException(500, "Invalid Request", new ArgumentOutOfRangeException("TopN", topN, "TopN should be between 1 and 25."));
            
            string channel = context.Request.QueryString["channel"];
            if ( !string.IsNullOrEmpty(channel) )
            {
                if (channel == "17")
                {
                    if (topN == 20)
                    {
                        if (theraID == "53")
                            channel = "TopAccounts_Dermatoses_PartD";
                        else if (theraID == "83")
                            channel = "TopAccounts_Actinic_Keratosis_PartD";
                        else
                            channel = "TopAccounts_Both_PartD";
                    }
                    else
                        channel = "MedicarePartD";
                }
                if (channel == "0")
                {
                    switch (topN)
                    {
                        case 0:
                            channel = "TopAccounts1";
                            break;
                        case 99:
                            channel = "TopAccounts1_Actinic_Keratosis";
                            break;
                        case 98:
                            channel = "TopAccounts1_Both";
                            break;
                        default:
                            channel = "Commercial";
                            break;
                    }
                }
                else if (channel == "1")
                    channel = "Commercial";
            }
            //else
            //    channel = "Commercial";
            
            ReportingServicesDownloader downloader = new ReportingServicesDownloader();
            downloader.ReportPath = string.Format("/pathfinder/webapp/leo_{0}", channel);
            downloader.ReportFormat = ReportFormat.EXCEL;
 
            
            using ( PathfinderEntities dataContext = new PathfinderEntities() )
            {
                Directory.CreateDirectory(tempFolder);
                string fileName = "";
                
                if (topN > 0 && topN != 99 && topN != 98)
                {
                    if (topN == 20)
                    {
                        downloader.Parameters["TopN"] = topN.ToString();
                        //downloader.Parameters["TheraID"] = theraID;
                        string fname = "";
                        if (theraID == "53")
                            fname = "TopAccounts_Dermatoses_PartD";
                        else if (theraID == "83")
                            fname = "TopAccounts_Actinic_Keratosis_PartD";
                        else
                            fname = "TopAccounts_Both_PartD";
                        fileName = Path.Combine(tempFolder, string.Format("{0}.xls", fname));
                        //downloader.DownloadFileTo(fileName, 20);
                        DownloadFileTo(downloader, fileName, 20);

                        fileName = Path.Combine(Support.ClientTempFolder, string.Format("{0}", exportID));
                        fileName = Path.Combine(fileName, string.Format("{0}.xls", fname));
                    }
                    else
                    {
                        foreach (State state in dataContext.StateSet)
                        {
                            fileName = Path.Combine(tempFolder, string.Format("{0}.xls", state.Name));

                            downloader.Parameters["StateID"] = state.ID;
                            downloader.Parameters["TopN"] = topN.ToString();
                            //if (!theraID.Contains(','))
                            downloader.Parameters["TheraID"] = theraID;
                            //downloader.DownloadFileTo(fileName, 1);
                            DownloadFileTo(downloader, fileName, 1);
                        }
                    
                    fileName = Path.Combine(Support.ClientTempFolder, string.Format("{0}.zip", exportID));
                    FastZip fastZip = new FastZip();
                    fastZip.CreateZip(fileName, tempFolder, false, @"\w+\.xls");
                    }
                }
                else if (topN == 0)        //if (topN == 0) then Top Accounts Report
                {
                    fileName = Path.Combine(tempFolder, string.Format("{0}.xls", "TopAccounts"));
                    //downloader.DownloadFileTo(fileName, 0);
                    DownloadFileTo(downloader, fileName, 0);
                    
                    fileName = Path.Combine(Support.ClientTempFolder, string.Format("{0}", exportID));
                    fileName = Path.Combine(fileName, string.Format("{0}.xls", "TopAccounts"));
                }

                else if (topN == 99)     // ActinicKeratosis Market Basket   
                {
                    fileName = Path.Combine(tempFolder, string.Format("{0}.xls", "ActinicKeratosis"));
                    //downloader.DownloadFileTo(fileName, 99);
                    DownloadFileTo(downloader, fileName, 99);

                    fileName = Path.Combine(Support.ClientTempFolder, string.Format("{0}", exportID));
                    fileName = Path.Combine(fileName, string.Format("{0}.xls", "ActinicKeratosis"));
                }
                else if (topN == 98)     // Both the Market Baskets   
                {
                    fileName = Path.Combine(tempFolder, string.Format("{0}.xls", "Both"));
                    //downloader.DownloadFileTo(fileName, 98);
                    DownloadFileTo(downloader, fileName, 98);

                    fileName = Path.Combine(Support.ClientTempFolder, string.Format("{0}", exportID));
                    fileName = Path.Combine(fileName, string.Format("{0}.xls", "Both"));
                }
 
                //fileName = Path.Combine(Support.ClientTempFolder, string.Format("{0}.zip", exportID));
                //FastZip fastZip = new FastZip();
                //fastZip.CreateZip(fileName, tempFolder, false, @"\w+\.xls");

                if (topN == 0)
                {
                    context.Response.AddHeader("Content-Disposition", String.Format("attachment; filename=\"{0}_{1:yyyyMMdd}.xls\"", channel, DateTime.Now));
                    //context.Response.ContentType = "application/x-excel";
                }
                if (topN == 20)
                {
                    context.Response.AddHeader("Content-Disposition", String.Format("attachment; filename=\"{0}_{1:yyyyMMdd}.xls\"", channel, DateTime.Now));
                    //context.Response.ContentType = "application/x-excel";
                }
                else if (topN == 99)     // ActinicKeratosis Market Basket   
                {
                    context.Response.AddHeader("Content-Disposition", String.Format("attachment; filename=\"{0}_{1:yyyyMMdd}.xls\"", channel, DateTime.Now));
                }

                else if (topN == 98)     // Both the Market Baskets   
                {
                    context.Response.AddHeader("Content-Disposition", String.Format("attachment; filename=\"{0}_{1:yyyyMMdd}.xls\"", channel, DateTime.Now));
                }
                   
                else
                {
                    context.Response.AddHeader("Content-Disposition", String.Format("attachment; filename=\"Top{0}{1}PayersByState_{2:yyyyMMdd}.zip\"", topN, channel, DateTime.Now));
                    context.Response.ContentType = System.Net.Mime.MediaTypeNames.Application.Zip;
                }
                
                context.Response.TransmitFile(fileName);
            }
        }
        finally
        {
            try
            {
                //Directory.Delete(tempFolder, true);
            }
            catch// (Exception ex)
            {
                //Support.EmailException(context.User.Identity.Name, context, ex);
            }
        }
    }
    
    //This function has been moved from ReportingServicesDownloader.cs to here for temporary purposes.
    //When the 'All Segment' changes have been pushed, this should be removed and the following lines of 
    //code should be uncommented above: //downloader.DownloadFileTo(x, n);
    public void DownloadFileTo(ReportingServicesDownloader r, string Path, int topN)
    {
        string queryString = Pinsonault.Data.Generic.CollectionToQueryString(r.Parameters);

        WebClient webClient = new WebClient();
        webClient.Credentials = r.Credentials;

        if (topN == 0 || topN == 99 || topN == 98) //if (topN == 0) then Top Accounts Report
            webClient.DownloadFile(string.Format("{0}?{1}&rs:Format={2}", r.ReportServer, r.ReportPath, r.ReportFormat), Path);

        else if (topN == 99 || topN == 98)     // ActinicKeratosis Market Basket   
        {
            webClient.DownloadFile(string.Format("{0}?{1}&rs:Format={2}", r.ReportServer, r.ReportPath, r.ReportFormat), Path);
        }
        else
            webClient.DownloadFile(string.Format("{0}?{1}&rs:Format={2}&{3}", r.ReportServer, r.ReportPath, r.ReportFormat, queryString), Path);
    }
}