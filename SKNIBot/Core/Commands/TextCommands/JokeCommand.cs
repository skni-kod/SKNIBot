using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SKNIBot.Core.Database;
using SKNIBot.Core.Database.Helpers;

namespace SKNIBot.Core.Commands.TextCommands
{
    [CommandsGroup("Tekst")]
    public class JokeCommand
    {
        private Random _random;

        public JokeCommand()
        {
            _random = new Random();
        }

        [Command("żart")]
        [Description("Żarty i suchary w postaci tekstu i obrazków.")]
        [Aliases("suchar", "joke", "itsJoke")]
        public async Task Joke(CommandContext ctx, [Description("Użytkownik do wzmienienia.")] DiscordMember member = null)
        {
            await ctx.TriggerTypingAsync();

            using (var databaseContext = new DatabaseContext())
            {
                var jokeToDisplay = databaseContext.Commands
                    .Include(p => p.SimpleResponses)
                    .First(p => p.Name == "Joke")
                    .SimpleResponses
                    .Random();

                var jokeContent = jokeToDisplay.Content;

                //Jeżeli długość jest jeden nie podano kodu
                if (member != null)
                {
                    jokeContent += " " + member.Mention;
                }

                await ctx.RespondAsync(jokeToDisplay.Content);
            }
        }
    }
}
