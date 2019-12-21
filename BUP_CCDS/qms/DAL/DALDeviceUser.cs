using qms.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Oracle.DataAccess.Client;
using System.Linq;
using System.Web;

namespace qms.DAL
{
    public class DALDeviceUsers
    {
        OracleDataManager manager = new OracleDataManager();
        public DataTable GetAll()
        {
            try
            {

                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_DepartmentUsers_SelectAll");
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
                manager.AddParameter(new OracleParameter("p_deviceuser_id", id));
                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_DepartmentUser_SelectList_ById");
            }
            catch (Exception)
            {

                throw;
            }


        }
        //public int Insert(tblDepartmentUser departmentuser)
        //{
        //    try
        //    {
        //        MapParameters(departmentuser);

        //        long? department_id = manager.CallStoredProcedure_Insert("USP_DepartmentUser_Insert");
        //        if (department_id.HasValue) return (int)department_id.Value;
        //        else return 0;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        ////public void Update(tblDepartmentUser departmentuser)
        ////{
        ////    try
        ////    {
        ////        manager.AddParameter(new OracleParameter("p_departmentuser_id", departmentuser.user_department_id));
        ////        MapParameters(departmentuser);
        ////        manager.CallStoredProcedure_Update("USP_Department_Update");
        ////    }
        ////    catch (Exception)
        ////    {
        ////        throw;
        ////    }
        ////}

        //private void MapParameters(tblDeviceUser departmentuser)
        //{
        //    manager.AddParameter(new OracleParameter("p_device_id", departmentuser.department_id));
        //    manager.AddParameter(new OracleParameter("p_user_id", departmentuser.user_id));




        //}
        //public void Delete(int id)
        //{
        //    try
        //    {
        //        manager.AddParameter(new OracleParameter("p_departmentuser_id", id));

        //        manager.CallStoredProcedure_Update("USP_DepartmentUser_Delete");
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
    }
}