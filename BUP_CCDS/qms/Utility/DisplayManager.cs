using qms.Models;
using qms.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace qms.Utility
{
    public class DisplayManager
    {
       
        private BLL.BLLToken dbManager = new BLL.BLLToken();
        public void CreateTextFile(int department_id, string static_ip)
        {
            string textFileValue = GetInProgressTokens(department_id);
            string nextTokens = GetNextTokens(department_id);

            if (nextTokens.Length > 0)
                textFileValue = textFileValue + "\n" + nextTokens;

            string filePath = Path.Combine(ApplicationSetting.DisplayPath, static_ip + ".txt");
            StreamWriter sw = File.CreateText(filePath);
            sw.Write(textFileValue);
            sw.Close();
        }

        public string GetInProgressTokens(int department_id)
        {
            List<VMTokenProgress> progressingTokenList = GetInProgressTokenList(department_id);

            StringBuilder sb = new StringBuilder();
            if (progressingTokenList.Any())
            {
                foreach (var token in progressingTokenList)
                {
                    sb.Append(token.token_no + "\t");
                }
            }


            return sb.ToString().TrimEnd('\t');
        }

        public List<VMTokenProgress> GetInProgressTokenList(int department_id)
        {
            try
            {

                List<VMTokenProgress> progressingTokens = dbManager.GetProgressTokenList(department_id).ToList();
                
                return progressingTokens;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public string GetNextTokens(int department_id)
        {
            return dbManager.GetNextTokenList(department_id);           
        }




        
    }
}