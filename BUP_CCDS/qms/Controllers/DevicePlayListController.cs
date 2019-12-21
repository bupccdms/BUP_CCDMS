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
    public class DevicePlayListController : Controller
    {
        private BLLDevices dbdevice = new BLLDevices();
        private BLLDevicePlayList dbManager = new BLLDevicePlayList();
        private BLL.BLLDepartment dbDepartment = new BLL.BLLDepartment();
        // GET: DisplayFooters
        public ActionResult Index()
        {
            ViewBag.departmentList = dbDepartment.GetAllDepartment();
            SessionManager sm = new SessionManager(Session);
            ViewBag.userDepartmentId = sm.department_id;
            int department_id = sm.department_id;
            if (department_id > 0)
            {
                return View(dbManager.GetAll().Where(u => u.department_id == department_id));
            }
            return View(dbManager.GetAll());
        }


        // GET: Branches/Create
        public ActionResult Create()
        {
            List<tblDepartment> departmentList = new BLLDepartment().GetAllDepartment().Where(x => x.is_active == 1).ToList();
            List<tblDevice> deviceList = new BLLDevices().GetAllDevice();
            List<VMDevicePlayList> devicePlayList = dbManager.GetAll();
            SessionManager sm = new SessionManager(Session);


            var qry = deviceList.GroupJoin(
                      devicePlayList,
                      b => b.device_id,
                      d => d.device_id,
                      (x, y) => new { deviceList = x, devicePlayList = y })
                      .Where(w => w.devicePlayList.Count() == 0)
                      .Select(
                      x => x.deviceList).ToList();

            var dqry = departmentList.GroupJoin(
                         devicePlayList,
                         b => b.department_id,
                         d => d.department_id,
                         (x, y) => new { departmentList = x, devicePlayList = y })
                         .Where(w => w.devicePlayList.Count() == 0)
                         .Select(
                         x => x.departmentList).ToList();
            int department_id = new SessionManager(Session).department_id;
            if (department_id > 0)
            {
                List<tblDevice> deviceListForDept;
                deviceListForDept = new BLLDevices().GetAllDevice().Where(u => u.department_id == department_id).ToList();
                var qryy = deviceListForDept.GroupJoin(
                      devicePlayList,
                      b => b.device_id,
                      d => d.device_id,
                      (x, y) => new { deviceList = x, devicePlayList = y })
                      .Where(w => w.devicePlayList.Count() == 0)
                      .Select(
                      x => x.deviceList).ToList();
                ViewBag.device_id = new SelectList(qryy, "device_id", "name");
            }
            else
            {
                ViewBag.device_id = new SelectList(qry, "device_id", "name");
            }



            //ViewBag.device_id = new SelectList(qry, "device_id", "name");
            //ViewBag.device_id = new SelectList(qry.Select(p => new { device_id = p.device_id, device_no = p.device_name + " " + p.device_no }));
            ViewBag.department_id = new SelectList(departmentList.Where(x => x.is_active == 1), "department_id", "department_name",department_id);
            ViewBag.playlist_id = new SelectList(new BLLPlayList().GetAll(), "playlist_id", "playlist_name");
            return View();
        }

        // POST: Branches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "device_playlist_id, device_id, playlist_id,is_global")] VMDevicePlayList playList)
        {
            playList.is_global = 0;
            if (ModelState.IsValid)
            {
                dbManager.Create(playList);
                TempData["message"] = true;
                return RedirectToAction("Index");
            }

            List<tblDevice> deviceList = new BLLDevices().GetAllDevice();
            List<VMDevicePlayList> devicePlayList = dbManager.GetAll();

            List<tblDepartment> departmentList = new BLLDepartment().GetAllDepartment().Where(x => x.is_active == 1).ToList();

            var qry = deviceList.GroupJoin(
                      devicePlayList,
                      b => b.device_id,
                      d => d.device_id,
                      (x, y) => new { deviceList = x, devicePlayList = y })
                      .Where(w => w.devicePlayList.Count() == 0)
                      .Select(
                      x => x.deviceList).ToList();
            int department_id = new SessionManager(Session).department_id;
            if (department_id > 0)
            {
                List<tblDevice> deviceListForDept;
                deviceListForDept = new BLLDevices().GetAllDevice().Where(u => u.department_id == department_id).ToList();
                var qryy = deviceListForDept.GroupJoin(
                      devicePlayList,
                      b => b.device_id,
                      d => d.device_id,
                      (x, y) => new { deviceList = x, devicePlayList = y })
                      .Where(w => w.devicePlayList.Count() == 0)
                      .Select(
                      x => x.deviceList).ToList();
                ViewBag.device_id = new SelectList(qryy, "device_id", "name");
            }
            else
            {
                ViewBag.device_id = new SelectList(qry, "device_id", "name");
            }

            //ViewBag.device_id = new SelectList(qry, "device_id", "device_name");
            ViewBag.playlist_id = new SelectList(new BLLPlayList().GetAll(), "playlist_id", "playlist_name");
            ViewBag.department_id = new SelectList(departmentList.Where(x => x.is_active == 1), "department_id", "department_name", department_id);
            return View(playList);
        }

        // GET: Branches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VMDevicePlayList devicePlayList = dbManager.GetById(id.Value);
            if (devicePlayList == null)
            {
                return HttpNotFound();
            }
            ViewBag.playlist_id = new SelectList(new BLLPlayList().GetAll(), "playlist_id", "playlist_name", devicePlayList.playlist_id);
            return View(devicePlayList);
        }

        // POST: Branches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "device_playlist_id, device_id, playlist_id,is_global,is_active")] VMDevicePlayList devicePlayList)
        {
            if (ModelState.IsValid)
            {
                //devicePlayList.is_active = (devicePlayList.is_active == 0 ? 1 : 0);
                dbManager.Edit(devicePlayList);
                TempData["mgs"] = true;
                return RedirectToAction("Index");
            }
            ViewBag.playlist_id = new SelectList(new BLLPlayList().GetAll(), "playlist_id", "playlist_name", devicePlayList.playlist_id);
            return View(devicePlayList);
        }

        // GET: Branches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VMDevicePlayList devicePlayList = dbManager.GetById(id.Value);
            if (devicePlayList == null)
            {
                return HttpNotFound();
            }
            return View(devicePlayList);
        }

        // POST: Branches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VMDevicePlayList devicePlayList = dbManager.GetById(id);
            if (devicePlayList == null)
            {
                return HttpNotFound();
            }
            dbManager.Remove(id);
            NotifyDisplay.SendMessages(devicePlayList.department_id, devicePlayList.device_id.ToString(), "", false, false, true, false, false, false);

            return RedirectToAction("Index");
        }

        public ActionResult Publish(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VMDevicePlayList devicePlayList = dbManager.GetById(id.Value);
            if (devicePlayList == null)
            {
                return HttpNotFound();
            }
            devicePlayList.is_active = 1;
            dbManager.Edit(devicePlayList);


            NotifyDisplay.SendMessages(devicePlayList.department_id, devicePlayList.device_id.ToString(), "", false, false, true, false, false, false);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public JsonResult DeviceLoadbyDepartmetId(int departmentId)
        {
            List<tblDevice> deviceList = dbdevice.GetAllDevicebyDepartment(departmentId);
            List<VMDevicePlayList> devicePlayList = dbManager.GetAll();



            var qry = deviceList.GroupJoin(
                      devicePlayList,
                      b => b.device_id,
                      d => d.device_id,
                      (x, y) => new { deviceList = x, devicePlayList = y })
                      .Where(w => w.devicePlayList.Count() == 0)
                      .Select(
                      x => x.deviceList).ToList();

            return Json(new { Success = true, deviceList = qry }, JsonRequestBehavior.AllowGet);
        }

    }
}