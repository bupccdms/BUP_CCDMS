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
using qms.Utility;

namespace qms.Controllers
{
    [AuthorizationFilter(Roles ="Admin, Department Admin")]
    public class DepartmentsController : Controller
    {
        private BLL.BLLDepartment dbManager = new BLL.BLLDepartment();
        private BLL.BLLServiceDetail dbService = new BLL.BLLServiceDetail();

        // GET: Departments
        //public async Task<ActionResult> Index()
        public ActionResult Index()
        {
            return View(dbManager.GetAllDepartment().OrderBy(x=>x.department_name));
            //return View(await db.tblDepartments.ToListAsync());
        }

        // GET: Departments
        //public async Task<ActionResult> Index()
        [AuthorizationFilter(Roles = "Department Admin")]
        public ActionResult DevicesStatus()
        {
            SessionManager sm = new SessionManager(Session);
            ViewBag.department_id = sm.department_id;
            return View(dbManager.GetDeviceCurrentStatus(sm.department_id, 0));
        }

        //public async Task<ActionResult> Index()
        [AuthorizationFilter(Roles = "Department Admin")]
        public JsonResult GetDeviceCurrentStatus()
        {
            try
            {
                SessionManager sm = new SessionManager(Session);
                ViewBag.department_id = sm.department_id;
                var DeviceStatusList = dbManager.GetDeviceCurrentStatus(sm.department_id, 0);
                return Json(new { Success = true, deviceStatusList = DeviceStatusList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);

            }
            
        }

        // GET: Departments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblDepartment tblDepartment =  dbManager.GetById(id.Value);
            if (tblDepartment == null)
            {
                return HttpNotFound();
            }
            return View(tblDepartment);
        }

        // GET: Departments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "department_id,department_name,address,contact_person,contact_no,display_next,static_ip")] tblDepartment tblDepartment)
        {
            if (ModelState.IsValid)
            {
                //db.tblDepartments.Add(tblDepartment);

                //await db.SaveChangesAsync();
                dbManager.Create(tblDepartment);

                //DisplayManager dm = new Utility.DisplayManager();
                //if (!String.IsNullOrEmpty(tblDepartment.static_ip))
                //    dm.CreateTextFile(tblDepartment.department_id, tblDepartment.static_ip);
                TempData["message"] = true;
                return RedirectToAction("Index");
            }

            return View(tblDepartment);
        }

        // GET: Departments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblDepartment tblDepartment = dbManager.GetById(id.Value);
            if (tblDepartment == null)
            {
                return HttpNotFound();
            }
            return View(tblDepartment);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "department_id,department_name,address,contact_person,contact_no,display_next,static_ip,is_active")] tblDepartment tblDepartment)
        {
            if (ModelState.IsValid)
            {
                dbManager.Edit(tblDepartment);
                TempData["mgs"] = true;
                return RedirectToAction("Index");
            }
            return View(tblDepartment);
        }
        //[HttpGet]
        //[AuthorizationFilter(Roles = "Admin, Department Admin")]
        public ActionResult Activate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblDepartment tblDepartment = dbManager.GetById(id.Value);
            if (tblDepartment == null)
            {
                return HttpNotFound();
            }
            tblDepartment.is_active = 1;
            dbManager.StatusModify(tblDepartment);
            //NotifyDisplay.SendMessages(tblDevice.department_id, tblDevice.device_no, "", false, true, false, false);


            return RedirectToAction("Index");


        }

        //[HttpGet]
        //[AuthorizationFilter(Roles = "Admin, Department Admin")]
        public ActionResult Deactivate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblDepartment tblDepartment = dbManager.GetById(id.Value);
            if (tblDepartment == null)
            {
                return HttpNotFound();
            }
            tblDepartment.is_active = 0;
            dbManager.StatusModify(tblDepartment);

            //NotifyDisplay.SendMessages(tblDepartment.department_id, tblDepartment.device_no, "", false, true, false, false);

            return RedirectToAction("Index");


        }

        // GET: Departments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblDepartment tblDepartment = dbManager.GetById(id.Value);
            if (tblDepartment == null)
            {
                return HttpNotFound();
            }
            return View(tblDepartment);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    tblDepartment tblDepartment = dbManager.GetById(id);
        //    db.tblDepartments.Remove(tblDepartment);
            
        //    return RedirectToAction("Index");
        //}
        //--------------- Edit ----------------
        public JsonResult AutocompleteDepartmentSuggestions(string term)
        {
            //   var suggestions = unitOfWork.EmployeesRepository.Get().Where(w => w.IdentificationNumber.ToLower().Trim().Contains(term.ToLower().Trim()) && w.OCode == OCode && w.PFStatus != 2).OrderBy(s => s.IdentificationNumber).Select(s => new { value = s.EmpName, label = s.IdentificationNumber }).ToList();
            //List<tblDepartment> departmentList = new List<tblDepartment>();
            var departmentList = dbManager.GetAllDepartment().Where(x => x.department_name.ToLower().Trim().Contains(term.ToLower().Trim())).Select(s => new { value = s.department_id, label = s.department_name }).ToList();
            return Json(departmentList, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetServicesByDepartmentId(string id)
        //{
        //    int ID= Convert.ToInt32(id);
        //    var tableResul= dbService.GetAll().Include(x => x.tblTokenQueue.department_id == ID).ToList();
        //    return Json(tableResul);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
