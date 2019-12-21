using qms.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data;
using Oracle.DataAccess.Client;
using qms.DAL;

namespace qms.Models
{
    public class CustomModel
    {
        private qmsEntities db = new qmsEntities();

        OracleDataManager manager = new OracleDataManager();

        public List<VMServiceDetails> GetCustomerServiceSummary(int? departmentID, DateTime? startDate, DateTime? endDate)
        {
            SqlParameter departmentId = new SqlParameter("@department", departmentID);
            SqlParameter sdate = new SqlParameter("@StartDate", startDate);
            SqlParameter edate = new SqlParameter("@EndDate", endDate);
            var result = db.Database.SqlQuery<VMServiceDetails>("GetCustomerServiceSummary @department,@StartDate,@EndDate", departmentId, sdate, edate).ToList();
            return result;
        }



        public List<VMServiceDetails> GetCustomerServiceDetails(int? device , int? token)
        {
            SqlParameter p1 = new SqlParameter("@device", device);
            SqlParameter p2 = new SqlParameter("@token", token);
            var result = db.Database.SqlQuery<VMServiceDetails>("GetCustomerServiceDetails @device , @token", p1,p2).ToList();
            return result;
        }

        public List<VMServiceDetails> GetCustomerServiceDetailsALL(int? DeviceID)
        {
            SqlParameter p1 = new SqlParameter("@device", DeviceID);
            var result = db.Database.SqlQuery<VMServiceDetails>("GetCustomerServiceDetailsALL @device", p1).ToList();
            return result;
        }
    }
}