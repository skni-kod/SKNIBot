using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SKNIBot.Core.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKNIBot.Core.Commands.PicturesCommands
{
    //[CommandsGroup("Obrazki")]
    class DogBoyCommand
    {
        private Random random;

        public DogBoyCommand()
        {
            random = new Random();
        }

        //[Command("catboy")]
        //[Description("Wyświetla słodkie dogboy.")]
        public async Task Dog(CommandContext ctx, [Description("Wzmianka")] DiscordMember member = null)
        {
            await ctx.TriggerTypingAsync();

            using (var databaseContext = new StaticDBContext())
            {
                // String.Equals doesn't work in SQLite provider (comparison is case sensitive) so it must be replaced with DbFunctions.Like().
                var dogBoys = databaseContext.SimpleResponses
                    .Where(catboy => catboy.Command.Name == "DogBoy")
                    .Select(p => p.Content)
                    .ToList();

                var wordIndex = random.Next(0, dogBoys.Count);
                var response = dogBoys[wordIndex];

                if (member != null)
                {
                    response += $" {member.Mention}";
                }

                await ctx.RespondAsync(response);
            }
        }
    }
}
