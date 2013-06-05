using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Text;

namespace ImageValidation.Service.Data
{
    [DataContract]
    public class User
    {
        [DataMember]
        public int UserID { get; set; }
        public int PermissionID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime UserCreateDate { get; set; }
        public string Email { get; set; }
        public int DeleteRecord { get; set; }
    }
}