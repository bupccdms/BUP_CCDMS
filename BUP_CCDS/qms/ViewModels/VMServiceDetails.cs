﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace qms.ViewModels
{
    public class VMServiceDetails
    {
        public long service_id { get; set; }
        public string user_id { get; set; }
        public int device_id { get; set; }
        public string device_no { get; set; }

        public int service_sub_type_id { get; set; }
        public string service_sub_type_name { get; set; }

        public int Total_Service { get; set; }
        public long token_id { get; set; }
        public long customer_id { get; set; }
        public int is_primary_contact { get; set; }
        public string issues { get; set; }
        public string solutions { get; set; }
        public string department_name { get; set; }
        public string Device_Name { get; set; }
        public string address { get; set; }
        public string service_type_name { get; set; }
        public string mobile_no { get; set; }
        public string UserName { get; set; }
        public string contact_no { get; set; }
        public string name { get; set; }

        public string customer_name { get; set; }
        public System.DateTime service_datetime { get; set; }
        public System.DateTime call_time { get; set; }
        public System.DateTime generate_time { get; set; }

        public System.DateTime start_time { get; set; }

        public System.DateTime end_time { get; set; }

        public string service_datetime_string {
            get
            {
                return service_datetime.ToString("dd-MM-yyyy");
            }
        }

        public string start_time_string
        {
            get
            {
                return start_time.ToString("hh:mm:ss tt");
            }
        }
        public string end_time_string
        {
            get
            {
                return end_time.ToString("hh:mm:ss tt");
            }
        }

        

        public string duration
        {
            get
            {
                return end_time.Subtract(start_time).ToString();
            }
        }

        public string waitingtime
        {
            get
            {
                return call_time.Subtract(generate_time).ToString();
            }
        }

    }
}