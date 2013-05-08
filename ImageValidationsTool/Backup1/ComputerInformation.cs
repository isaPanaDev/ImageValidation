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
    public class ComputerInformation
    {
        public Computer GetComputerInformation()
        {


            //[ComputerID] [int] IDENTITY(1,1) NOT NULL,
            //[UserID] [int] NULL,          
            //[Manufacturer2] [varchar](100) NULL,
            //[ComputerRecordAddDate] [datetime2](7) NULL,


            //ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM WIN32_OPERATINGSYSTEM");
            //ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPSignedDriver");           
            //ManagementObjectSearcher mosDisks = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");


            //Get basic computer information
            ManagementObjectSearcher mosCompSys = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
            Computer comp = new Computer();

            foreach (ManagementObject moComp in mosCompSys.Get())
            {
                comp.Model = moComp["Model"].ToString();
                comp.Manufacturer = moComp["Manufacturer"].ToString();
                //comp.InstallDate = Convert.ToDateTime(moComp["InstallDate"].ToString());

                //comp.InstallDate = (DateTime) moComp["InstallDate"];

                //DateTime dtInstallDate = new DateTime();
                //DateTime.TryParse(moComp["InstallDate"].ToString(), out dtInstallDate);
                //comp.InstallDate = dtInstallDate;


                comp.Caption = moComp["Caption"].ToString();

            }

            //Add baseboard information
            ManagementObjectSearcher mosBaseBoard = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
            foreach (ManagementBaseObject moBase in mosBaseBoard.Get())
            {
                comp.Version = moBase["Version"].ToString();
                //comp.Model = moBase["Model"].ToString();
                comp.Caption = moBase["Caption"].ToString();
                comp.Product = moBase["Product"].ToString();
                string SerialNumber = moBase["SerialNumber"].ToString();
            }


            //Get Operating system information
            ManagementObjectSearcher mosOperatingSys = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
            foreach (ManagementBaseObject mosOper in mosOperatingSys.Get())
            {
                //comp.CDSVersion = mosOper["CSDVersion"].ToString();
                comp.BuildNumber = mosOper["BuildNumber"].ToString();
                comp.ServicePackMajorVersion = mosOper["ServicePackMajorVersion"].ToString();
                comp.ServicePackMinorVersion = mosOper["ServicePackMinorVersion"].ToString();
                comp.SystemDirectory = mosOper["SystemDirectory"].ToString();
                comp.WindowsDirectory = mosOper["WindowsDirectory"].ToString();
                comp.OSArchitecture = mosOper["OSArchitecture"].ToString();
                comp.OSLanguage = mosOper["OSLanguage"].ToString();
                comp.OSProductSuite = mosOper["OSProductSuite"].ToString();
                comp.OSType = mosOper["OSType"].ToString();
                //comp.MUILanguages = mosOper["MUILanguages[]"].ToString();
                //comp.InstallDate = DateTime.Parse(mosOper["InstallDate[]"].ToString());
                //comp.CDSVersion = mosOper["CSDVersion"].ToString();
                comp.Caption = mosOper["Caption"].ToString();
                comp.ComputerRecordAddDate = DateTime.Now;

            }

            return comp;
        }        
    }
}
