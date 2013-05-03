using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageValidation.Core
{
    public class Registrys
    {
        private string _Key;
        public string Key
        {
            get
            {
                return _Key;
            }
            set
            {
                _Key = value;
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

        private string _DataType;
        public string DataType
        {
            get
            {
                return _DataType;
            }
            set
            {
                _DataType = value;
            }
        }

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
    }
}
