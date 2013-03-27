using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Pinsonault.Web.Security;
using PathfinderModel;

public partial class Controls_login : System.Web.UI.UserControl
{
    public bool LoginPageCheck { get; set; }

    //public bool AllowChangeUser { get; set; }

    //public bool AllowPasswordReset { get; set; }
    bool MaxWindowCheck { get; set; }

    public Controls_login()
    {
        string val = ConfigurationManager.AppSettings["ForceMaxWindow"];
        bool maxCheck;
        bool.TryParse(val, out maxCheck);
        MaxWindowCheck = maxCheck;
        LoginPageCheck = true;
        //AllowChangeUser = true;
        //AllowPasswordReset = true;
    }


    protected override void OnLoad(EventArgs e)
    {
        if(LoginPageCheck)
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "loginpagecheck", string.Format("{0} if ( window.top.location.pathname.indexOf(\"/login.aspx\") == -1 ) window.top.location = \"login.aspx\";", !MaxWindowCheck ? "" : "if ( window.name != \"PathfinderRx\" ) window.top.location = \"default.aspx\";"), true);

        Control control = Login1.FindControl("additionalOptions");
        control.Visible = LoginPageCheck;

        if ( !Page.IsPostBack )
        {
            string name = getUserNameCookieVal();

            TextBox textBox = (TextBox)Login1.FindControl("UserName");
            if(!string.IsNullOrEmpty(name))
            {
                textBox.Text = name;
                textBox.ReadOnly = !string.IsNullOrEmpty(textBox.Text);
                textBox.CssClass = "textBoxReadOnly";
            }
            else if ( !LoginPageCheck )
            {
                Response.Redirect("~/login.aspx"); //must go back to main login page because no way to validate user name (no switching)
            }
            else
            {
                textBox.Text = "";
                textBox.ReadOnly = false;
                textBox.CssClass = "textBox";
            }
        }
        else if ( !LoginPageCheck )
        {
            string name = getUserNameCookieVal();

            TextBox textBox = (TextBox)Login1.FindControl("UserName");
            
            if(string.IsNullOrEmpty(name) || textBox.Text != name)
            {
                Response.Redirect("~/login.aspx");
            }

            textBox.Text = name;
            textBox.ReadOnly = !string.IsNullOrEmpty(textBox.Text);
            textBox.CssClass = "textBoxReadOnly";

        }

        Login1.LoggedIn += new EventHandler(OnLoggedIn);
        base.OnLoad(e);
    }

    void OnLoggedIn(object sender, EventArgs e)
    {
        Session.Clear();

        HttpCookie cookie = new HttpCookie("u", FormsAuthentication.Encrypt(new FormsAuthenticationTicket(Login1.UserName, true, 0)));
        cookie.Expires = DateTime.MaxValue;
        Response.Cookies.Add(cookie);

        //code to check if the flag is set to change the password, then redirect the user to change password page.  
        PathfinderMembershipUser memberuser = Membership.GetUser(Login1.UserName) as PathfinderMembershipUser;
        if (memberuser != null)
        {
            //int userID = user.UserID;
            using (PathfinderEntities pfe = new PathfinderModel.PathfinderEntities())
            {
                var forcechangepassword = (from p in pfe.UserSet where p.User_ID == memberuser.UserID select p.Force_Change_Pwd).FirstOrDefault();
                //code to check if password is expired or not
                bool bpasswordexpired = false;

                TimeSpan dateDifference ;
                //check if the difference between latest last password expiration date and current date < clients passwrod expirations days 
                var lastpasswordmodifieddate = (from phist in pfe.ClientUserPwdHistorySet where phist.User_ID == memberuser.UserID orderby phist.Pwd_History_Id descending select phist.Pwd_Exp_DT);
                                
                if (lastpasswordmodifieddate.Count() > 0)
                    dateDifference = System.DateTime.Today.Date.Subtract(Convert.ToDateTime(lastpasswordmodifieddate.FirstOrDefault()).Date);

                else
                    dateDifference = System.DateTime.Today.Date.Subtract(memberuser.CreationDate.Date);

                //if client has set the reset password date and user does not belong to service account role ("login_sa" in User_roles table) 
                //and lastpasswordmodifieddate > client's set limit, then force user to change the password

                if (memberuser.PasswordResetDays > 0 && !memberuser.Roles.Contains("login_sa") && dateDifference.Days > memberuser.PasswordResetDays)
                {   
                      bpasswordexpired = true;
                }               
                
                if (forcechangepassword || bpasswordexpired)
                    Login1.DestinationPageUrl = "~/usercontent/ChangePassword_Forced.aspx";
            }
        }
        
        if (!LoginPageCheck)
            Login1.DestinationPageUrl = "~/content/reauthenticatesuccess.aspx";

    }

    string getUserNameCookieVal()
    {
        HttpCookie cookie = Request.Cookies["u"];
        if ( cookie != null )
        {            
            try
            {
                return FormsAuthentication.Decrypt(cookie.Value).Name;
            }
            catch //In case of error reset to basic configuration
            {                
            }
        }

        return null;
    }
}
