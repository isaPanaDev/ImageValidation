using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AdminWebPortal.Models
{
    public class DriverModel
    {
        public List<Driver> Driver { get; set; }


        [Display(Name = "Compat ID")]
        public string Compat_ID { get; set; }

        public int DriverID { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Descriptions { get; set; }

        [Required]
        [Display(Name = "Device Class")]
        public string Device_Class { get; set; }

        [Display(Name = "Device ID")]
        public string Device_ID { get; set; }

        [Display(Name = "Device Name")]
        public string Device_Name { get; set; }

        [Display(Name = "Driver Date")]
        public string Driver_Date { get; set; }

        [Required]
        [Display(Name = "DriverProvider Name")]
        public string Driver_ProviderName { get; set; }

        [Display(Name = "Driver Version")]
        public string Driver_Version { get; set; }

        [Display(Name = "FriendlyName")]
        public string Friendly_Name { get; set; }

        [Required]
        [Display(Name = "HardWare ID")]
        public string HardWare_ID { get; set; }

        [Display(Name = "Inf Name")]
        public string Inf_Name { get; set; }

        [Display(Name = "IsSigned")]
        public string IsSigned { get; set; }

        [Display(Name = "IsSigned")]
        public string Is_Signed { get; set; }

        [Display(Name = "Manufacturer")]
        public string Manufacturer_ { get; set; }

        [Display(Name = "Name")]
        public string Name_ { get; set; }

        [Display(Name = "PDO")]
        public string PDO_ { get; set; }

        [Display(Name = "Signer")]
        public string Signer_ { get; set; }

        [Display(Name = "Download Url")]
        public string DownloadLink { get; set; }

        [Display(Name = "IsRequired")]
        public bool Is_Required { get; set; }


        public int Application_ID { get; set; }
    }
}