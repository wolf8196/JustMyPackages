using System.IO;
using JustMyPackages.Serialization.MessagePack;
using MessagePack;
using Microsoft.IO;

namespace JustMyPackages.Http.Rpc.Serialization.MessagePack
{
    internal class MessagePackRpcSerializer : IRpcSerializer
    {
        private readonly RecyclableMemoryStreamManager manager;
        private readonly MessagePackSerializerOptions options;

        public MessagePackRpcSerializer(MessagePackSerializerOptions options, RecyclableMemoryStreamManager manager)
        {
            this.manager = manager;
            this.options = options;
        }

        public string MediaType => MediaTypes.MessagePack;

        public Stream Serialize<T>(T obj)
        {
            Stream stream = manager.GetStream();
            MessagePackSerializer.Serialize(stream, obj, options);
            stream.Position = 0;
            return stream;
        }

        public T Deserialize<T>(Stream stream)
        {
            return MessagePackSerializer.Deserialize<T>(stream, options);
        }
    }
}