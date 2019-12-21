
using System.Diagnostics.Contracts;
using System.Net;
using System.Web.Mvc;

namespace qms.Utility
{
    public class AuthorizationFilter : AuthorizeAttribute, IAuthorizationFilter
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            Contract.Assert(filterContext != null);

            if(!(filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Length>0
               || filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Length>0))
            {
                string actionName = filterContext.Controller.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = filterContext.Controller.ControllerContext.RouteData.Values["controller"].ToString();
                if (actionName == "ChangePassword" && controllerName == "Manage")
                {
                    base.OnAuthorization(filterContext);

                }
                else if ((actionName == "LogOff" && controllerName == "Account"))
                {
                    base.OnAuthorization(filterContext);
                }
                else if ((actionName == "SessionOut" && controllerName == "Account"))
                {
                    base.OnAuthorization(filterContext);
                }
                else
                {
                    if (filterContext.HttpContext.Session["user_id"] != null)
                    {
                        if (filterContext.HttpContext.Session["IsPasswordExpired"] != null)
                        {
                            if ((bool)filterContext.HttpContext.Session["IsPasswordExpired"])
                            {
                                filterContext.Result = new RedirectResult("~/Manage/ChangePassword");
                            }
                            else if (filterContext.HttpContext.Session["ForceChangeConfirmed"] != null)
                            {
                                if ((bool)filterContext.HttpContext.Session["ForceChangeConfirmed"]==false)
                                {
                                    filterContext.Result = new RedirectResult("~/Manage/ChangePassword");
                                }
                                else
                                {
                                    base.OnAuthorization(filterContext);
                                }
                            }
                            else
                            {
                                base.OnAuthorization(filterContext);
                            }
                        }
                        else
                            base.OnAuthorization(filterContext);
                    }
                    else if (filterContext.HttpContext.Request.IsAuthenticated)
                    {
                        filterContext.Result = new RedirectResult("~/Account/SessionOut");
                    }
                    else
                        base.OnAuthorization(filterContext);
                }
            }

            
                
        }
    }
}