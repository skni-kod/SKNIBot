using System.Linq;
using System.Reflection;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Net.WebSocket;
using SKNIBot.Core.Settings;

namespace SKNIBot.Core
{
    public class Bot
    {
        private DiscordClient Client { get; set; }
        private CommandsNextModule Commands { get; set; }

        public void Run()
        {
            Connect();
            RegisterCommands();
        }

        private async void Connect()
        {
            var connectionConfig = new DiscordConfiguration
            {
                Token = SettingsLoader.Container.Token,
                TokenType = TokenType.Bot,

                AutoReconnect = true,
                LogLevel = LogLevel.Debug,
                UseInternalLogHandler = true
            };

            Client = new DiscordClient(connectionConfig);
            Client.SetWebSocketClient<WebSocket4NetClient>();

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefix = SettingsLoader.Container.Prefix,
                EnableDms = true,
                EnableMentionPrefix = true
            };

            Commands = Client.UseCommandsNext(commandsConfig);
            await Client.ConnectAsync();
        }

        private void RegisterCommands()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyTypes = assembly.GetTypes();

            var registerCommandsMethod = Commands.GetType().GetMethods()
                .FirstOrDefault(p => p.Name == "RegisterCommands" && p.IsGenericMethod);

            foreach (var type in assemblyTypes)
            {
                var attributes = type.GetCustomAttributes();
                if (attributes.Any(p => p.GetType() == typeof(CommandsGroupAttribute)))
                {
                    var genericRegisterCommandMethod = registerCommandsMethod.MakeGenericMethod(type);
                    genericRegisterCommandMethod.Invoke(Commands, null);
                }
            }
        }
    }
}
