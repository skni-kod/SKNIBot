using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace SKNIBot.Core.Commands.ModerationCommands
{
    [CommandsGroup("Moderacja")]
    public class OnlineCommand
    {
        private Timer _updateOnlineTimer;
        private int _updateOnlineInterval;

        private CommandContext _ctx;

        public OnlineCommand()
        {
            _updateOnlineInterval = 1000 * 60;    // every 1 minute
            _updateOnlineTimer = new Timer(UpdateOnlineCallback, null, _updateOnlineInterval, Timeout.Infinite);
        }

        [Command("online")]
        [Description("Wyświetla statystyki dotyczące czasu online użytkowników.")]
        [RequirePermissions(Permissions.ManageMessages)]
        public async Task Online(CommandContext ctx)
        {

        }

        private void UpdateOnlineCallback(object state)
        {
            _updateOnlineTimer.Change(_updateOnlineInterval, Timeout.Infinite);
        }
    }
}
