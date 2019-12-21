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
    public class BLLPlayList
    {
        public List<tblPlayList> GetAll()
        {
            DALPlayList dal = new DALPlayList();
            DataTable dt = dal.GetAll();
            return ObjectMappingList(dt);
        }

        internal tblPlayList ObjectMapping(DataRow row)
        {
            tblPlayList playList = new tblPlayList();


            playList.playlist_id = Convert.ToInt32(row["PLAYLIST_ID"] == DBNull.Value ? 0 : row["PLAYLIST_ID"]);
            playList.playlist_name = (row["PLAYLIST_NAME"] == DBNull.Value ? "" : row["PLAYLIST_NAME"].ToString());
            playList.is_global = Convert.ToInt32(row["IS_GLOBAL"] == DBNull.Value ? 0 : row["IS_GLOBAL"]);
            playList.is_publish = Convert.ToInt32(row["IS_PUBLISH"] == DBNull.Value ? 0 : row["IS_PUBLISH"]);
            playList.userId = (row["USER_ID"] == DBNull.Value ? "" : row["USER_ID"].ToString());
            playList.hometown = (row["HOMETOWN"] == DBNull.Value ? "" : row["HOMETOWN"].ToString());
            if (row.Table.Columns.Contains("is_active")) playList.is_active = Convert.ToInt32(row["is_active"] == DBNull.Value ? 0 : row["is_active"]);


            return playList;
        }

        internal List<tblPlayList> ObjectMappingList(DataTable dt)
        {
            List<tblPlayList> list = new List<tblPlayList>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(ObjectMapping(row));

            }
            return list;
        }

        internal List<VMPlayList> ObjectMappingListVM(DataTable dt)
        {
            List<VMPlayList> list = new List<VMPlayList>();
            foreach (DataRow row in dt.Rows)
            {
                VMPlayList playList = new VMPlayList();

                playList.playlist_id = Convert.ToInt32(row["PLAYLIST_ID"] == DBNull.Value ? 0 : row["PLAYLIST_ID"]);
                playList.playlist_name = (row["PLAYLIST_NAME"] == DBNull.Value ? "" : row["PLAYLIST_NAME"].ToString());
                playList.item_url = (row["ITEM_URL"] == DBNull.Value ? "" : "~" + row["ITEM_URL"].ToString());
                playList.file_type = (row["FILE_TYPE"] == DBNull.Value ? "" : row["FILE_TYPE"].ToString());
                playList.file_name = (row["FILE_NAME"] == DBNull.Value ? "" : row["FILE_NAME"].ToString());
                playList.start_time = Convert.ToInt32(row["START_TIME"] == DBNull.Value ? 0 : row["START_TIME"]);
                playList.end_time = Convert.ToInt32(row["END_TIME"] == DBNull.Value ? 0 : row["END_TIME"]);
                playList.duration_in_second = Convert.ToInt32(row["DURATION_IN_SECOND"] == DBNull.Value ? 0 : row["DURATION_IN_SECOND"]);
                playList.sort_order = Convert.ToInt32(row["SORT_ORDER"] == DBNull.Value ? 0 : row["SORT_ORDER"]);
                if (row.Table.Columns.Contains("is_active")) playList.is_active = Convert.ToInt32(row["is_active"] == DBNull.Value ? 0 : row["is_active"]);
                if (row.Table.Columns.Contains("SHOW_IN_MOBILE")) playList.show_in_mobile = Convert.ToInt32(row["SHOW_IN_MOBILE"] == DBNull.Value ? 0 : row["SHOW_IN_MOBILE"]);
                if (row.Table.Columns.Contains("IS_IN_MUTE")) playList.is_in_mute = Convert.ToInt32(row["IS_IN_MUTE"] == DBNull.Value ? 0 : row["IS_IN_MUTE"]);
                if (row.Table.Columns.Contains("VOLUME")) playList.volume = Convert.ToInt32(row["VOLUME"] == DBNull.Value ? 0 : row["VOLUME"]);
                list.Add(playList);


            }
            return list;
        }

        public tblPlayList GetById(int id)
        {
            DALPlayList dal = new DALPlayList();
            DataTable dt = dal.GetById(id);
            if (dt.Rows.Count > 0)
                return ObjectMapping(dt.Rows[0]);
            else return null;
        }

        public List<VMPlayList> GetByDepartmentId(int department_id)
        {
            DALPlayList dal = new DALPlayList();
            DataTable dt = dal.GetByDepartmentId(department_id);
            return ObjectMappingListVM(dt);
            
        }

        public List<VMPlayList> GetByUserId(string user_id)
        {
            DALPlayList dal = new DALPlayList();
            DataTable dt = dal.GetByUserId(user_id);
            return ObjectMappingListVM(dt);

        }

        public void Create(tblPlayList playList)
        {
            DALPlayList dal = new DALPlayList();
            int playlist_id = dal.Insert(playList);
            playList.playlist_id = playlist_id;
        }
        public void Edit(tblPlayList playList)
        {
            DALPlayList dal = new DALPlayList();
            dal.Update(playList);

        }

        public void Force(int department_id, tblPlayList playList)
        {
            DALPlayList dal = new DALPlayList();
            dal.Force(department_id, playList);
        }
        public void Remove(int id)
        {
            DALPlayList dal = new DALPlayList();
            dal.Delete(id);

        }
        //public void Dispose()
        //{
        //    Dispose(true);

        //}
        
    }
}