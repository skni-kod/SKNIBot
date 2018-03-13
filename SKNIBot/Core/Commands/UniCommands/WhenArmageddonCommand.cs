using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace SKNIBot.Core.Commands.TextCommands
{
    [CommandsGroup("Uczelnia")]
    public class WhenArmageddonCommand
    {
        /// <summary>
        /// Zwraca kiedy są najbliższe wykłady z systemów wbudowanych
        /// </summary>
        /// <param name="ctx">Kontekst</param>
        /// <param name="member">Użytkownik do wzmienienia</param>
        /// <returns></returns>
        [Command("kiedyArmageddon")]
        [Description("Zwraca kiedy są najbliższe wykłady z systemów wbudowanych.")]
        [Aliases("whenArmageddon", "kiedyŚwider", "kiedyWbudowane")]
        public async Task WhenArmageddon(CommandContext ctx, [Description("Użytkownik do wzmienienia.")] DiscordMember member = null)
        {

            await ctx.TriggerTypingAsync();

            DateTime today = DateTime.Today;
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            int daysUntilWednesday = ((int)DayOfWeek.Wednesday - (int)today.DayOfWeek + 7) % 7;
            DateTime nextWednesday = today.AddDays(daysUntilWednesday);
            DateTime nextLectures = nextWednesday.AddHours(14);

            TimeSpan timeSpan = nextLectures - DateTime.Now;

            if (member == null)
            {
                await ctx.RespondAsync("Najbliższe wykłady z systemów wbudowanych są za: " + timeSpan.Days + " dni " +
                    timeSpan.Hours + " godzin " + timeSpan.Minutes + " minut " + timeSpan.Seconds + " sekund");
            }
            else
            {
                await ctx.RespondAsync("Najbliższe wykłady z systemów wbudowanych są za: " + timeSpan.Days + " dni " +
                    timeSpan.Hours + " godzin " + timeSpan.Minutes + " minut " + timeSpan.Seconds + " sekund " + ctx.User.Mention);
            }
        }
    }
}
