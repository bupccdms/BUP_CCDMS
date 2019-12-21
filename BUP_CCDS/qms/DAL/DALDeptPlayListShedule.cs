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
    public class DALDeptPlayListShedule
    {
        OracleDataManager manager = new OracleDataManager();
        public DataTable GetAll()
        {
            try
            {
                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_DPLS_SelectAll");
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
                manager.AddParameter(new OracleParameter("p_dept_playlist_shedule_id", id));
                OracleParameter param = new OracleParameter("po_cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_DPL_Shedule_SelectById");
            }
            catch (Exception)
            {

                throw;
            }
        }


        public int Insert(VMDepartmentPlayListShedule deptPlayListShedule)
        {
            try
            {
                MapParameters(deptPlayListShedule);
                long? Dept_PlayList_Shedule_id = manager.CallStoredProcedure_Insert("USP_DPL_Shedule_Insert");
                if (Dept_PlayList_Shedule_id.HasValue) return (int)Dept_PlayList_Shedule_id.Value;
                else return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(VMDepartmentPlayListShedule deptPlayListShedule)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_dept_playlist_shedule_id", deptPlayListShedule.Department_PlayList_Shedule_id));
                MapParameters(deptPlayListShedule);
                manager.CallStoredProcedure_Update("USP_DPL_Shedule_Update");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void MapParameters(VMDepartmentPlayListShedule deptPlayListShedule)
        {
            manager.AddParameter(new OracleParameter("p_department_id", deptPlayListShedule.department_id));
            manager.AddParameter(new OracleParameter("p_playlist_id", deptPlayListShedule.PlayList_id));
            manager.AddParameter(new OracleParameter("p_when_start", deptPlayListShedule.when_start));
            manager.AddParameter(new OracleParameter("p_duration", deptPlayListShedule.duration));
            manager.AddParameter(new OracleParameter("p_is_active", deptPlayListShedule.is_active));
            manager.AddParameter(new OracleParameter("p_is_start", deptPlayListShedule.is_start));
            manager.AddParameter(new OracleParameter("p_is_end", deptPlayListShedule.is_end));
        }
        public void Delete(int id)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_dept_playlist_shedule_id", id));

                manager.CallStoredProcedure_Update("USP_DPL_Shedule_Delete");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Approve(VMDepartmentPlayListShedule deptPlayListSheduleg)
        {
            try
            {
                MapParameters(deptPlayListSheduleg);
                manager.AddParameter(new OracleParameter("p_dept_playlist_shedule_id", deptPlayListSheduleg.Department_PlayList_Shedule_id));
                manager.CallStoredProcedure_Update("USP_DPL_Shedule_Approve");

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}