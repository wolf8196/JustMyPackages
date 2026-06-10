using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace JustMyPackages.Hosting
{
    public static class HostCreator
    {
        /// <summary>
        /// Creates and builds web host using the provided web host startup.
        /// Performs my default application setup, builds web app, and invokes the startup
        /// callbacks to configure the host, services, and application pipeline.
        /// </summary>
        /// <param name="startup">The startup implementation that configures host, services, and application.</param>
        /// <param name="args">Command-line arguments.</param>
        /// <returns>The built <see cref="IHost"/> instance representing the application.</returns>
        public static IHost Create(IWebHostStartup startup, string[] args)
        {
            SetupApplication();

            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            startup.ConfigureHost(builder.Host, builder.WebHost);
            startup.ConfigureServices(builder.Services);

            WebApplication app = builder.Build();

            startup.ConfigureApplication(app);

            return app;
        }

        /// <summary>
        /// Creates and builds a generic host using the provided generic host startup.
        /// Performs my default application setup, builds host, and invokes the startup
        /// callbacks to configure the host and services.
        /// </summary>
        /// <param name="startup">The startup implementation that configures host and services.</param>
        /// <param name="args">Command-line arguments.</param>
        /// <returns>The built <see cref="IHost"/> instance.</returns>
        public static IHost Create(IGenericHostStartup startup, string[] args)
        {
            SetupApplication();

            IHostBuilder builder = Host.CreateDefaultBuilder(args);

            startup.ConfigureHost(builder);
            builder.ConfigureServices((ctx, services) => startup.ConfigureServices(services));

            IHost app = builder.Build();

            return app;
        }

        private static void SetupApplication()
        {
            ApplicationSetup.SetRootExceptionHandling();
            ApplicationSetup.SetRootDirectory();
            ApplicationSetup.SetupBootstrapLogger();
        }
    }
}
