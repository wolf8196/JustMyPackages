using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using JustMyPackages.Http.Rpc.Client;
using JustMyPackages.Http.Rpc.Serialization.TextJson;
using JustMyPackages.Http.Rpc.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

namespace JustMyPackages.Http.Rpc.Tests
{
    public sealed class HttpRpcClientTest : IDisposable
    {
        private readonly IServiceCollection services;
        private readonly WireMockServer destinationService;

        public HttpRpcClientTest(GlobalFixture fixture)
        {
            destinationService = fixture.DestinationService;

            services = new ServiceCollection()
                .AddLogging()
                .AddSingleton(fixture.Logger)
                .AddTextJsonRpcSerializer()
                .AddHttpRpcClient<ITestApi, TestApiClient>(
                    GlobalFixture.ServiceName,
                    sp => new HttpRpcClientSettings(destinationService.Url!))
                .Services;
        }

        [Fact]
        public async Task ExecutesRpcOperationSuccessfully()
        {
            // Arrange
            using var sp = services.BuildServiceProvider();
            var client = sp.GetRequiredService<ITestApi>();
            var expected = new TestResponse { IsSuccess = true, Number = 1 };
            var request = new TestRequest { Number = expected.Number };

            destinationService
                .Given(Request.Create()
                    .UsingPost()
                    .WithBody(new JsonMatcher(MatchBehaviour.AcceptOnMatch, request, true)))
                .RespondWith(Response.Create()
                    .WithBodyAsJson(expected));

            // Act
            var actual = await client.Test(request, TestContext.Current.CancellationToken);

            // Assert
            actual.Should().NotBeSameAs(expected);
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task HandlesUnsuccessfulStatusCode()
        {
            // Arrange
            using var sp = services.BuildServiceProvider();
            var client = sp.GetRequiredService<ITestApi>();
            var expected = new TestResponse
            {
                IsSuccess = false,
                Error = "Unsuccessful Unauthorized status code returned from http://localhost:1111/api/test of TestService service. Reason: Unauthorized",
            };
            var request = new TestRequest { Number = 1 };

            destinationService
                .Given(Request.Create()
                    .UsingPost()
                    .WithBody(new JsonMatcher(MatchBehaviour.AcceptOnMatch, request, true)))
                .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.Unauthorized));

            // Act
            var actual = await client.Test(request, TestContext.Current.CancellationToken);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task HandlesRequestCancellation()
        {
            // Arrange
            using var sp = services.BuildServiceProvider();
            var client = sp.GetRequiredService<ITestApi>();
            var expected = new TestResponse
            {
                IsSuccess = false,
                Error = "Request to http://localhost:1111/api/test of TestService service was cancelled.",
            };
            var request = new TestRequest { Number = 1 };
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));

            destinationService
                .Given(Request.Create()
                    .UsingPost()
                    .WithBody(new JsonMatcher(MatchBehaviour.AcceptOnMatch, request, true)))
                .RespondWith(Response.Create().WithDelay(TimeSpan.FromSeconds(5)));

            // Act
            var actual = await client.Test(request, cts.Token);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        public void Dispose()
        {
            destinationService.Reset();
        }
    }
}