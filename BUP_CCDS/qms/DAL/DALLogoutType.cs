using qms.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Oracle.DataAccess.Client;
using System.Linq;
using System.Web;

namespace qms.DAL
{
    public class DALLogoutType
    {
        OracleDataManager manager = new OracleDataManager();
        public DataTable GetAll()
        {
            try
            {

                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor)
                {
                    Direction = ParameterDirection.Output
                };
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_LogoutType_SelectAll");
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
                manager.AddParameter(new OracleParameter("p_logout_type_id", id));
                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor)
                {
                    Direction = ParameterDirection.Output
                };
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_LogoutType_List_ById");
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
        public int Insert(tblLogoutType logoutType)
        {
            try
            {
                MapParameters(logoutType);
                long? logout_type_id = manager.CallStoredProcedure_Insert("USP_LogoutType_Insert");
                if (logout_type_id.HasValue) return (int)logout_type_id.Value;
                else return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(tblLogoutType logoutType)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_logout_type_id", logoutType.logout_type_id));
                MapParameters(logoutType);
                manager.CallStoredProcedure_Update("USP_LogoutType_Update");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void MapParameters(tblLogoutType logoutType)
        {
            manager.AddParameter(new OracleParameter("p_logout_type_name", logoutType.logout_type_name));
            manager.AddParameter(new OracleParameter("p_has_free_text", logoutType.has_free_text));
            manager.AddParameter(new OracleParameter("p_is_active", logoutType.is_active));
           
        }
        public void Delete(int id)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_logout_type_id", id));

                manager.CallStoredProcedure_Update("USP_LogoutType_Delete");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}