using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using AdminWebPortal.Repository;
using AdminWebPortal.Models;

namespace AdminWebPortal.Repository
{
    public class AdminWebPortalRepository
    {
         #region Variables

        private ImageValidationEntities entities = new ImageValidationEntities();

        private const string MissingRole = "Role does not exist";      
        private const string TooManyRole = "Role already exists";
        private const string AssignedRole = "Cannot delete a role with assigned users";

        private const string MissingUser = "User does not exist";
        private const string TooManyUser = "User already exists";

        #endregion

        #region Properties

        public int NumberOfUsers
        {
            get
            {
                return this.entities.Users.Count();
            }
        }

        public int NumberOfRoles
        {
            get
            {
                return this.entities.Permissions.Count();
            }
        }

        #endregion

        #region Constructors

        public AdminWebPortalRepository()
        {
            this.entities = new ImageValidationEntities();
        }

        #endregion

        #region Query Methods

        public Computer GetComputer(int id)
        {
            return entities.Computers.SingleOrDefault(x => x.ComputerID == id);
        }

        public Driver GetDriver(int id)
        {
            return entities.Drivers.SingleOrDefault(x => x.DriverID == id);
        }

        public HotFix GetHotFix(int id)
        {
            return entities.HotFixes.SingleOrDefault(x => x.HotFixID == id);
        }

        public RegistryGroup GetRegistryGroup(int id)
        {
            return entities.RegistryGroups.SingleOrDefault(x => x.RegistryGroupID == id);
        }

        public FileFolder GetFileFolder(int id)
        {
            return entities.FileFolders.SingleOrDefault(x => x.FileFolderID == id);
        }

        public Application GetApplication(int id)
        {
            return entities.Applications.SingleOrDefault(x => x.ApplicationID == id);
        }
        /// <summary>
        /// Getting all user from users entity.
        /// </summary>
        /// <returns></returns>
        public IQueryable<User> GetAllUsers()
        {
            return from user in entities.Users
                   orderby user.Username
                   select user;
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetUser(int id)
        {
            return entities.Users.SingleOrDefault(user => user.UserID == id);
        }

        /// <summary>
        /// Get User by UserName
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public User GetUser(string userName)
        {
             return entities.Users.SingleOrDefault(user => user.Username == userName);
        }

        /// <summary>
        /// Get Users for role by rolename.
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public IQueryable<User> GetUsersForRole(string roleName)
        {
            return GetUsersForRole(GetRole(roleName));
        }

        public IQueryable<User> GetUsersForRole(int id)
        {
            return GetUsersForRole(GetRole(id));
        }

        public IQueryable<User> GetUsersForRole(Permission role)
        {
            if (!RoleExists(role))
                throw new ArgumentException(MissingRole);

            return from user in entities.Users
                   where user.PermissionID == role.PermissionID
                   orderby user.Username
                   select user;
        }

        public IQueryable<Permission> GetAllUserRoles()
        {
            return from role in entities.Permissions
                   orderby role.RoleName
                   select role;
        }

        public Permission GetRole(int id)
        {
            return entities.Permissions.SingleOrDefault(role => role.PermissionID == id);
        }

        public Permission GetRole(string name)
        {
            return entities.Permissions.SingleOrDefault(role => role.RoleName == name);
        }

        public Permission GetRoleForUser(string userName)
        {
            return GetRoleForUser(GetUser(userName));
        }

        public Permission GetRoleForUser(int id)
        {
            return GetRoleForUser(GetUser(id));
        }

        public Permission GetRoleForUser(User user)
        {
            if (!UserExists(user))
                throw new ArgumentException(MissingUser);

            return user.Permission;
        }

        #endregion

        #region Insert/Delete

        /// <summary>
        /// This function for add user
        /// </summary>
        /// <param name="user"></param>
        private void AddUser(User user)
        {
            if (UserExists(user))
                throw new ArgumentException(TooManyUser);

            entities.Users.AddObject(user);
        }

        /// <summary>
        /// Create user with 6 parameters & checked null values of fields & checked exist user.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="roleName"></param>
        public void CreateUser(string username, string firstname, string lastname, string password, string email, string roleName)
        {
            Permission role = GetRole(roleName);

            if (string.IsNullOrEmpty(username.Trim()))
                throw new ArgumentException("The user name provided is invalid.");           
            if (string.IsNullOrEmpty(password.Trim()))
                throw new ArgumentException("The password provided is invalid.");
            if (string.IsNullOrEmpty(email.Trim()))
                throw new ArgumentException("The e-mail address provided is invalid.");
            if (!RoleExists(role))
                throw new ArgumentException("The role selected for this user does not exist! Contact an administrator!");
            if (this.entities.Users.Any(user => user.Username == username))
                throw new ArgumentException("Username already exists. Please enter a different user name.");

            User newUser = new User()
            {
                Username = username,
                FirstName = firstname,
                Password = HashPassword(password.Trim()),//FormsAuthentication.HashPasswordForStoringInConfigFile(password.Trim(), "md5"),
                Email = email,
                LastName=lastname,
                UserCreateDate=System.DateTime.UtcNow,
                PermissionID = role.PermissionID
            };

            try
            {
                AddUser(newUser);
            }
            catch (ArgumentException ae)
            {
                throw ae;
            }
            catch (Exception e)
            {
                throw new ArgumentException("The authentication provider returned an error. Please verify your entry and try again. " +
                    "If the problem persists, please contact your system administrator.");
            }

            // Immediately persist the user data
            Save();
        }

        public static string HashPassword(string password)
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes(password);
            data = System.Security.Cryptography.MD5.Create().ComputeHash(data);
            string hashString = Convert.ToBase64String(data);
            return hashString;
        }
        public void DeleteUser(User user)
        {
            if (!UserExists(user))
                throw new ArgumentException(MissingUser);

            entities.Users.DeleteObject(user);
        }

        public void DeleteUser(string userName)
        {
            DeleteUser(GetUser(userName));
        }

        public void AddRole(Permission role)
        {
            if (RoleExists(role))
                throw new ArgumentException(TooManyRole);

            entities.Permissions.AddObject(role);
        }

        public void AddRole(string roleName)
        {
            Permission role = new Permission()
            {
                RoleName = roleName
            };

            AddRole(role);
        }

        public void DeleteRole(Permission role)
        {
            if (!RoleExists(role))
                throw new ArgumentException(MissingRole);

            if (GetUsersForRole(role).Count() > 0)
                throw new ArgumentException(AssignedRole);

            entities.Permissions.DeleteObject(role);
        }

        public void DeleteRole(string roleName)
        {
            DeleteRole(GetRole(roleName));
        }

        #endregion

        #region Persistence

        public void Save()
        {
            entities.SaveChanges();
        }

        #endregion

        #region Helper Methods

        public bool UserExists(User user)
        {
            if (user == null)
                return false;

            return (entities.Users.SingleOrDefault(u => u.UserID == user.UserID || u.Username == user.Username) != null);
        }

        public bool RoleExists(Permission role)
        {
            if (role == null)
                return false;

            return (entities.Permissions.SingleOrDefault(r => r.PermissionID == role.PermissionID || r.RoleName == role.RoleName) != null);
        }

        #endregion
    }
}