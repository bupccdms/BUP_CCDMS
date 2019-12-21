using qms.DAL;
using qms.Models;
using qms.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace qms.BLL
{
    public class BLLLog
    {
        
        
        public void SignalRBroadCastLogCreate(SignalRBroadcastLog log)
        {
            DALLog dal = new DALLog();
            int broadcast_log_id = dal.SignalRBroadcastLogInsert(log);
            log.broadcast_log_id = broadcast_log_id;
        }

        public void ApiRequestLogCreate(ApiRequestLog log)
        {
            DALLog dal = new DALLog();
            int request_log_id = dal.ApiRequestLogInsert(log);
            log.request_log_id = request_log_id;
        }

    }
}