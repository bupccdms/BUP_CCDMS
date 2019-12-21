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
    public class BLLDeptPlayListShedule
    {
        public List<VMDepartmentPlayListShedule> GetAll()
        {
            DALDeptPlayListShedule dal = new DALDeptPlayListShedule();
            DataTable dt = dal.GetAll();
            return ObjectMappingList(dt);
        }

        //public List<tblPlayListItem> GetByFileName(string file_name)
        //{
        //    DALPlayListItem dal = new DALPlayListItem();
        //    DataTable dt = dal.GetByFileName(file_name);
        //    return ObjectMappingList(dt);
        //}

        internal VMDepartmentPlayListShedule ObjectMapping(DataRow row)
        {

            VMDepartmentPlayListShedule deptPlayListShedule = new VMDepartmentPlayListShedule();
            deptPlayListShedule.Department_PlayList_Shedule_id = Convert.ToInt32(row["DEPARTMENT_PLAYLIST_SHEDULE_ID"] == DBNull.Value ? 0 : row["DEPARTMENT_PLAYLIST_SHEDULE_ID"]);
            deptPlayListShedule.PlayList_id = Convert.ToInt32(row["PLAYLIST_ID"] == DBNull.Value ? 0 : row["PLAYLIST_ID"]);
            deptPlayListShedule.PlayList_name = (row["PLAYLIST_NAME"] == DBNull.Value ? "" : row["PLAYLIST_NAME"].ToString());
            deptPlayListShedule.department_id = Convert.ToInt32(row["DEPARTMENT_ID"] == DBNull.Value ? 0 : row["DEPARTMENT_ID"]);
            deptPlayListShedule.department_name = (row["DEPARTMENT_NAME"] == DBNull.Value ? "" : row["DEPARTMENT_NAME"].ToString());
            deptPlayListShedule.duration = Convert.ToInt32(row["DURATION"] == DBNull.Value ? 0 : row["DURATION"]);
            deptPlayListShedule.when_start =Convert.ToDateTime(row["WHEN_START"] == DBNull.Value ? null : row["WHEN_START"].ToString());
            deptPlayListShedule.is_active = Convert.ToInt32(row["IS_ACTIVE"] == DBNull.Value ? 0 : row["IS_ACTIVE"]);
            deptPlayListShedule.is_start = Convert.ToInt32(row["IS_START"] == DBNull.Value ? 0 : row["IS_START"]);
            deptPlayListShedule.is_end = Convert.ToInt32(row["IS_END"] == DBNull.Value ? 0 : row["IS_END"]);
            return deptPlayListShedule;
        }

        internal List<VMDepartmentPlayListShedule> ObjectMappingList(DataTable dt)
        {
            List<VMDepartmentPlayListShedule> list = new List<VMDepartmentPlayListShedule>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(ObjectMapping(row));

            }
            return list;
        }



        public VMDepartmentPlayListShedule GetById(int id)
        {
            DALDeptPlayListShedule dal = new DALDeptPlayListShedule();
            DataTable dt = dal.GetById(id);
            if (dt.Rows.Count > 0)
                return ObjectMapping(dt.Rows[0]);
            else return null;
        }




        public void Create(VMDepartmentPlayListShedule deptPlayListShedule)
        {
            DALDeptPlayListShedule dal = new DALDeptPlayListShedule();
            int Department_PlayList_Shedule_id = dal.Insert(deptPlayListShedule);
            deptPlayListShedule.Department_PlayList_Shedule_id = Department_PlayList_Shedule_id;
        }
        public void Edit(VMDepartmentPlayListShedule deptPlayListShedule)
        {
            DALDeptPlayListShedule dal = new DALDeptPlayListShedule();
            dal.Update(deptPlayListShedule);

        }
        public void Remove(int id)
        {
            DALDeptPlayListShedule dal = new DALDeptPlayListShedule();
            dal.Delete(id);
        }

        public void Approve(VMDepartmentPlayListShedule deptPlayListShedule)
        {
            DALDeptPlayListShedule dal = new DALDeptPlayListShedule();
            dal.Approve(deptPlayListShedule);

        }
        //public void Dispose()
        //{
        //    Dispose(true);

        //}

    }
}