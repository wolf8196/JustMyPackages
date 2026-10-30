using System.Runtime.Serialization;

namespace JustMyPackages.Http.Rpc
{
    /// <summary>
    /// Base RPC response.
    /// </summary>
    [DataContract]
    public class RpcApiResponse
    {
        /// <summary>
        /// Value indicating whether the RPC call was successful.
        /// </summary>
        [DataMember(Order = 0)]
        public bool IsSuccess { get; init; }

        /// <summary>
        /// Error message if the RPC call was not successful.
        /// </summary>
        [DataMember(Order = 1)]
        public string? Error { get; init; }
    }
}