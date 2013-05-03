using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AdminWebPortal.Models;
using System.Web.Security;
using AdminWebPortal.Repository;

namespace AdminWebPortal.MemberShipMember
{
    public class AdminWebPortalRoleProvider : RoleProvider
    {
        #region Variables

        private AdminWebPortalRepository repository;

        #endregion

        #region Constructors

        public AdminWebPortalRoleProvider()
        {
            this.repository = new AdminWebPortalRepository();
        }

        #endregion

        #region Implemented RoleProvider Methods

        public override bool IsUserInRole(string userName, string roleName)
        {
            User user = repository.GetUser(userName);
            Permission role = repository.GetRole(roleName);

            if (!repository.UserExists(user))
                return false;
            if (!repository.RoleExists(role))
                return false;

            return user.UserPermissions.SourceRoleName == role.RoleName;
        }

        public override string[] GetRolesForUser(string userName)
        {
            Permission role = this.repository.GetRoleForUser(userName);
            if (!this.repository.RoleExists(role))
                return new string[] { string.Empty };

            return new string[] { role.RoleName };
        }

        #endregion

        #region Not Implemented RoleProvider Methods

        #region Properties

        /// <summary>
        /// This property is not implemented.
        /// </summary>
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

        #endregion

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is not implemented.
        /// </summary>
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is not implemented.
        /// </summary>
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is not implemented.
        /// </summary>
        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}