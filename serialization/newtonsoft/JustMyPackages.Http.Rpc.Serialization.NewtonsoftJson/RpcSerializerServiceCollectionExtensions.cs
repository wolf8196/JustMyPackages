using System;
using JustMyPackages.Serialization.NewtonsoftJson;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IO;
using Newtonsoft.Json;

namespace JustMyPackages.Http.Rpc.Serialization.NewtonsoftJson
{
    public static class RpcSerializerServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Newtonsoft.Json-based RPC serializer with my default settings to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="settingsOverrides">An optional action to override the default <see cref="JsonSerializerSettings"/>.</param>
        /// <returns>The same service collection.</returns>
        public static IServiceCollection AddNewtonsoftJsonRpcSerializer(this IServiceCollection services, Action<JsonSerializerSettings>? settingsOverrides = null)
        {
            services.TryAddSingleton<RecyclableMemoryStreamManager>();
            services.TryAddSingleton<IRpcSerializer>(sp =>
            {
                var settings = DefaultJsonSerializerSettings.Create();
                settingsOverrides?.Invoke(settings);
                return new NewtonsoftRpcSerializer(settings, sp.GetRequiredService<RecyclableMemoryStreamManager>());
            });

            return services;
        }
    }
}
