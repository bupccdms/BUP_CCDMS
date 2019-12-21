using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace qms.Utility
{
    public class ApplicationSetting
    {
        public static string DisplayPath
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("DisplayPath");
            }
        }

        public static int PaddingLeft
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings.Get("PaddingLeft"));
            }
        }

        public static string dispalyFooterAdd
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("dispalyFooterAdd");
            }
        }

        public static string dispalyWelcome
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("dispalyWelcome");
            }
        }

        public static string dispalyVideo
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("dispalyVideo");
            }
        }
        public static string defaultTokenPrefix
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("defaultTokenPrefix");
            }
        }
        
        public static string DeviceText
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("DeviceText");
            }
        }

        public static string TokenText
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("TokenText");
            }
        }

        public static string voiceText
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("voiceText");
            }
        }

        public static string speakLanguage
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("speakLanguage");
            }
        }

        public static string speakGender
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("speakGender");
            }
        }

        public static string speakRate
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("speakRate");
            }
        }

        public static string DisplayWhenEmptyToken
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("DisplayWhenEmptyToken");
            }
        }

        public static string galleryDefaultPath
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("galleryDefaultPath");
            }
        }

        public static int passwordRequiredLength
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings.Get("passwordRequiredLength"));
            }
        }
        public static int MaxFailedAccessAttemptsBeforeLockout
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings.Get("MaxFailedAccessAttemptsBeforeLockout"));
            }
        }

        public static int PasswordExpiredAfter
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings.Get("PasswordExpiredAfter"));
            }
        }

        public static int PasswordExpiryNotifyBefore
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings.Get("PasswordExpiryNotifyBefore"));
            }
        }
        public static int PasswordLastCheckingCount
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings.Get("PasswordLastCheckingCount"));
            }
        }

        public static bool AllowActiveDirectoryUser
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings.Get("AllowActiveDirectoryUser"));
            }
        }

        public static bool UserEmailRequired
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings.Get("UserEmailRequired"));
            }
        }

        public static string ActiveDirectoryInfo
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("ActiveDirectoryInfo");
            }
        }

        public static string ReportCSVSeparator
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("ReportCSVSeparator");
            }
        }

        public static bool AllowAPILoggin
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings.Get("AllowAPILoggin"));
            }
        }

        public static bool AllowSignalRLoggin
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings.Get("AllowSignalRLoggin"));
            }
        }


        public static string msgText
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("msgText");
            }
        }

        public static string msgTextBn
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("msgTextBn");
            }
        }

        public static bool isMsgBn
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings.Get("isMsgBn"));
            }
        }
        public static int maxMediaFileSize
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings.Get("MaxMediaFileSize"));
            }
        }
    }
}