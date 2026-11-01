using System;
using System.Linq;
using JustMyPackages.Http.Rpc.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace JustMyPackages.Http.Rpc.Client
{
    public static class HttpRpcServiceCollectionExtensions
    {
        public static IHttpClientBuilder AddHttpRpcClient<TClient, TImplementation>(
            this IServiceCollection services,
            string targetName,
            Func<IServiceProvider, HttpRpcClientSettings> config)
            where TClient : class
            where TImplementation : class, TClient
        {
            var clientName = $"{targetName}_{typeof(TClient).Name}";
            return services.AddHttpClient<TClient, TImplementation>(
                clientName,
                (httpClient, sp) =>
                {
                    var opts = config(sp);
                    httpClient.BaseAddress = new Uri(opts.Url);
                    httpClient.Timeout = opts.Timeout;
                    var serializers = sp.GetServices<IRpcSerializer>();

                    if (!serializers.Any())
                    {
                        throw new ArgumentException("No serializers of type IRpcSerializer registered.");
                    }

                    var parameters = new HttpRpcClientParameters
                    {
                        TargetName = targetName,
                        ClientName = clientName,
                        ClientSettings = opts,
                        HttpClient = httpClient,
                        Serilaizers = sp.GetServices<IRpcSerializer>(),
                        Logger = sp.GetRequiredService<ILogger<TImplementation>>()
                    };

                    return ActivatorUtilities.CreateInstance<TImplementation>(sp, parameters);
                });
        }
    }
}