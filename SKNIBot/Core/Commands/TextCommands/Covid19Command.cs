using System;
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
        private DateTime _lastCacheUpdate;
        private Covid19Container _covidCache;
        private const int _cacheTimeMinutes = 120;

        public Covid19Command()
        {
            
        }

        [Command("covid19")]
        [Description("COVID-19")]
        [Aliases("korona", "wirus", "covid-19")]
        public async Task Covid19(CommandContext ctx, string country)
        {
            await ctx.TriggerTypingAsync();
            UpdateCache();

            var embed = BuildCountryEmbed(country);
            if (embed == null)
            {
                await ctx.RespondAsync("Country not found");
                return;
            }

            await ctx.RespondAsync(embed: embed);
        }

        [Command("covid19")]
        public async Task Covid19(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            UpdateCache();

            await ctx.RespondAsync(embed: BuildSummaryEmbed());
        }

        private void UpdateCache()
        {
            if ((DateTime.Now - _lastCacheUpdate).TotalMinutes < _cacheTimeMinutes)
            {
                return;
            }

            Console.WriteLine("Updating COVID-19 cache...");
            using (var client = new WebClient())
            {
                var url = client.DownloadString("https://coronavirus-tracker-api.herokuapp.com/all");

                _covidCache = JsonConvert.DeserializeObject<Covid19Container>(url);
                _lastCacheUpdate = DateTime.Now;
            }
            Console.WriteLine("Done!");
        }

        private DiscordEmbed BuildSummaryEmbed()
        {
            var embed = CreateEmbed();

            var confirmedPolandStats = _covidCache.Confirmed.Locations.First(p => p.Country == "Poland");
            var deathsPolandStats = _covidCache.Deaths.Locations.First(p => p.Country == "Poland");
            var recoveredPolandStats = _covidCache.Recovered.Locations.First(p => p.Country == "Poland");
            var deathRatioPoland = (float) deathsPolandStats.Latest / confirmedPolandStats.Latest;

            var confirmedPolandChange = GetChange(confirmedPolandStats.Latest, confirmedPolandStats.History);
            var deathsPolandChange = GetChange(deathsPolandStats.Latest, deathsPolandStats.History);
            var recoveredPolandChange = GetChange(recoveredPolandStats.Latest, recoveredPolandStats.History);

            embed.AddField("**Polska**",
                $"Zarażeni: {confirmedPolandStats.Latest} ({confirmedPolandChange:+0;-#} w ciągu ostatnich 24 godzin)\n" +
                $"Ofiary śmiertelne: {deathsPolandStats.Latest} ({deathsPolandChange:+0;-#} w ciągu ostatnich 24 godzin)\n" +
                $"Wyleczeni: {recoveredPolandStats.Latest} ({recoveredPolandChange:+0;-#} w ciągu ostatnich 24 godzin)\n" +
                "--------------------------\n" +
                $"Współczynnik śmiertelności: {deathRatioPoland:P}");

            var confirmedWorld = _covidCache.Confirmed.Latest;
            var deathsWorld = _covidCache.Deaths.Latest;
            var recoveredWorld = _covidCache.Recovered.Latest;
            var deathRatioWorld = (float)deathsWorld / confirmedWorld;

            var confirmedWorldChange = GetChange(confirmedWorld, _covidCache.Confirmed);
            var deathsWorldChange = GetChange(deathsWorld, _covidCache.Deaths);
            var recoveredWorldChange = GetChange(recoveredWorld, _covidCache.Recovered);

            embed.AddField("**Świat**",
                $"Zarażeni: {confirmedWorld} ({confirmedWorldChange:+0;-#} w ciągu ostatnich 24 godzin)\n" +
                $"Ofiary śmiertelne: {deathsWorld} ({deathsWorldChange:+0;-#} w ciągu ostatnich 24 godzin)\n" +
                $"Wyleczeni: {recoveredWorld} ({recoveredWorldChange:+0;-#} w ciągu ostatnich 24 godzin)\n" +
                "--------------------------\n" +
                $"Współczynnik śmiertelności: {deathRatioWorld:P}\n" +
                "\u200B");

            var topConfirmed = GetCountriesWithTheBiggestChange(_covidCache.Confirmed, 3);
            var topDeaths = GetCountriesWithTheBiggestChange(_covidCache.Deaths, 3);
            var topRecovered = GetCountriesWithTheBiggestChange(_covidCache.Recovered, 3);

            embed.AddField("**Największy przyrost zarażeń**",
                string.Join('\n', topConfirmed.Select(p => $"{p.Key}: {p.Value:+0;-#} w ciągu ostatnich 24 godzin")));

            embed.AddField("**Największy przyrost ofiar śmiertelnych**",
                string.Join('\n', topDeaths.Select(p => $"{p.Key}: {p.Value:+0;-#} w ciągu ostatnich 24 godzin")));

            embed.AddField("**Największy przyrost wyleczonych**",
                string.Join('\n', topRecovered.Select(p => $"{p.Key}: {p.Value:+0;-#} w ciągu ostatnich 24 godzin")));

            return embed;
        }

        private DiscordEmbed BuildCountryEmbed(string country)
        {
            if (!_covidCache.Confirmed.Locations.Any(p => p.Country == country))
            {
                return null;
            }

            var embed = CreateEmbed();

            var confirmedPolandStats = _covidCache.Confirmed.Locations.First(p => p.Country == country);
            var deathsPolandStats = _covidCache.Deaths.Locations.First(p => p.Country == country);
            var recoveredPolandStats = _covidCache.Recovered.Locations.First(p => p.Country == country);
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

        private DiscordEmbedBuilder CreateEmbed()
        {
            var lastUpdateTime = _covidCache.Confirmed.Locations.First().History.Max(p => p.Key);
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
