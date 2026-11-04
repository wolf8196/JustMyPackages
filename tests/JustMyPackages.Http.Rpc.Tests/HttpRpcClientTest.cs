using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using JustMyPackages.Http.Rpc.Client;
using JustMyPackages.Http.Rpc.Serialization.MessagePack;
using JustMyPackages.Http.Rpc.Serialization.TextJson;
using JustMyPackages.Http.Rpc.Tests.Infrastructure;
using JustMyPackages.Serialization.MessagePack;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

namespace JustMyPackages.Http.Rpc.Tests
{
    public sealed class HttpRpcClientTest : IDisposable
    {
        private readonly GlobalFixture fixture;
        private readonly IServiceCollection services;
        private readonly WireMockServer destinationService;

        public HttpRpcClientTest(GlobalFixture fixture)
        {
            this.fixture = fixture;
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
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(expected));

            // Act
            var actual = await client.Test(request, TestContext.Current.CancellationToken);

            // Assert
            actual.Should().NotBeSameAs(expected);
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task HandlesSocketException()
        {
            // Arrange
            using var sp = services.BuildServiceProvider();
            var client = sp.GetRequiredService<ITestApi>();
            var expected = new TestResponse
            {
                IsSuccess = false,
                Error = "Failed to connect to http://localhost:1111/api/test of TestService service.",
            };
            var request = new TestRequest { Number = 1 };

            destinationService.Stop();

            // Act
            var actual = await client.Test(request, TestContext.Current.CancellationToken);

            // Assert
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
                Error = "Unsuccessful status code Unauthorized returned from http://localhost:1111/api/test of TestService service. Reason: Unauthorized",
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

        [Fact]
        public async Task HandlesException()
        {
            // Arrange
            var mock = new Mock<HttpMessageHandler>();
            mock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new Exception("Unhandled exception."));

            var services = new ServiceCollection()
                .AddLogging()
                .AddSingleton(fixture.Logger)
                .AddTextJsonRpcSerializer()
                .AddHttpRpcClient<ITestApi, TestApiClient>(
                    GlobalFixture.ServiceName,
                    sp => new HttpRpcClientSettings(destinationService.Url!))
                .ConfigurePrimaryHttpMessageHandler(() => mock.Object)
                .Services;

            using var sp = services.BuildServiceProvider();
            var client = sp.GetRequiredService<ITestApi>();
            var expected = new TestResponse
            {
                IsSuccess = false,
                Error = "Error occurred while requesting http://localhost:1111/api/test from TestService service. Error: Unhandled exception.",
            };
            var request = new TestRequest { Number = 1 };

            // Act
            var actual = await client.Test(request, TestContext.Current.CancellationToken);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }


        [Fact]
        public async Task HandlesNoSuitableSerializers()
        {
            // Arrange
            var services = new ServiceCollection()
                .AddLogging()
                .AddSingleton(fixture.Logger)
                .AddMessagePackRpcSerializer()
                .AddHttpRpcClient<ITestApi, TestApiClient>(
                    GlobalFixture.ServiceName,
                    sp => new HttpRpcClientSettings(destinationService.Url!, MediaTypes.MessagePack))
                .Services;

            using var sp = services.BuildServiceProvider();
            var client = sp.GetRequiredService<ITestApi>();
            var expected = new TestResponse
            {
                IsSuccess = false,
                Error = "Unexpected content type application/json returned from http://localhost:1111/api/test of TestService service. No suitable serializers found.",
            };
            var request = new TestRequest { Number = 1 };

            destinationService
                .Given(Request.Create()
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(new object()));

            // Act
            var actual = await client.Test(request, TestContext.Current.CancellationToken);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        public void Dispose()
        {
            destinationService.Reset();
        }
    }
}