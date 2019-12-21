using qms.BLL;
using qms.Models;
using qms.SignalRHub;
using qms.Utility;
using qms.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace qms.Controllers
{
    public class ApiServiceController : Controller
    {
        //private qmsEntities db = new qmsEntities();
        private BLLDashboard db = new BLLDashboard();

        
        
 
        [HttpPost]
        public JsonResult GetServiceList(string securityToken)
        {
            string responseJson = String.Empty;
            try
            {
                ApiManager.ValidUserBySecurityToken(securityToken);
                BLLServiceType dbServiceType = new BLLServiceType();
                
                var serviceList = dbServiceType.GetAll().ToList();

                JsonResult json = Json(new { success = true, serviceList }, JsonRequestBehavior.AllowGet);
                responseJson = new JavaScriptSerializer().Serialize(json.Data);
                return json;
            }
            catch (Exception ex)
            {
                JsonResult json = Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
                responseJson = new JavaScriptSerializer().Serialize(json.Data);
                return json;
            }
            finally
            {
                string requestJson = new JavaScriptSerializer().Serialize(Json(new { securityToken }, JsonRequestBehavior.AllowGet).Data);
                ApiManager.Loggin(securityToken, "GetServiceList", requestJson, responseJson);
            }

        }

        [HttpGet]
        public JsonResult HasService()
        {
            try
            {
                return Json(new { success = true, message = "Service is running" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }


        [HttpGet]
        public JsonResult GetDBDate()
        {
            try
            {
                DateTime dbdate = new DAL.OracleDataManager().CallStoredProcedure_DBDate();
                return Json(new { success = true, dbdate = dbdate.ToString("hh:mm:ss tt") }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult GetAllDepartments()
        {
            try
            {
                BLLDepartment dbDepartment = new BLLDepartment();

                var List = dbDepartment.GetAllDepartment().ToList();

                return Json(new { success = true, departmentList = List }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult GetTokenList(string securityToken)
        {
            
                string responseJson = String.Empty;
                try
                {
                    AspNetUser user = ApiManager.ValidUserBySecurityToken(securityToken);

                    VMDepartmentLogin departmentUser = new BLLDepartmentUsers().GetAll().Where(w => w.UserName == user.UserName).FirstOrDefault();

                    if (departmentUser == null) throw new Exception("User is not assigned in any department");

                    BLLToken dbToken = new BLLToken();

                    var tokenList = dbToken.GetByDepartmentId(departmentUser.department_id);

                    //return Json(new { success = true, tokenList }, JsonRequestBehavior.AllowGet);
                    JsonResult json = Json(new { success = true, tokenList }, JsonRequestBehavior.AllowGet);
                    responseJson = new JavaScriptSerializer().Serialize(json.Data);
                    return json;
                }
                catch (Exception ex)
                {

                    JsonResult json = Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
                    responseJson = new JavaScriptSerializer().Serialize(json.Data);
                    return json;
                }
                finally
                {
                    string requestJson = new JavaScriptSerializer().Serialize(Json(new { securityToken }, JsonRequestBehavior.AllowGet).Data);
                    //responseJson = new JavaScriptSerializer().Serialize(Json(new { securityToken }, JsonRequestBehavior.AllowGet).Data);
                    ApiManager.Loggin(securityToken, "GetTokenList", requestJson, requestJson);
                }
           
            

        }
        [HttpPost]
        public JsonResult GenerateNewToken(string mobile, int service_type_id, string securityToken)
        {
            string responseJson = String.Empty;
            try
            {
                AspNetUser user = ApiManager.ValidUserBySecurityToken(securityToken);

                VMDepartmentLogin departmentUser = new BLLDepartmentUsers().GetAll().Where(w => w.UserName == user.UserName).FirstOrDefault();

                if (departmentUser == null) throw new Exception("User is not assigned in any department");

                tblTokenQueue tokenObj = new tblTokenQueue()
                {
                    department_id = departmentUser.department_id,
                    contact_no = mobile,
                    service_type_id = service_type_id
                };
                new BLLToken().Create(tokenObj);

                //if (!String.IsNullOrEmpty(department.static_ip))
                //    dm.CreateTextFile(department.department_id, department.static_ip);

                NotifyDisplay.SendMessages(departmentUser.department_id, "", "", true, false, false, false, false, false);

                string token_id = tokenObj.token_id.ToString();
                string token_no = tokenObj.token_no_formated;
                string date = Convert.ToString(DateTime.Now);

                //return Json(new { success = true, tokenNo = token_no }, JsonRequestBehavior.AllowGet);
                JsonResult json = Json(new { success = true, tokenNo = token_no }, JsonRequestBehavior.AllowGet);
                responseJson = new JavaScriptSerializer().Serialize(json.Data);
                return json;
            }
            catch (Exception ex)
            {

                JsonResult json = Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
                responseJson = new JavaScriptSerializer().Serialize(json.Data);
                return json;
            }
            finally
            {
                string requestJson = new JavaScriptSerializer().Serialize(Json(new { mobile, service_type_id, securityToken }, JsonRequestBehavior.AllowGet).Data);
                ApiManager.Loggin(securityToken, "GenerateNewToken", requestJson, responseJson);
            }

        }

        [HttpPost]
        public JsonResult SendSMS(string mobile, string tokenNo, string securityToken)
        {
            string responseJson = String.Empty;
            try
            {
                ApiManager.ValidUserBySecurityToken(securityToken);
                BLLToken tokenManager = new BLLToken();

                tokenManager.SendSMS(mobile, tokenNo);
                JsonResult json = Json(new { success = true, message = "SMS Sent Succesfully" }, JsonRequestBehavior.AllowGet);
                responseJson = new JavaScriptSerializer().Serialize(json.Data);
                return json;
            }
            catch (Exception ex)
            {

                JsonResult json = Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
                responseJson = new JavaScriptSerializer().Serialize(json.Data);
                return json;
            }
            finally
            {
                string requestJson = new JavaScriptSerializer().Serialize(Json(new { mobile, tokenNo, securityToken }, JsonRequestBehavior.AllowGet).Data);
                ApiManager.Loggin(securityToken, "SendSMS", requestJson, responseJson);
            }
        }

        [HttpPost]
        public JsonResult GetDisplayInfo(string securityToken)
        {
            string responseJson = String.Empty;
            try
            {
                AspNetUser user = ApiManager.ValidUserBySecurityToken(securityToken);

                BLLPlayList playListManager = new BLLPlayList();
                var playLists = playListManager.GetByUserId(user.Id).FirstOrDefault();
                if (playLists == null)
                    throw new Exception("Playlist not found");

                BLLScroll scrollManager = new BLLScroll();
                var scroll = scrollManager.GetByUserId(user.Id);
                string scrollValue = "";
                if (scroll != null)
                    scrollValue = (scroll.is_active == 1 ? scroll.content_bn : "");

                //return Json(new { success = "true", tokenInProgress, nextTokens = nextToken, playLists.playlist_id, displayFooter }, JsonRequestBehavior.AllowGet);
                JsonResult json = Json(new { success = true, playLists.playlist_id, scroll = scrollValue }, JsonRequestBehavior.AllowGet);
                responseJson = new JavaScriptSerializer().Serialize(json.Data);
                return json;
            }
            catch (Exception ex)
            {

                JsonResult json = Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
                responseJson = new JavaScriptSerializer().Serialize(json.Data);
                return json;
            }
            finally
            {
                string requestJson = new JavaScriptSerializer().Serialize(Json(new { securityToken }, JsonRequestBehavior.AllowGet).Data);
                ApiManager.Loggin(securityToken, "GetDisplayInfo", requestJson, responseJson);
            }

        }

        [HttpPost]
        public JsonResult GetPlayList(string securityToken)
        {
            string responseJson = String.Empty;
            try
            {
                AspNetUser user = ApiManager.ValidUserBySecurityToken(securityToken);

                BLLPlayList playListManager = new BLLPlayList();
                var playLists = playListManager.GetByUserId(user.Id);
                if (playLists == null)
                    throw new Exception("Playlist not found");
                //return Json(new { success = "true", playLists }, JsonRequestBehavior.AllowGet);
                JsonResult json = Json(new { success = true, playLists }, JsonRequestBehavior.AllowGet);
                responseJson = new JavaScriptSerializer().Serialize(json.Data);
                return json;
            }
            catch (Exception ex)
            {
                JsonResult json = Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
                responseJson = new JavaScriptSerializer().Serialize(json.Data);
                return json;
            }
            finally
            {
                string requestJson = new JavaScriptSerializer().Serialize(Json(new { securityToken }, JsonRequestBehavior.AllowGet).Data);
                ApiManager.Loggin(securityToken, "GetPlayList", requestJson, responseJson);
            }

        }

        [HttpPost]
        public JsonResult GetScroll(string securityToken)
        {
            string responseJson = String.Empty;
            try
            {
                AspNetUser user = ApiManager.ValidUserBySecurityToken(securityToken);

                BLLScroll scrollManager = new BLLScroll();
                var scroll = scrollManager.GetByUserId(user.Id);

                JsonResult json;
                if (scroll==null)
                    json = Json(new { success = true, scroll = "" }, JsonRequestBehavior.AllowGet);
                else
                    json = Json(new { success = true, scroll = (scroll.is_active == 1 ? scroll.content_bn : "") }, JsonRequestBehavior.AllowGet);
                responseJson = new JavaScriptSerializer().Serialize(json.Data);
                return json;
            }
            catch (Exception ex)
            {
                JsonResult json = Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
                responseJson = new JavaScriptSerializer().Serialize(json.Data);
                return json;
            }
            finally
            {
                string requestJson = new JavaScriptSerializer().Serialize(Json(new { securityToken }, JsonRequestBehavior.AllowGet).Data);
                ApiManager.Loggin(securityToken, "GetPlayList", requestJson, responseJson);
            }

        }

        [HttpPost]
        public JsonResult GetDeviceDisplayInfo(string securityToken)
        {
            string responseJson = String.Empty;
            try
            {
                AspNetUser user = ApiManager.ValidUserBySecurityToken(securityToken);

                VMDepartmentLogin departmentUser = new BLLDepartmentUsers().GetAll().Where(w => w.UserName == user.UserName).FirstOrDefault();

                if (departmentUser == null) throw new Exception("User is not assigned in any department");

                DisplayManager dm = new DisplayManager();

                var tokenInProgress = dm.GetInProgressTokenList(departmentUser.department_id);

                string nextToken = dm.GetNextTokens(departmentUser.department_id);

                //return Json(new { success = "true", tokenInProgress, nextTokens = nextToken }, JsonRequestBehavior.AllowGet);
                JsonResult json = Json(new { success = true, tokenInProgress, nextTokens = nextToken }, JsonRequestBehavior.AllowGet);
                responseJson = new JavaScriptSerializer().Serialize(json.Data);
                return json;
            }
            catch (Exception ex)
            {

                JsonResult json = Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
                responseJson = new JavaScriptSerializer().Serialize(json.Data);
                return json;
            }
            finally
            {
                string requestJson = new JavaScriptSerializer().Serialize(Json(new { securityToken }, JsonRequestBehavior.AllowGet).Data);
                ApiManager.Loggin(securityToken, "GetDeviceDisplayInfo", requestJson, responseJson);
            }

        }

        [HttpPost]
        public JsonResult GetNextTokens(string securityToken)
        {
            string responseJson = String.Empty;
            try
            {
                AspNetUser user = ApiManager.ValidUserBySecurityToken(securityToken);

                VMDepartmentLogin departmentUser = new BLLDepartmentUsers().GetAll().Where(w => w.UserName == user.UserName).FirstOrDefault();

                if (departmentUser == null) throw new Exception("User is not assigned in any department");

                DisplayManager dm = new DisplayManager();

                string nextToken = dm.GetNextTokens(departmentUser.department_id);

                //return Json(new { success = "true", nextTokens = nextToken }, JsonRequestBehavior.AllowGet);
                JsonResult json = Json(new { success = true, nextTokens = nextToken }, JsonRequestBehavior.AllowGet);
                responseJson = new JavaScriptSerializer().Serialize(json.Data);
                return json;
            }
            catch (Exception ex)
            {

                JsonResult json = Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
                responseJson = new JavaScriptSerializer().Serialize(json.Data);
                return json;
            }
            finally
            {
                string requestJson = new JavaScriptSerializer().Serialize(Json(new { securityToken }, JsonRequestBehavior.AllowGet).Data);
                ApiManager.Loggin(securityToken, "GetNextTokens", requestJson, responseJson);
            }

        }

        

        //[HttpPost]
        //public JsonResult GetScroll(string securityToken)
        //{
        //    string responseJson = String.Empty;
        //    try
        //    {
        //        AspNetUser user = ApiManager.ValidUserBySecurityToken(securityToken);

        //        VMDepartmentLogin departmentUser = new BLLDepartmentUsers().GetAll().Where(w => w.UserName == user.UserName).FirstOrDefault();

        //        if (departmentUser == null) throw new Exception("User is not assigned in any department");

        //        BLLScroll footerManager = new BLLScroll();

        //        var displayFooter = footerManager.GetByDepartmentId(departmentUser.department_id);

        //        //return Json(new { success = "true", displayFooter }, JsonRequestBehavior.AllowGet);
        //        JsonResult json = Json(new { success = true, displayFooter }, JsonRequestBehavior.AllowGet);
        //        responseJson = new JavaScriptSerializer().Serialize(json.Data);
        //        return json;
        //    }
        //    catch (Exception ex)
        //    {
        //        JsonResult json = Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
        //        responseJson = new JavaScriptSerializer().Serialize(json.Data);
        //        return json;
        //    }
        //    finally
        //    {
        //        string requestJson = new JavaScriptSerializer().Serialize(Json(new { securityToken }, JsonRequestBehavior.AllowGet).Data);
        //        ApiManager.Loggin(securityToken, "GetScroll", requestJson, responseJson);
        //    }

        //}

        [AuthorizationFilter(Roles = "Admin")]
        public JsonResult GetAdminDashboard(int id=0)
        {
            
            //List<VMDashboardDepartmentAdminServicesTokens> ServicesTokens = new List<VMDashboardDepartmentAdminServicesTokens>();
            //List<VMDashboardDepartmentAdminServicesWaitings> ServicesWaitings = new List<VMDashboardDepartmentAdminServicesWaitings>();
            //var dashboardData = db.GetDepartmentAdminDashboard(id, ServicesTokens, ServicesWaitings);
            var DeviceStatusList = db.GetDeviceStatusList(id);
            //var DepartmentServiceList = db.GetDepartmentServiceList(id);
            //var DepartmentServiceDetailList = db.GetDepartmentServiceDetailList(id);

           return Json(new { success = true, deviceStatusList = DeviceStatusList }, JsonRequestBehavior.AllowGet);
           // return Json(new { success = true }, JsonRequestBehavior.AllowGet);


        }

        [AuthorizationFilter(Roles = "Department Admin")]
        public JsonResult GetDepartmentAdminDashboard()
        {
            SessionManager sm = new SessionManager(Session);
            //List<VMDashboardDepartmentAdminServicesTokens> ServicesTokens = new List<VMDashboardDepartmentAdminServicesTokens>();
            //List<VMDashboardDepartmentAdminServicesWaitings> ServicesWaitings = new List<VMDashboardDepartmentAdminServicesWaitings>();
            //var dashboardData = db.GetDepartmentAdminDashboard(sm.department_id, ServicesTokens, ServicesWaitings);
            var DeviceStatusList = db.GetDeviceStatusList(sm.department_id);
            //var DepartmentServiceList = db.GetDepartmentServiceList(sm.department_id);
            //var DepartmentServiceDetailList = db.GetDepartmentServiceDetailList(sm.department_id);

            return Json(new { success = true, deviceStatusList = DeviceStatusList}, JsonRequestBehavior.AllowGet);
        }



    }
}