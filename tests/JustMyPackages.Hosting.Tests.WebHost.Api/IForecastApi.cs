using System.Threading;
using System.Threading.Tasks;
using JustMyPackages.Http.Rpc;

namespace JustMyPackages.Hosting.Tests.WebHost.Api
{
    public interface IForecastApi
    {
        Task<GetForecastResponse> GetAsync(RpcRequest request, CancellationToken token);
    }
}