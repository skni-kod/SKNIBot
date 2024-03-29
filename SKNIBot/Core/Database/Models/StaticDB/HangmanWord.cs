﻿namespace SKNIBot.Core.Database.Models.StaticDB
{
    public class HangmanWord
    {
        public int ID { get; set; }
        public string Word { get; set; }
        public bool IsDeleted { get; set; }

        public int HangmanCategoryID { get; set; }
        public virtual HangmanCategory HangmanCategory { get; set; }
    }
}
