using MessagePack;

namespace JustMyPackages.Http.Rpc.Tests.Infrastructure
{
    [MessagePackObject(AllowPrivate = true)]
    internal sealed class TestRequest
    {
        [Key(0)]
        public int Number { get; init; }
    }
}
