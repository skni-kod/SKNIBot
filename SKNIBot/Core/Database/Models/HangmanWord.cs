namespace SKNIBot.Core.Database.Models
{
    public class HangmanWord
    {
        public int ID { get; set; }
        public string Word { get; set; }

        public int HangmanCategoryID { get; set; }
        public virtual HangmanCategory HangmanCategory { get; set; }
    }
}
