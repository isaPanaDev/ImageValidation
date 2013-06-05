using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ImageValidation.Core
{
    [Serializable]
    public class ApplicationsXml
    {
        [XmlElement("Application")]
        public Applications Application { get; set; }
    }
}
