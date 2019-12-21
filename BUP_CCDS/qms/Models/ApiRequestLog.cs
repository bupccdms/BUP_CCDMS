using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace qms.Models
{
    public class ApiRequestLog
    {
        [Key]
        public long request_log_id { get; set; }

        [StringLength(128)]
        public string loginprovider { get; set; }

        public string methode_name { get; set; }

        public string request_json { get; set; }

        public string response_json { get; set; }

        public DateTime request_time { get; set; }
        
    }
}