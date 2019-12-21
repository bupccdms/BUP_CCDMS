using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace qms.ViewModels
{
    public class VMDepartmentLogin
    {
        [Display(Name="Department ID") ]
        public int department_id { get; set; }
        [Display(Name="User")]
        public string user_id { get; set; }
        public int device_id { get; set; }
        public string role_id { get; set; }
        public int user_department_id { get; set; }
        public string department_name { get; set; }
        public string Hometown { get; set; }

        
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }

        public string device_no { get; set; }
        public string MyProperty8 { get; set; }
        public int Lockout { get; set; }
        public int is_active { get; set; }
        public int is_active_directory_user { get; set; }
    }
}