using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Web;
using System.Web.Mvc;
using ImapX.Constants;
using ImapX.Enums;
using ImapX.WebSample.Enums;
using ImapX.WebSample.Attributes;
using ImapX.WebSample.Models;

namespace ImapX.WebSample.Controllers
{
    public class AuthController : ImapXControllerBase
    {
        
        [HttpGet]
        [PermissionFilter(ActionAccessType.NotAuthenticatedOnly, "home", "index", false)]
        public ActionResult SignIn()
        {
            return View();
        }

        [HttpGet]
        [PermissionFilter(ActionAccessType.Authenticated, "auth", "signin", false)]
        public ActionResult SignOut()
        {
            Session.Abandon();
            return RedirectToAction("signin");
        }


        [HttpPost]
        [PermissionFilter(ActionAccessType.NotAuthenticatedOnly, "home", "index", false)]
        public ActionResult SignIn(string server, int port, SslProtocols ssl, string username, string password)
        {
            if (string.IsNullOrWhiteSpace(server) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return HttpBadRequest();

            SessionModel.Client = new ImapClient(server, port, ssl);
            SessionModel.Client.Behavior.MessageFetchMode = MessageFetchMode.Headers | MessageFetchMode.Flags | MessageFetchMode.BodyStructure;
            SessionModel.Client.Behavior.RequestedHeaders =
                MessageHeaderSets.Minimal.Concat(new[] {MessageHeader.MessageId}).ToArray();


            if (SessionModel.Client.Connect())
            {
                if (SessionModel.Client.Login(username, password))
                {
                    return Json(new
                    {
                        result = true,
                        redirect = Url.Action("index", "home")
                    });
                }
                else
                {
                    return Json(new
                    {
                        result = false,
                        error = "Failed to connect"
                    });
                }
            }
            return Json(new
            {
                result = false,
                error = "Failed to connect"
            });
        }

    }
}
