using System.Runtime.Serialization;

namespace JustMyPackages.Http.Rpc
{
    /// <summary>
    /// Empty RPC request.
    /// </summary>
    [DataContract]
    public sealed class RpcApiRequest
    {
        /// <summary>
        /// Empty instance of <see cref="RpcApiRequest"/>.
        /// </summary>
        public static readonly RpcApiRequest Empty = new RpcApiRequest();
    }
}
