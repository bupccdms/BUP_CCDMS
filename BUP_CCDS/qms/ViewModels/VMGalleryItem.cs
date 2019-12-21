using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace qms.ViewModels
{
    public class VMGalleryItem
    {
        
        public string file_directory { get; set; }
        [Required(ErrorMessage = "Enter File Name!")]
        public string file_name { get; set; }
        public int vedio_duration { get; set; }

        public HttpPostedFileBase file_data { get; set; }

        public string file_full_path
        {
            get
            {
                if (file_type == "VIDEO")
                    return file_directory.Replace("~", "") + (file_directory.Last() == '/' ? "" : "/") + file_name;
                else
                    return file_directory.Replace("~", "") + (file_directory.Last() == '/' ? "" : "/") + file_name;
            }
        }

        public string file_extenstion
        {
            get
            {
                return file_name.Split('.').LastOrDefault();
            }
        }

        public string file_type
        {
            get
            {
                switch (file_extenstion.ToLower())
                {
                    case "mpg":
                    case "mpeg":
                    case "avi":
                    case "wmv":
                    case "x-ms-wmv":
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
                        return "Docx";
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
                    case "pdf":
                        return "PDF";
                        
                    default:
                        return "";
                }
            }
        }

    }
}