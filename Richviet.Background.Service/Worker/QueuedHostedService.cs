using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Richviet.Background.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Richviet.Background.Service.Worker
{
    public class QueuedHostedService : BackgroundService
    {
        private readonly ILogger logger;

        public QueuedHostedService(ILogger<QueuedHostedService> logger)
        {
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {

                    await Task.Delay(Config.SLEEP_TIME);
                }
                catch (Exception ex)
                {
                    logger.LogError("QueuedHostedService ExecuteAsync:" + ex.Message);
                }
            }
        }
    }
}
