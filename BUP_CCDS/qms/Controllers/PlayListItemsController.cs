using qms.BLL;
using qms.Models;
using qms.SignalRHub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using qms.Utility;
using qms.ViewModels;

namespace qms.Controllers
{
    [AuthorizationFilter(Roles = "Admin, Department Admin")]
    public class PlayListItemsController : Controller
    {
        private BLLPlayListItem dbManager = new BLLPlayListItem();
        // GET: Scrolls
        public ActionResult Index(int playlist_id)
        {
            ViewBag.playlist_id = playlist_id;
            tblPlayList playList = new BLLPlayList().GetById(playlist_id);
            if (playList != null)
            {
                ViewBag.playlist_name = playList.playlist_name;
            }
            else ViewBag.playlist_name = "Not Found";

            var playListItem =  new BLLPlayListItem().GetAll(playlist_id).Where(x=>x.item_url =="/");
            //ViewBag.customContentList = new BLLCustomContent().GetAll().Where(x=>x.is_url > 0);
            foreach (var item in playListItem)
            {
                string file_name = item.file_name;
                int custom_content_id = Convert.ToInt32(file_name);
                VMCustomContent customContent = new BLLCustomContent().GetById(custom_content_id);
                //List<VMCustomContent> customContent1 = new BLLCustomContent().GetAll().ToList();
                ViewBag.customContentList = new BLLCustomContent().GetAll();
                if (customContent != null)
                {
                    if (customContent.is_url > 0)
                    {
                        string content = new BLLCustomContent().GetById(custom_content_id).url;
                        ViewBag.content = content;
                    }
                    //string content = new BLLCustomContent().GetById(custom_content_id).content;
                }
            }


            return View(dbManager.GetAll(playlist_id));
        }
        public ActionResult List(int playlist_id)
        {
            ViewBag.playlist_id = playlist_id;
            tblPlayList playList = new BLLPlayList().GetById(playlist_id);
            if (playList != null)
            {
                ViewBag.playlist_name = playList.playlist_name;
            }
            else ViewBag.playlist_name = "Not Found";

            var playListItem = new BLLPlayListItem().GetAll(playlist_id).Where(x => x.item_url == "/");
            foreach (var item in playListItem)
            {
                string file_name = item.file_name;
                int custom_content_id = Convert.ToInt32(file_name);
                VMCustomContent customContent = new BLLCustomContent().GetById(custom_content_id);
                if (customContent != null)
                {
                    if (customContent.is_url > 0)
                    {
                        string content = new BLLCustomContent().GetById(custom_content_id).url;
                        ViewBag.content = content;
                    }
                    //string content = new BLLCustomContent().GetById(custom_content_id).content;
                }
            }


            return PartialView(dbManager.GetAll(playlist_id));
        }

        // GET: Departments/Create
        public ActionResult Create(int playlist_id=0)
        {
            if (playlist_id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPlayList playList = new BLLPlayList().GetById(playlist_id);
            if (playList == null)
            {
                return HttpNotFound();
            }
            tblPlayListItem playListItem = new tblPlayListItem()
            {
                playlist_id = playList.playlist_id,
                playlist_name = playList.playlist_name,
                item_url = ApplicationSetting.galleryDefaultPath
            };

            //List<tblPlayListItem> list = new BLLPlayListItem().GetAll(playlist_id).ToList();

            List<tblPlayListItem> list = new BLLPlayListItem().GetAll(playlist_id).ToList();
            if (list.Count>0)
            {
                int sortorder = new BLLPlayListItem().GetAll(playlist_id).Max(k => k.sort_order);
                ViewBag.SortOrder = sortorder + 1;
            }
            else
            {
                ViewBag.SortOrder = 1;
            }
            return View(playListItem);
        }

        // POST: Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "playlistitem_id,playlist_id, item_url, file_type, file_name, duration_in_second, sort_order,start_time,bool_show_in_mobile,end_time,bool_is_in_mute, volume")] tblPlayListItem playListItem)
        {
            //if (ModelState.IsValid)
            //{
                if (playListItem.file_type == "gallery")
                {
                    playListItem.file_type = playListItem.getFileType();
                if (playListItem.volume == 0)
                {
                    playListItem.is_in_mute = 1;
                }
                }
                else
                {
                    int custom_content_id = Convert.ToInt32(playListItem.file_name);
                    int is_url = new BLLCustomContent().GetById(custom_content_id).is_url;
                    playListItem.file_type = (is_url == 1 ? "URL" : "TEXT");
                    playListItem.item_url = "/";
                    //if (playListItem.file_name != playListItem.file_extenstion)
                    //{
                    //    int IsUrl = new BLLCustomContent().GetAll().Where(x => x.content == playListItem.file_name).FirstOrDefault().is_url;
                    //    if (IsUrl > 0)
                    //    {
                    //        playListItem.file_type = "URL";
                    //    }                        
                    //}
                    //else
                    //{
                    //    int IsUrl = new BLLCustomContent().GetAll().Where(x => x.custom_content_id == Convert.ToInt32(playListItem.file_name)).FirstOrDefault().is_url;
                    //    if (IsUrl == 0)
                    //    {
                    //        playListItem.file_type = "TEXT";
                    //    }
                    //}
                }
                dbManager.Create(playListItem);
                TempData["message"] = true;

                //NotifyDisplay.SendMessages(0, "", "", false, false, true, false);
                return RedirectToAction("Index", new { playListItem.playlist_id });
            //}

            //return View(playListItem);
        }
       

        // GET: Departments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPlayListItem playListItem = dbManager.GetById(id.Value);
            var e_Minutes = (playListItem.end_time / 60);
            playListItem.endTimeInMunites = Convert.ToInt32(e_Minutes);
            var e_Seconds = Math.Floor(Convert.ToDecimal(playListItem.end_time) % 60);
            playListItem.endTimeInSeconds = Convert.ToInt32(e_Seconds);

            var s_Minutes = (playListItem.start_time / 60);
            playListItem.startTimeInMinutes = Convert.ToInt32(s_Minutes);
            var s_Seconds = Math.Floor(Convert.ToDecimal(playListItem.start_time) % 60);
            playListItem.startTimeInSeconds = Convert.ToInt32(s_Seconds);

            var d_Minutes = (playListItem.duration_in_second / 60);
            playListItem.durationInMunites = Convert.ToInt32(d_Minutes);
            var d_Seconds = Math.Floor(Convert.ToDecimal(playListItem.duration_in_second) % 60);
            playListItem.durationInSecond = Convert.ToInt32(d_Seconds);
            ViewBag.Volume = playListItem.volume;

            if (playListItem == null)
            {
                return HttpNotFound();
            }
            return View(playListItem);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblPlayListItem playListItem)
        {
            if (ModelState.IsValid)
            {
                if (playListItem.file_type == "VIDEO" || playListItem.file_type == "IMAGE" || playListItem.file_type == "DOC" || playListItem.file_type == "PDF")
                {
                    playListItem.file_type = playListItem.getFileType();
                }
                else
                {
                    if (playListItem.file_type == "gallery")
                    {
                        playListItem.file_type = playListItem.getFileType();
                    }
                    else
                    {
                        if (playListItem.file_name != playListItem.file_extenstion)
                        {
                            int IsUrl = new BLLCustomContent().GetAll().Where(x => x.content == playListItem.file_name).FirstOrDefault().is_url;
                            if (IsUrl > 0)
                            {
                                playListItem.file_type = "URL";
                            }
                        }
                        else
                        {
                            int IsUrl = new BLLCustomContent().GetAll().Where(x => x.custom_content_id == Convert.ToInt32(playListItem.file_name)).FirstOrDefault().is_url;
                            if (IsUrl == 0)
                            {
                                playListItem.file_type = "TEXT";
                            }
                        }

                        //int custom_content_id = Convert.ToInt32(playListItem.file_name);
                        //int is_url = new BLLCustomContent().GetById(custom_content_id).is_url;
                        //playListItem.file_type = (is_url == 1 ? "URL" : "TEXT");
                        playListItem.item_url = "/";
                    }
                }
                dbManager.Edit(playListItem);
                TempData["mgs"] = true;

                return RedirectToAction("Index", new { playListItem.playlist_id });
            }
            return View(playListItem);
        }

        // GET: Departments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPlayListItem playListItem = dbManager.GetById(id.Value);
            if (playListItem == null)
            {
                return HttpNotFound();
            }
            return View(playListItem);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPlayListItem playListItem = dbManager.GetById(id);
            if (playListItem == null)
            {
                return HttpNotFound();
            }
            dbManager.Remove(id);
            NotifyDisplay.SendMessages(0, "", "", false, false, true, false, false, false);

            return RedirectToAction("Index", new { playListItem.playlist_id });
        }

        
    }
}