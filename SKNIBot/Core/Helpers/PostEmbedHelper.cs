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
        public static async Task PostEmbed(CommandContext ctx, string title = null, string description = null, string imageLink = null)
        {
            var embed = new DiscordEmbedBuilder
            {
                Color = new DiscordColor("#5588EE"),
                ImageUrl = imageLink,
                Description = description,
                Title = title

            };

            await ctx.RespondAsync(null, false, embed);
        }
    }
}
