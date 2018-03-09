using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Newtonsoft.Json;

namespace SKNIBot.Core.Commands.SpaceX
{
    [CommandsGroup]
    public class SpaceXCommand
    {
        private Random _random;

        private const string PastLaunchesURL = "https://api.spacexdata.com/v2/launches";
        private const string UpcomingLaunchesURL = "https://api.spacexdata.com/v2/upcoming";

        public SpaceXCommand()
        {
            _random = new Random();
        }

        [Command("spacex")]
        [Description("TODO.")]
        public async Task SpaceX(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

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

                    case "upcoming":
                    {
                        await SendUpcomingFlight(ctx);
                        break;
                    }
                }
            }
        }

        private async Task SendRandomFlight(CommandContext ctx)
        {
            var client = new WebClient();

            var pastLaunches = client.DownloadString(PastLaunchesURL);
            var flights = JsonConvert.DeserializeObject<List<FlightData>>(pastLaunches);
            var flightIndex = _random.Next(0, flights.Count);

            var flightToDisplay = flights[flightIndex];

            var responseBuilder = new StringBuilder();
            responseBuilder.Append($"**{flightToDisplay.Rocket.Rocket_Name} {flightToDisplay.Rocket.Rocket_Type} ({flightToDisplay.Launch_Date_UTC})**\r\n");
            responseBuilder.Append("\r\n");

            responseBuilder.Append("**Payloads:**\r\n");
            foreach (var payload in flightToDisplay.Rocket.Second_Stage.Payloads)
            {
                responseBuilder.Append($"{payload.Payload_ID} ({payload.Payload_Mass_KG} kg) to {payload.Orbit} orbit\r\n");
            }

            if (flightToDisplay.Details != null)
            {
                responseBuilder.Append("\r\n");
                responseBuilder.Append($"{flightToDisplay.Details}\r\n");
            }

            responseBuilder.Append("\r\n");
            responseBuilder.Append($"{flightToDisplay.Links.Video_Link}\r\n");

            await ctx.RespondAsync(responseBuilder.ToString());
        }

        private async Task SendUpcomingFlight(CommandContext ctx)
        {
            var client = new WebClient();

            var upcomingLaunches = client.DownloadString(UpcomingLaunchesURL);
            var flights = JsonConvert.DeserializeObject<List<FlightData>>(upcomingLaunches);
            var flightToDisplay = flights[0];

            var responseBuilder = new StringBuilder();
            responseBuilder.Append($"**{flightToDisplay.Rocket.Rocket_Name} {flightToDisplay.Rocket.Rocket_Type} ({flightToDisplay.Launch_Date_UTC})**\r\n");
            responseBuilder.Append("\r\n");

            responseBuilder.Append("**Payloads:**\r\n");
            foreach (var payload in flightToDisplay.Rocket.Second_Stage.Payloads)
            {
                responseBuilder.Append($"{payload.Payload_ID} ({payload.Payload_Mass_KG} kg) to {payload.Orbit} orbit\r\n");
            }

            if (flightToDisplay.Details != null)
            {
                responseBuilder.Append("\r\n");
                responseBuilder.Append($"{flightToDisplay.Details}\r\n");
            }

            responseBuilder.Append("\r\n");
            responseBuilder.Append(flightToDisplay.Links.Video_Link != null
                ? $"{flightToDisplay.Links.Video_Link}\r\n"
                : "Video not available yet.\r\n");

            await ctx.RespondAsync(responseBuilder.ToString());
        }
    }
}
