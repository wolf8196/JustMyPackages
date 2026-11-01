using System.Diagnostics.CodeAnalysis;
using JustMyPackages.Http.Rpc.Tests.Infrastructure;
using Xunit;
using System;
using Microsoft.Extensions.Configuration;
using Serilog;
using WireMock.Server;
using WireMock.Settings;

[assembly: AssemblyFixture(typeof(GlobalFixture))]
[assembly: ExcludeFromCodeCoverage]

namespace JustMyPackages.Http.Rpc.Tests.Infrastructure
{
    public sealed class GlobalFixture : IDisposable
    {
        public const string ServiceName = "TestService";

        public GlobalFixture()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            Logger = LoggingExtensions.CreateLogger(configuration);
            Log.Logger = Logger;

            DestinationService = WireMockServer.Start(new WireMockServerSettings
            {
                Port = 1111,
                AllowBodyForAllHttpMethods = true,
                AllowOnlyDefinedHttpStatusCodeInResponse = false,
            });
        }

        public ILogger Logger { get; }

        public WireMockServer DestinationService { get; private set; }

        public void Dispose()
        {
            DestinationService.Stop();
            DestinationService.Dispose();
        }
    }
}
