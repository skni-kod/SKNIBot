using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SKNIBot.Core.Commands.ModerationCommands
{
    [CommandsGroup("Moderacja")]
    public class SeppukuCommand : BaseCommandModule
    {
        [Command("Seppuku")]
        [Description("Zgiń honorową śmiercią!")]
        public async Task Seppuku(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            await ctx.Member.RemoveAsync("Popełnił Seppuku. Gloria Victis!");
            

            var embed = new DSharpPlus.Entities.DiscordEmbedBuilder()
            {
                Color = Helpers.ColorHelper.RandomColor()
            };
            var candle = DSharpPlus.Entities.DiscordEmoji.FromName(ctx.Client, ":candle:");
            embed.AddField($"{ctx.Member.DisplayName} popełnił Seppuku",candle.ToString());
            embed.ImageUrl = "https://media1.tenor.com/images/6f64764b4b7874465d83de68342347cc/tenor.gif?itemid=4854729";
            await ctx.RespondAsync(embed: embed);
        }
    }
}
