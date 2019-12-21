using qms.BLL;
//using qms.Models;
using qms.SignalRHub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using qms.Utility;
using qms.ViewModels;

namespace qms.Controllers
{
    [AuthorizationFilter(Roles = "Admin, Department Admin")]
    public class CustomContentController : Controller
    {
        private BLLCustomContent dbManager = new BLLCustomContent();
        // GET: Scrolls
        public ActionResult Index()
        {
            return View(dbManager.GetAll());
        }

        public ActionResult List()
        {
            return PartialView(dbManager.GetAll());
        }


        // GET: Departments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "custom_content_id,is_url,content,url")] VMCustomContent customContent)
        {
            //displayFooter.is_global = 0;
            if (ModelState.IsValid)
            {
                customContent.is_url = (customContent.url != null ? 1 : 0);
                dbManager.Create(customContent);
                TempData["message"] = true;
                return RedirectToAction("Index");
            }

            return View(customContent);
        }

        // GET: Departments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VMCustomContent customContent = dbManager.GetById(id.Value);
            if (customContent == null)
            {
                return HttpNotFound();
            }
            return View(customContent);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "custom_content_id,is_url,content,url")] VMCustomContent customContent)
        {
            if (ModelState.IsValid)
            {
                customContent.is_url = (customContent.url != null ? 1 : 0);
                dbManager.Edit(customContent);
                TempData["mgs"] = true;
                return RedirectToAction("Index");
            }
            return View(customContent);
        }

        // GET: Departments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VMCustomContent customContent = dbManager.GetById(id.Value);
            if (customContent == null)
            {
                return HttpNotFound();
            }

            //var departmentScroll = new BLLDepartmentScroll().GetAll().Where(w => w.scroll_id == displayFooter.scroll_id).FirstOrDefault();
            //if (departmentScroll != null)
                //NotifyDisplay.SendMessages(departmentScroll.department_id, "", "", false, false, false, true);

            return View(customContent);
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
            VMCustomContent customContent = dbManager.GetById(id);
            if (customContent == null)
            {
                return HttpNotFound();
            }
            dbManager.Remove(customContent.custom_content_id);
            //NotifyDisplay.SendMessages(0, "", "", false, false, false, true);

            return RedirectToAction("Index");
        }

        // GET: Departments/Edit/5
        public ActionResult SetNational(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VMCustomContent customContent = dbManager.GetById(id.Value);
            if (customContent == null)
            {
                return HttpNotFound();
            }
            //customContent.is_global = 1;
            dbManager.Edit(customContent);
            NotifyDisplay.SendMessages(0, "", "", false, false, false, true, false, false);

            return RedirectToAction("Index");
        }
    }
}