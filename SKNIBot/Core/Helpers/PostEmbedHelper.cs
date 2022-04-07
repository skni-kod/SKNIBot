using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SKNIBot.Core.Helpers
{
    static class PostEmbedHelper
    {
        /// <summary>
        /// Function construct embed and send it to channel from given context.
        /// </summary>
        /// <param name="ctx">Context from which response channel is taken.</param>
        /// <param name="title">Title of embed</param>
        /// <param name="description">Content of embed</param>
        /// <param name="imageLink">Link to image posted in embed</param>
        /// <param name="footerText">Footer text</param>
        /// <param name="footerThumbnailLink">Link to image to footer thumbnail</param>
        /// <param name="embedThumbnailLink">Link to image to embed thumbnail</param>
        /// <param name="color">Color of embed</param>
        /// <returns>Task</returns>
        public static async Task PostEmbed(CommandContext ctx, string title = null, string description = null, string imageLink = null, string footerText = null,
            string footerThumbnailLink = null, string embedThumbnailLink = null, string color = null)
        {
            // Discord can't handle links with japanese characters
            if (imageLink != null)
            {
                EncodeJapaneseCharactersInFilename(imageLink);
            }
            if (embedThumbnailLink != null)
            {
                EncodeJapaneseCharactersInFilename(embedThumbnailLink);
            }

            var embed = new DiscordEmbedBuilder
            {
                ImageUrl = imageLink,
                Description = description,
                Title = title
            };

            if (footerText != null || footerThumbnailLink != null)
            {
                embed.Footer = new DiscordEmbedBuilder.EmbedFooter { Text = footerText, IconUrl = footerThumbnailLink };
            }

            if (color == null)
            {
                embed.Color = new DiscordColor("#5588EE");
            }
            else
            {
                embed.Color = new DiscordColor(color);
            }

            if (embedThumbnailLink != null)
            {
                embed.Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail();
                embed.Thumbnail.Url = embedThumbnailLink;
            }

            await ctx.RespondAsync(embed);
        }

        /// <summary>
        /// Encodes japanese and other characters in filename in image link.
        /// Discord can't properly handle such characters in links.
        /// Funciton changes only image name from link.
        /// </summary>
        /// <param name="imageLink">Link to image</param>
        /// <returns>Changed link.</returns>
        private static string EncodeJapaneseCharactersInFilename(string imageLink)
        {
            int lastSlashPos = imageLink.LastIndexOf("/");
            int lastQuestionMarkPos = imageLink.LastIndexOf("?");
            if (lastQuestionMarkPos > lastSlashPos)
            {
                string toChange = imageLink.Substring(lastSlashPos + 1, lastQuestionMarkPos - lastSlashPos - 1);
                string afterLastQuestionMark = imageLink.Substring(lastQuestionMarkPos);
                string changed = HttpUtility.UrlEncode(toChange, Encoding.UTF8);
                imageLink = imageLink.Remove(lastSlashPos + 1);
                imageLink += changed;
                imageLink += afterLastQuestionMark;
            }
            else
            {
                string toChange = imageLink.Substring(lastSlashPos + 1);
                string changed = HttpUtility.UrlEncode(toChange, Encoding.UTF8);
                imageLink = imageLink.Remove(lastSlashPos + 1);
                imageLink += changed;
            }
            return imageLink;
        }
    }
}
