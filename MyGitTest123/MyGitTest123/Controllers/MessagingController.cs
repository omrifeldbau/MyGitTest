using MyGitTest123.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.IO;

using System.Text;

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

    
                
                return View(messagesVM);

            }
        }

        public JsonResult MessagesViewed()
        {
            string user = User.Identity.GetUserName();
            using (var db = new MessagingContext())
            {

                var updateQuery = db.Messages.Where(f => f.ToUserID == user).ToList();

                updateQuery.ForEach(a => a.Seen = true);

                var ImageDeleteQuery = db.Messages.Where(f => f.ToUserID == user && f.Image != null).ToList();

                foreach(var item in ImageDeleteQuery)
                {
                    string fullPath = Request.MapPath("~/ContentUser/" + Encoding.ASCII.GetString( item.Image ));
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }

                //db.Messages.RemoveRange(db.Messages.Where(f => f.ToUserID == user && f.Image != null));
                db.SaveChanges();

                return Json(new { result = "ok" });
               

            }
        }

        [HttpPost]
        public ActionResult FileUpload(HttpPostedFileBase file)
        {
            if (file != null)
            {
                

                var fileName = Guid.NewGuid().ToString() +
                      System.IO.Path.GetExtension(file.FileName);

                string contentFolder = Server.MapPath("~/ContentUser");

                string fullFileName = Path.Combine(contentFolder, fileName);

                file.SaveAs(Path.Combine(contentFolder, fileName));

                //string pic = System.IO.Path.GetFileName(file.FileName);

                /* byte[] array = null;
                 *   using (MemoryStream ms = new MemoryStream())
                   {
                       file.InputStream.CopyTo(ms);
                       array = ms.GetBuffer();
                   }*/

                Message msg = CreateNewMessage();

                msg.Image = Encoding.ASCII.GetBytes(fileName);

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
            msg.Time = DateTime.Now.ToUniversalTime();

              return msg;
        }
    }
}