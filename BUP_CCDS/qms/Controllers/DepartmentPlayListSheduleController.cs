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
    public class DepartmentPlayListSheduleController : Controller
    {
        private BLL.BLLPlayList dbPlayList = new BLL.BLLPlayList();
        private BLL.BLLDepartment dbDepartmentList = new BLL.BLLDepartment();
        //private BLL.BLLPlayListSheduling dbManager = new BLL.BLLPlayListSheduling();
        private BLL.BLLDeptPlayListShedule dbManager = new BLL.BLLDeptPlayListShedule();

        public ActionResult Index()
        {
            return View(dbManager.GetAll());
        }

        // GET: BreakTypes/Create
        public ActionResult Create()
        {
            int department_id = new SessionManager(Session).department_id;
            if (department_id > 0)
            {
                ViewBag.department_id = new SelectList(dbDepartmentList.GetAllDepartment().Where(x=>x.department_id == department_id), "department_id", "department_name", department_id);
            }
            else
            {
                ViewBag.department_id = new SelectList(dbDepartmentList.GetAllDepartment(), "department_id", "department_name");
            }
            //ViewBag.department_id = new SelectList(dbDepartmentList.GetAllDepartment(), "department_id", "department_name");
            ViewBag.playlist_id = new SelectList(dbPlayList.GetAll(), "playlist_id", "playlist_name");
            return View();
        }

        // POST: BreakTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VMDepartmentPlayListShedule deptPlayListShedule)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    dbManager.Create(deptPlayListShedule);
                    TempData["message"] = true;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.department_id = new SelectList(dbDepartmentList.GetAllDepartment(), "department_id", "department_name");
                    ViewBag.playlist_id = new SelectList(dbPlayList.GetAll(), "playlist_id", "playlist_name");
                }
            }
            catch (Exception ex)
            {
                new SessionManager(Session).error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }

            return View(deptPlayListShedule);
        }

        public JsonResult CreateDeptPlayList(VMDepartmentPlayListShedule deptPlayListShedule)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dbManager.Create(deptPlayListShedule);
                    TempData["message"] = true;
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                    //return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.department_id = new SelectList(dbDepartmentList.GetAllDepartment(), "department_id", "department_name");
                    ViewBag.playlist_id = new SelectList(dbPlayList.GetAll(), "playlist_id", "playlist_name");
                }
            }
            catch (Exception ex)
            {
                new SessionManager(Session).error = ex;
                //return RedirectToAction("Index", "ErrorHandler");
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            //return View(playListSheduling);
        }

        //GET: BreakTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VMDepartmentPlayListShedule deptPlayListShedule = dbManager.GetById(id.Value);
            if (deptPlayListShedule == null)
            {
                return HttpNotFound();
            }
            ViewBag.department_id = new SelectList(dbDepartmentList.GetAllDepartment(), "department_id", "department_name", deptPlayListShedule.department_id);
            ViewBag.playlist_id = new SelectList(dbPlayList.GetAll(), "playlist_id", "playlist_name", deptPlayListShedule.PlayList_id);
            return View(deptPlayListShedule);
        }

        //POST: BreakTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VMDepartmentPlayListShedule deptPlayListShedule)
        {
            if (ModelState.IsValid)
            {
                dbManager.Edit(deptPlayListShedule);
                TempData["mgs"] = true;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.department_id = new SelectList(dbDepartmentList.GetAllDepartment(), "department_id", "department_name", deptPlayListShedule.department_id);
                ViewBag.playlist_id = new SelectList(dbPlayList.GetAll(), "playlist_id", "playlist_name", deptPlayListShedule.PlayList_id);
            }
            return View(deptPlayListShedule);
        }

        // GET: BreakTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VMDepartmentPlayListShedule deptPlayListShedule = dbManager.GetById(id.Value);
            if (deptPlayListShedule == null)
            {
                return HttpNotFound();
            }
            return View(deptPlayListShedule);
        }

        // POST: BreakTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VMDepartmentPlayListShedule deptPlayListShedule = dbManager.GetById(id);
            dbManager.Remove(id);
            return RedirectToAction("Index");
        }

        public ActionResult Approved(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VMDepartmentPlayListShedule deptPlayListShedule = dbManager.GetById(id);
            if (deptPlayListShedule == null)
            {
                return HttpNotFound();
            }
            deptPlayListShedule.is_active = 1;
            dbManager.Approve(deptPlayListShedule);
            //NotifyDisplay.SendMessages(tblDevice.department_id, tblDevice.device_no, "", false, true, false, false);
            return RedirectToAction("Index");
        }

        public ActionResult DeptPlayListTime(int department_playlist_id, int department_id,int playList_id)
        {
            
            ViewBag.department_playlist_id = department_playlist_id;
            ViewBag.department_id = department_id;
            ViewBag.playList_id = playList_id;
            //return PartialView();

            return PartialView("DeptPlayListTime");
        }

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
