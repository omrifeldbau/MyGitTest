using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyGitTest123.Models
{
    public class MessageViewModel
    {
        public string fromUser{ get; set; }
        public DateTime Time { get; set; }
        public string MessageText { get; set; }       
    }

    public class MessagesViewModel
    {
        public List<MessageViewModel> Messages;
    }

    public class SendMessageViewModel
    {
        public string MessageText { get; set; }
        public string ToUser { get; set; }
        public string FromUser { get; set; }
    }
}