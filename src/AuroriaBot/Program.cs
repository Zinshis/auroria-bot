using AuroriaBot.Configuration;
using AuroriaBot.Discord;
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

        public static void Main(string[] args) 
        {
            ApplicationHostBuilder.ConfigureStartupLogger();

            Log.Information("Configuring Host ...");
            _host = ApplicationHostBuilder.CreateHostBuilder(args).Build();
            _host.RunAsync(); // Run async so thread is not blocked
            Log.Information("Host configured.");

            var serviceProvider = _host.Services.CreateScope().ServiceProvider;

            Log.Information("Starting main routine ...");
            new Program().MainAsync(serviceProvider).GetAwaiter().GetResult();
        }

        public async Task MainAsync(IServiceProvider serviceProvider)
        {
            try
            {
                var bot = serviceProvider.GetService<IDiscordBotClient>();
                await bot.Initialize();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unexpected error occured initializing the bot.");
                return;
            }

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }
    }
}
