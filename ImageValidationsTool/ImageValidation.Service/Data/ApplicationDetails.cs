using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Text;

namespace ImageValidation.Service.Data
{
    [DataContract]
    public class ApplicationDetails
    {
        [DataMember]
        public int ApplicationID { get; set; }
        public int ComputerID { get; set; }
        public string DisplayName { get; set; }
        public string DisplayVersion { get; set; }
        public string Publisher { get; set; }
        public string VersionMajor { get; set; }
        public string HelpLink { get; set; }
        public string HelpTelephone { get; set; }
        public string InstallDate { get; set; }
        public string InstallLocation { get; set; }
        public string InstallSource { get; set; }
        public string URLInfoAbout { get; set; }
        public string URLUpdateInfo { get; set; }
        public string Comments { get; set; }
        public string AuthorizedCDFPrefix { get; set; }
        public string Contact { get; set; }
        public string EstimatedSize { get; set; }
        public string Language { get; set; }
        public string ModifyPath { get; set; }
        public string ReadMe { get; set; }
        public string UnInstallString { get; set; }
        public string SettingIdentifier { get; set; }
        //public bit IsRequired { get; set; }
        public string ApplicationUrl { get; set; }
        
        
    }
}