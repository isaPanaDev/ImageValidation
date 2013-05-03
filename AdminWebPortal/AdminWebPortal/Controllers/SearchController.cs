using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminWebPortal.Models;
using AdminWebPortal.Repository;
using System.Web.Routing;

namespace AdminWebPortal.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        #region Initialize of context & repository/entity

        private ImageValidationEntities _context;
        private IRepository<Computer> _reporsitorycomputer;
        private IRepository<Product> _reporsitoryproduct;
        private IRepository<QuickSearch> _reporsitoryquicksearch;
        private IRepository<AdminWebPortal.Models.OperatingSystem> _reporsitoryoperatingsystem;

        protected override void Initialize(RequestContext requestContext)
        {

            _context = new ImageValidationEntities();
            _reporsitorycomputer = new Repository<Computer>(_context, false);
            _reporsitoryproduct = new Repository<Product>(_context, false);
            _reporsitoryquicksearch = new Repository<QuickSearch>(_context, false);
            _reporsitoryoperatingsystem = new Repository<AdminWebPortal.Models.OperatingSystem>(_context, false);

            base.Initialize(requestContext);
        }

        ComputerStatus computerstatus = new ComputerStatus();
        #endregion
       
        //
        // GET: /Search/

        #region Get Product list & Quick & Browse Serching And Results displays in JqGrid.

        /// <summary>
        /// Fill DropDownlist of Product, QuickSearch DDL and OS ect.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            SearchModel model = new SearchModel();
            model.ProductList = _reporsitoryproduct.GetAll().ToList();
            model.QuickSearchList = _reporsitoryquicksearch.GetAll().ToList();
            model.OperatingSystemList = _reporsitoryoperatingsystem.GetAll().ToList();
            computerstatus.ComputerIDFromSession = 0;
            return View(model);
        }

        /// <summary>
        /// This Post action use for Quick Search & redirect to search result page.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(SearchModel model)
        {
            if (model != null)
            {
                return RedirectToAction("SearchResult", new { sc = model.SelectQuickSearchItem.ToString(), qs = model.QuickSearch });
            }
            else
            {
                return RedirectToAction("Index", "Search");
            }
        }

        /// <summary>
        /// This Post action using for browse searching by Product & OS and redirect to search browse result page.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BrowseSearch(SearchModel model)
        {
            if (model != null)
            {
                return RedirectToAction("SearchBrowseResult", new { product = model.SelectProductItem.ToString(), caption = model.SelectOperatingSystemItem.ToString() });
            }
            else
            {
                return RedirectToAction("Index", "Search");
            }
        }

        /// <summary>
        /// Bind Quick search result.
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="qs"></param>
        /// <returns></returns>
        public ActionResult SearchResult(string sc, string qs)
        {
            SearchModel model = new SearchModel();

            if (sc != null && qs != null)
            {
                string CriteriaType = sc;
                string QuickSearch = qs;               
                if (CriteriaType.ToString() == "Model")
                {
                    model.Computer = _reporsitorycomputer.GetAll().Where(x => x.Model.ToLower().Contains(QuickSearch.ToLower())).ToList();
                }
                else if (CriteriaType.ToString() == "Product")
                {
                    model.Computer = _reporsitorycomputer.GetAll().Where(x => x.Product.ToLower().Contains(QuickSearch.ToLower())).ToList();
                }
                else if (CriteriaType.ToString() == "ComputerID")
                {
                    int num;
                    bool isNumeric = int.TryParse(QuickSearch, out num);
                    if (isNumeric)
                    {
                        model.Computer = _reporsitorycomputer.GetAll().Where(x => x.ComputerID == Convert.ToInt32(QuickSearch)).ToList();
                    }
                }
                else if (CriteriaType.ToString() == "OperatingSystem")
                {
                    model.Computer = _reporsitorycomputer.GetAll().Where(x => x.Caption.ToLower().Contains(QuickSearch.ToLower()) && x.OSArchitecture.ToLower().Contains(QuickSearch.ToLower())).ToList();
                }
                model.SearchCriteria = sc;
                model.QSearch = qs;
                return View(model);
            }
            else
            {
                model.Computer = _reporsitorycomputer.GetAll().ToList();
                model.SearchCriteria = sc;
                model.QSearch = qs;
                return View(model);
                //return RedirectToAction("Index", "Search");
            }
        }

        /// <summary>
        /// Bind Browse search result.
        /// </summary>
        /// <param name="product"></param>
        /// <param name="caption"></param>
        /// <returns></returns>
        public ActionResult SearchBrowseResult(string product, string caption)
        {
            if (product != null && caption != null)
            {
                SearchModel model = new SearchModel();
                string SearchProduct = product;
                string SearchCaption = caption;

                if (SearchCaption.ToString() == "Windows 7 x64")
                {
                    model.Computer = _reporsitorycomputer.GetAll().Where(x => x.Caption.Contains("Windows 7") && x.OSArchitecture.Contains("64") && x.Product.Contains(SearchProduct)).ToList();//
                }
                else if (SearchCaption.ToString() == "Windows 7 x86")
                {
                    model.Computer = _reporsitorycomputer.GetAll().Where(x => x.Caption.Contains("Windows 7") && x.OSArchitecture.Contains("32") && x.Product.Contains(SearchProduct)).ToList();//
                }
                else if (SearchCaption.ToString() == "Windows Vista x64")
                {
                    model.Computer = _reporsitorycomputer.GetAll().Where(x => x.Caption.Contains("Windows Vista") && x.OSArchitecture.Contains("64") && x.Product.Contains(SearchProduct)).ToList();
                }
                else if (SearchCaption.ToString() == "Windows Vista x86")
                {
                    model.Computer = _reporsitorycomputer.GetAll().Where(x => x.Caption.Contains("Windows Vista") && x.OSArchitecture.Contains("32") && x.Product.Contains(SearchProduct)).ToList();
                }
                else if (SearchCaption.ToString() == "Windows XP")
                {
                    model.Computer = _reporsitorycomputer.GetAll().Where(x => x.Caption.Contains("Windows XP") && x.Product.Contains(SearchProduct)).ToList();
                }
                else if (SearchCaption.ToString() == "Windows 8 x64")
                {
                    model.Computer = _reporsitorycomputer.GetAll().Where(x => x.Caption.Contains("Windows 8") && x.OSArchitecture.Contains("64") && x.Product.Contains(SearchProduct)).ToList();
                }
                else if (SearchCaption.ToString() == "Windows 8 x86")
                {
                    model.Computer = _reporsitorycomputer.GetAll().Where(x => x.Caption.Contains("Windows 8") && x.OSArchitecture.Contains("32") && x.Product.Contains(SearchProduct)).ToList();
                }

                model.SearchProduct = product;
                model.SearchCaption = caption;
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Search");
            }
        }

        #endregion
    }
}
