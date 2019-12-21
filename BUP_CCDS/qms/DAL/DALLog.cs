using qms.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Oracle.DataAccess.Client;
using System.Linq;
using System.Web;

namespace qms.DAL
{
    public class DALLog
    {
        OracleDataManager manager = new OracleDataManager();
        
        
        public int ApiRequestLogInsert(ApiRequestLog requestLog)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_loginprovider", requestLog.loginprovider));
                manager.AddParameter(new OracleParameter("p_methode_name", requestLog.methode_name));
                //manager.AddParameter(new OracleParameter("p_request_json", requestLog.request_json));
                OracleParameter request_param = new OracleParameter("p_request_json", OracleDbType.Blob, ParameterDirection.Input);
                byte[] request_bytes = System.Text.Encoding.UTF8.GetBytes(requestLog.request_json);
                request_param.Value = request_bytes;
                manager.AddParameter(request_param);
                OracleParameter response_param = new OracleParameter("p_response_json", OracleDbType.Blob, ParameterDirection.Input);
                byte[] response_bytes = System.Text.Encoding.UTF8.GetBytes(requestLog.response_json);
                response_param.Value = response_bytes;
                manager.AddParameter(response_param);
                long? log_id = manager.CallStoredProcedure_Insert("USP_APIRequestLog_Insert");
                if (log_id.HasValue) return (int)log_id.Value;
                else return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int SignalRBroadcastLogInsert(SignalRBroadcastLog broadcastLog)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_department_id", broadcastLog.department_id));
                manager.AddParameter(new OracleParameter("p_device_no", broadcastLog.device_no));
                manager.AddParameter(new OracleParameter("p_token_no", broadcastLog.token_no));
                manager.AddParameter(new OracleParameter("p_is_token_added", broadcastLog.is_token_added));
                manager.AddParameter(new OracleParameter("p_is_token_called", broadcastLog.is_token_called));
                manager.AddParameter(new OracleParameter("p_is_playlist_changed", broadcastLog.is_playlist_changed));
                manager.AddParameter(new OracleParameter("p_is_footer_changed", broadcastLog.is_footer_changed));
                long? log_id = manager.CallStoredProcedure_Insert("USP_SignalRBroadcastLog_Insert");
                if (log_id.HasValue) return (int)log_id.Value;
                else return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}