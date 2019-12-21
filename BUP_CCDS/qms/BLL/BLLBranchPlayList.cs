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
    public class BLLDepartmentPlayList
    {
        public List<VMDepartmentPlayList> GetAll()
        {
            DALDepartmentPlayList dal = new DALDepartmentPlayList();
            DataTable dt = dal.GetAll();
            return ObjectMappingList(dt);
        }

        internal VMDepartmentPlayList ObjectMapping(DataRow row)
        {

            VMDepartmentPlayList playlist = new VMDepartmentPlayList();
            playlist.department_playlist_id = Convert.ToInt32(row["DEPARTMENT_playlist_ID"] == DBNull.Value ? 0 : row["DEPARTMENT_playlist_ID"]);
            playlist.playlist_id = Convert.ToInt32(row["playlist_ID"] == DBNull.Value ? 0 : row["playlist_ID"]);
            playlist.playlist_name = (row["playlist_name"] == DBNull.Value ? "" : row["playlist_name"].ToString());
            playlist.department_id = Convert.ToInt32(row["DEPARTMENT_ID"] == DBNull.Value ? 0 : row["DEPARTMENT_ID"]);
            playlist.department_name = (row["DEPARTMENT_NAME"] == DBNull.Value ? "" : row["DEPARTMENT_NAME"].ToString());
            playlist.address = (row["ADDRESS"] == DBNull.Value ? "" : row["ADDRESS"].ToString());
            playlist.is_publish = Convert.ToInt32(row["IS_PUBLISH"] == DBNull.Value ? 0 : row["IS_PUBLISH"]);
            playlist.is_priority = Convert.ToInt32(row["IS_PRIORITY"] == DBNull.Value ? 0 : row["IS_PRIORITY"]);
            return playlist;
        }

        internal List<VMDepartmentPlayList> ObjectMappingList(DataTable dt)
        {
            List<VMDepartmentPlayList> list = new List<VMDepartmentPlayList>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(ObjectMapping(row));

            }
            return list;
        }
        public VMDepartmentPlayList GetById(int id)
        {
            DALDepartmentPlayList dal = new DALDepartmentPlayList();
            DataTable dt = dal.GetById(id);
            if (dt.Rows.Count > 0)
                return ObjectMapping(dt.Rows[0]);
            else return null;
        }

        public void Create(VMDepartmentPlayList playlist)
        {
            DALDepartmentPlayList dal = new DALDepartmentPlayList();
            int playlist_id = dal.Insert(playlist);
            playlist.playlist_id = playlist_id;
        }
        public void Edit(VMDepartmentPlayList playlist)
        {
            DALDepartmentPlayList dal = new DALDepartmentPlayList();
            dal.Update(playlist);

        }
        public void Remove(int id)
        {
            DALDepartmentPlayList dal = new DALDepartmentPlayList();
            dal.Delete(id);

        }
        //public void Dispose()
        //{
        //    Dispose(true);

        //}
        public void Force(VMDepartmentPlayList playlist)
        {
            DALDepartmentPlayList dal = new DALDepartmentPlayList();
            dal.Force(playlist);

        }

    }
}