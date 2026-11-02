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

    public static class RpcResponseExtensions
    {
        /// <summary>
        /// Creates a successful RPC response.
        /// </summary>
        /// <returns>A successful <see cref="RpcResponse"/>.</returns>
        public static RpcResponse Success() => new RpcResponse { IsSuccess = true };

        /// <summary>
        /// Creates a failed RPC response with the specified error message.
        /// </summary>
        /// <param name="error">The error message.</param>
        /// <returns>A failed <see cref="RpcResponse"/>.</returns>
        public static RpcResponse Failure(string error) => new RpcResponse { IsSuccess = false, Error = error };

        /// <summary>
        /// Checks if the RPC response indicates a failure.
        /// </summary>
        /// <param name="response">The RPC response to check.</param>
        /// <returns>True if the response indicates a failure, otherwise false.</returns>
        public static bool IsFailure(this RpcResponse response) => !response.IsSuccess;
    }
}