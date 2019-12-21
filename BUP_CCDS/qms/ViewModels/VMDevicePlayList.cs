using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace qms.ViewModels
{
    public class VMDevicePlayList
    {
        public int device_playlist_id { get; set; }
        [Required(ErrorMessage = "Please Select PlayList!")]
        public int playlist_id { get; set;}
        public string playlist_name { get; set; }
        [Required(ErrorMessage = "Please Select Device!")]
        public int device_id { get; set; }
        [Required(ErrorMessage = "Please Select Department!")]
        public int department_id { get; set; }
        //[Required(ErrorMessage = "Please Select Department!")]
        public string department_name { get; set; }
        public string Address { get; set; }
        public string device_name { get; set; }
        public string device_no { get; set; }
        public string location { get; set; }

        public int is_global { get; set; }
        public int is_active { get; set; }

        public string name
        {
            get
            {
                return device_name + " - " + device_no;
            }
        }

    }
}