using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Newtonsoft.Json;
using SKNIBot.Core.Containers.TechContainers.SpaceX;

namespace SKNIBot.Core.Commands.TechCommands
{
    [CommandsGroup("Tech")]
    public class SpaceXCommand : BaseCommandModule
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
        [Description("In Elon we trust, in thrust we trust.")]
        public async Task SpaceX(CommandContext ctx, [Description("random, upcoming, heavy")] string type = null)
        {
            await ctx.TriggerTypingAsync();
            if (type == null)
            {
                await SendParameterNotFoundError(ctx);
                return;
            }

            switch (type)
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

                default:
                    {
                        await SendParameterNotFoundError(ctx);
                        break;
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

        private async Task SendParameterNotFoundError(CommandContext ctx)
        {
            await ctx.RespondAsync("Parameter not found, check description (!help spacex) to see more info.");
        }

        private string GetResponse(FlightData data)
        {
            var responseBuilder = new StringBuilder();
            DateTime? launchDate = null;

            if (data.Launch_Date_UTC.HasValue)
            {
                launchDate = TimeZoneInfo.ConvertTimeFromUtc(data.Launch_Date_UTC.Value, TimeZoneInfo.Local);
            }

            responseBuilder.Append($"**{data.Rocket.Rocket_Name} {data.Rocket.Rocket_Type} ({(launchDate.HasValue ? $"{launchDate.Value:yyyy-MM-dd HH:mm:ss}" : "date undefined")})**\r\n");
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
