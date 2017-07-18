using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MyGitTest123.Models
{
    public class Message
    {
        public int MessageID { get; set;  }
        public string FromUserID { get; set; }
        public DateTime Time { get; set; }
        public string ToUserID { get; set; }
        public string MessageText { get; set; }
  
        public byte[] Image { get; set; }
        public bool Seen { get; set; }

    }

    public class MessagingContext : DbContext 
    {
        public MessagingContext() :base("DefaultConnection")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<MessagingContext>(null);
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Message> Messages { get; set; }
    }
}