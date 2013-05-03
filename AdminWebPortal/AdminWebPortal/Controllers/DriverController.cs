using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminWebPortal.Repository;
using AdminWebPortal.Models;
using System.Web.Routing;

namespace AdminWebPortal.Controllers
{
    public class DriverController : Controller
    {
        #region Initialize of context & repository/entity

        private ImageValidationEntities _context;
        private IRepository<Driver> _reporsitorydriver;
        public AdminWebPortalRepository _adminwebportalrepository;

        protected override void Initialize(RequestContext requestContext)
        {

            _context = new ImageValidationEntities();
            _reporsitorydriver = new Repository<Driver>(_context, false);
            _adminwebportalrepository = new AdminWebPortalRepository();
            base.Initialize(requestContext);
        }
        public ComputerStatus computerstatus = new ComputerStatus();

        #endregion

        //
        // GET: /Driver/

        #region Get all drivers of computer & displays in grid. Udate IsRquired Field of particular Driver
        //Get All Driver
        public ActionResult Index()
        {
            if (computerstatus.ComputerIDFromSession > 0)
            {
                DriverModel model = new DriverModel();
                model.Driver = _reporsitorydriver.GetAll().Where(x => x.ComputerID == computerstatus.ComputerIDFromSession).ToList();
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Computer");
            }
        }

        /// <summary>
        /// This action use for updating IsRequest Field to DB
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="IsRequired"></param>
        /// <returns></returns>
        public ActionResult UpdateDriverIsRequest(int ID, bool IsRequired)
        {
            Driver driver = _adminwebportalrepository.GetDriver(ID);
            driver.IsRequired = IsRequired;
            _adminwebportalrepository.Save();
            return View();
        }
        #endregion

        #region Add Driver & Edit/Update Driver

        public ActionResult AddDriver()
        {
            if (computerstatus.ComputerIDFromSession > 0)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Computer");
            }
        }

        [HttpPost]
        public ActionResult AddDriver(DriverModel model)
        {
            if (computerstatus.ComputerIDFromSession > 0)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        Driver driver = new Driver();
                        driver.ComputerID = computerstatus.ComputerIDFromSession;
                        driver.CompatID = model.Compat_ID;
                        driver.Description = model.Descriptions;
                        driver.DeviceClass = model.Device_Class;
                        driver.DeviceID = model.Device_ID;
                        driver.DeviceName = model.Device_Name;
                        driver.DriverDate = model.Driver_Date;
                        driver.DriverProviderName = model.Driver_ProviderName;
                        driver.DriverVersion = model.Driver_Version;
                        driver.FriendlyName = model.Friendly_Name;
                        driver.HardWareID = model.HardWare_ID;
                        driver.httpUrl = model.DownloadLink;
                        driver.InfName = model.Inf_Name;
                        driver.IsRequired = model.Is_Required;
                        driver.IsSigned = model.Is_Signed;
                        driver.Manufacturer = model.Manufacturer_;
                        driver.Name = model.Name_;
                        driver.PDO = model.PDO_;
                        driver.Signer = model.Signer_;

                        _reporsitorydriver.Add(driver);
                        _reporsitorydriver.SaveChanges();

                        ModelState.AddModelError("", "Driver Added Succussfully.");
                    }
                    catch (ArgumentException ae)
                    {
                        ModelState.AddModelError("", ae.Message);
                    }
                }

                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Computer");
            }
        }

        public ActionResult EditDriver(int ID)
        {
            if (computerstatus.ComputerIDFromSession > 0 && ID > 0)
            {
                DriverModel model = new DriverModel();
                Driver driver = _reporsitorydriver.Single(x => x.DriverID == ID);
                model.DriverID = driver.DriverID;
                model.Compat_ID = driver.CompatID;
                model.Descriptions = driver.Description;
                model.Device_Class = driver.DeviceClass;
                model.Device_ID = driver.DeviceID;
                model.Device_Name = driver.DeviceName;
                model.Driver_Date = driver.DriverDate;
                model.Driver_ProviderName = driver.DriverProviderName;
                model.Driver_Version = driver.DriverVersion;
                model.Friendly_Name = driver.FriendlyName;
                model.HardWare_ID = driver.HardWareID;
                model.DownloadLink = driver.httpUrl;
                model.Inf_Name = driver.InfName;
                model.Is_Required = (bool)driver.IsRequired;
                model.Is_Signed = driver.IsSigned;
                model.Manufacturer_ = driver.Manufacturer;
                model.Name_ = driver.Name;
                model.PDO_ = driver.PDO;
                model.Signer_ = driver.Signer;

                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Computer");
            }
        }

        [HttpPost]
        public ActionResult EditDriver(DriverModel model)
        {
            if (computerstatus.ComputerIDFromSession > 0)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        Driver driver = _adminwebportalrepository.GetDriver(model.DriverID);
                        driver.ComputerID = computerstatus.ComputerIDFromSession;
                        driver.CompatID = model.Compat_ID;
                        driver.Description = model.Descriptions;
                        driver.DeviceClass = model.Device_Class;
                        driver.DeviceID = model.Device_ID;
                        driver.DeviceName = model.Device_Name;
                        driver.DriverDate = model.Driver_Date;
                        driver.DriverProviderName = model.Driver_ProviderName;
                        driver.DriverVersion = model.Driver_Version;
                        driver.FriendlyName = model.Friendly_Name;
                        driver.HardWareID = model.HardWare_ID;
                        driver.httpUrl = model.DownloadLink;
                        driver.InfName = model.Inf_Name;
                        driver.IsRequired = model.Is_Required;
                        driver.IsSigned = model.Is_Signed;
                        driver.Manufacturer = model.Manufacturer_;
                        driver.Name = model.Name_;
                        driver.PDO = model.PDO_;
                        driver.Signer = model.Signer_;
                        _adminwebportalrepository.Save();

                        return RedirectToAction("Index", "Driver");

                    }
                    catch (ArgumentException ae)
                    {
                        ModelState.AddModelError("", ae.Message);
                    }
                }

                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Computer");
            }
        }

        #endregion
    }
}
