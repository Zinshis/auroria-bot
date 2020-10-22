using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AuroriaBot
{
    public class Worker : BackgroundService
    {
        private readonly IHost _host;

        public Worker(IHost host)
        {
            _host = host;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // TODO: implement "stoppingToken.IsCancellationRequested"

            Log.Information("Starting main routine ...");
            await new Program().MainAsync(_host.Services);
        }
    }
}
