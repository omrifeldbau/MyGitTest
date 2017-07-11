using MyGitTest123.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
namespace MyGitTest123.Controllers
{
    public class MessagingController : Controller
    {
    
        public ActionResult Index()
        {
            string Name1 = "omrikon@gmail.com";
            string Name2 = "golan.idan@gmail.com";

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
                                Time = m.Time
                            };


                MessagesViewModel messagesVM = new MessagesViewModel();
                messagesVM.Messages = query.ToList();

                var updateQuery = db.Messages.Where(f => f.ToUserID == user).ToList();

                updateQuery.ForEach(a => a.Seen = true);
                db.SaveChanges();
                return View(messagesVM);
                //messagesVM.Messages  = query.ToList<>

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendMessage(SendMessageViewModel model)
        {
            string Name1 = "omrikon@gmail.com";
            string Name2 = "golan.idan@gmail.com";

            using (var db = new MessagingContext())
            {
                Message msg = new Message();
                if(User.Identity.GetUserName() == Name1)
                {
                    msg.FromUserID = Name1;
                    msg.ToUserID = Name2;
                }
                else
                {
                    msg.FromUserID = Name2;
                    msg.ToUserID = Name1;
                }

                msg.MessageText = model.MessageText;
                msg.Seen = false;
                msg.Time = DateTime.Now;

                db.Messages.Add(msg);
                db.SaveChanges();
            }
            return View("Index");
        }
    }
}