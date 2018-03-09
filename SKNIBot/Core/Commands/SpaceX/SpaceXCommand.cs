using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Newtonsoft.Json;
using SKNIBot.Core.Settings;

namespace SKNIBot.Core.Commands.SpaceX
{
    [CommandsGroup]
    public class SpaceXCommand
    {
        private Random _random;

        public SpaceXCommand()
        {
            _random = new Random();
        }

        [Command("spacex")]
        [Description("TODO.")]
        public async Task SpaceX(CommandContext ctx)
        {
            var parameters = ctx.Message.Content.Split(' ').Skip(1).ToList();
            if (parameters.Count > 0)
            {
                switch (parameters[0])
                {
                    case "random":
                    {
                        await SendRandomFlight(ctx);
                        break;
                    }
                }
            }
        }

        private async Task SendRandomFlight(CommandContext ctx)
        {
            var client = new WebClient();

            var pastLaunches = client.DownloadString("https://api.spacexdata.com/v2/launches");
            var flights = JsonConvert.DeserializeObject<List<FlightData>>(pastLaunches);
            var flightIndex = _random.Next(0, flights.Count);

            var flightToDisplay = flights[flightIndex];

            var responseBuilder = new StringBuilder();
            responseBuilder.Append($"**{flightToDisplay.Launch_Date_UTC}**\r\n");
            responseBuilder.Append($"{flightToDisplay.Details}\r\n");
            responseBuilder.Append($"\r\n");
            responseBuilder.Append($"{flightToDisplay.Links.Video_Link}\r\n");

            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync(responseBuilder.ToString());
        }
    }
}
