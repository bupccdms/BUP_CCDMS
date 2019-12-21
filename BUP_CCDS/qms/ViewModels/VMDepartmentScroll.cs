using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace qms.ViewModels
{
    public class VMDepartmentScroll
    {
        public int department_scroll_id { get; set; }
        public int scroll_id { get; set; }
        public string content_en { get; set; }
        public string content_bn { get; set; }
        public int department_id { get; set; }
        public string department_name { get; set; }

        public int is_active { get; set; }

        public int is_publish { get; set; }

    }
}