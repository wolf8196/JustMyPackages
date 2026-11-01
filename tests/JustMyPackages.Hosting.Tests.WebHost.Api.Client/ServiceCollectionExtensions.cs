using System;
using JustMyPackages.Http.Rpc.Client;
using Microsoft.Extensions.DependencyInjection;

namespace JustMyPackages.Hosting.Tests.WebHost.Api.Client
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddForecaseApiClient(this IServiceCollection services, Func<IServiceProvider, HttpRpcClientSettings> config)
        {
            services.AddHttpRpcClient<IForecastApi, ForecastApiClient>("WebHost", config);
            return services;
        }
    }
}