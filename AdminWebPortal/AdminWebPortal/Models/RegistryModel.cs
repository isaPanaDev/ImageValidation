using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AdminWebPortal.Models
{
    public class RegistryModel
    {
        public List<RegistryGroup> RegistryGroup { get; set; }

        public List<Registry> Registry { get; set; }

        public int RegistryGroupID { get; set; }

        [Required]
        [Display(Name = "Add Note or Instructions to Registry File")]
        [AllowHtml]
        public string Note { get; set; }

    }
}