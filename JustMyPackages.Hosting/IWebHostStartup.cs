using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JustMyPackages.Hosting
{
    /// <summary>
    /// Provides startup configuration hooks for a web host.
    /// </summary>
    public interface IWebHostStartup
    {
        /// <summary>
        /// Configure host builder and web host builder.
        /// </summary>
        /// <param name="host"><see cref="IHostBuilder"/> to configure.</param>
        /// <param name="webHost"><see cref="IWebHostBuilder"/> to configure.</param>
        void ConfigureHost(IHostBuilder host, IWebHostBuilder webHost);

        /// <summary>
        /// Configure application services.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> to populate.</param>
        void ConfigureServices(IServiceCollection services);

        /// <summary>
        /// Configure application builder.
        /// </summary>
        /// <param name="app"><see cref="IApplicationBuilder"/> to configure.</param>
        void ConfigureApplication(IApplicationBuilder app);
    }
}
