using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JustMyPackages.Hosting.Tests.WebHost.Api;
using JustMyPackages.Http.Rpc;
using Microsoft.AspNetCore.Mvc;

namespace JustMyPackages.Hosting.Tests.WebHost.Controllers
{
    [ApiController]
    [Route(Routes.ForecastRoute)]
    public class WeatherForecastController : ControllerBase, IForecastApi
    {
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        [HttpPost(Routes.GetRoute)]
        public Task<GetForecastResponse> GetAsync(RpcRequest request, CancellationToken token)
        {
            var data = Enumerable.Range(1, 10000).Select(index => new WeatherForecastDto
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                Certainty = (Certainty)Random.Shared.Next(0, 4)
            })
            .ToArray();

            var response = new GetForecastResponse
            {
                Data = data
            };

            return Task.FromResult(response);
        }
    }
}