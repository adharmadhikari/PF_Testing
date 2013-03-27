using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Serialization.Json;
using System.IO;
using Pinsonault.Web;

public partial class _home : PageBase
{
    protected override void OnLoad(EventArgs e)
    {
        if ( Context.User.IsInRole("home") )
        {
            favoritesOptions.Disabled = !Context.User.IsInRole("fav");

            base.OnLoad(e);
        }
        else
        {
            Response.Redirect("~/dashboard.aspx");
        }
    }

    protected void OnApplicationsDataBound(object sender, EventArgs e)
    {        
        try
        {
            //Get last selected application id and select 
            HttpCookie cookie = Request.Cookies["s"];
            if ( cookie != null )
            {
                string val = cookie.Value;
                if ( !string.IsNullOrEmpty(val) )
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ApplicationState));
                    ApplicationState state;
                    
                    byte[] bytes = System.Text.UTF8Encoding.UTF8.GetBytes(HttpUtility.UrlDecode(val));

                    using ( MemoryStream ms = new MemoryStream(bytes) )
                    {
                        state = (ApplicationState)serializer.ReadObject(ms);
                    }                    
                    
                    rcbApplication.SelectedValue = state.Application.ToString();                    
                }
            }
        }
        catch
        {
            //Don't care about exception - just leave default selection (TA)
        }

    }
}
