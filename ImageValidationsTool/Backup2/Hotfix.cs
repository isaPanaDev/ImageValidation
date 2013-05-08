using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageValidation.Core
{
    public class Hotfix
    {
        public long? HotfixID
        {
            get;
            set;
        }

        public int ComputerID
        {
            get;
            set;
        }
        public string CSName
        {
            get;
            set;
        }
        public string Description
        {
            get;
            set;
        }
        public DateTime InstallDate
        {
            get;
            set;
        }
        public string InstalledBy
        {
            get;
            set;
        }

        public string IsRequired
        {
            get;
            set;
        }
        public string httpUrl
        {
            get;
            set;
        }
    }
}
