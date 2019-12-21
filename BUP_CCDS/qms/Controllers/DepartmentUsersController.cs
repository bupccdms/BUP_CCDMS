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
using qms.ViewModels;

namespace qms.Controllers
{
    [AuthorizationFilter(Roles = "Admin, Department Admin")]
    public class DepartmentUsersController : Controller
    {
       
        private BLL.BLLDepartmentUsers dbManager = new BLL.BLLDepartmentUsers();
        private BLL.BLLDepartment dbDepartment = new BLL.BLLDepartment();
        private BLL.BLLAspNetUser dbUser = new BLL.BLLAspNetUser();
        private BLL.BLLAspNetRole dbUserRole = new BLL.BLLAspNetRole();


        // GET: DepartmentUsers
        public ActionResult Index()
        {
            
            ViewBag.departmentList = dbDepartment.GetAllDepartment();
            List<AspNetRole> rolelist = dbUserRole.GetAllRoles();
            ViewBag.roleList = rolelist.Where(x => x.Name != "Display User");
            foreach (AspNetRole role in rolelist)
            {
                role.Name = (role.Name == "Admin" ? "Super Admin" : role.Name);
                //ViewBag.Name = role.Name;
            }


            SessionManager sm = new SessionManager(Session);
            ViewBag.userDepartmentId = sm.department_id;
            int department_id = sm.department_id;
            if (department_id > 0)
            {
                return View(dbManager.GetAll().Where(u => u.department_id == department_id && u.Name != "Display User"));
            }
            var list = dbManager.GetAll().Where(x => x.Name != "Display User");

            return View(dbManager.GetAll().Where(x=>x.Name != "Display User"));
            //return View(await db.tblDepartmentUsers.ToListAsync());
        }

        // GET: DepartmentUsers/Details/5


        // GET: DepartmentUsers/Create
        [AuthorizationFilter(Roles = "Admin")]
        public ActionResult Create(string userId)
        {
            string user_id = Cryptography.Decrypt(userId, true);
            List<VMDepartmentLogin> departmentUsers = dbManager.GetAll().Where(w => w.user_id == user_id).ToList();
            AspNetUser user = dbUser.GetAllUser().Where(w => w.Id == user_id).FirstOrDefault();
            if(user==null) return RedirectToAction("Index");

            ViewBag.department_id = new SelectList(dbDepartment.GetAllDepartment().Where(x => x.is_active == 1).GroupJoin(departmentUsers, br => br.department_id, bu => bu.department_id, (br, bu) => new { Departments = br, count = bu.Count() }).Where(w => w.count == 0).Select(s => s.Departments), "department_id", "department_name");

            VMDepartmentLogin departmentUser = departmentUsers.FirstOrDefault();
            return View(departmentUser);
        }

        // POST: DepartmentUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizationFilter(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "user_id,UserName,department_id")] VMDepartmentLogin DepartmentUser)
        {
            if (ModelState.IsValid)
            {
                tblDepartmentUser tblDepartmentUser = new tblDepartmentUser() { department_id = DepartmentUser.department_id, user_id = DepartmentUser.user_id };
                dbManager.Create(tblDepartmentUser);
                return RedirectToAction("Index");
            }
            List<VMDepartmentLogin> departmentUsers = dbManager.GetAll().Where(w => w.user_id == DepartmentUser.user_id).ToList();
            AspNetUser user = dbUser.GetAllUser().Where(w => w.Id == DepartmentUser.user_id).FirstOrDefault();
            if (user == null) return RedirectToAction("Index");

            ViewBag.department_id = new SelectList(dbDepartment.GetAllDepartment().GroupJoin(departmentUsers, br => br.department_id, bu => bu.department_id, (br, bu) => new { Departments = br, count = bu.Count() }).Where(w => w.count == 0).Select(s => s.Departments), "department_id", "department_name");
            

            return View();
        }

        // GET: DepartmentUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblDepartmentUser tblDepartmentUser = dbManager.GetById(id.Value);
            if (tblDepartmentUser == null)
            {
                return HttpNotFound();
            }
            return View(tblDepartmentUser);
        }

        // POST: DepartmentUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "user_department_id,user_id,department_id")] tblDepartmentUser tblDepartmentUser)
        {
            if (ModelState.IsValid)
            {
                dbManager.Edit(tblDepartmentUser);
                TempData["mgs"] = true;
                return RedirectToAction("Index");
            }
            return View(tblDepartmentUser);
        }

        // GET: DepartmentUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblDepartmentUser tblDepartmentUser = dbManager.GetById(id.Value);
            if (tblDepartmentUser == null)
            {
                return HttpNotFound();
            }
            return View(tblDepartmentUser);
        }

        // POST: DepartmentUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblDepartmentUser tblDepartmentUser = dbManager.GetById(id);
            dbManager.Remove(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SetActivationStatus(string user_id, int is_activate)
        {
            try
            {
                new BLL.BLLAspNetUser().SetActivation(user_id, is_activate);
                return Json(new { success = "true", message = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { success = "false", message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            
        }

        public ActionResult DepartmentRolewiseList(int department_id, string role_id)
        {
            try
            {
                var list = dbManager.GetAll().Where(u => u.department_id == department_id && u.role_id == role_id);
                return RedirectToAction("Index");
                //return Json(new { success = "true", message = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { success = "false", message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
