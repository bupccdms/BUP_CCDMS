using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace qms.ReportModels
{
    public class RM_Break
    {
        [Display(Name = "Department Name")]
        public string department_name { get; set; }

        [Display(Name = "Date")]
        public string create_time { get; set; }

        [Display(Name = "User")]
        public string username { get; set; }

        [Display(Name = "Device")]
        public string device_no { get; set; }

        [Display(Name = "Reason For Break")]
        public string break_type_name { get; set; }

        [Display(Name = "Start Time")]
        public string start_time { get; set; }

        [Display(Name = "End Time")]
        public string end_time { get; set; }

        [Display(Name = "Duration")]
        public string duration { get; set; }
        
    }
}