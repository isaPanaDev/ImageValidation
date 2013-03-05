using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ImageValidation.Core
{
    //[Serializable]
    [XmlRoot("Driver")]
    public class Driver
    {
        //[XmlElement("DriverID")]
        //public long? DriverID
        //{
        //    get;
        //    set;
        //}

        //private string _DisplayName;
        //public string DisplayName
        //{
        //    get
        //    {
        //        return _DisplayName;
        //    }
        //    set
        //    {
        //        _DisplayName = value;
        //    }
        //}


        //[XmlElement("ComputerID")]
        //public int ComputerID
        //{
        //    get;
        //    set;
        //}




        //[XmlElement("CompactID")]
        //public string CompactID
        //{
        //    get;
        //    set;
        //}

        private string _CompactID;
        public string CompactID
        {
            get
            {
                return _CompactID;
            }
            set
            {
                _CompactID = value;
            }
        }

        //[XmlElement("Description")]
        //public string Description
        //{
        //    get;
        //    set;
        //}

        private string _Description;
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
            }
        }


        //[XmlElement("DeviceClass")]
        //public string DeviceClass
        //{
        //    get;
        //    set;
        //}

        private string _DeviceClass;
        public string DeviceClass
        {
            get
            {
                return _DeviceClass;
            }
            set
            {
                _DeviceClass = value;
            }
        }


        //[XmlElement("DeviceID")]
        //public string DeviceID
        //{
        //    get;
        //    set;
        //}

        private string _DeviceID;
        public string DeviceID
        {
            get
            {
                return _DeviceID;
            }
            set
            {
                _DeviceID = value;
            }
        }


        //[XmlElement("DeviceName")]
        //public string DeviceName
        //{
        //    get;
        //    set;
        //}


        private string _DeviceName;
        public string DeviceName
        {
            get
            {
                return _DeviceName;
            }
            set
            {
                _DeviceName = value;
            }
        }

        //[XmlElement("DriverDate")]
        //public string DriverDate
        //{
        //    get;
        //    set;
        //}

        private string _DriverDate;
        public string DriverDate
        {
            get
            {
                return _DriverDate;
            }
            set
            {
                _DriverDate = value;
            }
        }


        //[XmlElement("DriverProviderName")]
        //public string DriverProviderName
        //{
        //    get;
        //    set;
        //}

        private string _DriverProviderName;
        public string DriverProviderName
        {
            get
            {
                return _DriverProviderName;
            }
            set
            {
                _DriverProviderName = value;
            }
        }


        //[XmlElement("DriverVersion")]
        //public string DriverVersion
        //{
        //    get;
        //    set;
        //}

        private string _DriverVersion;
        public string DriverVersion
        {
            get
            {
                return _DriverVersion;
            }
            set
            {
                _DriverVersion = value;
            }
        }


        //[XmlElement("friendlyName")]
        //public string friendlyName
        //{
        //    get;
        //    set;
        //}


        private string _friendlyName;
        public string friendlyName
        {
            get
            {
                return _friendlyName;
            }
            set
            {
                _friendlyName = value;
            }
        }


        //[XmlElement("HardWareID")]
        //public string HardWareID
        //{
        //    get;
        //    set;
        //}


        private string _HardWareID;
        public string HardWareID
        {
            get
            {
                return _HardWareID;
            }
            set
            {
                _HardWareID = value;
            }
        }


        //[XmlElement("InfName")]
        //public string InfName
        //{
        //    get;
        //    set;
        //}

        private string _InfName;
        public string InfName
        {
            get
            {
                return _InfName;
            }
            set
            {
                _InfName = value;
            }
        }



        //[XmlElement("IsSigned")]
        //public string IsSigned
        //{
        //    get;
        //    set;
        //}

        private string _IsSigned;
        public string IsSigned
        {
            get
            {
                return _IsSigned;
            }
            set
            {
                _IsSigned = value;
            }
        }


        //[XmlElement("Manufacturer")]
        //public string Manufacturer
        //{
        //    get;
        //    set;
        //}


        private string _Manufacturer;
        public string Manufacturer
        {
            get
            {
                return _Manufacturer;
            }
            set
            {
                _Manufacturer = value;
            }
        }


        //[XmlElement("Name")]
        //public string Name
        //{
        //    get;
        //    set;
        //}

        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }


        //[XmlElement("PDO")]
        //public string PDO
        //{
        //    get;
        //    set;
        //}

        private string _PDO;
        public string PDO
        {
            get
            {
                return _PDO;
            }
            set
            {
                _PDO = value;
            }
        }


        //[XmlElement("Signer")]
        //public string Signer
        //{
        //    get;
        //    set;
        //}

        private string _Signer;
        public string Signer
        {
            get
            {
                return _Signer;
            }
            set
            {
                _Signer = value;
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


        //[XmlElement("httpUrl")]
        //public string httpUrl
        //{
        //    get;
        //    set;
        //}

        private string _httpUrl;
        public string httpUrl
        {
            get
            {
                return _httpUrl;
            }
            set
            {
                _httpUrl = value;
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
