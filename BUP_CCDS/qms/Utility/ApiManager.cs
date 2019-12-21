using qms.BLL;
using qms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace qms.Utility
{
    public class ApiManager
    {
        internal static AspNetUser ValidUserBySecurityToken(string securityToken)
        {
            try
            {
                string loginProvider = Cryptography.Decrypt(securityToken, true);
                BLLAspNetUser dbUser = new BLLAspNetUser();
                AspNetUser user = dbUser.GetUserBySecurityCode(loginProvider);
                if (user != null)
                {

                    return user;
                }
                throw new Exception("Invalid security token");
            }
            catch (Exception)
            {

                throw;
            }
        }

        internal static void Loggin(string securityToken, string methode_name, string request_json, string response_json)
        {
            if (ApplicationSetting.AllowAPILoggin)
            {
                ApiRequestLog log = new ApiRequestLog()
                {
                    loginprovider = Cryptography.Decrypt(securityToken, true),
                    methode_name = methode_name,
                    request_json = request_json,
                    response_json = response_json
                };
                new BLLLog().ApiRequestLogCreate(log);
            }
        }
    }
}