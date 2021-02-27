using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using SKNIBot.Core.Database;

namespace SKNIBot.Core.Commands.TextCommands
{
    [CommandsGroup("Tekst")]
    public class EightBallCommand : BaseCommandModule {
        Random _rand;

        public EightBallCommand() {
            _rand = new Random();
        }

        [Command("8Ball")]
        [Description("Magic 8 ball odpowie na wszystkie twoje pytania!")]
        public async Task EightBall(CommandContext ctx, [Description("Twoje pytanie")] [RemainingText] string question)
        {
            await ctx.TriggerTypingAsync();

            using (var databaseContext = new StaticDBContext())
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

        [Command("8BallAdd")]
        [RequirePermissions(Permissions.ManageMessages)]
        public async Task EightBallAdd(CommandContext ctx, [Description("Nowa odpowiedź")][RemainingText] string newResponse) {
            await ctx.TriggerTypingAsync();

           using(var db = new StaticDBContext()) {
                var command = db.Commands.FirstOrDefault(c => c.Name == "8Ball");

                db.SimpleResponses.Add(new Database.Models.SimpleResponse {
                    Content = newResponse,
                    CommandID = command.ID
                });

                await db.SaveChangesAsync();
                await ctx.RespondAsync($"Odpowiedź '{newResponse}' dodana");
            }
        }

        [Command("8BallList")]
        public async Task EightBallList(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            using (var databaseContext = new StaticDBContext())
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
