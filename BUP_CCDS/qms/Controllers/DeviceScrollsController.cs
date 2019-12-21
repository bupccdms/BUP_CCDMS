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
    public class DeviceScrollsController : Controller
    {
        private BLLDeviceScroll dbManager = new BLLDeviceScroll();
        private BLLDevices dbDevice = new BLLDevices();
        private BLL.BLLDepartment dbDepartment = new BLL.BLLDepartment();

        // GET: Scrolls
        public ActionResult Index()
        {
            try
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
                List<tblDepartment> departmentList = new BLLDepartment().GetAllDepartment().Where(a => a.is_active == 1).ToList();
                List<tblDevice> deviceList = new BLLDevices().GetAllDevice();
                //For Merge All Scroll
                //ViewBag.Scroll = new BLLScroll().GetAll();
                //End
                List<VMDeviceScroll> displayFootersList = dbManager.GetAll();
                //For Merge All Scroll
                //VMDeviceScroll vMDeviceScroll = new VMDeviceScroll();
                //vMDeviceScroll._tblScroll= new BLLScroll().GetAll();
                //End
                var qry = deviceList.GroupJoin(
                          displayFootersList,
                          b => b.device_id,
                          d => d.device_id,
                          (x, y) => new { deviceList = x, displayFootersList = y })
                          .Where(w => w.displayFootersList.Count() == 0)
                          .Select(
                          x => x.deviceList).ToList();

                var dqry = departmentList.GroupJoin(
                         displayFootersList,
                         b => b.department_id,
                         d => d.department_id,
                         (x, y) => new { departmentList = x, displayFootersList = y })
                         .Where(w => w.displayFootersList.Count() == 0)
                         .Select(
                         x => x.departmentList).ToList();

                SessionManager sm = new SessionManager(Session);
                int department_id = sm.department_id;
                if (department_id > 0)
                {
                    List<tblDevice> deviceListForDept;
                    deviceListForDept = new BLLDevices().GetAllDevice().Where(u => u.department_id == department_id).ToList();
                    var qryy = deviceListForDept.GroupJoin(
                         displayFootersList,
                         b => b.device_id,
                         d => d.device_id,
                         (x, y) => new { deviceList = x, displayFootersList = y })
                         .Where(w => w.displayFootersList.Count() == 0)
                         .Select(
                         x => x.deviceList).ToList();
                    ViewBag.department_id = new SelectList(departmentList, "department_id", "department_name", department_id);

                    ViewBag.device_id = new SelectList(qryy, "device_id", "name");
                }
                else
                {
                    ViewBag.department_id = new SelectList(departmentList, "department_id", "department_name");
                    ViewBag.device_id = new SelectList(qry, "device_id", "name");
                }

                //ViewBag.department_id = new SelectList(departmentList, "department_id", "department_name");
                //ViewBag.device_id = new SelectList(qry, "device_id", "name");
                //For Merge All Scroll
                //ViewBag.device_id = new SelectList(deviceList, "device_id", "name");
                //End
                ViewBag.scroll_id = new SelectList(new BLLScroll().GetAll(), "scroll_id", "content_bn");
                //For Merge All Scroll
                //return View(vMDeviceScroll);
                //End
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
        public ActionResult Create(VMDeviceScroll displayFooter)
        {
            try
            {
                //For Merge All Scroll
                //foreach (var item in displayFooter._tblScroll)
                //{
                //    VMDeviceScroll dscroll = new VMDeviceScroll();
                //    displayFooter.scroll_id = item.scroll_id;
                //    displayFooter.status = item.IsSelected?1:0;

                //    dbManager.Create(displayFooter);
                //}


                //NotifyDisplay.SendMessages(displayFooter.department_id, (displayFooter.device_id).ToString(), "", false, false, false, true);
                //TempData["message"] = true;
                //return RedirectToAction("Index");
                //End
                List<tblDepartment> departmentList = new BLLDepartment().GetAllDepartment().Where(a => a.is_active == 1).ToList();
                if (ModelState.IsValid)
                {
                    dbManager.Create(displayFooter);
                    TempData["message"] = true;
                    NotifyDisplay.SendMessages(displayFooter.department_id, (displayFooter.device_id).ToString(), "", false, false, false, true, false, false);
                    return RedirectToAction("Index");
                }
                else
                {
                    
                    //List<VMDeviceScroll> displayFooterList = dbManager.GetAll();
                    //List<tblDevice> devicesList = new BLLDevices().GetAllDevice();
                    //var d_qry = devicesList.GroupJoin(
                    //      displayFooterList,
                    //      b => b.device_id,
                    //      d => d.device_id,
                    //      (x, y) => new { deviceList = x, displayFootersList = y })
                    //      .Where(w => w.displayFootersList.Count() == 0)
                    //      .Select(
                    //      x => x.deviceList).ToList();
                    ViewBag.department_id = new SelectList(departmentList, "department_id", "department_name");
                    //ViewBag.device_id = new SelectList(d_qry, "device_id", "name");
                }

                // List<tblDepartment> departmentList = new BLLDepartment().GetAllDepartment();
                List<tblDevice> deviceList = new BLLDevices().GetAllDevice();
                List<VMDeviceScroll> displayFootersList = dbManager.GetAll();



                var qry = deviceList.GroupJoin(
                          displayFootersList,
                          b => b.device_id,
                          d => d.device_id,
                          (x, y) => new { deviceList = x, displayFootersList = y })
                          .Where(w => w.displayFootersList.Count() == 0)
                          .Select(
                          x => x.deviceList).ToList();
                SessionManager sm = new SessionManager(Session);
                int department_id = sm.department_id;
                if (department_id > 0)
                {
                    deviceList = new BLLDevices().GetAllDevice().Where(u => u.department_id == department_id).ToList();
                    ViewBag.device_id = new SelectList(deviceList, "device_id", "name");
                    ViewBag.department_id = new SelectList(departmentList, "department_id", "department_name", department_id);
                }
                else
                {
                    ViewBag.device_id = new SelectList(qry, "device_id", "name");
                }


                // ViewBag.department_id = new SelectList(qry, "department_id", "department_name");
                //ViewBag.device_id = new SelectList(qry, "device_id", "device_name");
                ViewBag.scroll_id = new SelectList(new BLLScroll().GetAll(), "scroll_id", "content_bn");
                return View(displayFooter);
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
                VMDeviceScroll displayFooter = dbManager.GetById(id.Value);
                if (displayFooter == null)
                {
                    return HttpNotFound();
                }
                ViewBag.scroll_id = new SelectList(new BLLScroll().GetAll(), "scroll_id", "content_bn", displayFooter.scroll_id);
                return View(displayFooter);
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
        public ActionResult Edit([Bind(Include = "device_scroll_id, device_id, scroll_id, is_active")] VMDeviceScroll displayFooter)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dbManager.Edit(displayFooter);
                    TempData["mgs"] = true;
                    return RedirectToAction("Index");
                }
                ViewBag.scroll_id = new SelectList(new BLLScroll().GetAll(), "scroll_id", "content_bn", displayFooter.scroll_id);
                return View(displayFooter);
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
                VMDeviceScroll displayFooter = dbManager.GetById(id.Value);
                if (displayFooter == null)
                {
                    return HttpNotFound();
                }
                return View(displayFooter);
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
                VMDeviceScroll deviceScroll = dbManager.GetById(id);
                if (deviceScroll == null)
                {
                    return HttpNotFound();
                }
                dbManager.Remove(deviceScroll.device_scroll_id);
                NotifyDisplay.SendMessages(deviceScroll.department_id, deviceScroll.device_id.ToString(), "", false, false, false, true, false, false);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                new SessionManager(Session).error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }

        }

        public ActionResult Publish(int? id, string btnName)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VMDeviceScroll deviceScroll = dbManager.GetById(id.Value);
            if (deviceScroll == null)
            {
                return HttpNotFound();
            }
            if (btnName == "ScrollON")
            {
                deviceScroll.is_publish = 1;
            }
            else
            {
                deviceScroll.is_publish = 0;
            }
            //deviceScroll.is_publish = (deviceScroll.is_publish == 0 ? 1 : 0);
            dbManager.Edit(deviceScroll);
            SetActivationStatus(id,btnName);
            NotifyDisplay.SendMessages(deviceScroll.department_id, deviceScroll.device_id.ToString(), "", false, false, false, true, false, false);

            return RedirectToAction("Index");
        }
       

        [HttpGet]
        public JsonResult DeviceLoadbyDepartmetId(int departmentId)
        {
            List<tblDevice> deviceList = dbDevice.GetAllDevicebyDepartment(departmentId);
            List<VMDeviceScroll> displayFootersList = dbManager.GetAll();



            var qry = deviceList.GroupJoin(
                      displayFootersList,
                      b => b.device_id,
                      d => d.device_id,
                      (x, y) => new { deviceList = x, displayFootersList = y })
                      .Where(w => w.displayFootersList.Count() == 0)
                      .Select(
                      x => x.deviceList).ToList();

            return Json(new { Success = true, deviceList = qry }, JsonRequestBehavior.AllowGet);
            //For Merge All Scroll
            //return Json(new { Success = true, deviceList = deviceList }, JsonRequestBehavior.AllowGet);
            //End
        }
        public JsonResult ScrollLoadbyDevice(int deviceId)
        {
            List<VMDeviceScroll> deviceScrollList = dbManager.GetAllbyDevice(deviceId);
            //List<VMDeviceScroll> displayFootersList = dbManager.GetAll();



            //var qry = deviceList.GroupJoin(
            //          displayFootersList,
            //          b => b.device_id,
            //          d => d.device_id,
            //          (x, y) => new { deviceList = x, displayFootersList = y })
            //          .Where(w => w.displayFootersList.Count() == 0)
            //          .Select(
            //          x => x.deviceList).ToList();

            //return Json(new { Success = true, deviceList = qry }, JsonRequestBehavior.AllowGet);
            return Json(new { Success = true, deviceScrollList = deviceScrollList }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult SetActivationStatus(int? id, string btnName)
        {
            string responseJson = String.Empty;
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                VMDeviceScroll deviceScroll = dbManager.GetById(id.Value);
                if (deviceScroll == null)
                {
                    return HttpNotFound();
                }
                if (btnName == "ScrollON")
                {
                    deviceScroll.is_active = 1;
                }
                else
                {
                    deviceScroll.is_active = 0;
                }
                //deviceScroll.is_active = (deviceScroll.is_active == 0 ? 1 : 0);
                dbManager.Edit(deviceScroll);

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
