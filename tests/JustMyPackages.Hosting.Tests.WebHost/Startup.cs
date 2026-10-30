using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JustMyPackages.Hosting.Tests.WebHost
{
    internal sealed class Startup : IWebHostStartup
    {
        public void ConfigureHost(IHostBuilder host, IWebHostBuilder webHost)
        {
            host.ConfigureMyHost();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMyResponseCompression();
            services.AddMySwagger();
            services.AddControllers();
        }

        public void ConfigureApplication(IApplicationBuilder app)
        {
            app.UseMySwagger();
            app.UseResponseCompression();
            app.UseRouting();
            app.UseControllers();
        }
    }
}