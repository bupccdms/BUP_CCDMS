using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace qms.ViewModels
{
    public class VMDashboardAdmin
    {
        public int department_playlist_id { get; set; }
        public int department_id { get; set; }
        public string department_name { get; set; }
        public int playlist_id { get; set; }
        public string playlist_name { get; set; }
        public string playlistitem_id { get; set; }
        public string file_name { get; set; }
        public string file_type { get; set; }
        public int device_id { get; set; }
        public string device_no { get; set; }
        public string location { get; set; }
        public string device_name { get; set; }
        public string content_bn { get; set; }



        public int tokens { get; set; }
        public int services { get; set; }
        
    }

    public class VMDashboardDepartmentAdminDevices
    {
        public string device_no { get; set; }
        public int tokens { get; set; }
    }

    public class VMDashboardDepartmentAdminServicesTokens
    {
        public int service_id { get; set; }
        public string service_name { get; set; }
        public int tokens { get; set; }
    }

    public class VMDashboardDepartmentAdminServicesWaitings
    {
        public int service_id { get; set; }
        public string service_name { get; set; }
        public int tokens { get; set; }
    }
}