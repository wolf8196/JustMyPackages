using System;
using JustMyPackages.Serialization.MessagePack;
using MessagePack;
using MessagePack.AspNetCoreMvcFormatter;
using Microsoft.Extensions.DependencyInjection;

namespace JustMyPackages.Http.Rpc.AspNetCore.MessagePack
{
    public static class MessagePackMvcBuilderExtensions
    {
        /// <summary>
        /// Adds MessagePack with my default settings as MVC serializer.
        /// </summary>
        /// <param name="builder">The MVC builder.</param>
        /// <param name="settingsOverrides">An optional action to override the default <see cref="MessagePackSerializerOptions"/>.</param>
        /// <returns>The same MVC builder.</returns>
        public static IMvcBuilder AddMyMessagePack(this IMvcBuilder builder, Action<MessagePackSerializerOptions>? settingsOverrides = null)
        {
            return builder.AddMvcOptions(option =>
            {
                var opts = DefaultMessagePackSerializerOptions.Create();
                option.OutputFormatters.Add(new MessagePackOutputFormatter(opts));
                option.InputFormatters.Add(new MessagePackInputFormatter(opts));
            });
        }
    }
}