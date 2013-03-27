using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using PathfinderModel;

namespace Pinsonault.Web.Security
{
    /// <summary>
    /// Summary description for PathfinderRoleProvider
    /// </summary>
    public class PathfinderRoleProvider : RoleProvider
    {
        public PathfinderRoleProvider()
        {
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

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

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            List<string> roles = new List<string>();

            PathfinderMembershipUser user = Membership.GetUser(username) as PathfinderMembershipUser;
            if ( user != null )
            {
                int appid = 0;

                //Client Key as role
                roles.Add(user.ClientKey);

                //Application/Channels as roles
                foreach ( ClientApplicationAccess ca in user.ClientApplicationAccess.Values.OrderBy(i => i.ApplicationID) )
                {
                    if ( appid != ca.ApplicationID )
                        roles.Add(ca.Application.App_Key);

                    if ( ca.Section != null )
                        roles.Add(string.Format("{0}_{1}", ca.Application.App_Key, ca.Section.Section_Key));

                    appid = ca.ApplicationID;
                }
                
                //all user defined roles 
                roles.AddRange(user.Roles);
            }
            return roles.ToArray();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}