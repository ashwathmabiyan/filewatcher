using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileWatch.Server
{
    class WatcherService : BackgroundService
    {
        private static IHubContext<FileHub> _hubContext { get; set; }
        public WatcherService(IHubContext<FileHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            if (AppConfiguration.DirectoryToMonitor == null)
                 StopAsync(cancellationToken).ConfigureAwait(false);         

            return base.StartAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            FileWatch fileWatch = new FileWatch(_hubContext);
            fileWatch.InitFileWatcher(AppConfiguration.DirectoryToMonitor, AppConfiguration.FileTypeToMonitor);

            while (!stoppingToken.IsCancellationRequested)
                await Task.Delay(60 * 1000, stoppingToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
        }

    }
}
