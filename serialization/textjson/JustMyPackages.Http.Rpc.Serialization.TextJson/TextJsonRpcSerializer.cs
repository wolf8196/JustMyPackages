using System;
using System.IO;
using System.Net.Mime;
using System.Text.Json;
using Microsoft.IO;

namespace JustMyPackages.Http.Rpc.Serialization.TextJson
{
    internal sealed class TextJsonRpcSerializer : IRpcSerializer
    {
        private readonly RecyclableMemoryStreamManager manager;
        private readonly JsonSerializerOptions options;

        public TextJsonRpcSerializer(JsonSerializerOptions options, RecyclableMemoryStreamManager manager)
        {
            this.manager = manager;
            this.options = options;
        }

        public string MediaType => MediaTypeNames.Application.Json;

        public Stream Serialize<T>(T obj)
        {
            var stream = manager.GetStream();
            JsonSerializer.Serialize(stream, obj, options);
            stream.Position = 0;
            return stream;
        }

        public T Deserialize<T>(Stream stream)
        {
            return JsonSerializer.Deserialize<T>(stream, options) ?? throw new ArgumentException("Invalid stream provided. Serialization resulted in null.");
        }
    }
}
