using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminWebPortal.Models;
using AdminWebPortal.Repository;
using System.Web.Routing;
using System.Collections;
using System.Reflection;
using System.Data;
using System.Management;

namespace AdminWebPortal.Controllers
{
    public class ComputerController : Controller
    {
        #region Initialize of context & repository/entity

        private ImageValidationEntities _context;
        private IRepository<Computer> _reporsitorycomputer;
        public AdminWebPortalRepository _adminwebportalrepository;

        public ComputerStatus computerstatus = new ComputerStatus();
        protected override void Initialize(RequestContext requestContext)
        {
            _context = new ImageValidationEntities();
            _reporsitorycomputer = new Repository<Computer>(_context, false);
            _adminwebportalrepository = new AdminWebPortalRepository();
            base.Initialize(requestContext);
        }

        #endregion


        //
        // GET: /Computer/

        #region Displays computer information with operating system information & Model, Product, IsPrimaryProduct, IsPrimaryModel these field are editable.

        /// <summary>
        /// Displays computer information with operating system information & Model, Product, IsPrimaryProduct, IsPrimaryModel these field are editable.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index(int ID)
        {
            if (ID > 0)
            {
                computerstatus.ComputerIDFromSession = ID;

                ComputerModel model = new ComputerModel();
            
                Computer computer = _reporsitorycomputer.Single(x => x.ComputerID == ID);
                model.Windows_Directory = computer.WindowsDirectory;
                model.ComputerVersion = computer.Version;
                model.User_ID = (int)computer.UserID;
                model.System_Directory = computer.SystemDirectory;
                model.ServicePack_MinorVersion = computer.ServicePackMinorVersion;
                model.ServicePack_MajorVersion = computer.ServicePackMajorVersion;
                model.ComputerProduct = computer.Product;
                model.OS_Type = computer.OSType;
                model.OS_ProductSuite = computer.OSProductSuite;
                model.OS_Language = computer.OSLanguage;
                model.OS_Architecture = computer.OSArchitecture;
                model.MUI_Language = computer.MUILanguages;
                model.Computer_Model = computer.Model;
                model.ComputerProduct = computer.Product;
                model.ComputerManufacturer = computer.Manufacturer + computer.Manufacturer2;
                model.IsComputerPrimaryProduct = (bool)computer.IsPrimaryProduct;
                model.IsComputerPrimaryModel = (bool)computer.IsPrimaryModel;
                model.SerialNumber = computer.SerialNumber;
                model.SystemDrive = computer.SystemDrive;

                model.Install_Date = ManagementDateTimeConverter.ToDateTime(computer.InstallDate);
               
                model.ComputerRecord_AddDate = Convert.ToDateTime(computer.ComputerRecordAddDate);
                model.Computer_ID = computer.ComputerID;
                model.ComputerCaption = computer.Caption;
                model.ComputerBuildNumber = computer.BuildNumber;


                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Search");
            }
        }

        /// <summary>
        /// Update computer informations eg. Model, Product etc.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Index(ComputerModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Computer computer = _adminwebportalrepository.GetComputer(model.Computer_ID);
                    computer.ComputerID = model.Computer_ID;
                    computer.Model = model.Computer_Model;
                    computer.Product = model.ComputerProduct;
                    computer.IsPrimaryModel = model.IsComputerPrimaryModel;
                    computer.IsPrimaryProduct = model.IsComputerPrimaryProduct;
                    _adminwebportalrepository.Save();

                    ModelState.AddModelError("", "Updated Succussfully.");
                }
                catch (ArgumentException ae)
                {
                    ModelState.AddModelError("", ae.Message);
                }
            }

            return View(model);
        }

        #endregion

        #region Delete Computer Record By ComputerID

        /// <summary>
        /// Delete Computer Record By ComputerID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteComputer(int ID)
        {
            if (ID != null)
            {
                var user = _reporsitorycomputer.Single(c => c.ComputerID == ID);
                _reporsitorycomputer.DeleteRelatedEntities(user);
                _reporsitorycomputer.SaveChanges();
            }
            return RedirectToAction("Index", "AllUsers");
        }

        #endregion
    }
}
