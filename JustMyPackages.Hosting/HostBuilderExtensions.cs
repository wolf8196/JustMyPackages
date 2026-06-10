using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace JustMyPackages.Hosting
{
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Configure my default host settings.
        /// </summary>
        /// <param name="host"><see cref="IHostBuilder"/> to configure.</param>
        /// <returns>The same <see cref="IHostBuilder"/> instance.</returns>
        public static IHostBuilder ConfigureMyHost(this IHostBuilder host)
        {
            string assembly = Assembly.GetCallingAssembly().GetName().Name ?? "Undefined assembly";

            host
                .UseSerilog(assembly)
                .UseDefaultServiceProvider(configure =>
                {
                    configure.ValidateOnBuild = true;
                    configure.ValidateScopes = true;
                });

            return host;
        }
    }
}