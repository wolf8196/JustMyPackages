using System;
using System.IO.Compression;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;

namespace JustMyPackages.Hosting
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add response compression services configured to use Gzip with the specified compression level.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="compressionLevel"><see cref="CompressionLevel"/> to use for Gzip compression. Defaults to <see cref="CompressionLevel.Optimal"/>.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance.</returns>
        public static IServiceCollection AddMyResponseCompression(
            this IServiceCollection services,
            CompressionLevel compressionLevel = CompressionLevel.Optimal)
        {
            services.Configure<GzipCompressionProviderOptions>(options =>
                options.Level = compressionLevel);
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
            });

            return services;
        }

        /// <summary>
        /// Add and configure Swagger generation services.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="setupAction">Optional callback to further configure <see cref="SwaggerGenOptions"/> with access to an <see cref="IServiceProvider"/>.</param>
        /// <param name="assembly">Optional assembly name used as the API title. If <c>null</c>, the calling assembly's name is used.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance.</returns>
        public static IServiceCollection AddMySwagger(
            this IServiceCollection services,
            Action<SwaggerGenOptions, IServiceProvider>? setupAction = null,
            string? assembly = null)
        {
            assembly ??= Assembly.GetCallingAssembly().GetName().Name;

            services.AddSwaggerGen();
            services.ConfigureOptions<SwaggerGenOptions>((opts, sp) =>
            {
                setupAction?.Invoke(opts, sp);
                opts.SwaggerDoc("api", new OpenApiInfo { Title = $"{assembly} API", Version = "v1" });
                opts.CustomSchemaIds(type => type.ToString());
                opts.AddEnumsWithValuesFixFilters();
            });

            return services;
        }

        /// <summary>
        /// Register and configure named options instances using an <see cref="IServiceProvider"/> during configuration.
        /// </summary>
        /// <typeparam name="TOptions">The options type to register.</typeparam>
        /// <param name="services"><see cref="IServiceCollection"/> to add the options configuration to.</param>
        /// <param name="configureAction">Action used to configure the options instance.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance.</returns>
        public static IServiceCollection ConfigureOptions<TOptions>(this IServiceCollection services, Action<TOptions, IServiceProvider> configureAction)
            where TOptions : class
        {
            services.AddOptions<TOptions>().Configure<IServiceProvider>((opts, sp) => configureAction(opts, sp));
            return services;
        }
    }
}
