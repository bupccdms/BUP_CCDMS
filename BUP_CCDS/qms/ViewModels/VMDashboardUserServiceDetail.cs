using qms.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace qms.ViewModels
{
    public class VMDashboardUserServiceDetail
    {
        public string token_prefix { get; set; }
        public int token_no { get; set; }
        public string token_no_formated
        {
            get
            {
                return token_prefix + token_no.ToString().PadLeft(ApplicationSetting.PaddingLeft, '0');
            }
        }
        public string customer_type { get; set; }
        public string device { get; set; }
        public string department_name { get; set; }
        public string service { get; set; }
        public DateTime issue_time { get; set; }
        public string issue_time_formated {
            get
            {
                return issue_time.ToString("hh:mm:ss tt");
            }
        }
        public DateTime start_time { get; set; }
        public string start_time_formated
        {
            get
            {
                return start_time.ToString("hh:mm:ss tt");
            }
        }
        public DateTime end_time { get; set; }
        public string end_time_formated
        {
            get
            {
                if (end_time.ToString("hh:mm:ss tt") != "12:00:00 AM")
                {
                    return end_time.ToString("hh:mm:ss tt");
                }
                else
                {
                    return "";
                }
            }
        }
        public string service_status { get; set; }
        

    }
}