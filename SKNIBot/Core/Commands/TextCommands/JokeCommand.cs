using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using SKNIBot.Core.Containers.TextContainers;

namespace SKNIBot.Core.Commands.TextCommands
{
    [CommandsGroup("Tekst")]
    public class JokeCommand
    {
        private Random _random;
        private List<JokesData> _jokes;
        private const string _jokesFile = "jokes.json";

        public JokeCommand()
        {
            _random = new Random();
            using (var file = new StreamReader(_jokesFile))
            {
                _jokes = JsonConvert.DeserializeObject<List<JokesData>>(file.ReadToEnd());
            }
        }

        [Command("żart")]
        [Description("Żarty i suchary w postaci tekstu i obrazków.")]
        [Aliases("suchar", "joke", "itsJoke")]
        public async Task Joke(CommandContext ctx, [Description("Użytkownik do wzmienienia.")] DiscordMember member = null)
        {
            await ctx.TriggerTypingAsync();

            var jokeIndex = _random.Next(0, _jokes.Count);
            var jokeToDisplay = _jokes[jokeIndex]; 
            //Jeżeli długość jest jeden nie podano kodu
            if (member == null)
            {
                await ctx.RespondAsync(jokeToDisplay.Joke);
            }
            else
            {
                await ctx.RespondAsync(jokeToDisplay.Joke + " " + member.Mention);
            }
        }
    }
}
