using System.Threading;
using System.Threading.Tasks;
using Flurl;
using JustMyPackages.Http.Rpc;
using JustMyPackages.Http.Rpc.Client;

namespace JustMyPackages.Hosting.Tests.WebHost.Api.Client
{
    internal class ForecastApiClient : HttpRpcClient, IForecastApi
    {
        public ForecastApiClient(HttpRpcClientParameters parameters)
            : base(parameters)
        {
        }

        public Task<GetForecastResponse> GetAsync(RpcRequest request, CancellationToken token)
        {
            return Send<RpcRequest, GetForecastResponse>(request, Url.Combine(Routes.ForecastRoute, Routes.GetRoute), token);
        }
    }
}
