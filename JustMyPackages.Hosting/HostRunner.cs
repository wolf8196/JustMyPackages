using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JustMyPackages.Hosting
{
    public static class HostRunner
    {
        /// <summary>
        /// Creates and runs web host using the provided <see cref="IWebHostStartup"/>.
        /// </summary>
        /// <param name="startup">The web host startup implementation that configures the application.</param>
        /// <param name="args">Command-line arguments.</param>
        /// <param name="restartInDevMode">If true, restarts the host automatically when running in the Development environment.</param>
        /// <returns><see cref="Task"/> that completes when the host has stopped running.</returns>
        public static Task RunWebHost(IWebHostStartup startup, string[] args, bool restartInDevMode = true)
        {
            return RunHost(() => HostCreator.Create(startup, args), restartInDevMode);
        }

        /// <summary>
        /// Creates and runs generic host using the provided <see cref="IGenericHostStartup"/>.
        /// </summary>
        /// <param name="startup">The generic host startup implementation that configures the host and services.</param>
        /// <param name="args">Command-line arguments.</param>
        /// <param name="restartInDevMode">If true, restarts the host automatically when running in the Development environment.</param>
        /// <returns>see cref="Task"/> that completes when the host has stopped running.</returns>
        public static Task RunGenericHost(IGenericHostStartup startup, string[] args, bool restartInDevMode = true)
        {
            return RunHost(() => HostCreator.Create(startup, args), restartInDevMode);
        }

        private static async Task RunHost(Func<IHost> hostFactory, bool restartInDevMode)
        {
            IHostEnvironment? env;
            do
            {
                IHost app = hostFactory();
                env = app.Services.GetService<IHostEnvironment>();
                await app.RunAsync();
            }
            while (restartInDevMode && env?.IsDevelopment() == true);
        }
    }
}
