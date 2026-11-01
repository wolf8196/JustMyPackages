using System;
using System.Threading;
using System.Threading.Tasks;
using JustMyPackages.Hosting.Tests.WebHost.Api;
using JustMyPackages.Http.Rpc;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace JustMyPackages.Hosting.Tests.GenericHost
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly IForecastApi forecastApi;

        public Worker(IForecastApi forecastApi, ILogger<Worker> logger)
        {
            this.forecastApi = forecastApi;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                logger.Information("Worker running at: {Time}", DateTimeOffset.Now);
                var forecast = await forecastApi.GetAsync(RpcRequest.Empty, stoppingToken);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
