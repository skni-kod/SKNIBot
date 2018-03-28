using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using SKNIBot.Core.Database;

namespace SKNIBot.Core.Commands.EightBallCommands
{
    [CommandsGroup("8Ball")]
    public class EightBallCommand {
        Random _rand;

        public EightBallCommand() {
            _rand = new Random();
        }

        [Command("8Ball")]
        [Description("Magic 8 ball odpowie na wszystkie twoje pytania!")]
        public async Task EightBall(CommandContext ctx, [Description("Twoje pytanie")] string question)
        {
            await ctx.TriggerTypingAsync();

            using (var databaseContext = new DatabaseContext())
            {
                var eightBallResponses = databaseContext.SimpleResponses.Where(p => p.Command.Name == "8Ball");
                var randomIndex = _rand.Next(eightBallResponses.Count());

                var randomResponse = eightBallResponses
                    .OrderBy(p => p.ID)
                    .Select(p => p.Content)
                    .Skip(randomIndex)
                    .First();

                await ctx.RespondAsync($"8Ball mówi: {randomResponse}");
            }
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
        public async Task EightBallList(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            using (var databaseContext = new DatabaseContext())
            {
                var builder = new StringBuilder();

                var eightBallResponses = databaseContext.SimpleResponses
                    .Where(p => p.Command.Name == "8Ball")
                    .OrderBy(p => p.ID)
                    .Select(p => p.Content)
                    .ToList();

                for(var i=0; i< eightBallResponses.Count; i++)
                {
                    builder.Append($"{i + 1}: {eightBallResponses[i]}\n");
                }

                await ctx.RespondAsync(builder.ToString());
            }
        }
    }
}
