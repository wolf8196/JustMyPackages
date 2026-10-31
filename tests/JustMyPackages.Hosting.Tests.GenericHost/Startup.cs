using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JustMyPackages.Hosting.Tests.GenericHost
{
    internal sealed class Startup : IGenericHostStartup
    {
        public void ConfigureHost(IHostBuilder host)
        {
            host.ConfigureMyHost();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<Worker>();
        }
    }
}