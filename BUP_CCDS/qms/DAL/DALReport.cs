using qms.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Oracle.DataAccess.Client;
using System.Linq;
using System.Web;
using qms.Utility;

namespace qms.DAL
{
    public class DALReport
    {
        OracleDataManager manager = new OracleDataManager();
        public DataTable LocalCustomerReport(int department_id, string user_id, int device_id, int customer_type_id, int service_sub_type_id, DateTime start_date, DateTime end_date)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_department_id", department_id));
                manager.AddParameter(new OracleParameter("p_user_id", user_id));
                manager.AddParameter(new OracleParameter("p_device_id", device_id));
                manager.AddParameter(new OracleParameter("p_customer_type_id", customer_type_id));
                manager.AddParameter(new OracleParameter("p_service_sub_type_id", service_sub_type_id));
                manager.AddParameter(new OracleParameter("p_start_date", start_date));
                manager.AddParameter(new OracleParameter("p_end_date", end_date));

                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("RSP_LocalCustomer");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable BreakReport(int department_id, string user_id, int device_id, int break_type_id, DateTime start_date, DateTime end_date)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_department_id", department_id));
                manager.AddParameter(new OracleParameter("p_user_id", user_id));
                manager.AddParameter(new OracleParameter("p_device_id", device_id));
                manager.AddParameter(new OracleParameter("p_break_type_id", break_type_id));
                manager.AddParameter(new OracleParameter("p_start_date", start_date));
                manager.AddParameter(new OracleParameter("p_end_date", end_date));

                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("RSP_BrekReport");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable AgentWiseSummaryReport(int department_id, string user_id, int device_id, int service_sub_type_id, DateTime start_date, DateTime end_date)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_department_id", department_id));
                manager.AddParameter(new OracleParameter("p_user_id", user_id));
                manager.AddParameter(new OracleParameter("p_device_id", device_id));
                manager.AddParameter(new OracleParameter("p_service_sub_type_id", service_sub_type_id));
                manager.AddParameter(new OracleParameter("p_start_date", start_date));
                manager.AddParameter(new OracleParameter("p_end_date", end_date));

                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("RSP_AgentWiseSummury");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GeneralSearchReport(int department_id, string user_id, int device_id, string msisdn_no, int service_sub_type_id, DateTime start_date, DateTime end_date, string token_no)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_department_id", department_id));
                manager.AddParameter(new OracleParameter("p_user_id", user_id));
                manager.AddParameter(new OracleParameter("p_device_id", device_id));
                manager.AddParameter(new OracleParameter("p_customer_msisdn_no", msisdn_no));
                manager.AddParameter(new OracleParameter("p_service_sub_type_id", service_sub_type_id));
                manager.AddParameter(new OracleParameter("p_start_date", start_date));
                manager.AddParameter(new OracleParameter("p_end_date", end_date));
                manager.AddParameter(new OracleParameter("p_token_no", token_no));
                manager.AddParameter(new OracleParameter("p_padding_left", ApplicationSetting.PaddingLeft ));

                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("RSP_GeneralSearch");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable ServiceSummaryReport(int department_id, string user_id, int device_id, int service_sub_type_id, DateTime start_date, DateTime end_date)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_department_id", department_id));
                manager.AddParameter(new OracleParameter("p_user_id", user_id));
                manager.AddParameter(new OracleParameter("p_device_id", device_id));
                manager.AddParameter(new OracleParameter("p_service_sub_type_id", service_sub_type_id));
                manager.AddParameter(new OracleParameter("p_start_date", start_date));
                manager.AddParameter(new OracleParameter("p_end_date", end_date));

                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("RSP_ServiceSummury");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable SingleVSMultipleVisitedReport(int department_id, string user_id, int device_id, int service_sub_type_id, DateTime start_date, DateTime end_date)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_department_id", department_id));
                manager.AddParameter(new OracleParameter("p_user_id", user_id));
                manager.AddParameter(new OracleParameter("p_device_id", device_id));
                manager.AddParameter(new OracleParameter("p_service_sub_type_id", service_sub_type_id));
                manager.AddParameter(new OracleParameter("p_start_date", start_date));
                manager.AddParameter(new OracleParameter("p_end_date", end_date));

                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("RSP_SingleVSMultipleVisited");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable TokenExceedingReport(int department_id, string user_id, int device_id, int service_sub_type_id, DateTime start_date, DateTime end_date, int flag)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_department_id", department_id));
                manager.AddParameter(new OracleParameter("p_user_id", user_id));
                manager.AddParameter(new OracleParameter("p_device_id", device_id));
                manager.AddParameter(new OracleParameter("p_service_sub_type_id", service_sub_type_id));
                manager.AddParameter(new OracleParameter("p_start_date", start_date));
                manager.AddParameter(new OracleParameter("p_end_date", end_date));
                manager.AddParameter(new OracleParameter("p_flag", flag));

                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("RSP_TokenExceeding");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable TopNServiceReport(int department_id, string user_id, int device_id, DateTime start_date, DateTime end_date, int topN)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_department_id", department_id));
                manager.AddParameter(new OracleParameter("p_user_id", user_id));
                manager.AddParameter(new OracleParameter("p_device_id", device_id));
                manager.AddParameter(new OracleParameter("p_start_date", start_date));
                manager.AddParameter(new OracleParameter("p_end_date", end_date));
                manager.AddParameter(new OracleParameter("p_top_n", topN));

                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("RSP_TopNService");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable LogoutDetailReport(int department_id, string user_id, int device_id, DateTime start_date, DateTime end_date)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_department_id", department_id));
                manager.AddParameter(new OracleParameter("p_user_id", user_id));
                manager.AddParameter(new OracleParameter("p_device_id", device_id));
                manager.AddParameter(new OracleParameter("p_start_date", start_date));
                manager.AddParameter(new OracleParameter("p_end_date", end_date));

                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("RSP_LogOutDetails");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable LoginAttemptDetailsReport(int department_id, string user_id, int device_id, int is_success, DateTime start_date, DateTime end_date)
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

                return manager.CallStoredProcedure_Select("RSP_LoginAttemptDetails");
            }
            catch (Exception)
            {

                throw;
            }


        }
    }
}