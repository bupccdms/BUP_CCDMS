﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace qms.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class tblPlayListItem
    {
        

        [Key]
        public int playlistitem_id { get; set; }
        public int playlist_id { get; set; }
        public string playlist_name { get; set; }
        public string item_url { get; set; }
        public string file_type { get; set; }
        public string file_name { get; set; }
        public int duration_in_second { get; set; }
        public int sort_order { get; set; }
        public int start_time { get; set; }
        public int end_time { get; set; }
        //public DateTime? start_time { get; set; }
        //public DateTime? end_time { get; set; }
        public int show_in_mobile { get; set; }
        public int volume { get; set; }
        public int is_in_mute { get; set; }
        //[RegularExpression("([1-9][0-9]*)", ErrorMessage = "Input must be a number")]
        [Range(0, 59, ErrorMessage = "Please use values between 0 to 59")]
        public int endTimeInMunites { get; set; }
        //[RegularExpression("([1-9][0-9]*)", ErrorMessage = "Input must be a number")]
        [Range(0, 59, ErrorMessage = "Please use values between 0 to 59")]
        public int endTimeInSeconds { get; set; }

        public bool bool_show_in_mobile
        {
            get
            {
                return (show_in_mobile == 1 ? true : false);
            }
            set
            {
                show_in_mobile = (value == true ? 1 : 0);
            }
        }
        public bool bool_is_in_mute
        {
            get
            {
                return (is_in_mute == 1 ? true : false);
            }
            set
            {
                is_in_mute = (value == true ? 1 : 0);
            }
        }

        public int duration
        {
            get
            {
                return (end_time - start_time);
                //if (end_time.HasValue && start_time.HasValue)
                //return end_time.Value.Subtract(start_time.Value).ToString();
                //else return "";

            }
        }


        //[RegularExpression("([1-9][0-9]*)", ErrorMessage = "Input must be a number")]
        [Range(0, 59, ErrorMessage = "Please use values between 0 to 59")]
        public int startTimeInMinutes { get; set; }
        //[RegularExpression("([1-9][0-9]*)", ErrorMessage = "Input must be a number")]
        [Range(0, 59, ErrorMessage = "Please use values between 0 to 59")]
        public int startTimeInSeconds { get; set; }

        //[RegularExpression("([1-9][0-9]*)", ErrorMessage = "Input must be a number")]
        [Range(0, 59, ErrorMessage = "Please use values between 0 to 59")]
        public int durationInMunites { get; set; }
        //[RegularExpression("([1-9][0-9]*)", ErrorMessage = "Input must be a number")]
        [Range(0, 59, ErrorMessage = "Please use values between 0 to 59")]
        public int durationInSecond { get; set; }

    }
}
