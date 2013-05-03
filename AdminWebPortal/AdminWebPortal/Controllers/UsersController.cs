using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminWebPortal.MemberShipMember;
using System.Web.Routing;
using AdminWebPortal.Models;
using System.Web.Security;
using AdminWebPortal.Repository;

namespace AdminWebPortal.Controllers
{
    public class UsersController : Controller
    {
        #region Initialize of context & repository/entity

        public AdminWebPortalMembershipProvider MembershipService { get; set; }
        public AdminWebPortalRoleProvider AuthorizationService { get; set; }
        private ImageValidationEntities _context;
        private IRepository<AccountHistory> _reporsitoryaccounthistory;
        private IRepository<UserPermission> _reporsitoryuserpermission;
        private IRepository<FailedLogon> _reporsitoryfailedlogin;
        private IRepository<User> _reporsitoryuser;
        public AdminWebPortalRepository _adminwebportalrepository;

        protected override void Initialize(RequestContext requestContext)
        {
            if (MembershipService == null)
                MembershipService = new AdminWebPortalMembershipProvider();
            if (AuthorizationService == null)
                AuthorizationService = new AdminWebPortalRoleProvider();

            _context = new ImageValidationEntities();
            _reporsitoryaccounthistory = new Repository<AccountHistory>(_context, false);
            _reporsitoryfailedlogin = new Repository<FailedLogon>(_context, false);
            _reporsitoryuser = new Repository<User>(_context, false);
            _reporsitoryuserpermission = new Repository<UserPermission>(_context, false);
            _adminwebportalrepository = new AdminWebPortalRepository();

            base.Initialize(requestContext);
        }

        #endregion
        //
        // GET: /Users/

        public ActionResult Index()
        {
            return View();
        }

        #region Login : User Login & LogOff actions. Validate User when try to login.
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            AccountHistory accounthistory = new AccountHistory();
            FailedLogon failedlogin = new FailedLogon();

            if (ModelState.IsValid)
            {
                if (MembershipService.ValidateUser(model.UserName, model.Password))
                {
                    var role = AuthorizationService.GetRolesForUser(model.UserName);
                    string userrole = role[0].ToString();
                    if (userrole == "Admin" || userrole == "WebUser" || userrole == "PowerUser")
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);

                        accounthistory.LoginDate = System.DateTime.UtcNow;
                        accounthistory.SessionID = Session.SessionID;
                        accounthistory.UserID = _adminwebportalrepository.GetUser(model.UserName).UserID;

                        _reporsitoryaccounthistory.Add(accounthistory);
                        _reporsitoryaccounthistory.SaveChanges();

                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Search");
                        }
                    }
                    else
                    {
                        failedlogin.DateTime = System.DateTime.UtcNow;
                        failedlogin.UserID = _adminwebportalrepository.GetUser(model.UserName).UserID;

                        _reporsitoryfailedlogin.Add(failedlogin);
                        _reporsitoryfailedlogin.SaveChanges();


                        ModelState.AddModelError("", "You don’t have permission to access the Web Administration Portal. Please contact your administrator.");
                    }
                }
                else
                {
                    int userID = _adminwebportalrepository.GetUser(model.UserName).UserID;
                    if (userID != 0)
                    {
                        failedlogin.DateTime = System.DateTime.UtcNow;
                        failedlogin.UserID = userID;

                        _reporsitoryfailedlogin.Add(failedlogin);
                        _reporsitoryfailedlogin.SaveChanges();
                    }

                    ModelState.AddModelError("", "Login Failed. Please contact your Administrator for assistance.");
                }
            }

            return View(model);
        }
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            ComputerStatus computerstatus = new ComputerStatus();
            Session.Abandon();
            return RedirectToAction("Index", "Home");

        }
        #endregion

        #region Users : Get all users in grid, Edit user and Delete User By ID

        [Authorize(Roles = "Admin")]
        public ActionResult EditUser(int ID)
        {
            if (ID != null)
            {
                RegisterModel model = new RegisterModel();
                var userdata = _adminwebportalrepository.GetUser(ID);

                model.Email = userdata.Email;
                model.FirstName = userdata.FirstName;
                model.LastName = userdata.LastName;
                model.SelectRoleItem = (int)userdata.PermissionID;
                model.UserName = userdata.Username;
                model.Role = _adminwebportalrepository.GetAllUserRoles().ToList();

                model.UserID = ID;
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "AllUsers");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult EditUser(RegisterModel model, int ID)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    User user = _adminwebportalrepository.GetUser(ID);
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    string hash = AdminWebPortalMembershipProvider.HashPassword(model.Password.Trim());//FormsAuthentication.HashPasswordForStoringInConfigFile(model.Password.Trim(), "md5");
                    user.Password = hash;
                    user.PermissionID = model.SelectRoleItem;
                    user.Email = model.Email;

                    _adminwebportalrepository.Save();

                    ModelState.AddModelError("", "Updated Succussfully.");
                }
                catch (ArgumentException ae)
                {
                    ModelState.AddModelError("", ae.Message);
                }
            }

            //Get all roles to filling ddl.
            model.Role = _adminwebportalrepository.GetAllUserRoles().ToList();

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DeleteUser(int ID)
        {
            if (ID > 0)
            {
                DeleteUserPermission(ID);

                var user = _reporsitoryuser.Single(c => c.UserID == ID);
                _reporsitoryuser.DeleteRelatedEntities(user);
                _reporsitoryuser.SaveChanges();
            }
            return RedirectToAction("Index", "AllUsers");
        }

        public void DeleteUserPermission(int ID)
        {
            var userpermission = _reporsitoryuserpermission.Single(c => c.UserID == ID);
            _reporsitoryuserpermission.DeleteRelatedEntities(userpermission);
            _reporsitoryuserpermission.SaveChanges();
        }
        #endregion

        #region Create User By Admin 'Get & Post Action of Register'
        [Authorize(Roles = "Admin")]
        public ActionResult Register()
        {
            RegisterModel model = new RegisterModel();
            model.Role = _adminwebportalrepository.GetAllUserRoles().ToList(); //Get all roles 
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    //Create user & get rolename by selected item
                    model.RoleName = _adminwebportalrepository.GetRole(model.SelectRoleItem).RoleName;
                    MembershipService.CreateUser(model.UserName, model.FirstName, model.LastName, model.Password, model.Email, model.RoleName);

                    //Insert data in user permission with userID & PermissionID
                    UserPermission userpermission = new UserPermission();
                    User user = _adminwebportalrepository.GetUser(model.UserName);
                    userpermission.PermissionID = (int)user.PermissionID;
                    userpermission.UserID = user.UserID;
                    _reporsitoryuserpermission.Add(userpermission);
                    _reporsitoryuserpermission.SaveChanges();


                    //FormsAuthentication.SetAuthCookie(model.UserName, false);
                    ModelState.AddModelError("", "User Added Succussfully.");
                }
                catch (ArgumentException ae)
                {
                    ModelState.AddModelError("", ae.Message);
                }
            }

            //Get all roles to filling ddl.
            model.Role = _adminwebportalrepository.GetAllUserRoles().ToList();

            return View(model);
        }
        #endregion
    }
}
