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
    class DogGirlCommand : BaseCommandModule
    {
        private Random random;

        public DogGirlCommand()
        {
            random = new Random();
        }

        //[Command("doggirl")]
        //[Description("Wyświetla słodkie doggirl.")]
        public async Task Dog(CommandContext ctx, [Description("Wzmianka")] DiscordMember member = null)
        {
            await ctx.TriggerTypingAsync();

            using (var databaseContext = new StaticDBContext())
            {
                // String.Equals doesn't work in SQLite provider (comparison is case sensitive) so it must be replaced with DbFunctions.Like().
                var dogGirl = databaseContext.SimpleResponses
                    .Where(doggirl => doggirl.Command.Name == "DogGirl" && doggirl.IsDeleted == false)
                    .Select(p => p.Content)
                    .ToList();

                var wordIndex = random.Next(0, dogGirl.Count);
                var response = dogGirl[wordIndex];

                await PostEmbedHelper.PostEmbed(ctx, "Dog girl", member?.Mention, response);
            }
        }
    }
}
