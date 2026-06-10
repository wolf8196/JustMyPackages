using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace JustMyPackages.Hosting
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Add endpoint routing for controllers to the application's request pipeline.
        /// </summary>
        /// <param name="app">Application builder to configure.</param>
        /// <returns>The same <see cref="IApplicationBuilder"/> instance so additional calls can be chained.</returns>
        /// <remarks>This is a small convenience extension that maps controller routes using UseEndpoints.</remarks>
        public static IApplicationBuilder UseControllers(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints => endpoints.MapControllers());
            return app;
        }

        /// <summary>
        /// Add Swagger and the Swagger UI to the application's request pipeline.
        /// </summary>
        /// <param name="app">Application builder to configure.</param>
        /// <param name="setupAction">Optional action to configure <see cref="SwaggerUIOptions"/> before the endpoint is added.</param>
        /// <param name="assembly">Optional assembly name used in the Swagger UI title. If <c>null</c>, the calling assembly's name is used.</param>
        /// <returns>The same <see cref="IApplicationBuilder"/> instance so additional calls can be chained.</returns>
        public static IApplicationBuilder UseMySwagger(
            this IApplicationBuilder app,
            Action<SwaggerUIOptions>? setupAction = null,
            string? assembly = null)
        {
            assembly ??= Assembly.GetCallingAssembly().GetName().Name ?? "Undefined assembly";

            app.UseSwagger();
            app.UseSwaggerUI(opts =>
            {
                setupAction?.Invoke(opts);
                opts.SwaggerEndpoint("/swagger/api/swagger.json", $"{assembly} API");
            });

            return app;
        }
    }
}
