using MessagePack;

namespace JustMyPackages.Http.Rpc
{
    /// <summary>
    /// Base RPC response.
    /// </summary>
    [MessagePackObject]
    public class RpcResponse
    {
        /// <summary>
        /// Value indicating whether the RPC call was successful.
        /// </summary>
        [Key(0)]
        public bool IsSuccess { get; init; }

        /// <summary>
        /// Error message if the RPC call was not successful.
        /// </summary>
        [Key(1)]
        public string? Error { get; init; }
    }
}