using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using JustMyPackages.Http.Rpc.Client;
using JustMyPackages.Http.Rpc.ExceptionHandling;
using JustMyPackages.Http.Rpc.Serialization.MessagePack;
using JustMyPackages.Http.Rpc.Serialization.NewtonsoftJson;
using JustMyPackages.Http.Rpc.Tests.Infrastructure;
using JustMyPackages.Serialization.MessagePack;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using Xunit;

namespace JustMyPackages.Http.Rpc.Tests
{
    public sealed class HttpRpcExceptionHandlerTest
    {
        private readonly GlobalFixture fixture;

        public HttpRpcExceptionHandlerTest(GlobalFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task HandlesExceptions()
        {
            var expected = new TestResponse { IsSuccess = false, Error = "Test exception." };
            using var host = await new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseTestServer()
                        .ConfigureServices(services =>
                        {
                            services.AddNewtonsoftJsonRpcSerializer();
                        })
                        .Configure(app =>
                        {
                            app.UseHttpRpcExceptionHandler();
                            app.Run(ctx =>
                            {
                                throw new Exception(expected.Error);
                            });
                        });
                })
                .StartAsync(cancellationToken: TestContext.Current.CancellationToken);

            var services = new ServiceCollection()
                .AddLogging()
                .AddSingleton(fixture.Logger)
                .AddNewtonsoftJsonRpcSerializer()
                .AddSingleton(sp =>
                {
                    var mock = new Mock<IHttpClientFactory>();
                    mock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(host.GetTestClient());
                    return mock.Object;
                })
                .AddHttpRpcClient<ITestApi, TestApiClient>(
                    GlobalFixture.ServiceName,
                    sp => new HttpRpcClientSettings(fixture.DestinationService.Url!))
                .Services;
            using var sp = services.BuildServiceProvider();

            var actual = await sp.GetRequiredService<ITestApi>().Test(new TestRequest(), TestContext.Current.CancellationToken);
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task FallsBackToJsonWhenMediaTypeIsUnknown()
        {
            var expected = new TestResponse { IsSuccess = false, Error = "Test exception." };
            using var host = await new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseTestServer()
                        .ConfigureServices(services =>
                        {
                            services.AddNewtonsoftJsonRpcSerializer();
                            services.AddLogging();
                        })
                        .Configure(app =>
                        {
                            app.UseHttpRpcExceptionHandler();
                            app.Run(ctx =>
                            {
                                throw new Exception(expected.Error);
                            });
                        });
                })
                .StartAsync(cancellationToken: TestContext.Current.CancellationToken);

            var services = new ServiceCollection()
                .AddLogging()
                .AddSingleton(fixture.Logger)
                .AddMessagePackRpcSerializer()
                .AddNewtonsoftJsonRpcSerializer()
                .AddSingleton(sp =>
                {
                    var mock = new Mock<IHttpClientFactory>();
                    mock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(host.GetTestClient());
                    return mock.Object;
                })
                .AddHttpRpcClient<ITestApi, TestApiClient>(
                    GlobalFixture.ServiceName,
                    sp => new HttpRpcClientSettings(fixture.DestinationService.Url!, MediaTypes.MessagePack))
                .Services;
            using var sp = services.BuildServiceProvider();

            var actual = await sp.GetRequiredService<ITestApi>().Test(new TestRequest(), TestContext.Current.CancellationToken);
            actual.Should().BeEquivalentTo(expected);
        }
    }
}