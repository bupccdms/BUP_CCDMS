using qms.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Oracle.DataAccess.Client;
using System.Linq;
using System.Web;

namespace qms.DAL
{
    public class DALDepartment
    {
        OracleDataManager manager = new OracleDataManager();
        public DataTable GetAllDepartment()
        {
            try
            {
                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_Department_SelectAll");
            }
            catch (Exception)
            {

                throw;
            }


        }

        public DataTable GetDepartmentsByUserId(string user_id)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_user_id", user_id));

                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_Department_ByUserId");
            }
            catch (Exception)
            {

                throw;
            }


        }

        public DataTable GetDeviceCurrentStatus(int department_id, int device_id)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_department_id", department_id));
                manager.AddParameter(new OracleParameter("p_device_id", device_id));
                OracleParameter param = new OracleParameter("po_cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_Department_DeviceStatus");
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
                manager.AddParameter(new OracleParameter("p_department_id", id));
                OracleParameter param = new OracleParameter("po_cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_Department_List_ById");
            }
            catch (Exception)
            {

                throw;
            }


        }
        public int Insert(tblDepartment department)
        {
            try
            {
                MapParameters(department);
                long? department_id = manager.CallStoredProcedure_Insert("USP_Department_Insert");
                if (department_id.HasValue) return (int)department_id.Value;
                else return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(tblDepartment department)
        {
            try
            {
                MapParameters(department);

                manager.AddParameter(new OracleParameter("p_is_active", department.is_active));
                manager.AddParameter(new OracleParameter("p_department_id", department.department_id));
                //manager.CallStoredProcedure_Update("USP_Department_Update");
                manager.CallStoredProcedure_Update("USP_Department_Update");

            }
            catch (Exception)
            {
                throw;
            }
        }
        public void StatusModify(tblDepartment department)
        {
            try
            {
                MapParameters(department);

                manager.AddParameter(new OracleParameter("p_is_active", department.is_active));
                manager.AddParameter(new OracleParameter("p_department_id", department.department_id));
                //manager.CallStoredProcedure_Update("USP_Department_Update");
                manager.CallStoredProcedure_Update("USP_Department_Active_Deactive");

            }
            catch (Exception)
            {
                throw;
            }
        }
        private void MapParameters(tblDepartment department)
        {
            manager.AddParameter(new OracleParameter("p_department_name", department.department_name));
            manager.AddParameter(new OracleParameter("p_address", department.address));
            manager.AddParameter(new OracleParameter("p_contact_no", department.contact_no));
            manager.AddParameter(new OracleParameter("p_contact_person", department.contact_person));



        }
        public void Delete(int id)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_department_id", id));

                manager.CallStoredProcedure_Update("USP_Department_Delete");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}