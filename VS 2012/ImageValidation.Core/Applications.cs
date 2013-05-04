using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ImageValidation.Core
{
    //[Serializable ("HotFix")]
    [XmlRoot("Applications")]
    public class Applications
    {



        private string _DisplayName;
        public string DisplayName
        {
            get
            {
                return _DisplayName;
            }
            set
            {
                _DisplayName = value;
            }
        }



        private string _DisplayVersion;
        public string DisplayVersion
        {
            get
            {
                return _DisplayVersion;
            }
            set
            {
                _DisplayVersion = value;
            }
        }



        private string _Publisher;
        public string Publisher
        {
            get
            {
                return _Publisher;
            }
            set
            {
                _Publisher = value;
            }
        }



        private string _VersionMinor;
        public string VersionMinor
        {
            get
            {
                return _VersionMinor;
            }
            set
            {
                _VersionMinor = value;
            }
        }



        private string _VersionMajor;
        public string VersionMajor
        {
            get
            {
                return _VersionMajor;
            }
            set
            {
                _VersionMajor = value;
            }
        }




        private string _Version;
        public string Version
        {
            get
            {
                return _Version;
            }
            set
            {
                _Version = value;
            }
        }




        private string _HelpLink;
        public string HelpLink
        {
            get
            {
                return _HelpLink;
            }
            set
            {
                _HelpLink = value;
            }
        }



        private string _HelpTelephone;
        public string HelpTelephone
        {
            get
            {
                return _HelpTelephone;
            }
            set
            {
                _HelpTelephone = value;
            }
        }




        private string _InstallDate;
        public string InstallDate
        {
            get
            {
                return _InstallDate;
            }
            set
            {
                _InstallDate = value;
            }
        }



        private string _InstallLocation;
        public string InstallLocation
        {
            get
            {
                return _InstallLocation;
            }
            set
            {
                _InstallLocation = value;
            }
        }



        private string _InstallSource;
        public string InstallSource
        {
            get
            {
                return _InstallSource;
            }
            set
            {
                _InstallSource = value;
            }
        }



        private string _UrlInfoAbout;
        public string UrlInfoAbout
        {
            get
            {
                return _UrlInfoAbout;
            }
            set
            {
                _UrlInfoAbout = value;
            }
        }



        private string _URLUpdateInfo;
        public string URLUpdateInfo
        {
            get
            {
                return _URLUpdateInfo;
            }
            set
            {
                _URLUpdateInfo = value;
            }
        }



        private string _Comments;
        public string Comments
        {
            get
            {
                return _Comments;
            }
            set
            {
                _Comments = value;
            }
        }



        private string _AuthorizedCDFPrefix;
        public string AuthorizedCDFPrefix
        {
            get
            {
                return _AuthorizedCDFPrefix;
            }
            set
            {
                _AuthorizedCDFPrefix = value;
            }
        }



        private string _Contact;
        public string Contact
        {
            get
            {
                return _Contact;
            }
            set
            {
                _Contact = value;
            }
        }



        private string _EstimatedSize;
        public string EstimatedSize
        {
            get
            {
                return _EstimatedSize;
            }
            set
            {
                _EstimatedSize = value;
            }
        }



        private string _Language;
        public string Language
        {
            get
            {
                return _Language;
            }
            set
            {
                _Language = value;
            }
        }


        private string _ModifyPath;
        public string ModifyPath
        {
            get
            {
                return _ModifyPath;
            }
            set
            {
                _ModifyPath = value;
            }
        }




        private string _ReadMe;
        public string ReadMe
        {
            get
            {
                return _ReadMe;
            }
            set
            {
                _ReadMe = value;
            }
        }


        private string _UninstallString;
        public string UninstallString
        {
            get
            {
                return _UninstallString;
            }
            set
            {
                _UninstallString = value;
            }
        }




        private string _SettingsIdentifier;
        public string SettingsIdentifier
        {
            get
            {
                return _SettingsIdentifier;
            }
            set
            {
                _SettingsIdentifier = value;
            }
        }



        private string _IsRequired;
        public string IsRequired
        {
            get
            {
                return _IsRequired;
            }
            set
            {
                _IsRequired = value;
            }
        }




        private string _ApplicationUrl;
        public string ApplicationUrl
        {
            get
            {
                return _ApplicationUrl;
            }
            set
            {
                _ApplicationUrl = value;
            }
        }

        private int _IsCompared;
        public int IsCompared
        {
            get
            {
                return _IsCompared;
            }
            set
            {
                _IsCompared = value;
            }
        }
    }
}
