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
    public class DALDevices
    {
        OracleDataManager manager = new OracleDataManager();
        public DataTable GetAll()
        {
            try
            {
                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_Devices_SelectAll");
            }
            catch (Exception)
            {

                throw;
            }
        }
        public DataTable GetAllbyDepartment(int id)
        {
            try
            {
                manager.AddParameter(new OracleParameter("P_department_id", id));
                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_Devices_SelectByDepartment");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable GetFree(int department_id, string user_id)
        {
            try
            {
                manager.AddParameter(new OracleParameter("P_department_id", department_id));
                manager.AddParameter(new OracleParameter("P_user_id", user_id));
                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_DEVICE_SELECTFREE");
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
                manager.AddParameter(new OracleParameter("p_Device_id", id));
                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_Devices_List_ById");
            }
            catch (Exception)
            {

                throw;
            }


        }
        /// <summary>
        /// Call only for New Service Type Insert
        /// Return service_type_id
        /// </summary>
        /// <param name="serviceType">Service Type Object</param>
        /// <returns>Return service_type_id</returns>
        public int Insert(VMDevice device)
        {
            try
            {
                MapParameters(device);
                long? device_id = manager.CallStoredProcedure_Insert("USP_Devices_Insert");
                if (device_id.HasValue) return (int)device_id.Value;
                else return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(tblDevice device)
        {
            try
            {
                
                manager.AddParameter(new OracleParameter("p_location", device.location));
                manager.AddParameter(new OracleParameter("p_device_no", device.device_no));
                manager.AddParameter(new OracleParameter("p_department_id", device.department_id));
                manager.AddParameter(new OracleParameter("p_device_name", device.device_name));
                manager.AddParameter(new OracleParameter("p_user_id", device.user_id));
                //MapParameters(device);
                manager.AddParameter(new OracleParameter("p_is_active", device.is_active));
                manager.AddParameter(new OracleParameter("p_Device_id", device.device_id));
                
                manager.CallStoredProcedure_Update("USP_Devices_Update");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void StatusModify(VMDevice device)
        {
            try
            {
                MapParameters(device);
                manager.AddParameter(new OracleParameter("p_is_active", device.is_active));
                manager.AddParameter(new OracleParameter("p_Device_id", device.device_id));

                manager.CallStoredProcedure_Update("USP_Devices_Active_Deactive");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void MapParameters(VMDevice device)
        {
            
            manager.AddParameter(new OracleParameter("p_location", device.location));
            manager.AddParameter(new OracleParameter("p_device_no", device.device_no));
            manager.AddParameter(new OracleParameter("p_department_id", device.department_id));
            manager.AddParameter(new OracleParameter("p_device_name", device.device_name));
            manager.AddParameter(new OracleParameter("p_user_id", device.user_id));




        }
       
        public void Delete(int id)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_Device_id", id));

                manager.CallStoredProcedure_Update("USP_Devices_Delete");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}