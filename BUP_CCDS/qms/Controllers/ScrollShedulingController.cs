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
    public class ScrollShedulingController : Controller
    {
        private BLL.BLLScroll dbScroll = new BLL.BLLScroll();
        private BLL.BLLScrollSheduling dbManager = new BLL.BLLScrollSheduling();

        public ActionResult Index()
        {
            //dbManager.GetAll()
            return View(dbManager.GetAll());
        }

        // GET: BreakTypes/Create
        public ActionResult Create()
        {
            ViewBag.scroll_id = new SelectList(dbScroll.GetAll(), "scroll_id", "content_bn");
            return View();
        }

        // POST: BreakTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "scroll_Sheduling_id,scroll_id,when_start,duration,bool_is_active")] VMScrollSheduling scrollSheduling)
        {

                try
            {
                if (ModelState.IsValid)
                {
                    dbManager.Create(scrollSheduling);
                    TempData["message"] = true;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.scroll_id = new SelectList(dbScroll.GetAll(), "scroll_id", "content_bn");
                }
            }
            catch (Exception ex)
            {
                new SessionManager(Session).error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
        return View(scrollSheduling);
        }

        public JsonResult CreateScroll(VMScrollSheduling scrollSheduling)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    dbManager.Create(scrollSheduling);
                    TempData["message"] = true;
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                    //return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.scroll_id = new SelectList(dbScroll.GetAll(), "scroll_id", "content_bn");
                }
            }
            catch (Exception ex)
            {
                new SessionManager(Session).error = ex;
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
            VMScrollSheduling scrollSheduling = dbManager.GetById(id.Value);
            if (scrollSheduling == null)
            {
                return HttpNotFound();
            }
            ViewBag.scroll_id = new SelectList(dbScroll.GetAll(), "scroll_id", "content_bn", scrollSheduling.scroll_id);
            return View(scrollSheduling);
        }

        //POST: BreakTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "scroll_Sheduling_id,scroll_id,when_start,duration,bool_is_active")] VMScrollSheduling scrollSheduling)
        {
            if (ModelState.IsValid)
            {
                dbManager.Edit(scrollSheduling);
                TempData["mgs"] = true;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.scroll_id = new SelectList(dbScroll.GetAll(), "scroll_id", "content_bn", scrollSheduling.scroll_id);
            }
            return View(scrollSheduling);
        }

        // GET: BreakTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VMScrollSheduling scrollSheduling = dbManager.GetById(id.Value);
            if (scrollSheduling == null)
            {
                return HttpNotFound();
            }
            return View(scrollSheduling);
        }

        // POST: BreakTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VMScrollSheduling scrollSheduling = dbManager.GetById(id);
            dbManager.Remove(id);
            return RedirectToAction("Index");
        }
        public ActionResult Approved(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VMScrollSheduling scrollSheduling = dbManager.GetById(id.Value);
            if (scrollSheduling == null)
            {
                return HttpNotFound();
            }
            scrollSheduling.is_active = 1;
            dbManager.Approve(scrollSheduling);
            //NotifyDisplay.SendMessages(tblDevice.department_id, tblDevice.device_no, "", false, true, false, false);
            return RedirectToAction("Index");
        }
        public ActionResult SetScrollTime(int scroll_id)
        {
            ViewBag.scroll_id = scroll_id;
            //return PartialView();

            return PartialView("ScrollTime");
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
