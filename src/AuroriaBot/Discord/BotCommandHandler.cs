using AuroriaBot.Configuration;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AuroriaBot.Discord
{
    public interface IBotCommandHandler
    {
        Task Initialize();
    }

    /// <summary>
    /// BotCommandHandler hooks up to the Discord websocket gateway with the provided token
    /// and provides commands to subscribed servers as they are defined in the entry assembly's 
    /// command modules.
    /// </summary>
    public class BotCommandHandler : IBotCommandHandler
    {
        private readonly ConfigurationOptions _options;
        private readonly IServiceProvider _serviceProvider;
        private DiscordSocketClient _client;
        private CommandService _commandService;

        public BotState State { get; private set; }


        public BotCommandHandler(IHost host,
            ConfigurationOptions options, 
            DiscordSocketClient client, 
            CommandService commandService)
        {
            _serviceProvider = host.Services;
            _options = options;
            _client = client;
            _commandService = commandService;
            State = BotState.Created;
        }


        /// <summary>
        /// Initializes the BotCommandHandler to connect to the Discord gateway and accept commands.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task Initialize()
        {
            if (State != BotState.Created)
            {
                throw new InvalidOperationException("Discord bot client has already been initialized.");
            }

            _client.Log += LogHandler;
            _commandService.Log += LogHandler;

            await _client.LoginAsync(TokenType.Bot, _options.Token);
            await _client.StartAsync(); // TODO => https://docs.stillu.cc/guides/concepts/connections.html

            State = BotState.Initialized;

            _client.MessageReceived += MessageReceivedAsync;
            _commandService.CommandExecuted += CommandExecutedAsync;

            // Add all Command modules added in this assembly
            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);
        }


        /// <summary>
        /// Processes each message to see if it qualifies as a command.
        /// </summary>
        /// <param name="messageParam"></param>
        /// <returns></returns>
        private async Task MessageReceivedAsync(SocketMessage messageParam)
        {
            // Don't process the command if it was a system message
            if (!(messageParam is SocketUserMessage message))
            {
                return;
            }

            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;

            // Determine if the message is a command based on the prefix and make sure no bots trigger commands
            if (!(message.HasStringPrefix(_options.DefaultPrefix, ref argPos) ||
                message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
            {
                return;
            }

            Log.Debug("Message received from {Author}: {Content}", message.Author.Username, message.Content);

            // Create a WebSocket-based command context based on the message
            var context = new SocketCommandContext(_client, message);

            // Execute the command with the command context we just
            // created, along with the service provider for precondition checks.
            await _commandService.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: _serviceProvider);
        }

        /// <summary>
        /// Handles post-command-execution logic.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="context"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // Command was not found
            if (!command.IsSpecified)
            {
                Log.Debug("Command {Command} was not recognized", context.Message.Content);
                return;
            }

            // Command was successful
            if (result.IsSuccess)
            {
                Log.Debug("Command {Command} was succesful", command.Value.Name);
                return;
            }

            // Command failed
            Log.Warning("Command {Command} failed", command.Value.Name);
            await context.Channel.SendMessageAsync($"error: {result}");
        }

        /// <summary>
        /// <para>Passes through DiscordSocketClient log messages to Serilog.</para>
        /// <para>See <see href="https://docs.stillu.cc/guides/concepts/logging.html">Discord.Net documentation</see> for more details.</para>
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
