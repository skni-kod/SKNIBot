using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SKNIBot.Core.Database;
using SKNIBot.Core.Database.Models;

namespace SKNIBot.Core.Commands.ModerationCommands
{
    [CommandsGroup("Moderacja")]
    public class OnlineCommand
    {
        private const int UpdateOnlineInterval = 1000 * 60;
        private const int UsernameFieldLength = 25;
        private const int LastOnlineFieldLength = 25;
        private const int TotalTimeFieldLength = 20;

        private int TotalFieldsLength => UsernameFieldLength + LastOnlineFieldLength + TotalTimeFieldLength;

        private Timer _updateOnlineTimer;

        public OnlineCommand()
        {
            _updateOnlineTimer = new Timer(UpdateOnlineCallback, null, UpdateOnlineInterval, Timeout.Infinite);
        }

        [Command("online")]
        [Description("Wyświetla statystyki dotyczące czasu online użytkowników.")]
        [RequirePermissions(Permissions.ManageMessages)]
        public async Task Online(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var stringBuilder = new StringBuilder();
            stringBuilder.Append("```");
            stringBuilder.Append("Nazwa użytkownika".PadRight(UsernameFieldLength));
            stringBuilder.Append("Ostatnio online".PadRight(LastOnlineFieldLength));
            stringBuilder.Append("Łączny czas [minuty]".PadRight(TotalTimeFieldLength));
            stringBuilder.Append("\n");
            stringBuilder.Append(new string('-', TotalFieldsLength));
            stringBuilder.Append("\n");

            using (var databaseContext = new DatabaseContext())
            {
                var onlineStats = databaseContext.OnlineStats.OrderByDescending(p => p.LastOnline).ThenByDescending(p => p.TotalTime).ToList();
                foreach (var user in onlineStats)
                {
                    var username = user.Username.PadRight(UsernameFieldLength);
                    var lastOnline = user.LastOnline.ToString("yyyy-MM-dd HH:mm").PadRight(LastOnlineFieldLength);
                    var totalTime = user.TotalTime.ToString().PadRight(TotalTimeFieldLength);

                    stringBuilder.Append($"{username}{lastOnline}{totalTime}\n");
                }
            }

            stringBuilder.Append("```");
            await ctx.RespondAsync(stringBuilder.ToString());
        }

        private void UpdateOnlineCallback(object state)
        {
            using (var databaseContext = new DatabaseContext())
            {
                var onlineUsers = Bot.DiscordClient.Presences.Where(p => p.Value.Status != UserStatus.Offline).ToList();
                foreach (var user in onlineUsers)
                {
                    var onlineStats = databaseContext.OnlineStats.FirstOrDefault(p => p.Username == user.Value.User.Username);
                    if (onlineStats == null)
                    {
                        onlineStats = new OnlineStats
                        {
                            Username = user.Value.User.Username,
                            LastOnline = DateTime.Now,
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
    }
}
