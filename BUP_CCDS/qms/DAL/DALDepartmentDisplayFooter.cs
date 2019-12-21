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
    public class DALDepartmentScroll
    {
        OracleDataManager manager = new OracleDataManager();
        public DataTable GetAll()
        {
            try
            {

                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_Department_DF_SelectAll");
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
                manager.AddParameter(new OracleParameter("p_department_scroll_id", id));
                OracleParameter param = new OracleParameter("po_cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_Department_DF_SelectById");
            }
            catch (Exception)
            {

                throw;
            }


        }


        public int Insert(VMDepartmentScroll displayFooter)
        {
            try
            {
                MapParameters(displayFooter);
                long? scroll_id = manager.CallStoredProcedure_Insert("USP_Department_DF_Insert");
                if (scroll_id.HasValue) return (int)scroll_id.Value;
                else return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(VMDepartmentScroll departmentScroll)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_department_scroll_id", departmentScroll.department_scroll_id));
                manager.AddParameter(new OracleParameter("p_is_active", departmentScroll.is_active));
                manager.AddParameter(new OracleParameter("p_is_publish", departmentScroll.is_publish));
                MapParameters(departmentScroll);
                manager.CallStoredProcedure_Update("USP_Department_DF_Update");
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Force(VMDepartmentScroll departmentScroll)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_department_id", departmentScroll.department_id));
                MapParameters(departmentScroll);
                manager.CallStoredProcedure_Update("USP_Department_DF_Force");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void MapParameters(VMDepartmentScroll departmentScroll)
        {
            manager.AddParameter(new OracleParameter("p_department_id", departmentScroll.department_id));
            manager.AddParameter(new OracleParameter("p_scroll_id", departmentScroll.scroll_id));;
        }
        public void Delete(int id)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_department_scroll_id", id));

                manager.CallStoredProcedure_Update("USP_Department_DF_Delete");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}