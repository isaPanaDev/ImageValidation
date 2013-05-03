using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using AdminWebPortal.Models;
using AdminWebPortal.Repository;

namespace AdminWebPortal.MemberShipMember
{
    public class AdminWebPortalMembershipProvider : MembershipProvider
    {
        private AdminWebPortalRepository repository;

        public int MinPasswordLength
        {
            get
            {
                return 6;
            }
        }

		public override int MinRequiredPasswordLength
		{
			get
			{
				return MinPasswordLength;
			}
		}

        public AdminWebPortalMembershipProvider()
        {
            this.repository = new AdminWebPortalRepository();
        }

        public override bool ValidateUser(string username, string password)
        {
            if (string.IsNullOrEmpty(password.Trim()) || string.IsNullOrEmpty(username.Trim()))
                return false;

            string hash = HashPassword(password.Trim());//FormsAuthentication.HashPasswordForStoringInConfigFile(password.Trim(), "md5");

            return this.repository.GetAllUsers().Any(user => (user.Username == username.Trim()) && (user.Password == hash));
        }

        public void CreateUser(string username, string firstname,string lastname, string password, string email, string roleName)
        {
            this.repository.CreateUser(username, firstname,lastname, password, email, roleName);
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            if (!ValidateUser(username, oldPassword) || string.IsNullOrEmpty(newPassword.Trim()))
                return false;

            User user = repository.GetUser(username);
            string hash = HashPassword(newPassword.Trim());// FormsAuthentication.HashPasswordForStoringInConfigFile(newPassword.Trim(), "md5");

            user.Password = hash;
            repository.Save();

            return true;
        }


        public static string HashPassword(string password)
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes(password);
            data = System.Security.Cryptography.MD5.Create().ComputeHash(data);
            string hashString = Convert.ToBase64String(data);
            return hashString;
        }
        #region Not Implemented MembershipProvider Methods

        #region Properties

        /// <summary>
        /// This property is not implemented.
        /// </summary>
        public override string ApplicationName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

		/// <summary>
		/// This property is not implemented.
		/// </summary>
		public override int MaxInvalidPasswordAttempts
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// This property is not implemented.
		/// </summary>
		public override int MinRequiredNonAlphanumericCharacters
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// This property is not implemented.
		/// </summary>
		public override int PasswordAttemptWindow
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// This property is not implemented.
		/// </summary>
		public override MembershipPasswordFormat PasswordFormat
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// This property is not implemented.
		/// </summary>
		public override string PasswordStrengthRegularExpression
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// This property is not implemented.
		/// </summary>
		public override bool RequiresQuestionAndAnswer
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// This property is not implemented.
		/// </summary>
		public override bool RequiresUniqueEmail
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// This property is not implemented.
		/// </summary>
		public override bool EnablePasswordReset
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// This property is not implemented.
		/// </summary>
		public override bool EnablePasswordRetrieval
		{
			get { throw new NotImplementedException(); }
		}

        #endregion

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is not implemented.
        /// </summary>
        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}