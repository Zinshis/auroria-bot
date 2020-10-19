using AuroriaBot.Configuration;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Threading.Tasks;

namespace AuroriaBot
{
    class Program
    {
        private static IHost _host;
        private DiscordSocketClient _client;

        public static void Main(string[] args) 
        {
            ApplicationHostBuilder.ConfigureStartupLogger();

            Log.Information("Configuring Host ...");
            _host = ApplicationHostBuilder.CreateHostBuilder(args).Build();
            _host.RunAsync(); // Run async so thread is not blocked
            Log.Information("Host configured");

            var serviceProvider = _host.Services.CreateScope().ServiceProvider;

            Log.Information("Starting main routine ...");
            new Program().MainAsync(serviceProvider).GetAwaiter().GetResult();
        }

        public async Task MainAsync(IServiceProvider serviceProvider)
        {
            _client = new DiscordSocketClient();

            //_client.Log += Log;

            var token = serviceProvider.GetRequiredService<ConfigurationOptions>().Token;

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }
    }
}
