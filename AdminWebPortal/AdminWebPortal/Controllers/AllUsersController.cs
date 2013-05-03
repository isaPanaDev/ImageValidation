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
    [Authorize(Roles = "Admin")]
    public class AllUsersController : Controller
    {
        #region Initialize of context & repository/entity

        public AdminWebPortalRepository _adminwebportalrepository;
        protected override void Initialize(RequestContext requestContext)
        {
            _adminwebportalrepository = new AdminWebPortalRepository();

            base.Initialize(requestContext);
        }

        #endregion

        //
        // GET: /AllUsers/

        #region Load all user list & Displays in JqGrid
        /// <summary>
        /// Getting all user
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            try
            {
                GetAllUsers model = new GetAllUsers();
                model.User = _adminwebportalrepository.GetAllUsers().ToList();
                return View(model);
            }
            catch
            {
                throw;
            }
        }
        #endregion

    }
}
