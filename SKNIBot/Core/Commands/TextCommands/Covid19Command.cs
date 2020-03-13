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
using SKNIBot.Core.Helpers;

namespace SKNIBot.Core.Commands.TextCommands
{
    [CommandsGroup("Tekst")]
    public class Covid19Command : BaseCommandModule
    {
        [Command("covid19")]
        [Description("COVID-19")]
        [Aliases("korona", "wirus", "covid-19")]
        public async Task Covid19(CommandContext ctx, string country)
        {
            await ctx.TriggerTypingAsync();

            using (var client = new WebClient())
            {
                var url = client.DownloadString("https://coronavirus-tracker-api.herokuapp.com/all");
                var covid19Container = JsonConvert.DeserializeObject<Covid19Container>(url);

                await ctx.RespondAsync(embed: BuildCountryEmbed(covid19Container, country));
            }
        }

        [Command("covid19")]
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
            var embed = CreateEmbed(covid19Container);

            var confirmedPolandStats = covid19Container.Confirmed.Locations.First(p => p.Country == "Poland");
            var deathsPolandStats = covid19Container.Deaths.Locations.First(p => p.Country == "Poland");
            var recoveredPolandStats = covid19Container.Recovered.Locations.First(p => p.Country == "Poland");
            var deathRatioPoland = (float) deathsPolandStats.Latest / confirmedPolandStats.Latest;

            var confirmedPolandChange = GetChange(confirmedPolandStats.Latest, confirmedPolandStats.History);
            var deathsPolandChange = GetChange(deathsPolandStats.Latest, deathsPolandStats.History);
            var recoveredPolandChange = GetChange(recoveredPolandStats.Latest, recoveredPolandStats.History);

            embed.AddField($"**Polska**",
                $"Zarażeni: {confirmedPolandStats.Latest} ({confirmedPolandChange:+0;-#} w ciągu ostatnich 24 godzin)\n" +
                $"Ofiary śmiertelne: {deathsPolandStats.Latest} ({deathsPolandChange:+0;-#} w ciągu ostatnich 24 godzin)\n" +
                $"Wyleczeni: {recoveredPolandStats.Latest} ({recoveredPolandChange:+0;-#} w ciągu ostatnich 24 godzin)\n" +
                "--------------------------\n" +
                $"Współczynnik śmiertelności: {deathRatioPoland:P}");

            var confirmedWorld = covid19Container.Confirmed.Latest;
            var deathsWorld = covid19Container.Deaths.Latest;
            var recoveredWorld = covid19Container.Recovered.Latest;
            var deathRatioWorld = (float)deathsWorld / confirmedWorld;

            var confirmedWorldChange = GetChange(confirmedWorld, covid19Container.Confirmed);
            var deathsWorldChange = GetChange(deathsWorld, covid19Container.Deaths);
            var recoveredWorldChange = GetChange(recoveredWorld, covid19Container.Recovered);

            embed.AddField($"**Świat**",
                $"Zarażeni: {confirmedWorld} ({confirmedWorldChange:+0;-#} w ciągu ostatnich 24 godzin)\n" +
                $"Ofiary śmiertelne: {deathsWorld} ({deathsWorldChange:+0;-#} w ciągu ostatnich 24 godzin)\n" +
                $"Wyleczeni: {recoveredWorld} ({recoveredWorldChange:+0;-#} w ciągu ostatnich 24 godzin)\n" +
                "--------------------------\n" +
                $"Współczynnik śmiertelności: {deathRatioWorld:P}\n" +
                "\u200B");

            var topConfirmed = GetCountriesWithTheBiggestChange(covid19Container.Confirmed, 3);
            var topDeaths = GetCountriesWithTheBiggestChange(covid19Container.Deaths, 3);
            var topRecovered = GetCountriesWithTheBiggestChange(covid19Container.Recovered, 3);

            embed.AddField("**Największy przyrost zarażeń**",
                string.Join('\n', topConfirmed.Select(p => $"{p.Key}: {p.Value:+0;-#} w ciągu ostatnich 24 godzin")));

            embed.AddField("**Największy przyrost ofiar śmiertelnych**",
                string.Join('\n', topDeaths.Select(p => $"{p.Key}: {p.Value:+0;-#} w ciągu ostatnich 24 godzin")));

            embed.AddField("**Największy przyrost wyleczonych**",
                string.Join('\n', topRecovered.Select(p => $"{p.Key}: {p.Value:+0;-#} w ciągu ostatnich 24 godzin")));

            return embed;
        }

        private DiscordEmbed BuildCountryEmbed(Covid19Container covid19Container, string country)
        {
            var embed = CreateEmbed(covid19Container);

            var confirmedPolandStats = covid19Container.Confirmed.Locations.First(p => p.Country == country);
            var deathsPolandStats = covid19Container.Deaths.Locations.First(p => p.Country == country);
            var recoveredPolandStats = covid19Container.Recovered.Locations.First(p => p.Country == country);
            var deathRatioPoland = (float)deathsPolandStats.Latest / confirmedPolandStats.Latest;

            var confirmedPolandChange = GetChange(confirmedPolandStats.Latest, confirmedPolandStats.History);
            var deathsPolandChange = GetChange(deathsPolandStats.Latest, deathsPolandStats.History);
            var recoveredPolandChange = GetChange(recoveredPolandStats.Latest, recoveredPolandStats.History);

            embed.AddField($"**{country}**",
                $"Zarażeni: {confirmedPolandStats.Latest} ({confirmedPolandChange:+0;-#} w ciągu ostatnich 24 godzin)\n" +
                $"Ofiary śmiertelne: {deathsPolandStats.Latest} ({deathsPolandChange:+0;-#} w ciągu ostatnich 24 godzin)\n" +
                $"Wyleczeni: {recoveredPolandStats.Latest} ({recoveredPolandChange:+0;-#} w ciągu ostatnich 24 godzin)\n" +
                "--------------------------\n" +
                $"Współczynnik śmiertelności: {deathRatioPoland:P}");

            return embed;
        }

        private DiscordEmbedBuilder CreateEmbed(Covid19Container covid19Container)
        {
            var lastUpdateTime = covid19Container.Confirmed.Locations.First().History.Max(p => p.Key);
            return new DiscordEmbedBuilder()
                .WithTitle("Koronne statystyki")
                .WithColor(DiscordColor.Blue)
                .WithFooter($"Ostatnia aktualizacja: {lastUpdateTime:D}");
        }

        private int GetChange(int latest, Dictionary<DateTime, int> stats)
        {
            return latest - stats
                .OrderByDescending(p => p.Key)
                .Skip(1)
                .First()
                .Value;
        }

        private int GetChange(int latest, Covid19Stats stats)
        {
            return latest - stats.Locations
                .Select(p =>
                    p.History
                        .OrderByDescending(q => q.Key)
                        .Skip(1)
                        .First())
                .OrderByDescending(p => p.Key)
                .Sum(p => p.Value);
        }

        private Dictionary<DateTime, int> GetMergedData(Covid19Stats stats, string country = null)
        {
            return stats.Locations
                .AsQueryable()
                .WhereIf(country != null, p => p.Country == country)
                .SelectMany(p => p.History)
                .GroupBy(p => p.Key)
                .Select(p => new KeyValuePair<DateTime, int>(p.Key, p.Sum(q => q.Value)))
                .OrderByDescending(p => p.Key)
                .ToDictionary(p => p.Key, p => p.Value);
        }

        private Dictionary<string, int> GetCountriesWithTheBiggestChange(Covid19Stats stats, int count)
        {
            var countryNames = stats.Locations
                .GroupBy(p => p.Country)
                .Select(p => p.Key)
                .ToList();

            var mergedData = countryNames
                .Select(name => new
                {
                    Country = name,
                    Data = GetMergedData(stats, name)
                })
                .ToList();

            return mergedData
                .Select(p => new
                {
                    Country = p.Country,
                    Change = GetChange(p.Data.First().Value, p.Data)
                })
                .OrderByDescending(p => p.Change)
                .Take(count)
                .ToDictionary(p => p.Country, p => p.Change);
        }
    }
}
