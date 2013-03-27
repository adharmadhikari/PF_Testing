using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Pinsonault.Web.Security
{
    /// <summary>
    /// Summary description for PathfinderMembershipUser
    /// </summary>
    public class PathfinderMembershipUser : MembershipUser
    {
        public PathfinderMembershipUser(string providerName, string name, object providerUserKey, string email, string passwordQuestion, string comment, bool isApproved, bool isLockedOut, DateTime creationDate, DateTime lastLoginDate, DateTime lastActivityDate, DateTime lastPasswordChangedDate, DateTime lastLockoutDate)
            : base(providerName, name, providerUserKey, email, passwordQuestion, comment, isApproved, isLockedOut, creationDate, lastLoginDate, lastActivityDate, lastPasswordChangedDate, lastLockoutDate)
        {
        }

        public int ClientID { get; set; }
        public string ClientKey { get; set; }
        public string ClientName { get; set; }
        public bool CustomTheme { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int UserID { get; set; }
        public Dictionary<string, PathfinderModel.ClientApplicationAccess> ClientApplicationAccess { get; set; }
        public string[] Roles { get; set; }
        public Dictionary<string, string> Alignments { get; set; }
        public Dictionary<string, string> Territories { get; set; }
        public bool HighSecurityClient { get; set; }
        public bool ClientHasCustomPlans { get; set; }
        public int PasswordResetDays { get; set; }
    }
}