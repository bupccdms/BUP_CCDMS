using qms.BLL;
using qms.Models;
using qms.SignalRHub;
using qms.Utility;
using qms.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace qms.Controllers
{
    [AuthorizationFilter(Roles = "Admin, Department Admin")]
    public class DepartmentPlayListController : Controller
    {
        private BLLDepartmentPlayList dbManager = new BLLDepartmentPlayList();
        // GET: Scrolls
        public ActionResult Index()
        {
            SessionManager sm = new SessionManager(Session);
            int department_id = sm.department_id;
            if (department_id > 0)
            {
                return View(dbManager.GetAll().Where(u => u.department_id == department_id));
            }
            return View(dbManager.GetAll());
        }


        // GET: Departments/Create
        public ActionResult Create()
        {
            List<tblDepartment> departmentList = new BLLDepartment().GetAllDepartment().Where(a => a.is_active == 1).ToList(); ;
            List<VMDepartmentPlayList> departmentPlayList = dbManager.GetAll();



            var qry = departmentList.GroupJoin(
                      departmentPlayList,
                      b => b.department_id,
                      d => d.department_id,
                      (x, y) => new { departmentList = x, departmentPlayList = y })
                      .Where(w=>w.departmentPlayList.Count()==0)
                      .Select(
                      x => x.departmentList).ToList();
            int department_id = new SessionManager(Session).department_id;
            if (department_id > 0)
            {
                ViewBag.department_id = new SelectList(departmentList, "department_id", "department_name", department_id);
            }
            else
            {
                ViewBag.department_id = new SelectList(qry, "department_id", "department_name");
            }


            //ViewBag.department_id = new SelectList(qry, "department_id", "department_name");
            ViewBag.playlist_id = new SelectList(new BLLPlayList().GetAll(), "playlist_id", "playlist_name");
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "department_playlist_id, department_id, playlist_id, bool_is_priority")] VMDepartmentPlayList playList)
        {
            try
            {
                SessionManager sm = new SessionManager(Session);
                if (ModelState.IsValid)
                {
                    if (playList.department_id == 0)
                    {
                        playList.department_id = sm.department_id;
                        dbManager.Create(playList);
                    }
                    else
                    {
                        dbManager.Create(playList);
                    }

                    TempData["message"] = true;
                    return RedirectToAction("Index");
                }

                List<tblDepartment> departmentList = new BLLDepartment().GetAllDepartment().Where(a => a.is_active == 1).ToList(); ;
                List<VMDepartmentPlayList> departmentPlayList = dbManager.GetAll();



                var qry = departmentList.GroupJoin(
                          departmentPlayList,
                          b => b.department_id,
                          d => d.department_id,
                          (x, y) => new { departmentList = x, departmentPlayList = y })
                          .Where(w => w.departmentPlayList.Count() == 0)
                          .Select(
                          x => x.departmentList).ToList();
                int department_id = new SessionManager(Session).department_id;
                if (department_id > 0)
                {
                    ViewBag.department_id = new SelectList(departmentList, "department_id", "department_name", department_id);
                }
                else
                {
                    ViewBag.department_id = new SelectList(qry, "department_id", "department_name");
                }
                //ViewBag.department_id = new SelectList(qry, "department_id", "department_name");
                ViewBag.playlist_id = new SelectList(new BLLPlayList().GetAll(), "playlist_id", "playlist_name");
                return View(playList);
            }
            catch (Exception ex)
            {
                new SessionManager(Session).error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
            
        }

        // GET: Departments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VMDepartmentPlayList departmentPlayList = dbManager.GetById(id.Value);
            if (departmentPlayList == null)
            {
                return HttpNotFound();
            }
            ViewBag.playlist_id = new SelectList(new BLLPlayList().GetAll(), "playlist_id", "playlist_name", departmentPlayList.playlist_id);
            return View(departmentPlayList);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "department_playlist_id, department_id, playlist_id,is_publish,bool_is_priority")] VMDepartmentPlayList departmentPlayList)
        {
            if (ModelState.IsValid)
            {
                dbManager.Edit(departmentPlayList);
                TempData["mgs"] = true;
                return RedirectToAction("Index");
            }
            ViewBag.playlist_id = new SelectList(new BLLPlayList().GetAll(), "playlist_id", "playlist_name", departmentPlayList.playlist_id);
            return View(departmentPlayList);
        }

        // GET: Departments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VMDepartmentPlayList departmentPlayList = dbManager.GetById(id.Value);
            if (departmentPlayList == null)
            {
                return HttpNotFound();
            }
            return View(departmentPlayList);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VMDepartmentPlayList departmentPlayList = dbManager.GetById(id);
            if (departmentPlayList == null)
            {
                return HttpNotFound();
            }
            dbManager.Remove(id);
            NotifyDisplay.SendMessages(departmentPlayList.department_id, "", "", false, false, true, false, false, false);

            return RedirectToAction("Index");
        }

        public ActionResult PublishToAll(int id)
        {
            
            int department_playlist_id = new BLLDepartmentPlayList().GetAll().Where(x => x.department_id == id).FirstOrDefault().department_playlist_id;
            VMDepartmentPlayList departmentPlayList = dbManager.GetById(department_playlist_id);
            if (departmentPlayList == null)
            {
                return HttpNotFound();
            }
            departmentPlayList.is_publish = 1;

            dbManager.Edit(departmentPlayList);

            new BLLPlayList().Force(id, null);
            NotifyDisplay.SendMessages(departmentPlayList.department_id, "", "", false, false, true, false, false, false);

            return RedirectToAction("Index");
        }


        public JsonResult PublishToAllDepartmentPlayList(int id)
        {

            int department_playlist_id = new BLLDepartmentPlayList().GetAll().Where(x => x.department_id == id).FirstOrDefault().department_playlist_id;
            VMDepartmentPlayList departmentPlayList = dbManager.GetById(department_playlist_id);
            if (departmentPlayList == null)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            departmentPlayList.is_publish = 1;

            dbManager.Edit(departmentPlayList);

            new BLLPlayList().Force(id, null);
            NotifyDisplay.SendMessages(departmentPlayList.department_id, "", "", false, false, true, false, false, false);

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }


    }
}