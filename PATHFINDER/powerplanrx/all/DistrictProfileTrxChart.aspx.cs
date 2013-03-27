using System;
using System.IO;
using System.Text;
using System.Collections;
//using Persits.PDF;
using System.Reflection;
using System.Configuration;

public partial class DistrictProfileTrxChart : System.Web.UI.Page
{    
    protected void  Page_Load(object sender, EventArgs e)
    {
        //give the export page url
        hdnQString.Value = string.Format("Export.aspx{0}", Request.Url.Query);  
    }
  
}   
    

