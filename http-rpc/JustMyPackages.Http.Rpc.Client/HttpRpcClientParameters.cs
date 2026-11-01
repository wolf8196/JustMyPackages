using System.Collections.Generic;
using System.Net.Http;
using JustMyPackages.Http.Rpc.Serialization;
using Microsoft.Extensions.Logging;

namespace JustMyPackages.Http.Rpc.Client
{
    public class HttpRpcClientParameters
    {
        public required string TargetName { get; init; }

        public required string ClientName { get; init; }

        public required HttpRpcClientSettings ClientSettings { get; init; }

        public required HttpClient HttpClient { get; init; }

        public required IEnumerable<IRpcSerializer> Serilaizers { get; init; }

        public required ILogger Logger { get; init; }
    }
}
