using System;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SKNIBot.Core.Database;
using SKNIBot.Core.Services.SimpleResponseService;

namespace SKNIBot.Core.Commands.TextCommands
{
    [CommandsGroup("Tekst")]
    class JokeCommand : BaseCommandModule
    {
        private SimpleResponseService _simpleResponseService;

        public JokeCommand(SimpleResponseService simpleResponseService)
        {
            _simpleResponseService = simpleResponseService;
        }

        [Command("żart")]
        [Description("Żarty i suchary w postaci tekstu i obrazków.")]
        [Aliases("suchar", "joke", "itsJoke")]
        public async Task Joke(CommandContext ctx, [Description("Użytkownik do wzmienienia.")] DiscordMember member = null)
        {
            await ctx.TriggerTypingAsync();

            string answer = _simpleResponseService.GetAnswer("Joke");

            if (member != null)
            {
                answer += " " + member.Mention;
            }

            await ctx.RespondAsync(answer);
            
        }
    }
}
