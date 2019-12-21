using qms.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace qms.ViewModels
{
    public class VMDeviceScroll
    {
        public int device_scroll_id { get; set; }
        public IList<string> scroll_Id_List { get; set; }
        [Required(ErrorMessage = "Please Select Scroll!")]
        public int scroll_id { get; set; }
        public string content_en { get; set; }
        public string content_bn { get; set; }
        [Required(ErrorMessage = "Please Select Device!")]
        public int device_id { get; set; }
        public string device_no { get; set; }
        public string address { get; set; }
        public string location { get; set; }
        [Required(ErrorMessage = "Please Select Department!")]
        public int department_id { get; set; }
        public string department_name { get; set; }


        public string device_name { get; set; }
        public int is_active { get; set; }
        public int status { get; set; }
        public int is_publish { get; set; }

        public string name
        {
            get
            {
                return device_name + " - " + device_no;
            }
        }
        public virtual List<tblScroll> _tblScroll { get; set; }

    }
}