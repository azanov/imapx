using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ImapX.WebSample.Attributes;
using ImapX.WebSample.Models;

namespace ImapX.WebSample.Controllers
{
    [AsyncTimeout(20000)]
    [HandleErrorCode(ExceptionType = typeof(TimeoutException), StatusCode = HttpStatusCode.RequestTimeout)]
    public class ImapXControllerBase : Controller
    {

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            try
            {
                filterContext.Controller.ViewBag.IsAuthenticated = SessionModel.IsAuthenticated;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            base.OnActionExecuted(filterContext);
        }

        public HttpStatusCodeResult HttpOk()
        {
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public HttpStatusCodeResult HttpUnauthorized()
        {
            return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
        }

        public HttpStatusCodeResult HttpBadRequest()
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

    }
}
