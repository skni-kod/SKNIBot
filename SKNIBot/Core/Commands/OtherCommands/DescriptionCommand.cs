using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace SKNIBot.Core.Commands.OtherCommands
{
    [CommandsGroup("Różne")]
    public class DescriptionCommand : BaseCommandModule
    {
        private Timer _refreshDescriptionTimer;
        private int _refreshDescriptionInterval;

        private string _description;

        public DescriptionCommand()
        {
            _refreshDescriptionInterval = 1000 * 60;    // every 1 minute
            _refreshDescriptionTimer = new Timer(RefreshDescriptionCallback, null, _refreshDescriptionInterval, Timeout.Infinite);

            _description = string.Empty;
        }

        [Command("opis")]
        [Description("Zmienia opis bota.")]
        [RequirePermissions(Permissions.ManageMessages)]
        public async Task Description(CommandContext ctx, [Description("Nowy opis.")] string description = null)
        {
            _description = description;

            await ctx.Client.UpdateStatusAsync(new DiscordActivity(description));
        }

        private void RefreshDescriptionCallback(object state)
        {
            if (_description != string.Empty)
            {
                Bot.DiscordClient.UpdateStatusAsync(new DiscordActivity(_description));
            }

            _refreshDescriptionTimer.Change(_refreshDescriptionInterval, Timeout.Infinite);
        }
    }
}
