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
    public class DALDeviceScroll
    {
        OracleDataManager manager = new OracleDataManager();
        public DataTable GetAll()
        {
            try
            {

                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_Device_DF_SelectAll");
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
                manager.AddParameter(new OracleParameter("p_device_scroll_id", id));
                OracleParameter param = new OracleParameter("po_cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_Device_DF_SelectById");
            }
            catch (Exception)
            {

                throw;
            }


        }


        public int Insert(VMDeviceScroll displayFooter)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_is_active", displayFooter.is_active));
                //manager.AddParameter(new OracleParameter("p_status", displayFooter.status));
                MapParameters(displayFooter);
                long? scroll_id = manager.CallStoredProcedure_Insert("USP_Device_DF_Insert");
                if (scroll_id.HasValue) return (int)scroll_id.Value;
                else return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(VMDeviceScroll displayFooter)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_device_scroll_id", displayFooter.device_scroll_id));
                manager.AddParameter(new OracleParameter("p_is_active", displayFooter.is_active));
                manager.AddParameter(new OracleParameter("p_is_publish", displayFooter.is_publish));
                MapParameters(displayFooter);
                manager.CallStoredProcedure_Update("USP_Device_DF_Update");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void MapParameters(VMDeviceScroll displayFooter)

        {
            manager.AddParameter(new OracleParameter("p_device_id", displayFooter.device_id));
            //manager.AddParameter(new OracleParameter("p_department_id", displayFooter.department_id));
            manager.AddParameter(new OracleParameter("p_scroll_id", displayFooter.scroll_id));;
        }
        public void Delete(int id)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_device_scroll_id", id));

                manager.CallStoredProcedure_Update("USP_Device_DF_Delete");
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DataTable GetAllbyDevice(int id)
        {
            try
            {
                manager.AddParameter(new OracleParameter("P_device_id", id));
                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_scroll_SelectByDevice");
            }
            catch (Exception)
            {

                throw;
            }
        }

        
    }
}