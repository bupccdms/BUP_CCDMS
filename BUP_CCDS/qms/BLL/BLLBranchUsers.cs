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
    public class BLLDepartmentUsers
    {
        public List<VMDepartmentLogin> GetAll()
        {
            DALDepartmentUsers dal = new DALDepartmentUsers();
            DataTable dt = dal.GetAll();
            return ObjectMappingList(dt);
        }


        internal List<VMDepartmentLogin> ObjectMappingList(DataTable dt)
        {
            List<VMDepartmentLogin> list = new List<VMDepartmentLogin>();
            foreach (DataRow row in dt.Rows)
            {
                VMDepartmentLogin departmentuser = new VMDepartmentLogin();
                departmentuser.user_department_id = Convert.ToInt32(row["user_department_id"] == DBNull.Value ? 0 : row["user_department_id"]);
                departmentuser.department_id = Convert.ToInt32(row["department_id"] == DBNull.Value ? 0 : row["department_id"]);
                departmentuser.department_name = (row["department_name"] == DBNull.Value ? null : row["department_name"].ToString());
                departmentuser.user_id = (row["user_id"] == DBNull.Value ? null : row["user_id"].ToString());
                departmentuser.Hometown = (row["Hometown"] == DBNull.Value ? null : row["Hometown"].ToString());
                departmentuser.UserName = (row["UserName"] == DBNull.Value ? null : row["UserName"].ToString());
                if (dt.Columns.Contains("PhoneNumber")) departmentuser.PhoneNumber = (row["PhoneNumber"] == DBNull.Value ? null : row["PhoneNumber"].ToString());
                if (dt.Columns.Contains("Email")) departmentuser.Email = (row["Email"] == DBNull.Value ? null : row["Email"].ToString());
                departmentuser.Name = (row["Name"] == DBNull.Value ? null : row["Name"].ToString());
                if (dt.Columns.Contains("is_active")) departmentuser.is_active = Convert.ToInt32(row["is_active"] == DBNull.Value ? 0 : row["is_active"]);
                if (dt.Columns.Contains("ISACTIVEDIRECTORYUSER")) departmentuser.is_active_directory_user = Convert.ToInt32(row["ISACTIVEDIRECTORYUSER"] == DBNull.Value ? 0 : row["ISACTIVEDIRECTORYUSER"]);

                list.Add(departmentuser);


            }
            return list;
        }
        public tblDepartmentUser GetById(int id)
        {
            DALDepartmentUsers dal = new DALDepartmentUsers();
            DataTable dt = dal.GetById(id);
            if (dt.Rows.Count > 0)
                return ObjectMapping(dt.Rows[0]);
            else return null;
        }
        public void Create(tblDepartmentUser departmentuser)
        {
            DALDepartmentUsers dal = new DALDepartmentUsers();
            int user_department_id = dal.Insert(departmentuser);
            departmentuser.user_department_id = user_department_id;
        }
        public void Edit(tblDepartmentUser departmentuser)
        {
            DALDepartmentUsers dal = new DALDepartmentUsers();
            dal.Update(departmentuser);

        }
        public void Remove(int id)
        {
            DALDepartmentUsers dal = new DALDepartmentUsers();
            dal.Delete(id);

        }
        internal tblDepartmentUser ObjectMapping(DataRow row)
        {

            tblDepartmentUser departmentuser = new tblDepartmentUser();
            departmentuser.department_id = Convert.ToInt32(row["department_id"] == DBNull.Value ? 0 : row["department_id"]);
            departmentuser.user_id = (row["user_id"] == DBNull.Value ? null : row["user_id"].ToString());


            return departmentuser;
        }
    }
}