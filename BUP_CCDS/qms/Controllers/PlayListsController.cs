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

namespace qms.Controllers
{
    [AuthorizationFilter(Roles = "Admin, Department Admin")]
    public class PlayListsController : Controller
    {
        private BLLPlayList dbManager = new BLLPlayList();
        // GET: Scrolls
        public ActionResult Index()
        {
            return View(dbManager.GetAll().OrderByDescending(x=>x.playlist_id));
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
        public ActionResult Create([Bind(Include = "playlist_id,playlist_name,is_global")] tblPlayList playList)
        {
            SessionManager sm = new SessionManager(Session);
            playList.userId = sm.user_id;
            playList.is_global = 0;
            if (ModelState.IsValid)
            {
                //db.tblDepartments.Add(tblDepartment);

                //await db.SaveChangesAsync();
                dbManager.Create(playList);

                TempData["message"] = true;
                return RedirectToAction("Index");
            }

            return View(playList);
        }

        // GET: Departments/Delete/5
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPlayList playList = dbManager.GetById(id.Value);
            if (playList == null)
            {
                return HttpNotFound();
            }
            return View(playList);
        }


        // GET: Departments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPlayList playList = dbManager.GetById(id.Value);
            if (playList == null)
            {
                return HttpNotFound();
            }
            return View(playList);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "playlist_id,playlist_name,is_global")] tblPlayList playList)
        {
            if (ModelState.IsValid)
            {
                dbManager.Edit(playList);
                TempData["mgs"] = true;
                return RedirectToAction("Index");
            }
            return View(playList);
        }

        // GET: Departments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPlayList playList = dbManager.GetById(id.Value);
            if (playList == null)
            {
                return HttpNotFound();
            }
            return View(playList);
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
                tblPlayList playList = dbManager.GetById(id);
                if (playList == null)
                {
                    return HttpNotFound();
                }
                dbManager.Remove(playList.playlist_id);
                NotifyDisplay.SendMessages(0, "", "", false, false, true, false, false, false);

            }
            catch (Exception)
            {
                TempData["mgsDelete"] = false;
            }
            return RedirectToAction("Index");


        }

        //Should be post method
        // GET: Departments/Edit/5
        public ActionResult SetGlobal(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPlayList playList = dbManager.GetById(id.Value);
            if (playList == null)
            {
                return HttpNotFound();
            }
            playList.is_global = 1;
            dbManager.Edit(playList);
            
            return RedirectToAction("Index");
        }


        //Should be post method
        public ActionResult PublishToAll(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPlayList playList = dbManager.GetById(id);
            if (playList == null)
            {
                return HttpNotFound();
            }
            dbManager.Force(0, playList);
            NotifyDisplay.SendMessages(0, "", "", false, false, true, false, false, false);

            return RedirectToAction("Index");
        }

        public JsonResult PublishToAllPlayList(int id)
        {
            if (id == 0)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            tblPlayList playList = dbManager.GetById(id);
            if (playList == null)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            dbManager.Force(0, playList);
            NotifyDisplay.SendMessages(0, "", "", false, false, true, false, false, false);

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

    }
}