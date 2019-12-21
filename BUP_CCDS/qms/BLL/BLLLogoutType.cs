using qms.DAL;
using qms.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace qms.BLL
{
    public class BLLLogoutType
    {
        public List<tblLogoutType> GetAll()
        {
            DALLogoutType dal = new DALLogoutType();
            DataTable dt = dal.GetAll();
            return ObjectMappingList(dt);
        }

        public tblLogoutType GetById(int id)
        {
            DALLogoutType dal = new DALLogoutType();
            DataTable dt = dal.GetById(id);
            if (dt.Rows.Count > 0)
                return ObjectMapping(dt.Rows[0]);
            else return null;
        }
        public void Create(tblLogoutType LogoutType)
        {
            DALLogoutType dal = new DALLogoutType();
            int logout_type_id = dal.Insert(LogoutType);
            LogoutType.logout_type_id = logout_type_id;
        }
        public void Edit(tblLogoutType LogoutType)
        {
            DALLogoutType dal = new DALLogoutType();
            dal.Update(LogoutType);

        }
        public void Remove(int id)
        {
            DALLogoutType dal = new DALLogoutType();
            dal.Delete(id);

        }
        internal tblLogoutType ObjectMapping(DataRow row)
        {

            tblLogoutType LogoutType = new tblLogoutType();
            LogoutType.logout_type_id = Convert.ToInt32(row["logout_type_id"] == DBNull.Value ? 0 : row["logout_type_id"]);
            LogoutType.logout_type_name = (row["logout_type_name"] == DBNull.Value ? null : row["logout_type_name"].ToString());
            LogoutType.has_free_text = Convert.ToInt32(row["has_free_text"] == DBNull.Value ? null : row["has_free_text"].ToString());
            LogoutType.is_active = Convert.ToInt32(row["is_active"] == DBNull.Value ? null : row["is_active"].ToString());



            return LogoutType;
        }
        internal List<tblLogoutType> ObjectMappingList(DataTable dt)
        {
            List<tblLogoutType> list = new List<tblLogoutType>();
            foreach (DataRow row in dt.Rows)
            {
                tblLogoutType LogoutType = ObjectMapping(row);
                list.Add(LogoutType);

            }
            return list;
        }
    }
}