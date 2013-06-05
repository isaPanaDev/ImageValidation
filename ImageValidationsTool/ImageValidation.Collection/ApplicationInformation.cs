using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Management;
using ImageValidation.Core;
using Microsoft.Win32;

namespace ImageValidation.Collection
{
    public class ApplicationInformation
    {

        #region Collect Application information
        public List<Applications> GetApplicationInformation()
        {
            List<Applications> ObjAppsLst = new List<Applications>();

            RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\MICROSOFT\\WINDOWS\\CURRENTVERSION\\UNINSTALL");
            foreach (var v in key.GetSubKeyNames())
            {
                Console.WriteLine(v);

                RegistryKey productKey = key.OpenSubKey(v);
                if (productKey != null)
                {
                    foreach (var value in productKey.GetValueNames())
                    {
                        Applications apps = new Applications();

                        apps.DisplayName = Convert.ToString(productKey.GetValue("DisplayName"));
                        apps.DisplayVersion = Convert.ToString(productKey.GetValue("DisplayVersion"));
                        apps.Publisher = Convert.ToString(productKey.GetValue("Publisher"));
                        apps.VersionMinor = Convert.ToString(productKey.GetValue("VersionMinor"));
                        apps.VersionMajor = Convert.ToString(productKey.GetValue("VersionMajor"));
                        apps.Version = DateTime.Now.ToString(); //Convert.ToString(productKey.GetValue("Version"));
                        apps.HelpLink = Convert.ToString(productKey.GetValue("HelpLink"));
                        apps.HelpTelephone = Convert.ToString(productKey.GetValue("HelpTelephone"));
                        apps.InstallDate = DateTime.Now.ToString(); //Convert.ToString(productKey.GetValue("InstallDate"));
                        apps.InstallLocation = Convert.ToString(productKey.GetValue("InstallLocation"));
                        apps.InstallSource = Convert.ToString(productKey.GetValue("InstallSource"));
                        apps.UrlInfoAbout = Convert.ToString(productKey.GetValue("URLInfoAbout"));
                        apps.URLUpdateInfo = Convert.ToString(productKey.GetValue("URLUpdateInfo"));
                        apps.Comments = Convert.ToString(productKey.GetValue("Comments"));
                        apps.AuthorizedCDFPrefix = Convert.ToString(productKey.GetValue("AuthorizedCDFPrefix"));
                        apps.Contact = Convert.ToString(productKey.GetValue("Contact"));
                        apps.EstimatedSize = Convert.ToString(productKey.GetValue("EstimatedSize"));
                        apps.Language = Convert.ToString(productKey.GetValue("Language"));
                        apps.ModifyPath = Convert.ToString(productKey.GetValue("ModifyPath"));
                        apps.ReadMe = Convert.ToString(productKey.GetValue("Readme"));
                        apps.UninstallString = Convert.ToString(productKey.GetValue("UninstallString"));
                        apps.SettingsIdentifier = Convert.ToString(productKey.GetValue("SettingsIdentifier"));
                        apps.ApplicationUrl = Convert.ToString(productKey.GetValue("ApplicationUrl"));

                        //temporary adding value in these fields
                        apps.IsRequired = "0";

                        if (apps.DisplayName != "" && apps.DisplayVersion != "")
                        {
                            ObjAppsLst.Add(apps);
                        }


                    }
                }
            }

            return ObjAppsLst;

        }

        /// <summary>
        /// Get application information
        /// </summary>
        /// <returns>Application List</returns>    
        public List<Applications> GetApplicationInformation32(string p_name)
        {
            List<Applications> ObjAppsLst = new List<Applications>();

            string displayName;
            RegistryKey key;

            // search in: CurrentUser
            key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
            if (key != null)
            {
                foreach (String keyName in key.GetSubKeyNames())
                {
                    RegistryKey subkey = key.OpenSubKey(keyName);
                    displayName = subkey.GetValue("DisplayName") as string;


                    if (p_name.Equals(displayName, StringComparison.OrdinalIgnoreCase) == true)
                    {

                    }
                }
            }

            // search in: LocalMachine_32
            key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
            if (key != null)
            {
                foreach (String keyName in key.GetSubKeyNames())
                {
                    Applications apps = new Applications();
                    RegistryKey subkey = key.OpenSubKey(keyName);
                    displayName = subkey.GetValue("DisplayName") as string;
                    apps.DisplayName = Convert.ToString(subkey.GetValue("DisplayName"));
                    apps.DisplayVersion = Convert.ToString(subkey.GetValue("DisplayVersion"));
                    apps.Publisher = Convert.ToString(subkey.GetValue("Publisher"));
                    apps.VersionMinor = Convert.ToString(subkey.GetValue("VersionMinor"));
                    apps.VersionMajor = Convert.ToString(subkey.GetValue("VersionMajor"));
                    apps.Version = Convert.ToString(subkey.GetValue("Version"));
                    apps.HelpLink = Convert.ToString(subkey.GetValue("HelpLink"));
                    apps.HelpTelephone = Convert.ToString(subkey.GetValue("HelpTelephone"));
                    apps.InstallDate = Convert.ToString(subkey.GetValue("InstallDate"));
                    apps.InstallLocation = Convert.ToString(subkey.GetValue("InstallLocation"));
                    apps.InstallSource = Convert.ToString(subkey.GetValue("InstallSource"));
                    apps.UrlInfoAbout = Convert.ToString(subkey.GetValue("URLInfoAbout"));
                    apps.URLUpdateInfo = Convert.ToString(subkey.GetValue("URLUpdateInfo"));
                    apps.Comments = Convert.ToString(subkey.GetValue("Comments"));
                    apps.AuthorizedCDFPrefix = Convert.ToString(subkey.GetValue("AuthorizedCDFPrefix"));
                    apps.Contact = Convert.ToString(subkey.GetValue("Contact"));
                    apps.EstimatedSize = Convert.ToString(subkey.GetValue("EstimatedSize"));
                    apps.Language = Convert.ToString(subkey.GetValue("Language"));
                    apps.ModifyPath = Convert.ToString(subkey.GetValue("ModifyPath"));
                    apps.ReadMe = Convert.ToString(subkey.GetValue("Readme"));
                    apps.UninstallString = Convert.ToString(subkey.GetValue("UninstallString"));
                    apps.SettingsIdentifier = Convert.ToString(subkey.GetValue("SettingsIdentifier"));
                    apps.ApplicationUrl = Convert.ToString(subkey.GetValue("ApplicationUrl"));


                    //temporary adding value in these fields
                    apps.IsRequired = "1";

                    if (apps.DisplayName != "" && apps.DisplayVersion != "")
                    {
                        ObjAppsLst.Add(apps);
                    }

                    if (p_name.Equals(displayName, StringComparison.OrdinalIgnoreCase) == true)
                    {

                    }
                }

            }
            return ObjAppsLst;

        }

        /// <summary>
        /// Get application information
        /// </summary>
        /// <returns>Application List</returns>  
        public List<Applications> GetApplicationInformation64(string p_name)
        {
            List<Applications> ObjAppsLst = new List<Applications>();


            string displayName;
            RegistryKey key;

            // search in: CurrentUser
            key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
            if (key != null)
            {
                foreach (String keyName in key.GetSubKeyNames())
                {
                    RegistryKey subkey = key.OpenSubKey(keyName);
                    displayName = subkey.GetValue("DisplayName") as string;
                    if (p_name.Equals(displayName, StringComparison.OrdinalIgnoreCase) == true)
                    {

                    }
                }
            }

            // search in: LocalMachine_64
            key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall");
            if (key != null)
            {
                foreach (String keyName in key.GetSubKeyNames())
                {
                    Applications apps = new Applications();
                    RegistryKey subkey = key.OpenSubKey(keyName);
                    displayName = subkey.GetValue("DisplayName") as string;

                    apps.DisplayName = Convert.ToString(subkey.GetValue("DisplayName"));
                    apps.DisplayVersion = Convert.ToString(subkey.GetValue("DisplayVersion"));
                    apps.Publisher = Convert.ToString(subkey.GetValue("Publisher"));
                    apps.VersionMinor = Convert.ToString(subkey.GetValue("VersionMinor"));
                    apps.VersionMajor = Convert.ToString(subkey.GetValue("VersionMajor"));
                    apps.Version = Convert.ToString(subkey.GetValue("Version"));
                    apps.HelpLink = Convert.ToString(subkey.GetValue("HelpLink"));
                    apps.HelpTelephone = Convert.ToString(subkey.GetValue("HelpTelephone"));
                    apps.InstallDate = Convert.ToString(subkey.GetValue("InstallDate"));
                    apps.InstallLocation = Convert.ToString(subkey.GetValue("InstallLocation"));
                    apps.InstallSource = Convert.ToString(subkey.GetValue("InstallSource"));
                    apps.UrlInfoAbout = Convert.ToString(subkey.GetValue("URLInfoAbout"));
                    apps.URLUpdateInfo = Convert.ToString(subkey.GetValue("URLUpdateInfo"));
                    apps.Comments = Convert.ToString(subkey.GetValue("Comments"));
                    apps.AuthorizedCDFPrefix = Convert.ToString(subkey.GetValue("AuthorizedCDFPrefix"));
                    apps.Contact = Convert.ToString(subkey.GetValue("Contact"));
                    apps.EstimatedSize = Convert.ToString(subkey.GetValue("EstimatedSize"));
                    apps.Language = Convert.ToString(subkey.GetValue("Language"));
                    apps.ModifyPath = Convert.ToString(subkey.GetValue("ModifyPath"));
                    apps.ReadMe = Convert.ToString(subkey.GetValue("Readme"));
                    apps.UninstallString = Convert.ToString(subkey.GetValue("UninstallString"));
                    apps.SettingsIdentifier = Convert.ToString(subkey.GetValue("SettingsIdentifier"));
                    apps.ApplicationUrl = Convert.ToString(subkey.GetValue("ApplicationUrl"));

                    //temporary adding value in these fields
                    apps.IsRequired = "0";

                    if (apps.DisplayName != "" && apps.DisplayVersion != "")
                    {
                        ObjAppsLst.Add(apps);
                    }

                    if (p_name.Equals(displayName, StringComparison.OrdinalIgnoreCase) == true)
                    {

                    }
                }
            }
            return ObjAppsLst;

        }

        #endregion
    }
}


