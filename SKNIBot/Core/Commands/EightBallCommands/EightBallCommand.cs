using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace SKNIBot.Core.Commands.EightBallCommands
{
    [CommandsGroup("8Ball")]
    class EightBallCommand {
        List<string> _responses;
        Random _rand;

        public EightBallCommand() {
            _rand = new Random();

            _responses = new List<string> { "Zapytaj Andrzeja",
                                            "Masz ty w ogóle rozum i godność człowieka?!",
                                            "Na pewno być może",
                                            "Odpowiedź jest bliżej niż myślisz",
                                            "Zapytaj później"};
        }

        [Command("8Ball")]
        [Description("Magic 8 ball odpowie na wszystkie twoje pytania!")]
        public async Task EightBall(CommandContext ctx, [Description("Twoje pytanie")] string question) {
            await ctx.TriggerTypingAsync();

            int responseIndex = _rand.Next(0, _responses.Count);
            string response = string.Format("8Ball mówi: `{0}`", _responses[responseIndex]);

            await ctx.RespondAsync(response);
        }

        //[Command("8BallAdd")]
        //[RequirePermissions(DSharpPlus.Permissions.Administrator)]
        //public async Task EightBallAdd(CommandContext ctx, params string[] newResponse) {
        //    await ctx.TriggerTypingAsync();

        //    StringBuilder builder = new StringBuilder();
        //    foreach (var item in newResponse) {
        //        builder.AppendFormat("{0} ", item);
        //    }

        //    _responses.Add(builder.ToString());

        //    await ctx.RespondAsync("Odpowiedź dodana");
        //}

        [Command("8BallList")]
        public async Task EightBallList(CommandContext ctx) {
            await ctx.TriggerTypingAsync();

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < _responses.Count; i++) {
                builder.AppendFormat("{0}: {1}\n", i + 1, _responses[i]);
            }

            await ctx.RespondAsync(builder.ToString());
        }
    }
}
