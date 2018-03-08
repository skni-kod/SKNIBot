using DSharpPlus;
using SKNIBot.Core.Settings;

namespace SKNIBot.Core
{
    public class Bot
    {
        private DiscordClient Client { get; set; }

        public void Run()
        {
            Init();
        }

        private async void Init()
        {
            var config = new DiscordConfiguration
            {
                Token = SettingsLoader.Container.Token,
                TokenType = TokenType.Bot,

                AutoReconnect = true,
                LogLevel = LogLevel.Debug,
                UseInternalLogHandler = true
            };

            Client = new DiscordClient(config);
            await Client.ConnectAsync();
        }
    }
}
