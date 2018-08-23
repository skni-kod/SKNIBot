using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SKNIBot.Core.Database;
using SKNIBot.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKNIBot.Core.Commands.TextCommands
{
    //https://cdn.discordapp.com/attachments/420661733544427520/482237629224714260/MEMEM.jpg

    [CommandsGroup("Tekst")]
    public class JavaWhyCommand : BaseCommandModule
    {
        Random _random;

        public JavaWhyCommand()
        {
            _random = new Random();
        }

        [Command("JavaWhy")]
        public async Task JavaWhy(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            using(var db = new StaticDBContext())
            {
                var things = db.JavaThings.ToList();

                int randIndex = _random.Next(0, things.Count);

                var thing = things[randIndex];

                DiscordEmbedBuilder embed = new DiscordEmbedBuilder()
                {
                    Color = ColorHelper.RandomColor(),
                    ImageUrl = "https://cdn.discordapp.com/attachments/420661733544427520/482237629224714260/MEMEM.jpg"
                };
                embed.AddField(thing.Title, thing.Content);

                await ctx.RespondAsync(embed: embed);
            }
        }
    }
}
