using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageValidation.Core
{
    public class Users
    {        
        public int UserID { get; set; }
        public int PermissionID { get; set; }
        public string RoleName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime LoginDate { get; set; }
        public string Email { get; set; }
        public int DeleteRecord { get; set; }
        public string SessionID { get; set; }
             
    }
}
