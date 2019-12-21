using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace qms.Models
{
    public class SignalRBroadcastLog
    {
        [Key]
        public long broadcast_log_id { get; set; }

        public int department_id { get; set; }

        public string device_no { get; set; }

        public string token_no { get; set; }

        public int is_token_added { get; set; }

        public int is_token_called { get; set; }

        public int is_playlist_changed { get; set; }

        public int is_footer_changed { get; set; }

        public DateTime broadcast_time { get; set; }

        public int is_password_change { get; set; }

        public int is_deactive { get; set; }

        
        


    }
}