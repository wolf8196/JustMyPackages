using System.Net.Mime;
using JustMyPackages.Hosting.Tests.WebHost.Api.Client;
using JustMyPackages.Http.Rpc.Client;
using JustMyPackages.Http.Rpc.Serialization.MessagePack;
using JustMyPackages.Http.Rpc.Serialization.TextJson;
using JustMyPackages.Serialization.MessagePack;
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
            services.AddTextJsonRpcSerializer();
            services.AddMessagePackRpcSerializer();
            services.AddForecaseApiClient(sp => new HttpRpcClientSettings("http://localhost:1234", MediaTypes.MessagePack));
        }
    }
}