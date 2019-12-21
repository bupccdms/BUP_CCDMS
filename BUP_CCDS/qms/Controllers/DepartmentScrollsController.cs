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
using System.Web.Script.Serialization;

namespace qms.Controllers
{
    [AuthorizationFilter(Roles = "Admin, Department Admin")]
    public class DepartmentScrollsController : Controller
    {
        private BLLDepartmentScroll dbManager = new BLLDepartmentScroll();
        // GET: Scrolls
        public ActionResult Index()
        {
            try
            {
                SessionManager sm = new SessionManager(Session);
                int department_id = sm.department_id;
                if (department_id > 0)
                {
                    return View(dbManager.GetAll().Where(u => u.department_id == department_id));
                }
                return View(dbManager.GetAll());
            }

            catch (Exception ex)
            {
                new SessionManager(Session).error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }

        }


        // GET: Departments/Create
        public ActionResult Create()
        {
            try
            {
                List<tblDepartment> departmentList = new BLLDepartment().GetAllDepartment().Where(a=>a.is_active==1).ToList();
                List<VMDepartmentScroll> departmentScrollList = dbManager.GetAll();

                var qry = departmentList.GroupJoin(
                          departmentScrollList,
                          b => b.department_id,
                          d => d.department_id,
                          (x, y) => new { departmentList = x, departmentScrollList = y })
                          .Where(w => w.departmentScrollList.Count() == 0)
                          .Select(
                          x => x.departmentList).ToList();

                int department_id = new SessionManager(Session).department_id;
                if (department_id > 0)
                {
                    List<tblDepartment> deviceListForDept;
                    deviceListForDept = new BLLDepartment().GetAllDepartment().ToList();
                    var qryy = deviceListForDept.GroupJoin(
                          departmentScrollList,
                          b => b.department_id,
                          d => d.department_id,
                          (x, y) => new { departmentList = x, departmentScrollList = y })
                          .Where(w => w.departmentScrollList.Count() == 0)
                          .Select(
                          x => x.departmentList).ToList();
                    ViewBag.department_id = new SelectList(departmentList, "department_id", "department_name", department_id);
                }
                else
                {
                    ViewBag.department_id = new SelectList(qry, "department_id", "department_name");
                }

                //ViewBag.department_id = new SelectList(qry, "department_id", "department_name");
                ViewBag.scroll_id = new SelectList(new BLLScroll().GetAll(), "scroll_id", "content_bn");
                return View();
            }
            catch (Exception ex)
            {
                new SessionManager(Session).error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }

        }

        // POST: Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "department_scroll_id, department_id, scroll_id")] VMDepartmentScroll departmentScroll)
        {
            try
            {
                SessionManager sm = new SessionManager(Session);
                if (ModelState.IsValid)
                {
                    if (departmentScroll.department_id == 0)
                    {
                        departmentScroll.department_id = sm.department_id;
                        dbManager.Create(departmentScroll);
                    }
                    else
                    {
                        dbManager.Create(departmentScroll);
                    }

                    TempData["message"] = true;
                    return RedirectToAction("Index");
                }

                List<tblDepartment> departmentList = new BLLDepartment().GetAllDepartment().Where(a => a.is_active == 1).ToList(); ;
                List<VMDepartmentScroll> departmentScrollList = dbManager.GetAll();
                var qry = departmentList.GroupJoin(
                          departmentScrollList,
                          b => b.department_id,
                          d => d.department_id,
                          (x, y) => new { departmentList = x, departmentScrollList = y })
                          .Where(w => w.departmentScrollList.Count() == 0)
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
                ViewBag.scroll_id = new SelectList(new BLLScroll().GetAll(), "scroll_id", "content_en");
                return View(departmentScroll);
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
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                VMDepartmentScroll departmentScroll = dbManager.GetById(id.Value);
                if (departmentScroll == null)
                {
                    return HttpNotFound();
                }
                ViewBag.scroll_id = new SelectList(new BLLScroll().GetAll(), "scroll_id", "content_bn", departmentScroll.scroll_id);
                return View(departmentScroll);
            }
            catch (Exception ex)
            {
                new SessionManager(Session).error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }

        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "department_scroll_id, department_id, scroll_id, is_active,is_publish")] VMDepartmentScroll departmentScroll)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dbManager.Edit(departmentScroll);
                    TempData["mgs"] = true;
                    return RedirectToAction("Index");
                }
                ViewBag.scroll_id = new SelectList(new BLLScroll().GetAll(), "scroll_id", "content_bn", departmentScroll.scroll_id);
                return View(departmentScroll);
            }
            catch (Exception ex)
            {
                new SessionManager(Session).error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }

        }

        // GET: Departments/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                VMDepartmentScroll departmentScroll = dbManager.GetById(id.Value);
                if (departmentScroll == null)
                {
                    return HttpNotFound();
                }
                return View(departmentScroll);
            }
            catch (Exception ex)
            {
                new SessionManager(Session).error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }

        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                if (id == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                VMDepartmentScroll departmentScroll = dbManager.GetById(id);
                if (departmentScroll == null)
                {
                    return HttpNotFound();
                }
                dbManager.Remove(departmentScroll.department_scroll_id);
                NotifyDisplay.SendMessages(departmentScroll.department_id, "", "", false, false, false, true, false, false);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                new SessionManager(Session).error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }

        }
        public ActionResult PublishToAll(int id, string btnName, int? deptScrollId)
        {
           
            int department_scroll_id = new BLLDepartmentScroll().GetAll().Where(x => x.department_id == id).FirstOrDefault().department_scroll_id;
            VMDepartmentScroll departmentScroll = dbManager.GetById(department_scroll_id);
            if (departmentScroll == null)
            {
                return HttpNotFound();
            }
            //departmentScroll.is_publish = (departmentScroll.is_publish == 0 ? 1 : 0);
            if (btnName == "ScrollON")
            {
                departmentScroll.is_publish = 1;
            }
            else
            {
                departmentScroll.is_publish = 0;
            }
            dbManager.Edit(departmentScroll);
            SetActivationStatus(deptScrollId, btnName);
            new BLLScroll().Force(id, null);
            NotifyDisplay.SendMessages(departmentScroll.department_id, "", "", false, false, false, true, false, false);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SetActivationStatus(int? deptScrollId, string btnName)
        {
            string responseJson = String.Empty;
            try
            {
                if (deptScrollId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                VMDepartmentScroll departmentScroll = dbManager.GetById(deptScrollId.Value);
                if (departmentScroll == null)
                {
                    return HttpNotFound();
                }
                if (btnName == "ScrollON")
                {
                    departmentScroll.is_active = 1;
                }
                else
                {
                    departmentScroll.is_active = 0;
                }
                //departmentScroll.is_active = (departmentScroll.is_active == 0 ? 1 : 0);
                dbManager.Edit(departmentScroll);

                JsonResult json = Json(new { success = true, Message = "Success" }, JsonRequestBehavior.AllowGet);
                responseJson = new JavaScriptSerializer().Serialize(json.Data);
                return json;
            }
            catch (Exception ex)
            {

                JsonResult json = Json(new { success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
                responseJson = new JavaScriptSerializer().Serialize(json.Data);
                return json;
            }



        }


    }
}