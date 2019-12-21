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
    public class BLLDepartment 
    {
        public List<tblDepartment> GetAllDepartment()
        {
            DALDepartment dal = new DALDepartment();
            DataTable dt = dal.GetAllDepartment();
            return ObjectMappingList(dt);
        }

        public List<tblDepartment> GetDepartmentsByUserId(string userId)
        {
            DALDepartment dal = new DALDepartment();
            DataTable dt = dal.GetDepartmentsByUserId(userId);
            return ObjectMappingList(dt);
        }

        public List<VMDepartmentDeviceStatus> GetDeviceCurrentStatus(int department_id, int device_id)
        {
            DALDepartment dal = new DALDepartment();
            DataTable dt = dal.GetDeviceCurrentStatus(department_id, device_id);
            return ObjectMappingListDeviceStatus(dt);
        }

        internal List<VMDepartmentDeviceStatus> ObjectMappingListDeviceStatus(DataTable dt)
        {
            List<VMDepartmentDeviceStatus> list = new List<VMDepartmentDeviceStatus>();
            foreach (DataRow row in dt.Rows)
            {
                VMDepartmentDeviceStatus department = new VMDepartmentDeviceStatus();
                department.department_id = Convert.ToInt32(row["department_id"] == DBNull.Value ? 0 : row["department_id"]);
                department.department_name = (row["department_name"] == DBNull.Value ? null : row["department_name"].ToString());
                department.device_id = Convert.ToInt32(row["device_id"] == DBNull.Value ? 0 : row["device_id"]);
                department.device_no = (row["device_no"] == DBNull.Value ? null : row["device_no"].ToString());
                department.token_id = Convert.ToInt64(row["token_id"] == DBNull.Value ? 0 : row["token_id"]);
                department.token_prefix = (row["token_prefix"] == DBNull.Value ? null : row["token_prefix"].ToString());
                department.token_no = Convert.ToInt32(row["token_no"] == DBNull.Value ? 0 : row["token_no"]);
                if (row["call_time"] != DBNull.Value) department.call_time = Convert.ToDateTime(row["call_time"].ToString());
                department.user_id = (row["user_id"] == DBNull.Value ? null : row["user_id"].ToString());
                department.user_name = (row["user_name"] == DBNull.Value ? null : row["user_name"].ToString());
                department.user_full_name = (row["user_full_name"] == DBNull.Value ? null : row["user_full_name"].ToString());
                if(row["login_time"] != DBNull.Value) department.login_time = Convert.ToDateTime(row["login_time"].ToString());
                if (row["logout_time"] != DBNull.Value) department.logout_time = Convert.ToDateTime(row["logout_time"].ToString());
                department.is_idle = Convert.ToInt32(row["is_idle"] == DBNull.Value ? 0 : row["is_idle"]);
                if(department.is_idle==1) department.idle_from = Convert.ToDateTime(row["idle_from"].ToString());
                list.Add(department);

            }
            return list;
        }
        internal List<tblDepartment> ObjectMappingList(DataTable dt)
        {
            List<tblDepartment> list = new List<tblDepartment>();
            foreach (DataRow row in dt.Rows)
            {
                tblDepartment department = new tblDepartment();
                department.department_id = Convert.ToInt32(row["department_id"] == DBNull.Value ? 0 : row["department_id"]);
                department.department_name = (row["department_name"] == DBNull.Value ? null : row["department_name"].ToString());
                department.contact_no = (row["contact_no"] == DBNull.Value ? null : row["contact_no"].ToString());
                department.contact_person = (row["contact_person"] == DBNull.Value ? null : row["contact_person"].ToString());
                department.address = (row["address"] == DBNull.Value ? null : row["address"].ToString());
                //if (dt.Columns.Contains("is_active")) department.is_active = Convert.ToInt32(row["is_active"] == DBNull.Value ? 0 : row["is_active"]);
                department.is_active = Convert.ToInt32(row["is_active"] == DBNull.Value ? 0 : row["is_active"]);
                //department.static_ip = (row["static_ip"] == DBNull.Value ? null : row["static_ip"].ToString());

                //department.display_next = Convert.ToInt32(row["display_next"] == DBNull.Value ? null : row["display_next"].ToString());

                list.Add(department);

            }
            return list;
        }
        public tblDepartment GetById(int id)
        {
            DALDepartment dal = new DALDepartment();
            DataTable dt = dal.GetById(id);
            if (dt.Rows.Count > 0)
                return ObjectMapping(dt.Rows[0]);
            else return null;
        }
        public void Create(tblDepartment department)
        {
            DALDepartment dal = new DALDepartment();
            int department_id = dal.Insert(department);
            department.department_id = department_id;
        }
        public void Edit(tblDepartment department)
        {
            DALDepartment dal = new DALDepartment();
            dal.Update(department);

        }
        public void StatusModify(tblDepartment department)
        {
            DALDepartment dal = new DALDepartment();
            dal.StatusModify(department);

        }
        public void Remove(int id)
        {
            DALDepartment dal = new DALDepartment();
            dal.Delete(id);

        }
        //public void Dispose()
        //{
        //    Dispose(true);

        //}
        internal tblDepartment ObjectMapping(DataRow row)
        {

            tblDepartment department = new tblDepartment();
            department.department_id = Convert.ToInt32(row["department_id"] == DBNull.Value ? 0 : row["department_id"]);
            department.department_name = (row["department_name"] == DBNull.Value ? null : row["department_name"].ToString());
            department.contact_no = (row["contact_no"] == DBNull.Value ? null : row["contact_no"].ToString());
            department.contact_person = (row["contact_person"] == DBNull.Value ? null : row["contact_person"].ToString());
            department.address = (row["address"] == DBNull.Value ? null : row["address"].ToString());
            //if (row.Table.Columns.Contains("is_active")) department.is_active = Convert.ToInt32(row["is_active"] == DBNull.Value ? 0 : row["is_active"]);
            //department.static_ip = (row["static_ip"] == DBNull.Value ? null : row["static_ip"].ToString());
            department.is_active = Convert.ToInt32(row["is_active"] == DBNull.Value ? 0 : row["is_active"]);
            //department.display_next = Convert.ToInt32(row["display_next"] == DBNull.Value ? null : row["display_next"].ToString());


            return department;
        }
    }
}