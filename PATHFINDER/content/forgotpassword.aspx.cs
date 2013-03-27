using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Pinsonault.Web;
using Pinsonault.Web.Security;
using PathfinderModel;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;



public partial class content_forgotpassword : ContentPageBase
{

    protected void OnSubmitRequest(object sender, EventArgs e)
    {
        bool submitSuccess = false;
        PathfinderMembershipUser user = Membership.GetUser(emailAddress.Text) as PathfinderMembershipUser;
        if ( user != null )
        {
            string body = string.Format(Resources.Resource.ForgotPassword_MessageFormat,
                                                user.FirstName, user.LastName, user.ClientName, user.Email);
            string subject = string.Format(Resources.Resource.ForgotPassword_MessageSubject, user.FirstName, user.LastName, user.ClientName);

            if (user.ClientID == 48)  //alcon: send a temporary password
            {
                // sl: 4/30/2012 Random temp Password
                Random r = new Random();
                string newPassword = Convert.ToString(((r.Next()) * 10001));
                newPassword = newPassword.Substring(1, newPassword.Length - 1).Trim();  //to avoid negative number just in case

                using (PathfinderEntities context = new PathfinderEntities())
                {
                    //step1. current password should be saved in History table(Client_User_Pwd_History)
                    var userOldPassword = (from u in context.UserSet
                                           where u.User_ID == user.UserID
                                           select u.User_PWD).FirstOrDefault();

                    ClientUserPwdHistory passwordhistory = new ClientUserPwdHistory();
                    passwordhistory.Client_ID = user.ClientID;
                    passwordhistory.User_ID = user.UserID;
                    passwordhistory.User_PWD = userOldPassword;
                    passwordhistory.Pwd_Exp_DT = DateTime.UtcNow;
                    context.AddToClientUserPwdHistorySet(passwordhistory);
                    context.SaveChanges();

                    //step2. 
                    var user1 = (from u in context.UserSet
                                 where u.User_ID == user.UserID
                                 select u).FirstOrDefault();

                    user1.User_PWD = Pinsonault.Security.Security.CreateHashedPassword(user.Email, newPassword);
                    user1.Force_Change_Pwd = true;
                    context.SaveChanges();

                    StringBuilder sb = new StringBuilder();

                    sb.AppendFormat("{0} \n", "A forgot password request is received for your account and a temporary password has been created.")
                        .AppendFormat("{0} \n", "Please use the following temporary password to login to the PathfinderRx application.")
                        .AppendLine()
                        .AppendFormat("Temporary password# {0}\n", newPassword)
                        .AppendLine()
                        .AppendFormat("{0} \n", "Once you log in using the above password, you will need to re-set the password to one of your own choice. Please make sure your password meets the following Alcon IT security requirements.")
                        .AppendLine()
                        .AppendFormat("{0} \n", "At least eight characters long")
                        .AppendFormat("{0} \n", "Composed of at least three (3) of the following four (4) categories:")
                        .AppendFormat("{0}{1} \n", "     ", "Upper Case English Alpha Characters A, B, C… ")
                        .AppendFormat("{0}{1} \n", "     ", "Lower Case English Alpha Characters a, b, c… ")
                        .AppendFormat("{0}{1} \n", "     ", "Numeric Characters 0, 1, 2, 3… ")
                        .AppendFormat("{0}{1} \n", "     ", "Special Characters !, $, #, %, @... ")
                        .AppendFormat("{0} \n", "Previous 4 passwords cannot be reused")
                        .AppendLine()
                        .Append("Please contact pfsupport@pinsonault.com if you require additional assistance.");

                    subject = string.Format(Resources.Resource.ForgotPassword_MessageSubject_Auto);
                    body = string.Format(Resources.Resource.ForgotPassword_MessageFormat_Auto, sb.ToString());

                    if (Pinsonault.Web.Support.SendEmail(Pinsonault.Web.Support.CustomerSupportEmail, user.Email, subject, body, false))
                    {
                        requestEntry.Visible = false;
                        requestSubmitted.Visible = true;
                        submitNotification.Visible = false;
                        submitNotification_auto.Visible = true;

                        submitSuccess = true;

                    }
                    else
                        emailError.IsValid = false;
                }
            }

            else
            {
                //Change from address to CustomerSupportEmail since new email server will not send emails from client email addresses
                //if (Pinsonault.Web.Support.SendEmail(user.Email, Pinsonault.Web.Support.CustomerSupportEmail, subject, body, false))
                if (Pinsonault.Web.Support.SendEmail(Pinsonault.Web.Support.CustomerSupportEmail, Pinsonault.Web.Support.CustomerSupportEmail, subject, body, false))
                {
                    requestEntry.Visible = false;
                    requestSubmitted.Visible = true;
                    submitNotification.Visible = true;
                    submitNotification_auto.Visible = false;

                    submitSuccess = true;
                }
                else
                    emailError.IsValid = false;

            }


            // sl 5/1/2012:  Forgot Password tracking: for all clients
            if (submitSuccess)
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Pathfinder"].ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("usp_ForgotPassword_Tracking", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Client_ID", user.ClientID);
                    cmd.Parameters.AddWithValue("@Email", user.Email);

                    cmd.ExecuteScalar();

                }
            }
         
        }
        else
        {
            invalidUser.IsValid = false;
        }
    }
}
