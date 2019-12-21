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
    public class BLLDepartmentScroll
    {
        public List<VMDepartmentScroll> GetAll()
        {
            DALDepartmentScroll dal = new DALDepartmentScroll();
            DataTable dt = dal.GetAll();
            return ObjectMappingList(dt);
        }

        internal VMDepartmentScroll ObjectMapping(DataRow row)
        {

            VMDepartmentScroll displayFooter = new VMDepartmentScroll();
            displayFooter.department_scroll_id = Convert.ToInt32(row["DEPARTMENT_SCROLL_ID"] == DBNull.Value ? 0 : row["DEPARTMENT_SCROLL_ID"]);
            displayFooter.scroll_id = Convert.ToInt32(row["SCROLL_ID"] == DBNull.Value ? 0 : row["SCROLL_ID"]);
            displayFooter.content_en = (row["CONTENT_EN"] == DBNull.Value ? "" : row["CONTENT_EN"].ToString());
            displayFooter.content_bn = (row["CONTENT_BN"] == DBNull.Value ? "" : row["CONTENT_BN"].ToString()); // new BLLScroll().GetScrollBn(displayFooter.scroll_id);
            displayFooter.department_id = Convert.ToInt32(row["DEPARTMENT_ID"] == DBNull.Value ? 0 : row["DEPARTMENT_ID"]);
            displayFooter.department_name = (row["DEPARTMENT_NAME"] == DBNull.Value ? "" : row["DEPARTMENT_NAME"].ToString());
            if (row.Table.Columns.Contains("is_active")) displayFooter.is_active = Convert.ToInt32(row["is_active"] == DBNull.Value ? 0 : row["is_active"]);
            displayFooter.is_publish = Convert.ToInt32(row["IS_PUBLISH"] == DBNull.Value ? 0 : row["IS_PUBLISH"]);
            return displayFooter;
        }

        internal List<VMDepartmentScroll> ObjectMappingList(DataTable dt)
        {
            List<VMDepartmentScroll> list = new List<VMDepartmentScroll>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(ObjectMapping(row));

            }
            return list;
        }
        public VMDepartmentScroll GetById(int id)
        {
            DALDepartmentScroll dal = new DALDepartmentScroll();
            DataTable dt = dal.GetById(id);
            if (dt.Rows.Count > 0)
                return ObjectMapping(dt.Rows[0]);
            else return null;
        }

        public void Create(VMDepartmentScroll displayFooter)
        {
            DALDepartmentScroll dal = new DALDepartmentScroll();
            int scroll_id = dal.Insert(displayFooter);
            displayFooter.scroll_id = scroll_id;
        }
        public void Edit(VMDepartmentScroll departmentScroll)
        {
            DALDepartmentScroll dal = new DALDepartmentScroll();
            dal.Update(departmentScroll);

        }
        public void Force(VMDepartmentScroll departmentScroll)
        {
            DALDepartmentScroll dal = new DALDepartmentScroll();
            dal.Force(departmentScroll);

        }
        public void Remove(int id)
        {
            DALDepartmentScroll dal = new DALDepartmentScroll();
            dal.Delete(id);

        }


        //public void Dispose()
        //{
        //    Dispose(true);

        //}
        
    }
}