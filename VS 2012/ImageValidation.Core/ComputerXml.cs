using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ImageValidation.Core
{
    [Serializable, XmlRoot("ComputerInformation")]
    public class ComputerXml
    {
        [XmlElement("Computer")]
        public Computer Computer { get; set; }

        
        private Driver[] _Driver;

        public Driver[] Driver
        {
            get
            {
                return _Driver;
            }
            set
            {
                _Driver = value;
            }
        }


        private Applications[] _Applications;

        public Applications[] Applications
        {
            get
            {
                return _Applications;
            }
            set
            {
                _Applications = value;
            }
        }

       
        private HotFix[] _HotFix;

        public HotFix[] HotFix
        {
            get
            {
                return _HotFix;
            }
            set
            {
                _HotFix = value;
            }
        }

        private FileFolder[] _FileFolder;

        public FileFolder[] FileFolder
        {
            get
            {
                return _FileFolder;
            }
            set
            {
                _FileFolder = value;
            }
        }

        private RegistryGroup[] _RegistryGroup;

        public RegistryGroup[] RegistryGroup
        {
            get
            {
                return _RegistryGroup;
            }
            set
            {
                _RegistryGroup = value;
            }
        }

        private Registrys[] _Registrys;

        public Registrys[] Registrys
        {
            get
            {
                return _Registrys;
            }
            set
            {
                _Registrys = value;
            }
        }
    }



}
