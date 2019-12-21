using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace qms.Models
{
    public class AspNetUserLoginAttempts
    {
        [Key]
        public long attempt_id { get; set; }

        public string LoginProvider { get; set; }

        public int department_id { get; set; }

        [Display(Name = "Department Name")]
        public string department_name { get; set; }

        public string user_id { get; set; }

        [StringLength(256)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        public string Email { get; set; }

        [Display(Name = "Role")]
        public string RoleName { get; set; }

        public int device_id { get; set; }

        [Display(Name = "Device No")]
        public string device_no { get; set; }

        [StringLength(20)]
        [Display(Name = "IP Address")]
        public string ip_address { get; set; }

        [StringLength(100)]
        [Display(Name = "Machine Name")]
        public string machine_name { get; set; }

        public DateTime attempt_time { get; set; }

        [Display(Name = "Attempt Time")]
        public string attempt_time_formatted
        {
            get
            {
                return attempt_time.ToString("dd/MMM/yyyy hh:mm:ss tt");
            }
        }
        public int is_success { get; set; }

        [Display(Name = "Status")]
        public string status => (is_success == 0 ? "Fail" : "Success");


    }
}