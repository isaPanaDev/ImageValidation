using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Management;
using Microsoft.Win32;
using System.Xml;
using System.IO;
using System.Collections;
using System.Configuration;
using System.Web;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Reflection;
using System.Globalization;
using ImageValidation.Core;

namespace ImageValidation.Collection
{
    public class ClientXML
    {
        public void create()
        {

            XmlDocument docConfig = new XmlDocument();
            XmlNode xmlNode = docConfig.CreateNode(XmlNodeType.XmlDeclaration, "", "");
            XmlElement rootElement = docConfig.CreateElement("HEDDERS");
            docConfig.AppendChild(rootElement);

            for (int i = 10; i < 20; i++)
            {
                XmlElement hedder = docConfig.CreateElement("HEDDER");
                docConfig.DocumentElement.PrependChild(hedder);
                docConfig.ChildNodes.Item(0).AppendChild(hedder);
                // Create <installationid> Node
                XmlElement installationElement = docConfig.CreateElement("ID");
                XmlText installationIdText = docConfig.CreateTextNode(Convert.ToString(i));
                installationElement.AppendChild(installationIdText);
                hedder.PrependChild(installationElement);
                // Create <environment> Node
                XmlElement environmentElement = docConfig.CreateElement("NAME");
                XmlText environText = docConfig.CreateTextNode("ABC" + i);
                environmentElement.AppendChild(environText);

                hedder.PrependChild(environmentElement);

            }

            // Save xml document to the specified folder path.
            docConfig.Save("D:\\Sample.xml");
        }


        public void WriteXML(Computer comp)
        {

            //Book overview = new Book();
            //overview.title = "Serialization Overview";
            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(Computer));

            System.IO.StreamWriter file = new System.IO.StreamWriter(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\myXMLFile.xml");
            //System.IO.StreamWriter file = new System.IO.StreamWriter(Path.GetDirectoryName(@"XmlFiles\myXMLFile.xml"));
            //System.IO.StreamWriter file = new System.IO.StreamWriter(Assembly.GetExecutingAssembly().Location +  @"\SerializationOverview.xml");
            writer.Serialize(file, comp);
            file.Close();
        }
    }
}
