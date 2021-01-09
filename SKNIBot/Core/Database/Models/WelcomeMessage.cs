using System;
using System.Collections.Generic;
using System.Text;

namespace SKNIBot.Core.Database.Models
{
    public class WelcomeMessage
    {
        public int ID { get; set; }
        
        public int ServerID { get; set; }
        public string ChannelID { get; set; }
        public string Content { get; set; }
        public virtual Server Server { get; set; }
    }
}
