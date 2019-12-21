using qms.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Oracle.DataAccess.Client;
using System.Linq;
using System.Web;
using qms.ViewModels;

namespace qms.DAL
{
    public class DALPlayListSheduling
    {
        OracleDataManager manager = new OracleDataManager();
        public DataTable GetAll()
        {
            try
            {
                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_PLS_SelectAll");
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
                manager.AddParameter(new OracleParameter("p_playlist_sheduling_id", id));
                OracleParameter param = new OracleParameter("po_cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_PL_Sheduling_SelectById");
            }
            catch (Exception)
            {

                throw;
            }
        }


        public int Insert(VMPlayListSheduling playListSheduling)
        {
            try
            {
                MapParameters(playListSheduling);
                long? PlayList_Sheduling_id = manager.CallStoredProcedure_Insert("USP_PL_Sheduling_Insert");
                if (PlayList_Sheduling_id.HasValue) return (int)PlayList_Sheduling_id.Value;
                else return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(VMPlayListSheduling playListSheduling)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_playlist_sheduling_id", playListSheduling.PlayList_Sheduling_id));
                MapParameters(playListSheduling);
                manager.CallStoredProcedure_Update("USP_PL_Sheduling_Update");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void MapParameters(VMPlayListSheduling playListSheduling)
        {
            manager.AddParameter(new OracleParameter("p_playlist_id", playListSheduling.PlayList_id));
            manager.AddParameter(new OracleParameter("p_when_start", playListSheduling.when_start));
            manager.AddParameter(new OracleParameter("p_duration", playListSheduling.duration));
            manager.AddParameter(new OracleParameter("p_is_active", playListSheduling.is_active));
            manager.AddParameter(new OracleParameter("p_is_start", playListSheduling.is_start));
            manager.AddParameter(new OracleParameter("p_is_end", playListSheduling.is_end));
        }
        public void Delete(int id)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_playlist_sheduling_id", id));

                manager.CallStoredProcedure_Update("USP_PL_Sheduling_Delete");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Approve(VMPlayListSheduling playListShedulingg)
        {
            try
            {
                MapParameters(playListShedulingg);

                //manager.AddParameter(new OracleParameter("p_is_active", playListShedulingg.is_active));
                manager.AddParameter(new OracleParameter("p_playlist_sheduling_id", playListShedulingg.PlayList_Sheduling_id));
                manager.CallStoredProcedure_Update("USP_Playlist_Sheduling_Approve");

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}