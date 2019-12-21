using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace qms.ReportModels
{
    public class RM_TokenExceeding
    {
        [Display(Name = "Department Name")]
        public string department_name { get; set; }

        [Display(Name = "User Name")]
        public string user_name { get; set; }

        [Display(Name = "Total Served Tokens")]
        public int total_served_token { get; set; }

        [Display(Name = "Total Exceeding Tokens")]
        public int total_exceedig_token { get; set; }
    }
}