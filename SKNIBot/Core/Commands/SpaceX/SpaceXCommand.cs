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
        private const string UpcomingLaunchesURL = "https://api.spacexdata.com/v2/launches/upcoming";
        private const string FHLaunchURL = "https://api.spacexdata.com/v2/launches?rocket_id=falconheavy";

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

                    case "heavy":
                    {
                        await SendFHFlight(ctx);
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
            var response = GetResponse(flightToDisplay);

            await ctx.RespondAsync(response);
        }

        private async Task SendUpcomingFlight(CommandContext ctx)
        {
            var client = new WebClient();

            var upcomingLaunches = client.DownloadString(UpcomingLaunchesURL);
            var flights = JsonConvert.DeserializeObject<List<FlightData>>(upcomingLaunches);

            var flightToDisplay = flights[0];
            var response = GetResponse(flightToDisplay);

            await ctx.RespondAsync(response);
        }

        private async Task SendFHFlight(CommandContext ctx)
        {
            var client = new WebClient();

            var upcomingLaunches = client.DownloadString(FHLaunchURL);
            var flights = JsonConvert.DeserializeObject<List<FlightData>>(upcomingLaunches);

            var flightToDisplay = flights[0];
            var response = GetResponse(flightToDisplay);

            await ctx.RespondAsync(response);
        }

        private string GetResponse(FlightData data)
        {
            var responseBuilder = new StringBuilder();
            responseBuilder.Append($"**{data.Rocket.Rocket_Name} {data.Rocket.Rocket_Type} ({data.Launch_Date_UTC})**\r\n");
            responseBuilder.Append("\r\n");

            responseBuilder.Append("**Payloads:**\r\n");
            foreach (var payload in data.Rocket.Second_Stage.Payloads)
            {
                responseBuilder.Append($"{payload.Payload_ID} ({payload.Payload_Mass_KG} kg) to {payload.Orbit} orbit\r\n");
            }

            if (data.Details != null)
            {
                responseBuilder.Append("\r\n");
                responseBuilder.Append($"{data.Details}\r\n");
            }

            responseBuilder.Append("\r\n");
            responseBuilder.Append($"{data.Links.Video_Link}\r\n");

            return responseBuilder.ToString();
        }
    }
}
