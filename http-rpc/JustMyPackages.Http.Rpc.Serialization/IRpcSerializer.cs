using System.IO;

namespace JustMyPackages.Http.Rpc.Serialization
{
    public interface IRpcSerializer
    {
        string MediaType { get; }

        Stream Serialize<T>(T obj);

        T Deserialize<T>(Stream stream);
    }
}
