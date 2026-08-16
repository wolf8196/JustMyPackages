using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog.Core;
using Serilog.Enrichers.Span;

namespace Serilog
{
    public static class LoggingExtensions
    {
        /// <summary>
        /// Create Serilog ILogger with my default settings.
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        /// <param name="applicationName">Application name to attach to logger.</param>
        /// <returns>Serilog ILogger.</returns>
        public static ILogger CreateLogger(IConfiguration configuration, string applicationName)
        {
            Logger logger = new LoggerConfiguration()
                .Initialize(configuration, applicationName)
                .CreateLogger();

            Log.Logger = logger;

            return logger;
        }

        /// <summary>
        /// Create Serilog ILogger with my default settings and calling assembly name.
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        /// <returns>Serilog ILogger.</returns>
        public static ILogger CreateLogger(IConfiguration configuration)
        {
            return CreateLogger(configuration, Assembly.GetCallingAssembly().GetName().Name ?? "Undefined assembly");
        }

        /// <summary>
        /// Add Serilog logging with my default settings to the host.
        /// </summary>
        /// <param name="host">Host builder.</param>
        /// <param name="applicationName">Application name to attach to logger.</param>
        /// <returns>The same host builder.</returns>
        public static IHostBuilder UseSerilog(this IHostBuilder host, string applicationName)
        {
            return host
                .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration.Initialize(hostingContext.Configuration, applicationName))
                .ConfigureServices((ctx, services) => services.AddGenericSerilog());
        }

        /// <summary>
        /// Add Serilog logging with my default settings and calling assembly name to the host.
        /// </summary>
        /// <param name="host">Host builder.</param>
        /// <returns>The same host builder.</returns>
        public static IHostBuilder UseSerilog(this IHostBuilder host)
        {
            return host.UseSerilog(Assembly.GetCallingAssembly().GetName().Name ?? "Undefined assembly");
        }

        /// <summary>
        /// Add generic Serilog ILogger interface as singleton into service collection.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <returns>The same service collection.</returns>
        public static IServiceCollection AddGenericSerilog(this IServiceCollection services)
        {
            services.AddSingleton(typeof(ILogger<>), typeof(GenericLogger<>));
            return services;
        }

        private static LoggerConfiguration Initialize(this LoggerConfiguration loggerConfiguration, IConfiguration configuration, string applicationName)
        {
            return loggerConfiguration
                .ReadFrom.Configuration(configuration)
                .Enrich.WithProperty("Application", applicationName)
                .Enrich.FromLogContext()
                .Enrich.WithSpan(new SpanOptions
                {
                    IncludeBaggage = true,
                    IncludeOperationName = true,
                });
        }

        /// <summary>
        /// Create generic Serilog ILogger from base Serilog ILogger.
        /// </summary>
        /// <typeparam name="T">Class that will be attached as SourceContext property.</typeparam>
        /// <param name="logger">Base Serilog ILogger.</param>
        /// <returns>Generic Serilog ILogger.</returns>
        public static ILogger<T> WithContext<T>(this ILogger logger)
        {
            return new GenericLogger<T>(logger);
        }
    }
}