using System;
using System.Text.Json;
using JustMyPackages.Serialization.TextJson;
using Microsoft.Extensions.DependencyInjection;

namespace JustMyPackages.Http.Rpc.AspNetCore.TextJson
{
    public static class NewtonsoftJsonRpcMvcBuilderExtensions
    {
        /// <summary>
        /// Adds System.Text.Json with my default settings as MVC serializer.
        /// </summary>
        /// <param name="builder">The MVC builder.</param>
        /// <param name="optsOverrides">An optional action to override the default <see cref="JsonSerializerOptions"/>.</param>
        /// <returns>The same MVC builder.</returns>
        public static IMvcBuilder AddMyTextJson(this IMvcBuilder builder, Action<JsonSerializerOptions>? optsOverrides = null)
        {
            return builder.AddJsonOptions(opts =>
            {
                DefaultJsonSerializerOptions.Configure(opts.JsonSerializerOptions);
                optsOverrides?.Invoke(opts.JsonSerializerOptions);
            });
        }
    }
}