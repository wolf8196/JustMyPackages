using System.Threading;
using System.Threading.Tasks;

namespace JustMyPackages.Http.Rpc.Tests.Infrastructure
{
    internal interface ITestApi
    {
        Task<TestResponse> Test(TestRequest request, CancellationToken token);
    }
}
