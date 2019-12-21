using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace qms.ReportModels
{
    public class RM_SingleVSMultipleVisitedCustomer
    {
        [Display(Name = "Department Name")]
        public string department_name { get; set; }

        [Display(Name = "Service Name")]
        public string service_sub_type_name { get; set; }

        [Display(Name = "Total Served Token")]
        public string total_served_token { get; set; }

        [Display(Name = "Single Visited Customers")]
        public string single_visit_customer { get; set; }

        [Display(Name = "Multiple Visited Customers")]
        public string multiple_visit_customer { get; set; }


    }
}