using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using qms.Models;
using System.Threading.Tasks;
using qms.Utility;

namespace qms.SignalRHub
{
    [HubName("notifyDisplay")]
    public class NotifyDisplay : Hub
    {


        public override Task OnConnected()
        {
            return base.OnConnected();

        }

        [HubMethodName("callToken")]
        public static void CallToken(int device_id)
        {

            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotifyDisplay>();
            if (device_id > 0)
            {
                context.Clients.All.callToken(device_id);                
            }
            
        }

        [HubMethodName("deviceStatusChanged")]
        public static void DeviceStatusChanged(int department_id)
        {

            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotifyDisplay>();
            if (department_id > 0)
            {
                context.Clients.All.DeviceStatusChanged(department_id);
            }

        }

        [HubMethodName("sendMessages")]
        public static void SendMessages(int department_id, string device_no, string token_no, bool tokenAdded, bool tokenCalled, bool playListChanged, bool scrollChanged, bool isPasswordChange, bool isDeactive)
        {
            
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotifyDisplay>();
            if (token_no == "")
            {
                context.Clients.All.updateMessages("", department_id, device_no, token_no, tokenAdded, tokenCalled, playListChanged, scrollChanged, isPasswordChange, isDeactive);

                
            }
            else
            {
               
                string text = string.Format(ApplicationSetting.voiceText, token_no, device_no);

                context.Clients.All.updateMessages(text, department_id, device_no, token_no, tokenAdded, tokenCalled, playListChanged, scrollChanged, isPasswordChange, isDeactive);
                

            }
            
            Loggin(department_id, device_no, token_no, tokenAdded, tokenCalled, playListChanged, scrollChanged, isPasswordChange, isDeactive);
            
        }

        private static void Loggin(int department_id, string device_no, string token_no, bool tokenAdded, bool tokenCalled, bool playListChanged, bool scrollChanged, bool isPasswordChange, bool isDeactive)
        {
            if (ApplicationSetting.AllowSignalRLoggin)
            {
                SignalRBroadcastLog log = new SignalRBroadcastLog()
                {
                    department_id = department_id,
                    device_no = device_no,
                    token_no = token_no,
                    is_token_added = (tokenAdded ? 1 : 0),
                    is_token_called = (tokenCalled ? 1 : 0),
                    is_playlist_changed = (playListChanged ? 1 : 0),
                    is_footer_changed = (scrollChanged ? 1 : 0),
                    is_password_change = (isPasswordChange ? 1 : 0),
                    is_deactive = (isDeactive ? 1 : 0)
                };
                new BLL.BLLLog().SignalRBroadCastLogCreate(log);
            }
        }

        

    }

    public class BroadcustMessage
    {
        public int department_id;
        public String text;
    }

}