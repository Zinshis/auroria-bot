using AuroriaBot.Configuration;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Serilog;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AuroriaBot.Discord
{
    public interface IDiscordBotClient
    {
        Task Initialize();
    }

    public class DiscordBotClient : IDiscordBotClient
    {
        private readonly ConfigurationOptions _options;
        private DiscordSocketClient _client;
        private CommandService _commandService;

        public DiscordBotClientState State { get; private set; }

        public DiscordBotClient(ConfigurationOptions options)
        {
            _options = options;
            State = DiscordBotClientState.Created;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task Initialize()
        {
            if (State == DiscordBotClientState.Created)
            {
                _client = new DiscordSocketClient();
                _commandService = new CommandService();

                _client.Log += LogHandler;
                _commandService.Log += LogHandler;

                await _client.LoginAsync(TokenType.Bot, _options.Token);
                await _client.StartAsync(); // => https://docs.stillu.cc/guides/concepts/connections.html

                State = DiscordBotClientState.Initialized;

                // TODO: Move to seperate CommandHandler class
                _client.MessageReceived += HandleCommandAsync; // TODO: Enable on _client ready State (see connections above)
                await _commandService.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: null);
            }
            else
            {
                throw new InvalidOperationException("Discord bot client has already been initialized.");
            }
        }

        // TODO: Move to seperate CommandHandler class
        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            // Don't process the command if it was a system message
            if (!(messageParam is SocketUserMessage message)) return;

            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;

            // Determine if the message is a command based on the prefix and make sure no bots trigger commands
            if (!(message.HasCharPrefix('!', ref argPos) ||
                message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
                return;

            Log.Debug("Message received from {Author}: {Content}", message.Author.Username, message.Content);
            // Create a WebSocket-based command context based on the message
            var context = new SocketCommandContext(_client, message);

            // Execute the command with the command context we just
            // created, along with the service provider for precondition checks.
            await _commandService.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: null);
        }

        /// <summary>
        /// <para>Passes trhrough DiscordSocketClient log messages to Serilog</para>
        /// <para>See <see href="https://docs.stillu.cc/guides/concepts/logging.html">Discord.Net documentation</see> for more details</para>
        /// </summary>
        /// <param name="arg">The Discord.NET LogMessage</param>
        /// <returns></returns>
        private Task LogHandler(LogMessage arg)
        {
            // TODO: differentiate between _client and _CommandService log messages
            var logger = Log.ForContext("Source", arg.Source);
            switch (arg.Severity)
            {
                case LogSeverity.Critical:
                    logger.Fatal(arg.Exception, arg.Message);
                    break;
                case LogSeverity.Error:
                    logger.Error(arg.Exception, arg.Message);
                    break;
                case LogSeverity.Warning:
                    logger.Fatal(arg.Exception, arg.Message);
                    break;
                case LogSeverity.Info:
                    logger.Information(arg.Message);
                    break;
                case LogSeverity.Verbose:
                    logger.Verbose(arg.Message);
                    break;
                case LogSeverity.Debug:
                    logger.Debug(arg.Message);
                    break;
            }
            return Task.CompletedTask;
        }
    }
}
