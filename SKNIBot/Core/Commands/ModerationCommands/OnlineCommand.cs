using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using SKNIBot.Core.Database;
using SKNIBot.Core.Database.Models;
using SKNIBot.Core.Helpers;

namespace SKNIBot.Core.Commands.ModerationCommands
{
    [CommandsGroup("Moderacja")]
    public class OnlineCommand
    {
        private const int UpdateOnlineInterval = 1000 * 60;
        private const int UsernameFieldLength = 25;
        private const int LastOnlineFieldLength = 25;
        private const int TotalTimeFieldLength = 20;
        private const int ItemsPerPage = 20;
        private const string PaginationIdentifier = "#";

        private int TotalFieldsLength => UsernameFieldLength + LastOnlineFieldLength + TotalTimeFieldLength;

        private Timer _updateOnlineTimer;
        private PaginationManager _paginationManager;

        public OnlineCommand()
        {
            _updateOnlineTimer = new Timer(UpdateOnlineCallback, null, UpdateOnlineInterval, Timeout.Infinite);
            _paginationManager = new PaginationManager();

            Bot.DiscordClient.MessageReactionAdded += DiscordClientOnMessageReactionAdded;
        }

        [Command("online")]
        [Description("Wyświetla statystyki dotyczące czasu online użytkowników.")]
        [RequirePermissions(Permissions.ManageMessages)]
        public async Task Online(CommandContext ctx, [Description("username, last, total")] string orderBy = "total")
        {
            await ctx.TriggerTypingAsync();

            var onlineList = GetOnlineList(1, orderBy);

            var message = await ctx.RespondAsync(onlineList);
            await message.CreateReactionAsync(DiscordEmoji.FromName(Bot.DiscordClient, PaginationManager.LeftEmojiName));
            await message.CreateReactionAsync(DiscordEmoji.FromName(Bot.DiscordClient, PaginationManager.RightEmojiName));
        }

        private string GetOnlineList(int currentPage, string orderBy)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(PaginationIdentifier);
            stringBuilder.Append(" ");
            stringBuilder.Append(_paginationManager.GeneratePaginationHeader(currentPage, GetPagesCount()));
            stringBuilder.Append(" ");
            stringBuilder.Append(GetOrderByHeader(orderBy));
            stringBuilder.Append(".\n");

            stringBuilder.Append("```");

            stringBuilder.Append("Nazwa użytkownika".PadRight(UsernameFieldLength));
            stringBuilder.Append("Ostatnio online".PadRight(LastOnlineFieldLength));
            stringBuilder.Append("Łączny czas [h]".PadRight(TotalTimeFieldLength));
            stringBuilder.Append("\n");
            stringBuilder.Append(new string('-', TotalFieldsLength));
            stringBuilder.Append("\n");

            using (var databaseContext = new DynamicDBContext())
            {
                var onlineStatsQuery = databaseContext.OnlineStats
                    .OrderByDescending(p => p.TotalTime);

                switch (orderBy)
                {
                    case "username": onlineStatsQuery = onlineStatsQuery.OrderBy(p => p.Username); break;
                    case "last": onlineStatsQuery = onlineStatsQuery.OrderByDescending(p => p.LastOnline); break;
                    case "total": onlineStatsQuery = onlineStatsQuery.OrderByDescending(p => p.TotalTime); break;
                }

                var onlineStats = onlineStatsQuery.Skip((currentPage - 1) * ItemsPerPage).Take(ItemsPerPage).ToList();

                foreach (var user in onlineStats)
                {
                    var username = user.Username.PadRight(UsernameFieldLength);
                    var lastOnline = user.LastOnline.ToString("yyyy-MM-dd HH:mm").PadRight(LastOnlineFieldLength);

                    var totalTimeInHours = (float)user.TotalTime / 60;
                    var totalTime = totalTimeInHours.ToString("0.0").PadRight(TotalTimeFieldLength);

                    stringBuilder.Append($"{username}{lastOnline}{totalTime}\n");
                }
            }

            stringBuilder.Append("```");
            return stringBuilder.ToString();
        }

        private void UpdateOnlineCallback(object state)
        {
            using (var databaseContext = new DynamicDBContext())
            {
                var onlineUsers = Bot.DiscordClient.Presences.Where(p => p.Value.Status != UserStatus.Offline).ToList();
                foreach (var user in onlineUsers)
                {
                    var onlineStats = databaseContext.OnlineStats.FirstOrDefault(p => p.Username == user.Value.User.Username);
                    if (onlineStats == null)
                    {
                        // Remove seconds and milliseconds from date to better ordering
                        var now = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        var fixedNow = DateTime.Parse(now);

                        onlineStats = new OnlineStats
                        {
                            Username = user.Value.User.Username,
                            LastOnline = fixedNow,
                            TotalTime = 0
                        };

                        databaseContext.OnlineStats.Add(onlineStats);
                    }
                    else
                    {
                        onlineStats.LastOnline = DateTime.Now;
                        onlineStats.TotalTime += UpdateOnlineInterval / 1000 / 60;
                    }
                }

                databaseContext.SaveChanges();
            }

            _updateOnlineTimer.Change(UpdateOnlineInterval, Timeout.Infinite);
        }

        private int GetPagesCount()
        {
            using (var databaseContext = new DynamicDBContext())
            {
                return databaseContext.OnlineStats.Count() / ItemsPerPage + 1;
            }
        }

        private string GetOrderByHeader(string orderBy)
        {
            return $"Wybrane sortowanie: {orderBy}";
        }

        private string ParseOrderByHeader(string header)
        {
            var matches = Regex.Matches(header, "Wybrane sortowanie: (?<orderBy>[a-z]*)");
            var orderBy = matches[0].Groups["orderBy"].Value;

            return orderBy;
        }

        private async Task DiscordClientOnMessageReactionAdded(MessageReactionAddEventArgs reactionEvent)
        {
            if (reactionEvent.User.IsBot) return;

            // We have to get message instead of reactionEvent.Message - sometimes message here is null, especially
            // when the bot has been restarted.
            var message = await reactionEvent.Channel.GetMessageAsync(reactionEvent.Message.Id);
            if (!message.Content.StartsWith(PaginationIdentifier)) return;

            var header = message.Content.Substring(0, message.Content.IndexOf("\n"));

            var paginationData = _paginationManager.ParsePaginationHeader(header);
            var orderByData = ParseOrderByHeader(header);

            _paginationManager.UpdatePagination(paginationData, reactionEvent);

            var onlineList = GetOnlineList(paginationData.CurrentPage, orderByData);

            await reactionEvent.Message.DeleteReactionAsync(reactionEvent.Emoji, reactionEvent.User);
            await reactionEvent.Message.ModifyAsync(onlineList);
        }
    }
}
