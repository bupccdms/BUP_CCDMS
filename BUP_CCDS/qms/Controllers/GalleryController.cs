using qms.Utility;
using qms.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using qms.BLL;
using qms.Models;

namespace qms.Controllers
{
    [AuthorizationFilter(Roles = "Admin,Department Admin")]
    public class GalleryController : Controller
    {
        // GET: Gallery
        public ActionResult Index(string directory="")
        {
            try
            {
                
                if (directory.Length == 0) directory = ApplicationSetting.galleryDefaultPath;
                //if (directory.Length == 0) directory = ((Request.ApplicationPath == @"/" ? "" : Request.ApplicationPath) + ApplicationSetting.galleryDefaultPath);
                ViewBag.directory = directory;

                string[] patterns = new[] { "*.jpg", "*.jpeg", "*.gif", "*.png", "*.bmp",
                "*.mpg", "*.mpeg", "*.avi", "*.wmv", "*.mov", "*.rm", "*.ram", "*.swf", "*.flv", "*.ogg", "*.webm", "*.mp4","*.docx","*.pdf", "*.xlsx", "*.lsx" , "*.xlxs" , "*.xlsm" , "*.xlsb" , "*.pptx" , "*.ppt" , "*.pptm" , "*.txt" };

                string[] files = patterns.SelectMany(pattern => Directory.GetFiles(Server.MapPath(((Request.ApplicationPath == @"/" ? "" : Request.ApplicationPath) + ApplicationSetting.galleryDefaultPath)), pattern, SearchOption.TopDirectoryOnly)).Distinct().ToArray();
                List<VMGalleryItem> gallery = new List<VMGalleryItem>();
                if (files.Count() > 0)
                {
                    foreach (string file in files)
                    {
                        VMGalleryItem item = new VMGalleryItem() { file_directory = directory, file_name = Path.GetFileName(file) };
                        gallery.Add(item);
                    }
                }
                return View(gallery);
            }
            catch (Exception ex)
            {
                new SessionManager(Session).error = ex;
                return RedirectToAction("Index", "ErrorHandler");
                //throw ex;
            }
            
        }

        public ActionResult List(string directory = "")
        {
            try
            {
                //if (directory.Length == 0) directory = ((Request.ApplicationPath == @"/" ? "" : Request.ApplicationPath) + ApplicationSetting.galleryDefaultPath);
                if (directory.Length == 0) directory = ApplicationSetting.galleryDefaultPath;
                ViewBag.directory = directory;

                string[] patterns = new[] { "*.jpg", "*.jpeg", "*.gif", "*.png", "*.bmp",
                "*.mpg", "*.mpeg", "*.avi", "*.wmv", "*.mov", "*.rm", "*.ram", "*.swf", "*.flv", "*.ogg", "*.webm", "*.mp4","*.docx","*.pdf", "*.xlsx", "*.lsx" , "*.xlxs" , "*.xlsm" , "*.xlsb" , "*.pptx" , "*.ppt" , "*.pptm" , "*.txt" };

                string[] files = patterns.SelectMany(pattern => Directory.GetFiles(Server.MapPath(((Request.ApplicationPath == @"/" ? "" : Request.ApplicationPath) + ApplicationSetting.galleryDefaultPath)), pattern, SearchOption.TopDirectoryOnly)).Distinct().ToArray();
                //string[] files = patterns.SelectMany(pattern => Directory.GetFiles(Server.MapPath(directory), pattern, SearchOption.TopDirectoryOnly)).Distinct().ToArray();
                List<VMGalleryItem> gallery = new List<VMGalleryItem>();
                if (files.Count() > 0)
                {
                    foreach (string file in files)
                    {
                        VMGalleryItem item = new VMGalleryItem() { file_directory = directory, file_name = Path.GetFileName(file) };
                        gallery.Add(item);
                    }
                }
                return PartialView(gallery);
            }
            catch (Exception ex)
            {
                new SessionManager(Session).error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
            
        }

        public ActionResult Create(string directory = "")
        {
            try
            {
                if (directory.Length == 0) directory = ((Request.ApplicationPath == @"/" ? "" : Request.ApplicationPath) + ApplicationSetting.galleryDefaultPath);
                //if (directory.Length == 0) directory = ApplicationSetting.galleryDefaultPath;
                VMGalleryItem vMGalleryItem = new VMGalleryItem() { file_directory = directory };
                return View(vMGalleryItem);
            }
            catch (Exception ex)
            {
                new SessionManager(Session).error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }

        }

        [HttpPost]
        public ActionResult Create(VMGalleryItem galleryItem)
        {
            try
            {
                if(!Directory.Exists(Server.MapPath(galleryItem.file_directory)))
                    return Json(new { success = "false", message = "Invalid Gallery URL" }, JsonRequestBehavior.AllowGet);

                if (System.IO.File.Exists(Server.MapPath(galleryItem.file_full_path)))
                    return Json(new { success = "false", message = "One file already exists with this file name" }, JsonRequestBehavior.AllowGet);

                

                HttpPostedFileBase file = galleryItem.file_data;

                if (file.ContentLength>ApplicationSetting.maxMediaFileSize)
                    return Json(new { success = "false", message = "File size cross the maximum file size length." }, JsonRequestBehavior.AllowGet);
                if (file != null && file.ContentLength > 0)
                {
                    // extract only the filename
                    var fileExtension = Path.GetExtension(file.FileName);


                    if (MediaContentManager.FileType(fileExtension) == "")
                        return Json(new { success = "false", message = "Only Video, Image, Word, Excel, PowerPoint, PDF & TXT file support" }, JsonRequestBehavior.AllowGet);

                    // store the file inside ~/App_Data/uploads folder

                    string directory = ((Request.ApplicationPath == @"/" ? "" : Request.ApplicationPath) + ApplicationSetting.galleryDefaultPath);
                    if (fileExtension == ".mp4" || fileExtension == ".MP4" || fileExtension == ".wmv")
                    {
                        file.SaveAs(Server.MapPath(Path.Combine(directory, galleryItem.file_name + "_" + galleryItem.vedio_duration + "s" + fileExtension)));
                    }
                    else
                    {
                        file.SaveAs(Server.MapPath(Path.Combine(directory, galleryItem.file_name + fileExtension)));
                    }
                    //file.SaveAs(Server.MapPath(Path.Combine(directory, galleryItem.file_name + "_" + galleryItem.vedio_duration + "s" + fileExtension)));
                    TempData["message"] = true;
                    return RedirectToAction("Index", new { directory = ApplicationSetting.galleryDefaultPath});
                }
                else
                {
                    return View(galleryItem);
                }
                
            }
            catch (Exception ex)
            {
                new SessionManager(Session).error = ex;
                return RedirectToAction("Index", "ErrorHandler");
            }
            
            

           
        }

        [HttpPost]
        public ActionResult Delete(string file_full_path)
        {
            try
            {
                string filePath = ((Request.ApplicationPath == @"/" ? "" : Request.ApplicationPath) + file_full_path);
                if (System.IO.File.Exists(Server.MapPath(filePath)))
                {
                    
                    List<tblPlayListItem> usedList = new BLLPlayListItem().GetByFileName(Path.GetFileName(file_full_path));
                    if (usedList.Count == 1)
                    {
                        return Json(new { success = false, message = "File is used in " + usedList[0].playlist_name + " playlist" }, JsonRequestBehavior.AllowGet);
                    }
                    else if (usedList.Count > 1)
                    {
                        return Json(new { success = false, message = "File is used in multiple playlist" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        System.IO.File.Delete(Server.MapPath(filePath));
                        return Json(new { success = true, message = "Successfully deleted." }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                    return Json(new { success = false, message = "File not found" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            
        }
    }
}