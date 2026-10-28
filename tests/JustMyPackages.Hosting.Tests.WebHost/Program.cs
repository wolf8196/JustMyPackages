using System.Threading.Tasks;

namespace JustMyPackages.Hosting.Tests.WebHost
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            await HostRunner.RunWebHost(new Startup(), args, true);
        }
    }
}