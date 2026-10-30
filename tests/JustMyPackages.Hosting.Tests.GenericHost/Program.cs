using System.Threading.Tasks;

namespace JustMyPackages.Hosting.Tests.GenericHost
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await HostRunner.RunGenericHost(new Startup(), args, true);
        }
    }
}