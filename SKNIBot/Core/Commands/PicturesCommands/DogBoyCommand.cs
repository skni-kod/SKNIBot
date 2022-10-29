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

namespace SKNIBot.Core.Commands.PicturesCommands
{
    //[CommandsGroup("Obrazki")]
    class DogBoyCommand : BaseCommandModule
    {
        private Random random;

        public DogBoyCommand()
        {
            random = new Random();
        }

        //[Command("dogboy")]
        //[Description("Wyświetla słodkie dogboy.")]
        public async Task Dog(CommandContext ctx, [Description("Wzmianka")] DiscordMember member = null)
        {
            await ctx.TriggerTypingAsync();

            using (var databaseContext = new StaticDBContext())
            {
                // String.Equals doesn't work in SQLite provider (comparison is case sensitive) so it must be replaced with DbFunctions.Like().
                var dogBoys = databaseContext.SimpleResponses
                    .Where(catboy => catboy.Command.Name == "DogBoy" && catboy.IsDeleted == false)
                    .Select(p => p.Content)
                    .ToList();

                var wordIndex = random.Next(0, dogBoys.Count);
                var response = dogBoys[wordIndex];

                await PostEmbedHelper.PostEmbed(ctx, "Dog boy", member?.Mention, response);
            }
        }
    }
}
