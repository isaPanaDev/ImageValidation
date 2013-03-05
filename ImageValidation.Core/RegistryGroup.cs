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
}
