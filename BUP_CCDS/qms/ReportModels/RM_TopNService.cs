using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace qms.ReportModels
{
    public class RM_TopNService
    {
        [Display(Name = "Department Name")]
        public string department_name { get; set; }

        [Display(Name = "Service Name")]
        public string service_name { get; set; }

        [Display(Name = "Total Servic")]
        public int total_service { get; set; }

    }
}