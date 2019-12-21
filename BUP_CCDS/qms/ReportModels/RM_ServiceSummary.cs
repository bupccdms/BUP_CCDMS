using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace qms.ReportModels
{
    public class RM_ServiceSummary
    {
        [Display(Name = "Department Name")]
        public string department_name { get; set; }

        [Display(Name = "Service Name")]
        public string service_sub_type_name { get; set; }

        [Display(Name = "Token Served")]
        public int token_served { get; set; }

        [Display(Name = "%Total")]
        public decimal total_percentage { get; set; }

        [Display(Name = "Standard Time")]
        public string standard_time { get; set; }

        [Display(Name = "Actual Time")]
        public string actual_time { get; set; }

        [Display(Name = "Variance")]
        public string variance { get; set; }
       

    }
}