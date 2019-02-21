using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SKNIBot.Core.Helpers
{
    static class PostEmbedHelper
    {
        public static async Task PostEmbed(CommandContext ctx, string title = null, string description = null, string imageLink = null, string footerText = null)
        {
            var embed = new DiscordEmbedBuilder
            {
                Color = new DiscordColor("#5588EE"),
                ImageUrl = imageLink,
                Description = description,
                Title = title
            };
            if (footerText != null)
            {
                embed.Footer = new DiscordEmbedBuilder.EmbedFooter();
                embed.Footer.Text = footerText;
            }
            await ctx.RespondAsync(null, false, embed);
        }
    }
}
