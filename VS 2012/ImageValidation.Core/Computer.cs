using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ImageValidation.Core
{
    [Serializable, XmlRoot("Computer")]
    public class Computer
    {
        

        [XmlElement("UserID")]
        public int UserID
        {
            get;
            set;
        }
        [XmlElement("Model")]
        public string Model
        {
            get;
            set;
        }
        [XmlElement("Product")]
        public string Product
        {
            get;
            set;
        }
        [XmlElement("BuildNumber")]
        public string BuildNumber
        {
            get;
            set;
        }
        [XmlElement("Caption")]
        public string Caption
        {
            get;
            set;
        }
        [XmlElement("CDSVersion")]
        public string CDSVersion
        {
            get;
            set;
        }
        [XmlElement("InstallDate")]
        public string InstallDate
        {
            get;
            set;
        }
        [XmlElement("MUILanguages")]
        public string MUILanguages
        {
            get;
            set;
        }
        [XmlElement("OSArchitecture")]
        public string OSArchitecture
        {
            get;
            set;
        }
        [XmlElement("OSLanguage")]
        public string OSLanguage
        {
            get;
            set;
        }
        [XmlElement("OSProductSuite")]
        public string OSProductSuite
        {
            get;
            set;
        }
        [XmlElement("OSType")]
        public string OSType
        {
            get;
            set;
        }
        [XmlElement("ServicePackMajorVersion")]
        public string ServicePackMajorVersion
        {
            get;
            set;
        }
        [XmlElement("ServicePackMinorVersion")]
        public string ServicePackMinorVersion
        {
            get;
            set;
        }
        [XmlElement("SystemDirectory")]
        public string SystemDirectory
        {
            get;
            set;
        }
        [XmlElement("Version")]
        public string Version
        {
            get;
            set;
        }
        [XmlElement("WindowsDirectory")]
        public string WindowsDirectory
        {
            get;
            set;
        }
        [XmlElement("Manufacturer")]
        public string Manufacturer
        {
            get;
            set;
        }
        [XmlElement("Manufacturer2")]
        public string Manufacturer2
        {
            get;
            set;
        }
        [XmlElement("ComputerRecordAddDate")]
        public string ComputerRecordAddDate
        {
            get;
            set;
        }
        [XmlElement("IsPrimaryProduct")]
        public string IsPrimaryProduct
        {
            get;
            set;
        }
        [XmlElement("IsPrimaryModel")]
        public string IsPrimaryModel
        {
            get;
            set;
        }

        [XmlElement("SerialNumber")]
        public string SerialNumber
        {
            get;
            set;
        }
        [XmlElement("SystemDrive")]
        public string SystemDrive
        {
            get;
            set;
        }

    }
}
