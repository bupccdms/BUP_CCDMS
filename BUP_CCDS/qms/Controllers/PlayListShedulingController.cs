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
    public class PlayListShedulingController : Controller
    {
        private BLL.BLLPlayList dbPlayList = new BLL.BLLPlayList();
        private BLL.BLLPlayListSheduling dbManager = new BLL.BLLPlayListSheduling();

        public ActionResult Index()
        {
            //dbManager.GetAll()
            return View(dbManager.GetAll());
        }

        // GET: BreakTypes/Create
        public ActionResult Create()
        {
            ViewBag.playlist_id = new SelectList(dbPlayList.GetAll(), "playlist_id", "playlist_name");
            return View();
        }

        // POST: BreakTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PlayList_Sheduling_id,PlayList_id,when_start,duration,bool_is_active")] VMPlayListSheduling playListSheduling)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    dbManager.Create(playListSheduling);
                    TempData["message"] = true;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.playlist_id = new SelectList(dbPlayList.GetAll(), "playlist_id", "playlist_name");
                }
            }
            catch (Exception ex)
            {
                new SessionManager(Session).error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }

            return View(playListSheduling);
        }
      

        public JsonResult CreatePlayList(VMPlayListSheduling playListSheduling)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    dbManager.Create(playListSheduling);
                    TempData["message"] = true;
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                    //return RedirectToAction("Index");
                }
                else
                {
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
            VMPlayListSheduling playListSheduling = dbManager.GetById(id.Value);
            if (playListSheduling == null)
            {
                return HttpNotFound();
            }
            ViewBag.playlist_id = new SelectList(dbPlayList.GetAll(), "playlist_id", "playlist_name", playListSheduling.PlayList_id);
            return View(playListSheduling);
        }

        //POST: BreakTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PlayList_Sheduling_id,PlayList_id,when_start,duration,bool_is_active")] VMPlayListSheduling playListSheduling)
        {
            if (ModelState.IsValid)
            {
                dbManager.Edit(playListSheduling);
                TempData["mgs"] = true;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.playlist_id = new SelectList(dbPlayList.GetAll(), "playlist_id", "playlist_name", playListSheduling.PlayList_id);
            }
            return View(playListSheduling);
        }

        // GET: BreakTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VMPlayListSheduling playListSheduling = dbManager.GetById(id.Value);
            if (playListSheduling == null)
            {
                return HttpNotFound();
            }
            return View(playListSheduling);
        }

        // POST: BreakTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VMPlayListSheduling playListSheduling = dbManager.GetById(id);
            dbManager.Remove(id);
            return RedirectToAction("Index");
        }

        public ActionResult Approved(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VMPlayListSheduling playListSheduling = dbManager.GetById(id);
            if (playListSheduling == null)
            {
                return HttpNotFound();
            }
            playListSheduling.is_active = 1;
            dbManager.Approve(playListSheduling);
            //NotifyDisplay.SendMessages(tblDevice.department_id, tblDevice.device_no, "", false, true, false, false);
            return RedirectToAction("Index");
        }

        public ActionResult PlayListTime(int playlist_id)
        {
            ViewBag.playlist_id = playlist_id;
            //return PartialView();

            return PartialView("PlayListTime");
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
