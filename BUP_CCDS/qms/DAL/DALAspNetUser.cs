using System;
using System.Collections.Generic;
using System.Data;
using Oracle.DataAccess.Client;
using System.Linq;
using System.Web;
using qms.Models;
using qms.Utility;

namespace qms.DAL
{
    public class DALAspNetUser
    {
        OracleDataManager manager = new OracleDataManager();
        public DataTable GetAllUser()
        {
            try
            {

                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_AspNetUser_SelectAll");
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="securityToken"></param>
        /// <returns></returns>
        public void DeleteTable()
        {
            try
            {
                //OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                //param.Direction = ParameterDirection.Output;
                //manager.AddParameter(param);
                 manager.CallStoredProcedure_Delete("USP_ASPNETUSERT_DROP");
            }
            catch (Exception)
            {

                throw;
            }


        }
        public DataTable GetUserBySecurityCode(string securityToken)
        {
            try
            {
                manager.AddParameter(new OracleParameter() { ParameterName = "P_SecurityToken", Value = securityToken });
                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_AspNetUser_SelectByToken");
            }
            catch (Exception)
            {

                throw;
            }


        }


        public DataTable GetLoginInfo(string user_id)
        {
            try
            {
                manager.AddParameter(new OracleParameter() { ParameterName = "p_user_id", Value = user_id });
                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_AspNetUserLogin_SelectById");
            }
            catch (Exception)
            {

                throw;
            }


        }

        public void InsertLoginInfo(AspNetUserLogin loginInfo)
        {
            try
            {
                manager.AddParameter(new OracleParameter() { ParameterName = "P_LOGINPROVIDER", Value = loginInfo.LoginProvider });
                manager.AddParameter(new OracleParameter() { ParameterName = "P_PROVIDERKEY", Value = loginInfo.ProviderKey });
                manager.AddParameter(new OracleParameter() { ParameterName = "P_USERID", Value = loginInfo.UserId });
                manager.AddParameter(new OracleParameter() { ParameterName = "P_DEPARTMENT_ID", Value = loginInfo.department_id });

                manager.CallStoredProcedure_Insert("USP_AspNetUserLogin_Insert");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable GetLoginAttemptsInfo(int department_id, string user_id, int device_id, int is_success, DateTime start_date, DateTime end_date)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_department_id", department_id));
                manager.AddParameter(new OracleParameter("p_user_id", user_id));
                manager.AddParameter(new OracleParameter("p_device_id", device_id));
                manager.AddParameter(new OracleParameter("p_is_success", is_success));
                manager.AddParameter(new OracleParameter("p_start_date", start_date));
                manager.AddParameter(new OracleParameter("p_end_date", end_date));

                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_AspNetUserLAttempts_Select");
            }
            catch (Exception)
            {

                throw;
            }


        }

        public void InsertLoginAttemptInfo(AspNetUserLoginAttempts loginAttempt)
        {
            try
            {
                manager.AddParameter(new OracleParameter() { ParameterName = "P_USERNAME", Value = loginAttempt.UserName });
                manager.AddParameter(new OracleParameter() { ParameterName = "P_LOGINPROVIDER", Value = loginAttempt.LoginProvider });
                manager.AddParameter(new OracleParameter() { ParameterName = "P_IS_SUCCESS", Value = loginAttempt.is_success });
                manager.AddParameter(new OracleParameter() { ParameterName = "P_IP_ADDRESS", Value = loginAttempt.ip_address });
                manager.AddParameter(new OracleParameter() { ParameterName = "P_MACHINE_NAME", Value = loginAttempt.machine_name });

                manager.CallStoredProcedure_Insert("USP_AspNetUserLAttempts_Insert");
            }
            catch (Exception)
            {

                throw;
            }
        }

        //public void UpdateLoginInfo(string loginProvider, int device_id)
        //{
        //    try
        //    {
        //        manager.AddParameter(new OracleParameter() { ParameterName = "P_loginProvider", Value = loginProvider });
        //        manager.AddParameter(new OracleParameter() { ParameterName = "P_DEVICE_ID", Value = device_id });

        //        manager.CallStoredProcedure_Delete("USP_AspNetUserLogin_Update");
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        public void UpdateLoginInfo(string loginProvider, int department_id)
        {
            try
            {
                manager.AddParameter(new OracleParameter() { ParameterName = "P_loginProvider", Value = loginProvider });
                manager.AddParameter(new OracleParameter() { ParameterName = "P_DEPARTMENT_ID", Value = department_id });

                manager.CallStoredProcedure_Delete("USP_AspNetUserLogin_Update");
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void UpdateDepartmentAdminLoginInfo(string loginProvider, int department_id)
        {
            try
            {
                manager.AddParameter(new OracleParameter() { ParameterName = "P_loginProvider", Value = loginProvider });
                manager.AddParameter(new OracleParameter() { ParameterName = "P_DEPARTMENT_ID", Value = department_id });

                manager.CallStoredProcedure_Delete("USP_DepartmentAdminLogin_Update");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void UserForceChangeConfirmed(string user_id, bool isForceChangeConfirmed)
        {
            try
            {
                manager.AddParameter(new OracleParameter() { ParameterName = "p_user_id", Value = user_id });
                manager.AddParameter(new OracleParameter() { ParameterName = "p_forcechangeconfirmed", Value = (isForceChangeConfirmed ? 1 : 0) });

                manager.CallStoredProcedure_Delete("USP_AspNetUser_FCConfirm");
            }
            catch (Exception)
            {

                throw;
            }
        }

        
        public void InsertPasswordInfo(string user_id, string passwordhash)
        {
            try
            {
                manager.AddParameter(new OracleParameter() { ParameterName = "P_USERID", Value = user_id });
                manager.AddParameter(new OracleParameter() { ParameterName = "P_CHANGETIMEHASH", Value = Cryptography.Encrypt(DateTime.Now.ToString(),true) });
                manager.AddParameter(new OracleParameter() { ParameterName = "P_PASSWORDHASH", Value = passwordhash });

                manager.CallStoredProcedure_Insert("USP_AspNetUserPasswords_Insert");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable GetPasswordInfo(string user_id)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_user_id", user_id));

                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_AspNetUserPasswords_Select");
            }
            catch (Exception)
            {

                throw;
            }


        }

        public void SetActivation(string user_id, int is_active)
        {
            try
            {
                manager.AddParameter(new OracleParameter() { ParameterName = "p_user_id", Value = user_id });
                manager.AddParameter(new OracleParameter() { ParameterName = "p_is_active", Value = is_active });

                manager.CallStoredProcedure_Delete("USP_AspNetUser_SetActivation");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void SetActiveDirectoryUser(string user_id, int is_active_directory_user)
        {
            try
            {
                manager.AddParameter(new OracleParameter() { ParameterName = "p_user_id", Value = user_id });
                manager.AddParameter(new OracleParameter() { ParameterName = "p_is_active_directory_user", Value = is_active_directory_user });

                manager.CallStoredProcedure_Delete("USP_AspNetUser_SetAD");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void UpdateLogoutInfo(string loginProvider, int? logout_type_id, string logoutReason)
        {
            try
            {
                manager.AddParameter(new OracleParameter() { ParameterName = "P_loginProvider", Value = loginProvider });
                if(logout_type_id.HasValue) manager.AddParameter(new OracleParameter() { ParameterName = "P_logout_type_id", Value = logout_type_id.Value });
                else manager.AddParameter(new OracleParameter() { ParameterName = "P_logout_type_id", Value = DBNull.Value });
                manager.AddParameter(new OracleParameter() { ParameterName = "P_logout_reason", Value = logoutReason });

                manager.CallStoredProcedure_Delete("USP_AspNetUserLogOut");
            }
            catch (Exception)
            {

                throw;
            }
        }


        public void DeleteLoginInfo(string loginProvider)
        {
            try
            {
                manager.AddParameter(new OracleParameter() { ParameterName = "P_loginProvider", Value = loginProvider });
                
                manager.CallStoredProcedure_Delete("USP_AspNetUserLogin_Delete");
            }
            catch (Exception)
            {

                throw;
            }
        }


        public DataTable GetSessionInfoByUserName(string userName)
        {
            try
            {
                manager.AddParameter(new OracleParameter("P_USER_NAME", userName));
                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_SessionInfo_ByUserName");
            }
            catch (Exception)
            {

                throw;
            }


        }
    }
}