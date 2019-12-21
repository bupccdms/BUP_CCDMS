using qms.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace qms.Models
{
    [MetadataType(typeof(PlayListItemMeta))]
    public partial class tblPlayListItem
    {
        public string file_extenstion
        {
            get
            {
                return file_name.Split('.').LastOrDefault();
            }
        }

        public string getFileType()
        {

            switch (file_extenstion.ToLower())
            {
                case "mpg":
                case "mpeg":
                case "avi":
                case "wmv":
                case "mov":
                case "rm":
                case "ram":
                case "swf":
                case "flv":
                case "ogg":
                case "webm":
                case "mp4":
                    return "VIDEO";
                case "jpeg":
                case "jpg":
                case "png":
                case "gif":
                case "bmp":
                    return "IMAGE";
                case "docx":
                case "doc":
                    return "DOC";
                case "pdf":
                    return "PDF";
                case "xlsx":
                case "xlx":
                case "xlsm":
                case "xlsb":
                    return "EXCEL";
                case "pptx":
                case "ppt":
                case "pptm":
                    return "POWERPOINT";
                case "txt":
                    return "TXT";
                default:
                    return "";
            }
        }

    }

    public class PlayListItemMeta
    {

        [Required]
        public int playlist_id { get; set; }
        [Required]
        [Display(Name = "URL")]
        public string item_url { get; set; }
        [Required]
        [Display(Name ="File Name")]
        public string file_name { get; set; }
        [Display(Name = "File Type")]
        public string file_type { get; set; }
        [Required]
        [Display(Name = "Start Time")]
        public int start_time { get; set; }

        //[Required]
        [Display(Name = "End Time")]
        public int end_time { get; set; }

        public int volume { get; set; }

        [Required]
        [Display(Name = "Duration (Second)")]
        public int duration_in_second { get; set; }

        [Required]
        [Display(Name = "Sort Order")]
        public int sort_order { get; set; }
        public int show_in_mobile { get; set; }
        public int is_in_mute { get; set; }

        [Required]
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


    }
}