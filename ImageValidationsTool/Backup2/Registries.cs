using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageValidation.Core
{
    public class Registries
    {
        public long? RegistryID
        {
            get;
            set;
        }

        public string Key
        {
            get;
            set;
        }
        public string Value
        {
            get;
            set;
        }

        public string ValueData
        {
            get;
            set;
        }

        public string DataType
        {
            get;
            set;
        }
    }
}
