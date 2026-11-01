using System;
using JustMyPackages.Serialization.MessagePack;
using MessagePack;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IO;

namespace JustMyPackages.Http.Rpc.Serialization.MessagePack
{
    public static class RpcSerializerServiceCollectionExtensions
    {
        /// <summary>
        /// Adds MessagePack RPC serializer with my default settings to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="optsOverrides">An optional action to override the default <see cref="MessagePackSerializerOptions"/>.</param>
        /// <returns>The same service collection.</returns>
        public static IServiceCollection AddMessagePackRpcSerializer(this IServiceCollection services, Action<MessagePackSerializerOptions>? optsOverrides = null)
        {
            services.TryAddSingleton<RecyclableMemoryStreamManager>();
            services.TryAddSingleton<IRpcSerializer>(sp =>
            {
                var opts = DefaultMessagePackSerializerOptions.Create();
                optsOverrides?.Invoke(opts);
                return new MessagePackRpcSerializer(opts, sp.GetRequiredService<RecyclableMemoryStreamManager>());
            });
            return services;
        }
    }
}