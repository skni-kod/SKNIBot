using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using SKNIBot.Core.Containers.TextContainers.Covid19;

namespace SKNIBot.Core.Commands.TextCommands
{
    [CommandsGroup("Tekst")]
    public class Covid19Command : BaseCommandModule
    {
        [Command("covid19")]
        [Description("COVID-19")]
        [Aliases("korona")]
        public async Task Covid19(CommandContext ctx, string country)
        {
            await ctx.TriggerTypingAsync();

            using (var client = new WebClient())
            {
                var url = client.DownloadString("https://coronavirus-tracker-api.herokuapp.com/all");
                var covid19Container = JsonConvert.DeserializeObject<Covid19Container>(url);

                ///await ctx.RespondAsync(embed: BuildEmbed(covid19Container));
            }
        }

        [Command("covid19")]
        [Description("COVID-19")]
        public async Task Covid19(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            using (var client = new WebClient())
            {
                var url = client.DownloadString("https://coronavirus-tracker-api.herokuapp.com/all");
                var covid19Container = JsonConvert.DeserializeObject<Covid19Container>(url);

                await ctx.RespondAsync(embed: BuildSummaryEmbed(covid19Container));
            }
        }

        private DiscordEmbed BuildSummaryEmbed(Covid19Container covid19Container)
        {
            var embed = new DiscordEmbedBuilder()
                .WithTitle("Koronne statystyki")
                .WithColor(DiscordColor.Blue)
                .WithFooter($"Ostatnia aktualizacja: {covid19Container.Confirmed.LastUpdated:G}");

            var confirmedPolandStats = covid19Container.Confirmed.Locations.First(p => p.Country == "Poland");
            var deathsPolandStats = covid19Container.Deaths.Locations.First(p => p.Country == "Poland");
            var recoveredPolandStats = covid19Container.Recovered.Locations.First(p => p.Country == "Poland");
            var deathRatioPoland = (float) deathsPolandStats.Latest / confirmedPolandStats.Latest;

            var confirmedPolandChange = confirmedPolandStats.Latest - GetChange(confirmedPolandStats.History);
            var deathsPolandChange = deathsPolandStats.Latest - GetChange(deathsPolandStats.History);
            var recoveredPolandChange = recoveredPolandStats.Latest - GetChange(recoveredPolandStats.History);

            embed.AddField($"**Polska** (stan na {covid19Container.Confirmed.LastUpdated:G})",
                $"Zarażeni: {confirmedPolandStats.Latest} ({confirmedPolandChange:+0;-#} w ciągu ostatnich 24 godzin)\n" +
                $"Ofiary śmiertelne: {deathsPolandStats.Latest} ({deathsPolandChange:+0;-#} w ciągu ostatnich 24 godzin)\n" +
                $"Wyleczeni: {recoveredPolandStats.Latest} ({recoveredPolandChange:+0;-#} w ciągu ostatnich 24 godzin)\n" +
                "--------------------------\n" +
                $"Współczynnik śmiertelności: {deathRatioPoland:P}");

            var confirmedWorld = covid19Container.Confirmed.Latest;
            var deathsWorld = covid19Container.Deaths.Latest;
            var recoveredWorld = covid19Container.Recovered.Latest;
            var deathRatioWorld = (float)deathsWorld / confirmedWorld;

            var confirmedWorldChange = confirmedWorld - GetChange(covid19Container.Confirmed);
            var deathsWorldChange = deathsWorld - GetChange(covid19Container.Deaths);
            var recoveredWorldChange = recoveredWorld - GetChange(covid19Container.Recovered);

            embed.AddField($"**Świat** (stan na {covid19Container.Confirmed.LastUpdated:G})",
                $"Zarażeni: {confirmedWorld} ({confirmedWorldChange:+0;-#} w ciągu ostatnich 24 godzin)\n" +
                $"Ofiary śmiertelne: {deathsWorld} ({deathsWorldChange:+0;-#} w ciągu ostatnich 24 godzin)\n" +
                $"Wyleczeni: {recoveredWorld} ({recoveredWorldChange:+0;-#} w ciągu ostatnich 24 godzin)\n" +
                "--------------------------\n" +
                $"Współczynnik śmiertelności: {deathRatioWorld:P}");

            return embed;
        }

        private int GetChange(Dictionary<DateTime, int> stats)
        {
            return stats
                .OrderByDescending(p => p.Key)
                .Skip(1)
                .First()
                .Value;
        }

        private int GetChange(Covid19Stats stats)
        {
            return stats.Locations
                .Select(p =>
                    p.History
                        .OrderByDescending(q => q.Key)
                        .Skip(1)
                        .First())
                .Sum(p => p.Value);
        }
    }
}
