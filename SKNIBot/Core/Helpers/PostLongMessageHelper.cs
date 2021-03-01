using DSharpPlus.CommandsNext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKNIBot.Core.Helpers
{
    [Serializable]
    public class TooLongPartialStringException : Exception
    {
        public TooLongPartialStringException()
        { }

        public TooLongPartialStringException(string message)
            : base(message)
        { }

        public TooLongPartialStringException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
    class PostLongMessageHelper
    {
        public static async Task PostLongMessage(CommandContext ctx, string strings, string header = null, string imageLink = null, string footerText = null,
            string footerThumbnailLink = null, string embedThumbnailLink = null, string color = null)
        {
            await PostLongMessage(ctx, strings.Split(' ').ToList(), header, imageLink, footerText, footerThumbnailLink, embedThumbnailLink, color);
            return;
        }
        public static async Task PostLongMessage(CommandContext ctx, List<string> strings, string header = null, string imageLink = null, string footerText = null,
            string footerThumbnailLink = null, string embedThumbnailLink = null, string color = null)
        {
            StringBuilder response = new StringBuilder(2000);
            foreach (string s in strings)
            {
                // If part of message exceedes 1800 characters throw exception
                if(s.Length > 1800)
                {
                    throw new TooLongPartialStringException("Part of your message exceeded 1800 characters.");
                }
                if (response.Length + s.Length > 1800)
                {
                    await PostEmbedHelper.PostEmbed(ctx, header, response.ToString());
                    response.Clear();
                }
                response.Append(s);

                if (strings.IndexOf(s) != strings.Count - 1)
                {
                    response.Append(", ");
                }
            }
            if (response.Length > 0)
            {
                await PostEmbedHelper.PostEmbed(ctx, header, response.ToString(), imageLink, footerText, footerThumbnailLink, embedThumbnailLink, color);
                return;
            }
        }
    }
}
