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
            xmlTextWriter.WriteElementString("SerialNumber", ObjComp.SerialNumber);
            xmlTextWriter.WriteElementString("SystemDrive", ObjComp.SystemDrive);
            xmlTextWriter.WriteEndElement();


            xmlTextWriter.WriteStartElement("Driver");
            foreach (var item in ObjDriverLst)
            {
                xmlTextWriter.WriteStartElement("Driver");

                xmlTextWriter.WriteElementString("CompactID", item.CompactID);
                xmlTextWriter.WriteElementString("Description", item.Description);
                xmlTextWriter.WriteElementString("DeviceClass", item.DeviceClass);
                xmlTextWriter.WriteElementString("ExecutablePath", item.DeviceClass);
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

            xmlTextWriter.WriteStartElement("Applications");
            foreach (var Appsitem in ObjAppLst)
            {
                xmlTextWriter.WriteStartElement("Applications");
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

            // var xmlFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Model-uniqueid.xml");
            var xmlFilePath = Path.Combine(@"C:\Windows\Temp", ObjComp.Model + ".xml");
            docSave.Save(xmlFilePath);

            return docSave;

        }
    }
}
