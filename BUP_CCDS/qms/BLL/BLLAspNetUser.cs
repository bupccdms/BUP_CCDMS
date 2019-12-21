using qms.DAL;
using qms.Models;
using qms.Utility;
using qms.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace qms.BLL
{
    public class BLLAspNetUser
    {
        public List<AspNetUser> GetAllUser()
        {
            DALAspNetUser dal = new DALAspNetUser();
            DataTable dt = dal.GetAllUser();
            return ObjectMappingList(dt);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="securityToken"></param>
        /// <returns></returns>
        public void DeleteTable()
        {
            DALAspNetUser dal = new DALAspNetUser();
             dal.DeleteTable();
        }
        public AspNetUser GetUserBySecurityCode(string securityToken)
        {
            DALAspNetUser dal = new DALAspNetUser();
            DataTable dt = dal.GetUserBySecurityCode(securityToken);
            if (dt.Rows.Count > 0)
                return ObjectMapping(dt.Rows[0]);
            else return null;
        }

        public VMSessionInfo GetSessionInfoByUserName(string userName, HttpSessionStateBase session)
        {
            DALAspNetUser dal = new DALAspNetUser();
            DataTable dt = dal.GetSessionInfoByUserName(userName);
            VMSessionInfo sessionInfo = ObjectMappingSession(dt);
            SessionManager sm = new SessionManager(session);
            sm.ForceChangeConfirmed = sessionInfo.force_change_confirmed;
            List<AspNetUserPassword> passwordList = GetPasswordInfo(sessionInfo.user_id);
            PasswordExpiryCheck(passwordList, sm);

            return sessionInfo;
        }

        public VMSessionInfo GetUserInfoByUserName(string userName)
        {
            DALAspNetUser dal = new DALAspNetUser();
            DataTable dt = dal.GetSessionInfoByUserName(userName);
            VMSessionInfo sessionInfo = ObjectMappingSession(dt);
            
            return sessionInfo;
        }

        public AspNetUserLogin GetLoginInfo(string user_id)
        {
            DALAspNetUser dal = new DALAspNetUser();
            DataTable dt = dal.GetLoginInfo(user_id);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new AspNetUserLogin()
                {
                    LoginProvider = (row["LoginProvider"] == DBNull.Value ? null : row["LoginProvider"].ToString()),
                    ProviderKey = (row["ProviderKey"] == DBNull.Value ? null : row["ProviderKey"].ToString()),
                    UserId = (row["UserId"] == DBNull.Value ? null : row["UserId"].ToString()),
                    department_id = (row["department_id"] == DBNull.Value ? 0 : Convert.ToInt32(row["department_id"])),
                    device_id = (row["device_id"] == DBNull.Value ? 0 : Convert.ToInt32(row["device_id"])),
                    login_time = (row["login_time"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(row["login_time"])),
                    logout_time = (row["logout_time"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(row["logout_time"])),
                    idle_from = (row["idle_from"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(row["idle_from"])),
                    is_idle = (row["is_idle"] == DBNull.Value ? 0 : Convert.ToInt32(row["is_idle"]))
                };
            }

            return null;
        }

        public List<AspNetUserLoginAttempts> GetLoginAttemptsInfo(int department_id, string user_id, int device_id, int is_success, DateTime start_date, DateTime end_date)
        {
            DALAspNetUser dal = new DALAspNetUser();
            DataTable dt = dal.GetLoginAttemptsInfo(department_id, user_id, device_id, is_success, start_date, end_date);
            List<AspNetUserLoginAttempts> list = new List<AspNetUserLoginAttempts>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    list.Add(new AspNetUserLoginAttempts()
                    {
                        attempt_id = (row["attempt_id"] == DBNull.Value ? 0 : Convert.ToInt32(row["attempt_id"])),
                        LoginProvider = (row["LoginProvider"] == DBNull.Value ? "" : row["LoginProvider"].ToString()),
                        department_id = (row["department_id"] == DBNull.Value ? 0 : Convert.ToInt32(row["department_id"])),
                        department_name = (row["department_name"] == DBNull.Value ? "" : row["department_name"].ToString()),
                        user_id = (row["user_id"] == DBNull.Value ? "" : row["user_id"].ToString()),
                        UserName = (row["UserName"] == DBNull.Value ? "" : row["UserName"].ToString()),
                        FullName = (row["FullName"] == DBNull.Value ? "" : row["FullName"].ToString()),
                        Email = (row["Email"] == DBNull.Value ? "" : row["Email"].ToString()),
                        RoleName = (row["RoleName"] == DBNull.Value ? "" : row["RoleName"].ToString()),
                        device_id = (row["device_id"] == DBNull.Value ? 0 : Convert.ToInt32(row["device_id"])),
                        device_no = (row["device_no"] == DBNull.Value ? "" : row["device_no"].ToString()),
                        ip_address = (row["ip_address"] == DBNull.Value ? "" : row["ip_address"].ToString()),
                        machine_name = (row["machine_name"] == DBNull.Value ? "" : row["machine_name"].ToString()),
                        attempt_time = (row["attempt_time"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(row["attempt_time"])),
                        is_success = (row["is_success"] == DBNull.Value ? 0 : Convert.ToInt32(row["is_success"]))
                    });
                }
                
            }
            return list;
        }

        public void AddLoginInfo(AspNetUserLogin loginInfo)
        {
            DALAspNetUser dal = new DALAspNetUser();
            dal.InsertLoginInfo(loginInfo);
        }

        public void AddLoginAttemptInfo(AspNetUserLoginAttempts loginAttempt)
        {
            DALAspNetUser dal = new DALAspNetUser();
            dal.InsertLoginAttemptInfo(loginAttempt);
        }

        public void AddPasswordInfo(string user_id, string password)
        {
            DALAspNetUser dal = new DALAspNetUser();
            dal.InsertPasswordInfo(user_id, Cryptography.Encrypt(password, true));
        }

        public void UserForceChangeConfirmed(string user_id, bool isForceChangeConfirmed)
        {
            DALAspNetUser dal = new DALAspNetUser();
            dal.UserForceChangeConfirmed(user_id, isForceChangeConfirmed);
        }

        /// <summary>
        /// Check the new password is used previously when change password
        /// </summary>
        /// <param name="user_id">Encrypted ID data of user</param>
        /// <param name="password">New password</param>
        /// <param name="lastUsedCount">Configured Last Used Count</param>
        /// <returns>True for if used else return False</returns>
        public bool IsPasswordPreviouslyUsed(string user_id, string password)
        {
            List<AspNetUserPassword> passwordList = GetPasswordInfo(user_id);
            if (passwordList.Count > 0)
            {
                List<AspNetUserPassword> lastUsedList = passwordList.OrderByDescending(o => o.changetime).Take(ApplicationSetting.PasswordLastCheckingCount).ToList();
                if (lastUsedList.Any())
                {
                    return lastUsedList.Where(w => w.VerifyPassword(password)).Count() > 0;
                }
                return false;
            }
            return false;
        }

        private void PasswordExpiryCheck(List<AspNetUserPassword> passwordList, SessionManager sm)
        {
            
            if (passwordList.Count > 0)
            {
                sm.IsPasswordExpired = passwordList.OrderByDescending(o => o.changetime).FirstOrDefault().changetime.AddDays(ApplicationSetting.PasswordExpiredAfter) < DateTime.Now;
                int notifyDaysRemaining = passwordList.OrderByDescending(o => o.changetime).FirstOrDefault().changetime.AddDays(ApplicationSetting.PasswordExpiredAfter).Subtract(DateTime.Now).Days;
                if (notifyDaysRemaining <= ApplicationSetting.PasswordExpiryNotifyBefore)
                    sm.PasswordExpiryNotifyBeforeDays = notifyDaysRemaining;
                else
                    sm.PasswordExpiryNotifyBeforeDays = 0;
            }
            else
            {
                sm.IsPasswordExpired = true;
                sm.PasswordExpiryNotifyBeforeDays = 0;
            }
            
        }


        private List<AspNetUserPassword> GetPasswordInfo(string user_id)
        {
            DALAspNetUser dal = new DALAspNetUser();
            DataTable dt = dal.GetPasswordInfo(user_id);
            List<AspNetUserPassword> list = new List<AspNetUserPassword>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    list.Add(
                    new AspNetUserPassword()
                    {
                        password_id = (row["PASSWORD_ID"] == DBNull.Value ? 0 : Convert.ToInt32(row["PASSWORD_ID"])),
                        user_id = (row["USERID"] == DBNull.Value ? "" : row["USERID"].ToString()),
                        changetimehash = (row["CHANGETIMEHASH"] == DBNull.Value ? "" : row["CHANGETIMEHASH"].ToString()),
                        passwordhash = (row["PASSWORDHASH"] == DBNull.Value ? "" : row["PASSWORDHASH"].ToString())
                    });
                }
            }
            return list;
        }


        //public void UpdateLoginInfo(string loginProvider, int device_id)
        //{
        //    DALAspNetUser dal = new DALAspNetUser();
        //    dal.UpdateLoginInfo(loginProvider, device_id);
        //}
        public void UpdateLoginInfo(string loginProvider, int department_id)
        {
            DALAspNetUser dal = new DALAspNetUser();
            dal.UpdateLoginInfo(loginProvider, department_id);
        }
        public void UpdateDepartmentAdminLoginInfo(string loginProvider, int department_id)
        {
            DALAspNetUser dal = new DALAspNetUser();
            dal.UpdateDepartmentAdminLoginInfo(loginProvider, department_id);
        }

        public void SetActivation(string user_id, int is_active)
        {
            DALAspNetUser dal = new DALAspNetUser();
            dal.SetActivation(user_id, is_active);
        }

        public void SetActiveDirectoryUser(string user_id, int is_active_directory_user)
        {
            DALAspNetUser dal = new DALAspNetUser();
            dal.SetActiveDirectoryUser(user_id, is_active_directory_user);
        }

        public void UpdateLogoutInfo(string loginProvider, int? logout_type_id, string logoutReason)
        {
            DALAspNetUser dal = new DALAspNetUser();
            dal.UpdateLogoutInfo(loginProvider, logout_type_id, logoutReason);
        }

        public void DeleteLoginInfo(string loginProvider)
        {
            DALAspNetUser dal = new DALAspNetUser();
            dal.DeleteLoginInfo(loginProvider);
        }

        internal AspNetUser ObjectMapping(DataRow row)
        {
            AspNetUser user = new AspNetUser();
            user.Id = (row["Id"] == DBNull.Value ? null : row["Id"].ToString());
            user.PhoneNumber = (row["PhoneNumber"] == DBNull.Value ? null : row["PhoneNumber"].ToString());
            user.UserName = (row["UserName"] == DBNull.Value ? null : row["UserName"].ToString());
            user.Email = (row["Email"] == DBNull.Value ? null : row["Email"].ToString());
            user.Hometown = (row["Hometown"] == DBNull.Value ? null : row["Hometown"].ToString());
            return user;
        }

        internal List<AspNetUser> ObjectMappingList(DataTable dt)
        {
            List<AspNetUser> list = new List<AspNetUser>();
            foreach (DataRow row in dt.Rows)
            {
                AspNetUser user = new AspNetUser();
                user.Id = (row["Id"] == DBNull.Value ? null : row["Id"].ToString());
                user.PhoneNumber = (row["PhoneNumber"] == DBNull.Value ? null : row["PhoneNumber"].ToString());
                user.UserName = (row["UserName"] == DBNull.Value ? null : row["UserName"].ToString());
                user.Email = (row["Email"] == DBNull.Value ? null : row["Email"].ToString());
                user.Hometown = (row["Hometown"] == DBNull.Value ? null : row["Hometown"].ToString());
                

                list.Add(user);

            }
            return list;
        }

        internal VMSessionInfo ObjectMappingSession(DataTable dt)
        {
            if(dt.Rows.Count>0)
            {
                DataRow row = dt.Rows[0];
                VMSessionInfo sessionInfo = new VMSessionInfo();
                sessionInfo.user_id = (row["user_id"] == DBNull.Value ? null : row["user_id"].ToString());
                sessionInfo.user_name = (row["user_name"] == DBNull.Value ? null : row["user_name"].ToString());
                sessionInfo.role_name = (row["role_name"] == DBNull.Value ? null : row["role_name"].ToString());
                sessionInfo.department_id = (row["department_id"] == DBNull.Value ? 0 : Convert.ToInt32(row["department_id"]));
                sessionInfo.department_name = (row["department_name"] == DBNull.Value ? null : row["department_name"].ToString());
                sessionInfo.device_id = (row["device_id"] == DBNull.Value ? 0 : Convert.ToInt32(row["device_id"]));
                sessionInfo.force_change_confirmed = (row["FORCECHANGECONFIRMED"] == DBNull.Value ? false : (Convert.ToInt32(row["FORCECHANGECONFIRMED"]) == 1 ? true : false));
                return sessionInfo;

            }
            return null;
        }


        private class AspNetUserPassword
        {
            public int password_id { get; set; }
            
            public string user_id { get; set; }
            public string changetimehash { get; set; }
            public DateTime changetime
            {
                get
                {
                    string strChangetime = Cryptography.Decrypt(changetimehash, true);
                    try
                    {
                        return Convert.ToDateTime(strChangetime);
                    }
                    catch (Exception)
                    {
                        return DateTime.Now;
                    }
                }
            }
            public string passwordhash { get; set; }
            
            public bool VerifyPassword(string newPassword)
            {
                string decryptedPassword = Cryptography.Decrypt(passwordhash, true);
                return newPassword == decryptedPassword;
            }
        }
    }
}