﻿using System;
using System.Collections.Generic;
using System.Data;
using Oracle.DataAccess.Client;
using System.Linq;
using System.Web;

namespace qms.DAL
{
    public class OracleDataManager
    {
        OracleConnection connection;
        OracleCommand command;

        string connectionString = String.Empty;
        public OracleDataManager()
        {
            LoadConnectionString();
            connection = new OracleConnection(connectionString);
            command = new OracleCommand();
        }

        public OracleDataManager(string ConnectionString)
        {
            LoadConnectionString(ConnectionString);
            connection = new OracleConnection(connectionString);
            command = new OracleCommand();
        }

        private void LoadConnectionString()
        {
            try
            {
                connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        private void LoadConnectionString(string name)
        {
            try
            {
                connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[name].ConnectionString;
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// This method directly call the stored procedure where procedure name is mention in argument. If need any parameter passing, just do it before call this method by AddParameter()
        /// </summary>
        /// <param name="storedProcedureName">The store procedure name</param>
        public void CallStoredProcedure(string storedProcedureName)
        {
            using (connection)
            {
                try
                {
                    if (connection.State != ConnectionState.Open) connection.Open();

                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = storedProcedureName;
                    OracleTransaction transaction = connection.BeginTransaction();
                    command.Transaction = transaction;
                    try
                    {
                        command.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                    
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    if (connection.State != ConnectionState.Closed) connection.Close();
                }
            }
        }

        public DateTime CallStoredProcedure_DBDate()
        {
            DateTime pkValue = DateTime.Now;

            OracleParameter param = new OracleParameter("po_PKValue", OracleDbType.Date)
            {
                Direction = ParameterDirection.Output
            };
            AddParameter(param);
            
            try
            {
                CallStoredProcedure("SSP_GetDBDate");

                if (param.Value != DBNull.Value)
                    pkValue = (DateTime)((Oracle.DataAccess.Types.OracleDate)param.Value).Value;
            }
            catch (Exception)
            {

                throw;
            }


            return pkValue;
        }

        public void AddParameter(OracleParameter param)
        {
            command.Parameters.Add(param);
        }

        public DataSet CallStoredProcedure_SelectDS(string storedProcedureName)
        {

            using (connection)
            {
                try
                {
                    if (connection.State != ConnectionState.Open) connection.Open();

                    command.Connection = connection;
                    command.CommandText = storedProcedureName;
                    command.CommandType = CommandType.StoredProcedure;

                    DataSet dt = new DataSet(storedProcedureName);

                    using (OracleDataAdapter adapter = new OracleDataAdapter(command))
                    {
                        //adapter.SelectCommand = command;
                        adapter.Fill(dt);
                    }
                    return dt;
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {
                    if (connection.State != ConnectionState.Closed) connection.Close();
                }
            }
        }

        public DataTable CallStoredProcedure_Select(string storedProcedureName)
        {
            
            using (connection)
            {
                try
                {
                    if (connection.State != ConnectionState.Open) connection.Open();

                    command.Connection = connection;
                    command.CommandText = storedProcedureName;
                    command.CommandType = CommandType.StoredProcedure;
                    
                    DataTable dt = new DataTable(storedProcedureName);

                    using (OracleDataAdapter adapter = new OracleDataAdapter(command))
                    {
                        //adapter.SelectCommand = command;
                        adapter.Fill(dt);
                    }                       
                    return dt;
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {
                    if (connection.State != ConnectionState.Closed) connection.Close();
                }
            }
        }

        /// <summary>
        /// This method directly call the stored procedure where procedure name is mention in argument.It must return inserted primary key value. If need any parameter passing, just do it before call this method by AddParameter()
        /// Primary key value[Developmer must implement code in SP for return]
        /// </summary>
        /// <param name="storedProcedureName">Enter Stored Procedure Name</param>
        /// <returns>Primary key value[Developmer must implement code in SP for return]</returns>
        public long? CallStoredProcedure_Insert(string storedProcedureName)
        {
            long? pkValue = null;
            OracleParameter param = new OracleParameter("po_PKValue", OracleDbType.Decimal)
            {
                Direction = ParameterDirection.Output
            };

            AddParameter(param);
            try
            {
                CallStoredProcedure(storedProcedureName);

                if (param.Value != DBNull.Value)
                    pkValue = (long)((Oracle.DataAccess.Types.OracleDecimal)param.Value).Value; 
            }
            catch (Exception)
            {

                throw;
            }
           

            return pkValue;
        }
        public void CallStoredProcedure_Update(string storedProcedureName)
        {
            try
            {
                CallStoredProcedure(storedProcedureName);

                
            }
            catch (Exception)
            {

                throw;
            }

            
        }
        public void CallStoredProcedure_Delete(string storedProcedureName)
        {
            try
            {
                CallStoredProcedure(storedProcedureName);


            }
            catch (Exception)
            {

                throw;
            }


        }
    }
}