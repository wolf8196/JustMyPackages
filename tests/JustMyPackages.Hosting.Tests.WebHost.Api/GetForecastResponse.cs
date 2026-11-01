using System.Collections.Generic;
using JustMyPackages.Http.Rpc;
using MessagePack;

namespace JustMyPackages.Hosting.Tests.WebHost.Api
{
    [MessagePackObject]
    public class GetForecastResponse : RpcResponse
    {
        [Key(2)]
        public IReadOnlyCollection<WeatherForecastDto> Data { get; set; } = [];
    }
}
