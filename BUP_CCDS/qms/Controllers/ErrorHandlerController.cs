using qms.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace qms.Controllers
{
    public class ErrorHandlerController : Controller
    {
        // GET: ErrorHandler
        public ActionResult Index(string extra="")
        {
            
            Exception ex = new SessionManager(Session).error;
            if (extra.Length > 0)
                ViewBag.ExtraErrorMessage = Cryptography.Decrypt(extra, true);
            else
                ViewBag.ExtraErrorMessage = "";
            ViewBag.ErrorMessage = extra + "\n\n" + ex.Message;
            ViewBag.InnerErrorMessage =  ex.Data.ToString() + ex.InnerException + ex.TargetSite + ex.StackTrace + ex.Source + ex.HResult;
            return View();
        }
    }
}