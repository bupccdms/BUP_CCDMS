using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace qms.ViewModels
{
    public class VMPlayListSheduling
    {
        public int PlayList_Sheduling_id { get; set; }
        [Required(ErrorMessage = "Please Select PlayList!")]
        public int PlayList_id { get; set; }
        public int department_id { get; set; }
        public int is_active { get; set; }
        public int is_start { get; set; }
        public int is_end { get; set; }
        public string department_name { get; set; }
        public string PlayList_name { get; set; }
        [Required(ErrorMessage = "Enter When Start!")]
        public System.DateTime when_start { get; set; }
        [Required(ErrorMessage = "Enter Duration in Minuites!")]
        public int duration { get; set; }
        public string when_start_string
        {
            get
            {
                return when_start.ToString("dd-MM-yyyy");
            }
        }

        public bool bool_is_active
        {
            get
            {
                return (is_active == 1 ? true : false);
            }
            set
            {
                is_active = (value == true ? 1 : 0);
            }
        }


    }
}