using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using System.Web.Mvc;
using qms.Models;

using qms.ViewModels;
using qms.Utility;

using qms.SignalRHub;
using qms.BLL;
using System.Text.RegularExpressions;

namespace qms.Controllers
{
    [AuthorizationFilter(Roles = "Admin, Department Admin, Service Holder")]
    public class ServiceDetailsController : Controller
    {
        //private qmsEntities db = new qmsEntities();
        private BLL.BLLServiceDetail dbManager = new BLL.BLLServiceDetail();
        private BLL.BLLServiceType dbServiceType = new BLL.BLLServiceType();
        private BLL.BLLDepartment dbDepartment = new BLL.BLLDepartment();
        private BLL.BLLCustomer dbCustomer = new BLL.BLLCustomer();
        private readonly BLL.BLLToken dbtoken = new BLL.BLLToken();
        private readonly BLL.BLLDailyBreak dbBreak = new BLL.BLLDailyBreak();





        private BLL.BLLServiceSubType dbServiceSubType = new BLL.BLLServiceSubType();

        // GET: ServiceDetails
        public ActionResult Index()
        {
            ViewBag.departmentList = dbDepartment.GetAllDepartment();

            SessionManager sm = new SessionManager(Session);
            ViewBag.userDepartmentId = sm.department_id;

            int? department_id;
            string user_id;
            if (User.IsInRole("Admin"))
            {
                department_id = null;
                user_id = null;
            }
            else if (User.IsInRole("Department Admin"))
            {
                department_id = sm.department_id;
                user_id = null;
            }
            else
            {
                department_id = null;
                user_id = sm.user_id;
            }

            return View(dbManager.GetAllCurrentDate(department_id, user_id));

        }

        public ActionResult Create()
        {
            var servicetype = dbServiceType.GetAll();
            ViewBag.service_type_id = new SelectList(servicetype, "service_type_id", "service_type_name");
            ViewBag.service_sub_type_id = new SelectList(dbServiceSubType.GetAll(), "service_sub_type_id", "service_sub_type_name");
            //ViewBag.ServiceTypeList = new SelectList(dbServiceSubType.GetAll(), "service_sub_type_id", "service_sub_type_name");

            return View();
        }
        
        [HttpPost]
        [AuthorizationFilter(Roles = "Department Admin, Service Holder")]
        public JsonResult Create(VMServiceDetails model)
        {
            try
            {
                SessionManager sm = new SessionManager(Session);
                DisplayManager dm = new DisplayManager();
                model.service_datetime = DateTime.Now;
                model.customer_name = model.contact_no;
                model.end_time = DateTime.Now;
                model.user_id = sm.user_id;
                model.device_id = sm.device_id;
                if (ModelState.IsValid)
                {
                    dbManager.Create(model);
                }
                else
                {
                    return Json(new { Success = false, Message = "Model state is not valid" }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { Success = true, Message = "Customer Service Information updated on Server" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { Success = false, Message = "Problem with Information updated, Please Try Again! Error: " + ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }
        [HttpPost]
        [AuthorizationFilter(Roles = "Department Admin, Service Holder")]
        public JsonResult Done(VMServiceDetails model)
        {
            try
            {
                SessionManager sm = new SessionManager(Session);
                DisplayManager dm = new DisplayManager();
                int departmentId = sm.department_id;
                
                string device_no = sm.device_no;
                model.service_datetime = DateTime.Now;
                model.end_time = DateTime.Now;
                model.user_id = sm.user_id;
                model.device_id = sm.device_id;
                if (ModelState.IsValid)
                {
                    dbManager.Create(model);
                }
                else
                {
                    return Json(new { Success = false, Message = "Model state is not valid" }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { Success = true, Message = "Customer Service Information updated on Server" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { Success = false, Message = "Problem with Information updated, Please Try Again! Error: " + ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }
        [HttpPost]
        [AuthorizationFilter(Roles = "Department Admin, Service Holder")]
        public JsonResult AddService(VMServiceDetails model)
        {
            try
            {
                SessionManager sm = new SessionManager(Session);
                DisplayManager dm = new DisplayManager();
                model.service_datetime = DateTime.Now;
                model.end_time = DateTime.Now;
                model.user_id = sm.user_id;
                model.device_id = sm.device_id;
                if (ModelState.IsValid)
                {
                    dbManager.AddService(model);
                }
                // if (!String.IsNullOrEmpty(sm.department_static_ip))
                //dm.CreateTextFile(sm.department_id, sm.department_static_ip);

                return Json(new { Success = true, Message = "Customer Service Information updated on Server" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { Success = false, Message = "Problem with Information updated, Please Try Again! Error: " + ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost]
        public JsonResult NewTokenNo()
        {
            SessionManager sm = new SessionManager(Session);
            int departmentId = sm.department_id;
            int deviceid = sm.device_id;
            string device_no = sm.device_no;
            string user_id = sm.user_id;

            try
            {
                var serviceList = dbManager.GetNewToken(departmentId, deviceid, user_id, out long token_id, out string token_prefix, out int token_no, out string contact_no
                    , out string service_type, out DateTime start_time, out string customer_name, out string address,out DateTime generate_time,out int is_break);

                if (serviceList.Count>0)
                {
                    var services = new SelectList(serviceList, "service_sub_type_id", "service_sub_type_name");


                    var customer = new
                    {
                        token = token_prefix + token_no.ToString().PadLeft(ApplicationSetting.PaddingLeft, '0'),
                        start_time = start_time.ToString("hh:mm:ss tt"),
                        tokenid = token_id,
                        serviceType = service_type,
                        mobile_no = contact_no,
                        ser_id=user_id,
                        generate_time = generate_time.ToString("hh:mm:ss tt"),
                        call_time= start_time.ToString("hh:mm:ss tt"),
                        IsBreak= is_break,
                        waitingtime = start_time.Subtract(generate_time).ToString(),
                        service_type_id = (serviceList.Count>0? serviceList.FirstOrDefault().service_type_id : 0),
                        customer_name,
                        address
                    };
                    string voiceToken = Regex.Replace(customer.token, ".{1}", "$0, ");
                    NotifyDisplay.SendMessages(departmentId, device_no, voiceToken, false, true, false, false, false, false);
                    NotifyDisplay.DeviceStatusChanged(sm.department_id);
                    return Json(new { Success = true, Message = customer, Services = services }, JsonRequestBehavior.AllowGet);



                }
                else
                {
                    NotifyDisplay.SendMessages(departmentId, "", "", false, true, false, false, false, false);
                    NotifyDisplay.DeviceStatusChanged(sm.department_id);
                    return Json(new { Success = false, Message = "No token for new service!", IsBreak = is_break }, JsonRequestBehavior.AllowGet);
                }


            }
            catch (Exception ex)
            {
                if (ex.Message.ToUpper() == "invalid operation on null data".ToUpper())
                {
                    NotifyDisplay.SendMessages(departmentId, "", "", false, true, false, false, false, false);
                    NotifyDisplay.DeviceStatusChanged(sm.department_id);
                    return Json(new { Success = false, Message = "No token for new service!", IsBreak = 0 }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { Success = false, ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpPost]
        public JsonResult CallManualTokenNo(string token_no_string)
        {
            try
            {
                SessionManager sm = new SessionManager(Session);
                int departmentId = sm.department_id;
                int deviceid = sm.device_id;
                string device_no = sm.device_no;
                string user_id = sm.user_id;
                
                
                var serviceList = dbManager.CallManualToken(departmentId, deviceid, user_id, token_no_string, out long token_id, out string contact_no, out string service_type
                    , out DateTime start_time, out string customer_name, out string address);

                if (serviceList.Count > 0)
                {
                    var services = new SelectList(serviceList, "service_sub_type_id", "service_sub_type_name");


                    var customer = new
                    {
                        token = token_no_string,
                        start_time = start_time.ToString("dd-MMM-yyyy hh:mm:ss tt"),
                        tokenid = token_id,
                        serviceType = service_type,
                        mobile_no = contact_no,
                        customer_name,
                        address
                    };
                    //NotifyDisplay.SendMessages(departmentId, device_no, token_no.ToString());
                    return Json(new { Success = true, Message = customer, Services = services }, JsonRequestBehavior.AllowGet);



                }
                else
                {
                    NotifyDisplay.SendMessages(departmentId, device_no, "", false, false, false, false, false, false);
                    return Json(new { Success = false, Message = "No token for new service!" }, JsonRequestBehavior.AllowGet);
                }


            }
            catch (Exception ex)
            {
                return Json(new { Success = false, ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CancelTokenNo(long tokenID)
        {
            try
            {


                int token_no = dbManager.CancelToken(tokenID);

                //SessionManager sm = new SessionManager(Session);
                //DisplayManager dm = new DisplayManager();
                //if (!String.IsNullOrEmpty(sm.department_static_ip))
                //    dm.CreateTextFile(sm.department_id, sm.department_static_ip);

                return Json(new { Success = true, Message = "Service Canceled for Token No #" + token_no.ToString().PadLeft(ApplicationSetting.PaddingLeft, '0') }, JsonRequestBehavior.AllowGet);



            }
            catch(Exception)
            {
                return Json(new { Success = false, Message = "Problem with getting new service!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Cancel(long tokenID)
        {
            try
            {
                SessionManager sm = new SessionManager(Session);
                DisplayManager dm = new DisplayManager();
                int departmentId = sm.department_id;

                string device_no = sm.device_no;

                int token_no = dbManager.CancelToken(tokenID);

                //SessionManager sm = new SessionManager(Session);
                //DisplayManager dm = new DisplayManager();
                //if (!String.IsNullOrEmpty(sm.department_static_ip))
                //    dm.CreateTextFile(sm.department_id, sm.department_static_ip);
                //NotifyDisplay.SendMessages(departmentId, device_no, "");

                return Json(new { Success = true, Message = "Service Canceled for Token No #" + token_no.ToString().PadLeft(ApplicationSetting.PaddingLeft, '0') }, JsonRequestBehavior.AllowGet);



            }
            catch (Exception)
            {
                return Json(new { Success = false, Message = "Problem with getting new service!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Transfer(long token_id, string device_no)
        {
            try
            {

                SessionManager sm = new SessionManager(Session);

                dbManager.Transfer(sm.department_id, device_no, token_id);

                //SessionManager sm = new SessionManager(Session);
                //DisplayManager dm = new DisplayManager();
                //if (!String.IsNullOrEmpty(sm.department_static_ip))
                //    dm.CreateTextFile(sm.department_id, sm.department_static_ip);

                return Json(new { Success = true, Message = "Service transfered to device #" + device_no + ", customer must wait for calling" }, JsonRequestBehavior.AllowGet);



            }
            catch(Oracle.DataAccess.Client.OracleException ex)
            {
                if (ex.Number == 20001)
                    return Json(new { Success = false, Message = "Device not found" }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { Success = false, ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                
                return Json(new { Success = false, ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetCustomerInformation(long token_id, string contact_no)
        {
            try
            {

                //tblTokenQueue tokenDetail = db.tblTokenQueues.Where(a => a.token_id == token_id).FirstOrDefault();

                //tokenDetail.service_status_id = 2;

                //db.Entry(tokenDetail).State = EntityState.Modified;
                //db.SaveChanges();

                tblCustomer customerDetails = dbCustomer.GetAll().Where(a => a.contact_no == contact_no).FirstOrDefault();

                if (customerDetails != null)
                {
                    //List<tblTokenQueue> previousHistoryList = new List<tblTokenQueue>();
                    //var previousHistoryList = db.tblTokenQueues.Where(a => a.contact_no == contact_no).Include(x=>x.tblServiceDetails).ToList();

                    List<tblServiceDetail> previousHistoryList = dbManager.GetByCustomerID(customerDetails.customer_id);
                    List<VMServiceDetails> customerlist = new List<VMServiceDetails>();

                    foreach (tblServiceDetail item in previousHistoryList)
                    {
                        VMServiceDetails VMServiceDetails = new VMServiceDetails()
                        {
                            issues = item.issues,
                            solutions = item.solutions,
                            service_datetime = item.service_datetime
                        };
                        customerlist.Add(VMServiceDetails);
                    }


                    return Json(new { Success = true, Message = customerlist, customerDetails }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Success = false, Message = "", customerDetails = "" }, JsonRequestBehavior.AllowGet);
                }


            }
            catch(Exception)
            {
                return Json(new { Success = false, Message = "Problem with getting Customer Information!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizationFilter(Roles = "Service Holder")]
        public ActionResult ServiceList()
        {
            SessionManager sm = new SessionManager(Session);
            List<VMDashboardDepartmentService> serviceList = new BLLDashboard().GetDepartmentServiceList(sm.department_id);
            //List<VMDashboardDepartmentToken> tokenList = new BLLDashboard().GetDepartmentTokenList(sm.department_id);
            //ViewBag.tokenList = tokenList;

            return PartialView(serviceList);
        }

        [AuthorizationFilter(Roles = "Service Holder")]
        public ActionResult DeviceServiceList()
        {
            SessionManager sm = new SessionManager(Session);
            
            List<VMDashboardDeviceService> serviceList = new BLLDashboard().GetDeviceServiceList(sm.device_id, out string serving_time);
            List<VMDashboardDeviceToken> tokenList = new BLLDashboard().GetDeviceTokenList(sm.device_id);
            ViewBag.tokenList = tokenList;

            List<tblTokenQueue> tokens = new BLLToken().GetBy(from_date : DateTime.Now, to_date : DateTime.Now, department_id : sm.department_id);

            AspNetUserLogin login = new BLLAspNetUser().GetLoginInfo(sm.user_id);
            ViewBag.UserName = sm.user_name;
            ViewBag.DeviceNo = sm.device_no;
            ViewBag.LoginTime = login.login_time.ToString("hh:mm:ss tt");
            ViewBag.ServingTime = serving_time;
            if (tokens.Count > 0)
            {
                ViewBag.TotalPending = tokens.Where(w => w.service_status_id == 1 || w.service_status_id == 2).Count();
                ViewBag.TotalServed = tokens.Where(w => w.service_status_id == 5 && w.device_id==sm.device_id).Count();
                var deviceServedTokens = tokens.Where(w => w.service_status_id == 5 && w.device_id == sm.device_id).ToList();
                var hw = deviceServedTokens.Max(m => m.CallTime - m.service_date);
                ViewBag.HighestWaiting = (hw.HasValue ? string.Format("{0:hh\\:mm\\:ss}", (hw.Value)) : "00:00:00");
                var aw = CalculateAverageWaitingTime(deviceServedTokens);
                ViewBag.AverageWaiting = string.Format("{0:hh\\:mm\\:ss}", aw); 
            }
            else
            {
                ViewBag.TotalPending = 0;
                ViewBag.TotalServed = 0;
                ViewBag.HighestWaiting = string.Format("{0:hh\\:mm\\:ss}", default(TimeSpan));
                ViewBag.AverageWaiting = string.Format("{0:hh\\:mm\\:ss}", default(TimeSpan));
            }

            return PartialView(serviceList);
        }

        [AuthorizationFilter(Roles = "Service Holder")]
        public ActionResult UserServiceList()
        {
            SessionManager sm = new SessionManager(Session);
           

            List<VMDashboardUserService> serviceList = new BLLDashboard().GetUserServiceList(sm.user_id, out string serving_time);
            List<VMDashboardUserToken> tokenList = new BLLDashboard().GetUserTokenList(sm.user_id);
            List<VMDashboardUserServiceDetail> serviceDetailList = new BLLDashboard().GetUserServiceDetailList(sm.user_id);
            ViewBag.tokenList = tokenList;
            ViewBag.serviceDetailList = serviceDetailList;

            List<tblTokenQueue> tokens = new BLLToken().GetBy(from_date: DateTime.Now, to_date: DateTime.Now, department_id: sm.department_id);

            AspNetUserLogin login = new BLLAspNetUser().GetLoginInfo(sm.user_id);
            ViewBag.UserName = sm.user_name;
            ViewBag.DeviceNo = sm.device_no;
            ViewBag.LoginTime = login.login_time.ToString("hh:mm:ss tt");
            ViewBag.ServingTime = serving_time;
            if (tokens.Count > 0)
            {
                ViewBag.TotalPending = tokens.Where(w => w.service_status_id == 1 || w.service_status_id == 2).Count();
                ViewBag.TotalServed = tokens.Where(w => w.service_status_id == 5 && w.user_id == sm.user_id).Count();
                var deviceServedTokens = tokens.Where(w => w.service_status_id == 5 && w.user_id == sm.user_id).ToList();
                var hw = deviceServedTokens.Max(m => m.CallTime - m.service_date);
                ViewBag.HighestWaiting = (hw.HasValue ? string.Format("{0:hh\\:mm\\:ss}", (hw.Value)) : "00:00:00");
                var aw = CalculateAverageWaitingTime(deviceServedTokens);
                ViewBag.AverageWaiting = string.Format("{0:hh\\:mm\\:ss}", aw);
            }
            else
            {
                ViewBag.TotalPending = 0;
                ViewBag.TotalServed = 0;
                ViewBag.HighestWaiting = string.Format("{0:hh\\:mm\\:ss}", default(TimeSpan));
                ViewBag.AverageWaiting = string.Format("{0:hh\\:mm\\:ss}", default(TimeSpan));
            }

            return PartialView(serviceList);
        }



        private TimeSpan CalculateAverageWaitingTime(List<tblTokenQueue> tokens)
        {
            if (tokens.Count > 0)
            {
                double doubleAverageTicks = tokens.Average(timeSpan => timeSpan.waitingtimeToTimeStamp.Ticks);
                long longAverageTicks = Convert.ToInt64(doubleAverageTicks);

                return new TimeSpan(longAverageTicks);
            }
            else return default(TimeSpan);
        }
    }
}
