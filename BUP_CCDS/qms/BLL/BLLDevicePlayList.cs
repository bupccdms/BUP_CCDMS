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
    public class BLLDevicePlayList
    {
        public List<VMDevicePlayList> GetAll()
        {
            DALDevicePlayList dal = new DALDevicePlayList();
            DataTable dt = dal.GetAll();
            return ObjectMappingList(dt);
        }

        internal VMDevicePlayList ObjectMapping(DataRow row)
        {

            VMDevicePlayList playlist = new VMDevicePlayList();
            playlist.device_playlist_id = Convert.ToInt32(row["DEVICE_playlist_ID"] == DBNull.Value ? 0 : row["DEVICE_playlist_ID"]);
            playlist.playlist_id = Convert.ToInt32(row["playlist_ID"] == DBNull.Value ? 0 : row["playlist_ID"]);
            playlist.playlist_name = (row["playlist_name"] == DBNull.Value ? "" : row["playlist_name"].ToString());
            playlist.department_id = Convert.ToInt32(row["department_id"] == DBNull.Value ? 0 : row["department_id"]);
            playlist.department_name = (row["DEPARTMENT_NAME"] == DBNull.Value ? "" : row["DEPARTMENT_NAME"].ToString());
            playlist.device_id = Convert.ToInt32(row["DEVICE_ID"] == DBNull.Value ? 0 : row["DEVICE_ID"]);
            playlist.device_name = (row["DEVICE_NAME"] == DBNull.Value ? "" : row["DEVICE_NAME"].ToString());
            playlist.device_no = (row["DEVICE_NO"] == DBNull.Value ? "" : row["DEVICE_NO"].ToString());
            playlist.Address = (row["ADDRESS"] == DBNull.Value ? "" : row["ADDRESS"].ToString());
            playlist.location = (row["LOCATION"] == DBNull.Value ? "" : row["LOCATION"].ToString());
            playlist.is_global = Convert.ToInt32(row["IS_GLOBAL"] == DBNull.Value ? 0 : row["IS_GLOBAL"]);
            playlist.is_active = Convert.ToInt32(row["IS_ACTIVE"] == DBNull.Value ? 0 : row["IS_ACTIVE"]);
            return playlist;
        }

        internal List<VMDevicePlayList> ObjectMappingList(DataTable dt)
        {
            List<VMDevicePlayList> list = new List<VMDevicePlayList>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(ObjectMapping(row));

            }
            return list;
        }

        public VMDevicePlayList GetById(int id)
        {
            DALDevicePlayList dal = new DALDevicePlayList();
            DataTable dt = dal.GetById(id);
            if (dt.Rows.Count > 0)
                return ObjectMapping(dt.Rows[0]);
            else return null;
        }

        public void Create(VMDevicePlayList playlist)
        {
            DALDevicePlayList dal = new DALDevicePlayList();
            int playlist_id = dal.Insert(playlist);
            playlist.playlist_id = playlist_id;
        }
        public void Edit(VMDevicePlayList playlist)
        {
            DALDevicePlayList dal = new DALDevicePlayList();
            dal.Update(playlist);

        }
        public void Remove(int id)
        {
            DALDevicePlayList dal = new DALDevicePlayList();
            dal.Delete(id);

        }



        //public void Dispose()
        //{
        //    Dispose(true);

        //}

    }
}