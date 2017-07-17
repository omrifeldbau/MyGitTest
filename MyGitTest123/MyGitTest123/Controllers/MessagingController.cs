using MyGitTest123.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.IO;

namespace MyGitTest123.Controllers
{
    [Authorize]
    public class MessagingController : Controller
    {
    
        public ActionResult Index()
        {

            string user = User.Identity.GetUserName();
            using (var db = new MessagingContext())
            {
                var query = from m in db.Messages
                            orderby m.Time
                            where m.ToUserID == user && !m.Seen
                            select new MessageViewModel
                            {
                                fromUser = m.FromUserID,
                                MessageText = m.MessageText,
                                Time = m.Time,
                                Image = m.Image
                            };


                MessagesViewModel messagesVM = new MessagesViewModel();
                messagesVM.Messages = query.ToList();

                var updateQuery = db.Messages.Where(f => f.ToUserID == user).ToList();

                updateQuery.ForEach(a => a.Seen = true);

                db.Messages.RemoveRange(db.Messages.Where(f => f.ToUserID == user && f.Image != null));
                db.SaveChanges();
                
                return View(messagesVM);
                //messagesVM.Messages  = query.ToList<>

            }
        }
        [HttpPost]
        public ActionResult FileUpload(HttpPostedFileBase file)
        {
            if (file != null)
            {
                byte[] array = null;
                string pic = System.IO.Path.GetFileName(file.FileName);

                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    array = ms.GetBuffer();
                }

                Message msg = CreateNewMessage();

                msg.Image = array;

                using (var db = new MessagingContext())
                {
                    db.Messages.Add(msg);
                    db.SaveChanges();
                }

                ModelState.Clear();
                return View("Index");

            }
            // after successfully uploading redirect the user
            return RedirectToAction("actionname", "controller name");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendMessage(SendMessageViewModel model)
        {


            Message msg = CreateNewMessage();

            msg.MessageText = model.MessageText;

            using (var db = new MessagingContext())
            { 
                db.Messages.Add(msg);
                db.SaveChanges();
            }
            ModelState.Clear();
            return View("Index");
        }

        private Message CreateNewMessage()
        {
            string Name1 = "omrikon@gmail.com";
            string Name2 = "golan.idan@gmail.com";

            Message msg = new Message();
            if (User.Identity.GetUserName() == Name1)
            {
                msg.FromUserID = Name1;
                msg.ToUserID = Name2;
            }
            else
            {
                msg.FromUserID = Name2;
                msg.ToUserID = Name1;
            }

            msg.Seen = false;
            msg.Time = DateTime.Now;

              return msg;
        }
    }
}