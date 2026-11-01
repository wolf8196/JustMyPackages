using System;
using System.Text.Json;
using JustMyPackages.Serialization.TextJson;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IO;

namespace JustMyPackages.Http.Rpc.Serialization.TextJson
{
    public static class RpcSerializerServiceCollectionExtensions
    {
        /// <summary>
        /// Adds System.Text.Json RPC serializer to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="optsOverrides">An optional action to override the default <see cref="JsonSerializerOptions"/>.</param>
        /// <returns>The same service collection.</returns>
        public static IServiceCollection AddTextJsonRpcSerializer(this IServiceCollection services, Action<JsonSerializerOptions>? optsOverrides = null)
        {
            services.TryAddSingleton<RecyclableMemoryStreamManager>();
            services.TryAddSingleton<IRpcSerializer>(sp =>
            {
                var opts = DefaultJsonSerializerOptions.Create();
                optsOverrides?.Invoke(opts);
                opts.MakeReadOnly();
                return new TextJsonRpcSerializer(opts, sp.GetRequiredService<RecyclableMemoryStreamManager>());
            });
            return services;
        }
    }
}
