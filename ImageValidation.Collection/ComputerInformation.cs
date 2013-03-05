﻿using System;
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

        #region Collect computer information 

        /// <summary>
        /// Get computer information throught Win32_OperatingSystem
        /// </summary>
        /// <returns>Computer</returns>
        public Computer GetComputerInformation()
        {

            Computer comp = new Computer();

            //Get Operating system information
            ManagementObjectSearcher mosOperatingSys = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
            foreach (ManagementBaseObject mosOper in mosOperatingSys.Get())
            {

                if (mosOper["BuildNumber"] != null)
                {
                    comp.BuildNumber = mosOper["BuildNumber"].ToString();
                }
                else
                {
                    comp.BuildNumber = string.Empty;
                }

                if (mosOper["Caption"] != null)
                {
                    comp.Caption = mosOper["Caption"].ToString();
                }
                else
                {
                    comp.Caption = string.Empty;
                }

                if (mosOper["CSDVersion"] != null)
                {
                    comp.CDSVersion = mosOper["CSDVersion"].ToString();
                }
                else
                {
                    comp.CDSVersion = string.Empty;
                }

                if (mosOper["InstallDate"] != null)
                {
                    comp.InstallDate = mosOper["InstallDate"].ToString();
                }
                else
                {
                    comp.InstallDate = string.Empty;
                }

                if (mosOper["MUILanguages"] != null)//mosOper["MUILanguages[]"].ToString();
                {
                    comp.MUILanguages = mosOper["MUILanguages"].ToString();//mosOper["MUILanguages[]"].ToString();
                }
                else
                {
                    comp.MUILanguages = string.Empty;
                }

                if (mosOper["OSArchitecture"] != null)
                {
                    comp.OSArchitecture = mosOper["OSArchitecture"].ToString();
                }
                else
                {
                    comp.OSArchitecture = string.Empty;
                }

                if (mosOper["OSLanguage"] != null)
                {
                    comp.OSLanguage = mosOper["OSLanguage"].ToString();
                }
                else
                {
                    comp.OSLanguage = string.Empty;
                }

                if (mosOper["OSProductSuite"] != null)
                {
                    comp.OSProductSuite = mosOper["OSProductSuite"].ToString();
                }
                else
                {
                    comp.OSProductSuite = string.Empty;
                }

                if (mosOper["OSType"] != null)
                {
                    comp.OSType = mosOper["OSType"].ToString();
                }
                else
                {
                    comp.OSType = string.Empty;
                }

                if (mosOper["ServicePackMajorVersion"] != null)
                {
                    comp.ServicePackMajorVersion = mosOper["ServicePackMajorVersion"].ToString();
                }
                else
                {
                    comp.ServicePackMajorVersion = string.Empty;
                }

                if (mosOper["ServicePackMinorVersion"] != null)
                {
                    comp.ServicePackMinorVersion = mosOper["ServicePackMinorVersion"].ToString();
                }
                else
                {
                    comp.ServicePackMinorVersion = string.Empty;
                }

                if (mosOper["SystemDirectory"] != null)
                {
                    comp.SystemDirectory = mosOper["SystemDirectory"].ToString();
                }
                else
                {
                    comp.SystemDirectory = string.Empty;
                }

                /*********************************************************
               Mention systemDriver field in .doc file but not mention in database file 
               //comp.SystemDrive = mosOper["SystemDrive"].ToString();
               *********************************************************/
                if (mosOper["Version"] != null)
                {
                    comp.Version = mosOper["Version"].ToString();
                }
                else
                {
                    comp.Version = string.Empty;
                }

                if (mosOper["WindowsDirectory"] != null)
                {
                    comp.WindowsDirectory = mosOper["WindowsDirectory"].ToString();
                }
                else
                {
                    comp.WindowsDirectory = string.Empty;
                }

            }


            //Get baseboard information
            ManagementObjectSearcher mosBaseBoard = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
            foreach (ManagementBaseObject moBase in mosBaseBoard.Get())
            {

                if (moBase["Product"] != null)
                {
                    comp.Product = moBase["Product"].ToString();
                }
                else
                {
                    comp.Product = string.Empty;
                }

                if (moBase["Manufacturer"] != null)
                {
                    comp.Manufacturer = moBase["Manufacturer"].ToString();
                }
                else
                {
                    comp.Manufacturer = string.Empty;
                }

            }


            //Get basic computer information
            ManagementObjectSearcher mosCompSys = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
            foreach (ManagementObject moComp in mosCompSys.Get())
            {

                if (moComp["Model"] != null)
                {
                    comp.Model = moComp["Model"].ToString();
                }
                else
                {
                    comp.Model = string.Empty;
                }

                if (moComp["Manufacturer"] != null)
                {
                    comp.Manufacturer2 = moComp["Manufacturer"].ToString();
                }
                else
                {
                    comp.Manufacturer2 = string.Empty;
                }

            }


            Users _usersDetails = ApplicationState.GetValue<Users>("UserDetails") as Users;

            comp.UserID = _usersDetails.UserID;
           // comp.UserID = 4;
            comp.ComputerRecordAddDate = DateTime.Now.ToString();
            comp.IsPrimaryProduct = "1";
            comp.IsPrimaryModel = "1";

            return comp;
        }


        #endregion
    }
}