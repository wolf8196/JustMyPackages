using System;
using JustMyPackages.Serialization.NewtonsoftJson;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace JustMyPackages.Http.Rpc.AspNetCore.NewtonsoftJson
{
    public static class NewtonsoftJsonMvcBuilderExtensions
    {
        /// <summary>
        /// Adds Newtonsoft.Json with my default settings as MVC serializer.
        /// </summary>
        /// <param name="builder">The MVC builder.</param>
        /// <param name="settingsOverrides">An optional action to override the default <see cref="JsonSerializerSettings"/>.</param>
        /// <returns>The same MVC builder.</returns>
        public static IMvcBuilder AddMyNewtonsoftJson(this IMvcBuilder builder, Action<JsonSerializerSettings>? settingsOverrides = null)
        {
            return builder.AddNewtonsoftJson(opts =>
            {
                opts.ReadJsonWithRequestCulture = false;

                DefaultJsonSerializerSettings.Configure(opts.SerializerSettings);
                settingsOverrides?.Invoke(opts.SerializerSettings);
            });
        }
    }
}