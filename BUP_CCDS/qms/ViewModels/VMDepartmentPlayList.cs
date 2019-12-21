using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace qms.ViewModels
{
    public class VMDepartmentPlayList
    {
        public int department_playlist_id { get; set; }


        [Required(ErrorMessage = "Please Select PlayList!")]
        public int playlist_id { get; set; }
        public string playlist_name { get; set; }


        [Required(ErrorMessage = "Please Select Department!")]
        public int department_id { get; set; }
        public string department_name { get; set; }
        public int is_publish { get; set; }
        public string address { get; set; }

        public int is_priority { get; set; }

        [Required]
        public bool bool_is_priority
        {
            get
            {
                return (is_priority == 1 ? true : false);
            }
            set
            {
                is_priority = (value == true ? 1 : 0);
            }
        }

    }
}