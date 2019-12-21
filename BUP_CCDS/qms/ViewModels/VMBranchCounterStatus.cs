using qms.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace qms.ViewModels
{
    public class VMDepartmentDeviceStatus
    {
        public int department_id { get; set; }
        public string department_name { get; set; }
        public int device_id { get; set; }
        public string device_no { get; set; }
        public long token_id { get; set; }
        public string token_prefix { get; set; }
        public int token_no { get; set; }
        public string token_no_formated
        {
            get
            {
                if (token_no > 0)
                    return token_prefix + token_no.ToString().PadLeft(ApplicationSetting.PaddingLeft, '0');
                else
                    return "";
            }
        }
        public DateTime? call_time { get; set; }
        public string call_time_formated
        {
            get
            {
                if (!call_time.HasValue)
                    return "";
                else
                    return call_time.Value.ToString("hh:mm tt");
            }
        }
        public string user_id { get; set; }
        public string user_full_name { get; set; }
        public string user_name { get; set; }

        public DateTime? login_time { get; set; }
        public string login_time_formated
        {
            get
            {
                if (!login_time.HasValue)
                    return "";
                else
                    return login_time.Value.ToString("hh:mm tt");
            }
        }
        public DateTime? logout_time { get; set; }
        public string logout_time_formated
        {
            get
            {
                if (!logout_time.HasValue)
                    return "";
                else
                    return logout_time.Value.ToString("hh:mm tt");
            }
        }
        public int is_idle { get; set; }
        public DateTime? idle_from { get; set; }
        public string idle_from_formated
        {
            get
            {
                if (!idle_from.HasValue)
                    return "";
                else
                    return idle_from.Value.ToString("hh:mm tt");
            }
        }

        public string Status
        {
            get
            {
                if (!login_time.HasValue)
                    return "OFF";
                else if (is_idle > 0)
                    return "IDLE";
                else
                    return "SERVING";
            }
        }
    }
}