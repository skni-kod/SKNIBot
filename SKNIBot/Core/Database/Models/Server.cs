using System;
using System.Collections.Generic;
using System.Text;

namespace SKNIBot.Core.Database.Models
{
    public class Server
    {
        public Server() { }
        public Server(ulong id)
        {
            ServerID = id.ToString();
        }
        public int ID { get; set; }
        public string ServerID { get; set; }

        public virtual List<Emoji> Emojis { get; set; }
        public virtual List<AssignRole> AssignRoles { get; set; }
        public virtual WelcomeMessage WelcomeMessages { get; set; }
    }
}
