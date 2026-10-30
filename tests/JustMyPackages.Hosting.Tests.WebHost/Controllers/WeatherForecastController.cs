using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace JustMyPackages.Hosting.Tests.WebHost.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        [HttpGet]
        public IReadOnlyCollection<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 10000).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                Certainty = (Certainty)Random.Shared.Next(0, 3)
            })
            .ToArray();
        }
    }

    public class WeatherForecast
    {
        public required DateOnly Date { get; init; }

        public required int TemperatureC { get; init; }

        public required  string Summary { get; init; } = string.Empty;

        public required Certainty Certainty { get; init; }
    }

    public enum Certainty
    {
        Low,
        Medium,
        High
    }
}