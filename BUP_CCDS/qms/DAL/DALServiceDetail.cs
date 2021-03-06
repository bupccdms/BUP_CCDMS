﻿using qms.Models;
using qms.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using Oracle.DataAccess.Client;
using System.Linq;
using System.Web;

namespace qms.DAL
{
    public class DALServiceDetail
    {
        OracleDataManager manager = new OracleDataManager();

        public DataTable GetAllCurrentDate(int? department_id, string user_id)
        {
            try
            {
                manager.AddParameter(new OracleParameter() { ParameterName = "P_DEPARTMENT_ID", Value = department_id });
                manager.AddParameter(new OracleParameter() { ParameterName = "P_USER_ID", Value = user_id });

                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_ServiceDetail_SelectCDate");
            }
            catch (Exception)
            {

                throw;
            }


        }


        public DataTable GetAllServices(int? department_id, string user_id)
        {
            try
            {
                manager.AddParameter(new OracleParameter() { ParameterName = "P_DEPARTMENT_ID", Value = department_id });
                manager.AddParameter(new OracleParameter() { ParameterName = "P_USER_ID", Value = user_id });

                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_ServiceDetail_SelectAll");
            }
            catch (Exception)
            {

                throw;
            }


        }

        public DataTable GetByCustomerID(long customer_id)
        {
            try
            {
                manager.AddParameter(new OracleParameter() { ParameterName = "P_CUSTOMER_ID", Value = customer_id });

                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_ServiceDetail_SelectByCId");
            }
            catch (Exception)
            {

                throw;
            }


        }

        public DataTable GetNewToken(int department_id, int device_id, string userid, out long token_id, out string token_prefix, out int token_no, out string contact_no, out string service_type, out DateTime start_time, out string customer_name, out string address, out DateTime generate_time,out int is_break)
        {
            try
            {
                manager.AddParameter(new OracleParameter("P_DEPARTMENT_ID", department_id));
                manager.AddParameter(new OracleParameter("P_DEVICE_ID", device_id));
                manager.AddParameter(new OracleParameter("P_USER_ID", userid));
                OracleParameter param_TOKEN_ID = new OracleParameter("PO_TOKEN_ID", OracleDbType.Decimal);
                param_TOKEN_ID.Direction = ParameterDirection.Output;
                manager.AddParameter(param_TOKEN_ID);

                OracleParameter param_TOKEN_PREFIX = new OracleParameter("PO_TOKEN_PREFIX", OracleDbType.Varchar2,2);
                param_TOKEN_PREFIX.Direction = ParameterDirection.Output;
                manager.AddParameter(param_TOKEN_PREFIX);
                OracleParameter param_TOKEN_NO = new OracleParameter("PO_TOKEN_NO", OracleDbType.Decimal);
                param_TOKEN_NO.Direction = ParameterDirection.Output;
                manager.AddParameter(param_TOKEN_NO);
                OracleParameter param_CONTACT_NO = new OracleParameter("PO_CONTACT_NO", OracleDbType.Varchar2, 32767);
                param_CONTACT_NO.Direction = ParameterDirection.Output;
                manager.AddParameter(param_CONTACT_NO);
                OracleParameter param_SERVICE_TYPE = new OracleParameter("PO_SERVICE_TYPE", OracleDbType.Varchar2, 100);
                param_SERVICE_TYPE.Direction = ParameterDirection.Output;
                manager.AddParameter(param_SERVICE_TYPE);
                OracleParameter param_START_TIME = new OracleParameter("PO_START_TIME", OracleDbType.Date);
                param_START_TIME.Direction = ParameterDirection.Output;
                manager.AddParameter(param_START_TIME);
                OracleParameter param_CUSTOMER_NAME = new OracleParameter("PO_CUSTOMER_NAME", OracleDbType.Varchar2, 500);
                param_CUSTOMER_NAME.Direction = ParameterDirection.Output;
                manager.AddParameter(param_CUSTOMER_NAME);
                OracleParameter param_ADDRESS = new OracleParameter("PO_ADDRESS", OracleDbType.Varchar2, 250);
                param_ADDRESS.Direction = ParameterDirection.Output;
                manager.AddParameter(param_ADDRESS);
                OracleParameter param_SERVICE_TIME = new OracleParameter("PO_SERVICE_DATE", OracleDbType.Date);
                param_SERVICE_TIME.Direction = ParameterDirection.Output;
                manager.AddParameter(param_SERVICE_TIME);
                OracleParameter param_Break = new OracleParameter("PO_IS_BREAK_ASSIGNED", OracleDbType.Int32);
                param_Break.Direction = ParameterDirection.Output;
                manager.AddParameter(param_Break);
                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                DataTable dt = manager.CallStoredProcedure_Select("USP_SERVICEDETAIL_NEWCALL");
                if (dt.Rows.Count > 0)
                {
                    token_id = (long)((Oracle.DataAccess.Types.OracleDecimal)param_TOKEN_ID.Value).Value;
                    token_prefix = (((Oracle.DataAccess.Types.OracleString)param_TOKEN_PREFIX.Value).IsNull == true ? "" : param_TOKEN_PREFIX.Value.ToString());
                    token_no = (int)((Oracle.DataAccess.Types.OracleDecimal)param_TOKEN_NO.Value).Value;
                    contact_no = param_CONTACT_NO.Value.ToString();
                    service_type = param_SERVICE_TYPE.Value.ToString();
                    start_time = ((Oracle.DataAccess.Types.OracleDate)param_START_TIME.Value).Value;
                    customer_name = (((Oracle.DataAccess.Types.OracleString)param_CUSTOMER_NAME.Value).IsNull == true ? "" : param_CUSTOMER_NAME.Value.ToString());
                    is_break = (int)((Oracle.DataAccess.Types.OracleDecimal)param_Break.Value).Value;
                    address = (((Oracle.DataAccess.Types.OracleString)param_ADDRESS.Value).IsNull == true ? "" : param_ADDRESS.Value.ToString());
                    generate_time = ((Oracle.DataAccess.Types.OracleDate)param_SERVICE_TIME.Value).Value;
                }
                else
                {
                    token_id = 0;
                    token_prefix = "";
                    token_no = 0;
                    contact_no = null;
                    service_type = null;
                    start_time = DateTime.Now;
                    customer_name = null;
                    is_break = (int)((Oracle.DataAccess.Types.OracleDecimal)param_Break.Value).Value;
                    address = null;
                    generate_time = DateTime.Now; 
                }
                return dt;
            }
            catch (Exception)
            {

                throw;
            }


        }
        public DataTable CallManualToken(int department_id, int device_id, string userid, string token_no, out long token_id, out string contact_no, out string service_type, out DateTime start_time, out string customer_name, out string address)
        {
            try
            {
                manager.AddParameter(new OracleParameter("P_DEPARTMENT_ID", department_id));
                manager.AddParameter(new OracleParameter("P_DEVICE_ID", device_id));
                manager.AddParameter(new OracleParameter("P_USER_ID", userid));
                manager.AddParameter(new OracleParameter("P_TOKEN_NO", token_no));
                OracleParameter param_TOKEN_ID = new OracleParameter("PO_TOKEN_ID", OracleDbType.Decimal);
                param_TOKEN_ID.Direction = ParameterDirection.Output;
                manager.AddParameter(param_TOKEN_ID);

                OracleParameter param_CONTACT_NO = new OracleParameter("PO_CONTACT_NO", OracleDbType.Varchar2, 32767);
                param_CONTACT_NO.Direction = ParameterDirection.Output;
                manager.AddParameter(param_CONTACT_NO);
                OracleParameter param_SERVICE_TYPE = new OracleParameter("PO_SERVICE_TYPE", OracleDbType.Varchar2, 100);
                param_SERVICE_TYPE.Direction = ParameterDirection.Output;
                manager.AddParameter(param_SERVICE_TYPE);
                OracleParameter param_START_TIME = new OracleParameter("PO_START_TIME", OracleDbType.Date);
                param_START_TIME.Direction = ParameterDirection.Output;
                manager.AddParameter(param_START_TIME);
                OracleParameter param_CUSTOMER_NAME = new OracleParameter("PO_CUSTOMER_NAME", OracleDbType.Varchar2, 500);
                param_CUSTOMER_NAME.Direction = ParameterDirection.Output;
                manager.AddParameter(param_CUSTOMER_NAME);
                OracleParameter param_ADDRESS = new OracleParameter("PO_ADDRESS", OracleDbType.Varchar2, 250);
                param_ADDRESS.Direction = ParameterDirection.Output;
                manager.AddParameter(param_ADDRESS);
                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                DataTable dt = manager.CallStoredProcedure_Select("USP_SERVICEDETAIL_MANUALCALL");
                if (dt.Rows.Count > 0)
                {
                    token_id = (long)((Oracle.DataAccess.Types.OracleDecimal)param_TOKEN_ID.Value).Value;
                    contact_no = param_CONTACT_NO.Value.ToString();
                    service_type = param_SERVICE_TYPE.Value.ToString();
                    start_time = ((Oracle.DataAccess.Types.OracleDate)param_START_TIME.Value).Value;
                    customer_name = (((Oracle.DataAccess.Types.OracleString)param_CUSTOMER_NAME.Value).IsNull == true ? "" : param_CUSTOMER_NAME.Value.ToString());
                    address = (((Oracle.DataAccess.Types.OracleString)param_ADDRESS.Value).IsNull == true ? "" : param_ADDRESS.Value.ToString());
                }
                else
                {
                    token_id = 0;
                    contact_no = null;
                    service_type = null;
                    start_time = DateTime.Now;
                    customer_name = null;
                    address = null;
                }
                return dt;
            }
            catch (Exception)
            {

                throw;
            }


        }
        public int CancelToken(long token_id)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_token_id", token_id));
                //OracleParameter param = new OracleParameter("po_PKValue", OracleDbType.RefCursor);
                //param.Direction = ParameterDirection.Output;

                return (int) manager.CallStoredProcedure_Insert("USP_Token_Cancel");
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void Transfer(int department_id, string device_no,long token_id)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_department_id", department_id));
                manager.AddParameter(new OracleParameter("p_device_no", device_no));
                manager.AddParameter(new OracleParameter("p_token_id", token_id));

                manager.CallStoredProcedure("USP_SERVICEDETAIL_TRANSFER");
            }
            catch (Exception)
            {

                throw;
            }
        }
        public int Insert(VMServiceDetails servicedetail)
        {
            try
            {
                MapParameters(servicedetail);
                long? service_id = manager.CallStoredProcedure_Insert("USP_SERVICEDETAIL_INSERT");
                if (service_id.HasValue) return (int)service_id.Value;
                else return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int AddService(VMServiceDetails servicedetail)
        {
            try
            {
                MapParameters(servicedetail);
                long? service_id = manager.CallStoredProcedure_Insert("USP_SERVICEDETAIL_ADDSERVICE");
                if (service_id.HasValue) return (int)service_id.Value;
                else return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void MapParameters(VMServiceDetails servicedetail)
        {
            manager.AddParameter(new OracleParameter("P_TOKEN_ID", servicedetail.token_id));
            manager.AddParameter(new OracleParameter("P_CONTACT_NO", servicedetail.contact_no));
            manager.AddParameter(new OracleParameter("P_IS_PRIMARY_CONTACT", servicedetail.is_primary_contact));
            manager.AddParameter(new OracleParameter("P_START_TIME", servicedetail.start_time));
            manager.AddParameter(new OracleParameter("P_SERVICE_SUB_TYPE_ID", servicedetail.service_sub_type_id));
            manager.AddParameter(new OracleParameter("P_ISSUES", servicedetail.issues));
            manager.AddParameter(new OracleParameter("P_SOLUTIONS", servicedetail.solutions));
            manager.AddParameter(new OracleParameter("P_CUSTOMER_NAME", servicedetail.customer_name));
            manager.AddParameter(new OracleParameter("P_ADDRESS", servicedetail.address));
            manager.AddParameter(new OracleParameter("P_DEVICE_ID", servicedetail.device_id));
            manager.AddParameter(new OracleParameter("P_USER_ID", servicedetail.user_id));



        }
    }
}