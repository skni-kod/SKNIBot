using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Emzi0767.Utilities;
using SKNIBot.Core.Commands.VideoCommands;
using SKNIBot.Core.Settings;
using SKNIBot.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SKNIBot.Core.Services.RolesService;
using DSharpPlus.Exceptions;
using SKNIBot.Core.Helpers;
using SKNIBot.Core.Services.WelcomeMessageService;
using SKNIBot.Core.Handlers.WelcomeMessageHandlers;
using SKNIBot.Core.Services.ArchCounterService;
using SKNIBot.Core.Services.SimpleResponseService;
using SKNIBot.Core.Services.MessageResponseService;

namespace SKNIBot.Core
{
    public class Bot
    {
        public static DiscordClient DiscordClient { get; set; }
        private CommandsNextExtension _commands { get; set; }
        private MessageResponseService _messageResponseService;
        public void Run()
        {
            Connect();
            SetNetworkParameters();
            RegisterCommands();
        }

        private async void Connect()
        {
            var connectionConfig = new DiscordConfiguration
            {
                Token = SettingsLoader.SettingsContainer.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                Intents = DiscordIntents.All
            };

            DiscordClient = new DiscordClient(connectionConfig);

            _messageResponseService = new MessageResponseService();

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new [] {SettingsLoader.SettingsContainer.Prefix},
                EnableDms = false,
                EnableMentionPrefix = true,
                CaseSensitive = false,
                IgnoreExtraArguments = true,
                Services = BuildDependencies()
            };

            _commands = DiscordClient.UseCommandsNext(commandsConfig);
            _commands.SetHelpFormatter<CustomHelpFormatter>();
            _commands.CommandExecuted += Commands_CommandExecuted;
            _commands.CommandErrored += Commands_CommandErrored;

            DiscordClient.MessageCreated += DiscordClient_MessageCreatedAsync;
            DiscordClient.MessageUpdated += DiscordClient_MessageUpdatedAsync;
            DiscordClient.MessageReactionAdded += DiscordClient_MessageReactionAddedAsync;
            DiscordClient.GuildMemberAdded += DiscordClient_UserJoinedAsync;
            DiscordClient.MessageCreated += DiscordClient_MessageCreated;

            await DiscordClient.ConnectAsync();
        }

        private ServiceProvider BuildDependencies()
        {
            return new ServiceCollection()

            // Singletons
            .AddSingleton(_messageResponseService)

            // Helpers

            // Services
            .AddScoped<ArchCounterService>()
            .AddScoped<AssignRolesService>()
            .AddScoped<SimpleResponseService>()
            .AddScoped<WelcomeMessageService>()

            .BuildServiceProvider();
        }

        private async Task DiscordClient_MessageCreatedAsync(DiscordClient client, DSharpPlus.EventArgs.MessageCreateEventArgs e)
        {
            if (e.Channel.IsPrivate == false)
            {
                try
                {
                    EmojiCounterService emojiCounterService = new EmojiCounterService();
                    await emojiCounterService.CountEmojiInMessage(e.Message);
                }
                catch (Exception ie)
                {
                    Console.WriteLine("Error: Counting emoji in new message.");
                }
            }
        }

        private async Task DiscordClient_MessageUpdatedAsync(DiscordClient client, DSharpPlus.EventArgs.MessageUpdateEventArgs e)
        {
            if (e.Channel.IsPrivate == false)
            {
                try
                {
                    EmojiCounterService emojiCounterService = new EmojiCounterService();
                    await emojiCounterService.CountEmojiInMessage(e.Message);
                }
                catch (Exception ie)
                {
                    Console.WriteLine("Error: Counting emoji in edited message.");
                }
            }
        }

        private async Task DiscordClient_MessageReactionAddedAsync(DiscordClient client, DSharpPlus.EventArgs.MessageReactionAddEventArgs e)
        {
            if (e.Channel.IsPrivate == false)
            {
                try
                {
                    EmojiCounterService emojiCounterService = new EmojiCounterService();
                    await emojiCounterService.CountEmojiReaction(e.User, e.Emoji, e.Channel);
                }
                catch (Exception ie)
                {
                    Console.WriteLine("Error: Counting emoji in reactions.");
                }
            }
        }

        private async Task DiscordClient_UserJoinedAsync(DiscordClient client, DSharpPlus.EventArgs.GuildMemberAddEventArgs e)
        {
            WelcomeMessageHandler welcomeMessageHandler = new WelcomeMessageHandler(client.GetCommandsNext().Services.GetService<WelcomeMessageService>());
            welcomeMessageHandler.SendWelcomeMessage(client, e);  
        }
        private async Task DiscordClient_MessageCreated(DiscordClient sender, MessageCreateEventArgs e)
        {
            // Don't sent responses to bots
            if (e.Author.IsBot)
                return;
            var responses = _messageResponseService.GetResponses(e.Message.Content);
            if (responses.Count > 0)
            {
                await e.Channel.TriggerTypingAsync();
                foreach (var response in responses)
                {
                    await e.Channel.SendMessageAsync(response);
                }
            }
        }

        private void SetNetworkParameters()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        private void RegisterCommands()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyTypes = assembly.GetTypes();

            var registerCommandsMethod = _commands.GetType().GetMethods()
                .FirstOrDefault(p => p.Name == "RegisterCommands" && p.IsGenericMethod);

            foreach (var type in assemblyTypes)
            {
                var attributes = type.GetCustomAttributes();
                if (attributes.Any(p => p.GetType() == typeof(CommandsGroupAttribute)))
                {
                    var genericRegisterCommandMethod = registerCommandsMethod.MakeGenericMethod(type);
                    genericRegisterCommandMethod.Invoke(_commands, null);
                }

                foreach(var method in type.GetMethods())
                {
                    var attribute = method.GetCustomAttribute<MessageRespondAttribute>();
                    if (attribute != null)
                    {
                        if (!method.IsStatic)
                        {
                            throw new ArgumentException("Methods with MessageRespondAttribute must be static!");
                        }

                        DiscordClient.MessageCreated += async (client, e) =>
                        {
                            if (e.Author.IsBot)
                                return;
                               
                            var del = (AsyncEventHandler<Bot, MessageCreateEventArgs>)Delegate.CreateDelegate(typeof(AsyncEventHandler<Bot, MessageCreateEventArgs>), method);
                            await del.Invoke(this, e);
                        };
                    }
                }
            }
            
        }

        private Task Commands_CommandExecuted(CommandsNextExtension extension, CommandExecutionEventArgs e)
        {
            e.Context.Client.Logger.Log(LogLevel.Information, "SKNI Bot", $"{e.Context.User.Username} successfully executed '{e.Command.QualifiedName}'", DateTime.Now);
            return Task.FromResult(0);
        }

        private async Task Commands_CommandErrored(CommandsNextExtension extension, CommandErrorEventArgs e)
        {
            e.Context.Client.Logger.Log(LogLevel.Error, $"{e.Context.User.Username} tried executing '{e.Command?.QualifiedName ?? "<unknown command>"}' but it errored: {e.Exception.GetType()}: {e.Exception.Message ?? "<no message>"}.");

            StringBuilder messageToSend = new StringBuilder();

            switch (e.Exception)
            {
                case Checks​Failed​Exception ex:
                    {
                        
                        messageToSend.Append("Brak wystarczających uprawnień").AppendLine();

                        var failedChecks = ex.FailedChecks;
                        foreach (var failedCheck in failedChecks)
                        {
                            if (failedCheck is RequireBotPermissionsAttribute failBot)
                            {
                                messageToSend.Append("Ja potrzebuję");
                                messageToSend.Append(": ");
                                messageToSend.Append(failBot.Permissions.ToPermissionString());
                                messageToSend.AppendLine();
                            }
                            else if (failedCheck is RequireUserPermissionsAttribute failUser)
                            {
                                messageToSend.Append("Ty potrzebujesz");
                                messageToSend.Append(": ");
                                messageToSend.Append(failUser.Permissions.ToPermissionString());
                                messageToSend.AppendLine();
                            }
                            else if (failedCheck is RequireOwnerAttribute)
                            {
                                messageToSend.Append("Tylko mój włąściciel może to wykonać");
                                messageToSend.AppendLine();
                            }
                        }

                        messageToSend.Append($"---------------------------\n");
                        messageToSend.Append($"**{e.Exception.Message}**\n");
                        messageToSend.Append($"{e.Exception.StackTrace}\n");

                        await PostEmbedHelper.PostEmbed(e.Context, "Błąd", messageToSend.ToString());
                        return;
                    }
                case UnauthorizedException _:
                    {
                        messageToSend.Append($"---------------------------\n");
                        messageToSend.Append($"**{e.Exception.Message}**\n");
                        messageToSend.Append($"{e.Exception.StackTrace}\n");
                        await e.Context.Member.SendMessageAsync(messageToSend.ToString());
                        return;
                    }

                default:
                    {
                        messageToSend.Append($"**{e.Exception.Message}**\n");
                        messageToSend.Append($"{e.Exception.StackTrace}\n");

                        await PostEmbedHelper.PostEmbed(e.Context, "Błąd", messageToSend.ToString());
                        return;
                    }
            }    

        }
    }
}
