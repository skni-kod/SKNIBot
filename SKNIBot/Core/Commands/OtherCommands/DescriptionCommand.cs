using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace SKNIBot.Core.Commands.OtherCommands
{
    [CommandsGroup("Różne")]
    public class DescriptionCommand
    {
        private Timer _refreshDescriptionTimer;
        private int _refreshDescriptionInterval;

        private CommandContext _ctx;
        private string _description;

        public DescriptionCommand()
        {
            _refreshDescriptionInterval = 1000 * 60;    // every 1 minute

            _ctx = null;
            _description = string.Empty;

            _refreshDescriptionTimer = new Timer(RefreshDescriptionCallback, null, _refreshDescriptionInterval, Timeout.Infinite);
        }

        [Command("opis")]
        [Description("Zmienia opis bota.")]
        [RequirePermissions(Permissions.ManageMessages)]
        public async Task Description(CommandContext ctx, [Description("Nowy opis.")] string description = null)
        {
            _ctx = ctx;
            _description = description;

            await ctx.Client.UpdateStatusAsync(new DiscordGame(description));
        }

        private void RefreshDescriptionCallback(object state)
        {
            if (_ctx != null && _description != string.Empty)
            {
                _ctx.Client.UpdateStatusAsync(new DiscordGame(_description));
            }

            _refreshDescriptionTimer.Change(_refreshDescriptionInterval, Timeout.Infinite);
        }
    }
}
