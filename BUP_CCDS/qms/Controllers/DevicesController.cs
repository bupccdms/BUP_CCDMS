using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using qms.Models;
using qms.ViewModels;
using qms.Utility;
using System.Speech.Synthesis;
using Google.Apis.Auth.OAuth2;
using System.IO;
using Google.Cloud.TextToSpeech.V1;
using qms.SignalRHub;
using System.Text.RegularExpressions;
using qms.BLL;
using Microsoft.AspNet.Identity;
using System.DirectoryServices;

using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace qms.Controllers
{

    public class DevicesController : Controller
    {


        private BLL.BLLDevices dbManager = new BLL.BLLDevices();
        private BLL.BLLDepartment dbDepartment = new BLL.BLLDepartment();


        private BLL.BLLDepartmentUsers dbDepartmentUser = new BLLDepartmentUsers();
        DisplayManager dm = new DisplayManager();
        public DevicesController()
        {
        }
        public DevicesController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;
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
        // GET: Devices
        [AuthorizationFilter(Roles = "Admin, Department Admin")]
        public ActionResult Index()
        {
            ViewBag.departmentList = dbDepartment.GetAllDepartment();

            SessionManager sm = new SessionManager(Session);
            int deptId = sm.department_id;
            var list = dbManager.GetAll().OrderBy(x => x.department_name);
            var dlist = dbManager.GetAll().Where(u => u.department_id != deptId);
            var activelist = dbManager.GetAll().OrderBy(x => x.department_name).Where(d => d.is_active == 1);
            ViewBag.userDepartmentId = sm.department_id;
            if (deptId > 0)
            {
                 dlist = dbManager.GetAll().Where(u => u.department_id == deptId);
                activelist = dbManager.GetAll().OrderBy(x => x.department_name).Where(d => d.is_active == 1 && d.department_id==deptId);
            }
               
            int total = dlist.Count();
            ViewBag.TotalDevice = total;
            int active  = activelist.Count();
            ViewBag.TotalActiveDevice = active;
            
            if (deptId > 0)
            {
                return View(dbManager.GetAll().Where(u => u.department_id == deptId).OrderBy(x => x.department_name));
            }
            return View(dbManager.GetAll().OrderBy(x=>x.department_name));
        }


        // GET: Devices/Create
        [AuthorizationFilter(Roles = "Admin, Department Admin")]
        public ActionResult Create()
        {
            int department_id = new SessionManager(Session).department_id;
            ViewBag.department_id = new SelectList(dbDepartment.GetAllDepartment().Where(x => x.is_active == 1), "department_id", "department_name", department_id);
            ViewBag.deviceList = dbManager.GetAll().OrderBy(x => x.department_name);
            return View();
        }

        //[AuthorizationFilter(Roles = "Admin, Department Admin, Display User")]
        [AllowAnonymous]
        public ActionResult DeviceList()
        {
            SessionManager sm = new SessionManager(Session);
            //int department_id = sm.department_id;
            ViewBag.department_id = sm.department_id;
            ViewBag.dispalyFooterAdd = ApplicationSetting.dispalyFooterAdd;
            ViewBag.dispalyWelcome = ApplicationSetting.dispalyWelcome;
            ViewBag.dispalyVideo = ApplicationSetting.dispalyVideo;
            ViewBag.speakLanguage = ApplicationSetting.speakLanguage;
            ViewBag.speakGender = ApplicationSetting.speakGender;
            ViewBag.speakRate = ApplicationSetting.speakRate;
            return View();
        }

        //[AuthorizationFilter(Roles = "Admin, Department Admin, Display User")]
        [AllowAnonymous]
        public JsonResult GetDisplayInfo(int department_id)
        {
            try
            {
                DisplayManager dm = new DisplayManager();

                var tokenInProgress = dm.GetInProgressTokenList(department_id);

                string nextToken = dm.GetNextTokens(department_id);

                return Json(new { success = "true", tokenInProgress = tokenInProgress, nextTokens = nextToken, dispalyVideoUrl = ApplicationSetting.dispalyVideo }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = "false", message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        

        // POST: Devices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizationFilter(Roles = "Admin, Department Admin")]
        //public ActionResult Create([Bind(Include = "device_id,department_id,device_no,location")] tblDevice tblDevice)
        public async Task<ActionResult> Create(VMDevice model)

        {
            SessionManager sm = new SessionManager(Session);
            var user = new ApplicationUser { UserName = model.UserName, Email = model.UserName, Hometown = model.UserName, PhoneNumber = model.UserName };
            int department_id = 0;
            if (!User.IsInRole("Admin"))
            {
                department_id = new SessionManager(Session).department_id;
            }
            else if (model.department_id==0)
            {
                model.department_id = sm.department_id;
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
                        var res = UserManager.AddToRole(user.Id, "Display User");

                        new BLLAspNetUser().SetActiveDirectoryUser(user.Id, model.is_active_directory_user);
                        //new BLLDevices().Create(new tblDevice() { department_id = model.department_id,  device_no = model.device_no, is_active = 1, location = model.location, user_name = model.UserName,device_name=model.device_name });
                        new BLLDevices().Create(new VMDevice() { department_id = department_id, device_no = model.device_no, is_active = 1, location = model.location, user_id = user.Id, device_name = model.device_name });

                        TempData["message"] = res.Succeeded;
                        return RedirectToAction("Index", "Devices");
                    }
                    AddErrors(result);
                }
            }
            catch (Exception)
            {
                UserManager.Delete(user);
            }
            List<tblDepartment> departmentList = new BLLDepartment().GetAllDepartment().Where(a => a.is_active == 1).ToList();
            int ddepartment_id = sm.department_id;
            if (ddepartment_id > 0)
            {
                ViewBag.deviceList = dbManager.GetAll().OrderBy(x => x.department_name);
                ViewBag.department_id = new SelectList(departmentList, "department_id", "department_name", department_id);
            }
            else
            {
                ViewBag.deviceList = dbManager.GetAll().OrderBy(x => x.department_name);
                ViewBag.department_id = new SelectList(dbDepartment.GetAllDepartment().Where(x => x.is_active == 1), "department_id", "department_name", department_id); //------Kamrul
            }


           // ViewBag.department_id = new SelectList(dbDepartment.GetAllDepartment().Where(x => x.is_active == 1), "department_id", "department_name", department_id); //------Kamrul
            ViewBag.is_active_directory_user = DropDownListManager.GetUserType(model.is_active_directory_user.ToString());
            //ViewBag.deviceList = dbManager.GetAll().OrderBy(x => x.department_name);
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
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
        // GET: Devices/Edit/5
        [AuthorizationFilter(Roles = "Admin, Department Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //tblDevice tblDevice = dbManager.GetById(id.Value);
            VMDevice device = dbManager.GetById(id.Value);
            if (device == null)
            {
                return HttpNotFound();
            }
            ViewBag.department_id = new SelectList(dbDepartment.GetAllDepartment().Where(x => x.is_active == 1), "department_id", "department_name", device.department_id);
            return View(device);
        }


        // POST: Devices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizationFilter(Roles = "Admin, Department Admin")]
        public ActionResult Edit([Bind(Include = "device_id,department_id,device_no,device_name,location,is_active")] tblDevice tblDevice)
        {
            SessionManager sm = new SessionManager(Session);
            //int department_id = 0;
            if (!User.IsInRole("Admin"))
            {
                tblDevice.department_id = new SessionManager(Session).department_id;
            }
            else if (tblDevice.department_id == 0)
            {
                tblDevice.department_id = sm.department_id;
            }

            else
            {
                tblDevice.department_id = tblDevice.department_id;
            }
            if (ModelState.IsValid)
            {
                dbManager.Edit(tblDevice);
                NotifyDisplay.SendMessages(tblDevice.department_id, tblDevice.device_no, "", false, true, false, false, false, false);
                TempData["mgs"] = true;
                return RedirectToAction("Index");
            }



            ViewBag.department_id = new SelectList(dbDepartment.GetAllDepartment(), "department_id", "department_name", tblDevice.department_id);
            return View(tblDevice);
        }

        [HttpGet]
        [AuthorizationFilter(Roles = "Admin, Department Admin")]
        public ActionResult Activate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VMDevice tblDevice = dbManager.GetById(id.Value);
            if (tblDevice == null)
            {
                return HttpNotFound();
            }
            tblDevice.is_active = 1;
            dbManager.StatusModify(tblDevice);
            NotifyDisplay.SendMessages(tblDevice.department_id, tblDevice.device_no, "", false, true, false, false, false, false);


            return RedirectToAction("Index");


        }

        [HttpGet]
        [AuthorizationFilter(Roles = "Admin, Department Admin")]
        public ActionResult Deactivate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VMDevice tblDevice = dbManager.GetById(id.Value);
            if (tblDevice == null)
            {
                return HttpNotFound();
            }
            tblDevice.is_active = 0;
            dbManager.StatusModify(tblDevice);

            NotifyDisplay.SendMessages(tblDevice.department_id, tblDevice.device_id.ToString(), "", false, false, false, false, false, true);

            return RedirectToAction("Index");


        }

        // GET: Devices/Delete/5
        [AuthorizationFilter(Roles = "Admin, Department Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VMDevice tblDevice = dbManager.GetById(id.Value);
            if (tblDevice == null)
            {
                return HttpNotFound();
            }
            return View(tblDevice);
        }

        // POST: Devices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizationFilter(Roles = "Admin, Department Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            VMDevice tblDevice = dbManager.GetById(id);
            dbManager.Remove(id);
            return RedirectToAction("Index");
        }


        [AllowAnonymous]
        [HttpGet]
        public ActionResult Display(int id)
        {
            ViewBag.department_id = id;
            ViewBag.dispalyFooterAdd = ApplicationSetting.dispalyFooterAdd;
            ViewBag.dispalyWelcome = ApplicationSetting.dispalyWelcome;
            ViewBag.dispalyVideo = ApplicationSetting.dispalyVideo;
            return View();
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbManager = null;
                dbDepartment = null;
            }
            base.Dispose(disposing);
        }
    }
}
