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
    public class BLLScrollSheduling
    {
        public List<VMScrollSheduling> GetAll()
        {
            DALScrollSheduling dal = new DALScrollSheduling();
            DataTable dt = dal.GetAll();
            return ObjectMappingList(dt);
        }

        internal VMScrollSheduling ObjectMapping(DataRow row)
        {

            VMScrollSheduling scrollSheduling = new VMScrollSheduling();
            scrollSheduling.scroll_Sheduling_id = Convert.ToInt32(row["SCROLL_SHEDULING_ID"] == DBNull.Value ? 0 : row["SCROLL_SHEDULING_ID"]);
            scrollSheduling.scroll_id = Convert.ToInt32(row["SCROLL_ID"] == DBNull.Value ? 0 : row["SCROLL_ID"]);
            scrollSheduling.content_bn = (row["CONTENT_BN"] == DBNull.Value ? "" : row["CONTENT_BN"].ToString());
            scrollSheduling.duration = Convert.ToInt32(row["DURATION"] == DBNull.Value ? 0 : row["DURATION"]);
            scrollSheduling.when_start =Convert.ToDateTime(row["WHEN_START"] == DBNull.Value ? null : row["WHEN_START"].ToString());
            scrollSheduling.is_active = Convert.ToInt32(row["IS_ACTIVE"] == DBNull.Value ? 0 : row["IS_ACTIVE"]);
            scrollSheduling.is_start = Convert.ToInt32(row["IS_START"] == DBNull.Value ? 0 : row["IS_START"]);
            return scrollSheduling;
        }

        internal List<VMScrollSheduling> ObjectMappingList(DataTable dt)
        {
            List<VMScrollSheduling> list = new List<VMScrollSheduling>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(ObjectMapping(row));

            }
            return list;
        }



        public VMScrollSheduling GetById(int id)
        {
            DALScrollSheduling dal = new DALScrollSheduling();
            DataTable dt = dal.GetById(id);
            if (dt.Rows.Count > 0)
                return ObjectMapping(dt.Rows[0]);
            else return null;
        }




        public void Create(VMScrollSheduling scrollSheduling)
        {
            DALScrollSheduling dal = new DALScrollSheduling();
            int scroll_Sheduling_id = dal.Insert(scrollSheduling);
            scrollSheduling.scroll_Sheduling_id = scroll_Sheduling_id;
        }
        public void Edit(VMScrollSheduling scrollSheduling)
        {
            DALScrollSheduling dal = new DALScrollSheduling();
            dal.Update(scrollSheduling);

        }
        public void Remove(int id)
        {
            DALScrollSheduling dal = new DALScrollSheduling();
            dal.Delete(id);
        }

        public void Approve(VMScrollSheduling scrollSheduling)
        {
            DALScrollSheduling dal = new DALScrollSheduling();
            dal.Approve(scrollSheduling);

        }
        //public void Dispose()
        //{
        //    Dispose(true);

        //}

    }
}