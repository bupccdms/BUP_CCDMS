using qms.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Oracle.DataAccess.Client;
using System.Linq;
using System.Web;

namespace qms.DAL
{
    public class DALScroll
    {
        OracleDataManager manager = new OracleDataManager();
        public DataTable GetAll()
        {
            try
            {
                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);
                return manager.CallStoredProcedure_Select("USP_DF_SelectAll");
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
                manager.AddParameter(new OracleParameter("p_scroll_id", id));
                OracleParameter param = new OracleParameter("po_cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_DF_SelectById");
            }
            catch (Exception)
            {
                throw;
            }
        }


        public DataTable GetByDepartmentId(int department_id)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_department_id", department_id));
                OracleParameter param = new OracleParameter("po_cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_DF_SelectByDepartmentId");
            }
            catch (Exception)
            {

                throw;
            }
        }

        
        public DataTable GetByUserId(string user_id)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_user_id", user_id));
                OracleParameter param = new OracleParameter("po_cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_DF_SelectByUserId");
            }
            catch (Exception)
            {

                throw;
            }


        }

        public int Insert(tblScroll scroll)
        {
            try
            {
                MapParameters(scroll);
                long? scroll_id = manager.CallStoredProcedure_Insert("USP_DF_Insert");
                if (scroll_id.HasValue) return (int)scroll_id.Value;
                else return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(tblScroll scroll)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_scroll_id", scroll.scroll_id));
                manager.AddParameter(new OracleParameter("p_is_active", scroll.is_active));
                MapParameters(scroll);
                manager.CallStoredProcedure_Update("USP_DF_Update");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Force(int department_id, tblScroll scroll)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_department_id", department_id));
                if (scroll == null)
                {
                    int scroll_id = 0;
                    manager.AddParameter(new OracleParameter("p_scroll_id", scroll_id));
                }
                else
                {
                    manager.AddParameter(new OracleParameter("p_scroll_id", scroll.scroll_id));
                }
                manager.CallStoredProcedure_Update("USP_DF_Force");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SetActivation(int scroll_id, int is_active)
        {
            try
            {
                manager.AddParameter(new OracleParameter() { ParameterName = "p_scroll_id", Value = scroll_id });
                manager.AddParameter(new OracleParameter() { ParameterName = "p_is_active", Value = is_active });

                manager.CallStoredProcedure_Delete("USP_Scroll_SetOn");
            }
            catch (Exception)
            {

                throw;
            }
        }
       
        private void MapParameters(tblScroll scroll)
        {
            manager.AddParameter(new OracleParameter("p_content_en", scroll.content_en));
            OracleParameter p_content_bn = new OracleParameter();
            p_content_bn.ParameterName = "p_content_bn";
            p_content_bn.OracleDbType = OracleDbType.NVarchar2;
            p_content_bn.Value = scroll.content_bn;
            manager.AddParameter(p_content_bn);
            manager.AddParameter(new OracleParameter("p_is_global", scroll.is_global));
        }
        public void Delete(int id)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_scroll_id", id));

                manager.CallStoredProcedure_Update("USP_DF_Delete");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}