using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AdminWebPortal.Models
{
    public class ComputerModel
    {
        [Display(Name = "UserID")]
        public int User_ID { get; set; }

        [Display(Name = "Validation Record ID")]
        public int Computer_ID { get; set; }

        [Required]
        [Display(Name = "Model")]
        public string Computer_Model { get; set; }

        [Required]
        [Display(Name = "Product")]
        public string ComputerProduct { get; set; }

        [Display(Name = "Primary Model")]
        public bool IsComputerPrimaryModel { get; set; }

        [Display(Name = "Primary Product")]
        public bool IsComputerPrimaryProduct { get; set; }

        [Display(Name = "Build Number")]
        public string ComputerBuildNumber { get; set; }

        [Display(Name = "Caption")]
        public string ComputerCaption { get; set; }

        [Display(Name = "Computer Record Add Date")]
        public DateTime ComputerRecord_AddDate { get; set; }

        [Display(Name = "CSDVersion")]
        public string CSD_Version { get; set; }

        [Display(Name = "InstallDate")]
        public DateTime Install_Date { get; set; }

        [Display(Name = "Manufacturer")]
        public string ComputerManufacturer { get; set; }

        [Display(Name = "OSArchitecture")]
        public string OS_Architecture { get; set; }

        [Display(Name = "OSLanguage")]
        public string OS_Language { get; set; }

        [Display(Name = "OSProductSuite")]
        public string OS_ProductSuite { get; set; }

        [Display(Name = "OSType")]
        public string OS_Type { get; set; }

        [Display(Name = "ServicePackMajorVersion")]
        public string ServicePack_MajorVersion { get; set; }

        [Display(Name = "ServicePackMinorVersion")]
        public string ServicePack_MinorVersion { get; set; }

        [Display(Name = "SystemDirectory")]
        public string System_Directory { get; set; }


        [Display(Name = "Version")]
        public string ComputerVersion { get; set; }

        [Display(Name = "WindowsDirectory")]
        public string Windows_Directory { get; set; }


        [Display(Name = "MUILanguage")]
        public string MUI_Language { get; set; }

        [Display(Name = "Serial")]
        public string SerialNumber { get; set; }

        [Display(Name = "System Drive")]
        public string SystemDrive { get; set; }
    }
}