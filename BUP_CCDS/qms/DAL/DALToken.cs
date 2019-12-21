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
    public class DALToken
    {
        OracleDataManager manager = new OracleDataManager();
        public DataTable GetAll()
        {

            try
            {
                
                OracleParameter param = new OracleParameter("po_cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_Token_SelectAll");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable GetBy(DateTime from_date, DateTime to_date, int token_id=0, int department_id=0, int service_type_id=0, int service_status_id=0, 
            int device_id=0, string user_id=null, string token_prefix=null, string contact_no=null)
        {

            try
            {
                manager.AddParameter(new OracleParameter() { ParameterName = "P_from_date", Value = from_date });
                manager.AddParameter(new OracleParameter() { ParameterName = "P_to_date", Value = to_date });
                manager.AddParameter(new OracleParameter() { ParameterName = "P_token_id", Value = token_id });
                manager.AddParameter(new OracleParameter() { ParameterName = "P_department_id", Value = department_id });
                manager.AddParameter(new OracleParameter() { ParameterName = "P_service_type_id", Value = service_type_id });
                manager.AddParameter(new OracleParameter() { ParameterName = "P_service_status_id", Value = service_status_id });
                manager.AddParameter(new OracleParameter() { ParameterName = "P_device_id", Value = device_id });
                manager.AddParameter(new OracleParameter() { ParameterName = "P_user_id", Value = user_id });
                manager.AddParameter(new OracleParameter() { ParameterName = "P_token_prefix", Value = token_prefix });
                manager.AddParameter(new OracleParameter() { ParameterName = "P_contact_no", Value = contact_no });

                OracleParameter param = new OracleParameter("po_cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_Token_SelectBy");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable GetSkipped(int? department_id, string user_id)
        {

            try
            {
                manager.AddParameter(new OracleParameter() { ParameterName = "P_department_id", Value = department_id });
                manager.AddParameter(new OracleParameter() { ParameterName = "P_USER_ID", Value = user_id });

                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_TOKEN_SelectSkipped");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable GetByDepartmentId(int department_id)
        {

            try
            {
                manager.AddParameter(new OracleParameter() { ParameterName = "P_department_id", Value = department_id });
                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_TOKEN_SelectByDepartmentId");
            }
            catch (Exception)
            {

                throw;
            }
        }
        public string GetNextTokenList(int department_id)
        {

            try
            {
                manager.AddParameter(new OracleParameter("p_department_id", department_id));
                manager.AddParameter(new OracleParameter("p_padding_left", ApplicationSetting.PaddingLeft));
                OracleParameter param_NEXT_TOKENS = new OracleParameter("PO_NEXT_TOKENS", OracleDbType.Varchar2,1000);
                param_NEXT_TOKENS.Direction = ParameterDirection.Output;
                manager.AddParameter(param_NEXT_TOKENS);

                manager.CallStoredProcedure("USP_GetNextTokenList");

                return ((Oracle.DataAccess.Types.OracleString)param_NEXT_TOKENS.Value).Value;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public DataTable GetProgressTokenList(int department_id)
        {

            try
            {
                manager.AddParameter(new OracleParameter("p_department_id", department_id));
                OracleParameter param = new OracleParameter("cur", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_GetInProgressTokenList");
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void Insert(tblTokenQueue token)
        {
            try
            {
               
                
                OracleParameter param_token_id = new OracleParameter("po_token_id", OracleDbType.Decimal);
                param_token_id.Direction = ParameterDirection.Output;
                manager.AddParameter(param_token_id);
                OracleParameter param_token_no = new OracleParameter("po_token_no", OracleDbType.Decimal);
                param_token_no.Direction = ParameterDirection.Output;
                manager.AddParameter(param_token_no);
                OracleParameter param_token_prefix = new OracleParameter("po_token_prefix", OracleDbType.Varchar2, 20);
                param_token_prefix.Direction = ParameterDirection.Output;
                manager.AddParameter(param_token_prefix);
                OracleParameter param_DEVICE_ID = new OracleParameter("PO_DEVICE_ID", OracleDbType.Decimal);
                param_DEVICE_ID.Direction = ParameterDirection.Output;
                manager.AddParameter(param_DEVICE_ID);

                MapParameters(token);

                manager.CallStoredProcedure("USP_Token_Insert");

                token.token_id = (long)((Oracle.DataAccess.Types.OracleDecimal)param_token_id.Value).Value;
                token.token_no = (int)((Oracle.DataAccess.Types.OracleDecimal)param_token_no.Value).Value;
                token.token_prefix = ((Oracle.DataAccess.Types.OracleString)param_token_prefix.Value).Value;
                token.device_id = (int)((Oracle.DataAccess.Types.OracleDecimal)param_DEVICE_ID.Value).Value;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ReInitiate(long token_id)
        {
            try
            {
                
                manager.AddParameter(new OracleParameter("P_token_id", token_id));
                manager.CallStoredProcedure("USP_TOKEN_RE_INITIATE");

               

            }
            catch (Exception)
            {
                throw;
            }
        }
        public void AssignToMe(long token_id,int device_id)
        {
            try
            {

                manager.AddParameter(new OracleParameter("P_token_id", token_id));
                manager.AddParameter(new OracleParameter("P_device_id", device_id));

                manager.CallStoredProcedure("USP_TOKEN_RE_ASSIGNTOME");



            }
            catch (Exception)
            {
                throw;
            }
        }
        private void MapParameters(tblTokenQueue token)
        {
            manager.AddParameter(new OracleParameter("p_department_id", token.department_id));
            manager.AddParameter(new OracleParameter("p_service_type_id", token.service_type_id));
            manager.AddParameter(new OracleParameter("p_contact_no", token.contact_no));
            manager.AddParameter(new OracleParameter("p_default_token_prefix", ApplicationSetting.defaultTokenPrefix ));
        }
        
    }
}