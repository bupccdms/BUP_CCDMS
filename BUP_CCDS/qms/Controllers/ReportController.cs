using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using System.IO;
using qms.ViewModels;
using qms.ReportModels;
using qms.Models;
using CrystalDecisions.CrystalReports.Engine;
using qms.Utility;
using System.Data;

namespace qms.Controllers
{
    [AuthorizationFilter(Roles = "Admin, Department Admin, Service Holder")]
    public class ReportController : Controller
    {
        //
        // GET: /Report/
        ReportDataSource rd;
        private readonly BLL.BLLServiceSubType dbService = new BLL.BLLServiceSubType();
        private readonly BLL.BLLReport dbReport = new BLL.BLLReport();
        //private qmsEntities db = new qmsEntities();
        public ActionResult Index()
        {
            int department_id = new SessionManager(Session).department_id;
            ViewBag.department_id = new SelectList(new BLL.BLLDepartment().GetAllDepartment(), "department_id", "department_name", department_id);
            List<VMServiceType> serviceList = new BLL.BLLServiceSubType().GetAll();
            ViewBag.serviceList = serviceList;
            // ViewBag.serviceList = new SelectList(dbService.GetAll(), "service_sub_type_id", "service_sub_type_name", service_sub_type_id);
            return View();
        }

        public ActionResult DepartmentListReportViewer()
        {
            return View();
        }


        #region Views

        public ActionResult LocalCustomersReport()
        {
            int department_id = new SessionManager(Session).department_id;
            ViewBag.department_id = new SelectList(new BLL.BLLDepartment().GetAllDepartment(), "department_id", "department_name", department_id);
            List<VMServiceType> serviceList = new BLL.BLLServiceSubType().GetAll();
            ViewBag.serviceList = serviceList;
            List<tblCustomerType> customerTypeList = new BLL.BLLCustomerType().GetAll();
            ViewBag.customerTypeList = customerTypeList;
            return View();
        }

        public ActionResult SingleVSMultipleVisitSummaryReport()
        {
            int department_id = new SessionManager(Session).department_id;
            ViewBag.department_id = new SelectList(new BLL.BLLDepartment().GetAllDepartment(), "department_id", "department_name", department_id);
            List<VMServiceType> serviceList = new BLL.BLLServiceSubType().GetAll();
            ViewBag.serviceList = serviceList;
            return View();
        }

        public ActionResult AgentWiseReport()
        {
            int department_id = new SessionManager(Session).department_id;
            ViewBag.department_id = new SelectList(new BLL.BLLDepartment().GetAllDepartment(), "department_id", "department_name", department_id);
            List<VMServiceType> serviceList = new BLL.BLLServiceSubType().GetAll();
            ViewBag.serviceList = serviceList;
            return View();
        }

        public ActionResult ServiceSummaryReport()
        {
            int department_id = new SessionManager(Session).department_id;
            ViewBag.department_id = new SelectList(new BLL.BLLDepartment().GetAllDepartment(), "department_id", "department_name", department_id);
            List<VMServiceType> serviceList = new BLL.BLLServiceSubType().GetAll();
            ViewBag.serviceList = serviceList;
            return View();
        }

        public ActionResult GeneralSearchReport()
        {
            int department_id = new SessionManager(Session).department_id;
            ViewBag.department_id = new SelectList(new BLL.BLLDepartment().GetAllDepartment(), "department_id", "department_name", department_id);
            List<VMServiceType> serviceList = new BLL.BLLServiceSubType().GetAll();
            ViewBag.serviceList = serviceList;
            return View();
        }

        public ActionResult AgentLogSummaryReport()
        {
            int department_id = new SessionManager(Session).department_id;
            ViewBag.department_id = new SelectList(new BLL.BLLDepartment().GetAllDepartment(), "department_id", "department_name", department_id);
            return View();
        }

        public ActionResult BreakReport()
        {
            int department_id = new SessionManager(Session).department_id;
            ViewBag.department_id = new SelectList(new BLL.BLLDepartment().GetAllDepartment(), "department_id", "department_name", department_id);

            ViewBag.break_type_id = new SelectList(new BLL.BLLBreakType().GetAll(), "break_type_id", "break_type_name");



            return View();
        }

        public ActionResult TopNServicesReport()
        {
            int department_id = new SessionManager(Session).department_id;
            ViewBag.department_id = new SelectList(new BLL.BLLDepartment().GetAllDepartment(), "department_id", "department_name", department_id);
            List<VMServiceType> serviceList = new BLL.BLLServiceSubType().GetAll();
            ViewBag.serviceList = serviceList;
            return View();
        }

        public ActionResult TokenExceedingReport()
        {
            int department_id = new SessionManager(Session).department_id;
            ViewBag.department_id = new SelectList(new BLL.BLLDepartment().GetAllDepartment(), "department_id", "department_name", department_id);
            List<VMServiceType> serviceList = new BLL.BLLServiceSubType().GetAll();
            ViewBag.serviceList = serviceList;
            return View();
        }
        public ActionResult LogOutDetailReport()
        {
            int department_id = new SessionManager(Session).department_id;
            ViewBag.department_id = new SelectList(new BLL.BLLDepartment().GetAllDepartment(), "department_id", "department_name", department_id);
            return View();
        }
        public ActionResult LoginAttemptDetailsReport()
        {
            int department_id = new SessionManager(Session).department_id;
            ViewBag.department_id = new SelectList(new BLL.BLLDepartment().GetAllDepartment(), "department_id", "department_name", department_id);
            return View();
        }

        #endregion



        #region LoadHTMLData

        public JsonResult GetLocalCustomerInformation(int department_id, string user_id, int device_id, int customer_type_id, int service_sub_type_id, DateTime start_date, DateTime end_date)
        {
            try
            {
                List<RM_LocalCustomer> localCustomerList = new BLL.BLLReport().LocalCustomerReport(department_id, user_id, device_id, customer_type_id, service_sub_type_id, start_date, end_date);
                return Json(new { Success = true, Message = localCustomerList }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Success = false, ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetVisitedCustomerInformation(int department_id, string user_id, int device_id, int service_sub_type_id, DateTime start_date, DateTime end_date)
        {
            try
            {
                List<RM_SingleVSMultipleVisitedCustomer> visitedCustomerList = new BLL.BLLReport().SingleVSMultipleVisitedReport(department_id, user_id, device_id, service_sub_type_id, start_date, end_date);
                return Json(new { Success = true, Message = visitedCustomerList }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Success = false, ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAgentWiseReportInformation(int department_id, string user_id, int device_id, int service_sub_type_id, DateTime start_date, DateTime end_date)
        {
            try
            {
                List<RM_AgentWiseSummary> agentWiseSummary = new BLL.BLLReport().AgentWiseSummaryReport(department_id, user_id, device_id, service_sub_type_id, start_date, end_date);
                return Json(new { Success = true, Message = agentWiseSummary }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { Success = false, Message = "Problem with getting Agent Information!" }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetServiceSummaryInformation(int department_id, string user_id, int device_id, int service_sub_type_id, DateTime start_date, DateTime end_date)
        {
            try
            {
                List<RM_ServiceSummary> serviceSummaryList = new BLL.BLLReport().ServiceSummaryReport(department_id, user_id, device_id, service_sub_type_id, start_date, end_date);
                return Json(new { Success = true, Message = serviceSummaryList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetGeneralSearchInformation(int department_id, string user_id, int device_id, string msisdn_no, int service_sub_type_id, DateTime start_date, DateTime end_date, string token_no)
        {
            try
            {
                List<RM_GeneralSearch> generalSearch = new BLL.BLLReport().GeneralSearchReport(department_id, user_id, device_id, msisdn_no, service_sub_type_id, start_date, end_date, token_no);
                return Json(new { Success = true, Message = generalSearch }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetBreakReportInformation(int department_id, string user_id, int device_id, int break_type_id, DateTime start_date, DateTime end_date)
        {
            try
            {
                List<RM_Break> breakList = new BLL.BLLReport().BreakReport(department_id, user_id, device_id, break_type_id, start_date, end_date);
                return Json(new { Success = true, Message = breakList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetTopNServiceInformation(int department_id, string user_id, int device_id, DateTime start_date, DateTime end_date, int topn)
        {
            try
            {
                List<RM_TopNService> topNServiceList = new BLL.BLLReport().TopNServiceReport(department_id, user_id, device_id, start_date, end_date, topn);
                return Json(new { Success = true, Message = topNServiceList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetTokenExceedingInformation(int department_id, string user_id, int device_id, int service_sub_type_id, DateTime start_date, DateTime end_date, int flag)
        {
            try
            {
                List<RM_TokenExceeding> tokenExceedingList = new BLL.BLLReport().TokenExceedingReport(department_id, user_id, device_id, service_sub_type_id, start_date, end_date, flag);
                return Json(new { Success = true, Message = tokenExceedingList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetLogoutDetailInformation(int department_id, string user_id, int device_id, DateTime start_date, DateTime end_date)
        {
            try
            {
                List<RM_LogoutDetail> logoutDetailList = new BLL.BLLReport().LogoutDetailReport(department_id, user_id, device_id, start_date, end_date);
                return Json(new { Success = true, Message = logoutDetailList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetLoginAttemptDetailsInformation(int department_id, string user_id, int device_id, int is_success, DateTime start_date, DateTime end_date)
        {
            try
            {
                List<AspNetUserLoginAttempts> loginAttemptList = new BLL.BLLReport().LoginAttemptDetailsReport(department_id, user_id, device_id, is_success, start_date, end_date);
                return Json(new { Success = true, Message = loginAttemptList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion






        #region Exports



        public ActionResult ExportLocalCustomerReport(int department_id, string user_id, int device_id, int customer_type_id, int service_sub_type_id, DateTime start_date, DateTime end_date, string report_Name = "", string file_Type = "")
        {
            try
            {
                
                List<RM_LocalCustomer> localCustomerList = new BLL.BLLReport().LocalCustomerReport(department_id, user_id, device_id, customer_type_id, service_sub_type_id, start_date, end_date);
                return GetReportStream(localCustomerList, report_Name, file_Type);

            }
            catch (Exception ex)
            {
                new SessionManager(Session).error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }


        }

        public ActionResult ExportSinglevsMultipleVisitSummaryReport(int department_id, string user_id, int device_id, int service_sub_type_id, DateTime start_date, DateTime end_date, string report_Name = "", string file_Type = "")
        {
            try
            {
                List<RM_SingleVSMultipleVisitedCustomer> singleVSMultipleVisitedList = new BLL.BLLReport().SingleVSMultipleVisitedReport(department_id, user_id, device_id, service_sub_type_id, start_date, end_date);
                return GetReportStream(singleVSMultipleVisitedList, report_Name, file_Type);
            }
            catch (Exception ex)
            {
                new SessionManager(Session).error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }

        }

        public ActionResult ExportAgentWiseSummaryReport(int department_id, string user_id, int device_id, int service_sub_type_id, DateTime start_date, DateTime end_date, string report_Name = "", string file_Type = "")
        {
            try
            {
                List<RM_AgentWiseSummary> agentWiseSummary = new BLL.BLLReport().AgentWiseSummaryReport(department_id, user_id, device_id, service_sub_type_id, start_date, end_date);

                return GetReportStream(agentWiseSummary, report_Name, file_Type);
            }
            catch (Exception ex)
            {
                new SessionManager(Session).error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }


        }

        public ActionResult ExportServiceSummaryReport(int department_id, string user_id, int device_id, int service_sub_type_id, DateTime start_date, DateTime end_date, string report_Name = "", string file_Type = "")
        {
            try
            {
                List<RM_ServiceSummary> serviceSummaryList = new BLL.BLLReport().ServiceSummaryReport(department_id, user_id, device_id, service_sub_type_id, start_date, end_date);

                return GetReportStream(serviceSummaryList, report_Name, file_Type);
            }
            catch (Exception ex)
            {
                new SessionManager(Session).error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }



        }

        public ActionResult ExportGeneralSearchReport(int department_id, string user_id, int device_id, string msisdn_no, int service_sub_type_id, DateTime start_date, DateTime end_date, string token_no, string report_Name = "", string file_Type = "")
        {
            try
            {
                List<RM_GeneralSearch> generalSearch = new BLL.BLLReport().GeneralSearchReport(department_id, user_id, device_id, msisdn_no, service_sub_type_id, start_date, end_date, token_no);

                return GetReportStream(generalSearch, report_Name, file_Type);
            }
            catch (Exception ex)
            {
                new SessionManager(Session).error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }


        }

        public ActionResult ExportBreakReport(int department_id, string user_id, int device_id, int break_type_id, DateTime start_date, DateTime end_date, string report_Name = "", string file_Type = "")
        {
            try
            {
                List<RM_Break> breakList = new BLL.BLLReport().BreakReport(department_id, user_id, device_id, break_type_id, start_date, end_date);

                return GetReportStream(breakList, report_Name, file_Type);
            }
            catch (Exception ex)
            {
                new SessionManager(Session).error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }



        }

        public ActionResult ExportTopNServiceReport(int department_id, string user_id, int device_id, DateTime start_date, DateTime end_date, int topn, string report_Name = "", string file_Type = "")
        {
            try
            {
                List<RM_TopNService> topNServiceList = new BLL.BLLReport().TopNServiceReport(department_id, user_id, device_id, start_date, end_date, topn);

                return GetReportStream(topNServiceList, report_Name, file_Type);
            }
            catch (Exception ex)
            {
                new SessionManager(Session).error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }



        }

        public ActionResult ExportTokenExceedingReport(int department_id, string user_id, int device_id, int service_sub_type_id, DateTime start_date, DateTime end_date, int flag, string report_Name = "", string file_Type = "")
        {
            try
            {
                List<RM_TokenExceeding> tokenExceedingList = new BLL.BLLReport().TokenExceedingReport(department_id, user_id, device_id, service_sub_type_id, start_date, end_date, flag);

                return GetReportStream(tokenExceedingList, report_Name, file_Type);
            }
            catch (Exception ex)
            {
                new SessionManager(Session).error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }



        }

        public ActionResult ExportLogoutDetailReport(int department_id, string user_id, int device_id, DateTime start_date, DateTime end_date, string report_Name = "", string file_Type = "")
        {
            try
            {
                List<RM_LogoutDetail> logoutDetailList = new BLL.BLLReport().LogoutDetailReport(department_id, user_id, device_id, start_date, end_date);
                
                return GetReportStream(logoutDetailList, report_Name, file_Type);
            }
            catch (Exception ex)
            {
                new SessionManager(Session).error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }



        }

        public ActionResult ExportLoginAttemptDetailsReport(int department_id, string user_id, int device_id, int is_success, DateTime start_date, DateTime end_date, string report_Name = "", string file_Type = "")
        {
            try
            {
                List<AspNetUserLoginAttempts> loginAttemptList = new BLL.BLLReport().LoginAttemptDetailsReport(department_id, user_id, device_id, is_success, start_date, end_date);

                return GetReportStream(loginAttemptList, report_Name, file_Type);
            }
            catch (Exception ex)
            {
                new SessionManager(Session).error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }



        }

        private FileStreamResult GetReportStream<T>(List<T> list, string report_Name, string file_Type)
        {
            string reportPath = Server.MapPath((Request.ApplicationPath == @"/" ? "" : Request.ApplicationPath) + @"/Reports/" + report_Name + ".rpt");

            try
            {
                if (file_Type.ToUpper() == "CSV")
                {
                    ExportManager.ExportToCSV(list, report_Name);
                }
                else
                {
                    ReportDocument rd = new ReportDocument();
                    rd.Load(reportPath);

                    rd.SetDataSource(list);

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    if (file_Type.ToUpper() == "PDF")
                    {
                        Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                        stream.Seek(0, SeekOrigin.Begin);
                        return File(stream, "application/pdf", report_Name + ".pdf");
                    }
                    else if (file_Type.ToUpper() == "EXCEL")
                    {
                        Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                        stream.Seek(0, SeekOrigin.Begin);
                        return File(stream, "application/excel", report_Name + ".xls");
                    }
                    else if (file_Type.ToUpper() == "WORD")
                    {
                        Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.WordForWindows);
                        stream.Seek(0, SeekOrigin.Begin);
                        return File(stream, "application/pdf", report_Name + ".doc");
                    }
                }
                
                
            }
            catch (Exception)
            {

                throw;
            }
            return null;
        }

        private byte[] StreamToByte(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public JsonResult GetTokenByDepartmentId(int DeviceId)
        {
            var tokenList = new BLL.BLLToken().GetAllToken().Where(a => a.device_id == DeviceId).Select(x => new
            {
                x.token_id
            }).ToList();
            return Json(new { success = true, data = tokenList }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CustomerServiceReport(string id, string reportOptions, int? departmentID, int? DeviceID, int? tokenID, DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                LocalReport lr = new LocalReport();

                //DateTime? fdate = fromDate.GetValueOrDefault();
                //DateTime? tdate = toDate.GetValueOrDefault();
                DateTime? fdate = null;
                DateTime? tdate = null;

                if (reportOptions == "Summary")
                {
                    string path = Path.Combine(Server.MapPath("~/Reports"), "SummaryReport.rdlc");
                    if (System.IO.File.Exists(path))
                    {
                        lr.ReportPath = path;
                    }
                    List<VMServiceDetails> v = new List<VMServiceDetails>();
                    if (fromDate == null)
                    {
                        fdate = null;
                    }
                    else
                    {
                        fdate = fromDate.GetValueOrDefault();
                        //fdate = fdate.AddSeconds(-1);
                    }
                    if (toDate == null)
                    {
                        toDate = null;
                    }
                    else
                    {
                        tdate = toDate.GetValueOrDefault();
                        //tdate = tdate.AddDays(1).AddSeconds(-1);
                    }
                    CustomModel csmodel = new CustomModel();
                    v = csmodel.GetCustomerServiceSummary(departmentID, fdate, tdate).ToList();
                    ReportParameterCollection reportParameters = new ReportParameterCollection() {
                        new ReportParameter("rpFromDate", fromDate + ""),
                        new ReportParameter("rpToDate", toDate + "")
                    };
                    lr.SetParameters(reportParameters);
                    rd = new ReportDataSource("DataSet1", v);
                }
                else if (reportOptions == "ServiceDetails")
                {


                    List<VMServiceDetails> v = new List<VMServiceDetails>();
                    string path = Path.Combine(Server.MapPath("~/Reports"), "DeviceTokenDetailsALLReport.rdlc");
                    if (System.IO.File.Exists(path))
                    {
                        lr.ReportPath = path;
                    }

                    if (fromDate == null)
                    {
                        fromDate = DateTime.MinValue;
                    }
                    else
                    {
                        fdate = fromDate.GetValueOrDefault();
                        //fdate = fdate.AddSeconds(-1);
                    }
                    if (toDate == null)
                    {
                        toDate = DateTime.MaxValue;
                    }
                    else
                    {
                        tdate = toDate.GetValueOrDefault();
                        //tdate = tdate.AddDays(1).AddSeconds(-1);
                    }
                    CustomModel csmodel = new CustomModel();

                    v = csmodel.GetCustomerServiceDetails(DeviceID, tokenID).ToList();

                    ReportParameterCollection reportParameters = new ReportParameterCollection() {
                        new ReportParameter("rpFromDate", fromDate + ""),
                        new ReportParameter("rpToDate", toDate + "")
                    };

                    lr.SetParameters(reportParameters);

                    rd = new ReportDataSource("DataSet1", v);
                    //} 
                }

                lr.DataSources.Add(rd);
                string reportType = id;
                string deviceInfo =

                "<DeviceInfo>" +
                "  <OutputFormat>" + id + "</OutputFormat>" +
                "</DeviceInfo>";

                byte[] renderedBytes;

                renderedBytes = lr.Render(
                    reportType,
                    deviceInfo,
                    out string mimeType,
                    out string encoding,
                    out string fileNameExtension,
                    out string[] streams,
                    out Warning[] warnings);

                return File(renderedBytes, mimeType);
            }
            catch (Exception ex)
            {
                new SessionManager(Session).error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }

        }


        #endregion
    }
}