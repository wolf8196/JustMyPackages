using System;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using JustMyPackages.Http.Rpc.Serialization;
using System.Linq;
using System.Net.Mime;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace JustMyPackages.Http.Rpc.ExceptionHandling
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseHttpRpcExceptionHandler(this IApplicationBuilder appBuilder)
        {
            return appBuilder.UseHttpRpcExceptionHandler(
                ex => new RpcResponse { IsSuccess = false, Error = ex?.Message ?? "Unknown error." });
        }

        public static IApplicationBuilder UseHttpRpcExceptionHandler(
            this IApplicationBuilder appBuilder,
            Func<Exception?, RpcResponse> responseFactory)
        {
            return appBuilder.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandler = async context =>
                {
                    var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                    var serializers = context.RequestServices.GetServices<IRpcSerializer>();
                    var acceptableSerializer = serializers.IntersectBy(context.Request.Headers.Accept, x => x.MediaType).FirstOrDefault();
                    var response = responseFactory(exception);

                    if (acceptableSerializer is null)
                    {
                        var logger = context.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("HttpRpcExceptionHandler");
                        logger.LogWarning(
                            "No serializer found to handle exception response. Falling back to json. Accept header: {AcceptHeader}, Supported media types: {SupportedMediaTypes}",
                            context.Request.Headers.Accept,
                            serializers.Select(x => x.MediaType));

                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                        context.Response.ContentType = MediaTypeNames.Application.Json;
                        await context.Response.WriteAsJsonAsync(response);
                        return;
                    }

                    context.Response.ContentType = acceptableSerializer.MediaType;
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    using var bodyStream = acceptableSerializer.Serialize(response);
                    await bodyStream.CopyToAsync(context.Response.Body, context.RequestAborted);
                },
            });
        }
    }
}
