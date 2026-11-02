using System;
using MessagePack;

namespace JustMyPackages.Hosting.Tests.WebHost.Api
{
    [MessagePackObject]
    public class WeatherForecastDto
    {
        [Key(0)]
        public DateOnly Date { get; set; }

        [Key(1)]
        public int TemperatureC { get; set; }

        [Key(2)]
        public string Summary { get; set; } = string.Empty;

        [Key(3)]
        public Certainty Certainty { get; set; }
    }
}
