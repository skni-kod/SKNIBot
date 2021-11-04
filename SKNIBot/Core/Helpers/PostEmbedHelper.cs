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
        public static async Task PostEmbed(CommandContext ctx, string title = null, string description = null, string imageLink = null, string footerText = null,
            string footerThumbnailLink = null, string embedThumbnailLink = null, string color = null)
        {
            // Discord can't handle links with japanese characters
            if (imageLink != null)
            {
                int pos = imageLink.LastIndexOf("/");
                string toChange = imageLink.Substring(pos + 1);
                string changed = HttpUtility.UrlEncode(toChange, Encoding.UTF8);
                imageLink = imageLink.Remove(pos + 1);
                imageLink += changed;
            }
            if (embedThumbnailLink != null)
            {
                int posThumbnail = embedThumbnailLink.LastIndexOf("/");
                string toChangeThumbnail = embedThumbnailLink.Substring(posThumbnail + 1);
                string changedThumbnail = HttpUtility.UrlEncode(toChangeThumbnail, Encoding.UTF8);
                embedThumbnailLink = embedThumbnailLink.Remove(posThumbnail + 1);
                embedThumbnailLink += changedThumbnail;
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
    }
}
