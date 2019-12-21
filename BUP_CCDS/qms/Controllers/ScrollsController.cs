using qms.BLL;
using qms.Models;
using qms.SignalRHub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using qms.Utility;
using System.Web.Script.Serialization;

namespace qms.Controllers
{
    [AuthorizationFilter(Roles = "Admin, Department Admin")]
    public class ScrollsController : Controller
    {
        private BLLScroll dbManager = new BLLScroll();
        // GET: Scrolls
        public ActionResult Index()
        {
            return View(dbManager.GetAll());
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
        public ActionResult Create([Bind(Include = "scroll_id,content_en,content_bn,is_global,is_active")] tblScroll scroll)
        {
            scroll.is_global = 0;
            if (ModelState.IsValid)
            {
                //db.tblDepartments.Add(tblDepartment);

                //await db.SaveChangesAsync();
                dbManager.Create(scroll);

                //DisplayManager dm = new Utility.DisplayManager();
                //if (!String.IsNullOrEmpty(tblDepartment.static_ip))
                //    dm.CreateTextFile(tblDepartment.department_id, tblDepartment.static_ip);

                //NotifyDisplay.SendMessages(0, "", "", false, false, false, true);
                TempData["message"] = true;
                return RedirectToAction("Index");
            }

            return View(scroll);
        }

        // GET: Departments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblScroll scroll = dbManager.GetById(id.Value);
            if (scroll == null)
            {
                return HttpNotFound();
            }
            return View(scroll);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "scroll_id,content_en,content_bn,is_global, is_active")] tblScroll scroll)
        {
            if (ModelState.IsValid)
            {
                dbManager.Edit(scroll);
                TempData["mgs"] = true;
                return RedirectToAction("Index");
            }
            return View(scroll);
        }

        // GET: Departments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblScroll scroll = dbManager.GetById(id.Value);
            if (scroll == null)
            {
                return HttpNotFound();
            }
           
            return View(scroll);
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
                tblScroll scroll = dbManager.GetById(id);
                if (scroll == null)
                {
                    return HttpNotFound();
                }
                dbManager.Remove(scroll.scroll_id);
            }
            catch (Exception)
            {
                TempData["mgsDelete"] = false;
            }
            
            return RedirectToAction("Index");
        }

        


        // GET: Departments/Edit/5
        public ActionResult SetGlobal(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblScroll scroll = dbManager.GetById(id.Value);
            if (scroll == null)
            {
                return HttpNotFound();
            }
            scroll.is_global = 1;
            dbManager.Edit(scroll);

            return RedirectToAction("Index");
        }

        public ActionResult PublishToAll(int? id, string btnName)
        {
            tblScroll scroll = dbManager.GetById(id.Value);
            if (scroll == null)
            {
                return HttpNotFound();
            }
            SetActivationStatus(id, btnName);
            dbManager.Force(0, scroll);
            NotifyDisplay.SendMessages(0, "", "", false, false, false, true, false, false);

            return RedirectToAction("Index");
        }


        public JsonResult PublishToAllScroll(int? id, string btnName)
        {
            tblScroll scroll = dbManager.GetById(id.Value);
            if (scroll == null)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            SetActivationStatus(id, btnName);
            dbManager.Force(0, scroll);
            NotifyDisplay.SendMessages(0, "", "", false, false, false, true, false, false);

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult SetActivationStatus(int? id, string btnName)
        {
            string responseJson = String.Empty;
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                tblScroll tblScroll = dbManager.GetById(id.Value);
                if (tblScroll == null)
                {
                    return HttpNotFound();
                }
                if (btnName == "ScrollON")
                {
                    tblScroll.is_active = 1;
                }
                else
                {
                    tblScroll.is_active = 0;
                }
                //tblScroll.is_active = (tblScroll.is_active == 0 ? 1 : 0);
                dbManager.Edit(tblScroll);

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