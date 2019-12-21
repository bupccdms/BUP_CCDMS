using qms.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Oracle.DataAccess.Client;
using System.Linq;
using System.Web;

namespace qms.DAL
{
    public class DALPlayList
    {
        OracleDataManager manager = new OracleDataManager();
        public DataTable GetAll()
        {
            try
            {

                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_PL_SelectAll");
            }
            catch (Exception)
            {

                throw;
            }


        }
        public DataTable GetById(int id)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_playlist_id", id));
                OracleParameter param = new OracleParameter("po_cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_PL_SelectById");
            }
            catch (Exception)
            {

                throw;
            }


        }


        public DataTable GetByDepartmentId(int department_id)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_department_id", department_id));
                OracleParameter param = new OracleParameter("po_cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_PL_SelectByDepartmentId");
            }
            catch (Exception)
            {

                throw;
            }


        }

        public DataTable GetByUserId(string user_id)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_user_id", user_id));
                OracleParameter param = new OracleParameter("po_cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_PL_SelectByUserId");
            }
            catch (Exception)
            {

                throw;
            }


        }

        public int Insert(tblPlayList playlist)
        {
            try
            {
                MapParameters(playlist);
                manager.AddParameter(new OracleParameter("p_UserID", playlist.userId));
                long? playlist_id = manager.CallStoredProcedure_Insert("USP_PL_Insert");
                if (playlist_id.HasValue) return (int)playlist_id.Value;
                else return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(tblPlayList playlist)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_playlist_id", playlist.playlist_id));
                manager.AddParameter(new OracleParameter("p_is_publish", playlist.is_publish));

                MapParameters(playlist);
                manager.CallStoredProcedure_Update("USP_PL_Update");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Force(int department_id, tblPlayList playList)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_department_id", department_id));
                if (playList == null)
                {
                   int playlist_id =0;
                    manager.AddParameter(new OracleParameter("p_playlist_id", playlist_id));
                }
                else
                {
                    manager.AddParameter(new OracleParameter("p_playlist_id", playList.playlist_id));
                }
                manager.CallStoredProcedure_Update("USP_PL_Force");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void MapParameters(tblPlayList playlist)
        {
            manager.AddParameter(new OracleParameter("p_playlist_name", playlist.playlist_name));
            manager.AddParameter(new OracleParameter("p_is_global", playlist.is_global));
        }
        public void Delete(int id)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_playlist_id", id));

                manager.CallStoredProcedure_Update("USP_PL_Delete");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}