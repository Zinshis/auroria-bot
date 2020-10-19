using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuroriaBot.Configuration
{
    /// <summary>
    /// <para>The ApplicationHostBuilder creates host where:</para>
    /// <para>- Configuration options are read from the "appsettings.{Environment}.json" file and stored in the DI service container</para>
    /// <para>- Logging is set up via Serilog and the Console sink, specific log configurations are read from the Configuration options</para>
    /// <para>- Adds additional services to the DI service container, such as: ...</para>
    /// </summary>
    public static class ApplicationHostBuilder
    {
        public static void ConfigureStartupLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var configurationOptions = new ConfigurationOptions();

            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, configuration) =>
                {
                    // Remove default sources and add only "appsettings.json" files to configuration
                    configuration.Sources.Clear();

                    IHostEnvironment env = hostingContext.HostingEnvironment;

                    configuration
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

                    IConfigurationRoot configurationRoot = configuration.Build();

                    configurationRoot.Bind(configurationOptions);
                })
                .UseSerilog((hostingContext, services, loggerConfiguration) =>
                {
                    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
                })
                .ConfigureServices((_, services) => 
                {
                    services.AddSingleton(configurationOptions);
                });
        }
    }
}
