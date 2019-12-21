using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace qms.ViewModels
{
    public class VMScrollSheduling
    {
        public int scroll_Sheduling_id { get; set; }
        //[Required]
        //[Display(Name = "Scroll", Prompt = "Select Scroll")]
        [Required(ErrorMessage = "Please Select Scroll!")]
        public int scroll_id { get; set; }
        public int department_id { get; set; }
        public int is_active { get; set; }
        public int is_start { get; set; }
        public int is_end { get; set; }
        public string department_name { get; set; }
        public string content_bn { get; set; }
        [Required(ErrorMessage = "Enter When Start!")]
        public System.DateTime when_start { get; set; }
        [Required(ErrorMessage = "Enter Duration in Minuites!")]
        public int duration { get; set; }
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