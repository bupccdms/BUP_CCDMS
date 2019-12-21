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
using qms.DAL;

using System.Data.Entity.Core.Objects;
using qms.ViewModels;
using qms.SignalRHub;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Web.Configuration;
using qms.BLL;

namespace qms.Controllers
{

    //ReportingSerivceLib.PrintManager objPrintManager = new PrintManager();

    //IList<ParameterValue> parameters = new List<ParameterValue>();

    public class TokenQueuesController : Controller
    {
        //private qmsEntities db = new qmsEntities();
        private BLL.BLLToken dbManager = new BLL.BLLToken();
        private BLL.BLLDepartment dbDepartment = new BLL.BLLDepartment();
        private BLL.BLLStatus dbStatus = new BLL.BLLStatus();
        private BLL.BLLServiceType dbServiceType = new BLL.BLLServiceType();




        // GET: TokenQueues
        [AuthorizationFilter(Roles = "Admin, Department Admin,Token Generator")]
        public ActionResult Index(String department_name, string device_no)
        {
            ViewBag.departmentList = dbDepartment.GetAllDepartment();
            ViewBag.service_status = dbStatus.GetAll();


            SessionManager sm = new SessionManager(Session);
            //tblTokenQueue tokenObj = new tblTokenQueue();
            ViewBag.userDepartmentId = sm.department_id;
            //var tblTokenQueues = db.tblTokenQueues.Include(i => i.tblDevice).Include(i => i.tblServiceDetails);
            //string token_id = tokenObj.token_id.ToString();
            return View(dbManager.GetByDepartmentId(sm.department_id));
            //return View(await tblTokenQueues.OrderByDescending(o=>o.token_id).ToListAsync());
        }

        // GET: TokenQueues
        [AuthorizationFilter]
        public ActionResult Skipped()
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

            return View(dbManager.GetSkipped(department_id, user_id));
        }

        [AuthorizationFilter]
        [HttpPost]
        [AuthorizationFilter(Roles = "Department Admin,Service Holder")]
        public ActionResult ReInitiate(long token_id)
        {
            try
            {
                SessionManager sm = new SessionManager(Session);
                int departmentId = sm.department_id;

                string device_no = sm.device_no;

                dbManager.ReInitiate(token_id);
                NotifyDisplay.SendMessages(departmentId, "", "", true, false, false, false, false, false);
                return Json(new { Success = true, Message = "Successfully Token Re-initiated" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { Success = false, ex.Message }, JsonRequestBehavior.AllowGet);

            }



        }
        [AuthorizationFilter]
        [HttpPost]
        [AuthorizationFilter(Roles = "Service Holder")]
        public ActionResult AssignToMe(long token_id)
        {
            try
            {
                SessionManager sm = new SessionManager(Session);
                int departmentId = sm.department_id;
                int device_id = sm.device_id;
                string device_no = sm.device_no;

                dbManager.AssignToMe(token_id, device_id);
                NotifyDisplay.SendMessages(departmentId, "", "", true, false, false, false, false, false);
                return Json(new { Success = true, Message = "Successfully Token Assigned To Me" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { Success = false, ex.Message }, JsonRequestBehavior.AllowGet);

            }



        }

        [AuthorizationFilter]
        [HttpPost]
        [AuthorizationFilter(Roles = "Department Admin")]
        public ActionResult AssignToDevice(long token_id, string device_no)
        {
            try
            {
                SessionManager sm = new SessionManager(Session);
                int departmentId = sm.department_id;

                tblDevice device = new BLLDevices().GetAllDevice().Where(c => c.device_no.ToLower() == device_no.ToLower() && c.department_id==departmentId).FirstOrDefault();
                if (device == null)
                {
                    return Json(new { Success = false, Message = "Device not found" }, JsonRequestBehavior.AllowGet);
                }

                VMDepartmentDeviceStatus deviceStatus = new BLLDepartment().GetDeviceCurrentStatus(departmentId, device.device_id).FirstOrDefault();
                if (deviceStatus == null)
                {
                    return Json(new { Success = false, Message = "No user is available in this device" }, JsonRequestBehavior.AllowGet);
                }

                if (deviceStatus.login_time.HasValue == false)
                {
                    return Json(new { Success = false, Message = "No user is available in this device" }, JsonRequestBehavior.AllowGet);
                }

                dbManager.AssignToMe(token_id, device.device_id);
                NotifyDisplay.SendMessages(departmentId, "", "", true, false, false, false, false, false);
                return Json(new { Success = true, Message = "Successfully token assigned to device" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { Success = false, ex.Message }, JsonRequestBehavior.AllowGet);

            }



        }
        // GET: TokenQueues/Details/5
        //[AuthorizationFilter(Roles = "Admin, Department Admin")]
        //public async Task<ActionResult> Details(long? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    tblTokenQueue tblTokenQueue = await db.tblTokenQueues.FindAsync(id);
        //    if (tblTokenQueue == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tblTokenQueue);
        //}

        [AuthorizationFilter(Roles = "Department Admin, Token Generator")]
        public ActionResult Create()
        {
            var serviceList = dbServiceType.GetAll();
            ViewBag.ServiceTypeList = serviceList;
            return View();
        }

        //[AuthorizationFilter(Roles = "Department Admin, Token Generator")]
        //[HttpPost]
        //public JsonResult Create(string mobile, string service)
        //{
        //    try
        //    {
        //        SessionManager sm = new SessionManager(Session);
        //        int departmentId = sm.department_id;

        //        DisplayManager dm = new DisplayManager();



        //        string subString = "Generated Token No is  #";
        //        var maxToken = dbManager.GetAll()
        //            .Where(w =>
        //                w.department_id == departmentId
        //                && /*w.service_date == DateTime.Now*/
        //                DbFunctions.TruncateTime(w.service_date) == DbFunctions.TruncateTime(DateTime.Now)
        //            );

        //        int tokenNo = 0;

        //        if (maxToken.Any())
        //        {
        //            tokenNo = maxToken.Max(m => m.token_no);
        //        }


        //        tblTokenQueue tokenObj = new tblTokenQueue();
        //        tokenObj.contact_no = mobile;
        //        tokenObj.service_date = DateTime.Now;

        //        tokenObj.token_no = tokenNo + 1;



        //        //dm.SendSms(tokenNo);
        //        //db.SaveChanges();



        //        tokenObj.service_status_id = 1;

        //        tokenObj.department_id = departmentId;
        //        tokenObj.service_type_id = Convert.ToInt32(service);
        //        dbManager.Create(mobile,service);


        //        //if (!String.IsNullOrEmpty(sm.department_static_ip))
        //        //    dm.CreateTextFile(sm.department_id, sm.department_static_ip);

        //        string message = subString + tokenObj.token_no.ToString().PadLeft(ApplicationSetting.PaddingLeft, '0');
        //        NotifyDisplay.SendMessages(departmentId, "null", "null");

        //        string token_id = tokenObj.token_id.ToString();
        //        string token_no = tokenObj.token_no.ToString().PadLeft(ApplicationSetting.PaddingLeft, '0');
        //        string date =Convert.ToString(DateTime.Now);
        //        //DisplayManager dm = new DisplayManager();


        //        return Json(new { Success = true, Message = message, tokenId = token_id, tokenNo = token_no, Date = date,msisdn=mobile }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Success = false, ErrorMessage = "Problem with Token Create, Please Try Again!" }, JsonRequestBehavior.AllowGet);
        //    }

        //}
        [AuthorizationFilter(Roles = "Department Admin, Token Generator")]
        
        [HttpPost]
        public JsonResult Create(string mobile, int service)
        {
            try
            {
                SessionManager sm = new SessionManager(Session);
                int departmentId = sm.department_id;

                //DisplayManager dm = new DisplayManager();
                tblTokenQueue tokenObj = new tblTokenQueue()
                {
                    department_id = departmentId,
                    contact_no = mobile,
                    service_type_id = service
                };
                dbManager.Create(tokenObj);
                string subString = "Token No is  #";
                string token_id = tokenObj.token_id.ToString();
                string token_no = tokenObj.token_no_formated;
                string message = subString + tokenObj.token_no_formated;
                NotifyDisplay.SendMessages(departmentId, "", "", true, false, false, false, false, false);
                return Json(new { Success = true, Message = message, tokenId = token_id, tokenNo = token_no, msisdn = mobile }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { Success = false, ex.Message }, JsonRequestBehavior.AllowGet);
            }
           
        }

           

        //[AuthorizationFilter(Roles = "Admin, Department Admin")]
        //public async Task<ActionResult> Edit(long? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    tblTokenQueue tblTokenQueue = await db.tblTokenQueues.FindAsync(id);
        //    if (tblTokenQueue == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.department_id = new SelectList(db.tblDepartments, "department_id", "department_name", tblTokenQueue.department_id);
        //    ViewBag.service_status_id = new SelectList(db.tblServiceStatus, "service_status_id", "service_status", tblTokenQueue.service_status_id);
        //    return View(tblTokenQueue);
        //}

        // POST: TokenQueues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[AuthorizationFilter(Roles = "Admin, Department Admin")]
        //public async Task<ActionResult> Edit([Bind(Include = "token_id,department_id,token_no,service_date,service_status_id,contact_no")] tblTokenQueue tblTokenQueue)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(tblTokenQueue).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.department_id = new SelectList(db.tblDepartments, "department_id", "department_name", tblTokenQueue.department_id);
        //    ViewBag.service_status_id = new SelectList(db.tblServiceStatus, "service_status_id", "service_status", tblTokenQueue.service_status_id);
        //    return View(tblTokenQueue);
        //}

        // GET: TokenQueues/Delete/5
        //[AuthorizationFilter(Roles = "Admin, Department Admin")]
        //public async Task<ActionResult> Delete(long? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    tblTokenQueue tblTokenQueue = await db.tblTokenQueues.FindAsync(id);
        //    if (tblTokenQueue == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tblTokenQueue);
        //}

        //// POST: TokenQueues/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //[AuthorizationFilter(Roles = "Admin, Department Admin")]
        //public async Task<ActionResult> DeleteConfirmed(long id)
        //{
        //    tblTokenQueue tblTokenQueue = await db.tblTokenQueues.FindAsync(id);
        //    db.tblTokenQueues.Remove(tblTokenQueue);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
        //public ActionResult GetList(DateTime date)
        //{
        //    DisplayManager dm = new DisplayManager();

        //    var v = dm.DateListToken((DateTime)date).ToList();
        //    List<VMTokenQueue> dateList = v.Select(s => new VMTokenQueue
        //    {
        //        //Department_Name = s.Department_Name,
        //        //Device_Name = s.Device_Name,
        //        //UserName = s.UserName,
        //        //start_time = s.start_time,
        //        //end_time = s.end_time,
        //        //customer_name = s.customer_name,
        //        //issues = s.issues,
        //        //solutions = s.solutions
        //    }).ToList();

        //    return PartialView(dateList);
        //}


        [HttpPost]
        //[ValidateAntiForgeryToken]
        //[AuthorizationFilter(Roles = "Admin, Department Admin, Token Generator")]
        public ActionResult SMSSend(string mobileNo, string tokenNo)
        {
            try
            {
                BLLToken tokenManager = new BLLToken();

                tokenManager.SendSMS(mobileNo, tokenNo);
                return Json(new { Success = true, Message = "SMS Saved Succesfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, ex.Message }, JsonRequestBehavior.AllowGet);
            }

            
        }

        //private void PrintInvoice(string invoice)
        //{
        //     //connectionString= 
        //    SqlConnection con = new SqlConnection(connectionString);
        //    SqlCommand cmd = new SqlCommand("spINSERT_dbo_SmsLog", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@token_id", invoice);
            

        //    con.Open();
        //    SqlDataReader dr = cmd.ExecuteReader();

        //    //  frmInvoiceReportPrint ff = new frmInvoiceReportPrint(dr);
        //    // ff.ShowDialog();


        //    LocalReport report = new LocalReport();
        //    string exeFolder = System.Windows.Forms.Application.StartupPath;
        //    //report.ReportPath = Path.Combine(exeFolder, @"Report\InvoicePrintRpt.rdlc");
        //    report.ReportPath = Path.Combine(exeFolder, @"Report\AMBBookingInvoice.rdlc");
        //    report.DataSources.Add(new ReportDataSource("spINSERT_dbo_SmsLog", dr));
        //    TokenPrint objprint = new TokenPrint();
        //    objprint.Export(report);
        //    objprint.Print();

        //    dr.Close();
        //    con.Close();
        //}

        public ActionResult Print(int tokenNo)
        {

            string TokenText = "Token #";
            string token_no = TokenText + tokenNo.ToString().PadLeft(ApplicationSetting.PaddingLeft, '0');
            string date = Convert.ToString(DateTime.Now);
           
                return Json(new { Success = true, Message = token_no, Date = date }, JsonRequestBehavior.AllowGet);
           
        }


    }

}
