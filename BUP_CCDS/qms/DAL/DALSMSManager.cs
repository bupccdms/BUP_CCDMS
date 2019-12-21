using System;
using System.Collections.Generic;
using Oracle.DataAccess.Client;
using System.Linq;
using System.Web;

namespace qms.DAL
{
    public class DALSMSManager
    {
        public void SendSMS(string msisdn, string message)
        {
            try
            {
                OracleDataManager manager = new OracleDataManager();
                manager.AddParameter(new OracleParameter("P_MSISDN", msisdn));
                manager.AddParameter(new OracleParameter("P_MESSAGE", message));
                manager.CallStoredProcedure("USP_SENDSMS");
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public void SendSMSBn(string msisdn, string messageBn, string tokenBn)
        {
            try
            {
                OracleDataManager manager = new OracleDataManager();

                manager.AddParameter(new OracleParameter("P_MSISDN", msisdn));
                OracleParameter P_FULL_MESSAGE_BN = new OracleParameter();
                P_FULL_MESSAGE_BN.ParameterName = "P_FULL_MESSAGE_BN";
                P_FULL_MESSAGE_BN.OracleDbType = OracleDbType.NVarchar2;
                P_FULL_MESSAGE_BN.Value = messageBn;
                manager.AddParameter(P_FULL_MESSAGE_BN);

                OracleParameter P_TOKEN_NO_BN = new OracleParameter();
                P_TOKEN_NO_BN.ParameterName = "P_TOKEN_NO_BN";
                P_TOKEN_NO_BN.OracleDbType = OracleDbType.NVarchar2;
                P_TOKEN_NO_BN.Value = tokenBn;
                manager.AddParameter(P_TOKEN_NO_BN);
                
                manager.CallStoredProcedure("USP_SENDSMS_BN");
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}