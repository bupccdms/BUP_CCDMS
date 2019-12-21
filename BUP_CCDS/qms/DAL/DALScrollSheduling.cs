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
    public class DALScrollSheduling
    {
        OracleDataManager manager = new OracleDataManager();
        public DataTable GetAll()
        {
            try
            {
                OracleParameter param = new OracleParameter("po_Cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_SS_SelectAll");
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
                manager.AddParameter(new OracleParameter("p_scroll_sheduling_id", id));
                OracleParameter param = new OracleParameter("po_cursor", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                manager.AddParameter(param);

                return manager.CallStoredProcedure_Select("USP_SS_SelectById");
            }
            catch (Exception)
            {

                throw;
            }
        }


        public int Insert(VMScrollSheduling scrollSheduling)
        {
            try
            {
                MapParameters(scrollSheduling);
                long? scroll_Sheduling_id = manager.CallStoredProcedure_Insert("USP_SS_Insert");
                if (scroll_Sheduling_id.HasValue) return (int)scroll_Sheduling_id.Value;
                else return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(VMScrollSheduling scrollSheduling)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_scroll_sheduling_id", scrollSheduling.scroll_Sheduling_id));
                MapParameters(scrollSheduling);
                manager.CallStoredProcedure_Update("USP_SS_Update");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void MapParameters(VMScrollSheduling scrollSheduling)
        {
            manager.AddParameter(new OracleParameter("p_scroll_id", scrollSheduling.scroll_id));
            manager.AddParameter(new OracleParameter("p_when_start", scrollSheduling.when_start));
            manager.AddParameter(new OracleParameter("p_duration", scrollSheduling.duration));
            manager.AddParameter(new OracleParameter("p_is_active", scrollSheduling.is_active));
            manager.AddParameter(new OracleParameter("p_is_start", scrollSheduling.is_start));
            manager.AddParameter(new OracleParameter("p_is_end", scrollSheduling.is_end));
        }
        public void Delete(int id)
        {
            try
            {
                manager.AddParameter(new OracleParameter("p_scroll_sheduling_id", id));

                manager.CallStoredProcedure_Update("USP_SS_Delete");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Approve(VMScrollSheduling scrollSheduling)
        {
            try
            {
                MapParameters(scrollSheduling);

                //manager.AddParameter(new OracleParameter("p_is_active", scrollSheduling.is_active));
                manager.AddParameter(new OracleParameter("p_scroll_sheduling_id", scrollSheduling.scroll_Sheduling_id));
                manager.CallStoredProcedure_Update("USP_Scroll_Sheduling_Approve");

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}