using qms.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace qms.ViewModels
{
    public class VMTokenQueue
    {
        public long token_id { get; set; }
        public int department_id { get; set; }
        public string department_name { get; set; }
        public string token_prefix { get; set; }

        public int token_no { get; set; }

        public string token_no_formated
        {
            get
            {
                return token_prefix + token_no.ToString().PadLeft(ApplicationSetting.PaddingLeft,'0');
            }
        }
        public System.DateTime service_date { get; set; }
        

        public string creation_time
        {
            get
            {
                return service_date.ToString("dd-MMM-yy hh:mm:ss tt");
            }
        }

        public short service_status_id { get; set; }
        public string service_status { get; set; }
        public string device_no { get; set; }
        public string static_ip { get; set; }
        public string display_next { get; set; }


        [Required]
        [Display(Name = "Mobile No")]
        public string contact_no { get; set; }
    }
}