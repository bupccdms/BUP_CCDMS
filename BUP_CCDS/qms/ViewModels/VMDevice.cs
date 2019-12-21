using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace qms.ViewModels
{
    public class VMDevice
    {
        public int device_id { get; set; }
        public int department_id { get; set; }
        public string user_id { get; set; }
        public string department_name { get; set; }
        public string device_no { get; set; }
        public string device_name { get; set; }
        public string hometown { get; set; }
        
        public int is_active { get; set; }
        public int is_active_directory_user { get; set; }
        public string location { get; set; }
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}