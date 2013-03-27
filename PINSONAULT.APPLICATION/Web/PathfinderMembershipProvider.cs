using System;
using System.Web;
using System.Linq;
using System.Web.Security;
using System.Security.Cryptography;
using PathfinderModel;
using System.Collections.Generic;
using Pinsonault.Security;

namespace Pinsonault.Web.Security
{
    /// <summary>
    /// Summary description for PathfinderMembershipProvider
    /// </summary>
    public class PathfinderMembershipProvider : MembershipProvider
    {
        public PathfinderMembershipProvider()
        {
            
        }        
        private bool bIsChangePassword = false;

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            if ( string.IsNullOrEmpty(username) ) throw new ArgumentNullException("username");
            if ( string.IsNullOrEmpty(oldPassword) ) throw new ArgumentNullException("oldPassword");
            if ( string.IsNullOrEmpty(newPassword) ) throw new ArgumentNullException("newPassword");

            bIsChangePassword = true;
            //code to check for previous 4 (or MatchPreviousPasswords + (current pwd != old pwd)) passwords if client is high security client
            bool bpreviouspwdmatchfound = false;

            using (PathfinderEntities context = new PathfinderEntities())
            {
                 PathfinderMembershipUser memberuser = Membership.GetUser(username) as PathfinderMembershipUser;
                 if (memberuser != null && memberuser.HighSecurityClient)
                 {
                     var pwd = (from p in context.ClientUserPwdHistorySet where p.User_ID == memberuser.UserID orderby p.Pwd_History_Id descending select p.User_PWD).Take(MatchPreviousPasswords).ToList();
                     
                      string hashedPassword = Pinsonault.Security.Security.CreateHashedPassword(username, newPassword);
                      if (pwd.IndexOf(hashedPassword) > -1 || oldPassword == newPassword) 
                          bpreviouspwdmatchfound = true;
                 }           

                if (
                    newPassword.Length >= MinRequiredPasswordLength
                    && System.Text.RegularExpressions.Regex.IsMatch(newPassword, PasswordStrengthRegularExpression)
                    && ValidateUser(username, oldPassword)
                    && !bpreviouspwdmatchfound
                    )
                {                   
                    User user = context.UserSet.FirstOrDefault(u => u.Email == username);
                    if (user != null)
                    {
                        //change user's password and set the flag off for changing password 
                        user.User_PWD = Pinsonault.Security.Security.CreateHashedPassword(username, newPassword);
                        user.Force_Change_Pwd = false;
                        context.SaveChanges();

                        //if client is high security client or password reset days > 0 for the client, insert old password in history table
                        if (memberuser.HighSecurityClient || memberuser.PasswordResetDays > 0)
                        {                            
                            ClientUserPwdHistory passwordhistory = new ClientUserPwdHistory();
                            passwordhistory.Client_ID = memberuser.ClientID;
                            passwordhistory.User_ID = memberuser.UserID;
                            string hashedOldPassword = Pinsonault.Security.Security.CreateHashedPassword(username, oldPassword);
                            passwordhistory.User_PWD = hashedOldPassword;
                            passwordhistory.Pwd_Exp_DT = DateTime.UtcNow;
                            context.AddToClientUserPwdHistorySet(passwordhistory);
                            context.SaveChanges();                            
                        }                        
                        return true;
                    }
                    
                }
            }
                 
                
           
            return false;
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            using ( PathfinderModel.PathfinderEntities context = new PathfinderModel.PathfinderEntities() )
            {
                var q = from u in context.UserSet.Include("Client.ClientApplicationAccess.Section").Include("Territories").Include("Client.ClientApplicationAccess.Application").Include("UserRoles.Roles")
                        where u.Email == username
                        select u;

                PathfinderModel.User user = q.FirstOrDefault();

                if ( user != null )
                {
                    PathfinderMembershipUser muser = new PathfinderMembershipUser("PathfinderMembershipProvider", username, user.User_ID, user.Email, null, null, true, user.Is_Locked, user.Created_DT != null ? user.Created_DT.Value : DateTime.MinValue, DateTime.MinValue, DateTime.UtcNow, DateTime.MinValue, DateTime.MinValue);
                    muser.FirstName = user.User_F_Name;
                    muser.LastName = user.User_L_Name;
                    muser.ClientID = user.Client.Client_ID;
                    muser.UserID = user.User_ID;
                    muser.ClientKey = user.Client.Client_Key;
                    muser.ClientName = user.Client.Client_Name;
                    muser.CustomTheme = user.Client.Custom_Theme;                    

                    //Get User's Team Denied Apps
                    List<int> team_denied_apps = (from t in context.UserTeamApplicationAccessSet
                                                where t.User_ID == user.User_ID && t.Status == false
                                                select t.App_ID).ToList();

                    muser.ClientApplicationAccess = user.Client.ClientApplicationAccess.Where(e => !team_denied_apps.Contains(e.ApplicationID)).ToDictionary(ca => string.Format("{0}_{1}", ca.ApplicationID.ToString().PadLeft(3, '0'), ca.Section != null ? ca.Section.Name : "All"));
                    muser.Roles = user.UserRoles.Select(ur => ur.Roles.Role_Key).ToArray();
                    muser.Territories = user.Territories.Where(t => t.Client_ID == user.Client.Client_ID).ToDictionary(t => t.Territory_ID, t => t.Territory_Name);
                    muser.Alignments = context.GetUserTerritoryStates(user.User_ID).ToDictionary(a => a.ID, a => a.ID);
                    muser.HighSecurityClient = user.Client.Has_High_Security;
                    muser.ClientHasCustomPlans = user.Client.Has_Custom_Plans;
                    if (user.Client.Pwd_Reset_Days == null)
                        muser.PasswordResetDays = 0;
                    else
                        muser.PasswordResetDays = Convert.ToInt32(user.Client.Pwd_Reset_Days);
                    return muser;
                }
            }

            return null;

        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return Pinsonault.Security.Security.MinRequiredNonAlphanumericCharacters; }
        }

        public override int MinRequiredPasswordLength
        {
            get 
            {
                if (Pinsonault.Web.Session.ClientKey == "alcon")
                    return Pinsonault.Security.Security.MinPasswordLengthAlcon; 
                else
                    return Pinsonault.Security.Security.MinPasswordLength; 
            }
        }

        public int MatchPreviousPasswords
        {
            get
            {
                if (Pinsonault.Web.Session.ClientKey == "alcon")
                    return 3;
                else
                    return 1;
            }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get 
            {
                if (Pinsonault.Web.Session.ClientKey == "alcon")
                    return Pinsonault.Security.Security.PasswordStrengthRegularExpressionAlcon;
                else
                    return Pinsonault.Security.Security.PasswordStrengthRegularExpression; 
            }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            if ( string.IsNullOrEmpty(username) )
                throw new ArgumentNullException("username", "username argument is required.");

            string hashedPassword = Pinsonault.Security.Security.CreateHashedPassword(username, password);
            bool bChangePasswordRequest = false;
            if ( (HttpContext.Current.Session["UserID"] != null) && (bIsChangePassword) )
            {
                bChangePasswordRequest = true;
            }

            //get the IP address in Session
            string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if ( string.IsNullOrEmpty(ip) )
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            using ( PathfinderModel.PathfinderEntities context = new PathfinderModel.PathfinderEntities() )
            {
                PathfinderModel.User user = context.AuthenticateUser(username, hashedPassword, ip, bChangePasswordRequest).FirstOrDefault();
                if ( user != null )
                {
                    return true;
                }
                return false;
            }
        }
    }
}