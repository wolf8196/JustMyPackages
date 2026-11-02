using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using JustMyPackages.Http.Rpc.Serialization;
using Microsoft.Extensions.Logging;

namespace JustMyPackages.Http.Rpc.Client
{
    public abstract class HttpRpcClient
    {
        private readonly HttpRpcClientParameters parameters;
        private readonly IRpcSerializer serializer;

        protected HttpRpcClient(HttpRpcClientParameters parameters)
        {
            this.parameters = parameters;
            serializer = parameters.Serilaizers.Single(x => x.MediaType == parameters.ClientSettings.MediaType);
        }

        protected string TargetName => parameters.TargetName;

        protected HttpClient HttpClient => parameters.HttpClient;

        protected ILogger Logger => parameters.Logger;

        protected async Task<TResponse> Send<TRequest, TResponse>(TRequest request, string url, CancellationToken cancellationToken)
            where TResponse : RpcResponse, new()
        {
            var requestUrl = Url.Combine(HttpClient.BaseAddress!.ToString(), url);
            try
            {
                using var httpRequest = CreateRequest(request, url);
                using var httpResponse = await HttpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseContentRead, cancellationToken);

                if (!httpResponse.IsSuccessStatusCode)
                {
                    Logger.LogWarning(
                        "Unsuccessful {StatusCode} status code returned from {Url} of {ServiceName} service. Reason: {Reason}",
                        httpResponse.StatusCode,
                        requestUrl,
                        TargetName,
                        httpResponse.ReasonPhrase);

                    return new TResponse
                    {
                        IsSuccess = false,
                        Error = $"Unsuccessful {httpResponse.StatusCode} status code returned from {requestUrl} of {TargetName} service. Reason: {httpResponse.ReasonPhrase}",
                    };
                }

                return await ParseResponse<TResponse>(httpResponse, cancellationToken);
            }
            catch (OperationCanceledException ex)
            {
                Logger.LogWarning(ex, "Request to {Url} of {ServiceName} service was cancelled.", requestUrl, TargetName);
                return new TResponse
                {
                    IsSuccess = false,
                    Error = $"Request to {requestUrl} of {TargetName} service was cancelled.",
                };
            }
            catch (HttpRequestException ex) when (ex.InnerException is SocketException)
            {
                Logger.LogWarning(ex, "Failed to connect to {Url} of {ServiceName} service.", requestUrl, TargetName);
                return new TResponse
                {
                    IsSuccess = false,
                    Error = $"Failed to connect to {requestUrl} of {TargetName} service.",
                };
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error occurred while requesting {Url} from {ServiceName} service.", requestUrl, TargetName);
                return new TResponse
                {
                    IsSuccess = false,
                    Error = $"Error occurred while requesting {requestUrl} from {TargetName} service. Error: {ex.Message}",
                };
            }
        }

        protected HttpRequestMessage CreateRequest<TRequest>(TRequest request, string uri)
        {
            var requestContent = serializer.Serialize(request);
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StreamContent(requestContent),
            };
            httpRequest.Content.Headers.ContentType = new MediaTypeHeaderValue(serializer.MediaType);
            httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(serializer.MediaType));
            return httpRequest;
        }

        protected async Task<TResponse> ParseResponse<TResponse>(HttpResponseMessage httpResponse, CancellationToken cancellationToken)
        {
            using var responseContent = await httpResponse.Content.ReadAsStreamAsync(cancellationToken);
            var response = serializer.Deserialize<TResponse>(responseContent);
            return response;
        }
    }
}