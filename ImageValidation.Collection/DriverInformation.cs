using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Management;
using Microsoft.Win32;
using ImageValidation.Core;


namespace ImageValidation.Collection
{
    public class DriverInformation
    {

        #region Collect Driver information

        /// <summary>
        /// Get driver information through Win32_PnPSignedDriver
        /// </summary>
        /// <returns>Driver list</returns>
        public List<Driver> GetDriverInfo()
        {
            Driver driver = new Driver();

            List<Driver> ObjDriverLst = new List<Driver>();
            //string ComputerName = "SUDHIR-PC";
            string ComputerName = "localhost";
            ManagementScope Scope;
            Scope = new ManagementScope(String.Format("\\\\{0}\\root\\CIMV2", ComputerName), null);

            Scope.Connect();
            ObjectQuery Query = new ObjectQuery("SELECT * FROM Win32_PnPSignedDriver");
            ManagementObjectSearcher Searcher = new ManagementObjectSearcher(Scope, Query);

            foreach (ManagementObject WmiObject in Searcher.Get())
            {
                if (WmiObject["CompatID"] != null)
                {
                    driver.CompactID = WmiObject["CompatID"].ToString();
                }
                else
                {
                    driver.CompactID = string.Empty;
                }


                if (WmiObject["Description"] != null)
                {
                    driver.Description = WmiObject["Description"].ToString();
                }
                else
                {
                    driver.Description = string.Empty;
                }

                if (WmiObject["DeviceClass"] != null)
                {
                    driver.DeviceClass = WmiObject["DeviceClass"].ToString();
                }
                else
                {
                    driver.DeviceClass = string.Empty;
                }


                if (WmiObject["DeviceID"] != null)
                {
                    driver.DeviceID = WmiObject["DeviceID"].ToString();
                }
                else
                {
                    driver.DeviceID = string.Empty;
                }

                if (WmiObject["DeviceName"] != null)
                {
                    driver.DeviceName = WmiObject["DeviceName"].ToString();
                }
                else
                {
                    driver.DeviceName = string.Empty;
                }


                if (WmiObject["DriverDate"] != null)
                {
                    driver.DriverDate = WmiObject["DriverDate"].ToString(); 
                }
                else
                {
                    driver.DriverDate = string.Empty;
                }

                if (WmiObject["DriverProviderName"] != null)
                {
                    driver.DriverProviderName = WmiObject["DriverProviderName"].ToString();
                }
                else
                {
                    driver.DriverProviderName = string.Empty;
                }

                if (WmiObject["DriverVersion"] != null)
                {
                    driver.DriverVersion = WmiObject["DriverVersion"].ToString();
                }
                else
                {
                    driver.DriverVersion = string.Empty;
                }

                if (WmiObject["FriendlyName"] != null)
                {
                    driver.friendlyName = WmiObject["FriendlyName"].ToString();
                }
                else
                {
                    driver.friendlyName = string.Empty;
                }

                if (WmiObject["HardWareID"] != null)
                {
                    driver.HardWareID = WmiObject["HardWareID"].ToString();
                }
                else
                {
                    driver.HardWareID = string.Empty;
                }

                if (WmiObject["InfName"] != null)
                {
                    driver.InfName = WmiObject["InfName"].ToString();
                }
                else
                {
                    driver.InfName = string.Empty;
                }

                if (WmiObject["IsSigned"] != null)
                {
                    driver.IsSigned = WmiObject["IsSigned"].ToString();
                }
                else
                {
                    driver.IsSigned = string.Empty;
                }

                if (WmiObject["Manufacturer"] != null)
                {
                    driver.Manufacturer = WmiObject["Manufacturer"].ToString();
                }
                else
                {
                    driver.Manufacturer = string.Empty;
                }

                if (WmiObject["Name"] != null)
                {
                    driver.Name = WmiObject["Name"].ToString();
                }
                else
                {
                    driver.Name = string.Empty;
                }

                if (WmiObject["PDO"] != null)
                {
                    driver.PDO = WmiObject["PDO"].ToString();
                }
                else
                {
                    driver.PDO = string.Empty;
                }
                if (WmiObject["Signer"] != null)
                {
                    driver.Signer = WmiObject["Signer"].ToString();
                }
                else
                {
                    driver.Signer = string.Empty;
                }


                ObjDriverLst.Add(driver);
            }

            return ObjDriverLst;

        }

        #endregion
    }
}
