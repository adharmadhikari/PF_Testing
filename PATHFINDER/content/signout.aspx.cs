using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Pinsonault.Web.Data;
using Utilities = Pinsonault.Web.Utilities;
using Pinsonault.Data;

public partial class signout : ContentPageBase
{
    protected override void OnLoad(EventArgs e)
    {      
        //check if user has logged in        
        if (Session["UserID"] != null)
        {
            try
            {              
                int iUserID = Convert.ToInt32(Session["UserID"]);
                if (iUserID != 0)
                {
                    UserUpdate.UpdateUserLogout(iUserID);
                }
            }
            catch(Exception ex)
            {
                Utilities.EventLogLogger eventLogger = new Utilities.EventLogLogger();
                eventLogger.LogError(ex.Message.ToString() , ex.StackTrace.ToString());                
            }           
        }
        System.Web.Security.FormsAuthentication.SignOut();

        Session.Abandon();

        Response.Redirect("~/login.aspx");
        
                       
    }
    
}
