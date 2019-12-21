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
    public class DALDepartmentPlayList
    {
        OracleDataManager manager = new OracleDataManager();
        public DataTable GetAll()
        {
            try
            {

                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_Department_PL_SelectAll");
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
                manager.AddParameter(new OracleParameter("p_department_playlist_id", id));
                OracleParameter param = new OracleParameter("po_cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_Department_PL_SelectById");
            }
            catch (Exception)
            {

                throw;
            }


        }


        public int Insert(VMDepartmentPlayList departmentPlayList)
        {
            try
            {
                MapParameters(departmentPlayList);
                manager.AddParameter(new OracleParameter("p_is_priority", departmentPlayList.is_priority));
                long? playlist_id = manager.CallStoredProcedure_Insert("USP_Department_PL_Insert");
                if (playlist_id.HasValue) return (int)playlist_id.Value;
                else return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(VMDepartmentPlayList departmentPlayList)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_department_playlist_id", departmentPlayList.department_playlist_id));
                manager.AddParameter(new OracleParameter("p_is_publish", departmentPlayList.is_publish));
                MapParameters(departmentPlayList);
                manager.AddParameter(new OracleParameter("p_is_priority", departmentPlayList.is_priority));
                manager.CallStoredProcedure_Update("USP_Department_PL_Update");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void MapParameters(VMDepartmentPlayList displayFooter)
        {
            manager.AddParameter(new OracleParameter("p_department_id", displayFooter.department_id));
            manager.AddParameter(new OracleParameter("p_playlist_id", displayFooter.playlist_id));
        }
        public void Delete(int id)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_department_playlist_id", id));

                manager.CallStoredProcedure_Update("USP_Department_PL_Delete");
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Force(VMDepartmentPlayList playList)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_department_id", playList.department_id));
                MapParameters(playList);
                manager.CallStoredProcedure_Update("USP_Department_DL_Force");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}