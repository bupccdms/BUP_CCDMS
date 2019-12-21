using qms.DAL;
using qms.Models;
using qms.SignalRHub;
using qms.Utility;
using qms.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace qms.BLL
{
    public class BLLToken
    {
        public List<VMTokenQueue> GetAll()
        {
            DALToken dal = new DALToken();
            DataTable dt = dal.GetAll();
            return ObjectMappingList(dt);
        }

        public List<tblTokenQueue> GetBy(DateTime from_date, DateTime to_date, int token_id = 0, int department_id = 0, int service_type_id = 0, int service_status_id = 0,
            int device_id = 0, string user_id = null, string token_prefix = null, string contact_no = null)
        {
            DALToken dal = new DALToken();
            DataTable dt = dal.GetBy(from_date, to_date, token_id, department_id, service_type_id, service_status_id, device_id, user_id, token_prefix, contact_no);
            return ObjectMappingListToken(dt);
        }

        public List<VMTokenSkipped> GetSkipped(int? department_id, string user_id)
        {
            DALToken dal = new DALToken();
            DataTable dt = dal.GetSkipped(department_id, user_id);
            return ObjectMappingList_SkippedList(dt);
        }

        public List<VMTokenQueue> GetByDepartmentId(int department_id)
        {
            DALToken dal = new DALToken();
            DataTable dt = dal.GetByDepartmentId(department_id);
            return ObjectMappingList(dt);
        }

        public List<tblTokenQueue> GetAllToken()
        {
            DALToken dal = new DALToken();
            DataTable dt = dal.GetAll();
            return ObjectMappingListToken(dt);
        }
        public string GetNextTokenList(int department_id)
        {
            DALToken dal = new DALToken();
            return dal.GetNextTokenList(department_id);
        }
        public List<VMTokenProgress> GetProgressTokenList(int department_id)
        {
            DALToken dal = new DALToken();
            DataTable dt = dal.GetProgressTokenList(department_id);
            return ObjectMappingList_ProgressTokenList(dt);
        }
        internal List<VMTokenQueue> ObjectMappingList(DataTable dt)
        {
            List<VMTokenQueue> list = new List<VMTokenQueue>();
            foreach (DataRow row in dt.Rows)
            {
                VMTokenQueue token = new VMTokenQueue();
                token.token_id = Convert.ToInt64(row["token_id"] == DBNull.Value ? 0 : row["token_id"]);
                token.token_prefix = (row["token_prefix"] == DBNull.Value ? ApplicationSetting.defaultTokenPrefix : row["token_prefix"].ToString());
                token.token_no = Convert.ToInt32(row["token_no"] == DBNull.Value ? null : row["token_no"].ToString());
                if (dt.Columns.Contains("device_no")) token.device_no = (row["device_no"] == DBNull.Value ? null : row["device_no"].ToString());
                token.department_name = (row["department_name"] == DBNull.Value ? null : row["department_name"].ToString());
                token.contact_no = (row["contact_no"] == DBNull.Value ? null : row["contact_no"].ToString());
                token.service_date = Convert.ToDateTime(row["service_date"] == DBNull.Value ? null : row["service_date"].ToString());
                
                token.service_status = (row["service_status"] == DBNull.Value ? null : row["service_status"].ToString());
                if (dt.Columns.Contains("service_status_id")) token.service_status_id = Convert.ToInt16(row["service_status_id"] == DBNull.Value ? 0 : row["service_status_id"]);

                list.Add(token);

            }
            return list;
        }
        internal List<tblTokenQueue> ObjectMappingListToken(DataTable dt)
        {
            List<tblTokenQueue> list = new List<tblTokenQueue>();
            foreach (DataRow row in dt.Rows)
            {
                
                tblTokenQueue token = new tblTokenQueue();
                token.token_id = Convert.ToInt64(row["token_id"] == DBNull.Value ? 0 : row["token_id"]);
                token.token_prefix = (row["token_prefix"] == DBNull.Value ? ApplicationSetting.defaultTokenPrefix : row["token_prefix"].ToString());
                token.token_no = Convert.ToInt32(row["token_no"] == DBNull.Value ? null : row["token_no"].ToString());
                
                token.contact_no = (row["contact_no"] == DBNull.Value ? null : row["contact_no"].ToString());
                token.service_date = Convert.ToDateTime(row["service_date"] == DBNull.Value ? null : row["service_date"].ToString());
                
                if(dt.Columns.Contains("department_id")) token.department_id = Convert.ToInt32(row["department_id"] == DBNull.Value ? 0 : row["department_id"]);
                if (dt.Columns.Contains("department_name")) token.department_name = (row["department_name"] == DBNull.Value ? null : row["department_name"].ToString());
                if (dt.Columns.Contains("service_type_id")) token.service_type_id = Convert.ToInt32(row["service_type_id"] == DBNull.Value ? 0 : row["service_type_id"]);
                if (dt.Columns.Contains("service_type_name")) token.service_type_name = (row["service_type_name"] == DBNull.Value ? null : row["service_type_name"].ToString());
                if (dt.Columns.Contains("cancel_time")) token.cancel_time = Convert.ToDateTime(row["cancel_time"] == DBNull.Value ? null : row["cancel_time"].ToString());
                if (dt.Columns.Contains("service_status_id")) token.service_status_id = Convert.ToInt16(row["service_status_id"] == DBNull.Value ? 0 : row["service_status_id"]);
                if (dt.Columns.Contains("service_status")) token.service_status = (row["service_status"] == DBNull.Value ? null : row["service_status"].ToString());
                if (dt.Columns.Contains("device_id")) token.device_id = Convert.ToInt32(row["device_id"] == DBNull.Value ? 0 : row["device_id"]);
                if (dt.Columns.Contains("device_no")) token.device_no = (row["device_no"] == DBNull.Value ? null : row["device_no"].ToString());
                if (dt.Columns.Contains("user_id")) token.user_id = (row["user_id"] == DBNull.Value ? null : row["user_id"].ToString());
                if (dt.Columns.Contains("username")) token.device_no = (row["username"] == DBNull.Value ? null : row["username"].ToString());
                if (dt.Columns.Contains("hometown")) token.fullname = (row["hometown"] == DBNull.Value ? null : row["hometown"].ToString());
                if (dt.Columns.Contains("calltime")) token.CallTime = Convert.ToDateTime(row["calltime"] == DBNull.Value ? null : row["calltime"].ToString());

                list.Add(token);

            }
            return list;
        }
        internal List<VMNextToken> ObjectMappingList_GetNextTokenList(DataTable dt)
        {
            List<VMNextToken> list = new List<VMNextToken>();
            foreach (DataRow row in dt.Rows)
            {
                VMNextToken token = new VMNextToken();
                token.token_prefix = (row["token_prefix"] == DBNull.Value ? ApplicationSetting.defaultTokenPrefix : row["token_prefix"].ToString());
                token.token_no = Convert.ToInt64(row["token_no"] == DBNull.Value ? null : row["token_no"]);
                token.display_next = Convert.ToInt32(row["display_next"] == DBNull.Value ? null : row["display_next"]);
                token.static_ip = (row["static_ip"] == DBNull.Value ? null : row["static_ip"].ToString());

                list.Add(token);

            }
            return list;
        }
        internal List<VMTokenProgress> ObjectMappingList_ProgressTokenList(DataTable dt)
        {
            List<VMTokenProgress> list = new List<VMTokenProgress>();
            foreach (DataRow row in dt.Rows)
            {
                VMTokenProgress token = new VMTokenProgress();

                token.token_prefix = (row["token_prefix"] == DBNull.Value ? ApplicationSetting.defaultTokenPrefix : row["token_prefix"].ToString());

                token.token_no = (row["token_no"] == DBNull.Value ? ApplicationSetting.DisplayWhenEmptyToken : row["token_no"].ToString().PadLeft(ApplicationSetting.PaddingLeft,'0'));
                
                token.device_no = (row["device_no"] == DBNull.Value ? null : row["device_no"].ToString());
               
                //token.static_ip = (row["static_ip"] == DBNull.Value ? null : row["static_ip"].ToString());

                list.Add(token);

            }
            return list;
        }

        internal List<VMTokenSkipped> ObjectMappingList_SkippedList(DataTable dt)
        {
            List<VMTokenSkipped> list = new List<VMTokenSkipped>();
            foreach (DataRow row in dt.Rows)
            {
                VMTokenSkipped token = new VMTokenSkipped();

                token.token_id = Convert.ToInt64(row["token_id"] == DBNull.Value ? 0 : row["token_id"]);
                token.token_prefix = (row["token_prefix"] == DBNull.Value ? ApplicationSetting.defaultTokenPrefix : row["token_prefix"].ToString());
                token.token_no = Convert.ToInt32(row["token_no"] == DBNull.Value ? null : row["token_no"].ToString());
                token.department_name = (row["department_name"] == DBNull.Value ? null : row["department_name"].ToString());
                token.device_no = (row["device_no"] == DBNull.Value ? null : row["device_no"].ToString());
                token.service_date = Convert.ToDateTime(row["service_date"] == DBNull.Value ? null : row["service_date"].ToString());
                token.service_status_id = Convert.ToInt16(row["service_status_id"] == DBNull.Value ? 0 : row["service_status_id"]);
                token.service_status = (row["service_status"] == DBNull.Value ? null : row["service_status"].ToString());
                token.contact_no = (row["contact_no"] == DBNull.Value ? null : row["contact_no"].ToString());
                token.customer_name = (row["customer_name"] == DBNull.Value ? null : row["customer_name"].ToString());
                token.cancel_time = Convert.ToDateTime(row["cancel_time"] == DBNull.Value ? null : row["cancel_time"].ToString());
                token.user_full_name = (row["HOMETOWN"] == DBNull.Value ? null : row["HOMETOWN"].ToString());

                list.Add(token);

            }
            return list;
        }


        public void Create(tblTokenQueue token)
        {
            DALToken dal = new DALToken();
            dal.Insert(token);
            if (token.device_id.Value>0) NotifyDisplay.CallToken(token.device_id.Value);
        }

        public void ReInitiate(long token_id)
        {
            DALToken dal = new DALToken();
            dal.ReInitiate(token_id);
        }
        public void AssignToMe(long token_id,int device_id)
        {
            DALToken dal = new DALToken();
            dal.AssignToMe(token_id,device_id);
        }
        public void SendSMS(string msisdn, string tokenNo)
        {
            DALSMSManager dal = new DALSMSManager();
            if (ApplicationSetting.isMsgBn)
            {
                string tokenNoBn = EnglishToBangla.convertDigit(tokenNo);
                dal.SendSMSBn(msisdn, string.Format(ApplicationSetting.msgTextBn, tokenNoBn), tokenNoBn);
            }
            else
            {
                dal.SendSMS(msisdn, string.Format(ApplicationSetting.msgText, tokenNo));
            }
            
            
        }
    }
}