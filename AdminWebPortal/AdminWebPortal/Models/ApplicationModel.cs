using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AdminWebPortal.Models
{
    public class ApplicationModel
    {
        public List<Application> Application { get; set; }

        [Required]
        [Display(Name = "Display Name")]
        public string Display_Name { get; set; }

        [Required]
        [Display(Name = "Display Version")]
        public string Display_Version { get; set; }

        public int Application_ID { get; set; }

        [Display(Name = "Computer Id")]
        public string Computer_Id { get; set; }

        [Display(Name = "Version")]
        public string Version_ { get; set; }

        [Display(Name = "Publisher")]
        public string Publisher_ { get; set; }


        [Display(Name = "Version Minor")]
        public string Version_Minor { get; set; }

        [Display(Name = "Version Major")]
        public string Version_Major { get; set; }

        [Display(Name = "Help Link")]
        public string Help_Link { get; set; }

        [Display(Name = "Help Telephone")]
        public string Help_Telephone { get; set; }

        [Display(Name = "Install Date")]
        public string Install_Date { get; set; }

        [Display(Name = "Install Location")]
        public string Install_Location { get; set; }

        [Display(Name = "Install Source")]
        public string Install_Source { get; set; }

        [Display(Name = "URL Info About")]
        public string URL_InfoAbout { get; set; }

        [Display(Name = "URL Update Info")]
        public string URL_UpdateInfo { get; set; }

        [Display(Name = "Comments")]
        public string Comment { get; set; }

        [Display(Name = "Authorized CDFPrefix")]
        public string Authorized_CDFPrefix { get; set; }

        [Display(Name = "Contact")]
        public string Contact_ { get; set; }

        [Display(Name = "Estimated Size")]
        public string Estimated_Size { get; set; }

        [Display(Name = "Language")]
        public string Language_ { get; set; }

        [Display(Name = "Modify Path")]
        public string Modify_Path { get; set; }

        [Display(Name = "Read me")]
        public string Read_me { get; set; }

        [Display(Name = "Uninstall String")]
        public string Uninstall_String { get; set; }

        [Display(Name = "Settings Identifier")]
        public string Settings_Identifier { get; set; }

        [Display(Name = "Is Required")]
        public bool Is_Required { get; set; }

        [Display(Name = "Download Link")]
        public string Download_Link { get; set; }

























    }
}