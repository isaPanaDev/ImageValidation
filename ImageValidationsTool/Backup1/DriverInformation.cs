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
        public Driver GetDriverInfo()
        {
            Driver driver = new Driver();

            string ComputerName = "localhost";
            ManagementScope Scope;
            Scope = new ManagementScope(String.Format("\\\\{0}\\root\\CIMV2", ComputerName), null);

            Scope.Connect();
            ObjectQuery Query = new ObjectQuery("SELECT * FROM Win32_PnPSignedDriver");
            ManagementObjectSearcher Searcher = new ManagementObjectSearcher(Scope, Query);

            foreach (ManagementObject WmiObject in Searcher.Get())
            {
                //driver.CompactID = WmiObject["CompatID"].ToString();
                //driver.Description = WmiObject["Description"].ToString();
                driver.DeviceClass = WmiObject["DeviceClass"].ToString();
                driver.DeviceID = WmiObject["DeviceID"].ToString();
                driver.DeviceName = WmiObject["DeviceName"].ToString();
                //driver.DriverDate = (DateTime)moDriver["DriverDate"];



                //driver.DriverProviderName = WmiObject["DriverProviderName"].ToString();
                driver.DriverVersion = WmiObject["DriverVersion"].ToString();
                //driver.friendlyName = moDriver["FriendlyName"].ToString();
                driver.HardWareID = WmiObject["HardWareID"].ToString();
                driver.InfName = WmiObject["InfName"].ToString();
                driver.IsSigned = WmiObject["IsSigned"].ToString();
                driver.Manufacturer = WmiObject["Manufacturer"].ToString();
                //driver.Name = moDriver["Name"].ToString();
                driver.PDO = WmiObject["PDO"].ToString();
                driver.Signer = WmiObject["Signer"].ToString();

            }



            ////ManagementObjectSearcher mosCompSys = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
            //ManagementObjectSearcher mosDriverInfo = new ManagementObjectSearcher("SELECT * FROM Win32_PnPSignedDriver");

            //foreach (ManagementObject moDriver in mosDriverInfo.Get())
            //{

            //    //driver.CompactID = moDriver["CompatID"].ToString();
            //    driver.Description = moDriver["Description"].ToString();
            //    driver.DeviceClass = moDriver["DeviceClass"].ToString();
            //    driver.DeviceID = moDriver["DeviceID"].ToString();
            //    driver.DeviceName = moDriver["DeviceName"].ToString();
            //    //driver.DriverDate = (DateTime)moDriver["DriverDate"];
            //    driver.DriverProviderName = moDriver["DriverProviderName"].ToString();
            //    driver.DriverVersion = moDriver["DriverVersion"].ToString();
            //    //driver.friendlyName = moDriver["FriendlyName"].ToString();
            //    driver.HardWareID = moDriver["HardWareID"].ToString();
            //    driver.InfName = moDriver["InfName"].ToString();
            //    driver.IsSigned = moDriver["IsSigned"].ToString();
            //    driver.Manufacturer = moDriver["Manufacturer"].ToString();
            //    //driver.Name = moDriver["Name"].ToString();
            //    driver.PDO = moDriver["PDO"].ToString();
            //    driver.Signer = moDriver["Signer"].ToString();
            //    //driver.IsRequired = moDriver["CompatID"].ToString();
            //    //driver.httpUrl = moDriver["CompatID"].ToString();

            //    //string ClassGuid;
            //    //string CompatID;
            //    //string Description;
            //    //string DeviceClass;
            //    //string DeviceID;
            //    //string DeviceName;
            //    //string DevLoader;
            //    //string DriverDate;
            //    //string DriverName;
            //    //string DriverVersion;
            //    //string FriendlyName;
            //    //string HardWareID;
            //    //string InfName;
            //    //datetime InstallDate;
            //    //boolean IsSigned;
            //    //string Location;
            //    //string Manufacturer;
            //    //string Name;
            //    //string PDO;
            //    //string DriverProviderName;
            //    //string Signer;
            //    //boolean Started;
            //    //string StartMode;
            //    //string Status;
            //    //string SystemCreationClassName;
            //    //string SystemName;

            //}



            return driver;

        }
    }
}
