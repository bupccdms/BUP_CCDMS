using System;
using System.Collections.Generic;
using System.Data;
using Oracle.DataAccess.Client;
using System.Linq;
using System.Web;

namespace qms.DAL
{
    public class DALDashboard
    {
        OracleDataManager manager = new OracleDataManager();
        public DataTable GetAll()
        {
            try
            {

                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_Device_Dashboard");
            }
            catch (Exception)
            {

                throw;
            }


        }
        public DataSet GetDepartmentAdminDashboard( int department_id)
        {
            try
            {
                manager.AddParameter(new OracleParameter("P_DEPARTMENT_ID", department_id));
                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                DataTable dtDevice = manager.CallStoredProcedure_Select("DSP_CHART_DEVICES_TOKENS");
                DataTable dtServicesTokens = GetDepartmentAdminDashboardServicesTokens(department_id);
                DataTable dtServicesWaitings = GetDepartmentAdminDashboardServicesWaitings(department_id);

                dtDevice.TableName = "DEVICES";
                dtServicesTokens.TableName = "ServicesTokens";
                dtServicesWaitings.TableName = "ServicesWaitings";
                DataSet ds = new DataSet();
                ds.Tables.Add(dtDevice);
                ds.Tables.Add(dtServicesTokens);
                ds.Tables.Add(dtServicesWaitings);
                return ds;
            }
            catch (Exception)
            {

                throw;
            }


        }

        public DataTable GetDepartmentAdminDashboardServicesTokens(int department_id)
        {
            try
            {
                manager = new OracleDataManager();
                manager.AddParameter(new OracleParameter("P_DEPARTMENT_ID", department_id));
                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("DSP_CHART_SERVICES_TOKENS");
            }
            catch (Exception)
            {

                throw;
            }


        }

        public DataTable GetDepartmentAdminDashboardServicesWaitings(int department_id)
        {
            try
            {
                manager = new OracleDataManager();
                manager.AddParameter(new OracleParameter("P_DEPARTMENT_ID", department_id));
                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("DSP_CHART_SERVICES_WAITINGS");
            }
            catch (Exception)
            {

                throw;
            }


        }

        public DataTable GetAdminDashboard()
        {
            try
            {
                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_DASHBOARD_ADMIN");
            }
            catch (Exception)
            {

                throw;
            }


        }

        public DataTable GetDepartmentServiceList(int department_id)
        {
            try
            {
                manager = new OracleDataManager();
                manager.AddParameter(new OracleParameter("p_department_id", department_id));
                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("DSP_Department_ServiceList");
            }
            catch (Exception)
            {

                throw;
            }


        }

        public DataTable GetDeviceStatusList(int department_id)
        {
            try
            {
                manager = new OracleDataManager();
                manager.AddParameter(new OracleParameter("p_department_id", department_id));
                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("DSP_DEVICE_STATUS_LIST");
            }
            catch (Exception)
            {

                throw;
            }


        }

        public DataTable GetDepartmentServiceDetailList(int department_id)
        {
            try
            {
                manager = new OracleDataManager();
                manager.AddParameter(new OracleParameter("p_department_id", department_id));
                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("DSP_Department_ServiceDetailList");
            }
            catch (Exception)
            {

                throw;
            }


        }

        public DataTable GetDeviceServiceList(int device_id, out string serving_time)
        {
            try
            {
                manager = new OracleDataManager();

                

                manager.AddParameter(new OracleParameter("p_device_id", device_id));

                OracleParameter param_SERVINGTIME = new OracleParameter("PO_SERVINGTIME", OracleDbType.Varchar2,50);
                param_SERVINGTIME.Direction = ParameterDirection.Output;
                manager.AddParameter(param_SERVINGTIME);

                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                DataTable dt = manager.CallStoredProcedure_Select("DSP_Device_ServiceList");
                if (param_SERVINGTIME.Value != DBNull.Value)
                    serving_time = ((Oracle.DataAccess.Types.OracleString)param_SERVINGTIME.Value).Value;
                else serving_time = "00:00:00";

                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable GetDeviceTokenList(int device_id)
        {
            try
            {
                manager = new OracleDataManager();

                manager.AddParameter(new OracleParameter("p_device_id", device_id));

                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                DataTable dt = manager.CallStoredProcedure_Select("DSP_Device_TokenList");
               

                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable GetUserServiceList(string user_id, out string serving_time)
        {
            try
            {
                manager = new OracleDataManager();
                manager.AddParameter(new OracleParameter("p_user_id", user_id));

                OracleParameter param_SERVINGTIME = new OracleParameter("PO_SERVINGTIME", OracleDbType.Varchar2, 50);
                param_SERVINGTIME.Direction = ParameterDirection.Output;
                manager.AddParameter(param_SERVINGTIME);

                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                DataTable dt = manager.CallStoredProcedure_Select("DSP_User_ServiceList");
                if (param_SERVINGTIME.Value != DBNull.Value)
                    serving_time = ((Oracle.DataAccess.Types.OracleString)param_SERVINGTIME.Value).Value;
                else serving_time = "00:00:00";

                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public DataTable GetUserTokenList(string user_id)
        {
            try
            {
                manager = new OracleDataManager();
                manager.AddParameter(new OracleParameter("p_user_id", user_id));

                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                DataTable dt = manager.CallStoredProcedure_Select("DSP_User_TokenList");

                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetUserServiceDetailList(string user_id)
        {
            try
            {
                manager = new OracleDataManager();
                manager.AddParameter(new OracleParameter("p_user_id", user_id));
                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("DSP_User_ServiceDetailList");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}