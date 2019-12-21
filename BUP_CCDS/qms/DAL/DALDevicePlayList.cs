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
    public class DALDevicePlayList
    {
        OracleDataManager manager = new OracleDataManager();
        public DataTable GetAll()
        {
            try
            {

                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_Device_PL_SelectAll");
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
                manager.AddParameter(new OracleParameter("p_device_playlist_id", id));
                OracleParameter param = new OracleParameter("po_cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_Device_PL_SelectById");
            }
            catch (Exception)
            {

                throw;
            }


        }


        public int Insert(VMDevicePlayList devicePlayList)
        {
            try
            {
                MapParameters(devicePlayList);
                long? playlist_id = manager.CallStoredProcedure_Insert("USP_Device_PL_Insert");
                if (playlist_id.HasValue) return (int)playlist_id.Value;
                else return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(VMDevicePlayList devicePlayList)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_device_playlist_id", devicePlayList.device_playlist_id));
                manager.AddParameter(new OracleParameter("p_is_active", devicePlayList.is_active));
                MapParameters(devicePlayList);
                manager.CallStoredProcedure_Update("USP_Device_PL_Update");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void MapParameters(VMDevicePlayList devicePlayList)
        {
            manager.AddParameter(new OracleParameter("p_device_id", devicePlayList.device_id));
            manager.AddParameter(new OracleParameter("p_playlist_id", devicePlayList.playlist_id));
            manager.AddParameter(new OracleParameter("p_is_global", devicePlayList.is_global));
        }
        public void Delete(int id)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_device_playlist_id", id));

                manager.CallStoredProcedure_Update("USP_Device_PL_Delete");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}