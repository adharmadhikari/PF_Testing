using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pinsonault.Web.Security;
using System.Web.Security;

public partial class usercontent_PasswordGuidelines : PageBase 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PathfinderMembershipUser memberuser = Membership.GetUser(HttpContext.Current.User.Identity.Name) as PathfinderMembershipUser;
        if (memberuser != null)
        {
            PathfinderMembershipProvider pfmp = new PathfinderMembershipProvider();            
            
            if (memberuser.HighSecurityClient)
            {
                lblhighsecurityPwdLength.Text = string.Format("New Password should be at least {0} characters long and it should not match previous {1} passwords.", pfmp.MinRequiredPasswordLength, pfmp.MatchPreviousPasswords + 1);
                divHighSecurityClient.Visible = true;
                divGenericSecurityClient.Visible = false;
            }
            else
                lblPwdLength.Text = string.Format("New Password should be at least {0} characters long and it should have at least {1} Non-alphanumeric character.", pfmp.MinRequiredPasswordLength, pfmp.MinRequiredNonAlphanumericCharacters);
        }
    }
}
