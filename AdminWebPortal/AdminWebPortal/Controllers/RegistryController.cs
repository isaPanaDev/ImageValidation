using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using AdminWebPortal.Models;
using AdminWebPortal.Repository;
using System.Web.Routing;
using RegFileParser;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace AdminWebPortal.Controllers
{
    public class RegistryController : Controller
    {
        #region Initialize of context & repository/entity
        private ImageValidationEntities _context;
        private IRepository<Computer> _reporsitorycomputer;
        private IRepository<Registry> _reporsitoryregistry;
        private IRepository<RegistryGroup> _reporsitoryregistrygroup;
        private IRepository<RegistryGrouping> _reporsitoryregistrygrouping;
        public AdminWebPortalRepository _adminwebportalrepository;

        protected override void Initialize(RequestContext requestContext)
        {
            _context = new ImageValidationEntities();
            _reporsitorycomputer = new Repository<Computer>(_context, false);
            _reporsitoryregistry = new Repository<Registry>(_context, false);
            _reporsitoryregistrygroup = new Repository<RegistryGroup>(_context, false);
            _reporsitoryregistrygrouping = new Repository<RegistryGrouping>(_context, false);
            _adminwebportalrepository = new AdminWebPortalRepository();

            base.Initialize(requestContext);
        }

        ComputerStatus computerstatus = new ComputerStatus();
        #endregion

        //
        // GET: /Registry/       

        #region Load Registry File List & Importing .Reg file & parse the file and store the information in the database

        /// <summary>
        ///  Load List of Registry File
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.ErrorMsg = "";
            if (computerstatus.ComputerIDFromSession > 0)
            {
                //Load List of Registry File
                RegistryModel model = GetRegistryGroup();

                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Search");
            }
        }

        /// <summary>
        /// Load List of Registry File & Upload .Reg file and parse the file and store the information in the database to relative tables.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]       
        public ActionResult Index(HttpPostedFileBase file)
        {
            try
            {
                ViewBag.ErrorMsg = "";
                if (ModelState.IsValid)
                {
                    if (computerstatus.ComputerIDFromSession > 0)
                    {
                        if (file.ContentLength > 0)
                        {
                            string FileExtention = Path.GetExtension(file.FileName);

                            if (FileExtention == ".reg")
                            {
                                var fileName = Path.GetFileName(file.FileName);
                                var path = Path.Combine(Server.MapPath("~/Content"), fileName);
                                file.SaveAs(path);


                                //Insert Data into RegistryGroup Table & Get RegistryGroupID
                                int RegistryGroupID = 0;
                                RegistryGroup reggroup = new RegistryGroup();
                                reggroup.FileName = file.FileName;
                                reggroup.Note = "Saved Data";
                                _reporsitoryregistrygroup.Add(reggroup);
                                _reporsitoryregistrygroup.SaveChanges();
                                RegistryGroupID = reggroup.RegistryGroupID;

                                try
                                {
                                    RegFileObject regfile = new RegFileObject(path);

                                    //Importing .Reg file & parse the file and store the information in the database
                                    foreach (KeyValuePair<String, Dictionary<String, RegValueObject>> entry in regfile.RegValues)
                                    {

                                        foreach (KeyValuePair<String, RegValueObject> item in entry.Value)
                                        {
                                            //Insert Data into Registry Table & Get RegistryID
                                            int RegistryID = 0;
                                            Registry registry = new Registry();
                                            registry.Key = entry.Key;
                                            registry.Value = item.Key;
                                            registry.ValueData = item.Value.Value;
                                            registry.DataType = item.Value.Type;
                                            _reporsitoryregistry.Add(registry);
                                            _reporsitoryregistry.SaveChanges();
                                            RegistryID = registry.RegistryID;

                                            //Insert Data into RegistryGrouping Table
                                            RegistryGrouping regGrouping = new RegistryGrouping();
                                            regGrouping.ComputerID = computerstatus.ComputerIDFromSession;
                                            regGrouping.RegistryID = RegistryID;
                                            regGrouping.RegistryGroupID = RegistryGroupID;
                                            _reporsitoryregistrygrouping.Add(regGrouping);
                                            _reporsitoryregistrygrouping.SaveChanges();

                                        }
                                    }

                                    ViewBag.ErrorMsg = "Uploaded successfully.";

                                    System.IO.File.Delete(path);
                                }
                                catch (ArgumentException ae)
                                {
                                    ViewBag.ErrorMsg = ae.Message;
                                }
                            }
                            else
                            {
                                ViewBag.ErrorMsg = "Please upload only registry[.reg].";
                            }
                        }

                    }

                    //Load List of Registry File
                    RegistryModel model = new RegistryModel();
                    model = GetRegistryGroup();
                    return View(model);

                }
                else
                {
                    return RedirectToAction("Index", "Search");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
                //Load List of Registry File
                RegistryModel model = new RegistryModel();
                model = GetRegistryGroup();
                return View(model);
            }

        }

        #endregion

        #region Edit/Update Data Related to Registry eg. Note And Get list of data eg. Registry, Registry File etc.

        /// <summary>
        /// Load list of Registry Key/Value/ValueData etc.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult Edit(int ID)
        {
            RegistryModel model = GetRegistryFiles(ID);
            return View(model);
        }

        /// <summary>
        /// Load list of Registry Key/Value/ValueData etc. Update/Add Note from this and update to RegistryGroup.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(RegistryModel model)
        {
            RegistryGroup registrygroup = _adminwebportalrepository.GetRegistryGroup(model.RegistryGroupID);
            registrygroup.Note = model.Note;
            _adminwebportalrepository.Save();

            model = GetRegistryFiles(model.RegistryGroupID);

            return View(model);
        }

        /// <summary>
        /// Getting list of existing registry files 
        /// </summary>
        /// <returns></returns>
        private RegistryModel GetRegistryGroup()
        {
            RegistryModel model = new RegistryModel();
            model.RegistryGroup = new List<RegistryGroup>();

            List<RegistryGrouping> RegGrouping = new List<RegistryGrouping>();

            //Getting list of data from RegistryGrouping by ComputerID
            RegGrouping = _reporsitoryregistrygrouping.Find(x => x.ComputerID == computerstatus.ComputerIDFromSession).ToList();

            //Get Distinct data from RegGrouping on RegistryGroupID 
            var data = RegGrouping.Select(x => x.RegistryGroupID).Distinct().ToList();

            foreach (var item in data)
            {
                RegistryGroup RegGroup = _reporsitoryregistrygroup.Single(x => x.RegistryGroupID == item.Value);
                model.RegistryGroup.Add(RegGroup);
            }
            return model;
        }

        /// <summary>
        /// This Function use for getting Registry Files By ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private RegistryModel GetRegistryFiles(int ID)
        {
            RegistryModel model = new RegistryModel();
            model.Registry = new List<Registry>();
            model.RegistryGroupID = ID;
            List<RegistryGrouping> RegGrouping = new List<RegistryGrouping>();

            //Getting list of data from RegistryGrouping by RegistryGroupID
            RegGrouping = _reporsitoryregistrygrouping.Find(x => x.RegistryGroupID == ID).ToList();

            foreach (var item in RegGrouping)
            {
                //Bind Registry list By RegistryGroupID(Registry File ID)
                Registry Reg = _reporsitoryregistry.Single(x => x.RegistryID == item.RegistryID);
                model.Registry.Add(Reg);
            }
            return model;
        }

        #endregion

        #region Delete Action & Functions (Delete Registry, RegistryGrouping & RegistryGroup etc)

        /// <summary>
        /// Delete Registry By Registry ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteRegistry(int ID)
        {
            if (ID > 0)
            {
                DeleteRegstry(ID);
                DeleteRegistryGrouping(ID);

                var registrygroup = _reporsitoryregistrygroup.Single(c => c.RegistryGroupID == ID);
                _reporsitoryregistrygroup.DeleteRelatedEntities(registrygroup);
                _reporsitoryregistrygroup.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// This function use for delete all Registry by RegistryGroupID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private bool DeleteRegstry(int ID)
        {
            RegistryModel model = new RegistryModel();
            model.Registry = new List<Registry>();
            model.RegistryGroupID = ID;
            List<RegistryGrouping> RegGrouping = new List<RegistryGrouping>();

            //Getting list of data from RegistryGrouping by RegistryGroupID
            RegGrouping = _reporsitoryregistrygrouping.Find(x => x.RegistryGroupID == ID).ToList();

            foreach (var item in RegGrouping)
            {
                //Bind Registry list By RegistryGroupID(Registry File ID)
                Registry Reg = _reporsitoryregistry.Single(x => x.RegistryID == item.RegistryID);
                _reporsitoryregistry.DeleteRelatedEntities(Reg);
                _reporsitoryregistry.SaveChanges();

            }
            return true;
        }

        /// <summary>
        /// This function use for delete all RegistryGrouping items by RegistryGroupID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private bool DeleteRegistryGrouping(int ID)
        {
            RegistryModel model = new RegistryModel();
            model.Registry = new List<Registry>();
            model.RegistryGroupID = ID;
            List<RegistryGrouping> RegGrouping = new List<RegistryGrouping>();

            //Getting list of data from RegistryGrouping by RegistryGroupID
            RegGrouping = _reporsitoryregistrygrouping.Find(x => x.RegistryGroupID == ID).ToList();

            foreach (var item in RegGrouping)
            {
                //Bind Registry list By RegistryGroupID(Registry File ID)
                RegistryGrouping RegistryGrouping = _reporsitoryregistrygrouping.Single(x => x.RegistryGroupingID == item.RegistryGroupingID);
                _reporsitoryregistrygrouping.DeleteRelatedEntities(RegistryGrouping);
                _reporsitoryregistry.SaveChanges();

            }
            return true;
        }

        #endregion

    }
}
