using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using System.Collections.Specialized;
using Pinsonault.Data;
using System.IO;
using System.Configuration;

namespace Pinsonault.Web
{
    public enum ReportFormat
    {
        HTML32, 
        HTML40, 
        MHTML, 
        IMAGE, 
        EXCEL, 
        WORD, 
        CSV, 
        PDF, 
        XML
    }

    public class ReportingServicesDownloader
    {
        public string ReportServer { get; set; }
        public string ReportPath { get; set; }

        public ReportFormat ReportFormat { get; set; }

        string ReportFormatAsString
        {
            get
            {
                if ( ReportFormat == ReportFormat.HTML32 )
                    return "HTML3.2";
                else if ( ReportFormat == ReportFormat.HTML40 )
                    return "HTML4.0";
                else
                    return ReportFormat.ToString();
            }
        }

        public NameValueCollection Parameters { get; private set; }

        public ReportingServicesDownloader() : this(string.Empty) { } 

        public ReportingServicesDownloader(string ReportPath)
        {
            Parameters = new NameValueCollection();

            NameValueCollection config = ConfigurationManager.GetSection(@"pinsonault/reportingServicesDownloader") as NameValueCollection;
            
            string userName = config["userName"];
            string password = config["password"];
            string domain = config["domain"];
            string serverUrl = config["serverUrl"];
            
            this.ReportPath = ReportPath;

            this.ReportServer = serverUrl;

            if ( !string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password) )
                Credentials = new System.Net.NetworkCredential(userName, password, domain);
        }

        public ReportingServicesDownloader(string ReportServer, string ReportPath, string UserName, string Password, string Domain)
        {
            Parameters = new NameValueCollection();

            this.ReportServer = ReportServer;
            this.ReportPath = ReportPath;
            Credentials = new System.Net.NetworkCredential(UserName, Password, Domain);
        }

        public NetworkCredential Credentials { get; set; }

        /// <summary>
        /// Downloads file to client's temp folder with a auto generated name based on a GUID and file extension of "tmp".
        /// </summary>
        /// <returns></returns>
        //public string DownloadFile()
        //{
        //    string exportID = Guid.NewGuid().ToString();
        //    string tempFile = Support.GetClientFolder(string.Format(@"temp\{0}.tmp", exportID));

        //    DownloadFileTo(tempFile);

        //    return tempFile;
        //}

        /// <summary>
        /// Downloads a file to the specified location.
        /// </summary>
        /// <param name="Path">Path where file will be stored.  It should include folder and file name.</param>
        public void DownloadFileTo(string Path, int topN)
        {
            string queryString = Generic.CollectionToQueryString(Parameters);

            WebClient webClient = new WebClient();
            webClient.Credentials = Credentials;

            if (topN == 0 || topN == 99 || topN == 98) //if (topN == 0) then Top Accounts Report
                webClient.DownloadFile(string.Format("{0}?{1}&rs:Format={2}", ReportServer, ReportPath, ReportFormatAsString), Path);

            else if (topN == 99 || topN == 98)     // ActinicKeratosis Market Basket   
            {
                webClient.DownloadFile(string.Format("{0}?{1}&rs:Format={2}", ReportServer, ReportPath, ReportFormatAsString), Path);
            }
            else
                webClient.DownloadFile(string.Format("{0}?{1}&rs:Format={2}&{3}", ReportServer, ReportPath, ReportFormatAsString, queryString), Path);
        }

        /// <summary>
        /// Writes a file directly to the HttpResponse stream.
        /// </summary>
        /// <param name="FileName">File name that appears in the Open/Save dialog box of the user's web browser.</param>
        public void TransmitFile(string FileName)
        {            
            HttpResponse response = HttpContext.Current.Response;
            
            response.Clear();
            
            response.AddHeader("Content-Disposition", String.Format("attachment; filename=\"{0}\"", FileName));
            response.ContentType = System.Net.Mime.MediaTypeNames.Application.Octet;

            string queryString = Generic.CollectionToQueryString(Parameters);

            WebClient webClient = new WebClient();
            webClient.Credentials = Credentials;

            byte[] buffer = new byte[4096];
            int length;
            using ( Stream stream = webClient.OpenRead(string.Format("{0}?{1}&rs:Format={2}&{3}", ReportServer, ReportPath, ReportFormatAsString, queryString)) )
            {                
                while ( (length = stream.Read(buffer, 0, 4096)) > 0 )
                {
                    response.BinaryWrite(buffer.Take(length).ToArray());
                }
            }
        }

    }
}
