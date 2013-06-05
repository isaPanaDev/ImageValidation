using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageValidation.Service.Data
{
    public static class Utility
    {
        public static string HashPassword(string password)
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes(password);
            data = System.Security.Cryptography.MD5.Create().ComputeHash(data);
            string hashString = Convert.ToBase64String(data);
            return hashString;
        }

    }
}