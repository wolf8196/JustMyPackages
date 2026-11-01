using System;
using MessagePack;

namespace JustMyPackages.Hosting.Tests.WebHost.Api
{
    [MessagePackObject]
    public class WeatherForecastDto
    {
        [Key(0)]
        public required DateOnly Date { get; init; }

        [Key(1)]
        public required int TemperatureC { get; init; }

        [Key(2)]
        public required string Summary { get; init; }

        [Key(3)]
        public required Certainty Certainty { get; init; }
    }
}
