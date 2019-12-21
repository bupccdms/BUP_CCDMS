using qms.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Oracle.DataAccess.Client;
using System.Linq;
using System.Web;

namespace qms.DAL
{
    public class DALPlayListItem
    {
        OracleDataManager manager = new OracleDataManager();
        public DataTable GetAll(int playlist_id)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_playlist_id", playlist_id));
                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_PL_Item_SelectAll");
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
                manager.AddParameter(new OracleParameter("p_playlistitem_id", id));
                OracleParameter param = new OracleParameter("po_cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_PL_Item_SelectById");
            }
            catch (Exception)
            {

                throw;
            }


        }

        public DataTable GetByFileName(string file_name)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_file_name", file_name));
                OracleParameter param = new OracleParameter("po_cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_PL_Item_SelectByFileName");
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

        public int Insert(tblPlayListItem playlistItem)
        {
            try
            {
                MapParameters(playlistItem);
                //manager.AddParameter(new OracleParameter("p_volume", playlistItem.volume));
                long? playlistitem_id = manager.CallStoredProcedure_Insert("USP_PL_Item_Insert");
                if (playlistitem_id.HasValue) return (int)playlistitem_id.Value;
                else return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(tblPlayListItem playlistItem)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_playlistitem_id", playlistItem.playlistitem_id));
                MapParameters(playlistItem);
                manager.CallStoredProcedure_Update("USP_PL_Item_Update");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void MapParameters(tblPlayListItem playlistItem)
        {
            manager.AddParameter(new OracleParameter("p_playlist_id", playlistItem.playlist_id));
            manager.AddParameter(new OracleParameter("p_item_url", playlistItem.item_url));
            manager.AddParameter(new OracleParameter("p_file_type", playlistItem.file_type));
            manager.AddParameter(new OracleParameter("p_file_name", playlistItem.file_name));
            manager.AddParameter(new OracleParameter("p_duration_in_second", playlistItem.duration_in_second));
            manager.AddParameter(new OracleParameter("p_sort_order", playlistItem.sort_order));
            manager.AddParameter(new OracleParameter("p_start_time", playlistItem.start_time));
            manager.AddParameter(new OracleParameter("p_end_time", playlistItem.end_time));
            manager.AddParameter(new OracleParameter("p_Show_in_mobile", playlistItem.show_in_mobile));
            manager.AddParameter(new OracleParameter("p_is_in_mute", playlistItem.is_in_mute));
            manager.AddParameter(new OracleParameter("p_volume", playlistItem.volume));
        }
        public void Delete(int id)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_playlistitem_id", id));

                manager.CallStoredProcedure_Update("USP_PL_Item_Delete");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}