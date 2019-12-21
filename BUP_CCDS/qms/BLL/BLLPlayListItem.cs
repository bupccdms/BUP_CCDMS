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
    public class BLLPlayListItem
    {
        public List<tblPlayListItem> GetAll(int playlist_id)
        {
            DALPlayListItem dal = new DALPlayListItem();
            DataTable dt = dal.GetAll(playlist_id);
            return ObjectMappingList(dt);
        }

        public List<tblPlayListItem> GetByFileName(string file_name)
        {
            DALPlayListItem dal = new DALPlayListItem();
            DataTable dt = dal.GetByFileName(file_name);
            return ObjectMappingList(dt);
        }

        internal tblPlayListItem ObjectMapping(DataRow row)
        {

            tblPlayListItem playListItem = new tblPlayListItem();
            playListItem.playlistitem_id = Convert.ToInt32(row["PLAYLISTITEM_ID"] == DBNull.Value ? 0 : row["PLAYLISTITEM_ID"]);
            playListItem.playlist_id = Convert.ToInt32(row["PLAYLIST_ID"] == DBNull.Value ? 0 : row["PLAYLIST_ID"]);
            playListItem.playlist_name = (row["PLAYLIST_NAME"] == DBNull.Value ? "" : row["PLAYLIST_NAME"].ToString());
            playListItem.item_url = (row["ITEM_URL"] == DBNull.Value ? "" : row["ITEM_URL"].ToString());
            playListItem.file_type = (row["FILE_TYPE"] == DBNull.Value ? "" : row["FILE_TYPE"].ToString());
            playListItem.file_name = (row["FILE_NAME"] == DBNull.Value ? "" : row["FILE_NAME"].ToString());
            playListItem.duration_in_second = Convert.ToInt32(row["DURATION_IN_SECOND"] == DBNull.Value ? 0 : row["DURATION_IN_SECOND"]);
            playListItem.sort_order = Convert.ToInt32(row["SORT_ORDER"] == DBNull.Value ? 0 : row["SORT_ORDER"]);
            playListItem.start_time = Convert.ToInt32(row["START_TIME"] == DBNull.Value ? 0 : row["START_TIME"]);
            playListItem.end_time = Convert.ToInt32(row["END_TIME"] == DBNull.Value ? 0 : row["END_TIME"]);
            //playListItem.start_time = Convert.ToDateTime(row["START_TIME"] == DBNull.Value ? null : row["START_TIME"].ToString());
            //playListItem.end_time = Convert.ToDateTime(row["END_TIME"] == DBNull.Value ? null : row["END_TIME"].ToString());
            playListItem.show_in_mobile = Convert.ToInt32(row["SHOW_IN_MOBILE"] == DBNull.Value ? 0 : row["SHOW_IN_MOBILE"]);
            playListItem.is_in_mute = Convert.ToInt32(row["IS_IN_MUTE"] == DBNull.Value ? 0 : row["IS_IN_MUTE"]);
            playListItem.volume = Convert.ToInt32(row["VOLUME"] == DBNull.Value ? 0 : row["VOLUME"]);

            return playListItem;
        }

        internal List<tblPlayListItem> ObjectMappingList(DataTable dt)
        {
            List<tblPlayListItem> list = new List<tblPlayListItem>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(ObjectMapping(row));

            }
            return list;
        }

        

        public tblPlayListItem GetById(int id)
        {
            DALPlayListItem dal = new DALPlayListItem();
            DataTable dt = dal.GetById(id);
            if (dt.Rows.Count > 0)
                return ObjectMapping(dt.Rows[0]);
            else return null;
        }

        


        public void Create(tblPlayListItem playListItem)
        {
            DALPlayListItem dal = new DALPlayListItem();
            int playlistItem_id = dal.Insert(playListItem);
            playListItem.playlistitem_id = playlistItem_id;
        }
        public void Edit(tblPlayListItem playListItem)
        {
            DALPlayListItem dal = new DALPlayListItem();
            dal.Update(playListItem);

        }
        public void Remove(int id)
        {
            DALPlayListItem dal = new DALPlayListItem();
            dal.Delete(id);

        }
        //public void Dispose()
        //{
        //    Dispose(true);

        //}
        
    }
}