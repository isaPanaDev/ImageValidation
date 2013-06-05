using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageValidation.Core
{
    public class Utilities
    {
        public static string GetSessionId()
        {
            int length = 15;
            char[] chars = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            string session = string.Empty;
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                int x = random.Next(1, chars.Length);

                if (!session.Contains(chars.GetValue(x).ToString()))
                    session += chars.GetValue(x);
                else
                    i--;
            }
            return "s" + session;
        }
    }
}
