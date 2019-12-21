using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace qms.ViewModels
{
    public class VMCustomContent
    {
        public int custom_content_id { get; set; }
        public int is_url { get; set; }
        //[Display(Name = "Content")]
        //[Required]
        public string content { get; set; }
        public string url { get; set; }
        public string file_name { get; set; }

    }
}