//using qms.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Oracle.DataAccess.Client;
using System.Linq;
using System.Web;
using qms.ViewModels;

namespace qms.DAL
{
    public class DALCustomContent
    {
        OracleDataManager manager = new OracleDataManager();
        public DataTable GetAll()
        {
            try
            {

                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_CC_SelectAll");
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
                manager.AddParameter(new OracleParameter("p_custom_content_id", id));
                OracleParameter param = new OracleParameter("po_cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_CC_SelectById");
            }
            catch (Exception)
            {

                throw;
            }


        }


        //public DataTable GetByDepartmentId(int department_id)
        //{
        //    try
        //    {
        //        manager.AddParameter(new OracleParameter("p_department_id", department_id));
        //        OracleParameter param = new OracleParameter("po_cursor", OracleDbType.RefCursor);
        //        param.Direction = ParameterDirection.Output;
        //        manager.AddParameter(param);

        //        return manager.CallStoredProcedure_Select("USP_DF_SelectByDepartmentId");
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }


        //}

        public int Insert(VMCustomContent customContent)
        {
            try
            {
                MapParameters(customContent);
                long? custom_content_id = manager.CallStoredProcedure_Insert("USP_CC_Insert");
                if (custom_content_id.HasValue) return (int)custom_content_id.Value;
                else return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(VMCustomContent customContent)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_custom_content_id", customContent.custom_content_id));
                MapParameters(customContent);
                manager.CallStoredProcedure_Update("USP_CC_Update");
            }
            catch (Exception)
            {
                throw;
            }
        }

        //OracleParameter p_content = new OracleParameter();
        //p_content.ParameterName = "p_content";
        //p_content.OracleDbType = OracleDbType.NVarchar2;
        //p_content.Value = customContent.content;
        //manager.AddParameter(p_content);

        private void MapParameters(VMCustomContent customContent)
        {
            manager.AddParameter(new OracleParameter("p_is_url", customContent.is_url));
            manager.AddParameter(new OracleParameter("p_url", customContent.url));

            OracleParameter p_content = new OracleParameter();
            p_content.ParameterName = "p_content";
            p_content.OracleDbType = OracleDbType.NVarchar2;
            p_content.Value = customContent.content;
            manager.AddParameter(p_content);
            //manager.AddParameter(new OracleParameter("p_content", customContent.content));


        }
        public void Delete(int id)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_custom_content_id", id));

                manager.CallStoredProcedure_Update("USP_CC_Delete");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}