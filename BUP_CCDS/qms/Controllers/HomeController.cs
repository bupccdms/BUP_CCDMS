using qms.BLL;
using qms.Models;
using qms.SignalRHub;
using qms.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace qms.Controllers
{
    [AuthorizationFilter]
    public class HomeController : Controller
    {
        private qmsEntities db = new qmsEntities();
        private BLL.BLLDepartment dbDepartment = new BLL.BLLDepartment();
        private BLL.BLLDevices dbDevice = new BLL.BLLDevices();
        private BLL.BLLDepartmentUsers dbUser = new BLL.BLLDepartmentUsers();
        private BLL.BLLAspNetUser db_User = new BLL.BLLAspNetUser();

        [AuthorizationFilter(Roles = "Service Holder")]
        public ActionResult Index()
        {
            int department_id = 0;
            if (!User.IsInRole("Admin"))
            {
                department_id = new SessionManager(Session).department_id;
            }
            //ViewBag.departmentList = dbDepartment.GetAllDepartment().Where(x => x.is_active == 1);

            //SessionManager sm = new SessionManager(Session);
            //ViewBag.userDepartmentId = sm.department_id;

            List<tblDepartment> departmentList = new List<tblDepartment>();
            departmentList = dbDepartment.GetAllDepartment().Where(x => x.is_active == 1).ToList();
            ViewBag.departmentList = departmentList;
            ViewBag.userDepartmentId = department_id;
            return View();
        }

        [AuthorizationFilter(Roles = "Admin, Department Admin, Token Generator")]
        public ActionResult Home()
        {
            return View();
        }



        [AuthorizationFilter]
        public JsonResult GetUserAndDeviceByDepartmentId(int departmentId, bool isOnlyServiceHolder)
        {

            var userList = dbUser.GetAll().Where(a => (!isOnlyServiceHolder && a.department_id == departmentId) || (a.department_id == departmentId && a.Name == "Service Holder")).Select(x => new
            {
                x.user_id,
                user_name = (isOnlyServiceHolder ? x.Hometown : x.UserName)
            }).ToList();

            var deviceList = dbDevice.GetAllDevice().Where(a => a.department_id == departmentId).Select(x => new
            {
                x.device_id,
                x.device_no
            }).ToList();
            return Json(new { success = true, userList = userList, deviceList = deviceList }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizationFilter(Roles = "Service Holder")]
        public ActionResult DepartmentLogin()
        {
            return View();
        }


        [AuthorizationFilter(Roles = "Admin")]
        public ActionResult AdminDashboard()
        {
            ViewBag.departmentList = dbDepartment.GetAllDepartment();

            SessionManager sm = new SessionManager(Session);
            ViewBag.userDepartmentId = sm.department_id;

            //int department_id = new SessionManager(Session).department_id;
            //ViewBag.department_id = new SelectList(dbDepartment.GetAllDepartment(), "department_id", "department_name");
            return View(new BLLDepartmentUsers().GetAll());
        }




        [AuthorizationFilter(Roles = "Department Admin")]
        public ActionResult DepartmentSelection()
        {


            SessionManager sm = new SessionManager(Session);
            List<tblDepartment> departmentList = dbDepartment.GetDepartmentsByUserId(sm.user_id);
            if (departmentList.Count == 1)
            {
                tblDepartment department = departmentList.FirstOrDefault();
                sm.department_id = department.department_id;
                sm.department_name = department.department_name;
                //new BLL.BLLAspNetUser().UpdateLoginInfo(sm.LoginProvider, sm.department_id);
                return RedirectToAction("DepartmentAdminDashboard", "Home");
            }
            ViewBag.departmentList = departmentList;
            return View();
        }

        [AuthorizationFilter(Roles = "Department Admin")]
        [HttpPost]
        public ActionResult DepartmentSelection(DepartmentLoginModel model)
        {
            if (model.department_id!=0)
            {
                SessionManager sm = new SessionManager(Session);
                sm.department_id = model.department_id;
                sm.department_name = dbDepartment.GetById(model.department_id).department_name;
                new BLL.BLLAspNetUser().UpdateDepartmentAdminLoginInfo(sm.LoginProvider, sm.department_id);
                return RedirectToAction("DepartmentAdminDashboard", "Home");
            }
            else
            {
                return View();
            }

        }

        [AuthorizationFilter(Roles = "Department Admin")]
        public ActionResult DepartmentAdminDashboard()
        {
            return View();
        }

        [AuthorizationFilter(Roles = "Service Holder")]
        public ActionResult DeviceSelection()
        {
            SessionManager sm = new SessionManager(Session);
            int department_id = 0;
            string department_name = "";
            if (!User.IsInRole("Admin"))
            {

                department_id = sm.department_id;
                department_name = sm.department_name;
            }


            List<tblDevice> deviceList = dbDevice.GetFree(department_id, sm.user_id);
            ViewBag.deviceList = deviceList;
            ViewBag.department_id = department_id;
            ViewBag.department_name = department_name;
            ViewBag.VMDepartmentDeviceStatus = new BLL.BLLDepartment().GetDeviceCurrentStatus(sm.department_id, 0);
            return View();
        }

        [AuthorizationFilter(Roles = "Service Holder")]
        [HttpPost]
        public ActionResult DeviceSelection(DepartmentLoginModel model)
        {
            if (ModelState.IsValid)
            {
                SessionManager sm = new SessionManager(Session);
                sm.device_id = model.device_id;
                sm.device_no = dbDevice.GetAllDevice().Where(w => w.device_id == model.device_id).FirstOrDefault().device_no;
                new BLL.BLLAspNetUser().UpdateLoginInfo(sm.LoginProvider, sm.device_id);
                NotifyDisplay.DeviceStatusChanged(sm.department_id);
                return RedirectToAction("Create", "ServiceDetails");
            }
            else
            {
                return View();
            }

        }
    }
}
