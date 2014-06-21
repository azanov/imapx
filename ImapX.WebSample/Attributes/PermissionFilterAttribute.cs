using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using ImapX.WebSample.Enums;
using ImapX.WebSample.Models;

namespace ImapX.WebSample.Attributes
{
    // ReSharper disable PossibleNullReferenceException
    public class PermissionFilterAttribute : ActionFilterAttribute
    {

        public ActionAccessType AccessType { get; set; }
        public string TargetController { get; set; }
        public string TargetAction { get; set; }
        public bool AddReturnUrl { get; set; }

        public PermissionFilterAttribute(ActionAccessType accessType = ActionAccessType.Public, string targetController = null, string targetAction = null, bool addReturnUrl = true)
        {
            AccessType = accessType;
            TargetController = targetController;
            TargetAction = targetAction;
            AddReturnUrl = addReturnUrl;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (AccessType.HasFlag(ActionAccessType.Public))
                return;

            if (AccessType.HasFlag(ActionAccessType.NotAuthenticatedOnly) && !SessionModel.IsAuthenticated)
                return;

            if (AccessType.HasFlag(ActionAccessType.Authenticated) && SessionModel.IsAuthenticated)
                return;

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.ContentType = "application/json";
                var javaScriptSerializer = new JavaScriptSerializer();
                filterContext.HttpContext.Response.Write(
                    javaScriptSerializer.Serialize(
                        new
                        {
                            redirect =
                                UrlHelper.GenerateUrl("Default", TargetAction ?? "signin", TargetController ?? "auth", AddReturnUrl ?
                                    new RouteValueDictionary(

                                        new { returnUrl = filterContext.HttpContext.Request.Url.PathAndQuery }) : new RouteValueDictionary(),
                                    RouteTable.Routes, filterContext.RequestContext, false).ToLower()
                        }));
            }
            else
            {
                if (AddReturnUrl)
                {
                    filterContext.HttpContext.Response.RedirectToRoute(new
                    {
                        controller = TargetController ?? "auth",
                        action = TargetAction ?? "signin",
                        returnUrl = filterContext.HttpContext.Request.Url.PathAndQuery
                    });
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        controller = TargetController ?? "auth",
                        action = TargetAction ?? "signin",
                        returnUrl = filterContext.HttpContext.Request.Url.PathAndQuery
                    }));
                }
                else
                {
                    filterContext.HttpContext.Response.RedirectToRoute(new
                    {
                        controller = TargetController ?? "auth",
                        action = TargetAction ?? "signin"
                    });
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        controller = TargetController ?? "auth",
                        action = TargetAction ?? "signin"
                    }));
                }
            }


            filterContext.HttpContext.Response.End();
        }
    }
}