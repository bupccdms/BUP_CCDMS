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
    [AuthorizationFilter(Roles = "Admin")]
    public class LogoutTypesController : Controller
    {
        //private qmsEntities db = new qmsEntities();
        private BLL.BLLLogoutType dbManager = new BLL.BLLLogoutType();
        // GET: LogoutTypes
        public ActionResult Index()
        {
            return View(dbManager.GetAll());
        }

        // GET: LogoutTypes/Details/5
        //public async Task<ActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    tblLogoutType tblLogoutType = await db.tblLogoutTypes.FindAsync(id);
        //    if (tblLogoutType == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tblLogoutType);
        //}

        // GET: LogoutTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LogoutTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "logout_type_id,logout_type_name,bool_has_free_text,bool_is_active")] tblLogoutType tblLogoutType)
        {
            if (ModelState.IsValid)
            {
                dbManager.Create(tblLogoutType);
                return RedirectToAction("Index");
            }

            return View(tblLogoutType);
        }

        // GET: LogoutTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblLogoutType tblLogoutType = dbManager.GetById(id.Value);
            if (tblLogoutType == null)
            {
                return HttpNotFound();
            }
            return View(tblLogoutType);
        }

        // POST: LogoutTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "logout_type_id,logout_type_name,bool_has_free_text,bool_is_active")] tblLogoutType tblLogoutType)
        {
            if (ModelState.IsValid)
            {
                dbManager.Edit(tblLogoutType);
                return RedirectToAction("Index");
            }
            return View(tblLogoutType);
        }

        // GET: LogoutTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblLogoutType tblLogoutType = dbManager.GetById(id.Value);
            if (tblLogoutType == null)
            {
                return HttpNotFound();
            }
            return View(tblLogoutType);
        }

        // POST: LogoutTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblLogoutType tblLogoutType = dbManager.GetById(id);
            dbManager.Remove(id);
            return RedirectToAction("Index");
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
