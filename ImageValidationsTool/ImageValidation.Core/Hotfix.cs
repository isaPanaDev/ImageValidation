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
