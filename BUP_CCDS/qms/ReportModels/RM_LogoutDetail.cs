using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace qms.ReportModels
{
    public class RM_LogoutDetail
    {
        [Display(Name = "Department Name")]
        public string department_name { get; set; }

        [Display(Name = "Agent Name")]
        public string user_name { get; set; }

        [Display(Name = "Device")]
        public string device_no { get; set; }

        [Display(Name = "Date")]
        public string service_date_formated
        {
            get
            {
                return login_time.ToString("dd/MMM/yyyy");
            }
        }
        
        public DateTime login_time { get; set; }

        [Display(Name = "Login Time")]
        public string login_time_formated
        {
            get
            {
                return login_time.ToString("hh:mm:ss tt");
            }
        }

        

        [Display(Name = "Logout Time")]
        public string logout_time_formated { get; set; }


        [Display(Name = "Duration")]
        public string duration { get; set; }

        [Display(Name = "Reason")]
        public string logout_reason { get; set; }
    }
}