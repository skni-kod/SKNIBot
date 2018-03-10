using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKNIBot.Core.Commands {
    [CommandsGroup("Różne")]
    class RandomCommand {
        private Random _random;

        public RandomCommand() {
            _random = new Random();
        }

        [Description("Rzut kością n-ścienną")]
        [Command("K")]
        public async Task K(CommandContext ctx, [Description("Ilość ścian kości. ")] int n = 6) {
            await ctx.TriggerTypingAsync();

            if (n > 0) {
                int rnd = _random.Next(1, n);

                await ctx.RespondAsync(String.Format("Kość wyrzuciła: {0}", rnd));
            }
            else {
                await ctx.RespondAsync("Invalid Parametr, type '!help K' for more info!");
            }
        }

        [Description("Pseudo Random Number Generator. \nLosuje licznę od a do b.")]
        [Command("PRNG")]
        public async Task PRNG(CommandContext ctx, [Description("Dolna granica.")] int a = 0, [Description("Górna granica.")] int b = 10) {
            await ctx.TriggerTypingAsync();

            if (a <= b) {
                int rnd = _random.Next(a, b);
                await ctx.RespondAsync(String.Format("Wylosowano: {0}", rnd));
            }
            else {
                await ctx.RespondAsync("Invalid Parametr, type '!help K' for more info!");

            }
        }

    }
}
