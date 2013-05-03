using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminWebPortal.Models;
using AdminWebPortal.Repository;
using System.Web.Routing;
using System.IO;
namespace AdminWebPortal.Controllers
{
    public class FileAndFolderController : Controller
    {
        #region Initialize of context & repository/entity
        private ImageValidationEntities _context;
        private IRepository<FileFolder> _reporsitoryfilefolder;
        public AdminWebPortalRepository _adminwebportalrepository;
        private IRepository<FileFolderType> _reporsitoryfilefoldertype;

        protected override void Initialize(RequestContext requestContext)
        {
            _context = new ImageValidationEntities();
            _reporsitoryfilefolder = new Repository<FileFolder>(_context, false);
            _reporsitoryfilefoldertype = new Repository<FileFolderType>(_context, false);

            _adminwebportalrepository = new AdminWebPortalRepository();

            base.Initialize(requestContext);
        }

        ComputerStatus computerstatus = new ComputerStatus();
        #endregion

        //
        // GET: /FileAndFolder/

        #region Load FileFolder List And Add File/Folder with Note/Instruction

        /// <summary>
        /// Load FileFolder List
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.ErrorMsg = "";
            if (computerstatus.ComputerIDFromSession > 0)
            {
                FileAndFolderModel model = new FileAndFolderModel();
                model.FileFoler = _reporsitoryfilefolder.Find(x => x.ComputerID == computerstatus.ComputerIDFromSession).ToList();
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Search");
            }
        }

        /// <summary>
        /// Post Form & Add File & Folder with Note/Instruction
        /// </summary>
        /// <param name="file"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file, FileAndFolderModel model, System.Web.Mvc.FormCollection form)
        {
            if (computerstatus.ComputerIDFromSession > 0)
            {
                try
                {
                    FileFolder filefolder = new FileFolder();
                    if (file != null)
                    {
                        if (file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            var path = Path.Combine(@"C:\windows\system32\", fileName);
                            var location = path;
                            filefolder.Location = location;
                            filefolder.Note = model.Note;
                            filefolder.FileFolderTypeID = 1;
                            filefolder.ComputerID = computerstatus.ComputerIDFromSession;
                            _reporsitoryfilefolder.Add(filefolder);
                            _reporsitoryfilefolder.SaveChanges();

                            ViewBag.ErrorMsg = "Uploaded successfully.";

                        }
                        else
                        {
                            if (model.FilePath != null && model.FilePath != "")
                            {
                                filefolder.Location = model.FilePath;
                                filefolder.Note = model.Note;
                                if (model.FilePath.Contains("."))
                                {
                                    filefolder.FileFolderTypeID = 1;
                                }
                                else
                                {
                                    filefolder.FileFolderTypeID = 2;
                                }
                                filefolder.ComputerID = computerstatus.ComputerIDFromSession;
                                _reporsitoryfilefolder.Add(filefolder);
                                _reporsitoryfilefolder.SaveChanges();

                                ViewBag.ErrorMsg = "Uploaded successfully.";
                            }
                            else
                            {
                                ViewBag.ErrorMsg = "Please enter File/Folder path.";
                            }

                        }
                    }
                    else
                    {
                        if (model.FilePath != null && model.FilePath != "")
                        {
                            filefolder.Location = model.FilePath;
                            filefolder.Note = model.Note;
                            if (model.FilePath.Contains("."))
                            {
                                filefolder.FileFolderTypeID = 1;
                            }
                            else
                            {
                                filefolder.FileFolderTypeID = 2;
                            }
                            filefolder.ComputerID = computerstatus.ComputerIDFromSession;
                            _reporsitoryfilefolder.Add(filefolder);
                            _reporsitoryfilefolder.SaveChanges();

                            ViewBag.ErrorMsg = "Uploaded successfully.";
                        }
                        else
                        {
                            ViewBag.ErrorMsg = "Please enter File/Folder path.";
                        }

                    }
                }
                catch (ArgumentException ae)
                {
                    ViewBag.ErrorMsg = ae.Message;
                }

                model.FileFoler = _reporsitoryfilefolder.Find(x => x.ComputerID == computerstatus.ComputerIDFromSession).ToList();
                return View(model);

            }
            else
            {
                return RedirectToAction("Index", "Search");
            }
        }

        #endregion

        #region Edit/Delete FileFolder and Function: Get FileFolder Type

        /// <summary>
        /// Load filefolder details for edit data
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult Edit(int ID)
        {
            if (computerstatus.ComputerIDFromSession > 0)
            {
                FileAndFolderModel model = new FileAndFolderModel();
                FileFolder filefolder = new FileFolder();
                filefolder = _reporsitoryfilefolder.Single(x => x.FileFolderID == ID);

                model.CurrentFilePath = filefolder.Location;
                model.Note = filefolder.Note;
                model.FileFolderID = filefolder.FileFolderID;

                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Search");
            }
        }

        /// <summary>
        /// Update file and folder, Note/Instruction
        /// </summary>
        /// <param name="file"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(HttpPostedFileBase file, FileAndFolderModel model)
        {
            FileFolder filefolder = _adminwebportalrepository.GetFileFolder(model.FileFolderID);

            if (file != null)
            {

                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFullPath(file.FileName);
                    filefolder.Location = fileName;
                    filefolder.Note = model.Note;

                    _adminwebportalrepository.Save();

                    ViewBag.ErrorMsg = "Updated successfully.";

                }
                else
                {
                    if (model.FilePath != null && model.FilePath != "")
                    {
                        filefolder.Location = model.FilePath;
                        filefolder.Note = model.Note;

                        _adminwebportalrepository.Save();

                        ViewBag.ErrorMsg = "Updated successfully.";
                    }
                    else
                    {
                        filefolder.Note = model.Note;
                        _adminwebportalrepository.Save();

                        ViewBag.ErrorMsg = "Note/Instruction updated successfully.";
                    }

                }
            }
            else
            {
                if (model.FilePath != null && model.FilePath != "")
                {
                    filefolder.Location = model.FilePath;
                    filefolder.Note = model.Note;

                    _adminwebportalrepository.Save();

                    ViewBag.ErrorMsg = "Updated successfully.";
                }
                else
                {
                    filefolder.Note = model.Note;
                    _adminwebportalrepository.Save();

                    ViewBag.ErrorMsg = "Note/Instruction updated successfully.";
                }

            }

            filefolder = _reporsitoryfilefolder.Single(x => x.FileFolderID == model.FileFolderID);
            model.CurrentFilePath = filefolder.Location;

            return View(model);
        }

        /// <summary>
        /// This action use for delete filefolder record
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int ID)
        {
            if (ID > 0)
            {
                var filefolder = _reporsitoryfilefolder.Single(c => c.FileFolderID == ID);
                _reporsitoryfilefolder.DeleteRelatedEntities(filefolder);
                _reporsitoryfilefolder.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// This Function return a FileFolder Type
        /// </summary>
        /// <param name="TypeID"></param>
        /// <returns></returns>
        public string GetFileFolderType(int TypeID)
        {
            _context = new ImageValidationEntities();
            _reporsitoryfilefoldertype = new Repository<FileFolderType>(_context, false);
            FileFolderType type = new FileFolderType();
            type = _reporsitoryfilefoldertype.Find(x => x.FileFolderTypeID == TypeID).SingleOrDefault();

            return type.Type;
        }


        #endregion
       
    }
}
