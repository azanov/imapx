using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImapX.WebSample.Attributes;
using ImapX.WebSample.Enums;
using ImapX.WebSample.Models;
using PagedList;

namespace ImapX.WebSample.Controllers
{
    public class HomeController : ImapXControllerBase
    {
        [PermissionFilter(ActionAccessType.Authenticated)]
        public ActionResult Index(int page = 1, string folder = null)
        {
            var fld = string.IsNullOrWhiteSpace(folder)
                ? SessionModel.Client.Folders.Inbox
                : SessionModel.Client.Folders.Find(folder);

            if (fld == null)
                return HttpNotFound();

            ViewBag.Folders = SessionModel.Client.Folders;
            ViewBag.CurrentFolder = fld;
            ViewBag.Messages = fld.Search().ToPagedList(page <= 0 ? 1 : page, 10);
            ViewBag.IsInbox = fld == SessionModel.Client.Folders.Inbox;
            return View();
        }

        [ValidateInput(false)]
        public ActionResult Message(string folder, string id)
        {

            var fld = string.IsNullOrWhiteSpace(folder)
                ? null
                : SessionModel.Client.Folders.Find(folder);

            if (fld == null)
                return HttpNotFound();

            var msg = fld.Messages.FirstOrDefault(_ => _.MessageId.Trim('<','>') == id);

            if (msg == null)
                return HttpNotFound();

            ViewBag.Body = msg.Body.Html;
            return View();
        }
    }
}
