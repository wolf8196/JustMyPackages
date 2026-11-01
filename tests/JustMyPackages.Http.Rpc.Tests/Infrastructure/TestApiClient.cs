using System.Threading;
using System.Threading.Tasks;
using JustMyPackages.Http.Rpc.Client;

namespace JustMyPackages.Http.Rpc.Tests.Infrastructure
{
    internal class TestApiClient : HttpRpcClient, ITestApi
    {
        public TestApiClient(HttpRpcClientParameters parameters)
            : base(parameters)
        {
        }

        public Task<TestResponse> Test(TestRequest request, CancellationToken token)
        {
            return Send<TestRequest, TestResponse>(request, "api/test", token);
        }
    }
}