using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace qms.ViewModels
{
    public class VMPlayList
    {
        public int playlist_id { get; set; }
        public string playlist_name { get; set; }
        public string item_url { get; set; }
        public string file_type { get; set; }
        public string file_name { get; set; }
        public int start_time { get; set; }
        public int is_active { get; set; }
        public int show_in_mobile { get; set; }
        public int is_in_mute { get; set; }

        public int volume { get; set; }

        public int end_time { get; set; }
        public int duration_in_second { get; set; }
        public int sort_order { get; set; }

    }
}