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
using System.Xml.Xsl;

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


            //II method

//            string xslMarkup = @"<?xml version='1.0'?>
//            <xsl:stylesheet xmlns:xsl='http://www.w3.org/1999/XSL/Transform' version='1.0'>
//                <xsl:template match='/Parent'>
//                    <Root>
//                        <C1>
//                        <xsl:value-of select='Child1'/>
//                        </C1>
//                        <C2>
//                        <xsl:value-of select='Child2'/>
//                        </C2>
//                    </Root>
//                </xsl:template>
//            </xsl:stylesheet>";

//            XDocument xmlTree = new XDocument("Parent", (typeof(Computer)));

//            XDocument newTree = new XDocument();
//            using (XmlWriter writer = newTree.CreateWriter())
//            {
//                // Load the style sheet.
//                XslCompiledTransform xslt = new XslCompiledTransform();
//                xslt.Load(XmlReader.Create(new StringReader(xslMarkup)));

//                // Execute the transformation and output the results to a writer.
//                xslt.Transform(xmlTree.CreateReader(), writer);
//            }
//            Console.WriteLine(newTree);









            ////Method for deserializer
            //var serializer = new XmlSerializer(typeof(Users));
            //using (var reader = XmlReader.Create("test.xml"))
            //{
            //    Computer user = (Computer)serializer.Deserialize(reader);
            //}



            //XmlTextWriter xmlWriter = new XmlTextWriter(Server.MapPath("EmployeeDetails.xml"), Encoding.UTF8);
            //xmlWriter.WriteStartDocument();
            ////Create Parent element
            //xmlWriter.WriteStartElement("EmployeeDetails");
            ////Create Child elements
            //xmlWriter.WriteStartElement("Details");
            //xmlWriter.WriteElementString("Name", txtName.Text);
            //xmlWriter.WriteElementString("Department", txtDept.Text);
            //xmlWriter.WriteElementString("Location", txtLocation.Text);
            //xmlWriter.WriteEndElement();

            ////End writing top element and XML document
            //xmlWriter.WriteEndElement();
            //xmlWriter.WriteEndDocument();
            //xmlWriter.Close();
        }

        /// <summary>
        /// Write all computer information in xml format
        /// </summary>
        /// <param name="ObjComp">Computer</param>
        /// <param name="ObjDriverLst">Driver</param>
        /// <param name="ObjAppLst">Applications</param>
        /// <param name="ObjHotfixLst">List</param>
        public XmlDocument WriteXML(Computer ObjComp, List<Driver> ObjDriverLst, List<Applications> ObjAppLst, List<HotFix> ObjHotfixLst)
        {
            StringWriter stringWriter = new StringWriter();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
            xmlTextWriter.Formatting = Formatting.Indented;

            xmlTextWriter.WriteStartDocument();
            xmlTextWriter.WriteStartElement("ComputerInformation");

            xmlTextWriter.WriteStartElement("Computer");
            //xmlTextWriter.WriteElementString("ComputerID", ObjComp.ComputerID.ToString());
            xmlTextWriter.WriteElementString("UserID", ObjComp.UserID.ToString());
            xmlTextWriter.WriteElementString("Model", ObjComp.Model);
            xmlTextWriter.WriteElementString("Product", ObjComp.Product);
            xmlTextWriter.WriteElementString("BuildNumber", ObjComp.BuildNumber);
            xmlTextWriter.WriteElementString("Caption", ObjComp.Caption);
            xmlTextWriter.WriteElementString("CDSVersion", ObjComp.CDSVersion);
            xmlTextWriter.WriteElementString("InstallDate", ObjComp.InstallDate.ToString());
            xmlTextWriter.WriteElementString("OSArchitecture", ObjComp.OSArchitecture);
            xmlTextWriter.WriteElementString("OSLanguage", ObjComp.OSLanguage);
            xmlTextWriter.WriteElementString("OSProductSuite", ObjComp.OSProductSuite);
            xmlTextWriter.WriteElementString("OSType", ObjComp.OSType);
            xmlTextWriter.WriteElementString("ServicePackMajorVersion", ObjComp.ServicePackMajorVersion);
            xmlTextWriter.WriteElementString("ServicePackMinorVersion", ObjComp.ServicePackMinorVersion);
            xmlTextWriter.WriteElementString("SystemDirectory", ObjComp.SystemDirectory);
            xmlTextWriter.WriteElementString("Version", ObjComp.Version);
            xmlTextWriter.WriteElementString("WindowsDirectory", ObjComp.WindowsDirectory);
            xmlTextWriter.WriteElementString("Manufacturer", ObjComp.Manufacturer);
            xmlTextWriter.WriteElementString("Manufacturer2", ObjComp.Manufacturer2);
            xmlTextWriter.WriteElementString("ComputerRecordAddDate", ObjComp.ComputerRecordAddDate.ToString());
            xmlTextWriter.WriteElementString("IsPrimaryProduct", ObjComp.IsPrimaryProduct);
            xmlTextWriter.WriteElementString("IsPrimaryModel", ObjComp.IsPrimaryModel);
            xmlTextWriter.WriteEndElement();

            #region comments
            xmlTextWriter.WriteStartElement("Driver");
            foreach (var item in ObjDriverLst)
            {
                xmlTextWriter.WriteStartElement("Driver");
                //xmlTextWriter.WriteElementString("DriverID", item.DriverID.ToString());
                //xmlTextWriter.WriteElementString("ComputerID", item.ComputerID.ToString());
                xmlTextWriter.WriteElementString("CompactID", item.CompactID);
                xmlTextWriter.WriteElementString("Description", item.Description);
                xmlTextWriter.WriteElementString("DeviceClass", item.DeviceClass);
                xmlTextWriter.WriteElementString("DeviceID", item.DeviceID);
                xmlTextWriter.WriteElementString("DeviceName", item.DeviceName);
                xmlTextWriter.WriteElementString("DriverDate", item.DriverDate.ToString());
                xmlTextWriter.WriteElementString("DriverProviderName", item.DriverProviderName);
                xmlTextWriter.WriteElementString("DriverVersion", item.DriverVersion);
                xmlTextWriter.WriteElementString("friendlyName", item.friendlyName);
                xmlTextWriter.WriteElementString("HardWareID", item.HardWareID);
                xmlTextWriter.WriteElementString("InfName", item.InfName);
                xmlTextWriter.WriteElementString("IsSigned", item.IsSigned);
                xmlTextWriter.WriteElementString("Manufacturer", item.Manufacturer);
                xmlTextWriter.WriteElementString("Name", item.Name);
                xmlTextWriter.WriteElementString("PDO", item.PDO);
                xmlTextWriter.WriteElementString("Signer", item.Signer);
                xmlTextWriter.WriteElementString("IsRequired", item.IsRequired);
                xmlTextWriter.WriteElementString("httpUrl", item.httpUrl);
                xmlTextWriter.WriteEndElement();

            }

            xmlTextWriter.WriteEndElement();
            #endregion

            xmlTextWriter.WriteStartElement("Applications");
            foreach (var Appsitem in ObjAppLst)
            {
                xmlTextWriter.WriteStartElement("Applications");
                //xmlTextWriter.WriteElementString("ApplicationID", Appsitem.ApplicationID.ToString());
                //xmlTextWriter.WriteElementString("ComputerID", Appsitem.ComputerID.ToString());
                xmlTextWriter.WriteElementString("DisplayName", Appsitem.DisplayName);
                xmlTextWriter.WriteElementString("DisplayVersion", Appsitem.DisplayVersion);
                xmlTextWriter.WriteElementString("Publisher", Appsitem.Publisher);
                xmlTextWriter.WriteElementString("VersionMinor", Appsitem.VersionMinor);
                xmlTextWriter.WriteElementString("VersionMajor", Appsitem.VersionMajor);
                xmlTextWriter.WriteElementString("Version", Appsitem.Version.ToString());
                xmlTextWriter.WriteElementString("HelpLink", Appsitem.HelpLink);
                xmlTextWriter.WriteElementString("HelpTelephone", Appsitem.HelpTelephone);
                xmlTextWriter.WriteElementString("InstallDate", Appsitem.InstallDate.ToString());
                xmlTextWriter.WriteElementString("InstallSource", Appsitem.InstallSource);
                xmlTextWriter.WriteElementString("UrlInfoAbout", Appsitem.UrlInfoAbout);
                xmlTextWriter.WriteElementString("URLUpdateInfo", Appsitem.URLUpdateInfo);
                xmlTextWriter.WriteElementString("Comments", Appsitem.Comments);
                xmlTextWriter.WriteElementString("AuthorizedCDFPrefix", Appsitem.AuthorizedCDFPrefix);
                xmlTextWriter.WriteElementString("Contact", Appsitem.Contact);
                xmlTextWriter.WriteElementString("EstimatedSize", Appsitem.EstimatedSize);
                xmlTextWriter.WriteElementString("Language", Appsitem.Language);
                xmlTextWriter.WriteElementString("ModifyPath", Appsitem.ModifyPath);
                xmlTextWriter.WriteElementString("ReadMe", Appsitem.ReadMe);
                xmlTextWriter.WriteElementString("UninstallString", Appsitem.UninstallString);
                xmlTextWriter.WriteElementString("SettingsIdentifier", Appsitem.SettingsIdentifier);
                xmlTextWriter.WriteElementString("IsRequired", Appsitem.IsRequired);
                xmlTextWriter.WriteElementString("ApplicationUrl", Appsitem.ApplicationUrl);

                xmlTextWriter.WriteEndElement();
            }
            xmlTextWriter.WriteEndElement();

            


            xmlTextWriter.WriteStartElement("HotFix");
            foreach (var ObjHotfix in ObjHotfixLst)
            {
                xmlTextWriter.WriteStartElement("HotFix");
                //xmlTextWriter.WriteElementString("HotfixID", ObjHotfix.HotfixID.ToString());
                //xmlTextWriter.WriteElementString("ComputerID", ObjHotfix.ComputerID.ToString());
                xmlTextWriter.WriteElementString("CSName", ObjHotfix.CSName);
                xmlTextWriter.WriteElementString("Description", ObjHotfix.Description);
                xmlTextWriter.WriteElementString("HotFixIDs", ObjHotfix.HotFixIDs);
                xmlTextWriter.WriteElementString("InstallDate", ObjHotfix.InstallDate.ToString());
                xmlTextWriter.WriteElementString("InstalledBy", ObjHotfix.InstalledBy);
                xmlTextWriter.WriteElementString("IsRequired", ObjHotfix.IsRequired.ToString());
               xmlTextWriter.WriteEndElement();
            }

            xmlTextWriter.WriteEndElement();            
            
            xmlTextWriter.WriteEndElement();
            xmlTextWriter.WriteEndDocument();

            XmlDocument docSave = new XmlDocument();
            docSave.LoadXml(stringWriter.ToString());

            var xmlFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Model-uniqueid.xml");       
            docSave.Save(xmlFilePath);

            return docSave;
            
        }
    }
}
