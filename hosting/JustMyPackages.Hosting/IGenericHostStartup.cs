using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JustMyPackages.Hosting
{
    /// <summary>
    /// Provides startup configuration hooks for a generic host.
    /// </summary>
    public interface IGenericHostStartup
    {
        /// <summary>
        /// Configure host builder.
        /// </summary>
        /// <param name="host"><see cref="IHostBuilder"/> to configure.</param>
        void ConfigureHost(IHostBuilder host);

        /// <summary>
        /// Configure application services.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> to populate.</param>
        void ConfigureServices(IServiceCollection services);
    }
}
