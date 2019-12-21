using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace qms.ViewModels
{
    public class VMDashboardDeviceToken
    {
        public string service_name { get; set; }
        public int served { get; set; }
        public int serving { get; set; }
        public int missing { get; set; }
        public int waiting { get; set; }
        public int total
        {
            get
            {
                return served + serving + missing + waiting;
            }
        }

    }
}