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
    public class ApplicationController : Controller
    {
        #region Initialize of context & repository/entity

        private ImageValidationEntities _context;
        private IRepository<Application> _reporsitoryapplication;
        public AdminWebPortalRepository _adminwebportalrepository;
        private IRepository<Driver> _reporsitorydriver;
        private IRepository<ApplicationDriverDepency> _reporsitoryapplicationdriverdepency;

        protected override void Initialize(RequestContext requestContext)
        {

            _context = new ImageValidationEntities();
            _reporsitoryapplication = new Repository<Application>(_context, false);
            _reporsitorydriver = new Repository<Driver>(_context, false);
            _reporsitoryapplicationdriverdepency = new Repository<ApplicationDriverDepency>(_context, false);
            _adminwebportalrepository = new AdminWebPortalRepository();
            base.Initialize(requestContext);
        }

        public ComputerStatus computerstatus = new ComputerStatus();

        #endregion

        //
        // GET: /Application/

        #region Load all application & Bind in grid & table
        //Load Applications
        public ActionResult Index()
        {
            try
            {
                if (computerstatus.ComputerIDFromSession > 0)
                {
                    ApplicationModel model = new ApplicationModel();
                    model.Application = _reporsitoryapplication.GetAll().Where(x => x.ComputerID == computerstatus.ComputerIDFromSession).ToList();
                    return View(model);
                }
                else
                {
                    return RedirectToAction("Index", "Computer");
                }
            }
            catch
            {
                throw;
            }
        }


        #endregion

        #region Add Application Get/Post And Add Device Dependency
        /// <summary>
        /// Add New Application under specific computer 
        /// </summary>
        /// <returns></returns>
        public ActionResult AddApplication()
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
        public ActionResult AddApplication(ApplicationModel model)
        {
            if (computerstatus.ComputerIDFromSession > 0)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        Application application = new Application();
                        application.ComputerID = computerstatus.ComputerIDFromSession;
                        application.DisplayName = model.Display_Name;
                        application.DisplayVersion = model.Display_Version;
                        application.Publisher = model.Publisher_;
                        application.VersionMinor = model.Version_Minor;
                        application.VersionMajor = model.Version_Major;
                        application.Version = model.Version_;
                        application.HelpLink = model.Help_Link;
                        application.HelpTelephone = model.Help_Telephone;
                        application.InstallDate = model.Install_Date;
                        application.InstallLocation = model.Install_Location;
                        application.InstallSource = model.Install_Source;
                        application.URLInfoAbout = model.URL_InfoAbout;
                        application.URLUpdateInfo = model.URL_UpdateInfo;
                        application.Comments = model.Comment;
                        application.AuthorizedCDFPrefix = model.Authorized_CDFPrefix;
                        application.Contact = model.Contact_;
                        application.EstimatedSize = model.Estimated_Size;
                        application.Language = model.Language_;
                        application.ModifyPath = model.Modify_Path;
                        application.ReadMe = model.Read_me;
                        application.UnInstallString = model.Uninstall_String;
                        application.SettingIdentifier = model.Settings_Identifier;
                        application.IsRequired = model.Is_Required;
                        application.ApplicationUrl = model.Download_Link;

                        _reporsitoryapplication.Add(application);
                        _reporsitoryapplication.SaveChanges();

                        ModelState.AddModelError("", "Application Added Succussfully.");
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

        public ActionResult AddDeviceDependency(int ID)
        {
            if (computerstatus.ComputerIDFromSession > 0 && ID > 0)
            {
                DriverModel model = new DriverModel();
                model.Application_ID = ID;
                model.Driver = _reporsitorydriver.GetAll().Where(x => x.ComputerID == computerstatus.ComputerIDFromSession).ToList();
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Computer");
            }
        }

        public ActionResult UpdateDriverIsRequest(int ID, int DrvID)
        {
            ApplicationDriverDepency devicedepency = new ApplicationDriverDepency();
            devicedepency.ApplicationID = ID;
            devicedepency.DriverID = DrvID;
            _reporsitoryapplicationdriverdepency.Add(devicedepency);
            _reporsitoryapplicationdriverdepency.SaveChanges();
            return View();
        }
        #endregion

        #region Edit/Update/Delete and Update IsRequired field
        /// <summary>
        /// Edit Application Information & Update
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "Admin, PowerUser")]
        public ActionResult EditApplication(int ID)
        {
            if (computerstatus.ComputerIDFromSession > 0 && ID > 0)
            {
                ApplicationModel model = new ApplicationModel();
                Application application = _reporsitoryapplication.Single(x => x.ApplicationID == ID);
                model.Application_ID = application.ApplicationID;
                model.Display_Name = application.DisplayName;
                model.Display_Version = application.DisplayVersion;
                model.Display_Version = application.Publisher;
                model.Version_Minor = application.VersionMinor;
                model.Version_Major = application.VersionMajor;
                model.Version_ = application.Version;
                model.Help_Link = application.HelpLink;
                model.Help_Telephone = application.HelpTelephone;
                model.Install_Date = application.InstallDate;
                model.Install_Location = application.InstallLocation;
                model.Install_Source = application.InstallSource;
                model.URL_InfoAbout = application.URLInfoAbout;
                model.URL_UpdateInfo = application.URLUpdateInfo;
                model.Comment = application.Comments;
                model.Authorized_CDFPrefix = application.AuthorizedCDFPrefix;
                model.Contact_ = application.Contact;
                model.Estimated_Size = application.EstimatedSize;
                model.Language_ = application.Language;
                model.Modify_Path = application.ModifyPath;
                model.Read_me = application.ReadMe;
                model.Uninstall_String = application.UnInstallString;
                model.Settings_Identifier = application.SettingIdentifier;
                model.Is_Required = (bool)application.IsRequired;
                model.Download_Link = application.ApplicationUrl;

                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Computer");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin, PowerUser")]
        public ActionResult EditApplication(ApplicationModel model)
        {
            if (computerstatus.ComputerIDFromSession > 0)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        Application application = _adminwebportalrepository.GetApplication(model.Application_ID);
                        int ComputerID = computerstatus.ComputerIDFromSession;
                        application.ComputerID = computerstatus.ComputerIDFromSession;
                        application.DisplayName = model.Display_Name;
                        application.DisplayVersion = model.Display_Version;
                        application.Publisher = model.Publisher_;
                        application.VersionMinor = model.Version_Minor;
                        application.VersionMajor = model.Version_Major;
                        application.Version = model.Version_;
                        application.HelpLink = model.Help_Link;
                        application.HelpTelephone = model.Help_Telephone;
                        application.InstallDate = model.Install_Date;
                        application.InstallLocation = model.Install_Location;
                        application.InstallSource = model.Install_Source;
                        application.URLInfoAbout = model.URL_InfoAbout;
                        application.URLUpdateInfo = model.URL_UpdateInfo;
                        application.Comments = model.Comment;
                        application.AuthorizedCDFPrefix = model.Authorized_CDFPrefix;
                        application.Contact = model.Contact_;
                        application.EstimatedSize = model.Estimated_Size;
                        application.Language = model.Language_;
                        application.ModifyPath = model.Modify_Path;
                        application.ReadMe = model.Read_me;
                        application.UnInstallString = model.Uninstall_String;
                        application.SettingIdentifier = model.Settings_Identifier;
                        application.IsRequired = model.Is_Required;
                        application.ApplicationUrl = model.Download_Link;

                        _adminwebportalrepository.Save();

                        return RedirectToAction("Index", "Application");

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

        /// <summary>
        /// Delete Application By Application ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteApplication(int ID)
        {
            if (ID != null)
            {
                var user = _reporsitoryapplication.Single(c => c.ApplicationID == ID);
                _reporsitoryapplication.DeleteRelatedEntities(user);
                _reporsitoryapplication.SaveChanges();
            }
            return RedirectToAction("Index", "Application");
        }

        /// <summary>
        /// Update IsRequired Field in application entity
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="IsRequired"></param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "Admin, PowerUser")]
        public ActionResult UpdateIsRequired(int ID, bool IsRequired)
        {
            Application application = _adminwebportalrepository.GetApplication(ID);
            application.IsRequired = IsRequired;
            _adminwebportalrepository.Save();
            return View();
        }
        #endregion
    }
}
