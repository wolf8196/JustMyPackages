using System.Runtime.Serialization;

namespace JustMyPackages.Http.Rpc
{
    /// <summary>
    /// Empty RPC request.
    /// </summary>
    [DataContract]
    public sealed class RpcRequest
    {
        /// <summary>
        /// Empty instance of <see cref="RpcRequest"/>.
        /// </summary>
        public static readonly RpcRequest Empty = new RpcRequest();
    }
}
