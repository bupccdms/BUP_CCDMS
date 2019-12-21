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
    public class BLLPlayListSheduling
    {
        public List<VMPlayListSheduling> GetAll()
        {
            DALPlayListSheduling dal = new DALPlayListSheduling();
            DataTable dt = dal.GetAll();
            return ObjectMappingList(dt);
        }

        //public List<tblPlayListItem> GetByFileName(string file_name)
        //{
        //    DALPlayListItem dal = new DALPlayListItem();
        //    DataTable dt = dal.GetByFileName(file_name);
        //    return ObjectMappingList(dt);
        //}

        internal VMPlayListSheduling ObjectMapping(DataRow row)
        {

            VMPlayListSheduling playListSheduling = new VMPlayListSheduling();
            playListSheduling.PlayList_Sheduling_id = Convert.ToInt32(row["PLAYLIST_SHEDULING_ID"] == DBNull.Value ? 0 : row["PLAYLIST_SHEDULING_ID"]);
            playListSheduling.PlayList_id = Convert.ToInt32(row["PLAYLIST_ID"] == DBNull.Value ? 0 : row["PLAYLIST_ID"]);
            playListSheduling.PlayList_name = (row["PLAYLIST_NAME"] == DBNull.Value ? "" : row["PLAYLIST_NAME"].ToString());
            playListSheduling.duration = Convert.ToInt32(row["DURATION"] == DBNull.Value ? 0 : row["DURATION"]);
            playListSheduling.when_start =Convert.ToDateTime(row["WHEN_START"] == DBNull.Value ? null : row["WHEN_START"].ToString());
            playListSheduling.is_active = Convert.ToInt32(row["IS_ACTIVE"] == DBNull.Value ? 0 : row["IS_ACTIVE"]);
            playListSheduling.is_start = Convert.ToInt32(row["IS_START"] == DBNull.Value ? 0 : row["IS_START"]);
            playListSheduling.is_end = Convert.ToInt32(row["IS_END"] == DBNull.Value ? 0 : row["IS_END"]);
            return playListSheduling;
        }

        internal List<VMPlayListSheduling> ObjectMappingList(DataTable dt)
        {
            List<VMPlayListSheduling> list = new List<VMPlayListSheduling>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(ObjectMapping(row));

            }
            return list;
        }



        public VMPlayListSheduling GetById(int id)
        {
            DALPlayListSheduling dal = new DALPlayListSheduling();
            DataTable dt = dal.GetById(id);
            if (dt.Rows.Count > 0)
                return ObjectMapping(dt.Rows[0]);
            else return null;
        }




        public void Create(VMPlayListSheduling playListSheduling)
        {
            DALPlayListSheduling dal = new DALPlayListSheduling();
            int PlayList_Sheduling_id = dal.Insert(playListSheduling);
            playListSheduling.PlayList_Sheduling_id = PlayList_Sheduling_id;
        }
        public void Edit(VMPlayListSheduling playListSheduling)
        {
            DALPlayListSheduling dal = new DALPlayListSheduling();
            dal.Update(playListSheduling);

        }
        public void Remove(int id)
        {
            DALPlayListSheduling dal = new DALPlayListSheduling();
            dal.Delete(id);
        }

        public void Approve(VMPlayListSheduling playListSheduling)
        {
            DALPlayListSheduling dal = new DALPlayListSheduling();
            dal.Approve(playListSheduling);

        }
        //public void Dispose()
        //{
        //    Dispose(true);

        //}

    }
}