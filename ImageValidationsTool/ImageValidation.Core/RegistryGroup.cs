using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageValidation.Core
{
    public class RegistryGroup
    {
        private string _RegistryGroupID;
        public string RegistryGroupID
        {
            get
            {
                return _RegistryGroupID;
            }
            set
            {
                _RegistryGroupID = value;
            }
        }

        private string _FileName;
        public string FileName
        {
            get
            {
                return _FileName;
            }
            set
            {
                _FileName = value;
            }
        }

        private string _Note;
        public string Note
        {
            get
            {
                return _Note;
            }
            set
            {
                _Note = value;
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


    public class RegistryGroupData
    {
        private string _RegistryGroupID;
        public string RegistryGroupID
        {
            get
            {
                return _RegistryGroupID;
            }
            set
            {
                _RegistryGroupID = value;
            }
        }

        private string _FileName;
        public string FileName
        {
            get
            {
                return _FileName;
            }
            set
            {
                _FileName = value;
            }
        }

        private string _Note;
        public string Note
        {
            get
            {
                return _Note;
            }
            set
            {
                _Note = value;
            }
        }

        private string _Type;
        public string Type
        {
            get
            {
                return _Type;
            }
            set
            {
                _Type = value;
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

        private string _Value;
        public string Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
            }
        }

        private string _ValueData;
        public string ValueData
        {
            get
            {
                return _ValueData;
            }
            set
            {
                _ValueData = value;
            }
        }

        private string _RegKey;
        public string RegKey
        {
            get
            {
                return _RegKey;
            }
            set
            {
                _RegKey = value;
            }
        }

    }
}
