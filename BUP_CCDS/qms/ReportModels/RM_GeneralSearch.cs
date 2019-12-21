using qms.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace qms.ReportModels
{
    public class RM_GeneralSearch
    {
        [Display(Name = "Department Name")]
        public string department_name { get; set; }

        [Display(Name = "Date")]
        public string Service_date { get; set; }

        [Display(Name = "Agent Name")]
        public string user_name { get; set; }


        public string token_no { get; set; }
        public string token_prefix { get; set; }

        [Display(Name = "Token No")]
        public string token_no_formated
        {
            get
            {
                if (token_no == ApplicationSetting.DisplayWhenEmptyToken)
                    return token_no.ToString().PadLeft(ApplicationSetting.PaddingLeft, '0');
                else
                    return token_prefix + token_no.ToString().PadLeft(ApplicationSetting.PaddingLeft, '0');
            }
        }

        [Display(Name = "Mobile No")]
        public string mobile_no { get; set; }

        [Display(Name = "Service Name")]
        public string service_sub_type_name { get; set; }

        [Display(Name = "Issue Time")]
        public string issue_time { get; set; }

        [Display(Name = "Start Time")]
        public string start_time { get; set; }

        [Display(Name = "End Time")]
        public string end_time { get; set; }

        [Display(Name = "Waiting Time")]
        public string wating_time { get; set; }

        [Display(Name = "Std. Time")]
        public string std_time { get; set; }

        [Display(Name = "Actual Time")]
        public string actual_time { get; set; }

        [Display(Name = "Variance")]
        public string variance { get; set; }

        [Display(Name = "Remarks")]
        public string remarks { get; set; }


    }
}