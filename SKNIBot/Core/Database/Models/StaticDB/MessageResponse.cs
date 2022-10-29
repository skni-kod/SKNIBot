using System;
using System.Collections.Generic;
using System.Text;

namespace SKNIBot.Core.Database.Models.StaticDB
{
    public class MessageResponse
    {
        public int ID { get; set; }
        public string Message { get; set; }
        public string Response { get; set; }
        public bool IsDeleted { get; set; }
    }
}
