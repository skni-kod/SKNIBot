﻿namespace SKNIBot.Core.Database.Models.StaticDB
{
    public enum SimpleResponseType { Text, ImageLink, YoutubeLink, Other }
    public class SimpleResponse
    {
        public int ID { get; set; }
        public string Content { get; set; }
        public bool IsDeleted { get; set; }

        public SimpleResponseType Type {get; set;}

        public int CommandID { get; set; }
        public virtual Command Command { get; set; }
    }
}
