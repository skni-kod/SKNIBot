﻿using System.Collections.Generic;

namespace SKNIBot.Core.Database.Models.DynamicDB
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
        public virtual WelcomeMessage WelcomeMessage { get; set; }
        public virtual ServerVariable ServerVariable { get; set; }
        public virtual List<UserMessageStat> UserMessageStats { get; set; }
        public virtual List<DateMessageStat> DateMessageStats { get; set; }
    }
}
