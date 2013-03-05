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
        //[XmlElement("ApplicationID")]
        //public long? ApplicationID
        //{
        //    get;
        //    set;
        //}

        //[XmlElement("ComputerID")]
        //public int ComputerID
        //{
        //    get;
        //    set;
        //}

        //[XmlElement("DisplayName")]
        //public string DisplayName
        //{
        //    get;
        //    set;
        //}


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


        //[XmlElement("DisplayVersion")]
        //public string DisplayVersion
        //{
        //    get;
        //    set;
        //}



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


        //[XmlElement("Publisher")]
        //public string Publisher
        //{
        //    get;
        //    set;
        //}



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



        //[XmlElement("VersionMinor")]
        //public string VersionMinor
        //{
        //    get;
        //    set;
        //}



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


        //[XmlElement("VersionMajor")]
        //public string VersionMajor
        //{
        //    get;
        //    set;
        //}


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



        //[XmlElement("Version")]
        //public DateTime Version
        //{
        //    get;
        //    set;
        //}



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


        //[XmlElement("HelpLink")]
        //public string HelpLink
        //{
        //    get;
        //    set;
        //}


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



        //[XmlElement("HelpTelephone")]
        //public string HelpTelephone
        //{
        //    get;
        //    set;
        //}


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




        //[XmlElement("InstallDate")]
        //public DateTime InstallDate
        //{
        //    get;
        //    set;
        //}



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



        //[XmlElement("InstallLocation")]
        //public string InstallLocation
        //{
        //    get;
        //    set;
        //}


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


        //[XmlElement("InstallSource")]
        //public string InstallSource
        //{
        //    get;
        //    set;
        //}


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



        //[XmlElement("UrlInfoAbout")]
        //public string UrlInfoAbout
        //{
        //    get;
        //    set;
        //}



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


        //[XmlElement("URLUpdateInfo")]
        //public string URLUpdateInfo
        //{
        //    get;
        //    set;
        //}



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


        //[XmlElement("Comments")]
        //public string Comments
        //{
        //    get;
        //    set;
        //}


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



        //[XmlElement("AuthorizedCDFPrefix")]
        //public string AuthorizedCDFPrefix
        //{
        //    get;
        //    set;
        //}



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



        //[XmlElement("Contact")]
        //public string Contact
        //{
        //    get;
        //    set;
        //}



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


        //[XmlElement("EstimatedSize")]
        //public string EstimatedSize
        //{
        //    get;
        //    set;
        //}



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



        //[XmlElement("Language")]
        //public string Language
        //{
        //    get;
        //    set;
        //}


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


        //[XmlElement("ModifyPath")]
        //public string ModifyPath
        //{
        //    get;
        //    set;
        //}

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



        //[XmlElement("ReadMe")]
        //public string ReadMe
        //{
        //    get;
        //    set;
        //}


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


        //[XmlElement("UninstallString")]
        //public string UninstallString
        //{
        //    get;
        //    set;
        //}



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



        //[XmlElement("SettingsIdentifier")]
        //public string SettingsIdentifier
        //{
        //    get;
        //    set;
        //}


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


        //[XmlElement("IsRequired")]
        //public string IsRequired
        //{
        //    get;
        //    set;
        //}


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



        //[XmlElement("ApplicationUrl")]
        //public string ApplicationUrl
        //{
        //    get;
        //    set;
        //}



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
