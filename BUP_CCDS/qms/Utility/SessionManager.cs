using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace qms.Utility
{
    public class SessionManager
    {
        private HttpSessionStateBase _session;
        public SessionManager(HttpSessionStateBase session)
        {
            _session = session;
        }

        public string user_id
        {
            get
            {
                return (_session["user_id"] == null ? "" : _session["user_id"] as string);
            }
            set
            {
                _session["user_id"] = value;
            }
        }

        public string user_name
        {
            get
            {
                return (_session["user_name"] == null ? "" : _session["user_name"] as string);
            }
            set
            {
                _session["user_name"] = value;
            }
        }

        public string LoginProvider
        {
            get
            {
                return (_session["LoginProvider"] == null ? "" : _session["LoginProvider"] as string);
            }
            set
            {
                _session["LoginProvider"] = value;
            }
        }

        public int department_id
        {
            get
            {
                return (_session["department_id"] == null ? 0 : (int)_session["department_id"]);
            }
            set
            {
                _session["department_id"] = value;
            }
        }

        public string department_name
        {
            get
            {
                return (_session["department_name"] == null ? "" : _session["department_name"] as string);
            }
            set
            {
                _session["department_name"] = value;
            }
        }

        public string department_static_ip
        {
            get
            {
                return (_session["department_static_ip"] == null ? "" : _session["department_static_ip"] as string);
            }
            set
            {
                _session["department_static_ip"] = value;
            }
        }

        public int device_id
        {
            get
            {
                return (_session["device_id"] == null ? 0 :(int) _session["device_id"]);
            }
            set
            {
                _session["device_id"] = value;
            }
        }
        public string role_id
        {
            get
            {
                return (_session["Id"] == null ? "" : _session["Id"] as string);
            }
            set
            {
                _session["Id"] = value;
            }
        }
        public string device_no
        {
            get
            {
                return (_session["device_no"] == null ? "" : _session["device_no"] as string);
            }
            set
            {
                _session["device_no"] = value;
            }
        }

        public Exception error
        {
            get
            {
                return (_session["error"] == null ? null : _session["error"] as Exception);
            }
            set
            {
                
                _session["error"] = value;
            }
        }

        public int PasswordExpiryNotifyBeforeDays
        {
            get
            {
                return (_session["PasswordExpiryNotifyBeforeDays"] == null ? 0 : (int)_session["PasswordExpiryNotifyBeforeDays"]);
            }
            set
            {
                _session["PasswordExpiryNotifyBeforeDays"] = value;
            }
        }

        public bool IsPasswordExpired
        {
            get
            {
                return (_session["IsPasswordExpired"] == null ? false : (bool)_session["IsPasswordExpired"]);
            }
            set
            {
                _session["IsPasswordExpired"] = value;
            }
        }

        public bool ForceChangeConfirmed
        {
            get
            {
                return (_session["ForceChangeConfirmed"] == null ? false : (bool)_session["ForceChangeConfirmed"]);
            }
            set
            {
                _session["ForceChangeConfirmed"] = value;
            }
        }

        public bool IsActiveDirectoryUser
        {
            get
            {
                return (_session["IsActiveDirectoryUser"] == null ? false : (bool)_session["IsActiveDirectoryUser"]);
            }
            set
            {
                _session["IsActiveDirectoryUser"] = value;
            }
        }

        public int LOCKOUTENABLED
        {
            get
            {
                return (_session["LOCKOUTENABLED"] == null ? 0 : (int)_session["LOCKOUTENABLED"]);
            }
            set
            {
                _session["LOCKOUTENABLED"] = value;
            }
        }
        public int playList_id
        {
            get
            {
                return (_session["playList_id"] == null ? 0 : (int)_session["playList_id"]);
            }
            set
            {
                _session["playList_id"] = value;
            }
        }


        public void Close()
        {
            _session.Clear();
            _session.Abandon();
            
        }
    }
}