using System;
using System.Net.Mime;

namespace JustMyPackages.Http.Rpc.Client
{
    public class HttpRpcClientSettings
    {
        public static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(180);

        public HttpRpcClientSettings(string url, string mediaType = MediaTypeNames.Application.Json)
            : this(url, mediaType, DefaultTimeout)
        {
        }

        public HttpRpcClientSettings(string url, string mediaType, TimeSpan timeout)
        {
            Url = url;
            Timeout = timeout;
            MediaType = mediaType;
        }

        public string Url { get; }

        public string MediaType { get; }

        public TimeSpan Timeout { get; }
    }
}