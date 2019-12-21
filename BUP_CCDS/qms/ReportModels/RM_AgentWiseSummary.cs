using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace qms.ReportModels
{
    public class RM_AgentWiseSummary
    {
        [Display(Name = "Department Name")]
        public string department_name { get; set; }

        [Display(Name = "Agent Name")]
        public string user_name { get; set; }

        [Display(Name = "Handled Customers")]
        public string handled_customer { get; set; }

        [Display(Name = "Average Waiting Time")]
        public string average_waiting_time { get; set; }

        [Display(Name = "Average Service Time")]
        public string average_service_time { get; set; }

        [Display(Name = "Avg. TAT")]
        public string average_TAT { get; set; }

    }
}