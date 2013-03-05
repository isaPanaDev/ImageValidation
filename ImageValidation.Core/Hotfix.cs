using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ImageValidation.Core
{
    [XmlRoot("HotFix")]
    public class HotFix
    {
        //[XmlElement("HotfixID")]
        //public long? HotfixID
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
        //[XmlElement("CSName")]
        //public string CSName
        //{
        //    get;
        //    set;
        //}
        private string _CSName;
        public string CSName
        {
            get
            {
                return _CSName;
            }
            set
            {
                _CSName = value;
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


        //[XmlElement("HotFixIDs")]
        //public string HotFixIDs
        //{
        //    get;
        //    set;
        //}

        private string _HotFixIDs;
        public string HotFixIDs
        {
            get
            {
                return _HotFixIDs;
            }
            set
            {
                _HotFixIDs = value;
            }
        }


        //[XmlElement("InstallDate")]
        //public string InstallDate
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


        //[XmlElement("InstalledBy")]
        //public string InstalledBy
        //{
        //    get;
        //    set;
        //}
        private string _InstalledBy;
        public string InstalledBy
        {
            get
            {
                return _InstalledBy;
            }
            set
            {
                _InstalledBy = value;
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
        //public string httpUrl
        //{
        //    get;
        //    set;
        //}
        private bool _IsCompared;
        public bool IsCompared
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
