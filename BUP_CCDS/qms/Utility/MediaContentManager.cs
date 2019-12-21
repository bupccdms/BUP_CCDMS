using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace qms.Utility
{
    public class MediaContentManager
    {
        public static string FileType(string fileExtension)
        {
            switch (fileExtension.ToLower())
            {
                case ".mpg":
                case ".mpeg":
                case ".avi":
                case ".wmv":
                case ".mov":
                case ".rm":
                case ".ram":
                case ".swf":
                case ".flv":
                case ".ogg":
                case ".webm":
                case ".mp4":
                    return "VIDEO";
                case ".jpeg":
                case ".jpg":
                case ".png":
                case ".gif":
                case ".bmp":
                    return "IMAGE";
                case ".docx":
                case ".doc":
                    return "DOC";
                case ".xlsx":
                case ".xlx":
                case ".xlsm":
                case ".xlsb":
                    return "EXCEL";
                case ".pptx":
                case ".ppt":
                case ".pptm":
                    return "POWERPOINT";
                case ".txt":
                    return "TXT";
                case ".pdf":
                    return "PDF";
                default:
                    return "";
            }

        }
    }
}