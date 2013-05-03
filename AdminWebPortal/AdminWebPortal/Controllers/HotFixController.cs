using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminWebPortal.Models;
using AdminWebPortal.Repository;
using System.Web.Routing;
using System.Web.UI;

namespace AdminWebPortal.Controllers
{
    public class HotFixController : Controller
    {
        #region Initialize of context & repository/entity
        private ImageValidationEntities _context;
        private IRepository<HotFix> _reporsitoryhotfix;
        public AdminWebPortalRepository _adminwebportalrepository;

        protected override void Initialize(RequestContext requestContext)
        {
            _context = new ImageValidationEntities();
            _reporsitoryhotfix = new Repository<HotFix>(_context, false);
            _adminwebportalrepository = new AdminWebPortalRepository();

            base.Initialize(requestContext);
        }

        ComputerStatus computerstatus = new ComputerStatus();
        #endregion
        //
        // GET: /HotFix/

        #region Load all MS hotfixes By Computer ID

        /// <summary>
        /// Load hotfix list by computer id.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (computerstatus.ComputerIDFromSession > 0)
            {
                HotFixModel model = new HotFixModel();

                model.HotFix = _reporsitoryhotfix.Find(x => x.ComputerID == computerstatus.ComputerIDFromSession).ToList();
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Search");
            }
        }

        #endregion

        #region Delete hotfix record and Update IsRequest field to db.

        /// <summary>
        /// This action use for delete hotfix record
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int ID)
        {
            if (ID > 0)
            {
                var filefolder = _reporsitoryhotfix.Single(c => c.HotFixID == ID);
                _reporsitoryhotfix.DeleteRelatedEntities(filefolder);
                _reporsitoryhotfix.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// This action use for updating IsRequest Field to DB
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="IsRequired"></param>
        /// <returns></returns>
        public ActionResult UpdateHotFixIsRequest(int ID, bool IsRequired)
        {
          
            HotFix hotfix = _adminwebportalrepository.GetHotFix(ID);
            hotfix.IsRequired = IsRequired;
            _adminwebportalrepository.Save();
            return View();
        }

        #endregion

    }
}
