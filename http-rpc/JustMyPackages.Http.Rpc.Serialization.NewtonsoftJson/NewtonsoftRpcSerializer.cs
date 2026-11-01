using System;
using System.IO;
using System.Net.Mime;
using Microsoft.IO;
using Newtonsoft.Json;

namespace JustMyPackages.Http.Rpc.Serialization.NewtonsoftJson
{
    internal class NewtonsoftRpcSerializer : IRpcSerializer
    {
        private readonly JsonSerializer jsonSerializer;
        private readonly RecyclableMemoryStreamManager manager;

        public NewtonsoftRpcSerializer(JsonSerializerSettings settings, RecyclableMemoryStreamManager manager)
        {
            jsonSerializer = JsonSerializer.Create(settings);
            this.manager = manager;
        }

        public string MediaType => MediaTypeNames.Application.Json;

        public Stream Serialize<T>(T obj)
        {
            var stream = manager.GetStream();
            using (var sw = new StreamWriter(stream, leaveOpen: true))
            using (var jtw = new JsonTextWriter(sw))
            {
                jsonSerializer.Serialize(jtw, obj);
            }

            stream.Position = 0;
            return stream;
        }

        public T Deserialize<T>(Stream stream)
        {
            using var sr = new StreamReader(stream);
            using var jtr = new JsonTextReader(sr);
            return jsonSerializer.Deserialize<T>(jtr) ?? throw new ArgumentException("Invalid stream provided. Serialization resulted in null.");
        }
    }
}
