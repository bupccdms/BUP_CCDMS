using qms.SignalRHub;
using qms.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;

namespace qms.Utility
{
    public class SchedulerManager
    {
        public static List<VMPlayListSheduling> PlayListSheduling = new List<VMPlayListSheduling>();
        public static List<VMScrollSheduling> ScrollSheduling = new List<VMScrollSheduling>();
        private static BLL.BLLPlayListSheduling dbManager = new BLL.BLLPlayListSheduling();
        private static BLL.BLLScrollSheduling dbManager1 = new BLL.BLLScrollSheduling();
        public static List<VMPlayListSheduling> LoadAllPlayLisSchedule()
        {
            try
            {
                PlayListSheduling = dbManager.GetAll();
                return PlayListSheduling;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public static List<VMScrollSheduling> LoadAllScrollSchedule()
        {
            try
            {
                ScrollSheduling = dbManager1.GetAll();
                return ScrollSheduling;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public static void StartSchedulerTimer()
        {
            Timer t = new Timer(TimeSpan.FromMinutes(1).TotalMilliseconds);
            t.AutoReset = true;
            t.Elapsed += new System.Timers.ElapsedEventHandler(SchedulerMethod);
            t.Start();
        }
        private static void SchedulerMethod(object sender, ElapsedEventArgs e)
        {
            ChackPlayListSchedul();
            ChackScrollSchedul();
            System.Diagnostics.Debug.WriteLine("I am here. Now at " + DateTime.Now);
        }
        public static void ChackPlayListSchedul()
        {
            LoadAllPlayLisSchedule();
            
            foreach (VMPlayListSheduling item in PlayListSheduling)
            {
                DateTime from = item.when_start;
                DateTime to = item.when_start.AddMinutes(item.duration);
                DateTime today = DateTime.Now;
                 if (today>=from  && today <= to && item.is_active == 1 && item.is_start == 0)
                {
                    //change Status
                    item.is_start = 1;
                    dbManager.Edit(item);
                    NotifyDisplay.SendMessages(0, "", "", false, false, true, false, false, false);
                    System.Diagnostics.Debug.WriteLine("PlayList Schedul Start at "+ DateTime.Now);
                }
                //else if (today < from && item.is_active == 1 && item.is_start == 0 && item.is_end == 0)
                //{

                    //NotifyDisplay.SendMessages(0, "", "", false, false, true, false);

                    //NotifyDisplay.SendMessages(0, "", "", false, false, true, false, false, false);

                //}
                else if (today > to && item.is_active ==1 && item.is_start== 1 && item.is_end ==0)
                {
                    //change Status
                    item.is_end = 1;
                    item.is_start = 0;
                    //item.is_active = 0;
                    dbManager.Edit(item);
                    NotifyDisplay.SendMessages(0, "", "", false, false, true, false, false, false);
                    System.Diagnostics.Debug.WriteLine("PlayList Schedul End at " + DateTime.Now);
                }
            }
        }
        public static void ChackScrollSchedul()
        {
            LoadAllScrollSchedule();
            foreach (VMScrollSheduling item in ScrollSheduling)
            {
                DateTime from = item.when_start;
                DateTime to = item.when_start.AddMinutes(item.duration);
                DateTime today = DateTime.Now;
                if (today >= from && today <= to && item.is_active == 1 && item.is_start == 0)
                {
                    //change Status
                    item.is_start = 1;
                    dbManager1.Edit(item);
                    NotifyDisplay.SendMessages(0, "", "", false, false, false, true, false, false);
                    System.Diagnostics.Debug.WriteLine("Scroll Schedul Start at " + DateTime.Now);
                }
                //else if (today < from && item.is_active == 1 && item.is_start == 0 && item.is_end == 0)
                //{
                    //NotifyDisplay.SendMessages(0, "", "", false, false, true, false);
                    //NotifyDisplay.SendMessages(0, "", "", false, false, true, false, false, false);
                //}
                else if (today > to && item.is_active == 1 && item.is_start == 1 && item.is_end == 0)
                //else
                {
                    //change Status
                    item.is_end = 1;
                    item.is_start = 0;
                    //item.is_active = 0;
                    dbManager1.Edit(item);
                    NotifyDisplay.SendMessages(0, "", "", false, false, false, true, false, false);
                    System.Diagnostics.Debug.WriteLine("Scroll Schedul End at " + DateTime.Now);
                }
            }
           
        }
    }
}