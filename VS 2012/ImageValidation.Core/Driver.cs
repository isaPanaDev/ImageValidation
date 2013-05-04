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
