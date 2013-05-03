using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AdminWebPortal.Repository;
using System.Web.Routing;

namespace AdminWebPortal.Models
{
    public class SearchModel
    {
        //6BCE31E3582EBBAD4363C731F11347CF14E7FE61
        public List<Computer> Computer { get; set; }
        //Quick Search       
        [Display(Name = "Quick Search")]
        public string QuickSearch { get; set; }

        public string SearchCriteria { get; set; }
        public string QSearch { get; set; }
        public string SearchCaption { get; set; }
        public string SearchProduct { get; set; }


        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Required]
        public string SelectProductItem { set; get; }
        public IList<Product> ProductList { set; get; }

        [Display(Name = "Operating System")]
        public string OSName { get; set; }

        [Required]
        public string SelectOperatingSystemItem { set; get; }
        public IList<OperatingSystem> OperatingSystemList { set; get; }


        [Display(Name = "Quick Search")]
        public string QuickSearchName { get; set; }

        [Required]
        public string SelectQuickSearchItem { set; get; }
        public IList<QuickSearch> QuickSearchList { set; get; }

        public List<string> Caption { get; set; }


    }


}