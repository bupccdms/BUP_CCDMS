using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using qms.BLL;
using qms.Models;
using qms.SignalRHub;
using qms.Utility;
using qms.ViewModels;

namespace qms.Controllers
{
    [AuthorizationFilter]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        //private qmsEntities db = new qmsEntities();
        private BLL.BLLDepartment dbDepartment = new BLL.BLLDepartment();
        private BLL.BLLAspNetUser dbUser = new BLL.BLLAspNetUser();
        private BLL.BLLAspNetRole dbRoles = new BLL.BLLAspNetRole();

        private BLL.BLLDepartmentUsers dbDepartmentUser = new BLL.BLLDepartmentUsers();


        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // The Authorize Action is the end point which gets called when you access any
        // protected Web API. If the user is not logged in then they will be redirected to 
        // the Login page. After a successful login you can call a Web API.
        [HttpGet]
        public ActionResult Authorize()
        {
            var claims = new ClaimsPrincipal(User).Claims.ToArray();
            var identity = new ClaimsIdentity(claims, "Bearer");
            AuthenticationManager.SignIn(identity);
            return new EmptyResult();
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                SessionManager sm = new SessionManager(Session);
                if (sm.user_id == "")
                {
                    return RedirectToAction("SessionOut", "Account");
                }
                if (sm.LOCKOUTENABLED > 0)
                {
                    return RedirectToAction("Login", "Account");
                }
                else if (sm.IsPasswordExpired || sm.ForceChangeConfirmed == false)
                {
                    return RedirectToAction("ChangePassword", "Manage");
                }
                else if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("AdminDashboard", "Home");
                }
                else if (User.IsInRole("Department Admin"))
                {
                    return RedirectToAction("DepartmentAdminDashboard", "Home");
                }
                else if (User.IsInRole("Token Generator"))
                {
                    return RedirectToAction("Create", "TokenQueues");
                }
                else if (User.IsInRole("Service Holder"))
                {
                    return RedirectToAction("DeviceSelection", "Home");
                }
                else if (User.IsInRole("Display User"))
                {
                    return RedirectToAction("DeviceList", "Devices");
                }
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        private bool CheckActiveDirectoryLogin(string userName, string password)
        {
            string user = userName.Replace("'", "#").Trim();
            string adDomainName = ApplicationSetting.ActiveDirectoryInfo;
            bool result = false;
            using (DirectoryEntry _entry = new DirectoryEntry(String.Format("LDAP://{0}", adDomainName), user, password))
            {
                using (DirectorySearcher directorySearcher = new DirectorySearcher(_entry))
                {
                    directorySearcher.Filter = "(sAMAccountName=" + user + ")";
                    try
                    {
                        SearchResult searchResult = directorySearcher.FindOne();
                        searchResult.GetDirectoryEntry();
                        result = true;
                    }
                    catch (Exception)
                    {
                        
                        result = false;
                    }

                }
            }
            return result;
        }

        private bool CheckActiveDirectory(string userName)
        {
            //return true;
            string user = userName.Replace("'", "#").Trim();
            string adDomainName = ApplicationSetting.ActiveDirectoryInfo;
            bool result = false;
            using (DirectoryEntry _entry = new DirectoryEntry(String.Format("LDAP://{0}", adDomainName)))
            {
                using (DirectorySearcher directorySearcher = new DirectorySearcher(_entry))
                {
                    directorySearcher.Filter = "(sAMAccountName=" + user + ")";
                    try
                    {
                        SearchResult searchResult = directorySearcher.FindOne();
                        searchResult.GetDirectoryEntry();
                        result = true;
                    }
                    catch (Exception)
                    {

                        result = false;
                    }

                }
            }
            return result;
        }
        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            /// For Security Test purpose POC to client Environment . For a Certain Date Client can Test
            /// our application. 

            //DateTime date = DateTime.Now;
            //DateTime fixdate = Convert.ToDateTime("8/30/2019 12:00:00 AM");
            //int userexist = dbDepartmentUser.GetAll().Count();

            //if (date >= fixdate && userexist != 0)
            //{
            //    dbUser.DeleteTable();
            //    return RedirectToAction("SessionOut", "Account");
            //}
            //else
            //{
            //    return RedirectToAction("SessionOut", "Account");
            //}

            ////
            SessionManager sm = new SessionManager(Session);
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            AspNetUserLoginAttempts loginAttempt = new AspNetUserLoginAttempts()
            {
                UserName = model.UserName,
                is_success = 0,
                ip_address = Request.UserHostAddress,
                machine_name = DetermineCompName(Request.UserHostAddress)
            };

            VMDepartmentLogin departmentLogin = new BLLDepartmentUsers().GetAll().Where(w => w.UserName == model.UserName).FirstOrDefault();
            if (departmentLogin != null)
            {
                if (departmentLogin.is_active_directory_user > 0)
                {
                    //Check active directory login
                    if(CheckActiveDirectoryLogin(model.UserName, model.Password))
                    {
                        model.Password = "Ssl@1234";
                    }
                    else
                    {
                        loginAttempt.is_success = 0;
                        dbUser.AddLoginAttemptInfo(loginAttempt);
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(model);
                    }
                    
                }
                if (departmentLogin.is_active == 0)
                {
                    return View("Deactive");
                }
            }

            VMDevice deviceLogin = new BLLDevices().GetAll().Where(w => w.UserName == model.UserName).FirstOrDefault();
            if (deviceLogin != null)
            {
                if (deviceLogin.is_active == 0)
                {
                    return View("Deactive");
                }
            }




            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: true);
            
            switch (result)
            {
                case SignInStatus.Success:
                    {
                        //SessionManager sm = new SessionManager(Session);
                        VMSessionInfo user = dbUser.GetSessionInfoByUserName(model.UserName, Session); //-------Kamrul

                        

                        string loginProvider = Guid.NewGuid().ToString();
                        string securityToken = Cryptography.Encrypt(loginProvider, true);
                        AspNetUserLogin login = new AspNetUserLogin()
                        {
                            LoginProvider = loginProvider,
                            ProviderKey = securityToken,
                            UserId = user.user_id,
                            logout_reason=""
                        };

                        

                        sm.user_name = user.user_name;
                        sm.user_id = user.user_id;
                        sm.LoginProvider = loginProvider;
                        if (user.department_id > 0)
                        {
                            login.department_id = user.department_id;
                            if (!user.role_name.Equals("Department Admin"))
                            {
                                sm.department_id = user.department_id;
                                sm.department_name = user.department_name;
                                sm.device_id = user.device_id;
                            }
                        }
                        else
                        {
                            sm.department_id = 0;
                        }
                        sm.IsActiveDirectoryUser = (departmentLogin.is_active_directory_user > 0);
                        if (departmentLogin == null)
                        {
                            sm.IsActiveDirectoryUser= false;
                        }
                        dbUser.AddLoginInfo(login);

                        loginAttempt.is_success = 1;
                        loginAttempt.LoginProvider = loginProvider;
                        dbUser.AddLoginAttemptInfo(loginAttempt);




                        
                         if(sm.IsPasswordExpired || sm.ForceChangeConfirmed == false)
                        {
                            return RedirectToAction("ChangePassword", "Manage");
                        }
                        else if(user.role_name == "Admin")



                        {
                            return RedirectToAction("AdminDashboard", "Home");
                            //return RedirectToAction("Index", "Devices");

                        }
                        else if (user.role_name == "Department Admin")
                        {
                            return RedirectToAction("DepartmentSelection", "Home");
                            //return RedirectToAction("DepartmentAdminDashboard", "Home");
                        }
                        else if (user.role_name == "Token Generator")
                        {
                            return RedirectToAction("Create", "TokenQueues");
                        }
                        else if (user.role_name == "Service Holder")
                        {
                            return RedirectToAction("DeviceSelection", "Home");
                        }
                        else if (user.role_name == "Display User")
                        {
                            
                            //return RedirectToAction("DeviceList", "Devices");
                            return RedirectToAction("SessionOut", "Account");
                        }

                        else if (departmentLogin.Lockout == 1)
                        {

                            //return RedirectToAction("DeviceList", "Devices");
                            return RedirectToAction("SessionOut", "Account");
                        }

                        return RedirectToLocal(returnUrl);
                    }
                case SignInStatus.LockedOut:
                    loginAttempt.is_success = 0;
                    dbUser.AddLoginAttemptInfo(loginAttempt);
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, model.RememberMe });
                case SignInStatus.Failure:
                default:
                    loginAttempt.is_success = 0;
                    dbUser.AddLoginAttemptInfo(loginAttempt);
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        private string DetermineCompName(string IP)
        {
            try
            {
                IPAddress myIP = IPAddress.Parse(IP);
                IPHostEntry GetIPHost = Dns.GetHostEntry(myIP);
                List<string> compName = GetIPHost.HostName.ToString().Split('.').ToList();
                return compName.First();
            }
            catch (Exception)
            {
                return "Not Found";
            }
            
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        public JsonResult OuterLogin(string username, string password)
        {
            string responseJson = String.Empty;
            string securityToken = username;
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            try
            {
                var result = SignInManager.PasswordSignIn(username, password, false, shouldLockout: true);
                switch (result)
                {
                    case SignInStatus.Success:
                        {
                            
                            SessionManager sm = new SessionManager(Session);
                            VMSessionInfo user = dbUser.GetSessionInfoByUserName(username, Session); //-------Kamrul

                            string loginProvider = Guid.NewGuid().ToString();
                            securityToken = Cryptography.Encrypt(loginProvider, true);
                            AspNetUserLogin login = new AspNetUserLogin()
                            {
                                LoginProvider = loginProvider,
                                ProviderKey = securityToken,
                                UserId = user.user_id,
                                logout_reason = ""
                            };

                            VMDevice deviceLogin = new BLLDevices().GetAll().Where(w => w.UserName == username).FirstOrDefault();
                            if (deviceLogin != null)
                            {
                                if (deviceLogin.is_active == 0)
                                {
                                    ModelState.AddModelError("", "This account has been Deactiveted.");
                                    JsonResult jsonResult = Json(new { success = false, message = "This account has been Deactiveted." }, JsonRequestBehavior.AllowGet);
                                    responseJson = jsonResult.Data.ToString();
                                    return jsonResult;
                                }
                            }




                            sm.user_name = user.user_name;
                            sm.user_id = user.user_id;
                            sm.LoginProvider = loginProvider;
                            if (user.department_id > 0)
                            {
                                sm.department_id = login.department_id = user.department_id;
                                sm.department_name = user.department_name;
                                sm.device_id = user.device_id;
                            }
                            else
                            {
                                sm.department_id = 0;
                            }

                            dbUser.AddLoginInfo(login);

                            if(user.role_name.ToLower().Equals("display user"))
                            {
                                JsonResult jsonSuccessResult = Json(new { success = true, message = "login success", sm.department_id, sm.department_name, sm.device_id, securityToken }, JsonRequestBehavior.AllowGet);
                                responseJson = jsonSuccessResult.Data.ToString();
                                return jsonSuccessResult;
                            }
                            else
                            {
                                ModelState.AddModelError("", "Only device user can login.");
                                JsonResult jsonError = Json(new { success = false, message = "Only device user can login." }, JsonRequestBehavior.AllowGet);
                                responseJson = jsonError.Data.ToString();
                                return jsonError;
                            }

                            
                        }
                    case SignInStatus.LockedOut:
                        //return Json(new { success = false, message = "User is locked, please contact to admin" }, JsonRequestBehavior.AllowGet);
                        JsonResult jsonLockedOut = Json(new { success = false, message = "User is locked, please contact to admin" }, JsonRequestBehavior.AllowGet);
                        responseJson = jsonLockedOut.Data.ToString();
                        return jsonLockedOut;
                    case SignInStatus.RequiresVerification:
                        JsonResult jsonRequiresVerification = Json(new { success = false, message = "User is not verified, please contact to admin" }, JsonRequestBehavior.AllowGet);
                        responseJson = jsonRequiresVerification.Data.ToString();
                        return jsonRequiresVerification;
                        //return Json(new { success = false, message = "User is not verified, please contact to admin" }, JsonRequestBehavior.AllowGet);
                    case SignInStatus.Failure:
                        JsonResult jsonFailure = Json(new { success = false, message = "Invalid login attemp" }, JsonRequestBehavior.AllowGet);
                        responseJson = jsonFailure.Data.ToString();
                        return jsonFailure;
                        //return Json(new { success = false, message = "Invalid login attemp" }, JsonRequestBehavior.AllowGet);
                    default:
                        ModelState.AddModelError("", "Invalid login attempt.");
                        JsonResult jsonDefault = Json(new { success = false, message = "Invalid login attempt." }, JsonRequestBehavior.AllowGet);
                        responseJson = jsonDefault.Data.ToString();
                        return jsonDefault;
                        //return Json(new { success = false, message = "Invalid login attempt" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                JsonResult json = Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
                responseJson = new JavaScriptSerializer().Serialize(json.Data);
                return json;
            }
            finally
            {
                string requestJson = new JavaScriptSerializer().Serialize(Json(new { securityToken }, JsonRequestBehavior.AllowGet).Data);
                ApiManager.Loggin(securityToken, "OuterLogin", requestJson, responseJson);
            }

        }


        public ActionResult BLogin(string returnUrl)
        {

            //ViewBag.department_id = new SelectList("Department1", "Department2", "Department3", "Department4", "Department5");
            return View();
        }

        //
        // POST: /Account/Login
        //[HttpPost]
        //[AllowAnonymous]
        //
        //public async Task<ActionResult> BLogin(DepartmentLoginModel model, string returnUrl)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    // This doesn't count login failures towards account lockout
        //    // To enable password failures to trigger account lockout, change to shouldLockout: true
        //    var result = await SignInManager.PasswordSignInAsync(model.department_id, model.device_id, shouldLockout: false);
        //    switch (result)
        //    {
        //        case SignInStatus.Success:
        //            return RedirectToLocal(returnUrl);
        //        case SignInStatus.LockedOut:
        //            return View("Lockout");
        //        case SignInStatus.RequiresVerification:
        //            return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
        //        case SignInStatus.Failure:
        //        default:
        //            ModelState.AddModelError("", "Invalid login attempt.");
        //            return View(model);
        //    }
        //}

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AuthorizationFilter(Roles = "Admin,Department Admin")]
        public ActionResult Register()
        {
            int department_id = new SessionManager(Session).department_id;
            ViewBag.departmentList = dbDepartment.GetAllDepartment().Where(x=>x.is_active == 1);
            ViewBag.department_id = new SelectList(dbDepartment.GetAllDepartment().Where(x => x.is_active == 1), "department_id", "department_name", department_id); ///----Kamrul
            ViewBag.name = new SelectList(dbRoles.GetAllRoles().Where(e => e.Name != "Admin"), "name", "name");
            ViewBag.is_active_directory_user = DropDownListManager.GetUserType();
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AuthorizationFilter(Roles = "Admin,Department Admin")]
        
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            
            var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, Hometown = model.Hometown, PhoneNumber=model.Mobile };
            
            int department_id = 0;
            if (!User.IsInRole("Admin"))
            {
                department_id = new SessionManager(Session).department_id;
            }
            else
            {
                department_id = model.department_id;
            }

            try
            {
                

                if (model.is_active_directory_user > 0)
                {
                    if (!CheckActiveDirectory(model.UserName))
                    {
                        ModelState.AddModelError("", "User not found in Active Directory.");
                    }
                    else
                    {
                        model.ConfirmPassword = model.Password = "Ssl@1234";
                        ModelState.Clear();
                    }
                    
                }
                else
                {
                    if (model.Password.ToUpper().Contains(model.UserName.ToUpper()))
                    {
                        ModelState.AddModelError("", "Password must have not contained User Name.");
                    }
                    Regex regex = new Regex(@"^[a-zA-Z0-9.\s]+$");
                    if (!regex.Match(model.UserName).Success)
                    {
                        ModelState.AddModelError("", "User name must not have special character, only letter and digit allow.");
                    }
                }

                if (TryValidateModel(model))
                {
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        var res = UserManager.AddToRole(user.Id, model.name);
                        
                        new BLLAspNetUser().SetActiveDirectoryUser(user.Id, model.is_active_directory_user);
                        dbDepartmentUser.Create(new tblDepartmentUser() { department_id = department_id, user_id = user.Id });
                        TempData["message"] = res.Succeeded;
                        return RedirectToAction("Index", "DepartmentUsers");
                    }
                    AddErrors(result);
                }
            }
            catch (Exception)
            {
                UserManager.Delete(user);
            }

            ViewBag.department_id = new SelectList(dbDepartment.GetAllDepartment(), "department_id", "department_name",department_id); //------Kamrul
            ViewBag.name = new SelectList(dbRoles.GetAllRoles(), "name", "name", model.name);
            ViewBag.is_active_directory_user = DropDownListManager.GetUserType(model.is_active_directory_user.ToString());
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AuthorizationFilter(Roles = "Admin")]
        public ActionResult AdminRegister()
        {
            ViewBag.is_active_directory_user = DropDownListManager.GetUserType();
            return View();
        }

        //
        // POST: /Account/AdminRegister
        [HttpPost]
        [AuthorizationFilter(Roles = "Admin")]

        public async Task<ActionResult> AdminRegister(AdminRegisterViewModel model)
        {
            var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, Hometown = model.Hometown, PhoneNumber=model.Mobile };

            try
            {
                

                if (model.is_active_directory_user > 0)
                {
                    if (!CheckActiveDirectory(model.UserName))
                    {

                        ModelState.AddModelError("", "User not found in Active Directory.");
                    }
                    else
                    {
                        model.ConfirmPassword = model.Password = "Ssl@1234";
                        ModelState.Clear();
                    }
                    
                }
                else
                {
                    if (model.Password.ToUpper().Contains(model.UserName.ToUpper()))
                    {
                        ModelState.AddModelError("", "Password must have not contained User Name.");
                    }
                    Regex regex = new Regex(@"^[a-zA-Z0-9.\s]+$");
                    if (!regex.Match(model.UserName).Success)
                    {
                        ModelState.AddModelError("", "User name must not have special character, only letter and digit allow.");
                    }
                }

                if (TryValidateModel(model))
                {
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        new BLLAspNetUser().SetActiveDirectoryUser(user.Id, model.is_active_directory_user);

                       var res= UserManager.AddToRole(user.Id, "Admin");
                        TempData["message"] = res.Succeeded;
                        return RedirectToAction("Index", "DepartmentUsers");
                    }
                    AddErrors(result);
                }
            }
            catch (Exception)
            {
                UserManager.Delete(user);
            }

            // If we got this far, something failed, redisplay form
            ViewBag.is_active_directory_user = DropDownListManager.GetUserType(model.is_active_directory_user.ToString());
            return View(model);
        }

        [AuthorizationFilter(Roles = "Admin,Department Admin")]
        public ActionResult EditUser(string userId)
        {
            if (userId == null)
            {
                return View("Error");
            }
            string user_id = Cryptography.Decrypt(userId, true);
            
            AspNetUser user = new BLLAspNetUser().GetAllUser().Where(w => w.Id == user_id).FirstOrDefault();
            if (user == null)
            {
                return View("Error");
            }

            EditUserViewModel model = new EditUserViewModel()
            {
                UserName = user.UserName,
                Mobile = user.PhoneNumber,
                Email=user.Email,
                Hometown=user.Hometown
            };
            return View(model);
        }

        //
        // POST: /Account/AdminRegister
        [HttpPost]
        [AuthorizationFilter(Roles = "Admin,Department Admin")]

        public async Task<ActionResult> EditUser(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            ApplicationUser user = await UserManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return View("Error");
            }

            user.Hometown = model.Hometown;
            user.Email = model.Email;
            user.PhoneNumber = model.Mobile;

            var result = await UserManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["message"] = result.Succeeded;
                return RedirectToAction("Index", "DepartmentUsers");
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.UserName);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AuthorizationFilter(Roles = "Admin, Department Admin")]
        public ActionResult ResetPassword(string userId)
        {
            if (userId == null)
            {
                return View("Error");
            }
            string user_id = Cryptography.Decrypt(userId, true);
            var Code = UserManager.GeneratePasswordResetTokenAsync(user_id);

            AspNetUser user = new BLLAspNetUser().GetAllUser().Where(w => w.Id == user_id).FirstOrDefault();
            if (user == null)
            {
                return View("Error");
            }

            ResetPasswordViewModel model = new ResetPasswordViewModel()
            {
                UserName=user.UserName,
                Code = Code.Result
            };
            return View(model);
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AuthorizationFilter(Roles = "Admin, Department Admin")]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return View("Error");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                new BLLAspNetUser().UserForceChangeConfirmed(user.Id, false);
                VMSessionInfo resetuser = dbUser.GetUserInfoByUserName(model.UserName); //-------Kamrul
                if (resetuser.device_id > 0)
                {
                    NotifyDisplay.SendMessages(resetuser.department_id, resetuser.device_id.ToString(), "", false, false, false, false, true, false);
                }
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AuthorizationFilter(Roles = "Admin, Department Admin")]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/SetDirectoryUser
        [AuthorizationFilter(Roles = "Admin, Department Admin")]
        public ActionResult SetDirectoryUser(string userId, int is_active_directory_user)
        {
            if (userId == null)
            {
                return View("Error");
            }
            string user_id = Cryptography.Decrypt(userId, true);
            var Code = UserManager.GeneratePasswordResetTokenAsync(user_id);

            AspNetUser user = new BLLAspNetUser().GetAllUser().Where(w => w.Id == user_id).FirstOrDefault();
            if (user == null)
            {
                return View("Error");
            }

            ActiveDirectoryTransferViewModel model = new ActiveDirectoryTransferViewModel()
            {
                UserName = user.UserName,
                Code = Code.Result,
                is_active_directory_user = is_active_directory_user,
                Password = (is_active_directory_user>0 ? "Ssl@1234" : ""),
                ConfirmPassword = (is_active_directory_user > 0 ? "Ssl@1234" : "")
            };
            return View(model);
        }

        //
        // POST: /Account/SetDirectoryUser
        [HttpPost]
        [AuthorizationFilter(Roles = "Admin, Department Admin")]
        public async Task<ActionResult> SetDirectoryUser(ActiveDirectoryTransferViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return View("Error");
            }
            if (model.is_active_directory_user > 0)
            {
                if (!CheckActiveDirectory(user.UserName))
                {

                    ModelState.AddModelError("", "User not found in Active Directory.");
                    return View(model);
                }
                else
                {
                    var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
                    if (result.Succeeded)
                    {
                        new BLLAspNetUser().SetActiveDirectoryUser(user.Id, 1);
                        return RedirectToAction("SetDirectoryUserConfirmation", "Account", new { isTransferToActiveDirectory = true });
                    }
                    AddErrors(result);
                }

            }
            else
            {
                var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
                if (result.Succeeded)
                {
                    new BLLAspNetUser().UserForceChangeConfirmed(user.Id, false);
                    new BLLAspNetUser().SetActiveDirectoryUser(user.Id, 0);
                    return RedirectToAction("SetDirectoryUserConfirmation", "Account", new { isTransferToActiveDirectory =false});
                }
                AddErrors(result);
            }
            
            return View();
        }

        //
        // GET: /Account/SetDirectoryUserConfirmation
        [AuthorizationFilter(Roles = "Admin, Department Admin")]
        public ActionResult SetDirectoryUserConfirmation(bool isTransferToActiveDirectory)
        {
            ViewBag.isTransferToActiveDirectory = isTransferToActiveDirectory;
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, model.ReturnUrl, model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, Hometown = model.Hometown };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult SessionOut()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            SessionManager sm = new SessionManager(Session);
            if (sm.LoginProvider.Length > 0)
            {
                if (sm.device_id > 0)
                {
                    NotifyDisplay.SendMessages(sm.department_id, sm.device_no, "", false, true, false, false, false, false);
                    NotifyDisplay.DeviceStatusChanged(sm.department_id);
                }
                dbUser.UpdateLogoutInfo(sm.LoginProvider, null, "Session time out");
            }
            sm.Close();
            return View();
        }
        [HttpGet]
        [Authorize]
        public ActionResult ForeceLogOut()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            SessionManager sm = new SessionManager(Session);
            if (sm.LoginProvider.Length > 0)
            {
                if (sm.device_id > 0)
                {
                    NotifyDisplay.SendMessages(sm.department_id, sm.device_no, "", false, true, false, false, false, false);
                    NotifyDisplay.DeviceStatusChanged(sm.department_id);
                }
                dbUser.UpdateLogoutInfo(sm.LoginProvider, null, "Session time out");
            }
            sm.Close();
            return View();
        }
        //
        // GET: /Account/LogOff
        [HttpGet]
        [Authorize]
        public ActionResult LogOff()
        {
            ViewBag.logout_type_id = new SelectList(new BLLLogoutType().GetAll().Where(w=>w.is_active==1), "logout_type_id", "logout_type_name");
            return View();
        }
        //
        // POST: /Account/LogOff
        [HttpPost]
        public ActionResult LogOff(int? logout_type_id)
        {
            ////if (String.IsNullOrEmpty(logout_type_id))
            ////{
            ////    ModelState.AddModelError("", "Logout reason can not be empty");
            ////    ViewBag.logout_type_id = new SelectList(new BLLLogoutType().GetAll().Where(w => w.is_active == 1), "logout_type_id", "logout_type_name");
            ////    return View();
            ////}
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            SessionManager sm = new SessionManager(Session);
            if (sm.LoginProvider.Length > 0)
            {
                if (sm.device_id > 0)
                {
                    NotifyDisplay.SendMessages(sm.department_id, sm.device_no, "", false, true, false, false, false, false);
                    NotifyDisplay.DeviceStatusChanged(sm.department_id);
                }
                if(logout_type_id.HasValue)
                    dbUser.UpdateLogoutInfo(sm.LoginProvider, logout_type_id.Value, " ");
                else
                    dbUser.UpdateLogoutInfo(sm.LoginProvider, null, " ");
            }
            sm.Close();
            return RedirectToAction("Login", "Account");
        }

        




        [HttpPost]
        [AllowAnonymous]
        public JsonResult OuterLogOff(string securityToken)
        {
            string responseJson = String.Empty;
            try
            {
                ApiManager.ValidUserBySecurityToken(securityToken);
                string loginProvider = Cryptography.Decrypt(securityToken, true);
                dbUser.UpdateLogoutInfo(loginProvider, null, "External Use only");

                JsonResult json = Json(new { success = true, message = "Log out success" }, JsonRequestBehavior.AllowGet);
                responseJson = new JavaScriptSerializer().Serialize(json.Data);
                return json;
               // return Json(new { success = true, message = "Log out success" }, JsonRequestBehavior.AllowGet);
                


            }
            catch (Oracle.DataAccess.Client.OracleException ex)
            {
                if (ex.Number == 20001)
                {
                    JsonResult json = Json(new { success = false, message = "Invalid security token" }, JsonRequestBehavior.AllowGet);
                    responseJson = new JavaScriptSerializer().Serialize(json.Data);
                    return json;
                    //return Json(new { success = false, message = "Invalid security token" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    JsonResult json = Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
                    responseJson = new JavaScriptSerializer().Serialize(json.Data);
                    return json;
                    //return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
                    
            }
            catch (Exception ex)
            {
                JsonResult json = Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
                responseJson = new JavaScriptSerializer().Serialize(json.Data);
                return json;
            }
            finally
            {
                string requestJson = new JavaScriptSerializer().Serialize(Json(new { securityToken }, JsonRequestBehavior.AllowGet).Data);
                ApiManager.Loggin(securityToken, "OuterLogOff", requestJson, responseJson);
            }

        }


        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}
