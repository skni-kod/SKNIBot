using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SKNIBot.Core.Helpers
{
    static class PostPictureHelper
    {
        public static async Task PostPicture(CommandContext ctx, string link)
        {
            var embed = new DiscordEmbedBuilder
            {
                Color = new DiscordColor("#5588EE"),
                ImageUrl = link
            };

            await ctx.RespondAsync(null, false, embed);
        }
    }
}
