using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using Pinsonault.Web;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Security.Principal;

public partial class powerplanrx_all_ReportViewer : System.Web.UI.Page
{

    public class CustomReportCredentials : Microsoft.Reporting.WebForms.IReportServerCredentials
    {
        // local variable fornetwork credential.
        private string _UserName;
        private string _PassWord;
        private string _DomainName;
        public CustomReportCredentials(string UserName, string PassWord, string DomainName)
        {
            _UserName = UserName;
            _PassWord = PassWord;
            _DomainName = DomainName;
        }
        public WindowsIdentity ImpersonationUser
        {
            get
            {
                return null; // not use ImpersonationUser
            }
        }
        public ICredentials NetworkCredentials
        {
            get
            {
                // use NetworkCredentials
                return new NetworkCredential(_UserName, _PassWord, _DomainName);
            }
        }
        public bool GetFormsCredentials(out Cookie authCookie, out string user, out string password, out string authority)
        {
            // not use FormsCredentials unless you have implements acustom autentication.
            authCookie = null;
            user = password = authority = null;
            return false;
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //get report server url        
            ReportingServicesDownloader downloader = new ReportingServicesDownloader();

            ReportViewer1.ServerReport.ReportServerUrl = new Uri(downloader.ReportServer);

            NameValueCollection config = ConfigurationManager.GetSection(@"pinsonault/reportingServicesDownloader") as NameValueCollection;

            string userName = config["userName"];
            string password = config["password"];
            string domain = config["domain"];

            IReportServerCredentials irsc = new CustomReportCredentials(userName, password, domain);

            ReportViewer1.ServerReport.ReportServerCredentials = irsc;

            // report name and path
            if (!string.IsNullOrEmpty(Request.QueryString["reportname"]) && !string.IsNullOrEmpty(Request.QueryString["report"]))
                ReportViewer1.ServerReport.ReportPath = string.Format("/{0}/{1}", Request.QueryString["reportname"], Request.QueryString["report"]);


            //get query string key names
            String[] a;
            a = Request.QueryString.AllKeys;
            int i = 0;
            //loop through keys
            foreach (string key in Request.QueryString.Keys)
            {
                //set report parameters for key name and value, skip reportname and report
                if (!string.IsNullOrEmpty(Request.QueryString[key]) && !a[i].Equals("reportname") && !a[i].Equals("report"))
                {
                    ReportParameter param = new ReportParameter(a[i], Request.QueryString[key]);
                    this.ReportViewer1.ServerReport.SetParameters(new ReportParameter[] { param });
                }
                i++;
            }

            //hide the parameter section in report viewer
            this.ReportViewer1.ShowParameterPrompts = false;
        }
    }
}
